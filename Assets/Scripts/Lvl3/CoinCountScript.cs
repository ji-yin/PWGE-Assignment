using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCountScript : MonoBehaviour
{
    public static float coinAmount;
    public PlayerCollision collision;
    public float defaultHealth;

    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        text.text = coinAmount.ToString();

        if (coinAmount >= 10)
        {
            collision.healthAmount = defaultHealth;
            coinAmount -= 10;
        }
    }
}
