using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(PersonSight))]
public class PersonSightEditor : Editor
{
    private void OnSceneGUI()
    {
        PersonSight fov = (PersonSight)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewDistance);
        Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2, false);
        Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2, false);

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewDistance);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewDistance);
    }
}
