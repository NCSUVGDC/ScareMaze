using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asset.Code
{
    public class ConnectPoint : Point
    {
        [SerializeField]
        protected float connectivityRadius = 50f;

        List<ConnectPoint> connections;

        public void Start()
        {
            // Grab all Points in the Scene
            GameObject[] allConnectedPoints = GameObject.FindGameObjectsWithTag("Point");

            // Create list of Points to refer to
            connections = new List<ConnectPoint>();

            // Check if they're a connected Point
            for (int i = 0; i < allConnectedPoints.Length; i++)
            {
                ConnectPoint nextPoint = allConnectedPoints[i].GetComponent<ConnectPoint>();

                // If a ConnectedPoint is found
                if(nextPoint != null)
                {
                    if(Vector3.Distance(this.transform.position, nextPoint.transform.position) <= connectivityRadius && nextPoint != this)
                    {
                        connections.Add(nextPoint);
                    }
                }
            }
        }

        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, debugDrawRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, connectivityRadius);
        }
        /**
        public ConnectPoint NextPoint(ConnectPoint previousPoint)
        {
            if (connections.Count == 0)
            {
                //Return null if no points
                Debug.LogError("Not enough ConnectedPoints");
                return null;
            }
            else if (connections.Count == 1 && connections.Contains(previousPoint))
            {
                // If the previous one
            }
        }
        **/
    }
}

