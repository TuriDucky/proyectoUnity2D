using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed; //Velocidad en el eje Y. Es el impulso que recibe al pulsar el boton de salto
    public float xAcceleration; //Aceleracion en el eje X. El tiempo que cuesta alcanzar la velocidad maxima y detenerse.
    public float xSpeedCap; //La velocidad maxima que puede alcanzar el personaje mediante los botones de movimiento
    public float xSpeedCapAirborn; //La velocidad maxima que puede alcanzar el personaje mediante los botones de movimiento en el aire
    public bool facingRight; //true si el ultimo input fue el de ir a la derecha, false si lo mismo pero izquierda

    public float jumptime; // El tiempo en el que el jugador puede seguir apretando el boton de salto para saltar mas alto
    public float jumptimeCounter;
    public bool isJumping; //true si ha usado botones para saltar, false sino
    public bool canJump; //true si puede saltar, false sino



    public char lastKeyPressed; // Ultima tecla pulsada. Solo tiene en cuenta las teclas de direccion



    public bool isDashing; //Es true cuando se esta dahseando, false cuando no

    public bool dashStartedGround; //true si la accion de dash comienza en el suelo
    public float dashTime; //Tiempo que dura el dash
    float dashTimeCounter;

    public float dashCount; //Numero de dashes que puede hacer en el aire
    public float dashCountCounter;

    public float dashDelay; //Tiempo de espera entre dos dashes
    public float dashDelayCounter;



    public float slashDelay; //Tiempo de espera entre espadazos
    float slashDelayCounter;
    public bool isHit; //Cuando un ememigo lo golpea, sera true


    public float knockbackTime; //Tiempo de knockback
    float knockbackTimeCounter;
    
    public int comboRebote;

    public bool isRespawning;
    public float respawnTime;
    public float respawnTimeCounter;

    public bool animationJump;

    public Vector3 lastCheckpoint; //Las cordenadas del ultimo checkpoint visitado

    public GameObject SlashPrefab; //El ataque del jugador

    public Rigidbody2D rb2D;
    public Animator animator;
    SpriteRenderer sr;
    
    // Inicializa varios valores
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        facingRight = true;
        isHit = false;
        lastCheckpoint = transform.position;
    }

    void Update(){

        if (!GroundCheck.isGrounded && rb2D.velocity.y > 0){
            animator.SetBool("isAscending", true);
            animator.SetBool("isFalling", false);
            animationJump = true;
        }
        if (!GroundCheck.isGrounded && rb2D.velocity.y < 0){
            animator.SetBool("isAscending", false);
            animator.SetBool("isFalling", true);
            animationJump = true;
        }
        if (GroundCheck.isGrounded){
            animator.SetBool("isAscending", false);
            animator.SetBool("isFalling", false);
            animationJump = false;
        }


        if (respawnTimeCounter > 0){
            respawnTimeCounter -= Time.deltaTime;
            
            if (respawnTimeCounter <= 0){
                respawnTimeCounter = 0;
            }
        }

        //Temporizador para el tiempo entre ataques
        if(slashDelayCounter > 0){
            slashDelayCounter -= Time.deltaTime;
        }
        else{
            slashDelayCounter = 0;
        }

        //Temporizador para el delay entre dashes
        if (dashDelayCounter > 0){
            dashDelayCounter -= Time.deltaTime;
        }
        else{
            dashDelayCounter = 0;
        }

        //Detecta el imput del boton de aqtaque y llama al metodo de ataque.
        //Tambien inicia el temporizador de tiempo entre ataques
        if (Input.GetMouseButtonDown(0) && slashDelayCounter <= 0){
            slashDelayCounter = slashDelay;
            slash();
        }


        //Detecta si se ha dejado de pulsar el boton de salto para resetear el
        //temporizador de saltos y settear isJumping = false
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow)){
            jumptimeCounter = 0f;
            isJumping = false;
        }


        //Detecta e ultimo boton de direccion que el jugador haya pulsado (siempre que no este dasheando)
        if (!isDashing){
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
                lastKeyPressed = 'a';
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
                lastKeyPressed = 'd';
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
                lastKeyPressed = 'w';
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
                lastKeyPressed = 's';
            }
        }

        //coumprueba si el jugador esta tocando el suelo para resetear el valor
        //de dashes que el jugador puede hacer en el aire.
        if (GroundCheck.isGrounded){
            dashCountCounter = dashCount;
        }


        //Detecta si el usuario ha presionado el boton de dash, y comprueba si
        //hay disponibles dahses en el aire y que haya pasado el delay entre
        //dashes. Si todo devuelve true, llama al metodo slash(), y activa el
        //temporizador entre dashes y el temporizador de duracion del dash.
        if (Input.GetMouseButtonDown(1) && dashCountCounter > 0 && dashDelayCounter <= 0){
            dashDelayCounter = dashDelay;
            slashDelayCounter = slashDelay / 2;
            slash();
            isDashing = true;
            animator.SetBool("isDashing", true);
            if (GroundCheck.isGrounded){
                dashStartedGround = true;
            }
            else{
                dashStartedGround = false;
            }
            dashTimeCounter = dashTime;
            dashCountCounter --;
        }


        if (GroundCheck.touchingSemisolid && Input.GetKey(KeyCode.S)){
            canJump = false;
        }
        else{
            canJump = true;
        }
    }

    void FixedUpdate()
    {

        //En caso de que la velocidad del rigidbody es muy cercana al zero, setea
        //tanto la velocidad tanto SpeedX a zero
        if (rb2D.velocity.x > -0.5 && rb2D.velocity.x < 0.5){
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            xSpeed = 0;
        } 

        //Si el personaje esta dasheando, dependiendo de cual fue el ultimo input
        //direccional antes del dash, aplica velocidad hacia esa direccion. En caso
        //de que el dash sea hacia arriba, aplica mas fuerza para que se eleve mas.

        //Aqui hayq ue solucionar el error de los dash jumps
        if (isDashing){
            
            xSpeed = 17;
            
            if (lastKeyPressed == 'a'){
                xSpeed = -xSpeed;
                if(dashStartedGround){
                    rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
                }
                else{
                    rb2D.velocity = new Vector2(xSpeed, 0);
                }
                
            }
            if(lastKeyPressed == 'd'){
                if(dashStartedGround){
                    rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
                }
                else{
                    rb2D.velocity = new Vector2(xSpeed, 0);
                }
            }
            if (lastKeyPressed == 'w'){
                float dashup = (xSpeed / 4) * 3;
                xSpeed = 0;
                rb2D.velocity = new Vector2(0, dashup);
            }
            if (lastKeyPressed == 's'){
                float dashdown = -xSpeed;
                xSpeed = 0;
                rb2D.velocity = new Vector2(0, dashdown);
            }
            
            dashTimeCounter -= Time.deltaTime; //Temporizador del tiempo en estado de dash

            //Cuando termine el temporizador, termina el estado de dash y reinicia el temporizador
            if (dashTimeCounter <= 0){
                isDashing = false;
                animator.SetBool("isDashing", false);
                dashTimeCounter = 0;
            }
        }
        
        //Solo se puede hacer mover el personaje hacia los lados si no esta dasheando o no esta ene stado de hit
        if (!isDashing && !isHit){
            //Izquierda
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
                
                facingRight = false;
                sr.flipX = true;

                //Cuando este en el suelo aplic
                if (GroundCheck.isGrounded){
                
                    animator.SetBool("isRunning", false);

                    if (!animationJump){
                        animator.SetBool("isRunning", true);
                    }
                    
                    
                    xSpeed = xSpeed - xAcceleration * 3;
                    if (xSpeed < -xSpeedCap){
                        xSpeed = -xSpeedCap;
                    }
                }
                else{
                    if (xSpeed >= -xSpeedCapAirborn){
                        xSpeed = xSpeed - xAcceleration;

                        if (xSpeed < -xSpeedCapAirborn){
                            xSpeed = -xSpeedCapAirborn;
                        }
                    }
                }
                rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
            }

            //Derecha
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
                
                facingRight = true;
                sr.flipX = false;
                
                if (GroundCheck.isGrounded){
                    
                    animator.SetBool("isRunning", false);
                    
                    if (!animationJump){
                        animator.SetBool("isRunning", true);
                    }


                    xSpeed = xSpeed + xAcceleration * 3;
                    if (xSpeed > xSpeedCap){
                        xSpeed = xSpeedCap;
                    }
                }
                else{
                    if (xSpeed <= xSpeedCapAirborn){
                        xSpeed = xSpeed + xAcceleration;

                        if (xSpeed > xSpeedCapAirborn){
                            xSpeed = xSpeedCapAirborn;
                        }
                    }
                }
                rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
            }
            else{
                animator.SetBool("isRunning", false);
                if (GroundCheck.isGrounded){
                    if (xSpeed != 0){
                        if (xSpeed > 0){
                            xSpeed = xSpeed - xAcceleration * 5;
                            if (xSpeed < 0){
                                rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                            }
                            else{
                                rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
                            }
                            
                        }
                        else{
                            xSpeed = xSpeed + xAcceleration * 5;
                            if (xSpeed > 0){
                                rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                            }
                            else{
                                rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
                            }
                        }
                    }
                }  
            
    }
            
        }
        
        if (canJump){
            if (GroundCheck.isGrounded || (isDashing && dashStartedGround)){
                if(Input.GetKey(KeyCode.Space)){
                    jumptimeCounter = jumptime;
                    isJumping = true;
                    rb2D.velocity = new Vector2(rb2D.velocity.x, ySpeed);
                    
                }
            }
        }

        if(Input.GetKey(KeyCode.Space)){
            if (jumptimeCounter > 0){
                rb2D.velocity = new Vector2(rb2D.velocity.x, ySpeed);
                jumptimeCounter -= Time.deltaTime;
            }
            else{
                jumptimeCounter = 0;
            }
        }
    }

    /*
        A partir de aqui hay metodos dedicados del personaje.
    */


    void slash(){
        GameObject slash = Instantiate(SlashPrefab, transform.position, Quaternion.identity);
        slash.transform.parent = gameObject.transform;
            if (lastKeyPressed == 'a'){
                slash.transform.Rotate(0,0,180);
                slash.transform.GetComponent<Slash>().playerDirection = 3;
            }
            
            if (lastKeyPressed == 'w'){
                slash.transform.Rotate(0,0,90);
                slash.transform.GetComponent<Slash>().playerDirection = 2;
            }
            if (lastKeyPressed == 's'){
                slash.transform.Rotate(0,0,-90);
                slash.transform.GetComponent<Slash>().playerDirection = 1;
            }
            
    }

    public void hitEnemy(bool enemyIsDying){
        if (lastKeyPressed == 's' && !enemyIsDying && !GroundCheck.isGrounded){
            rb2D.velocity = new Vector2(rb2D.velocity.x, 20);
        }
        if (enemyIsDying){
            comboRebote ++;
        }
    }

    public void setCheckpoint(Vector3 newCheckpoint){
        lastCheckpoint = newCheckpoint;
    }

    public void Respawn(){
        rb2D.velocity = new Vector2(0,0);
        xSpeed = 0;
        respawnTimeCounter = respawnTime;
        transform.position = lastCheckpoint;
    }


    void OnCollisionEnter2D(Collision2D coll){
        if (coll.collider.tag == "Enemy"){
            isHit = true;
            knockbackTimeCounter = knockbackTime;
            var direction = transform.InverseTransformPoint (coll.transform.position); 
            
            if (direction.x > 0){
                rb2D.velocity = new Vector2 (-3 , rb2D.velocity.y);
            }
            if (direction.x < 0){
                rb2D.velocity = new Vector2 (3 , rb2D.velocity.y);
            }
        }
    }
}
