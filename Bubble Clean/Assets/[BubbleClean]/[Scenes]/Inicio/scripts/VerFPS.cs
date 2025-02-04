using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VerFPS : MonoBehaviour
{
    float deltaTime = 0.0f;
    GUIStyle style;
    bool showFPS = true; // Variable para controlar la visibilidad de los FPS

    void Start()
    {
        Application.targetFrameRate = 120; 
        style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = 40;
        style.normal.textColor = Color.white;
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Detectar la pulsación de la tecla Enter o el botón de TextMeshPro
        if (Input.GetKeyDown(KeyCode.Return))
        {
            showFPS = !showFPS; // Cambiar el estado de visibilidad
        }
    }

    void OnGUI()
    {
        if (showFPS)
        {
            int fps = Mathf.RoundToInt(1.0f / deltaTime);
            string text = $"FPS: {fps}";

            // Mostrar el texto en la esquina superior izquierda
            GUI.Label(new Rect(10, 10, 100, 20), text, style);
        }
    }
}
