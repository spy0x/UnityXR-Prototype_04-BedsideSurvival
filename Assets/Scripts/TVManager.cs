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
    public static TVManager Instance => instance;

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
        int randomIndex = Random.Range(0, tvShows.Count);
        PlayShow(randomIndex);
    }

    private void PlayShow(int randomIndex)
    {
        tvScreen.SetActive(true);
        videoPlayer.clip = tvShows[randomIndex].ShowClip;
        videoPlayer.Play();
        // audioSource.clip = tvShows[randomIndex].ShowAudio;
        // audioSource.Play();
    }
    public void StopShow()
    {
        tvScreen.SetActive(false);
        videoPlayer.Stop();
        // audioSource.Stop();
    }
}
