using System.Collections;
using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float dmgAmount);
}

public interface IEXPReward
{
    public void EarnEXP(float expAmount);
}

public class PlayerManager : MonoBehaviour, IDamageable, IEXPReward
{
    [Header("Component References")]
    [SerializeField] CharacterController playerController;
    [SerializeField] Transform cam;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sprite;

    [Header("Basic Stats")]
    [SerializeField] float vigor;       // affects health
    [SerializeField] float strength;    // affects atk/armour
    [SerializeField] float endurance;   // affects stamina/armour
    [SerializeField] float intelligence;// affects mana/magicDamage
    [SerializeField] float dexterity;   // affects atk/movementSpeed/turnSpeed
    [SerializeField] float luck;        // affects
    [SerializeField] int level;
    [SerializeField] float currentEXP;
    [SerializeField] float neededEXP;

    [Header("Attribute Affected Stats")]
    [SerializeField] float currentHealth;
    [SerializeField] int maxHealth;
    [SerializeField] float atk;
    [SerializeField] float armour;
    [SerializeField] float stamina;
    [SerializeField] float magicDmg;
    [SerializeField] float mana;
    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed = 0.1f;

    [Header("Hidden Stats")]
    [SerializeField] float attackRange;
    [SerializeField] float expMultiplier;

    private float turnSmoothVelocity;
    private Vector3 direction;
    private float xInput;
    private float zInput;
    private Attack atkScript;
    
    public bool wasLastMovingXPos;
    public bool wasLastMovingXNeg;
    public bool wasLastMovingZPos;
    public bool wasLastMovingZNeg;

    private void Start()
    {
        Cursor.visible = false;
        atkScript = GetComponent<Attack>();
    }

    private void Update()
    {
        if (atkScript.animator.GetBool("isAttacking"))
        {
            BrackeyMovement(0);
        }
        else
        {
            BrackeyMovement(moveSpeed);
        }
    }
   /* private void OldMovement()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        zInput = Input.GetAxisRaw("Vertical");

        direction = new Vector3(xInput, 0f, zInput).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // calculates the target angle based on the direction of vector3
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSpeed);

            if (Mathf.Abs(xInput) > 0)
            {
                transform.rotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f); ;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            
            if(atkScript.animator.GetBool("isAttacking") == true)
            {
                playerController.Move(Vector3.zero);
                Debug.Log("attacking");
            }
            playerController.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
            playerController.SimpleMove(moveDirection.normalized * moveSpeed * Time.deltaTime);

        }
        else
        {
            // play idle animation according to (target angle?)
        }

        WalkAnimation(xInput, zInput);
    }*/

    private void WalkAnimation(float xInput, float zInput)
    {
        if (Mathf.Abs(xInput) > 0)
        {
            if (xInput > 0)
            {
                wasLastMovingXPos = true;
                wasLastMovingXNeg = false;
                wasLastMovingZPos = false;
                wasLastMovingZNeg = false;
                animator.SetBool("isWalkingRight", true);
                animator.SetBool("isWalkingLeft", false);
            }
            else if (xInput < 0)
            {
                wasLastMovingXPos = false;
                wasLastMovingXNeg = true;
                wasLastMovingZPos = false;
                wasLastMovingZNeg = false;
                animator.SetBool("isWalkingRight", false);
                animator.SetBool("isWalkingLeft", true);
            }
        }
        else
        {
            animator.SetBool("isWalkingRight", false);
            animator.SetBool("isWalkingLeft", false);
        }

        if (Mathf.Abs(zInput) > 0)
        {
            if (zInput > 0)
            {
                wasLastMovingXPos = false;
                wasLastMovingXNeg = false;
                wasLastMovingZPos = true;
                wasLastMovingZNeg = false;
                animator.SetBool("isWalkingUp", true);
                animator.SetBool("isWalkingDown", false);
            }
            else if (zInput < 0)
            {
                wasLastMovingXPos = false;
                wasLastMovingXNeg = false;
                wasLastMovingZPos = false;
                wasLastMovingZNeg = true;
                animator.SetBool("isWalkingUp", false);
                animator.SetBool("isWalkingDown", true);
            }
        }
        else
        {
            animator.SetBool("isWalkingUp", false);
            animator.SetBool("isWalkingDown", false);
        }
    }

    public bool IsPlayerDead()
    {
        if(currentHealth <= 0)
        {
            return true;
        }

        return false;
    }

    private void BrackeyMovement(float speed)
    {
        xInput = Input.GetAxisRaw("Horizontal");
        zInput = Input.GetAxisRaw("Vertical");

        direction = new Vector3(xInput, 0f, zInput).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            playerController.Move(moveDirection.normalized * speed * Time.deltaTime);
        }

        WalkAnimation(xInput, zInput);
    }

    // GETTERS AND SETTERS ////////////////////////////////////////////////////////////////////////////////////////////////
    public float GetAttackRange()
    {
        return attackRange;
    }

    public float GetInputX()
    {
        return xInput;
    }

    public float GetInputZ()
    {
        return zInput;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    // INTERFACE METHODS //////////////////////////////////////////////////////////////////////////////////////////////////
    public void TakeDamage(float dmgAmount)
    {
        if (!IsPlayerDead())
        {
            currentHealth -= dmgAmount;
        }
    }

    /*Enemies drops exp (particles) on death,
    when they collide with the player call this method*/
    public void EarnEXP(float expAmount) 
    {
        currentEXP += expAmount;
        Debug.Log("Earned " + expAmount + " amount of exp");

        if(currentEXP >= neededEXP)
        {
            Levelup();
        }
    }

    // MISCS /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Levelup()
    {
        Debug.Log("Level up!");
        level++;
        currentEXP = 0;
        neededEXP = neededEXP * expMultiplier;

        /*
         * Add level up rewards here
         */
    }
}
