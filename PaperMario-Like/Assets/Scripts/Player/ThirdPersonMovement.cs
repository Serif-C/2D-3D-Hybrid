using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController playerController;
    public Transform cam;
    public Animator animator;
    public SpriteRenderer sprite;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float turnSpeed = 0.1f;
    private float turnSmoothVelocity;

    private Vector3 direction;
    private float xInput;
    private float zInput;

    private Attack atkScript;

    private void Start()
    {
        Cursor.visible = false;
        atkScript = GetComponent<Attack>();
    }

    public float GetInputX()
    {
        return xInput;
    }

    public float GetInputZ()
    {
        return zInput;
    }

    private void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        zInput = Input.GetAxisRaw("Vertical");

        direction = new Vector3(xInput, 0f, zInput).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +  cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSpeed);

            if (Mathf.Abs(xInput) > 0)
            {
                transform.rotation = Quaternion.Euler(0f,  cam.eulerAngles.y, 0f); ;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if(!atkScript.IsAttacking())
            {
                playerController.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
                playerController.SimpleMove(moveDirection.normalized * moveSpeed * Time.deltaTime);
            }
            else
            {
                Debug.Log("NOT MOVING");
            }
        }

        WalkAnimation(xInput, zInput);
    }

    private void WalkAnimation(float xInput, float zInput)
    {
        if (Mathf.Abs(xInput) > 0)
        {
            if (xInput > 0)
            {
                animator.SetBool("isWalkingRight", true);
                animator.SetBool("isWalkingLeft", false);
            }
            else if (xInput < 0)
            {
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
                animator.SetBool("isWalkingUp", true);
                animator.SetBool("isWalkingDown", false);
            }
            else if (zInput < 0)
            {
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
}
