using UnityEngine;

public class Slimer : MonoBehaviour
{
    public int direction;

    public float xSpeed;
    public float xRunSpeed;
    public float roamCounter;
    public float roamValue;
    public bool isMoving;
    public float deathCounter;
    public bool isDying;
    private float deathDirection;
    public bool isVisible;


    Rigidbody2D rb2D;
    SpriteRenderer sr;
    BoxCollider2D bc2D;
    Animator animator;
    void Start()
    {   
        isDying = false;
        roamCounter = 0;
        rb2D = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        deathCounter = 2;
    }

    // Update is called once per frame
    void Update()
    {

        

        if (roamCounter <= 0){
            roamCounter = roamValue;
            if (isMoving){
                isMoving = false;
            }
            else{
                isMoving = true;
                if (direction == 1){
                    direction = 0;
                }
                else{
                    direction = 1;
                }
            }
        }
        roamCounter -= Time.deltaTime;
        
        if (isDying){
            if (!isVisible){
                deathCounter -= Time.deltaTime;
            }
            if (deathCounter <= 0){
                Destroy(gameObject);
            }
        }
        else{
            if (transform.GetChild (1).GetComponent<SlimerAttack>().getPlayer()){
                animator.SetBool("isAttacking", true);
            }
            else{
                animator.SetBool("isAttacking", false);
            }
        }

        if (rb2D.velocity.x > 0){
            sr.flipX = true;
        }
        else{
            sr.flipX = false;
        }
    }

    void FixedUpdate(){
        if(!isDying){
            if (!EnemyPatroll.hasDetectedPlayer){
                if (isMoving){
                    if (direction == 1){
                        rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
                    }
                    if (direction == 0){
                        rb2D.velocity = new Vector2(-xSpeed, rb2D.velocity.y);
                    }
                }
            }
            else{
                roamCounter = 1;
                isMoving = true;
                var direction = transform.InverseTransformPoint (EnemyPatroll.coll.transform.position); 
            
                if (direction.x > 0){
                    rb2D.velocity = new Vector2 (xRunSpeed , rb2D.velocity.y);
                }
                if (direction.x < 0){
                    rb2D.velocity = new Vector2 (-xRunSpeed , rb2D.velocity.y);
                }
            }
        }
        else{
            rb2D.velocity = new Vector2(deathDirection, rb2D.velocity.y);
        }
            

        
    }

    private void OnCollisionEnter2D(Collision2D collision2D){
        
    }

    private void OnTriggerEnter2D(Collider2D collider2D){
        if (collider2D.gameObject.CompareTag("Attack")){
            Debug.Log("ataque");
            Death();
        }
    }


    public void Death(){
        rb2D.constraints = RigidbodyConstraints2D.None;
        deathDirection = Random.Range(-4, 4);
        rb2D.velocity = new Vector2(deathDirection, 15);
        rb2D.angularVelocity = Random.Range(-360, 360);
        bc2D.isTrigger = true;
        isDying = true;
    }

    void OnBecameVisible(){
        isVisible = true;
    }
    void OnBecameInvisible(){
        isVisible = false;
    }
}
