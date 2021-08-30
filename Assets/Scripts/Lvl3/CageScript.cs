using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageScript : MonoBehaviour
{
    public Transform clearStage;

    private bool hasKey;

    // Update is called once per frame
    void Update()
    {
        if (KeyCountScript.keyAmount == 1)
        {
            hasKey = true;
        }
        else
        {
            hasKey = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && hasKey)
        {
            KeyCountScript.keyAmount -= 1;
            Destroy(gameObject);
            clearStage.gameObject.SetActive(true);
        }
    }
}
