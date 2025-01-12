using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VignetteController : MonoBehaviour
{
    [SerializeField] OVRVignette vignette;
    [SerializeField] private float vignetteFullFallOff = 0.5f;
    private float originalVignetteFallOff;
    private bool hasEyesClosed = false;
    public bool HasEyesClosed => hasEyesClosed;

    private void Start()
    {
        originalVignetteFallOff = vignette.VignetteFalloffDegrees;
    }

    void Update()
    {
        // Get the current value of the right trigger
        float rightTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        // Get the current value of the left trigger
        float leftTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
        SetVignette(rightTriggerValue, leftTriggerValue);
    }

    private void SetVignette(float rightTriggerValue, float leftTriggerValue)
    {
        float targetFallOff;

        if (Mathf.Approximately(rightTriggerValue, 1.0f) && Mathf.Approximately(leftTriggerValue, 1.0f))
        {
            targetFallOff = vignetteFullFallOff;
        }
        else if (rightTriggerValue > 0.0f || leftTriggerValue > 0.0f)
        {
            float averageTriggerValue = (rightTriggerValue + leftTriggerValue) / 2.0f;
            targetFallOff = Mathf.Lerp(originalVignetteFallOff, vignetteFullFallOff, averageTriggerValue);
        }
        else
        {
            targetFallOff = originalVignetteFallOff;
        }

        vignette.VignetteFalloffDegrees = targetFallOff;
        hasEyesClosed = Mathf.Approximately(targetFallOff, vignetteFullFallOff);
    }
}