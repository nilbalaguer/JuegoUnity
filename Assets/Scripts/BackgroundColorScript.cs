using UnityEngine;

public class BackgroundColorScript : MonoBehaviour
{
    [Range(0f, 1f)]
    public float saturation = 1f;
    [Range(0f, 1f)]
    public float value = 1f;
    public float hueSpeed = 0.1f; // Velocidad del cambio de color

    private float hue = 0f;

    void Update()
    {
        hue += hueSpeed * Time.deltaTime;
        if (hue > 1f) hue -= 1f; // Mantener hue entre 0 y 1

        Color color = Color.HSVToRGB(hue, saturation, value);
        Camera.main.backgroundColor = color;
    }
}
