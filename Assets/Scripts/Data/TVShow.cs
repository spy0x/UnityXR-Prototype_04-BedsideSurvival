using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "TVShow", menuName = "ScriptableObjects/TVShow", order = 1)]
public class TVShow : ScriptableObject
{
    [SerializeField] VideoClip showClip;
    [SerializeField] AudioClip showAudio;
    
    public VideoClip ShowClip => showClip;
    public AudioClip ShowAudio => showAudio;
}
