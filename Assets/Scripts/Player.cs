using UnityEngine;

public class Player : MonoBehaviour
{
    public float xSpeed; //La velocidad del personaje. Es una VARIABLE, la velocidad maxima del personaje es xSpeedCap
    public float ySpeed; //Velocidad en el eje Y. Es el impulso que recibe al pulsar el boton de salto.
    public float xAcceleration; //Aceleracion en el eje X. El tiempo que cuesta alcanzar la velocidad maxima y detenerse.
    public float xSpeedCap; //La velocidad maxima que puede alcanzar el personaje mediante los botones de movimiento
    public float xSpeedCapAirborn; //La velocidad maxima que puede alcanzar el personaje mediante los botones de movimiento en el aire

    public char lastKeyPressed; // Ultima tecla pulsada. Solo tiene en cuenta las teclas de direccion
    public int comboRebote;
    public int lives;

    public bool facingRight; //true si el ultimo input fue el de ir a la derecha, false si lo mismo pero izquierda
    public bool isJumping; //true si ha usado botones para saltar, false sino
    public bool canJump; //true si puede saltar, false sino
    public bool isDashing; //Es true cuando se esta dahseando, false cuando no
    public bool dashStartedGround; //true si la accion de dash comienza en el suelo
    public bool isHit; //Cuando un ememigo lo golpea, sera true
    public bool downSlam;
    public bool animationJump;
    public bool playerHasControl;


    public float jumpTimeValue; // El tiempo en el que el jugador puede seguir apretando el boton de salto para saltar mas alto (Valor por defecto: 0.25)
    public float jumpTimeCounter;

    public float dashTimeValue; //Tiempo que dura el dash (Valor por defecto: 0.25)
    public float dashTimeCounter;

    public float dashCountValue; //Numero de dashes que puede hacer en el aire (Valor por defecto: 1)
    public float dashCountCounter;

    public float dashDelayValue; //Tiempo de espera entre dos dashes (Valor por defecto: 0.5)
    public float dashDelayCounter;

    public float slashDelayValue; //Tiempo de espera entre ataques (Valor por defecto: 0.4)
    public float slashDelayCounter;

    public float knockbackTimeValue; //Tiempo de knockback (Valor por defecto: 0.5)
    public float knockbackTimeCounter;

    public float slamStartTimeValue;
    public float slamStartTimeCounter;

    public float iFramesValue;
    public float iFramesCounter;

    public AudioSource jumpSFX;
    public AudioSource fireSFX;
    public AudioSource landSFX;
    public AudioSource stompSFX;
    public AudioSource dashSFX;
    public AudioSource hurtSFX;



    public Vector3 lastCheckpoint; //Las cordenadas del ultimo checkpoint visitado

    public GameObject SlashPrefab; //El ataque del jugador
    public GameObject StompPrefab;
    public GameObject LandPrefab;

    public Rigidbody2D rb2D;
    public Animator animator;
    SpriteRenderer sr;

    // Inicializa varios valores
    void Start()
    {
        playerHasControl = true;
        rb2D = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        facingRight = true;
        lastCheckpoint = transform.position;
    }

