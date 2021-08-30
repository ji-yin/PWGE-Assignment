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
    public TMPro.TMP_Dropdown myWeather;
    public TMPro.TMP_Dropdown mySeason;

    public Light2D sunLight;
    private float defaultLightIntensity;
    public float summerLightIntensity;
    public float autumnLightIntensity;
    public float winterLightIntensity;

    private Color defaultLightColor;
    public Color summerColor;
    public Color autumnColor;
    public Color winterColor;

    private void Start()
    {
        this.defaultLightColor = this.sunLight.color;
        this.defaultLightIntensity = this.sunLight.intensity;
    }

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

    public void ChangeWeather()
    {
        if (myWeather.value == 1)
        {
            WeatherManager.transform.Find("Snow").gameObject.SetActive(false);
            WeatherManager.transform.Find("Rain").gameObject.SetActive(true);
        }
        else if (myWeather.value == 2)
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

    public void ChangeSeason()
    {
        switch (mySeason.value)
        {
            case 0:
                LerpSunIntensity(this.sunLight, defaultLightIntensity);
                LerpLightColor(this.sunLight, defaultLightColor);
                break;
            case 1:
                LerpSunIntensity(this.sunLight, summerLightIntensity);
                LerpLightColor(this.sunLight, summerColor);
                break;
            case 2:
                LerpSunIntensity(this.sunLight, autumnLightIntensity);
                LerpLightColor(this.sunLight, autumnColor);
                break;
            case 3:
                LerpSunIntensity(this.sunLight, winterLightIntensity);
                LerpLightColor(this.sunLight, winterColor);
                break;

        }
    }

    private void LerpLightColor(Light2D light, Color c)
    {
        light.color = Color.Lerp(light.color, c, Mathf.PingPong(Time.time, 1));
    }

    private void LerpSunIntensity(Light2D light, float intensity)
    {
        light.intensity = Mathf.Lerp(light.intensity, intensity, Mathf.PingPong(Time.time, 1));
    }


}
