using UnityEngine;

public class NPC_Friend : MonoBehaviour
{
    public float xSpeed;
    private int direction;
    private float counter;
    public float maxWalkTime;
    public float minWalkTime;
    private bool isMoving;
    private Animator andar;

    Rigidbody2D rb2D;
    SpriteRenderer sr;

    void Start()
    {   
        counter = 0;
        rb2D = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        andar = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (counter <= 0){
            counter = Random.Range(minWalkTime, maxWalkTime);
            if (isMoving){
                isMoving = false;
            }
            else{
                isMoving = true;
                direction = Random.Range(0,2);
            }
        }
        counter -= Time.deltaTime;
        
        

        if (transform.GetChild (0).GetComponent<edgeDetection>().getEdge() || transform.GetChild (1).GetComponent<edgeDetection>().getEdge()){
            transform.GetChild (0).GetComponent<edgeDetection>().setEdge(false);
            transform.GetChild (1).GetComponent<edgeDetection>().setEdge(false);
            if (direction == 1){
                direction = 0;
            }
            else{
                direction = 1;
            }
        }
        
    }

    void FixedUpdate(){
         if (isMoving){
            if (direction == 1){
                sr.flipX = false;
                rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
                andar.Play("Run");
            }
            if (direction == 0){
                sr.flipX = true;
                rb2D.velocity = new Vector2(-xSpeed, rb2D.velocity.y);
                andar.Play("Run");
            }
        }

        if (!isMoving){
            rb2D.velocity = new Vector2 (0, rb2D.velocity.y);
            andar.Play("Idle");
        }
    }
}
