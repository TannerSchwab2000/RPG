using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float detectionRange;
    public float walkSpeed;

    Player player;
    Rigidbody rb;
    Animator animator;

    void Start()
    {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Animate();
    }

    void Move()
    {
        float distanceToPlayer = (player.gameObject.transform.position - transform.position).magnitude;
        if(distanceToPlayer < detectionRange && distanceToPlayer > 0.35f)
        {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            if(rb.velocity.magnitude < walkSpeed)
            {
                rb.AddForce(transform.forward);
                rb.AddForce(transform.up*0.5f);
            }
        }
    }

    void Animate()
    {
        if(rb.velocity.magnitude > 0.1f)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false); 
        }
    }
}
