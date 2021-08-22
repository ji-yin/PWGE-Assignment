using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPause = false;

    public GameObject pauseMenuUI;

    [SerializeField] private GameObject WeatherManager;
    public TMPro.TMP_Dropdown myDrop;
    public Light2D sunLight;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPause = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void Option()
    {
        if (myDrop.value == 1)
        {
            WeatherManager.transform.Find("Snow").gameObject.SetActive(false);
            WeatherManager.transform.Find("Rain").gameObject.SetActive(true);
        }
        else if (myDrop.value == 2)
        {
            WeatherManager.transform.Find("Rain").gameObject.SetActive(false);
            WeatherManager.transform.Find("Snow").gameObject.SetActive(true);
        }
        else
        {
            WeatherManager.transform.Find("Rain").gameObject.SetActive(false);
            WeatherManager.transform.Find("Snow").gameObject.SetActive(false);
        }
    }
}
