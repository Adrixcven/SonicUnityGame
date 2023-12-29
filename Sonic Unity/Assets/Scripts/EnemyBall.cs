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
    /// Asigna el GameController encontrando un objeto con la etiqueta "GC".
    /// </summary>
    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GC");
    }
    /// <summary>
    /// Invoca el método Setup durante la inicialización.
    /// </summary>
    private void Start()
    {
        Setup();
    }
    /// <summary>
    /// Actualiza el tiempo de vida del objeto, destruyéndolo si se excede el tiempo máximo de vida.
    /// </summary>
    private void Update()
    {
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0) Destroy(gameObject);
    }
    /// <summary>
    /// Verifica la presencia de un objeto padre y, si es un jugador, realiza acciones
    /// como el registro, el manejo de escudos, la invencibilidad y la manipulación de los valores del GameController.
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
    /// Configura el objeto mediante la configuración de sus propiedades de material físico y el uso de la gravedad.
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

