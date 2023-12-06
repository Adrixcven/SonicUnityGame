using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public GameObject player;
    public GameObject GameController;
    public AudioSource RingSound;
    public AudioSource livesUpSound;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, 1f, 0f));
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject enteringObject = other.transform.parent.gameObject;
        if(enteringObject.CompareTag("Player"))
        {
            GameController.GetComponent<GameController>().AddRings(1);
            RingSound.Play();
            if(GameController.GetComponent<GameController>().Rings % 100 == 0)
            {
                GameController.GetComponent<GameController>().AddLives(1);
                livesUpSound.Play();
            }
                
            
            gameObject.SetActive(false);
        }
    }
}
