using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [Header("Flicker Settings")]
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 0.1f;

    [Header("Flicker Randomness")]
    public bool randomize = true;
    public float minFlickerDelay = 0.05f;
    public float maxFlickerDelay = 0.2f;

    private Light flickerLight;
    private float defaultIntensity;


    private void Start()
    {
        flickerLight = GetComponent<Light>();
        defaultIntensity = flickerLight.intensity;
        StartCoroutine(Flicker());
    }

    private System.Collections.IEnumerator Flicker()
    {
        while (true)
        {
            float newIntensity = Random.Range(minIntensity, maxIntensity);
            flickerLight.intensity = newIntensity;

            float waitTime = randomize 
                ? Random.Range(minFlickerDelay, maxFlickerDelay) 
                : flickerSpeed;

            yield return new WaitForSeconds(waitTime);
        }
    }

    public void ResetIntensity()
    {
        flickerLight.intensity = defaultIntensity;
    }
}
