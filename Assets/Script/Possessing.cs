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
        if (objects.Length != 0)
        {
            Hide(objects[0]);
        }
    }
    private void Hide(Collider c)
    {
        //destroy player, and transfer to pumpkin
        Camera.main.transform.position = new Vector3(c.transform.position.x, Camera.main.transform.position.y, c.transform.position.z);
        Possession possession = c.gameObject.GetComponent<Possession>();
        possession.enabled = true;
        possession.height = transform.position.y;
        if (possession.type == Possession.PossessionType.scarecrow)
        {
            c.gameObject.GetComponent<Movement>().enabled = true;
        }
        Destroy(gameObject);
    }
}
