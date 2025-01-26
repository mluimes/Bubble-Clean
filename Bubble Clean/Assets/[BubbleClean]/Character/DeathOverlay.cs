using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathOverlay : MonoBehaviour
{
    [Header("Overlay Settings")]
    [SerializeField] private Image overlayImage; // Imagen del overlay
    [SerializeField] private float fadeToRedDuration = 2f; // Tiempo para pasar de transparente a rojo
    [SerializeField] private float fadeToBlackDuration = 2f; // Tiempo para pasar de rojo a negro

    [SerializeField] GameObject gameOverScreen;

    private Color transparent = new Color(0, 0, 0, 0);
    private Color red = new Color(1, 0, 0, 0.5f); // Rojo con transparencia
    private Color black = Color.black;

    public void StartDeathOverlay()
    {
        Debug.Log("LLEGAMOS");
        overlayImage.gameObject.SetActive(true);
        StartCoroutine(PlayDeathOverlay());
    }

    private IEnumerator PlayDeathOverlay()
    {
        // Fase 1: De transparente a rojo
        float elapsedTime = 0f;
        while (elapsedTime < fadeToRedDuration)
        {
            overlayImage.color = Color.Lerp(transparent, red, elapsedTime / fadeToRedDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        overlayImage.color = red;

        // Fase 2: De rojo a negro
        elapsedTime = 0f;
        while (elapsedTime < fadeToBlackDuration)
        {
            overlayImage.color = Color.Lerp(red, black, elapsedTime / fadeToBlackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        overlayImage.color = black;

        gameOverScreen.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnDestroy() {
        overlayImage.gameObject.SetActive(false);
    }
}
