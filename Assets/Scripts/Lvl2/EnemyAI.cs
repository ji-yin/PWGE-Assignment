using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    public Transform enemyGFX;

    Path path;
    int currentWaypoint = 0;
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD:Assets/Scripts/Lvl2/Enemy/Frog&Eagle/Eagle/Eagle.cs
=======
    bool reachedEndOfPath = false;

>>>>>>> parent of b93abbce (lvl3):Assets/Scripts/Lvl2/EnemyAI.cs
=======
    //bool reachedEndOfPath = false;

>>>>>>> parent of 639fb924 (Delete files)
=======
<<<<<<<< HEAD:Assets/Scripts/Lvl2/EnemyAI.cs
    bool reachedEndOfPath = false;

========
>>>>>>>> parent of b93abbce (lvl3):Assets/Scripts/Lvl2/Enemy/Frog&Eagle/Eagle/Eagle.cs
>>>>>>> parent of b93abbce (lvl3)
    Seeker seeker;
    Rigidbody2D rb;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);

        
    }

    void UpdatePath(){
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
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
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD:Assets/Scripts/Lvl2/Enemy/Frog&Eagle/Eagle/Eagle.cs
            return;
        }
=======
            reachedEndOfPath = true;
=======
            //reachedEndOfPath = true;
>>>>>>> parent of 639fb924 (Delete files)
=======
<<<<<<<< HEAD:Assets/Scripts/Lvl2/EnemyAI.cs
            reachedEndOfPath = true;
>>>>>>> parent of b93abbce (lvl3)
            return;
        }
        else
        {
<<<<<<< HEAD
<<<<<<< HEAD
            reachedEndOfPath = false;
        }
>>>>>>> parent of b93abbce (lvl3):Assets/Scripts/Lvl2/EnemyAI.cs
=======
            //reachedEndOfPath = false;
        }
>>>>>>> parent of 639fb924 (Delete files)
=======
            reachedEndOfPath = false;
        }
========
            return;
        }
>>>>>>>> parent of b93abbce (lvl3):Assets/Scripts/Lvl2/Enemy/Frog&Eagle/Eagle/Eagle.cs
>>>>>>> parent of b93abbce (lvl3)

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance){
            currentWaypoint++;
        }

        if (force.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);

        }
        else if (force.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
