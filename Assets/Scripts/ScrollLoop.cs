using UnityEngine;

public class ScrollLoop : MonoBehaviour
{
    public float speed = 2f;
    private float spriteHeight;
    private Vector3 startPos;

    void Start()
    {
        // Get original position and sprite height
        startPos = transform.position;
        spriteHeight = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        // Move downward each frame
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // Once background moves past its height, move it back up
        if (transform.position.y < startPos.y - spriteHeight)
        {
            transform.position = new Vector3(
                startPos.x,
                startPos.y + spriteHeight * 2f,
                startPos.z
            );
        }
    }
}