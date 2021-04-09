using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedWaypoint : Point
{
    [SerializeField]
    protected float connectivityRadius = 50f;

    List<ConnectedWaypoint> connections;

    public void Start()
    {
        // Grab all Points in the Scene
        GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Point");

        // Create list of Points to refer to
        connections = new List<ConnectedWaypoint>();

        // Check if they're a connected Point
        for (int i = 0; i < allWaypoints.Length; i++)
        {
            ConnectedWaypoint nextWaypoint = allWaypoints[i].GetComponent<ConnectedWaypoint>();
            // If a ConnectedPoint is found
            if (nextWaypoint != null)
            {
                if (Vector3.Distance(this.transform.position, nextWaypoint.transform.position) <= connectivityRadius && nextWaypoint != this)
                {
                    connections.Add(nextWaypoint);
                }
            }
        }
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);

        // Visual Connective Range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, connectivityRadius);
    }

    public ConnectedWaypoint NextWaypoint(ConnectedWaypoint previousWaypoint)
    {
        if (connections.Count == 0)
        {
            //Return null if no points
            Debug.LogError("Not enough ConnectedPoints");
            return null;
        }
        else if (connections.Count == 1 && connections.Contains(previousWaypoint))
        {
            // If the previous one is only Point, use it
            return previousWaypoint;
        }
        else // Otherwise, find a random one
        {
            ConnectedWaypoint nextWaypoint;
            int nextIndex = 0;

            do
            {
                nextIndex = UnityEngine.Random.Range(0, connections.Count);
                nextWaypoint = connections[nextIndex];
            } while (nextWaypoint == previousWaypoint);

            return nextWaypoint;
        }
    }
}
