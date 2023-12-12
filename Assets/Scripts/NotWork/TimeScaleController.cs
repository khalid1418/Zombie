using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleController : MonoBehaviour
{
    [SerializeField] private float m_Scale;

    public void SlowDownTime()
    {
        Time.timeScale = m_Scale;
    }

    public void SpeedUpTime()
    {
        Time.timeScale = 1f;
    }

}
