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
    /// Sets up the GameController and adjusts the object's activity based on the current game difficulty.
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
    /// Checks for the entering object, and if it's a player and not grounded,
    /// performs actions such as adding lives, playing sounds, applying force, and deactivating the object.
    /// </summary>
    /// <param name="collision">Information about the collision event.</param>
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
