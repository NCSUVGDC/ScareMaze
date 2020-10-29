using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class LightDamage : MonoBehaviour
{
    public float lightDistance = 15f;
    [Range(0, 360)]
    public float fovAngle = 55f;
    private Vector3 rayDirection;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public string ghostTagName = "Ghost";
    public GhostHealth ghostDamage;

    void FindVisibleTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, lightDistance, targetMask);
       
        for (int i = 0; i < targets.Length; i++) { 
            Transform target = targets[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            
            if ((Vector3.Angle(transform.up, dirToTarget) < (fovAngle / 2)))
            {
                
                float distToTarget = Vector3.Distance(transform.position, target.position);
                RaycastHit hit;
                // debug: print("found target, angle: " + Vector3.Angle(transform.up, dirToTarget) + " distance: " + Vector3.Distance(transform.position, target.position));
                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    Physics.Raycast(transform.position, dirToTarget, out hit, distToTarget, targetMask);
                    
                    GameObject hitObject = hit.collider.gameObject;
                    if (hitObject.tag == ghostTagName)
                    {
                        //print("hit ghost");
                        ghostDamage.takeDamage();
                    }
                }
                else
                {
                    //debug: print("hit something else");
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    public Vector3 Dir2FromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(0, Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        FindVisibleTarget();
    }
}