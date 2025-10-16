using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TextureScroller : MonoBehaviour
{
    public float scrollSpeedY = 0.2f;   // + = up screen, - = down screen
    public float scrollSpeedX = 0f;

    Renderer rend;
    Vector2 offset;  // persistent so it doesn’t snap every frame

    void Awake()
    {
        rend = GetComponent<Renderer>();
        // material instance so we don’t modify the shared asset
        rend.material = new Material(rend.material);
    }

    void Update()
    {
        // scrolly scrolly
        offset.x += scrollSpeedX * Time.deltaTime;
        offset.y += scrollSpeedY * Time.deltaTime;
        rend.material.mainTextureOffset = offset; // apply new offset to texture
    }
}