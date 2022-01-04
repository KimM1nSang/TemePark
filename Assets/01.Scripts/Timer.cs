using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Timer
{
    public float time { get; set; }
    private float maxTime;

    public void TimeFlow(Action action)
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            TimerReset();
            action?.Invoke();
        }
    }

    public float GetMaxTime()
    {
        return maxTime;
    }
    public void Setup(float maxTime)
    {
        SetMaxTime(maxTime);
        TimerReset();
    }
    public void SetMaxTime(float maxTime)
    {
        this.maxTime = maxTime;
    }
    public void TimerReset()
    {
        time = maxTime;
    }
}