using UnityEngine;

public class CarLooper : MonoBehaviour
{
    private float speed;
    private float despawnZ;
    private float respawnZ;

    public void Init(float carSpeed, float despawnLimit, float respawnPoint)
    {
        speed = carSpeed;
        despawnZ = despawnLimit;
        respawnZ = respawnPoint;
    }

    void Update()
    {
        // Move car downward (along Z)
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);

        if (transform.position.z < despawnZ)
        {
            // Pick lane to not reappear in same col
            float laneX = Random.Range(-0.9f, 0.9f);
            // Move back to top of road
            transform.position = new Vector3(laneX, 0.16f, respawnZ);
        }
    }
}