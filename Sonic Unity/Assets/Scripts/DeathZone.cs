using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{

    public GameObject player;
    public GameObject GameController;
    public AudioSource DeathSound;
    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject enteringObject = other.transform.parent.gameObject;
        Animator anim = enteringObject.GetComponent<Animator>();
        if (enteringObject.CompareTag("Player"))
        {
            DeathSound.Play();
            anim.Play("sn_death_loop");
            Invoke("GameControllerOptions", 0.3f);

        }
    }
    private void GameControllerOptions()
    {
        GameController.GetComponent<GameController>().LoseRings(GameController.GetComponent<GameController>().Rings);
        GameController.GetComponent<GameController>().LoseLives(1);


        GameController.GetComponent<GameController>().respawn();
    }
}
