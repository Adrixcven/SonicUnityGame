using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    
    public AudioSource gameOverSound;
    public float audioLength;
    /// <summary>
    /// Configura la configuración del cursor, establece la duración del audio y comienza la corrutina MainMenu.
    /// </summary>
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        audioLength = gameOverSound.clip.length;
        StartCoroutine(MainMenu());
    }
    /// <summary>
    /// Corrutina para manejar acciones después de un retardo especificado.
    /// Espera la duración del clip de audio y activa el botón de salida en la interfaz de usuario.
    /// </summary>
    /// <returns>Instrucción de yield para esperar la duración especificada.</returns>
    IEnumerator MainMenu()
    {
        yield return new WaitForSeconds(audioLength);
        HUDControl.instance.OnButtonExit();
    }
}
