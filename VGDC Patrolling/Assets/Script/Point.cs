using System.Collections;
using UnityEngine;
public class Point : MonoBehaviour
{
    [SerializeField]
    protected float debugDrawRadius = 1.0f;


    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);
    }
}
