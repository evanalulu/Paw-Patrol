using UnityEngine;
using System.Collections;

public class FiretruckPusher : MonoBehaviour
{
    public float pushSpeed = 6f;
    public float pushDuration = 1.2f;
    
    // Second box collider stuff
    private BoxCollider playerCollider;
    
    void Start()
    {
        CreatePlayerCollider();
    }
    
    private void CreatePlayerCollider()
    {
        // Get the existing trigger collider
        BoxCollider triggerCollider = GetComponent<BoxCollider>();
        
        if (triggerCollider == null)
        {
            Debug.LogError("🚒 FiretruckPusher: No BoxCollider found on the firetruck!");
            return;
        }
        
        // Add a second box collider for player collision
        playerCollider = gameObject.AddComponent<BoxCollider>();
        playerCollider.size = triggerCollider.size;
        playerCollider.center = triggerCollider.center;
        
        playerCollider.size = new Vector3(
            playerCollider.size.x * 0.9f,
            playerCollider.size.y * 0.9f,
            playerCollider.size.z
        );
        
        playerCollider.isTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        PullOverCar car = other.GetComponent<PullOverCar>();

        if (car != null)
        {
            Debug.Log($"🚓 Firetruck requesting pull-over from: {other.name}");
            car.PullOver();
        }
    }
    
    IEnumerator ReenablePlayerCollider()
    {
        // Wait a short time for the pull-over to complete
        yield return new WaitForSeconds(1.5f);
    }
}