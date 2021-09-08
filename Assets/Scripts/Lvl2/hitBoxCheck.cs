using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitBoxCheck : MonoBehaviour
{
    public float cameraShakeIntensity = 9f;
    public float cameraShakeTime = 3f;
    public GameObject fireballSystem;
    private Collider2D coll;
    private Enemy_behaviour enemyParent;
    [SerializeField] private LayerMask ground;

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

    /*private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("CaveGround"))
        {
            CinemachineShake.Instance.ShakeCamera(cameraShakeIntensity, cameraShakeTime);
            Instantiate(fireballSystem, fireballSystem.transform.position, fireballSystem.transform.rotation);
        }
    }*/

    private void Update()
    {
        if (coll.IsTouchingLayers(ground))
        {
            CinemachineShake.Instance.ShakeCamera(cameraShakeIntensity, cameraShakeTime);

          
        }
    }


}
