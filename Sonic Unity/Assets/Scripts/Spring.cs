using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{

    public AudioSource springSound;

    public float springForce = 1;

    private void OnTriggerEnter(Collider other)
    {

        GameObject enteringObject = other.transform.parent.gameObject;
        Animator anim = enteringObject.GetComponent<Animator>();
        Rigidbody rb = enteringObject.GetComponent<Rigidbody>();
        Vector3 upwardForce = Vector3.up * springForce;
        anim.SetBool("InGround", false);
        anim.Play("sn_springjump_loop");

        springSound.Play();
        rb.AddForce(upwardForce, ForceMode.Impulse);
    }
}
