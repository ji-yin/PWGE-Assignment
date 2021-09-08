using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnFallingGrenade : MonoBehaviour
{
    public Collider2D spawnAreaCollider;
    public int spawnCount = 5;
    public float spawnInterval = 0.3f;
    public GameObject grenade;
    private float intTimer;
    private int intSpawn;

    // Start is called before the first frame update
    void Start()
    {
        intTimer = spawnInterval;
        intSpawn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (intTimer <= 0)
        {
            intTimer = 0;
        }
        else
        {
            intTimer -= Time.deltaTime;
        }
        while(intTimer == 0 && intSpawn<spawnCount)
        {
            SpawnRock();
            intSpawn++;
            
            
        }
        
    }
    private void SpawnRock()
    {
        var randomX = Random.Range(spawnAreaCollider.bounds.min.x, spawnAreaCollider.bounds.max.x);
        Object.Instantiate(grenade, new Vector3(randomX, spawnAreaCollider.bounds.min.y), Quaternion.identity);
    }
}
