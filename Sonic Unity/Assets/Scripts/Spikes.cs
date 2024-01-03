using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public GameController gameController;
    /// <summary>
    /// Asigna el componente GameController de un GameObject con la etiqueta "GC" a la variable local.
    /// </summary>
    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>();
    }
    /// <summary>
    /// Verifica la presencia de un objeto padre y, si es un jugador, realiza acciones
    /// basadas en la dificultad actual del juego, como decrementar anillos en el GameController.
    /// </summary>
    /// <param name="other">El Collider que entra en la zona de activación.</param>
    private void OnTriggerEnter(Collider other)
    {
        GameObject enteringObject = other.transform.parent.gameObject;
        if (enteringObject.CompareTag("Player"))
        {
            if (!gameController.GetComponent<GameController>().shieldOn)
            {
                if (Difficulty.instance.currentDifficulty == 0)
                    gameController.GetComponent<GameController>().LoseRings(gameController.GetComponent<GameController>().rings);
                else
                {
                    if (gameController.GetComponent<GameController>().rings < 10)
                        gameController.GetComponent<GameController>().LoseRings(gameController.GetComponent<GameController>().rings);
                    else
                        gameController.GetComponent<GameController>().LoseRings(10);
                }
            }
            else
                gameController.GetComponent<GameController>().EnableDisableShield();
        }
    }
}
