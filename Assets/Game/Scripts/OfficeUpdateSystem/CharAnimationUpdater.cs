using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(AIPath))]
public class CharAnimationUpdater : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float moveThreshold = 0.05f;

    private AIPath ai;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        ai = GetComponent<AIPath>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (animator == null || ai == null)
            return;

        Vector3 velocity = ai.velocity;
        velocity.y = 0f;

        bool isMoving = velocity.magnitude > moveThreshold && !ai.reachedDestination;
        animator.SetBool("isRuning", isMoving);

        // flip char based on direction
        if (Mathf.Abs(velocity.x) > 0.01f)
        {
            spriteRenderer.flipX = velocity.x < 0;
        }
    }
}
