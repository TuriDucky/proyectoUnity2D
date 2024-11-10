using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Minotaur : MonoBehaviour
{
    public int xSpeed;

    public bool isMoving;

    public static bool idle;
    public bool isPlayerRight;
    public bool bigAttack;
    public bool spinAttack;
    public bool spinDash;
    public bool isStunned;
    public bool isVulnerable;
    public bool attackStarted;
    public int wallDirection;
    public int lives;

    public float bigAttackTime;
    public float bigAttackTimeValue;

    public float spinAttackTime;
    public float spinAttackTimeValue;

    public float dashStartDelay;
    public float dashStartDelayValue;

    public float stunTime;
    public float stunTimeValue;

    public float levelEndTime;
    public float levelEndTimeValue;

    public float redTime;
    public float redTimeValue;

    Color redColor;
    Color defaultColor;

    public AudioSource attackSFX;
    public AudioSource impactSFX;
    public AudioSource hitSFX;
    public AudioSource deathSFX;
    public AudioSource angrySFX;
    public AudioSource inmuneSFX;
    public AudioSource spinSFX;

    Rigidbody2D rb2D;
    SpriteRenderer sr;
    CapsuleCollider2D cc2D;
    Animator animator;

    void Start()
    {
        idle = true;
        spinAttackTime = spinAttackTimeValue;
        wallDirection = Random.Range(0, 2);

        rb2D = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cc2D = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();

        defaultColor = sr.color;

        redColor = new Color();
        redColor.r = 255;
        redColor.g = 0;
        redColor.b = 0;
        redColor.a = 255;
    }

    // Update is called once per frame
    void Update()
    {
        if (!idle)
        {
            checkPlayerPos();
            updateTimers();
        }
        else
        {
            resetMinotaur();
            
            if (levelEndTime > 0)
            {
                levelEndTime -= Time.deltaTime;
            }
            if (levelEndTime < 0)
            {
                GameObject.Find("transition").GetComponent<Transition>().close(true);
            }

        }


        if (!isStunned && !idle)
        {
            if (MinotaurPlayerDetection.hasDetecedPlayer && !spinAttack && !spinDash)
            {
                isMoving = false;
                bigAttack = true;
            }

            if (bigAttack && !spinAttack)
            {
                if (bigAttackTime <= 0)
                {
                    bigAttackTime = bigAttackTimeValue;
                }
                isMoving = false;
                BigAttack();
            }


            if (isMoving && !spinDash)
            {
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }
        }



    }

    void FixedUpdate()
    {
        if (!isStunned && !idle)
        {
            if (isMoving && !spinAttack && !spinDash)
            {
                if (isPlayerRight)
                {
                    rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
                }
                else
                {
                    rb2D.velocity = new Vector2(-xSpeed, rb2D.velocity.y);
                }
            }
            else
            {
                rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            }

            if (spinAttack && !spinDash)
            {
                if (wallDirection == 0)
                {
                    rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
                    isPlayerRight = true;
                    sr.flipX = false;
                }
                else
                {
                    rb2D.velocity = new Vector2(-xSpeed, rb2D.velocity.y);
                    isPlayerRight = false;
                    sr.flipX = true;
                }
            }

            if (spinDash && dashStartDelay <= 0)
            {
                if (wallDirection == 0)
                {
                    rb2D.velocity = new Vector2(-10, rb2D.velocity.y);
                }
                else
                {
                    rb2D.velocity = new Vector2(10, rb2D.velocity.y);
                }
            }
        }
    }

    void BigAttack()
    {
        if (!attackStarted)
        {
            attackSFX.Play();
        }
        attackStarted = true;
        animator.SetBool("bigAttack", true);
        if (isPlayerRight)
        {
            sr.flipX = false;
            GameObject.Find("minotaurSlashRight").GetComponent<PolygonCollider2D>().enabled = true;
        }
        else
        {
            sr.flipX = true;
            GameObject.Find("minotaurSlashLeft").GetComponent<PolygonCollider2D>().enabled = true;
        }
    }

    void checkPlayerPos()
    {
        if (!bigAttack && !spinAttack)
        {
            if (transform.position.x > GameObject.Find("Player").GetComponent<Transform>().position.x)
            {
                isPlayerRight = false;
                sr.flipX = true;
                cc2D.offset = new Vector2(0.5f, cc2D.offset.y);
            }
            else
            {
                isPlayerRight = true;
                sr.flipX = false;
                cc2D.offset = new Vector2(-0.5f, cc2D.offset.y);
            }
        }
    }

    void updateTimers()
    {
        if(redTime > 0){
            redTime -= Time.deltaTime;
            if (redTime < 0){
                redTime = 0;
                sr.color = defaultColor;
            }
        }

        if (!isStunned)
        {
            if (dashStartDelay > 0)
            {
                dashStartDelay -= Time.deltaTime;
            }
            else
            {
                dashStartDelay = 0;
            }

            if (bigAttackTime > 0)
            {
                bigAttackTime -= Time.deltaTime;
                isMoving = false;
            }
            if (bigAttackTime < 1)
            {
                GameObject.Find("minotaurSlashRight").GetComponent<PolygonCollider2D>().enabled = false;
                GameObject.Find("minotaurSlashLeft").GetComponent<PolygonCollider2D>().enabled = false;
            }
            if (bigAttackTime < 0)
            {
                animator.SetBool("bigAttack", false);
                bigAttack = false;
                attackStarted = false;

                isMoving = true;
            }



            if (!spinDash && !isStunned)
            {
                if (spinAttackTime > 0 && !spinAttack)
                {
                    spinAttackTime -= Time.deltaTime;
                }
                else
                {
                    if (!bigAttack)
                    {
                        spinAttack = true;
                        spinAttackTime = spinAttackTimeValue;
                    }
                }
            }

        }
        else
        {
            if (stunTime > 0)
            {
                stunTime -= Time.deltaTime;
            }
            else
            {
                isStunned = false;
                isVulnerable = false;
                animator.SetBool("isStunned", false);
            }
        }
    }

    public void hurt()
    {
        redTime = redTimeValue;

        sr.color = redColor;
        inmuneSFX.Stop();
        hitSFX.Play();
        isVulnerable = false;
        lives--;
        if (lives <= 0)
        {
            hitSFX.Stop();
            deathSFX.Play();
            levelEndTime = levelEndTimeValue;
            idle = true;
            animator.SetBool("death", true);
            gameObject.layer = 6;
            GetComponentInChildren<MinotaurPlayerDetection>().disable();
        }
    }

    public void resetMinotaur()
    {
        Debug.Log("lefsdfihbsud");
        idle = true;
        isPlayerRight = false;
        bigAttack = false;
        spinAttack = false;
        spinDash = false;
        isStunned = false;
        isVulnerable = false;
        attackStarted = false;

        animator.SetBool("bigAttack", false);
        animator.SetBool("isStunned", false);
        animator.SetBool("SpinAttack", false);

        spinSFX.Stop();

        GameObject.Find("dashAttack").GetComponent<PolygonCollider2D>().enabled = false;
    }


    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.collider.tag == "Wall")
        {
            if (spinDash)
            {
                spinSFX.Stop();
                impactSFX.Play();
                spinDash = false;
                GameObject.Find("dashAttack").GetComponent<PolygonCollider2D>().enabled = false;
                animator.SetBool("SpinAttack", false);
                isStunned = true;
                isVulnerable = true;
                animator.SetBool("isStunned", true);
                stunTime = stunTimeValue;
                if (collision2D.transform.position.x > transform.position.x)
                {
                    rb2D.velocity = new Vector2(-2, 5);
                }
                else
                {
                    rb2D.velocity = new Vector2(2, 5);
                }

            }
            if (spinAttack)
            {
                angrySFX.Play();
                spinSFX.Play();
                checkPlayerPos();
                spinAttack = false;
                spinDash = true;
                GameObject.Find("dashAttack").GetComponent<PolygonCollider2D>().enabled = true;

                animator.SetBool("SpinAttack", true);
                animator.SetBool("bigAttack", false);
                animator.SetBool("isRunning", false);
                dashStartDelay = dashStartDelayValue;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D colider)
    {
        if (colider.tag == "Attack")
        {
            if (isVulnerable)
            {
                hurt();
            }
            else
            {
                inmuneSFX.Play();
            }
        }

        if (colider.tag == "Big Attack")
        {
            if (isVulnerable)
            {
                hurt();
            }
            else
            {
                inmuneSFX.Play();
            }
        }
    }
}
