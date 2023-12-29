using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{

    public AudioSource springSound;

    public float springForce = 1;

    public bool diagonalSpring = false;

    /// <summary>
    /// Obtiene el GameObject padre del Collider y sus componentes Animator y Rigidbody.
    /// Aplica una fuerza de resorte para impulsar el objeto hacia arriba, reproduce una animación y un efecto de sonido.
    /// </summary>
    /// <param name="other">El Collider que entra en la zona del trigger.</param>
    private void OnTriggerEnter(Collider other)
    {

        GameObject enteringObject = other.transform.parent.gameObject;
        Animator anim = enteringObject.GetComponent<Animator>();
        Rigidbody rb = enteringObject.GetComponent<Rigidbody>();
        Vector3 upwardForce;

        if (diagonalSpring)
            upwardForce = new Vector3(-1f, 200f, 0f).normalized * springForce;
        else
            upwardForce = Vector3.up * springForce;

        anim.SetBool("InGround", false);
        anim.Play("sn_springjump_loop");
        springSound.Play();
        rb.AddForce(upwardForce, ForceMode.Impulse);
    }
}
