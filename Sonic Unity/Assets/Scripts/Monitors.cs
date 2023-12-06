using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitors : MonoBehaviour
{
    public GameObject GameController;
    public GameObject player;
    public AudioSource livesUpSound;
    public ParticleSystem particleSystem;


    private void OnCollisionEnter(Collision collision)
    {
        GameObject enteringObject = collision.gameObject;
        Rigidbody rb = enteringObject.GetComponent<Rigidbody>();
        if (enteringObject.CompareTag("Player"))
        {
            if (!player.GetComponent<PlayerMovement>().grounded)
            {
                GameController.GetComponent<GameController>().AddLives(1);
                livesUpSound.Play();
                Vector3 upwardForce = Vector3.up * 0.5f;
                rb.AddForce(upwardForce, ForceMode.Impulse);
                particleSystem.Play();  
                gameObject.SetActive(false);
            }
        }
    }
}
