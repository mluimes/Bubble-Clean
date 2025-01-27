using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicClass : MonoBehaviour 
{
    private static MusicClass _instance;

    private AudioSource _audioSource;
    public AudioClip[] musicClips; // Array para almacenar las canciones

    [SerializeField] private int IndiceMusica;

    // Singleton para asegurarse de que solo haya una instancia
    public static MusicClass Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MusicClass>();
                if (_instance == null)
                {
                    Debug.LogWarning("MusicClass instance not found, creating one.");
                    GameObject singleton = new GameObject("MusicClass");
                    _instance = singleton.AddComponent<MusicClass>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }

    private void Awake()     
    {
        // Asegurarse de que no haya duplicados
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }

        _audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded; // Detecta cuando se carga una nueva escena
    }

    private void Start()
    {
        PlaySpecificMusic(IndiceMusica);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cambiar la música según la escena
        PlayMusicForScene(scene.name);
    }

    // Cambiar música dependiendo de la escena
    public void PlayMusicForScene(string sceneName)
    {
        if (sceneName == "Inicio")  // Si la escena es "Inicio"
        {
            PlaySpecificMusic(0);  // Reproduce la primera canción
        }
        else if (sceneName == "Juego")  // Si la escena es "Juego"
        {
            PlaySpecificMusic(1);  // Reproduce la segunda canción
        }
        else
        {
            // Si no está especificada, puedes elegir música por defecto
            PlaySpecificMusic(0);  // O cualquier otra canción por defecto
        }
    }

    // Reproducir música específica desde el array (pasando un índice)
    public void PlaySpecificMusic(int index)
    {
        if (index >= 0 && index < musicClips.Length)
        {
            _audioSource.clip = musicClips[index];
            Debug.Log("Playing specific clip: " + musicClips[index].name); // Log de depuración
            _audioSource.Play(); // Reproduce el AudioClip específico
        }
        else
        {
            Debug.LogWarning("Índice fuera de rango para la lista de música.");
        }
    }

    public void StopMusic()     
    {
        _audioSource.Stop();
    }

    public void PauseMusic()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Pause();
        }
    }

    public void ResumeMusic()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.UnPause();
        }
    }

    private void OnDestroy()
    {
        // Desuscribirse del evento al destruir el objeto
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
