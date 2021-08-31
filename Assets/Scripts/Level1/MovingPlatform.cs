/*
 Script for moving platform in lvl1 by Eshan Kang
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public List<Transform> points;
    public Transform platform;
    int goalPoint = 0;
    public float speed = 2;

    private void Update()
    {
        MoveToNextPoint();
    }

    void MoveToNextPoint()
    {
        platform.position = Vector2.MoveTowards(platform.position, points[goalPoint].position, Time.deltaTime * speed);
        if (Vector2.Distance(platform.position, points[goalPoint].position) < 0.1f)
        {
            if (goalPoint == points.Count - 1)
                goalPoint = 0;
            else
                goalPoint++;
        }
    }
}
