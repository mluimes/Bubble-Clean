using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public Slider sensitivitySlider;
    public TMP_Text sensitivityValueText;

    public Slider volumeSlider;
    public TMP_Text volumeValueText;

    public TMP_Dropdown qualityDropdown;
    //public TMP_Dropdown fpsDropdown;  // Añadido para la selección de FPS

    private float defaultSensitivity = 400f;

    private Character character;

    private void Start()
    {
        character = FindObjectOfType<Character>();
        
        // Cargar configuración guardada al iniciar
        LoadSettings();

        // Suscribirse a los eventos de cambio
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
        //fpsDropdown.onValueChanged.AddListener(OnFPSChanged);  // Añadido para FPS
    }

    public void LoadSettings()
    {
        // Cargar sensibilidad guardada
        float savedSensitivity = PlayerPrefs.GetFloat("sensitivity", defaultSensitivity);
        sensitivitySlider.value = savedSensitivity;
        OnSensitivityChanged(savedSensitivity);

        // Actualiza la sensibilidad del personaje
        if (character != null)
        {
            character.MouseSensitivity = savedSensitivity;
        }

        // Cargar volumen guardado
        float savedVolume = PlayerPrefs.GetFloat("volume", 0.5f);
        volumeSlider.value = savedVolume;
        OnVolumeChanged(savedVolume);

        // Cargar calidad gráfica guardada
        int savedQualityIndex = PlayerPrefs.GetInt("quality", QualitySettings.GetQualityLevel());
        qualityDropdown.value = savedQualityIndex;
        OnQualityChanged(savedQualityIndex);

        // Cargar FPS guardado
        // int savedFPSIndex = PlayerPrefs.GetInt("fps", 1);  // Por defecto 60 FPS (índice 1)
        // fpsDropdown.value = savedFPSIndex;
        // OnFPSChanged(savedFPSIndex);
    }

    public void OnSensitivityChanged(float value)
    {
        sensitivityValueText.text = "Sensibilidad: " + value.ToString("F0");
        PlayerPrefs.SetFloat("sensitivity", value);

        // Actualiza la sensibilidad del personaje
        if (character != null)
        {
            character.MouseSensitivity = value;
        }
    }

    public void OnVolumeChanged(float value)
    {
        volumeValueText.text = "Volumen General: " + value.ToString("F2");
        PlayerPrefs.SetFloat("volume", value);
        AudioListener.volume = value; // Aplicar volumen inmediatamente
    }

    public void OnQualityChanged(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("quality", index);
    }

    // public void OnFPSChanged(int index)
    // {
    //     int[] fpsValues = { 30, 60, 90, 120, 144, 0 };  // Opciones de FPS, 0 significa sin límite
    //     int fps = fpsValues[index];
    //     Application.targetFrameRate = fps;
    //     PlayerPrefs.SetInt("fps", index);
    //     Debug.Log("FPS Changed to: " + fps);  // Agregar depuración
    // }

    private void OnDestroy()
    {
        PlayerPrefs.Save(); // Guardar todos los cambios al salir
    }
}
