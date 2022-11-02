using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour

{
    public Rigidbody RB;
    
    bool isGrounded;
    int isJumping;
    bool isTouchingWallRight;
    bool isTouchingWallLeft;
    float horizontalValue;
    int maxHealth=5;
    int currentHealth;
    Vector3 normal;
    [SerializeField] int WallForce;
    [SerializeField] int speed;
    [SerializeField] int jumpspeed;
    public HealthBar healthBar;
    

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        isTouchingWallRight = false;
        isTouchingWallLeft = false;
        isJumping = 0;
        
        RB = GetComponent<Rigidbody>();

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            TakeDamage(1);
        }
        horizontalValue = Input.GetAxis("Horizontal");
        //Debug.Log("Horizontal " + horizontalValue);
        WallJump();
        //Dash();
        Movement();
        Jump();
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    public void WallJump()
    {

        if (Input.GetKeyDown(KeyCode.Space) && (isTouchingWallRight == true))
        {
            RB.AddForce(normal * WallForce + new Vector3(0, 0, 1));



        }



        if (Input.GetKeyDown(KeyCode.Space) && (isTouchingWallLeft == true))
        {
            RB.AddForce(-normal * WallForce);
            RB.AddForce(new Vector3(0, 0, 1));

        }

    }


    public void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && ((Input.GetKeyDown(KeyCode.A) || (Input.GetKeyDown(KeyCode.D)))))
        {

            RB.AddExplosionForce(1000, RB.position, 8000f);
        }

    }



    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (isJumping < 1))
        {
            RB.AddForce(Vector3.up * jumpspeed);

            isJumping++;
        }
        else if (isGrounded == true || isTouchingWallRight == true || isTouchingWallLeft == true)
        {
            {
                isJumping = 0;
            }
        }

    }
    public void Movement()
    {
        RB.AddForce(Vector3.forward * speed * Time.deltaTime * Input.GetAxis("Horizontal"));


    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "WallRight")
        {
            isTouchingWallRight = true;
            normal = collision.gameObject.transform.forward;
        }

        if (collision.gameObject.tag == "WallLeft")
        {
            isTouchingWallLeft = true;
            normal = collision.gameObject.transform.forward;
        }
        if (collision.gameObject.tag == "Spike")
        {
            TakeDamage(1);
        }

    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;

        }
        if (collision.gameObject.tag == "WallRight")
        {
            isTouchingWallRight = false;
        }

        if (collision.gameObject.tag == "WallLeft")
        {
            isTouchingWallLeft = false;
        }

    }
}
