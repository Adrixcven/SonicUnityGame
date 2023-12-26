using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop : MonoBehaviour
{
    public GameObject player;
    public RaycastHit hit;
    public Quaternion rotation;
    /// <summary>
    /// Retrieves the parent GameObject and Rigidbody of the entering Collider.
    /// </summary>
    /// <param name="other">The Collider entering the trigger zone.</param>
    private void OnTriggerEnter(Collider other)
    {
        GameObject enteringObject = other.transform.parent.gameObject;
        Rigidbody rb = enteringObject.GetComponent<Rigidbody>();


    }
    /// <summary>
    /// Performs a raycast from the entering object's position downward and adjusts its rotation based on the surface normal.
    /// Stores the resulting rotation for later use.
    /// </summary>
    /// <param name="other">The Collider staying in the trigger zone.</param>
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
    /// Triggered when a Collider exits the trigger zone.
    /// Retrieves the parent GameObject and Rigidbody of the exiting Collider and restores its rotation.
    /// </summary>
    /// <param name="other">The Collider exiting the trigger zone.</param>
    private void OnTriggerExit(Collider other)
    {
        GameObject enteringObject = other.transform.parent.gameObject;
        Rigidbody rb = enteringObject.GetComponent<Rigidbody>();
        enteringObject.transform.rotation = rotation;

    }
}
