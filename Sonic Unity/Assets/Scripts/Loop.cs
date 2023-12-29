using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop : MonoBehaviour
{
    public GameObject player;
    public RaycastHit hit;
    public Quaternion rotation;
    /// <summary>
    /// Obtiene el GameObject padre y el Rigidbody del Collider que entra en la zona de activación.
    /// </summary>
    /// <param name="other">El Collider que entra en la zona de activación.</param>
    private void OnTriggerEnter(Collider other)
    {
        GameObject enteringObject = other.transform.parent.gameObject;
        Rigidbody rb = enteringObject.GetComponent<Rigidbody>();
    }
    /// <summary>
    /// Realiza un raycast desde la posición del objeto que entra hacia abajo y ajusta su rotación basándose en la normal de la superficie.
    /// Almacena la rotación resultante para su uso posterior.
    /// </summary>
    /// <param name="other">El Collider que permanece en la zona de activación.</param>
    private void OnTriggerStay(Collider other)
    {
        GameObject enteringObject = other.transform.parent.gameObject;
        if (Physics.Raycast(enteringObject.transform.position, -enteringObject.transform.up, out hit, 3f))
        {
            enteringObject.transform.rotation = Quaternion.FromToRotation(enteringObject.transform.up, hit.normal) * enteringObject.transform.rotation;
        }
        rotation = Quaternion.Euler(0, enteringObject.transform.localRotation.eulerAngles.y, 0);

    }
    /// <summary>
    /// Se activa cuando un Collider sale de la zona de activación.
    /// Obtiene el GameObject padre y el Rigidbody del Collider que sale y restaura su rotación.
    /// </summary>
    /// <param name="other">El Collider que sale de la zona de activación.</param>

    private void OnTriggerExit(Collider other)
    {
        GameObject enteringObject = other.transform.parent.gameObject;
        Rigidbody rb = enteringObject.GetComponent<Rigidbody>();
        enteringObject.transform.rotation = rotation;

    }
}
