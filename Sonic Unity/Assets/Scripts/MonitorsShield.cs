using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorsShield : MonoBehaviour
{
    public GameObject gameController;
    public GameObject player;
    public ParticleSystem particleSystem;

    /// <summary>
    /// Asigna el GameController encontrando un objeto con la etiqueta "GC".
    /// </summary>
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GC");

    }

    /// <summary>
    /// Verifica el objeto colisionado y, si es un jugador no en el suelo, realiza acciones
    /// como habilitar/deshabilitar escudos, aplicar fuerza hacia arriba, reproducir efectos de partículas y desactivar el objeto actual.
    /// </summary>
    /// <param name="collision">Información sobre la colisión.</param>
    private void OnCollisionEnter(Collision collision)
    {
        GameObject enteringObject = collision.gameObject;
        Rigidbody rb = enteringObject.GetComponent<Rigidbody>();
        if (enteringObject.CompareTag("Player"))
        {
            if (!player.GetComponent<PlayerMovement>().grounded)
            {
                gameController.GetComponent<GameController>().EnableDisableShield();
                Vector3 upwardForce = Vector3.up * 1f;
                rb.AddForce(upwardForce, ForceMode.Impulse);
                particleSystem.Play();
                gameObject.SetActive(false);
            }
        }
    }
}
