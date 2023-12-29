using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorsRings : MonoBehaviour
{
    public GameObject gameController;
    public GameObject player;
    public ParticleSystem particleSystem;
    public AudioSource ringSound;
    public AudioSource livesUpSound;

    /// <summary>
    /// Asigna el GameController encontrando un GameObject con la etiqueta "GC".
    /// </summary>
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GC");

    }
    /// <summary>
    /// Verifica si el objeto en colisión es un jugador y no está en el suelo.
    /// Si se cumplen las condiciones, agrega anillos, puntos, reproduce sonidos y maneja la lógica adicional del juego.
    /// </summary>
    /// <param name="collision">Datos de colisión asociados a la colisión.</param>
    private void OnCollisionEnter(Collision collision)
    {
        GameObject enteringObject = collision.gameObject;
        Rigidbody rb = enteringObject.GetComponent<Rigidbody>();
        if (enteringObject.CompareTag("Player"))
        {
            if (!player.GetComponent<PlayerMovement>().grounded)
            {
                gameController.GetComponent<GameController>().AddRings(10);
                gameController.GetComponent<GameController>().AddPointsRing();
                ringSound.Play();
                if (gameController.GetComponent<GameController>().rings % 100 == 0)
                {
                    gameController.GetComponent<GameController>().AddLives(1);
                    livesUpSound.Play();
                }
                Vector3 upwardForce = Vector3.up * 1f;
                rb.AddForce(upwardForce, ForceMode.Impulse);
                particleSystem.Play();
                gameObject.SetActive(false);
            }
        }
    }
}
