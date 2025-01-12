using System;
using Oculus.Voice;
using TMPro;
using UnityEngine;

public class VoiceController : MonoBehaviour
{
    [SerializeField] AppVoiceExperience appVoiceExperience;
    [SerializeField] TextMeshPro textMeshPro;
    [SerializeField] private bool debug;

    bool isListening = false;


    private void Start()
    {
        appVoiceExperience.VoiceEvents.OnRequestInitialized.AddListener((msg) =>
        {
            isListening = true;
            Debug.Log("Request initialized");
        });
        appVoiceExperience.VoiceEvents.OnFullTranscription.AddListener((transcription) =>
        {
            if (debug) textMeshPro.text = transcription;
            Debug.Log("Full transcription: " + transcription);
        });
        appVoiceExperience.VoiceEvents.OnRequestCompleted.AddListener(() =>
        {
            isListening = false;
            Debug.Log("Request completed");
        });
    }

    //Called by building Meta SDKb lock Controller Input Map.
    public void OnButtonOneUp()
    {
        if (isListening) return;
        appVoiceExperience.Activate();
    }
}