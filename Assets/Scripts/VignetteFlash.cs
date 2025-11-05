using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteFlash : MonoBehaviour
{
    private Volume volume;
    private Vignette vignette;

    [Header("Flash Settings")]
    public float flashDuration = 0.4f;
    public float maxIntensity = 0.45f;
    public Color goodColor = Color.green;
    public Color badColor = Color.red;

    private float timer;
    private bool isFlashing = false;
    private Color targetColor;

    void Start()
    {
        volume = GetComponent<Volume>();

        if (volume.profile.TryGet(out vignette))
        {
            vignette.intensity.value = 0f;
            vignette.active = true;
        }
        else
        {
            Debug.LogError("No Vignette override found in the Volume!");
        }
    }

    void Update()
    {
        if (!isFlashing || vignette == null) return;

        timer += Time.deltaTime;
        float t = timer / flashDuration;

        
        vignette.intensity.value = Mathf.Lerp(maxIntensity, 0f, t);

        
        if (t >= 1f)
        {
            vignette.intensity.value = 0f;
            isFlashing = false;
        }
    }

    public void Flash(Color color)
    {
        if (vignette == null) return;

        Debug.Log("Triggered Flash");
        targetColor = color;
        vignette.color.value = targetColor;
        vignette.intensity.value = maxIntensity;

        timer = 0f;
        isFlashing = true;
    }

    // helpers
    public void FlashGood() => Flash(goodColor);
    public void FlashBad() => Flash(badColor);
}
