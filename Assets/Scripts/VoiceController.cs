using System;
using Oculus.Voice;
using TMPro;
using UnityEngine;

public class VoiceController : MonoBehaviour
{
    [SerializeField] AppVoiceExperience appVoiceExperience;
    [SerializeField] TextMeshPro textMeshPro;
    [SerializeField] private bool debug;
    [SerializeField] OVRPassthroughLayer passthroughLayer;

    bool isListening = false;
    private bool canUseVoice = true;
    private float originalPassthroughBrightness;
    private static VoiceController instance;
    public static VoiceController Instance => instance;
    public bool CanUseVoice {get => canUseVoice; set => canUseVoice = value;}

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
        if (!debug) textMeshPro.gameObject.SetActive(false);
        originalPassthroughBrightness = passthroughLayer.colorMapEditorBrightness;
        appVoiceExperience.VoiceEvents.OnRequestInitialized.AddListener((msg) =>
        {
            isListening = true;
            AssistantDevice.Instance.SetLedsEmissiveColor(AssistantDeviceState.Listening);
            Debug.Log("Request initialized");
        });
        appVoiceExperience.VoiceEvents.OnFullTranscription.AddListener((transcription) =>
        {
            if (debug) textMeshPro.text = transcription;
            AssistantDevice.Instance.SetLedsEmissiveColor(AssistantDeviceState.Speaking);
            Debug.Log("Full transcription: " + transcription);
        });
        appVoiceExperience.VoiceEvents.OnRequestCompleted.AddListener(() =>
        {
            isListening = false;
            AssistantDevice.Instance.Invoke(nameof(AssistantDevice.Instance.GoToIdle), 1.5f);
            Debug.Log("Request completed");
        });
        appVoiceExperience.VoiceEvents.OnPartialResponse.AddListener((response) =>
        {
            // AssistantDevice.Instance.SetLedsEmissiveColor(AssistantDeviceState.Thinking);
            Debug.Log("Partial response: " + response);
        });
    }

    //Called by building Meta SDKb lock Controller Input Map.
    public void OnButtonOneUp()
    {
        if (isListening || !canUseVoice) return;
        appVoiceExperience.Activate();
    }

    //Called by Meta Voice SDK intent "turn_device"
    public void TurnDevice(String[] result)
    {
        Debug.Log("Turn Device: " + result[0] + " " + result[1]);
        if (result[0] == "light")
        {
            if (result[1] == "on")
            {
                LightController.Instance.SetLight(true);
                passthroughLayer.colorMapEditorBrightness = originalPassthroughBrightness;
            }
            else if (result[1] == "off")
            {
                LightController.Instance.SetLight(false);
                passthroughLayer.colorMapEditorBrightness = -0.8f;
            }
        }
    }
}