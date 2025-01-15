using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class TVManager : MonoBehaviour
{
    [SerializeField] private List<TVShow> tvShows = new List<TVShow>();
    [SerializeField] private GameObject tvScreen;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private AudioSource audioSource;
    
    private static TVManager instance;
    private TVShow currentShow;
    public static TVManager Instance => instance;

    private void OnEnable()
    {
        GameManager.OnGameFinished += StopShow;
        // EnemyManager.OnEnemyAttack += StopShow;
    }

    private void OnDisable()
    {
        GameManager.OnGameFinished -= StopShow;
        // EnemyManager.OnEnemyAttack -= StopShow;
    }

    private void Start()
    {
        tvScreen.SetActive(false);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayRandomShow()
    {
        TVShow randomShow;
        do
        {
            int randomIndex = Random.Range(0, tvShows.Count);
            randomShow = tvShows[randomIndex];
        } while (randomShow == currentShow);
        PlayShow(randomShow);
    }

    private void PlayShow(TVShow show)
    {
        currentShow = show;
        tvScreen.SetActive(true);
        videoPlayer.Stop();
        videoPlayer.clip = show.ShowClip;
        videoPlayer.Play();
        videoPlayer.isLooping = true;
        // audioSource.clip = tvShows[randomIndex].ShowAudio;
        // audioSource.Play();
    }
    public void StopShow()
    {
        tvScreen.SetActive(false);
        videoPlayer.Stop();
        // audioSource.Stop();
    }
    public void ChangeChannel()
    {
        if (tvScreen.activeSelf == false) return;
        int currentShowIndex = tvShows.IndexOf(currentShow);
        int nextShowIndex = (currentShowIndex + 1) % tvShows.Count;
        PlayShow(tvShows[nextShowIndex]);
    }
}
