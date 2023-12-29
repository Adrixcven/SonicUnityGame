using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    
    public TMPro.TextMeshProUGUI finalPointsText;
    public AudioSource victorySound;
    public float audioLength;
    /// <summary>
    /// Configura la configuración del cursor, muestra el cursor y actualiza la pantalla de victoria.
    /// </summary>
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UpdateVictoryScreenPoints();
        audioLength = victorySound.clip.length;
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
    /// <summary>
    /// Actualiza el texto en pantalla con los puntos finales del juego.
    /// </summary>
     public void UpdateVictoryScreenPoints()
    {
        finalPointsText.text = GameController.instance.score.ToString();
    }
}
