using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possession : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Possession>().enabled = false;
    }
    public enum PossessionType
    {
        pumpkin,
        scarecrow
    }
    public PossessionType type;
    private Collider[] objects;
    public LayerMask NPC;
    public LayerMask block;
    public float radius = 3.0f;
    public GameObject player;
    public float height;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        switch (type)
        {
            case PossessionType.scarecrow:
            {
                    Vector3 forward = transform.forward;
                    if (Input.GetKey(KeyCode.E))
                    {
                        transform.rotation = Quaternion.Euler(25, transform.rotation.eulerAngles.y, 0);
                        Identify();
                    }
                    break;
            }
            case PossessionType.pumpkin:
            {
                    break;
            }
            default: break;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            //create new player and deactivate code
            Instantiate(player,new Vector3(transform.position.x+transform.forward.x, height, transform.position.z + transform.forward.z), transform.rotation);
            Identify();
            Camera.main.transform.position = new Vector3(player.transform.position.x, Camera.main.transform.position.y, player.transform.position.z);
            gameObject.GetComponent<Possession>().enabled = false;
        }
    }
    void Identify()
    {
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
        c.transform.rotation = Quaternion.Euler(90, 0, 0);
        c.gameObject.tag = "Dead";
        c.gameObject.layer = 9;
        c.gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
    }
}
