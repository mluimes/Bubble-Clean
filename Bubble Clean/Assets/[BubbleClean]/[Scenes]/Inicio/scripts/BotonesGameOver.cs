using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonesGameOver : MonoBehaviour
{
    public void VolverInicio()
    {
        SceneManager.LoadScene("Inicio");
    }

    public void Reintentar() {
        SceneManager.LoadScene("Juego");
    }
}
