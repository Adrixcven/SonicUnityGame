 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public AudioSource checkpointSound;
    private GameController gc;

    /// <summary>
    /// Al despertar, el objeto busca un GameObject con la etiqueta "GC" y, si se encuentra, almacena el componente GameController en una variable.
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
    ///  Cuando el padre del colisionador tiene la etiqueta "Player", si el punto de reaparición en GameController no es este objeto, cambia a este objeto.
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
