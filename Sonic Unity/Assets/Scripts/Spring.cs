using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{

    public AudioSource springSound;

    public float springForce = 1;

    public bool diagonalSpring = false;

    /// <summary>
    /// Retrieves the parent GameObject from the Collider and obtains its Animator and Rigidbody components.
    /// Applies a spring force to propel the object upwards, playing an animation and sound effect.
    /// </summary>
    /// <param name="other">The Collider entering the trigger zone.</param>
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
