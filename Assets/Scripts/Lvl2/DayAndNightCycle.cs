using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DayAndNightCycle : MonoBehaviour
{
    [SerializeField] private Gradient lightColor;
    [SerializeField] private GameObject globalLight;

    private int days;

    public int Days => days;

    private float time = 10;

    private bool canChangeDay = true;

    public delegate void OnDayChanged();

    public OnDayChanged DayChanged;

    private void Update()
    {
        if(time > 50)
        {
            time = 0;
        }

        if((int)time == 100 && canChangeDay) //250 is where we want the day to essentially complete when the "day" variable then increments by 1
        {
            canChangeDay = false;
            DayChanged();
            days++;

        }

        if((int) time == 200)
        {
            canChangeDay = true;
        }

        time += Time.deltaTime;
        globalLight.GetComponent<Light2D>().color = lightColor.Evaluate(time * 0.02f); //the 0.002f is for the evaluate method to get 500 between value of 0 and 1 so take
        //1 and divide it by the length of the cycle to get the number that goes here. 
    }

}
