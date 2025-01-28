using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour
{
    public Sprite onSprite; // Sprite para el estado "on"
    public Sprite offSprite; // Sprite para el estado "off"

    private Button button; // Referencia al componente Button
    private const string MusicPrefKey = "MusicPlaying";

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ToggleMusic);

        // Cargar el estado guardado o asignar por defecto
        bool savedState = PlayerPrefs.GetInt(MusicPrefKey, 1) == 1;
        SetMusicState(savedState);
    }

    void ToggleMusic()
    {
        bool currentState = PlayerPrefs.GetInt(MusicPrefKey, 1) == 1;
        bool newState = !currentState;

        SetMusicState(newState);

        // Guardar el nuevo estado
        PlayerPrefs.SetInt(MusicPrefKey, newState ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void SetMusicState(bool playMusic)
    {
        MusicClass musicObject = FindObjectOfType<MusicClass>();
        if (musicObject != null)
        {
            if (playMusic)
            {
                musicObject.ResumeMusic();
                button.image.sprite = onSprite;
            }
            else
            {
                musicObject.PauseMusic();
                button.image.sprite = offSprite;
            }
        }
    }
}
