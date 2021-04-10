using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possessing : MonoBehaviour
{
    public float radius = 3.0f;
    public LayerMask hide;

    //On possesion effect variables
    private ParticleSystem possessionFX;
    private bool isPossesed = false;

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
            if (objects[0].gameObject.GetComponent<Possession>().possessable)
            {
                Hide(objects[0]);
            }
        }
    }
    private void Hide(Collider c)
    {
        //destroy player, and transfer to pumpkin
        Camera.main.transform.position = new Vector3(c.transform.position.x, Camera.main.transform.position.y, c.transform.position.z);
        Possession possession = c.gameObject.GetComponent<Possession>();
        possessionFX = c.gameObject.GetComponentInChildren<ParticleSystem>();
        possession.Possess();
        possession.height = transform.position.y;
        possession.enabled = true;
        if (possession.type == Possession.PossessionType.scarecrow)
        {
            c.gameObject.GetComponent<Movement>().enabled = true;
            c.gameObject.GetComponent<Animator>().SetBool("isPossesed", true);
            //c.gameObject.transform.Find("ScarePost").transform.SetParent(null);
        }
        possessionFX.Play();
        Destroy(gameObject);
    }
}
