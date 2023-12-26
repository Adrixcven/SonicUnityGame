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
    /// Assigns the GameController by finding a GameObject with the tag "GC".
    /// </summary>
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GC");

    }
    /// <summary>
    /// Checks if the colliding object is a player and not grounded.
    /// If conditions are met, adds rings, points, plays sounds, and handles additional game logic.
    /// </summary>
    /// <param name="collision">The Collision data associated with the collision.</param>
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
