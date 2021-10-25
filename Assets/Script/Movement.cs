using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //code for basic movement and to enter possession state
    public Rigidbody player;
    public int speed = 5;
    public int dashSpeed = 15;
    private Vector3 currentDir;
    public float dashTimer = 0f;
    public float dashDuration = .3f;
    private bool dashing = false;


    //array of objects player can sense
    //private Collider[] objects;

    private Animator animator;
    private ParticleSystem ectoplasm;
    private bool moving = false;

    //how far away the player can sense
    public float radius = 3.0f;
    private Camera newcamera = new Camera();
    public LayerMask hide;
    Vector3 moveUp;
    Vector3 moveRight;
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        ectoplasm = gameObject.GetComponentInChildren<ParticleSystem>();
        moveUp = Camera.main.transform.up;
        moveRight = Camera.main.transform.right;
    }
    void FixedUpdate()
    {
        //face mouse (probably could be improved)
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Plane plane = new Plane(Vector3.up, Vector3.zero);
        //float distance;
        //if (plane.Raycast(ray, out distance))
        //{
        //    Vector3 target = ray.GetPoint(distance);
        //    Vector3 direction = target - transform.position;
        //    float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        //    transform.rotation = Quaternion.Euler(0, rotation, 0);
        //}
        //top lines are movement relative to ghost direction, bottom are constants

        dashTimer -= Time.deltaTime;
        //Try to dash first if able (only for ghost)
        if (dashing && gameObject.CompareTag("Ghost")) 
        {
            dashDuration -= Time.deltaTime;
            player.position += currentDir * dashSpeed * Time.deltaTime;
            Camera.main.transform.position = new Vector3(player.transform.position.x, Camera.main.transform.position.y, player.transform.position.z);
            if (dashDuration < 0) {
                dashDuration = .3f;
                dashing = false;
            }
        }
        //move forward
        else if (Input.GetKey(KeyCode.W))
        {
            //player.position += transform.forward * speed * Time.deltaTime;
            currentDir = moveUp;
            if (Input.GetKey(KeyCode.LeftShift) && dashTimer < 0 && !dashing)
            {
                dashTimer = 1f;
                dashing = true;
            }
            else
            {
                Move();
            }

            animator.SetBool("isMoving", true);
        }
        //move backward
        else if (Input.GetKey(KeyCode.S))
        {
            //player.position += -transform.forward * speed * Time.deltaTime;
            currentDir = -moveUp;
            if (Input.GetKey(KeyCode.LeftShift) && dashTimer < 0 && !dashing)
            {
                dashTimer = 1f;
                dashing = true;
            }
            else
            {
                Move();
            }
            animator.SetBool("isMoving", true);
        }
        //strafe right
        else if (Input.GetKey(KeyCode.D))
        {
            //player.position += transform.right * speed*Time.deltaTime;
            currentDir = moveRight;
            if (Input.GetKey(KeyCode.LeftShift) && dashTimer < 0 && !dashing)
            {
                dashTimer = 1f;
                dashing = true;
            }
            else
            {
                Move();
            }
            animator.SetBool("isMoving", true);
        }
        //strafe left
        else if (Input.GetKey(KeyCode.A))
        {
            //player.position += -transform.right * speed*Time.deltaTime;
            currentDir = -moveRight;
            if (Input.GetKey(KeyCode.LeftShift) && dashTimer < 0 && !dashing)
            {
                dashTimer = 1f;
                dashing = true;
            }
            else
            {
                Move();
            }
            animator.SetBool("isMoving", true);
        }
        else {
            //set animator to idle
            animator.SetBool("isMoving", false);
            ectoplasm.Stop();
            moving = false;
        }
    }

    void Move()
    {
        //move player and camera
        //Camera.main.transform.position += dir * speed * Time.deltaTime;

        if (!moving) 
        {
            ectoplasm.Play();
            moving = true;
        }

        //Lerp player to move direction
        float rotation = Mathf.Atan2(currentDir.x, currentDir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation, 0), Time.deltaTime * speed);

        //Move player
        player.position += currentDir * speed * Time.deltaTime;
        Camera.main.transform.position = new Vector3(player.transform.position.x, Camera.main.transform.position.y, player.transform.position.z);
    }

}
