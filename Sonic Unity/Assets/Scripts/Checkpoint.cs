using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the checkpoint methods.
/// </summary>
public class Checkpoint : MonoBehaviour
{
    public AudioSource checkpointSound;
    private GameController gc;

    /// <summary>
    /// When waking up, the object looks for a GameObject with the "GC" tag and, if found, stores the GameController component in a variable.
    /// </summary>
    void Awake()
    {
        GameObject respawnObject = GameObject.FindGameObjectWithTag("GC");
        if (respawnObject != null)
        {
            gc = respawnObject.GetComponent<GameController>();
            if (gc == null)
            {
                Debug.LogError("No GameController");
            }
        }
        else
        {
            Debug.LogError("No object found with the GC tag.");
        }
    }

    /// <summary>
    /// When the parent of the collider has the tag "Player", if the respawnPoint in GameController is not this object, it changes to this object.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        GameObject enteringObject = other.transform.parent.gameObject;
        if (enteringObject.CompareTag("Player"))
        {
            if (gc.respawnPoint != this.gameObject)
            {
                checkpointSound.Play();
                gc.respawnPoint = this.gameObject;
            }

        }
    }

}
