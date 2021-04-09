using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScarecrowBar : MonoBehaviour
{
    private Canvas scarecrowCanvas;
    public Slider timebar;
    public Possession objectPossess;

    public float value;
    public bool TimerOn = false;
    bool exit = true;
    // Start is called before the first frame update
    void Start()
    {
        scarecrowCanvas = GetComponent<Canvas>();
        scarecrowCanvas.enabled = false;
    }

    //enables canvas and starts the scarecrow timer
    //if leave is true then its for scarecrow movement
    //if leave is false then its for scarecrow possessability
    public void StartTimer(float val, bool leave)
    {
        exit = leave;
        value = val;
        timebar.maxValue = value;
        timebar.value = value;
        scarecrowCanvas.enabled = true;
        TimerOn = true;
    }
    public void StopTimer()
    {
        TimerOn = false;
        scarecrowCanvas.enabled = false;
        timebar.value = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (TimerOn)
        {
            //Decrease timer
            value -= Time.deltaTime;
            timebar.value = value;
        }
        if (timebar.value <= 0 && TimerOn)
        {
            //Stop timer, and exit scarecrow
            StopTimer();
            //removes player from scarecrow or makes scarecrow possessable
            if (exit)
                objectPossess.Exit();
            else
                objectPossess.possessable = true;
        }
    }
}
