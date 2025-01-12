using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] Light ceilingLight;
    [SerializeField] private Renderer ceilingLightRenderer;

    private static LightController instance;
    private float originalLightIntensity;
    private Color originalEmissionColor;

    public static LightController Instance
    {
        get { return instance; }
    }

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
        originalLightIntensity = ceilingLight.intensity;
        originalEmissionColor = ceilingLightRenderer.material.GetColor("_EmissionColor");
    }

    public void SetLight(bool state)
    {
        ceilingLight.intensity = state ? originalLightIntensity : 0;
        Color emissionColor = state ? originalEmissionColor : Color.black;
        ceilingLightRenderer.material.SetColor("_EmissionColor", emissionColor);
    }
}