using UnityEngine;
using System.Collections;

public class PullOverCar : MonoBehaviour
{
    public float laneShiftSpeed = 3f;

    private bool isPullingOver = false;
    private CarLooper looper;
    private Rigidbody rb;
    private LevelConfig config;

    void Start()
    {
        looper = GetComponent<CarLooper>();
        rb = GetComponent<Rigidbody>();
        config = Resources.Load<LevelConfig>(PlayerPrefs.GetString("SelectedLevel"));
        
        // Ensure firetruck has the correct tag for player collision
        if (gameObject.name.Contains("Firetruck") && !CompareTag("Firetruck"))
        {
            gameObject.tag = "Firetruck";
            Debug.Log($"🚒 Set tag to 'Firetruck' for {gameObject.name}");
        }
    }

    public void PullOver()
    {
        if (isPullingOver) return; // already moving
        StartCoroutine(PullOverRoutine());
    }

    IEnumerator PullOverRoutine()
    {
        isPullingOver = true;

        // Disable collider during lane change to prevent unwanted collisions
        Collider carCollider = GetComponent<Collider>();
        if (carCollider != null)
        {
            carCollider.enabled = false;
        }

        // Pick nearest free lane (left or right)
        int newLane = looper.currentLaneIndex;

        if (newLane > 0) newLane--; // try left
        else if (newLane < config.lanePositions.Length - 1) newLane++; // try right

        looper.currentLaneIndex = newLane;

        Vector3 targetPos = new Vector3(config.lanePositions[newLane], transform.position.y, transform.position.z);

        // Smooth lane slide
        while (Mathf.Abs(transform.position.x - targetPos.x) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, laneShiftSpeed * Time.deltaTime);
            yield return null;
        }

        // Re-enable collider after lane change is complete
        if (carCollider != null)
        {
            carCollider.enabled = true;
            Debug.Log($"🚒 Firetruck collider re-enabled after pull-over");
        }

        isPullingOver = false;
        
        // Double-check that collider is enabled after a short delay
        StartCoroutine(VerifyColliderEnabled());
    }
    
    IEnumerator VerifyColliderEnabled()
    {
        // Wait a short time to ensure everything has settled
        yield return new WaitForSeconds(0.5f);
        
        Collider carCollider = GetComponent<Collider>();
        if (carCollider != null && !carCollider.enabled)
        {
            carCollider.enabled = true;
            Debug.Log($"🚒 Firetruck collider re-enabled during verification check");
        }
    }
}