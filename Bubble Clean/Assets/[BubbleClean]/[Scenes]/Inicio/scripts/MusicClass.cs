using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicClass : MonoBehaviour 
{
    private static MusicClass _instance;
    private AudioSource _audioSource;

    public AudioClip[] musicClips; 
    [SerializeField] private int IndiceMusica;

    private const string MusicPrefKey = "MusicPlaying";

    public static MusicClass Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MusicClass>();
                if (_instance == null)
                {
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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // Verificar si la música debe estar sonando o no
        bool isMusicPlaying = PlayerPrefs.GetInt(MusicPrefKey, 1) == 1;
        PlaySpecificMusic(IndiceMusica);
        
        if (!isMusicPlaying)
        {
            PauseMusic();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);

        // Verificar el estado guardado al cargar la escena
        bool isMusicPlaying = PlayerPrefs.GetInt(MusicPrefKey, 1) == 1;
        if (!isMusicPlaying)
        {
            PauseMusic();
        }
    }

    public void PlayMusicForScene(string sceneName)
    {
        if (sceneName == "Inicio")
        {
            PlaySpecificMusic(0);
        }
        else if (sceneName == "Juego")
        {
            PlaySpecificMusic(1);
        }
        else
        {
            PlaySpecificMusic(0);
        }
    }

    public void PlaySpecificMusic(int index)
    {
        if (index >= 0 && index < musicClips.Length)
        {
            _audioSource.clip = musicClips[index];
            _audioSource.Play();
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
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
