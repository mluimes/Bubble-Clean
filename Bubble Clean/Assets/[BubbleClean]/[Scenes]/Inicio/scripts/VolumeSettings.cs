using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSettings : MonoBehaviour
{
    public Slider volumeSlider;           // Slider para ajustar el volumen
    public TMP_Text volumeValueText;      // Texto que muestra el valor del volumen

    private void Start()
    {
        // Cargar configuración guardada al iniciar
        LoadVolumeSettings();

        // Suscribirse al evento de cambio
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    public void LoadVolumeSettings()
    {
        // Cargar volumen guardado
        float savedVolume = PlayerPrefs.GetFloat("volume", 0.5f);
        volumeSlider.value = savedVolume;
        OnVolumeChanged(savedVolume);
    }

    public void OnVolumeChanged(float value)
    {
        volumeValueText.text = "Volumen General: " + value.ToString("F2");
        PlayerPrefs.SetFloat("volume", value);  // Guardar el volumen
        AudioListener.volume = value;            // Aplicar volumen inmediatamente
    }

    private void OnDestroy()
    {
        PlayerPrefs.Save(); // Guardar todos los cambios al salir
    }
}
