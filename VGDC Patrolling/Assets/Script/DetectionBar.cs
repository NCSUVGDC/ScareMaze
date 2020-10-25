using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectionBar : MonoBehaviour
{
    private Canvas personCanvas;
    public Slider detectionBar;
    public PersonSight personSight;

    public float alert;
    public float maxAlert = 100.0f;

    public bool TimerOn;

    // Start is called before the first frame update
    void Start()
    {
        personCanvas = GetComponent<Canvas>();
        // HIDE
        personCanvas.enabled = false;
        SetupTimer();
    }

    void SetupTimer()
    {
        TimerOn = false;
        alert = maxAlert;
        detectionBar.maxValue = maxAlert;
        detectionBar.value = alert;
    }

    // Update is called once per frame
    void Update()
    {
        // Face Camera
        transform.LookAt(Camera.main.transform);

        if (TimerOn)
        {
            // SHOW
            personCanvas.enabled = true;
            // Decrease Timer
            alert -= 0.1f;
            detectionBar.value = alert;
        }

        if (detectionBar.value <= 0)
        {
            // Timer ends, reset Timer
            //HIDE
            personCanvas.enabled = false;
            personSight.ghostSpotted = false;
            SetupTimer();

        }
    }
}
