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
    /// initializes the gameController by finding the GameObject with the tag "GC" in the scene.
    /// </summary>
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GC");
    }

    /// <summary>
    /// Checks for the presence of a parent object, and if it's a player, performs actions such as disabling shields,
    /// resetting rings, and decrementing lives in the GameController.
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
