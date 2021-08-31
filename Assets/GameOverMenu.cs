using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public TextMeshProUGUI gemsText;

    public void Setup(string score)
    {
        gameObject.SetActive(true);
        gemsText.text = score + " GEMS";
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Lvl2");
    }

    public void GameOver()
    {
        this.enabled = true;
    }
}
