using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float walkSpeed;
    public float jumpHeight;
    public float cameraSensitivity;
    public GameObject cameraCenter;
    public GameObject leftFist;
    public GameObject rightFist;
    Animator animator;
    Rigidbody rb;
    bool onGround;
    float cameraVerticalAngle;
    Vector2 attackDirection;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        Animate();
    }

    void Move()
    {
        //Walk
        if(rb.velocity.magnitude < walkSpeed)
        {
            if(Input.GetKey(KeyCode.W))
            {
                rb.AddForce(transform.forward*walkSpeed);
                rb.AddForce(transform.up*0.75f);    
            }
            if(Input.GetKey(KeyCode.S))
            {
                rb.AddForce(transform.forward*-walkSpeed);
                rb.AddForce(transform.up*0.75f);   
            }
            if(Input.GetKey(KeyCode.A))
            {
                rb.AddForce(transform.right*-walkSpeed); 
            }
            if(Input.GetKey(KeyCode.D))
            {
                rb.AddForce(transform.right*walkSpeed);  
            }
        } 

        //Jump
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(onGround)
            {
                StartCoroutine("Jump");    
            }
        }

        //Move Camera
        gameObject.transform.Rotate(transform.up*Input.GetAxis("Mouse X")*cameraSensitivity);
        cameraVerticalAngle -= Input.GetAxis("Mouse Y")*cameraSensitivity;
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -30, 30);
        cameraCenter.transform.localRotation = Quaternion.Euler(cameraVerticalAngle,0,0);

        //Combat
        if(Input.GetMouseButton(0))
        {
            if(Input.GetAxis("Mouse X") > 0.15f)
            {
                attackDirection = new Vector2(1,0);
                animator.SetTrigger("WindUpRight");
            }
            else if(Input.GetAxis("Mouse X") < -0.15f)
            {
                attackDirection = new Vector2(-1,0);
                animator.SetTrigger("WindUpLeft");
            }
            else if(Input.GetAxis("Mouse Y") > 0.1f)
            {
                attackDirection = new Vector2(0,1);
                animator.SetTrigger("WindUpTop");
            }
        }

        if(Input.GetMouseButtonUp(0) && attackDirection.magnitude > 0)
        {
            StartCoroutine("Attack");
        }
    }

    void Animate()
    {
        if(rb.velocity.magnitude > 0.1f)
        {
            animator.SetBool("Walking", true);
        }
        else if(rb.velocity.magnitude < 0.01f)
        {
            animator.SetBool("Walking", false);
        }
    }

    IEnumerator Attack()
    {
        ResetTriggers(animator);
        animator.SetTrigger("Attack");
        if(attackDirection.x > 0 || attackDirection.y > 0)
        {
            rightFist.SetActive(true); 
        }
        else if(attackDirection.x < 0)
        {
            leftFist.SetActive(true); 
        }
        yield return new WaitForSeconds(0.5f);
        rightFist.SetActive(false); 
        leftFist.SetActive(false);
    }

    IEnumerator Jump()
    {
        onGround = false;
        GetComponent<BoxCollider>().enabled = false;
        animator.SetBool("Jumping", true);
        yield return new WaitForSeconds(0.3f);
        rb.AddForce(transform.up*jumpHeight);
        yield return new WaitForSeconds(0.5f);
        GetComponent<BoxCollider>().enabled = true;
    }

    void ResetTriggers(Animator animator)
    {
        foreach(AnimatorControllerParameter parameter in animator.parameters)
        {
            if(parameter.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(parameter.name);
            }
        }
    }

    void OnTriggerStay()
    {
        onGround = true;
        animator.SetBool("Jumping", false); 
    }

    void OnTriggerExit()
    {
        onGround = false;
    }
}
