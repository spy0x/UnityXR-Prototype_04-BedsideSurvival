using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] float maxSleepiness = 100;
    [SerializeField] private float sleepinessIncreaseRate = 2f;
    [SerializeField] private float lightsOnDecreaseRatio = 0.5f;
    [SerializeField] private float tvOnDecreaseRatio = 0.4f;
    [SerializeField] private float eyesClosedIncreaseRatio = 5f;
    [SerializeField] private float timeLeft = 180.0f;
    
    [Header("Components")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] Slider sleepinessSlider;
    [SerializeField] private TextMeshPro timeDisplayText;
    [SerializeField] private GameObject gameWinPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject sleepPanel;
    [SerializeField] private VignetteController vignetteController;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftHand;

    [Header("Jump Scare")] 
    [SerializeField] private GameObject enemyJumpScare;
    [SerializeField] private AudioClip jumpScareSound;
    [SerializeField] private float delayBeforeRestart = 3f;
    
    [Header("Sleep")] 
    [SerializeField] private AudioClip sleepSound;
    [SerializeField] private float delayBeforeGameOver = 3f;

    public static event Action OnGameFinished;
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
        if (currentSleepiness >= maxSleepiness)
        {
            isGameFinished = true;
            OnGameFinished?.Invoke();
            StartCoroutine(Sleep());
        }
    }

    private void SetTimeLeft()
    {
        timeLeft -= Time.deltaTime;
        timeDisplayText.text = "Time Left: " + ((int)timeLeft / 60).ToString("00") + ":" +
                               ((int)timeLeft % 60).ToString("00");
        if (timeLeft <= 0)
        {
            OnGameFinished?.Invoke();
            isGameFinished = true;
            if (currentSleepiness >= maxSleepiness)
            {
                StartCoroutine(GameOver());
            }
            else
            {
                GameWin();
            }
        }
    }

    private IEnumerator GameOver()
    {
        if (gameOverPanel)
        {
            DisablePlayer();
            gameOverPanel.SetActive(true);
            audioSource.PlayOneShot(jumpScareSound);
            yield return new WaitForSeconds(jumpScareSound.length);
            enemyJumpScare.SetActive(false);
            yield return new WaitForSeconds(delayBeforeRestart);
            // RestartScene();
        }
    }
    
    private IEnumerator Sleep()
    {
        if (sleepPanel)
        {
            sleepPanel.SetActive(true);
            audioSource.PlayOneShot(sleepSound);
            yield return new WaitForSeconds(sleepSound.length + delayBeforeGameOver);
            sleepPanel.SetActive(false);
            StartCoroutine(GameOver());
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GameWin()
    {
        if (gameWinPanel)
        {
            gameWinPanel.SetActive(true);
            // RestartScene();
        }
    }

    private void DisablePlayer()
    {
        leftHand.SetActive(false);
        rightHand.SetActive(false);
    }
}