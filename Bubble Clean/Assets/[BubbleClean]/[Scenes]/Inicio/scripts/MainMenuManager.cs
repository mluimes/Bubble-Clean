using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuManager : MonoBehaviour
{

    public void NewGame()
    {
        SceneManager.LoadScene("Juego"); 
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego..."); 
        Application.Quit(); 
    }
}

