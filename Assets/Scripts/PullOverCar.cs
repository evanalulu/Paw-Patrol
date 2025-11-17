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
    }

    public void PullOver()
    {
        if (isPullingOver) return; // already moving
        StartCoroutine(PullOverRoutine());
    }

    IEnumerator PullOverRoutine()
    {
        isPullingOver = true;

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

        isPullingOver = false;
    }
}