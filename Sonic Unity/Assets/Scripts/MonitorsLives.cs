using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitors : MonoBehaviour
{
    public GameObject gameController;
    public GameObject player;
    public AudioSource livesUpSound;
    public ParticleSystem particleSystem;

    /// <summary>
    /// Configura el GameController y ajusta la actividad del objeto según la dificultad actual del juego.
    /// </summary>
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GC");
        if (Difficulty.instance.currentDifficulty == 1)
        {
            if (gameObject.CompareTag("DifficultyAffected"))
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);

        }
        else
            gameObject.SetActive(true);

    }
    /// <summary>
    /// Verifica el objeto que ingresa y, si es un jugador y no está en el suelo,
    /// realiza acciones como agregar vidas, reproducir sonidos, aplicar fuerza y desactivar el objeto.
    /// </summary>
    /// <param name="collision">Información sobre el evento de colisión.</param>
    private void OnCollisionEnter(Collision collision)
    {
        GameObject enteringObject = collision.gameObject;
        Rigidbody rb = enteringObject.GetComponent<Rigidbody>();
        if (enteringObject.CompareTag("Player"))
        {
            if (!player.GetComponent<PlayerMovement>().grounded)
            {
                gameController.GetComponent<GameController>().AddLives(1);
                livesUpSound.Play();
                Vector3 upwardForce = Vector3.up * 1f;
                rb.AddForce(upwardForce, ForceMode.Impulse);
                particleSystem.Play();
                gameObject.SetActive(false);
            }
        }
    }
}
