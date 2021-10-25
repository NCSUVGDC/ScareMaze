using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessableFX : MonoBehaviour
{

    private ParticleSystem p;
    public float timer;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (p != null && timer < 0)
        {
            p.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ghost"))
        {
            p = gameObject.GetComponentInChildren<ParticleSystem>();
            timer = .5f;
        }
        if (p != null) {
            p.Play();
        }
        
    }

    
}
