using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBall : MonoBehaviour
{
    public Rigidbody rb;

    public GameObject gameController;
    public bool useGravity;
    public float maxLifetime;

    int collisions;
    PhysicMaterial physicMaterial;

    /// <summary>
    /// Assigns the GameController by finding a GameObject with the tag "GC".
    /// </summary>
    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GC");
    }
    /// <summary>
    /// Invokes the Setup method during initialization.
    /// </summary>
    private void Start()
    {
        Setup();
    }
    /// <summary>
    /// Updates the lifetime of the object, destroying it if the maximum lifetime is exceeded.
    /// </summary>
    private void Update()
    {
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0) Destroy(gameObject);
    }
    /// <summary>
    /// Checks for the presence of a parent object, and if it's a player, performs actions
    /// such as logging, handling shields, invincibility, and manipulating GameController values.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null)
        {
            GameObject enteringObject = other.transform.parent.gameObject;
            if (enteringObject.CompareTag("Player"))
            {
                Debug.Log("Entra");
                if (!gameController.GetComponent<GameController>().shieldOn)
                {
                    if (!gameController.GetComponent<GameController>().invincible)
                    {
                        gameController.GetComponent<GameController>().LoseRings(gameController.GetComponent<GameController>().rings);
                        gameController.GetComponent<GameController>().InvincibleEnabled();
                        Destroy(gameObject);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                else
                    gameController.GetComponent<GameController>().EnableDisableShield();
            }
        }


    }
    /// <summary>
    /// Sets up the object by configuring its physical material properties and gravity usage.
    /// </summary>
    private void Setup()
    {
        physicMaterial = new PhysicMaterial();
        physicMaterial.bounciness = 0f;
        physicMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        physicMaterial.bounceCombine = PhysicMaterialCombine.Maximum;

        GetComponent<SphereCollider>().material = physicMaterial;

        rb.useGravity = useGravity;
    }
}

