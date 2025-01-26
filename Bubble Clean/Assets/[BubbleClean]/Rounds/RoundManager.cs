using System.Collections;
using TMPro;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    private static RoundManager _instance;
    [SerializeField] TextMeshProUGUI[] roundTxt;

    private int currentRound = 0;
    public int CurrentRound => currentRound;

    public delegate void RoundChanged(int newRound);
    public event RoundChanged OnRoundChanged;  // Evento para que otros scripts reaccionen al cambio de ronda
    [SerializeField] public int WaitTime = 3;
    [SerializeField] GameObject roundChangeAnimation;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip roundChangeSound;

    // Para cambiar la música a partir de la ronda 5
    [SerializeField] private AudioSource musicAudioSource; // Referencia al AudioSource que está reproduciendo la música
    [SerializeField] private AudioClip newMusicClip; // El nuevo clip de música para la ronda 5

    public static RoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<RoundManager>();
                if (_instance == null)
                {
                    Debug.LogWarning("RoundManager instance not found. Creating a new one.");
                    GameObject singleton = new GameObject(typeof(RoundManager).ToString());
                    _instance = singleton.AddComponent<RoundManager>();
                }
            }
            return _instance;
        }
    }


    private void Awake()
    {
        if (_instance == null)
        {
            Debug.Log("RoundManager instance created");
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject); // Destruir objeto duplicado
        }

        UpdateUI();
    }

    public void NextRound()
    {
        StartCoroutine(StartNextRound());
        currentRound++;
        UpdateUI();

        // Reproducir el sonido solo si no es la primera ronda
        if (currentRound > 1 && audioSource != null && roundChangeSound != null)
        {
            audioSource.PlayOneShot(roundChangeSound);
        }
        else if (currentRound == 1)
        {
            Debug.Log("Primera ronda iniciada, no reproducir sonido.");
        }

        // Cambiar música en la ronda 5
        if (currentRound == 5 && musicAudioSource != null && newMusicClip != null)
        {
            musicAudioSource.clip = newMusicClip;
            musicAudioSource.Play(); // Iniciar la nueva canción
            Debug.Log("Música cambiada a la ronda 5");
        }

        Debug.Log($"Round {currentRound}");
        if (OnRoundChanged != null)
        {
            OnRoundChanged.Invoke(currentRound); // Disparar el evento
        }
        else
        {
            Debug.LogWarning("No subscribers to OnRoundChanged event.");
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < roundTxt.Length; i++)
        {
            roundTxt[i].text = "Ronda " + currentRound;
        }
    }

    IEnumerator StartNextRound()
    {
        Debug.Log("Hola");
        roundTxt[0].enabled = false;
        roundChangeAnimation.SetActive(true);
        yield return new WaitForSeconds(2f);
        Debug.Log("2 seconds passed");
        roundChangeAnimation.SetActive(false);
        roundTxt[0].enabled = true;
    }
}
