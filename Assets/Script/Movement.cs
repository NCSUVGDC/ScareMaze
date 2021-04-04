using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //code for basic movement and to enter possession state
    public Rigidbody player;
    public int speed = 5;

    //array of objects player can sense
    //private Collider[] objects;
    
    //Animator animator;

    //how far away the player can sense
    public float radius = 3.0f;
    private Camera newcamera = new Camera();
    public LayerMask hide;
    Vector3 moveUp;
    Vector3 moveRight;
    void Start()
    {
        //animator = gameObject.GetComponent<Animator>();
        moveUp = Camera.main.transform.up;
        moveRight = Camera.main.transform.right;
    }
    void FixedUpdate()
    {
        //face mouse (probably could be improved)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 target = ray.GetPoint(distance);
            Vector3 direction = target - transform.position;
            float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
        //top lines are movement relative to ghost direction, bottom are constants
        //move forward
        if (Input.GetKey(KeyCode.W))
        {
            //animator.SetTrigger("Move");
            //player.position += transform.forward * speed * Time.deltaTime;
            Move(moveUp);
        }
        //move backward
        if (Input.GetKey(KeyCode.S))
        {
            //player.position += -transform.forward * speed * Time.deltaTime;
            Move(-moveUp);
        }
        //strafe right
        if (Input.GetKey(KeyCode.D))
        {
            //player.position += transform.right * speed*Time.deltaTime;
            Move(moveRight);
        }
        //strafe left
        if (Input.GetKey(KeyCode.A))
        {
            //player.position += -transform.right * speed*Time.deltaTime;
            Move(-moveRight);
        }
    }
    void Move(Vector3 dir)
    {
        //move player and camera
        //Camera.main.transform.position += dir * speed * Time.deltaTime;
        player.position += dir * speed * Time.deltaTime;
        Camera.main.transform.position = new Vector3(player.transform.position.x, Camera.main.transform.position.y, player.transform.position.z);
    }
}
