using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class for the methods used when falling out of the map.
/// </summary>
public class DeathZone : MonoBehaviour
{

    public GameObject player;
    public GameObject gameController;
    public AudioSource deathSound;
    /// <summary>
    /// Inicializa el gameController encontrando el GameObject con la etiqueta "GC" en la escena.
    /// </summary>
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GC");
    }

    /// <summary>
    /// Verifica la presencia de un objeto padre y, si es un jugador, realiza acciones como desactivar escudos,
    /// restablecer anillos y decrementar vidas en el GameController.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entra DeathZone");
        if (other.transform.parent != null)
        {
            GameObject enteringObject = other.transform.parent.gameObject;
            Animator anim = enteringObject.GetComponent<Animator>();
            if (enteringObject.CompareTag("Player"))
            {
                if (gameController.GetComponent<GameController>().shieldOn == true)
                    gameController.GetComponent<GameController>().EnableDisableShield();

                if (gameController.GetComponent<GameController>().rings > 0)
                    gameController.GetComponent<GameController>().rings = 0;

                gameController.GetComponent<GameController>().LoseLives(1);

            }
        }

    }
}
