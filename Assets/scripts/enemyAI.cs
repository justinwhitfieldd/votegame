using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float blockChance = 0.4f;
    public float minDistance = 1f;
    public float maxDistance = 3f;
    public float minMoveDuration = 1f;
    public float maxMoveDuration = 3f;
    public float attackProbability = 0.2f;
    public Animator animator;
    public Transform player;
    public float MinPlayerDistance = 0.1f;
    public AttributesManager playerAttribute;
    public bool isBlocking = false;
    private bool isPlayerAttacking = false;
    private bool isMoving = false;
    private float moveTimer = 0f;
    private Vector3 targetPosition;
    public float offset = 0f;
    public Transform finalPunchPosition;
    public Collider playerCollider;
    public bool isAttacking=false;
    public bool contactMade = false;
    private void OnAnimatorMove()
    {
        // Apply root motion to the enemy's position
        Vector3 deltaPosition = animator.deltaPosition;
        transform.position += deltaPosition;
    }
    public void ToggleBlocking()
    {
        isBlocking = false;
    }
    private void Update()
    {
        if(playerAttribute.PlayerIsPunching)
        {
            if(Random.value <= blockChance && !isBlocking)
            {
                animator.SetTrigger("Block");
                isBlocking = true;
            }
        }
        // Rotate the enemy to face the player
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        
        // Apply the offset as a rotation around the y-axis
        // This allows for a more controlled adjustment to the enemy's facing direction
        targetRotation *= Quaternion.Euler(0, offset, 0);
        
        // Smoothly rotate the enemy towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        
        // Calculate the distance between the enemy and the player
        float distance = Vector3.Distance(transform.position, player.position);
        
        if (!isMoving)
        {
            // Generate random movement duration and destination
            float moveDuration = Random.Range(minMoveDuration, maxMoveDuration);
            Vector3 randomDirection = Random.insideUnitSphere * Random.Range(minDistance, maxDistance);
            targetPosition = player.position;
            targetPosition.y = transform.position.y; // Maintain the same height as the enemy
            moveTimer = moveDuration;
            if (Vector3.Distance(transform.position, targetPosition) > MinPlayerDistance)
            {
                isMoving = true;
            }
        }
        
        if (isMoving)
        {
            // Move towards the target position using root motion
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            animator.SetFloat("MoveSpeed", 1f);
            
            // Check if the enemy has reached the target position or the move timer has expired
            if (Vector3.Distance(transform.position, targetPosition) <= MinPlayerDistance)
            {
                isMoving = false;
                animator.SetFloat("MoveSpeed", 0f);
            }
            
            moveTimer -= Time.deltaTime;
        }
        
        // Check if the player is within attack range
        if (!isAttacking)
        {
            // Check if the final punch position is inside the player character
            if (playerCollider.bounds.Contains(finalPunchPosition.position))
            {
                // Randomly decide whether to attack or not
                if (Random.value <= attackProbability)
                {
                    isAttacking = true;
                    Invoke("ResetEnemyAttacking", 1.5f);
                    animator.SetTrigger("Attack");
                }
            }
        }
        
        // Update animator parameters
        animator.SetBool("isWalking", isMoving);
        animator.SetBool("isWalkingBackward", false);
    }
    
    public void OnPlayerAttack()
    {
        Debug.Log("attackLogged");
        isPlayerAttacking = true;
        Invoke("ResetPlayerAttacking", 1f); // Reset the flag after 1 second
        Invoke("ResetBlocking", 0.3f);
    }
    
    private void ResetPlayerAttacking()
    {
        isPlayerAttacking = false;
    }
    public void ResetEnemyAttacking()
    {
        isAttacking = false;
        contactMade = false;
    }
    private void ResetBlocking()
    {
        isBlocking = false;
    }
}