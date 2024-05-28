using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("References")]
    [SerializeField] NavMeshAgent enemy;
    [SerializeField] float detectionRange;
    [SerializeField] Animator animator;

    [Header("Enemy Basic Stats")]
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;
    [SerializeField] float armour;

    private Color sphereColor = Color.red;
    
    [Header("Movement Behaviour Attributes")]
    private Transform player;
    private float distanceToPlayer;
    private Vector3 startPosition; // Enemies return to their start position when they lose aggro
    private bool isRoaming;
    private Vector3 roamDestination;
    private bool isReturning;
    private bool hasReachedDestination;

    [Header("Attack Behaviour Attributes")]
    private float attackRange = 1f;
    private float attacksPerSecond = 2f;
    private bool isAttacking = false;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        currentHealth = maxHealth;

        startPosition = enemy.transform.position;
        //Debug.Log(startPosition);
    }

    private void FixedUpdate()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if(distanceToPlayer < detectionRange)
        {
            ChasePlayer();
        }
        else
        {   
            // Roam
            if (!isRoaming && !hasReachedDestination)
            {
                //Debug.Log("roaming");
                Roam();
            }
            // Return to start position after reaching roam destination
            else if(Vector3.Distance(enemy.transform.position, roamDestination) <= 0.1f)
            {
                //Debug.Log("Arrived at roam destination");
                if(!isReturning)
                {
                    hasReachedDestination = true;
                    ReturnToStartPoint();
                }
            }
            else
            {
                if(enemy.remainingDistance <= 0.1f)
                {
                    if(isRoaming && isReturning)
                    {
                        isRoaming = false;
                        isReturning = false;
                        hasReachedDestination = false;
                    }
                }
            }
        }

        MovementAnimation();

        if(distanceToPlayer < attackRange)
        {
            if(!isAttacking)
            {
                StartCoroutine(EnemyAttack());
            }
        }

        if(!IsAlive())
        {
            //Debug.Log("enemy dead");
            Destroy(gameObject);
        }
    }

    private void ChasePlayer()
    {
        isRoaming = false;
        isReturning = false;
        hasReachedDestination = false;
        enemy.SetDestination(player.position);
    }

    private void Roam()
    {
        isRoaming = true;
        hasReachedDestination = false;
        isReturning = false;

        /*Vector3 randomLocation = Random.insideUnitSphere * detectionRange;
        roamDestination = new Vector3(randomLocation.x, 0.5833333f, randomLocation.z);*/
        roamDestination = RandomNavSphere(startPosition, detectionRange, -1);

        enemy.SetDestination(roamDestination);
        //Debug.Log("Roaming Destination: " + roamDestination);
    }

    Vector3 RandomNavSphere(Vector3 origin, float range, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * range;

        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, range, layermask);

        return navHit.position;
    }

    private void ReturnToStartPoint()
    {
        isReturning = true;

        enemy.SetDestination(startPosition);
        //enemy.SetDestination(gameObject.GetComponentInParent<Transform>().position);
        //Debug.Log("return destination: " + enemy.destination);
        //Debug.Log("Returning");
    }

    private void MovementAnimation()
    {
        float backAngle = 65f;
        float sideAngle = 130f;

        Vector3 cameraForwardDirection = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);

        float signedAngle = Vector3.SignedAngle(enemy.transform.forward, cameraForwardDirection, Vector3.up);
        float angle = Mathf.Abs(signedAngle);

        Vector2 animationDirection = new Vector2(0f, -1f);

        if(angle > backAngle)
        {
            animationDirection = new Vector2(0f, -1f);
        }
        else if(angle < sideAngle)
        { 
            if (signedAngle < 0)
            {
                animationDirection = new Vector2(-1f, 0f);
            }
            animationDirection = new Vector2(1f, 0f);
        }
        else
        {
            animationDirection = new Vector2(0f, 1f);
        }

        animator.SetFloat("directionX", animationDirection.x);
        animator.SetFloat("directionZ", animationDirection.y);
    }

    public void TakeDamage(float dmgAmount)
    {
        Debug.Log("Enemy took " + dmgAmount + " amount of dmg");
        currentHealth -= dmgAmount;
    }

    public bool IsAlive()
    {
        if(currentHealth > 0.0f)
        {
            return true;
        }
        return false;
    }

    public IEnumerator EnemyAttack()
    {
        isAttacking = true;
        enemy.speed = 0f;

        /*
         * trigger attack animation with animation speed tied to attackSpeed
         */

        
        yield return new WaitForSeconds(1 / attacksPerSecond);
        

        isAttacking = false;
        enemy.speed = 1f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = sphereColor;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
