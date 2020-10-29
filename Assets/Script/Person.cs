using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Code
{
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
        ConnectedWaypoint currentWaypoint;
        ConnectedWaypoint previousWaypoint;

        PersonSight personSight;
        bool traveling;
        bool waiting;
        float waitTimer;
        int pointsVisited;

        [Header("Detection")]
        public DetectionBar detectionBar;
        [HideInInspector]
        public Transform lastLocation = null;
        public float rotationSpeed = 10f;

        private void Start()
        {
            agent = this.GetComponent<NavMeshAgent>();
            personSight = this.GetComponent<PersonSight>();

            if(currentWaypoint == null)
            {
                // Set it to random
                // Grab all Points in Scene
                GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Point");

                if(allWaypoints.Length > 0)
                {
                    while (currentWaypoint == null)
                    {
                        int random = UnityEngine.Random.Range(0, allWaypoints.Length);
                        ConnectedWaypoint startingWaypoint = allWaypoints[random].GetComponent<ConnectedWaypoint>();

                        // If found a Point
                        if (startingWaypoint != null)
                        {
                            currentWaypoint = startingWaypoint;
                        }
                    }
                }
                else
                {
                    Debug.LogError("Failed to find any Points");
                }
            }

            SetDestination();
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
                    pointsVisited++;

                    // Wait if waiting
                    if (patrolWaiting)
                    {
                        waiting = true;
                        waitTimer = 0f;
                    }
                    else
                    {
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

                        SetDestination();
                    }
                }
            }
            else
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

            if (lastLocation != null)
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
            if (pointsVisited > 0)
            {
                ConnectedWaypoint nextWaypoint = currentWaypoint.NextWaypoint(previousWaypoint);
                previousWaypoint = currentWaypoint;
                currentWaypoint = nextWaypoint;
            }

            Vector3 targetVector = currentWaypoint.transform.position;
            agent.SetDestination(targetVector);
            traveling = true;
        }
    }
}
