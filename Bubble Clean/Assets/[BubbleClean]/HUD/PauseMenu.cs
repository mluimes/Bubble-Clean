using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Asigna el panel del menú en el inspector
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Oculta el menú
        Time.timeScale = 1f; // Reanuda el tiempo del juego
        Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor
        Cursor.visible = false;
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // Muestra el menú
        Time.timeScale = 0f; // Pausa el tiempo del juego
        Cursor.lockState = CursorLockMode.None; // Desbloquea el cursor
        Cursor.visible = true;
        isPaused = true;
    }

    public void QuitGame()
    {
        Application.Quit(); // Cierra la aplicación
    }
}
