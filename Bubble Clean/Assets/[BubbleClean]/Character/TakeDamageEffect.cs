using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TakeDamageEffect : MonoBehaviour
{
    public float intensity = 0;
    PostProcessVolume volume;
    Vignette vignette;

    void Start()
    {
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out vignette);

        if (!vignette)
        {
            Debug.LogError("No se pudo encontrar el efecto de viñeta en el volumen de postprocesamiento.");
        }
        else
        {
            vignette.enabled.Override(false);
        }

    }

    public IEnumerator TakeDamageCoroutine()
    {
        intensity = 0.4f;

        vignette.enabled.Override(true);
        vignette.intensity.Override(intensity);

        yield return new WaitForSeconds(0.4f);

        while (intensity > 0)
        {
            intensity -= 0.1f;

            if (intensity < 0)
            {
                intensity = 0;
            }

            vignette.intensity.Override(intensity);

            yield return new WaitForSeconds(0.1f);
        }

        vignette.enabled.Override(false);
        yield break;
    }
}
