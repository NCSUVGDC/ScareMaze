using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Person : MonoBehaviour
{
    [Header("Person AI")]
    // Whether the Person waits on each Point
    [SerializeField]
    bool patrolWaiting = true;

    // Time to wait at each Point
    [SerializeField]
    float waitTime = 3f;

    // Probability of switching direction
    [SerializeField]
    float switchProb = 0.2f;

    // List of Points to vist
    [SerializeField]
    List<Point> patrolPoints = null;

    // Private variables for behaviour
    NavMeshAgent agent;
    PersonSight personSight;
    int currentPatrolIndex;
    bool traveling;
    bool waiting;
    bool patrolForward;
    float waitTimer;

    [Header("Detection")]
    public DetectionBar detectionBar;
    [HideInInspector]
    public Transform lastLocation = null;
    public float rotationSpeed = 10f;

    private void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        personSight = this.GetComponent<PersonSight>();

        if (patrolPoints != null && patrolPoints.Count >= 2)
        {
            currentPatrolIndex = 0;
            SetDestination();
        } else
        {
            Debug.LogError("Need more than 2 Points on " + gameObject.name);
        }
    }

    private void Update()
    {
        if (!personSight.ghostSpotted)
        {
            agent.isStopped = false;
            lastLocation = null;
            // Check if close to destination
            if (traveling && agent.remainingDistance <= 1.0f)
            {
                traveling = false;

                // Wait if waiting
                if (patrolWaiting)
                {
                    waiting = true;
                    waitTimer = 0f;
                }
                else
                {
                    ChangePatrolPoint();
                    SetDestination();
                }

            }

            // Instead, if we're waiting
            if (waiting)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitTime)
                {
                    waiting = false;

                    ChangePatrolPoint();
                    SetDestination();
                }
            }
        } else
        {
            GhostSeen();
        }
    }

    public void GhostSeen()
    {
        //Toggle movement
        agent.isStopped = true;

        // Restart and start Timer
        //detectionBar.alert = detectionBar.maxAlert;
        detectionBar.TimerOn = true;

        if(lastLocation != null)
        {
            // Look towards lastLocation
            lastLocation = personSight.targetLastLocation.transform;
            Vector3 lookPos = lastLocation.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);
        }
    }

    private void SetDestination()
    {
        if (patrolPoints != null)
        {
            // Get Vector of current Patrol Point from List
            Vector3 targetVector = patrolPoints[currentPatrolIndex].transform.position;
            // Set destination to Vector and travel to the Point
            agent.SetDestination(targetVector);
            traveling = true;
        }
    }

    // Selects a new Patrol Point in the list with a probability for moving forward or backwards
    private void ChangePatrolPoint()
    {
        // Random number between 0-1 & if less than Probability
        if (UnityEngine.Random.Range(0f, 01) <= switchProb)
        {
            // Patrol forward or backwards
            patrolForward = !patrolForward;
        }

        // Check if "overlapping" list (Can't go back if you're at the top of the list)
        if (patrolForward)
        {

            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }
        else
        {
            if(--currentPatrolIndex < 0)
            {
                currentPatrolIndex = patrolPoints.Count - 1;
            }
        }
    }

}
