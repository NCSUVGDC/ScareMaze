using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHealth : MonoBehaviour
{
    public GameObject ghost;
    public float health = 100f;
    public float dps = 10f;
    public float damageCooldown = .1f;
    private float lastHitTime;
    // Start is called before the first frame update
    void Start()
    {
        lastHitTime = -damageCooldown;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
    public void takeDamage()
    {
        if (Time.time > damageCooldown + lastHitTime)
        {
            print("ghost hit, remaining health: " + health);
            health -= (dps * damageCooldown);
            if (health <= 0)
            {
                print("ghost dead");
                Destroy(gameObject);
            }
            lastHitTime = Time.time;
        }
    }
}
