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

        public AudioSource audioSource;
        public List<AudioClip> clips;

        // Private variables for behaviour
        NavMeshAgent agent;
        ConnectedWaypoint currentWaypoint;
        ConnectedWaypoint previousWaypoint;

        PersonSight personSight;
        bool traveling;
        bool waiting;
        float waitTimer;
        int pointsVisited;

        private Animator animator;

        [Header("Detection")]
        public DetectionBar detectionBar;
        [HideInInspector]
        public Transform lastLocation = null;
        public float rotationSpeed = 10f;

        private void Start()
        {
            agent = this.GetComponent<NavMeshAgent>();
            personSight = this.GetComponent<PersonSight>();
            animator = this.GetComponent<Animator>();

            agent.enabled = true;

            setRigidbodyState(true);
            setColliderState(false);

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
            // Animations
            animator.SetFloat("movementSpeed", agent.velocity.magnitude);
            animator.SetBool("ghostSpotted", agent.isStopped);

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

        public void Death()
        {
            Debug.Log("Scared!! ");

            Destroy(gameObject, 3);
            agent.enabled = false;

            // Disable animations
            animator.enabled = false;

            // Ragdoll
            setRigidbodyState(false);
            setColliderState(true);
            int i = Random.Range(0, clips.Count);
            audioSource.clip = clips[i];
            audioSource.Play();
        }

        void setRigidbodyState(bool state)
        {
            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

            foreach(Rigidbody rigidbody in rigidbodies)
            {
                rigidbody.isKinematic = state;
            }

        }

        void setColliderState(bool state)
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();

            foreach (Collider collider in colliders)
            {
                collider.enabled = state;
            }

            GetComponent<Collider>().enabled = !state;
        }
    }
}