    void Update()
    {
        if (lives <= 0)
        {
            death();
        }

        updateTimers();



        //Detecta e ultimo boton de direccion que el jugador haya pulsado (siempre que no este dasheando)
        if (!isDashing && playerHasControl)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                lastKeyPressed = 'a';
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                lastKeyPressed = 'd';
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                lastKeyPressed = 'w';
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                lastKeyPressed = 's';
            }
        }

        // Actualiza la animacion del personaje de cuando esta en el aire
        if (!GroundCheck.isGrounded && rb2D.velocity.y > 0 && iFramesCounter <= 0)
        {
            animator.SetBool("isAscending", true);
            animator.SetBool("isFalling", false);
            animationJump = true;
        }
        if (!GroundCheck.isGrounded && rb2D.velocity.y < 0 && iFramesCounter <= 0)
        {
            animator.SetBool("isAscending", false);
            animator.SetBool("isFalling", true);
            animationJump = true;
        }
        if (GroundCheck.isGrounded && iFramesCounter <= 0)
        {
            animator.SetBool("isAscending", false);
            animator.SetBool("isFalling", false);
            animationJump = false;
        }

        //Detecta el imput del boton de aqtaque y llama al metodo de ataque.
        //Tambien inicia el temporizador de tiempo entre ataques
        if (Input.GetMouseButtonDown(0) && slashDelayCounter <= 0 && playerHasControl)
        {
            slashDelayCounter = knockbackTimeValue;
            slash();
        }


        //Detecta si se ha dejado de pulsar el boton de salto para resetear el
        //temporizador de saltos y settear isJumping = false
        if (playerHasControl)
        {
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow))
            {
                jumpTimeCounter = 0f;
                isJumping = false;
            }
        }



        //Detecta si el usuario ha presionado el boton de dash, y comprueba si
        //hay disponibles dahses en el aire y que haya pasado el delay entre
        //dashes. Si todo devuelve true, llama al metodo slash(), y activa el
        //temporizador entre dashes y el temporizador de duracion del dash.
        // En caso de que el jugador este presionando abajo, realizara el slam
        if (Input.GetMouseButtonDown(1) && dashCountCounter > 0 && dashDelayCounter <= 0 && playerHasControl)
        {
            dashDelayCounter = dashDelayValue;
            slashDelayCounter = knockbackTimeValue / 2;
            if (lastKeyPressed != 's')
            {
                dashSFX.Play();
                isDashing = true;
                animator.SetBool("isDashing", true);
                if (GroundCheck.isGrounded)
                {
                    dashStartedGround = true;
                }
                else
                {
                    dashStartedGround = false;
                }
                dashTimeCounter = dashTimeValue;
                dashCountCounter--;
            }
            else
            {
                if (!GroundCheck.isGrounded)
                {
                    slamStartTimeCounter = slamStartTimeValue;
                    downSlam = true;
                    stomp();
                }
            }
        }

        // Si el jugador esta encima de una plataforma semisolida, mientras este presionando
        // abajo no podra saltar, para que pueda bajar si presiona el boton de salto
        if (GroundCheck.touchingSemisolid && Input.GetKey(KeyCode.S) && playerHasControl)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }


    }

    void FixedUpdate()
    {

        //En caso de que la velocidad del rigidbody es muy cercana al zero, setea
        //tanto la velocidad tanto SpeedX a zero. Principalemente para evitar errores
        if (rb2D.velocity.x > -0.5 && rb2D.velocity.x < 0.5)
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            xSpeed = 0;
        }

        //La logica del slam. Si aun esta en el delay, ascendera un poquito, y cuando
        //acabe, bajara muy rapido. Cuando detecte suelo, creara el ataque de aterrizar
        if (downSlam)
        {
            if (slamStartTimeCounter > 0)
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x / 2, 7);
            }
            else
            {
                rb2D.velocity = new Vector2(0, -40);
                if (GroundCheck.isGrounded)
                {
                    downSlam = false;
                    land();
                }
            }

        }

        //Si el personaje esta dasheando, dependiendo de cual fue el ultimo input
        //direccional antes del dash, aplica velocidad hacia esa direccion. En caso
        //de que el dash sea hacia arriba, aplica mas fuerza para que se eleve mas.

        //Aqui hayq ue solucionar el error de los dash jumps
        // Actualizacion sobre la linea de encima: Me ha gustado el error por lo que ahora es una mecanica
        if (isDashing)
        {
            xSpeed = 17;

            if (lastKeyPressed == 'a')
            {
                xSpeed = -xSpeed;
                if (dashStartedGround)
                {
                    rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
                }
                else
                {
                    rb2D.velocity = new Vector2(xSpeed, 0);
                }

            }
            if (lastKeyPressed == 'd')
            {
                if (dashStartedGround)
                {
                    rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
                }
                else
                {
                    rb2D.velocity = new Vector2(xSpeed, 0);
                }
            }
            if (lastKeyPressed == 'w')
            {
                float dashup = (xSpeed / 4) * 3;
                xSpeed = 0;
                rb2D.velocity = new Vector2(0, dashup);
            }


            dashTimeCounter -= Time.deltaTime; //Temporizador del tiempo en estado de dash

            //Cuando termine el temporizador, termina el estado de dash y reinicia el temporizador
            if (dashTimeCounter <= 0)
            {
                isDashing = false;
                animator.SetBool("isDashing", false);
                dashTimeCounter = 0;
            }
        }

        // La logica del movimiento horizontal del peronaje.

        //Solo se puede hacer mover el personaje hacia los lados si no esta dasheando o no esta ene stado de hit




        if (!isDashing && !isHit && !downSlam)
        {

            //Izquierda
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                if (playerHasControl)
                {
                    facingRight = false;
                    sr.flipX = true;

                    //Cuando este en el suelo aplic
                    if (GroundCheck.isGrounded)
                    {

                        animator.SetBool("isRunning", false);

                        if (!animationJump)
                        {
                            animator.SetBool("isRunning", true);
                        }


                        xSpeed = xSpeed - xAcceleration * 3;
                        if (xSpeed < -xSpeedCap)
                        {
                            xSpeed = -xSpeedCap;
                        }
                    }
                    else
                    {
                        if (xSpeed >= -xSpeedCapAirborn)
                        {
                            xSpeed = xSpeed - xAcceleration;

                            if (xSpeed < -xSpeedCapAirborn)
                            {
                                xSpeed = -xSpeedCapAirborn;
                            }
                        }
                    }
                    rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
                }
            }

            //Derecha
            else

                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                if (playerHasControl)
                {
                    facingRight = true;
                    sr.flipX = false;

                    if (GroundCheck.isGrounded)
                    {

                        animator.SetBool("isRunning", false);

                        if (!animationJump)
                        {
                            animator.SetBool("isRunning", true);
                        }


                        xSpeed = xSpeed + xAcceleration * 3;
                        if (xSpeed > xSpeedCap)
                        {
                            xSpeed = xSpeedCap;
                        }
                    }
                    else
                    {
                        if (xSpeed <= xSpeedCapAirborn)
                        {
                            xSpeed = xSpeed + xAcceleration;

                            if (xSpeed > xSpeedCapAirborn)
                            {
                                xSpeed = xSpeedCapAirborn;
                            }
                        }
                    }
                    rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
                }
            }

            // Movimiento horizontal del personaje en caso de no recivir imputs
            else
            {
                animator.SetBool("isRunning", false);
                if (GroundCheck.isGrounded)
                {
                    if (xSpeed != 0)
                    {
                        if (xSpeed > 0)
                        {
                            xSpeed = xSpeed - xAcceleration * 5;
                            if (xSpeed < 0)
                            {
                                rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                            }
                            else
                            {
                                rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
                            }

                        }
                        else
                        {
                            xSpeed = xSpeed + xAcceleration * 5;
                            if (xSpeed > 0)
                            {
                                rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                            }
                            else
                            {
                                rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
                            }
                        }
                    }
                }
            }
        }


        // Logica del salto de personaje.
        if (playerHasControl)
        {
            if (canJump)
            {
                if (GroundCheck.isGrounded || (isDashing && dashStartedGround))
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        if (!isJumping)
                        {
                            jumpSFX.Play();
                        }
                        jumpTimeCounter = jumpTimeValue;
                        isJumping = true;
                        rb2D.velocity = new Vector2(rb2D.velocity.x, ySpeed);
                    }
                }
            }

            if (Input.GetKey(KeyCode.Space))
            {
                if (jumpTimeCounter > 0)
                {
                    rb2D.velocity = new Vector2(rb2D.velocity.x, ySpeed);
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    jumpTimeCounter = 0;
                }
            }
        }

    }

    /*
        A partir de aqui hay metodos dedicados del personaje.
    */

    // El metodo se llama al inicio del Update, y actualiza diferentes temporizadores y contadores de estados del jugador
    void updateTimers()
    {

        //coumprueba si el jugador esta tocando el suelo para resetear el valor
        //de dashes que el jugador puede hacer en el aire.
        if (GroundCheck.isGrounded)
        {
            dashCountCounter = dashCountValue;
        }

        // Tiempo en el que el jugador tarda en empezar el slam
        if (slamStartTimeCounter > 0)
        {
            slamStartTimeCounter -= Time.deltaTime;
        }
        else
        {
            slamStartTimeCounter = 0;
        }

        //Temporizador para el tiempo entre ataques
        if (slashDelayCounter > 0)
        {
            slashDelayCounter -= Time.deltaTime;
        }
        else
        {
            slashDelayCounter = 0;
        }

        //Temporizador para el delay entre dashes
        if (dashDelayCounter > 0)
        {
            dashDelayCounter -= Time.deltaTime;
        }
        else
        {
            dashDelayCounter = 0;
        }

        // Tiempo de inmunidad;
        if (iFramesCounter > 0)
        {
            if (iFramesCounter < 0.8)
            {
                animator.SetBool("isHit", false);
            }
            iFramesCounter -= Time.deltaTime;
        }
        else
        {
            animator.SetBool("isHit", false);
            iFramesCounter = 0;
            if (lives <= 0)
            {
                GameObject.Find("transition").GetComponent<Transition>().close(false);
            }
        }
    }

    // Metodo del ataque del jugador. Inicializa el prefab del ataque, y lo
    //posiciona adecuadamente depende de la ultima tecla de direccion presionada
    void slash()
    {
        fireSFX.Play();
        GameObject slash = Instantiate(SlashPrefab, transform.position, Quaternion.identity);
        slash.transform.parent = gameObject.transform;

        if (lastKeyPressed == 'a')
        {
            slash.transform.Rotate(0, 0, 180);
            slash.transform.GetComponent<Slash>().playerDirection = 3;
        }
        if (lastKeyPressed == 'w')
        {
            slash.transform.Rotate(0, 0, 90);
            slash.transform.GetComponent<Slash>().playerDirection = 2;
        }
        if (lastKeyPressed == 's')
        {
            slash.transform.Rotate(0, 0, -90);
            slash.transform.GetComponent<Slash>().playerDirection = 1;
        }
    }

    // Inicializa el ataque hacia abajo del personaje. Crea el objeto de ataque, y mueve al
    // jugador a una capa en la que los enemigos no le afectan, para evitar problemas.
    void stomp()
    {
        stompSFX.Play();
        GameObject stomp = Instantiate(StompPrefab, transform.position, Quaternion.identity);
        stomp.transform.parent = gameObject.transform;
        gameObject.layer = LayerMask.NameToLayer("Enemy Inmune");
    }

    //Cuando aterriza el personaje de un ataque hacia abajo, se crea el trigger de ataque de
    //aterrizaje y devuelve al jugador a un estado normal.
    void land()
    {
        stompSFX.Stop();
        landSFX.Play();
        Instantiate(LandPrefab, transform.position, Quaternion.identity);
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    // El metodo se llama cuando un enemigo es golpeado por un ataque del jugador. En caso de
    // golpearlo con un ataque hacia abajo, dara un pequeño inpulso hacia arriba al jugador
    public void hitEnemy()
    {
        comboRebote++;
        if (lastKeyPressed == 's' && !GroundCheck.isGrounded && !downSlam)
        {

            rb2D.velocity = new Vector2(rb2D.velocity.x, 20);
            if (dashCountCounter < 1)
            {
                dashCountCounter++;
            }
        }

    }

    // El metodo es llamado cuando el jugador entra en un checkpoint y guarda las cordenadas de este
    public void setCheckpoint(Vector3 newCheckpoint)
    {
        lastCheckpoint = newCheckpoint;
    }

    // El metodo es llamado cuando el jugador muere o cae en un abismo. Si habia muerto, le restablezce
    // los puntosd e vida, y teletransporta el jugador al ultimo checkpoint que ha tocado. Tambien realiza
    // la transicion de nivel de abrirse
    public void Respawn()
    {
        if (lives <= 0)
        {
            lives = 3;
        }
        transform.position = lastCheckpoint;

        animator.SetBool("death", false);
        playerHasControl = true;
        rb2D.velocity = new Vector2(0, 0);
        xSpeed = 0;

        GameObject.Find("transition").GetComponent<Transition>().open();
        
    }

    public void setStomp(bool stomp)
    {
        downSlam = stomp;
    }

    // El metodo es llamado cuando el jugador sufre daño. Le reduce un punto de vida, y restablece
    // varios estados del jugador. tambin le da un pequeño inpulso hacia atras, y unos frames de invencibilidad.
    public void beenHit(Collider2D coll)
    {
        hurtSFX.Play();
        lives--;
        animator.SetBool("isDashing", false);
        isDashing = false;
        dashCountCounter = 0;
        animator.SetBool("isHit", true);
        if (coll.transform.position.x > transform.position.x)
        {
            xSpeed = -15;
        }
        else
        {
            xSpeed = 15;
        }

        rb2D.velocity = new Vector2(xSpeed, 10);
        iFramesCounter = iFramesValue;
    }

    // Logica de cuando el jugador muere. Este pierde el control y la animacion de muerte
    // empieza. Tras un rato, la transicion de cierre empieza y el jugador respawnea (logica en
    // el metodo de actualizar temporizadores)
    public void death()
    {
        animator.SetBool("death", true);
        playerHasControl = false;
        Minotaur.idle = true;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.tag == "Enemy") //Detectar si colisiona con un enemigo
        {
            if (iFramesCounter <= 0) //Comprobar si el jugador es inmune
            {
                beenHit(coll.collider);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Minotaur Attack") //Misma logica que arriba
        {
            if (iFramesCounter <= 0)
            {
                beenHit(coll);
            }
        }
    }
}
