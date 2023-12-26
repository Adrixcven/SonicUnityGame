using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorsShield : MonoBehaviour
{
    public GameObject gameController;
    public GameObject player;
    public ParticleSystem particleSystem;

    /// <summary>
    /// Assigns the GameController by finding a GameObject with the tag "GC".
    /// </summary>
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GC");

    }

    /// <summary>
    /// Checks the collided object, and if it's a player not grounded, performs actions
    /// such as enabling/disabling shields, applying upward force, playing particle effects, and deactivating the current object.
    /// </summary>
    /// <param name="collision">Information about the collision.</param>
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
