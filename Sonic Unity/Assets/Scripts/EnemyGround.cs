using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class EnemyGround : MonoBehaviour
{
    public GameController gameController;
    public NavMeshAgent agent;
    public NavMeshSurface navMeshSurface;
    public Animator anim;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRange;

    public AudioSource enemyHitted;

    /// <summary>
    /// Inicializa las referencias al GameController, player, NavMeshAgent y Animator en Awake().
    /// </summary>
    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>();
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    /// <summary>
    /// Controla la activación del GameObject basándose en la dificultad actual del juego en Start().
    /// </summary>
    private void Start()
    {
        if (Difficulty.instance.currentDifficulty == 1)
        {
            if (gameObject.CompareTag("DifficultyAffected"))
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);

        }
        else
            gameObject.SetActive(true);
    }
    /// <summary>
    /// Verifica la presencia del jugador dentro del rango de visión y activa el comportamiento de patrulla o persecución según corresponda en Update().
    /// </summary>
    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (!playerInSightRange) Patroling();
        if (playerInSightRange) ChasePlayer();

    }
    /// <summary>
    /// Maneja el comportamiento de patrulla, estableciendo puntos de destino y moviéndose hacia ellos.
    /// </summary>
    private void Patroling()
    {
        anim.SetBool("IsFollowing", false);

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

    }
    /// <summary>
    /// Busca un punto de patrulla aleatorio dentro del rango especificado en el NavMesh.
    /// </summary>
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        NavMeshHit hit;

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        bool insideNavMesh = NavMesh.SamplePosition(walkPoint, out hit, 0.1f, NavMesh.AllAreas);


        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround) && insideNavMesh)
            walkPointSet = true;
    }
    /// <summary>
    /// Inicia el comportamiento de persecución, estableciendo el destino a la posición del jugador.
    /// </summary>
    private void ChasePlayer()
    {
        anim.SetBool("IsFollowing", true);
        agent.SetDestination(player.position);
    }
    /// <summary>
    /// Maneja la colisión con el jugador, aplicando daño y realizando acciones basadas en las condiciones del juego.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        GameObject enteringObject = collision.gameObject;
        Rigidbody rb = enteringObject.GetComponent<Rigidbody>();
        if (enteringObject.CompareTag("Player"))
        {
            if (!player.GetComponent<PlayerMovement>().grounded)
            {
                Vector3 upwardForce = Vector3.up * 3.0f;
                rb.AddForce(upwardForce, ForceMode.Impulse);
                TakeDamage();
            }
            else
            {
                if (!gameController.GetComponent<GameController>().shieldOn)
                {
                    if (!gameController.GetComponent<GameController>().invincible)
                    {
                        playerInSightRange = false;
                        if (Difficulty.instance.currentDifficulty == 0)
                            gameController.GetComponent<GameController>().LoseRings(gameController.GetComponent<GameController>().rings);
                        else
                        {
                            if (gameController.GetComponent<GameController>().rings < 10)
                                gameController.GetComponent<GameController>().LoseRings(gameController.GetComponent<GameController>().rings);
                            else
                                gameController.GetComponent<GameController>().LoseRings(10);
                        }
                        gameController.GetComponent<GameController>().InvincibleEnabled();
                    }
                }
                else
                    gameController.GetComponent<GameController>().EnableDisableShield();
            }
        }
    }
    /// <summary>
    /// Inicia la secuencia de recibir daño, incluyendo la animación y la destrucción del enemigo.
    /// </summary>
    public void TakeDamage()
    {
        enemyHitted.Play();
        anim.SetBool("IsDead", true);
        anim.Play("Death");
        StartCoroutine(EnemyDeadRoutine());
    }
    /// <summary>
    /// Coroutine para manejar la secuencia de muerte del enemigo, incluyendo un retraso antes de la destrucción.
    /// También otorga puntos al GameController.
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemyDeadRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        gameController.GetComponent<GameController>().AddPointsEnemy();

    }
}
