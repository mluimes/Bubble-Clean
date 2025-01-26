using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs; // Prefabs de los enemigos
    public Transform[] spawnPoints; // Puntos donde los enemigos pueden aparecer
    public int enemiesPerRound = 5; // Número de enemigos por ronda
    private int enemiesToSpawn = 0;
    private int enemiesAlive = 0;

    [Header("Enemy Probability")]
    [Range(0, 100)]
    public int type1Probability = 75; // Probabilidad de que aparezca el primer enemigo
    [Range(0, 100)]
    public int type2Probability = 25; // Probabilidad de que aparezca el segundo enemigo
    [Range(0, 100)]
    public int type3Probability = 5; // Probabilidad de que aparezca el tercer enemigo

    private void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("EnemySpawner instance created");
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Asegurarse de que solo hay una instancia
            return;
        }
    }

    private void OnEnable()
    {
        Debug.Log("EnemySpawner enabled");
        if (RoundManager.Instance != null)
        {
            Debug.Log("RoundManager instance found");
            RoundManager.Instance.OnRoundChanged += HandleRoundChanged; // Suscribir primero
            RoundManager.Instance.NextRound(); // Luego iniciar la primera ronda
        }
        else
        {
            Debug.LogWarning("RoundManager instance not found. Waiting for initialization.");
        }
    }


    private void OnDisable()
    {
        if (RoundManager.Instance != null)
        {
            RoundManager.Instance.OnRoundChanged -= HandleRoundChanged;
        }
    }


    private void HandleRoundChanged(int newRound)
    {
        StartCoroutine(WaitAndStartNextRound());
        Debug.Log("Starting round " + newRound);
        StartCoroutine(SpawnEnemies(newRound));
    }

    private IEnumerator SpawnEnemies(int round)
    {
        Debug.Log("Spawning enemies for round " + round);
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned in the Inspector.");
            yield break; // Stop spawning if no points are available
        }

        enemiesToSpawn = enemiesPerRound + (round - 1);
        enemiesAlive = enemiesToSpawn;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject enemyPrefab = SelectEnemyPrefab(round);
            if (enemyPrefab != null)
            {
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                enemy.GetComponent<Enemy>().OnDeath += OnEnemyDeath;
            }
            else
            {
                Debug.LogError("No enemy prefab selected!");
            }

            yield return new WaitForSeconds(1f);
        }
    }


    private GameObject SelectEnemyPrefab(int round)
    {
        // Determinar qué enemigo aparecerá basado en las probabilidades
        int randomValue = Random.Range(0, 100);

        // El tercer enemigo aparece desde la ronda 5
        if (round >= 5 && randomValue < type3Probability)
        {
            return enemyPrefabs[2];
        }
        // El segundo enemigo aparece desde la ronda 3
        else if (round >= 3 && randomValue < type2Probability + type1Probability)
        {
            return enemyPrefabs[1];
        }
        // El primer enemigo aparece desde la ronda 1
        else
        {
            return enemyPrefabs[0];
        }
    }

    private void OnEnemyDeath()
    {
        enemiesAlive--; // Reducimos el contador de enemigos vivos

        if (enemiesAlive <= 0)
        {
            // Todos los enemigos han muerto, pasamos a la siguiente ronda
            RoundManager.Instance.NextRound();
        }
    }

    private IEnumerator WaitAndStartNextRound() {
        yield return new WaitForSeconds(RoundManager.Instance.WaitTime);
    }
}
