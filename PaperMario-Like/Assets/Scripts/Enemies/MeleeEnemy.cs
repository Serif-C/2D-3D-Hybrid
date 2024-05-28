using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : EnemyAbs
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected void FixedUpdate()
    {
        EnemyMovement();
        MovementAnimation();

        if (distanceToPlayer < attackRange)
        {
            if (!isAttacking)
            {
                StartCoroutine(EnemyAttack());
            }
        }

        if (!IsAlive())
        {
            //Debug.Log("enemy dead");
            Destroy(gameObject);
        }
    }

    protected override void EnemyMovement()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
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
            else if (Vector3.Distance(enemy.transform.position, roamDestination) <= 0.1f)
            {
                //Debug.Log("Arrived at roam destination");
                if (!isReturning)
                {
                    hasReachedDestination = true;
                    ReturnToStartPoint();
                }
            }
            else
            {
                if (enemy.remainingDistance <= 0.1f)
                {
                    if (isRoaming && isReturning)
                    {
                        isRoaming = false;
                        isReturning = false;
                        hasReachedDestination = false;
                    }
                }
            }
        }
    }

    protected override IEnumerator EnemyAttack()
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

    // MELEE ENEMY PROPERTIES //

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
        /*Debug.Log("return destination: " + enemy.destination);
        Debug.Log("Returning");*/
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = sphereColor;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
