using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour
{
    private Button button;
    private Color originalColor;
    public Color hoverColor = Color.green;  
    public Color clickColor = Color.blue;   

    public AudioClip hoverSound;            
    public AudioClip clickSound;            
    private AudioSource audioSource;

    void Start()
    {

        audioSource = GetComponent<AudioSource>();

        button = GetComponent<Button>();
        originalColor = button.image.color; 

        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void  PlayHover()
    {
        audioSource.PlayOneShot(hoverSound);

    }

    public void PlayClick()
    {
        audioSource.PlayOneShot(clickSound);
    }

    /*
    public void OnPointerEnter()
    {
        button.image.color = hoverColor;

        
        if (hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    
    public void OnPointerExit()
    {
        button.image.color = originalColor;
    }

    
    public void OnPointerClick()
    {
        button.image.color = clickColor;

        
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    
     */
}
