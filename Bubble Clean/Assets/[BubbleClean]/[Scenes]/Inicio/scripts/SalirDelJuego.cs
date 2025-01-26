using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalirDelJuego : MonoBehaviour
{
    // Este método debe ser público para ser accesible desde el Inspector
    public void Salir()
    {
        // Si estamos en el editor de Unity, dejamos de reproducir la escena.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // Si estamos fuera del editor, cerramos el juego.
            Application.Quit();
#endif
    }
}
