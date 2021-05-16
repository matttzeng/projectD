
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public int health ;
    public int exp;
    public int value;

    //patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInsightRange, playerInAttackRange;

    int dieNum;

    private void Awake()
    {
        player = GameObject.Find("PlayerObj").transform;
        agent = GetComponent<NavMeshAgent>();

    }

    private void Update()
    {
        playerInsightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange , whatIsPlayer);

        if (!playerInsightRange && !playerInAttackRange) Patroling();
        if (playerInsightRange && !playerInAttackRange) ChasePlayer();
        if (playerInsightRange && playerInAttackRange) AttackPlayer();

    }
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


    private void SearchWalkPoint()
    {
        



        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;

    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///attack code here

            Rigidbody rb = Instantiate(projectile,transform.position,Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 8f, ForceMode.Impulse);
            rb.AddForce(transform.up * 2f, ForceMode.Impulse);
           

            ///

            alreadyAttacked = true;
            Invoke(nameof(ResetAttck), timeBetweenAttacks);
        }
    }

    private void ResetAttck()
    {
        alreadyAttacked = false;
        
    }
    
    public void TakeDamage(int amount)
    {
        
        health -= amount;
        
        if (health <= 0)
        {
            die();
           
            
        };
           
       
         
    }

    public void die()
    {
        PlayerStats11.Score += 1;
        PlayerStats11.Exp += exp;
        PlayerStats11.Money += value;
        Destroy(gameObject);
        //add money and exp
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
