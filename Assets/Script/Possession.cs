using Assets.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Possession : MonoBehaviour
{
    private Person person;
    public float timeLimit = 30.0f;
    public ScarecrowBar scarecrowBar;
    float possessionTime = 0f;
    Vector3 startpos;
    Quaternion startrot;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        startpos = transform.position;
        startrot = transform.rotation;
        scarecrowBar = gameObject.GetComponent<ScarecrowBar>();
    }
    public enum PossessionType
    {
        pumpkin,
        scarecrow
    }
    [HideInInspector]
    public bool possessable = true;
    public PossessionType type;
    private Collider[] objects;
    public LayerMask NPC;
    public LayerMask block;
    public float radius = 3.0f;
    public GameObject player;
    public float height;
    bool possessed = false; 
    public float waitTime = 5f;
    public void Possess()
    {
        possessed = true;
        if (type == PossessionType.scarecrow)
        {
            scarecrowBar.StartTimer(timeLimit,true);
        }
    }
    // Update is called once per frame
    private void Update()
    {
        if (possessed)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                possessed = false;
                if (type.Equals(PossessionType.scarecrow))
                    scarecrowBar.StopTimer();
                Exit();
            }
        }
    }

    public void Exit()
    {
        possessed = false;
        //jump out of object, scare NPC, instantiate new player
        if (type.Equals(PossessionType.scarecrow))
        {
            gameObject.GetComponent<Movement>().enabled = false;
            gameObject.GetComponent<Animator>().SetBool("isMoving", false);
            gameObject.GetComponent<Animator>().SetBool("isPossesed", false);
            transform.rotation = Quaternion.identity;
        }
        var newplayer = Instantiate(player, new Vector3(transform.position.x + transform.forward.x, height, transform.position.z + transform.forward.z), transform.rotation) as GameObject;
        Identify();
        Camera.main.transform.position = new Vector3(player.transform.position.x, Camera.main.transform.position.y, player.transform.position.z);
        this.enabled = false;
        possessionTime = 0f;
        transform.position = startpos;
        transform.rotation = startrot;
        //transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
    void Identify()
    {
        //find all NPC objects within the radius then verify there is not a wall between you and them
        objects = Physics.OverlapSphere(transform.position, radius, NPC);
        foreach (Collider c in objects)
        {
            Transform target = c.transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            float disToTarget = Vector3.Distance(transform.position, target.position);
            if (!Physics.Raycast(transform.position, dirToTarget, disToTarget, block))
            {
                Kill(c);
                break;
            }
        }
        objects = null;
    }
    void Kill(Collider c)
    {
        // Run "Death" Function
        c.gameObject.GetComponent<Person>().Death();

        // Decrease counter of AI players
        FindObjectOfType<GameManager>().PlayerDeath();

        //c.gameObject.GetComponent<Person>().enabled = false;
        //c.gameObject.GetComponent<PersonSight>().enabled = false;
        //c.transform.rotation = Quaternion.Euler(90, 0, 0);
        //c.gameObject.tag = "Dead";
        //c.gameObject.layer = 9;
        //c.gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
    }
    private void OnDrawGizmos()
    {
        //shows scare distance
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
