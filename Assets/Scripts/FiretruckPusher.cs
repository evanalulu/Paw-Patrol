using UnityEngine;
using System.Collections;

public class FiretruckPusher : MonoBehaviour
{
    public float pushSpeed = 6f;
    public float pushDuration = 1.2f;

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
}