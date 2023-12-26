using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public GameController gameController;
    /// <summary>
    /// Assigns the GameController component from a GameObject with the tag "GC" to the local variable.
    /// </summary>
    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>();
    }
    /// <summary>
    /// Checks for the presence of a parent object, and if it's a player, performs actions
    /// based on the current game difficulty, such as decrementing rings in the GameController.
    /// </summary>
    /// <param name="other">The Collider entering the trigger zone.</param>
    private void OnTriggerEnter(Collider other)
    {
        GameObject enteringObject = other.transform.parent.gameObject;
        if (enteringObject.CompareTag("Player"))
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
    }
}
