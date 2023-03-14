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
        volume = gameObject.AddComponent<Volume>();
        volume.sharedProfile = new VolumeProfile();
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