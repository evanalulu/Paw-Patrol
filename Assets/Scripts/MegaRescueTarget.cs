using UnityEngine;

public class MegaRescueTarget : MonoBehaviour
{
    private int hitCount = 0;

    [Header("Hit Rules")]
    public int ballHitsRequired = 5;
    public int boneValue = 3;          // each bone counts as 3
    public int rescueScore = 50;

    public GameObject rescueEffect;

    public void TakeHit(string projectileType)
    {
        hitCount += (projectileType == "Bone") ? boneValue : 1;

        Debug.Log($"Mega rescue hit! Progress: {hitCount}/{ballHitsRequired}");

        if (hitCount >= ballHitsRequired)
        {
            Debug.Log("🎉 MEGA rescue saved!");

            GameUIController ui = FindObjectOfType<GameUIController>();
            ui?.AddScore(rescueScore);

            // Play effect
            if (rescueEffect != null)
            {
                GameObject fx = Instantiate(rescueEffect, transform.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity);
                fx.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                Destroy(fx, 0.417f);
            }
            
            // Play pet collect sound at higher volume only when mega rescue is completed
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            audioManager?.PlayPetCollectSound();

            Destroy(gameObject, 0.3f);
        }
    }
}