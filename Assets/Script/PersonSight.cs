using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonSight : MonoBehaviour
{
    // Max View Distance 
    public float viewDistance;
    // View Angle
    [Range(0,360)]
    public float viewAngle;
    public bool ghostSpotted;
    public bool scarecrowSpotted;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    [HideInInspector]
    public Transform targetLastLocation;

    private void Start()
    {
        StartCoroutine("FindTargetsWithDelay", 0.2f);
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisableTargets();
        }
    }

    void FindVisableTargets()
    {
        targetLastLocation = null;
        // Make array of target Colliders within View radius
        Collider[] targetsinViewRadius = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for(int i = 0; i < targetsinViewRadius.Length; i++)
        {
            Transform target = targetsinViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            // If angle from forward vector is withing visual range...
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle/2)
            {
                // Get distance to target
                float disToTarget = Vector3.Distance(transform.position, target.position);
                // Use Raycast to find obstacles in the way
                if (!Physics.Raycast(transform.position, dirToTarget, disToTarget, obstacleMask))
                {                    
                    // No obstacles in way, Ghost spotted!
                    targetLastLocation = target.transform;
                    scarecrowSpotted = target.parent.name.Equals("Scarecrow");
                    ghostSpotted = !scarecrowSpotted;
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDeg, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDeg += transform.eulerAngles.y;
        }
        // Trig Time
        return new Vector3(Mathf.Sin(angleInDeg * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDeg * Mathf.Deg2Rad));
    }
}
