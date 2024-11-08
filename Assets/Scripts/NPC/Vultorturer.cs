using UnityEngine;

public class Vultorturer : MonoBehaviour
{


    public float xSpeed;
    public float xAccel;
    public float maxXSpeed;

    public float ySpeed;
    public float yAccel;
    public float maxYSpeed;


    public bool isAttacking;
    public float attackTimeCounter;
    public float attackTimeValue;

    public int lives;

    public bool hasDetectedPlayer;
    public float deathCounter;
    public bool isDying;
    private float deathDirection;
    public bool isVisible;
    public int pointValue;
    public bool isStatic;


    public AudioSource attackSFX;
    public AudioSource hitSFX;
    public AudioSource deathSFX;
    Vector3 player;
    Rigidbody2D rb2D;
    SpriteRenderer sr;
    BoxCollider2D bc2D;
    Animator animator;
    EnemyPatroll patroll;
    Enemy generic;
    void Start()
    {
        isDying = false;
        rb2D = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        patroll = transform.GetChild(0).GetComponent<EnemyPatroll>();
        deathCounter = 2;
        generic = GetComponent<Enemy>();
        generic.setPoints(pointValue);
        generic.setLives(lives);

        if (isStatic){
            
            rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
            animator.SetBool("Fly", true);
        }
    }

    // Update is called once per frame
    void Update()
    {

        player = GameObject.Find("Player").transform.position;

        if (!isStatic){
            if (hasDetectedPlayer)
            {
                if (attackTimeCounter > 0)
                {
                    attackTimeCounter -= Time.deltaTime;
                }
                else
                {

                    if (isAttacking)
                    {
                        isAttacking = false;
                        attackTimeCounter = attackTimeValue;
                    }
                    else
                    {
                        isAttacking = true;
                        attackSFX.Play();
                        attackTimeCounter = attackTimeValue / 3;
                    }
                }
            }
        }
        
        
        if (isDying)
        {
            if (!isVisible)
            {
                deathCounter -= Time.deltaTime;
            }
            if (deathCounter <= 0)
            {
                Destroy(gameObject);
            }
        }

        if (!isStatic){
            if (patroll.getDetected())
            {
                hasDetectedPlayer = true;
                animator.SetBool("Fly", true);
                attackSFX.Play();
            }
        }
    }

    void FixedUpdate()
    {
        if (!isDying)
        {
            if (!isStatic){
                if (hasDetectedPlayer)
                {

                    float above;

                    if (isAttacking)
                    {
                        above = 0;
                    }
                    else
                    {
                        above = 5;
                    }

                    if (transform.position.y - above < player.y)
                    {
                        ySpeed += yAccel;
                        if (ySpeed > maxYSpeed)
                        {
                            ySpeed = maxYSpeed;
                        }
                    }
                    else
                    {
                        ySpeed -= yAccel;
                        if (ySpeed < -maxYSpeed)
                        {
                            ySpeed = -maxYSpeed;
                        }
                    }

                    if (transform.position.x < player.x)
                    {
                        sr.flipX = false;
                        xSpeed += xAccel;
                        if (xSpeed > maxXSpeed)
                        {
                            xSpeed = maxXSpeed;
                        }
                    }
                    else
                    {
                        sr.flipX = true;
                        xSpeed -= xAccel;
                        if (xSpeed < -maxXSpeed)
                        {
                            xSpeed = -maxXSpeed;
                        }
                    }

                    rb2D.velocity = new Vector2(xSpeed, ySpeed);
                }
            }
            
            
        }
        else
        {
            rb2D.velocity = new Vector2(deathDirection, rb2D.velocity.y);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {

    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Attack"))
        {
            hit();
            if (!isStatic){
                hitSFX.Play();
            }
        }

        if (collider2D.gameObject.CompareTag("Big Attack"))
        {
            hit();
            if (!isStatic){
                hitSFX.Play();
            }
        }
    }

    public void hit()
    {
        lives--;
        if (lives <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        rb2D.constraints = RigidbodyConstraints2D.None;
        if(!isDying){
            animator.SetBool("Fly", false);
            GameObject.Find("Level").GetComponent<Level>().addScore(pointValue);
        }
        rb2D.constraints = RigidbodyConstraints2D.None;
        deathDirection = Random.Range(-4, 4);
        rb2D.velocity = new Vector2(deathDirection, 15);
        rb2D.angularVelocity = Random.Range(-360, 360);
        bc2D.isTrigger = true;
        isDying = true;
        deathSFX.Play();
    }

    void OnBecameVisible()
    {
        isVisible = true;
    }
    void OnBecameInvisible()
    {
        isVisible = false;
    }
}
