using UnityEngine;

public class RescueTarget : MonoBehaviour
{
    private int hitCount = 0;
    public GameObject rescueEffect;

    public void TakeHit(string projectileType)
    {
        hitCount += (projectileType == "Bone") ? 3 : 1;

        if (hitCount >= 3) // 3 Ball hits or 1 Bone hit
        {
            Debug.Log("🐾 Pet rescued!");
            if (rescueEffect != null)
            {
                GameObject fx = Instantiate(rescueEffect, transform.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity);
                fx.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                Destroy(fx, 0.417f);
            }
            
            // Play pet collect sound at higher volume only when pet is actually rescued
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            audioManager?.PlayPetCollectSound();

            Destroy(gameObject, 0.3f);
        }
    }
}