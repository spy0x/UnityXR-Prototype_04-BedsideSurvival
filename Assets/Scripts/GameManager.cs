using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] float maxSleepiness = 100;
    [SerializeField] private float sleepinessIncreaseRate = 2f;
    [SerializeField] private float lightsOnDecreaseRatio = 0.5f;
    [SerializeField] private float tvOnDecreaseRatio = 0.4f;
    [SerializeField] private float eyesClosedIncreaseRatio = 5f;
    [SerializeField] Slider sleepinessSlider;
    [SerializeField] private TextMeshPro timeDisplayText;
    [SerializeField] private float timeLeft = 180.0f;
    [SerializeField] private GameObject gameWinPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private VignetteController vignetteController;
    private static GameManager instance;
    public static GameManager Instance => instance;
    private float currentSleepiness = 0f;
    private bool isGameFinished = false;
    private bool isLightOn = true;
    private bool isTVOn = false;

    public bool IsTVOn
    {
        get => isTVOn;
        set => isTVOn = value;
    }

    public bool IsLightOn
    {
        get => isLightOn;
        set => isLightOn = value;
    }

    public bool IsGameFinished
    {
        get => isGameFinished;
        set => isGameFinished = value;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isGameFinished) return;
        SetTimeLeft();
        SetSleepinessBar();
    }

    private void SetSleepinessBar()
    {
        float decreaseRatio = 1f;

        if (isLightOn)
        {
            decreaseRatio *= lightsOnDecreaseRatio;
        }

        if (isTVOn)
        {
            decreaseRatio *= tvOnDecreaseRatio;
        }

        if (vignetteController.HasEyesClosed)
        {
            decreaseRatio *= eyesClosedIncreaseRatio;
        }

        currentSleepiness += sleepinessIncreaseRate * decreaseRatio * Time.deltaTime;
        currentSleepiness = Mathf.Clamp(currentSleepiness, 0, maxSleepiness);

        sleepinessSlider.value = currentSleepiness / maxSleepiness;
    }

    private void SetTimeLeft()
    {
        timeLeft -= Time.deltaTime;
        timeDisplayText.text = "Time Left: " + ((int)timeLeft / 60).ToString("00") + ":" +
                               ((int)timeLeft % 60).ToString("00");
        if (timeLeft <= 0)
        {
            isGameFinished = true;
            if (currentSleepiness >= maxSleepiness)
            {
                GameOver();
            }
            else
            {
                GameWin();
            }
        }
    }

    private void GameOver()
    {
        if (gameOverPanel)
        {
            gameOverPanel.SetActive(true);
        }
    }

    private void GameWin()
    {
        if (gameWinPanel)
        {
            gameWinPanel.SetActive(true);
        }
    }
}
