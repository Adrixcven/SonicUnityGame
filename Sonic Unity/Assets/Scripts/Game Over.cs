using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    
    public AudioSource gameOverSound;
    public float audioLength;
    /// <summary>
    /// Configures cursor settings, sets audio length, and starts the MainMenu coroutine.
    /// </summary>
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        audioLength = gameOverSound.clip.length;
        StartCoroutine(MainMenu());

    }
    /// <summary>
    /// Coroutine for handling actions after a specified delay.
    /// Waits for the duration of the audio clip and triggers the exit button in the HUD.
    /// </summary>
    /// <returns>Yield instruction to wait for the specified duration.</returns>
    IEnumerator MainMenu()
    {
        yield return new WaitForSeconds(audioLength);
        HUDControl.instance.OnButtonExit();
    }
    
   
}
