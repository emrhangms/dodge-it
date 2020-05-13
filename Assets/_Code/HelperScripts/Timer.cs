using System;
using UnityEngine;

[Serializable]
public class Timer
{
    public float Duration;
    [HideInInspector]
    public float StartTime;
    [HideInInspector]
    public bool TimeStart;

    public void StartCountDown ()
    {
        StartTime = Time.time;
    }

    public (float, bool) IsFinished ()
    {
        float ta = Time.time - StartTime;
        if (Time.time - StartTime > Duration)
            return (0f, true);

        return (ta, false);
    }

}