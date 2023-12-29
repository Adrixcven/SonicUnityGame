using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalPlate : MonoBehaviour
{

    public float audioLength;
    public int nextLevel;
    public AudioSource stageClearSound;
    public AudioSource musicSound;

    /// <summary>
    /// Maneja la lógica cuando un objeto entra en el área del objetivo.
    /// Detiene el temporizador, detiene la música, congela al jugador, reproduce el sonido de nivel completado,
    /// calcula y muestra la bonificación, actualiza la interfaz de usuario y avanza al siguiente nivel.
    /// </summary>
    /// <param name="other">Collider del objeto que entra en el área.</param>
    private void OnTriggerEnter(Collider other)
    {
        audioLength = stageClearSound.clip.length;
        GameObject enteringObject = other.transform.parent.gameObject;
        if (enteringObject.CompareTag("Player"))
        {
            GameController.instance.isTimerRunning = false;
            musicSound.Stop();
            Rigidbody rigidbody = enteringObject.GetComponent<Rigidbody>();
            StartCoroutine(StopCharacter(rigidbody));
            StartCoroutine(Rotation());
            stageClearSound.Play();
            GameController.instance.CalculateBonus();
            HUDControl.instance.UpdateEndLevelText(SceneManager.GetActiveScene().buildIndex);
            HUDControl.instance.ChangeStatesEndScoreScreen(true);
            StartCoroutine(ChangeBonusVictory());
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            StartCoroutine(NextLevel());
        }
    }
    /// <summary>
    /// Corrutina para aplicar una bonificación a la puntuación después de un retardo específico.
    /// </summary>
    IEnumerator ChangeBonusVictory()
    {
        yield return new WaitForSeconds(audioLength / 2);
        GameController.instance.BonusInScore();
    }
    /// <summary>
    /// Corrutina para detener el movimiento del personaje después de un breve retardo.
    /// Congela el Rigidbody del personaje para detener cualquier movimiento.
    /// </summary>
    /// <param name="rigidbody">Rigidbody del personaje.</param>
    IEnumerator StopCharacter(Rigidbody rigidbody)
    {
        yield return new WaitForSeconds(0.01f);
        if (rigidbody != null)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    /// <summary>
    /// Corrutina para rotar gradualmente el objeto durante un período de tiempo.
    /// </summary>
    IEnumerator Rotation()
    {
        yield return new WaitForSeconds(1);

        float timePassed = 0;
        while (timePassed < 5)
        {
            // Code to go left here
            timePassed += Time.deltaTime;
            transform.Rotate(new Vector3(0f, 1f, 0f));
            yield return null;
        }
    }
    /// <summary>
    /// Corrutina para cargar el siguiente nivel después de un retardo específico.
    /// </summary>
    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(8f);
        HUDControl.instance.NextLevel(nextLevel);
    }
}
