using UnityEngine;

public class BalaScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pared") || collision.CompareTag("Enemigo"))
        {
            Destroy(gameObject);
        }
    }
}