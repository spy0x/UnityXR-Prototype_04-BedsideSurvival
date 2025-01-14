using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AssistantDeviceState
{
    Idle,
    Listening,
    Thinking,
    Speaking
}
public class AssistantDevice : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] ledsRenderer;
    [SerializeField] [ColorUsage(true, true)] private Color assistantColorListening;
    [SerializeField] [ColorUsage(true, true)] private Color assistantColorThinking;
    [SerializeField] [ColorUsage(true, true)] private Color assistantColorSpeaking;
    [SerializeField] [ColorUsage(true, true)] private Color assistantColorIdle;
    [SerializeField] AudioSource audioSource;
    
    private static AssistantDevice instance;
    public static AssistantDevice Instance => instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        SetLedsEmissiveColor(AssistantDeviceState.Idle);
    }

    public void SetLedsEmissiveColor(AssistantDeviceState state)
    {
        Color color = assistantColorIdle;
        switch (state)
        {
            case AssistantDeviceState.Listening:
                color = assistantColorListening;
                break;
            case AssistantDeviceState.Thinking:
                color = assistantColorThinking;
                break;
            case AssistantDeviceState.Speaking:
                color = assistantColorSpeaking;
                break;
            case AssistantDeviceState.Idle:
                color = assistantColorIdle;
                break;
        }
        foreach (var ledRenderer in ledsRenderer)
        {
            ledRenderer.material.SetColor("_EmissionColor", color);
        }
    }
    public void PlayAudio(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
    public void GoToIdle()
    {
        SetLedsEmissiveColor(AssistantDeviceState.Idle);
    }
}
