using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    private static RoundManager _instance;

    private int currentRound = 1;
    public int CurrentRound => currentRound;

    public static RoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<RoundManager>();
                if (_instance == null)
                {
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
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void NextRound()
    {
        currentRound++;
        Debug.Log($"Round {currentRound}");
    }
}
