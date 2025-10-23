using UnityEngine;

public class RescueTarget : MonoBehaviour
{
    private int hitCount = 0;
    public GameObject rescueEffect;

    public void TakeHit(string projectileType)
    {
        hitCount += (projectileType == "Bone") ? 2 : 1;

        if (hitCount >= 2) // 2 Ball hits or 1 Bone hit
        {
            Debug.Log("🐾 Pet rescued!");
            if (rescueEffect != null)
            {
                Quaternion fxRotation = Quaternion.Euler(90f, 180f, 0f);
                GameObject fx = Instantiate(rescueEffect, transform.position + new Vector3(0f, 0.1f, 0f), fxRotation);
                Destroy(fx, 0.417f);
            }

            Destroy(gameObject, 0.3f);
        }
    }
}