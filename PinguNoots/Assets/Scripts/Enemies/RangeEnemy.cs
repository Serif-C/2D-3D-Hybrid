using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : EnemyAbs
{
    [SerializeField] GameObject projAtk;
    [SerializeField] float atkSpeed;

    private float launchVelocity = 700f;
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
        if (!isAttacking)
        {
            StartCoroutine(EnemyAttack());
        }
    }

    protected override void EnemyMovement()
    {
        /*
         * Enemy should not roam
         * Enemy should attempt to walk away when player is nearby
         * Enemy should return to start position when player is far away
         */
    }

    protected override IEnumerator EnemyAttack()
    {
        /*
         * A projectile should be thrown towards the player's position
         * The projectile is not homing
         * The projectile is destroyed on collission
         */
        isAttacking = true;

        GameObject proj = Instantiate(projAtk, transform.position, Quaternion.identity);
        proj.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, launchVelocity, 0));

        yield return new WaitForSeconds(1 / atkSpeed);

        isAttacking = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = sphereColor;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
