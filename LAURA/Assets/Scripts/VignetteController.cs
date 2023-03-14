using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteController : MonoBehaviour
{
    public float intensity = 0.5f;
    public float smoothness = 0.5f;

    private Volume volume;
    private Vignette vignette;

    void Start()
    {
        // change the intensity from a different script with:
        // myObject.GetComponent<VignetteEffect>().SetIntensity(0.8f);

        volume = gameObject.AddComponent<Volume>();
        volume.sharedProfile = ScriptableObject.CreateInstance<VolumeProfile>();
        vignette = volume.sharedProfile.Add<Vignette>();
        vignette.intensity.Override(intensity);
        vignette.smoothness.Override(smoothness);
    }

    void Update()
    {
        vignette.intensity.Override(intensity);
        vignette.smoothness.Override(smoothness);
    }

    public void SetIntensity(float newIntensity)
    {
        intensity = newIntensity;
    }

    public void SetSmoothness(float newSmoothness)
    {
        smoothness = newSmoothness;
    }
}