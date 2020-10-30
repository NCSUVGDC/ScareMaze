using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possessing : MonoBehaviour
{
    public float radius = 3.0f;
    public LayerMask hide;
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Nearby(transform.position);
        }
    }
    private void Nearby(Vector3 center)
    {
        //create array of all objects within certain distance
        Collider[] objects = Physics.OverlapSphere(center, radius, hide);
        Hide(objects[0]);
    }
    private void Hide(Collider c)
    {
        //destroy player, and transfer to pumpkin
        Camera.main.transform.position = new Vector3(c.transform.position.x, Camera.main.transform.position.y, c.transform.position.z);
        c.gameObject.GetComponent<Possession>().enabled = true;
        c.gameObject.GetComponent<Possession>().height = transform.position.y;
        Destroy(gameObject);
    }
}
