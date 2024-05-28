using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyAbs : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected NavMeshAgent enemy;
    [SerializeField] protected float detectionRange;
    [SerializeField] protected Animator animator;

    [Header("Enemy Basic Stats")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float armour;

    protected Color sphereColor = Color.red;

    [Header("Movement Behaviour Attributes")]
    protected Transform player;
    protected float distanceToPlayer;
    protected Vector3 startPosition;
    protected bool isRoaming;
    protected Vector3 roamDestination;
    protected bool isReturning;
    protected bool hasReachedDestination;

    [Header("Attack Behaviour Attributes")]
    protected float attackRange = 1f;
    protected float attacksPerSecond = 2f;
    protected bool isAttacking = false;

    protected virtual void Awake()
    {
        enemy = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        startPosition = enemy.transform.position;
    }

    protected abstract void EnemyMovement();

    protected void MovementAnimation()
    {
        float backAngle = 65f;
        float sideAngle = 130f;

        Vector3 cameraForwardDirection = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);

        float signedAngle = Vector3.SignedAngle(enemy.transform.forward, cameraForwardDirection, Vector3.up);
        float angle = Mathf.Abs(signedAngle);

        Vector2 animationDirection = new Vector2(0f, -1f);

        if (angle > backAngle)
        {
            animationDirection = new Vector2(0f, -1f);
        }
        else if (angle < sideAngle)
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

    protected bool IsAlive()
    {
        if (currentHealth > 0.0f)
        {
            return true;
        }
        return false;
    }

    protected abstract IEnumerator EnemyAttack();
}
