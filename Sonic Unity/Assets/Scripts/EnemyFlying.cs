using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class EnemyFlying : MonoBehaviour
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
    public bool playerInSightRange, playerInAttackRange;

    public AudioSource enemyHitted;
    public GameObject projectile;
    /// <summary>
    /// Initializes references to the GameController, player, NavMeshAgent, and Animator components.
    /// </summary>
    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>();
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    /// <summary>
    /// Configures the initial state of the enemy based on the current game difficulty.
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
    /// Checks for player presence within sight and attack ranges and triggers corresponding behaviors.
    /// </summary>
    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

    }
    /// <summary>
    /// Handles patrolling behavior, moving to a randomly selected walk point.
    /// </summary>
    private void Patroling()
    {

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);


        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

    }
    /// <summary>
    /// Searches for a random walk point within the specified range and sets it as the destination.
    /// </summary>
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        NavMeshHit hit;

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        bool insideNavMesh = NavMesh.SamplePosition(walkPoint, out hit, 100f, NavMesh.AllAreas);


        if (Physics.Raycast(walkPoint, -transform.up, 100f, whatIsGround) && insideNavMesh)
            walkPointSet = true;
    }
    /// <summary>
    /// Sets the destination to the player's position for chasing behavior.
    /// </summary>
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    /// <summary>
    /// Initiates the attack behavior, shooting projectiles at the player.
    /// </summary>
    private void AttackPlayer()
    {
        anim.SetBool("IsShooting", true);
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        if (!alreadyAttacked)
        {

            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 3f, ForceMode.Impulse);
            rb.AddForce(transform.up * 0f, ForceMode.Impulse);

            alreadyAttacked = true;
            anim.SetBool("IsShooting", false);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    /// <summary>
    /// Resets the attack state after the specified time interval.
    /// </summary>
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    /// <summary>
    ///  Handles collision events with the player, applying damage and handling shield mechanics.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        GameObject enteringObject = collision.gameObject;
        if (enteringObject.CompareTag("Player"))
        {
            Rigidbody rb = enteringObject.GetComponent<Rigidbody>();
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
                            if(gameController.GetComponent<GameController>().rings < 10)
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
    ///  Initiates the enemy taking damage sequence, playing the appropriate animations and sound.
    /// </summary>
    public void TakeDamage()
    {
        enemyHitted.Play();
        anim.SetBool("IsDead", true);
        anim.Play("Armature|death");
        StartCoroutine(EnemyDeadRoutine());
    }
    /// <summary>
    /// Coroutine for handling the enemy's death sequence, including a delay before destruction.
    /// Adds points to the GameController upon enemy's death.
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemyDeadRoutine()
    {

        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        gameController.GetComponent<GameController>().AddPointsEnemy();

    }
}
