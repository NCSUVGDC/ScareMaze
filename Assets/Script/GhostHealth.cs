using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostHealth : MonoBehaviour
{
    public Slider healthBar;
    public GameObject ghost;
    public float health = 100f;
    public float dps = 10f;
    public float damageCooldown = .1f;
    private float lastHitTime;
    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = health;
        healthBar.value = health;
        lastHitTime = -damageCooldown;
    }
    public void takeDamage()
    {
        if (Time.time > damageCooldown + lastHitTime)
        {
            //print("ghost hit, remaining health: " + health);
            health -= (dps * damageCooldown);
            //displays ghost health
            healthBar.value = health;
            if (health <= 0)
            {
                print("ghost dead");
                Destroy(gameObject);
                FindObjectOfType<GameManager>().LoseGame();
            }
            lastHitTime = Time.time;
        }
    }
}
