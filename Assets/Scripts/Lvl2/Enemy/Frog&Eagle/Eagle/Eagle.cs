using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Eagle : Enemy
{
    [NamedArrayAttribute(new string[] { "speed", "nextWaypointDistance"})]
    [SerializeField] private float[] eagleVar;
    [NamedArrayAttribute(new string[] { "target", "eagle" })]
    [SerializeField] private Transform[] eagleTrans;

    Path path;
    int currentWaypoint = 0;
    Seeker seeker;

    protected override void Start()
    {
        base.Start();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.5f); 
    }

    void UpdatePath(){
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, eagleTrans[0].position, OnPathComplete);
        }
        
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
    {
        if(path == null)
        {
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * eagleVar[0] * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < eagleVar[1])
        {
            currentWaypoint++;
        }

        if (force.x >= 0.01f)
        {
            eagleTrans[1].localScale = new Vector3(-1f, 1f, 1f);

        }
        else if (force.x <= -0.01f)
        {
            eagleTrans[1].localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
