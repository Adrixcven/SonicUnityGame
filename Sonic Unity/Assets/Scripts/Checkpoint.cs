using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public AudioSource checkpointSound;
    private GameController GC;

    void Awake()
    {
        GameObject respawnObject = GameObject.FindGameObjectWithTag("GC");
        if (respawnObject != null)
        {
            GC = respawnObject.GetComponent<GameController>();
            if (GC == null)
            {
                Debug.LogError("No GameController");
            }
        }
        else
        {
            Debug.LogError("No object found with the GC tag.");
        }
    }
    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject enteringObject = other.transform.parent.gameObject;
        if (enteringObject.CompareTag("Player"))
        {
            if (GC.RespawnPoint != this.gameObject)
            {
                checkpointSound.Play();
                GC.RespawnPoint = this.gameObject;
            }

        }
    }

}
