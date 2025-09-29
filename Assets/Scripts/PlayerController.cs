
using System.Collections;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;

    private bool isMoving = false;

    private Vector2 input;

    private Animator animator;

    public LayerMask solidObjectsLayer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0; // Prevent diagonal movement

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                //var targetPos = transform.position;
                //targetPos.x += input.x;
                //targetPos.y += input.y;

                Vector3 targetPos = transform.position;
                targetPos.x += input.x * moveSpeed * Time.deltaTime;
                targetPos.y += input.y * moveSpeed * Time.deltaTime;

                if (IsWalkable(targetPos))
                    StartCoroutine(Move(targetPos));
                
            }
        }

        animator.SetBool("isMoving", isMoving);
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
    }


    public bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.5f, solidObjectsLayer) != false)
            return false;
        return true;
    }

}
