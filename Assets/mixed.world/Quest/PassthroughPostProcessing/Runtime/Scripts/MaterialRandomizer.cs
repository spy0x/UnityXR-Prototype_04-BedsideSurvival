using UnityEngine;
using System.Collections;

public class MaterialRandomizer : MonoBehaviour
{
    [Header("Material Settings")]
    public Material targetMaterial; // Assign the URP material here
    public float minEmissionStrength = 0.5f;
    public float maxEmissionStrength = 2.0f;
    public float lerpDuration = 1f; // Duration of the lerp in seconds

    private static readonly int ColorProperty = Shader.PropertyToID("_BaseColor"); // URP base color property
    private static readonly int EmissionColorProperty = Shader.PropertyToID("_EmissionColor"); // URP emission color property

    private Coroutine colorLerpCoroutine;

    // Function to randomize the color and emission strength
    [ContextMenu("Randomize Color")]
    public void RandomizeColor()
    {
        if (targetMaterial == null)
        {
            Debug.LogError("Target material is not assigned!");
            return;
        }

        // Generate a random color
        Color randomColor = new Color(Random.value, Random.value, Random.value);

        // Generate a random emission strength
        float randomEmissionStrength = Random.Range(minEmissionStrength, maxEmissionStrength);

        // Calculate the target emission color
        Color targetEmissionColor = randomColor * randomEmissionStrength;

        // Stop any ongoing color lerp
        if (colorLerpCoroutine != null)
        {
            StopCoroutine(colorLerpCoroutine);
        }

        // Start the color lerp coroutine
        colorLerpCoroutine = StartCoroutine(LerpColor(randomColor, targetEmissionColor));

        Debug.Log($"Material updating to Color: {randomColor}, Emission Strength: {randomEmissionStrength}");
    }

    private IEnumerator LerpColor(Color targetColor, Color targetEmissionColor)
    {
        Color startColor = targetMaterial.GetColor(ColorProperty);
        Color startEmissionColor = targetMaterial.GetColor(EmissionColorProperty);
        float elapsedTime = 0f;

        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / lerpDuration;

            // Lerp the base color
            Color lerpedColor = Color.Lerp(startColor, targetColor, t);
            targetMaterial.SetColor(ColorProperty, lerpedColor);

            // Lerp the emission color
            Color lerpedEmissionColor = Color.Lerp(startEmissionColor, targetEmissionColor, t);
            targetMaterial.SetColor(EmissionColorProperty, lerpedEmissionColor);

            // Ensure the material's emission is enabled
            targetMaterial.EnableKeyword("_EMISSION");

            yield return null;
        }

        // Ensure we set the final values
        targetMaterial.SetColor(ColorProperty, targetColor);
        targetMaterial.SetColor(EmissionColorProperty, targetEmissionColor);

        Debug.Log("Color lerp completed");
    }
}
