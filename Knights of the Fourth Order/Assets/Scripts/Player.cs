using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float walkSpeed;
    public float jumpHeight;
    public float cameraSensitivity;
    public GameObject cameraCenter;
    Animator animator;
    Rigidbody rb;
    bool onGround;
    float cameraVerticalAngle;

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
