using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitBoxCheck : MonoBehaviour
{
    [NamedArrayAttribute(new string[] { "shakeIntensity", "shakeTime"})]
    [SerializeField] private float[] camVar;
    [SerializeField] private GameObject fireballSystem;
    [SerializeField] private LayerMask ground;
    private Collider2D coll;

    void Start()
    {
        coll = GetComponent<Collider2D>();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
        
            Instantiate(fireballSystem, fireballSystem.transform.position, fireballSystem.transform.rotation);
        }


    }

    private void Update()
    {
        if (coll.IsTouchingLayers(ground))
        {
            CinemachineShake.Instance.ShakeCamera(camVar[0], camVar[1]);

          
        }
    }


}
