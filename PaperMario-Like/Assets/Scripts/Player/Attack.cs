using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] public Animator animator;
    [SerializeField] private PlayerManager player;
    [SerializeField] private GameObject attackPoint;
    [SerializeField] private GameObject prefab;

    private float playerInputX;
    private float playerInputZ;
    private bool isAttacking = false;

    private void Update()
    {
        playerInputX = player.GetInputX();
        playerInputZ = player.GetInputZ();
        IsAttacking();

        if(Input.GetMouseButtonUp(0) && !isAttacking)
        {
            StartCoroutine(NormalAttack());
        }
    }

    private IEnumerator NormalAttack()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);

        Instantiate(prefab, attackPoint.transform.position, Quaternion.identity, attackPoint.transform);

        if (Mathf.Abs(playerInputX) > 0)
        {
            if (playerInputX > 0)
            {
                animator.SetTrigger("atkRight");
            }
            else if (playerInputX < 0)
            {
                animator.SetTrigger("atkLeft");
            }
        }
        else if (Mathf.Abs(playerInputZ) > 0)
        {
            if (playerInputZ > 0)
            {
                animator.SetTrigger("atkUp");
            }
            else if (playerInputZ < 0)
            {
                animator.SetTrigger("atkDown");
            }
        }
        else
        {
            // check for player direction
            if(player.wasLastMovingXPos)
            {
                animator.SetTrigger("atkRight");
            }
            else if(player.wasLastMovingXNeg)
            {
                animator.SetTrigger("atkLeft");
            }
            else if(player.wasLastMovingZPos)
            {
                animator.SetTrigger("atkUp");
            }
            else
            {
                animator.SetTrigger("atkDown");
            }
        }


        yield return new WaitForSeconds(0.25f);


        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(attackPoint.transform.position, 0.5f);
    }
}
