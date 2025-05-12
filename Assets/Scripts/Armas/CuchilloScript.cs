using UnityEngine;

public class CuchilloScript : MonoBehaviour
{

    private EnemigoScript enemigo = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Disparar()
    {
        if (Input.GetButtonDown("Fire1") && enemigo != null)
        {
            enemigo.vida -= 20;

            enemigo.CooldownAtaque = Time.time + 2;

            if (enemigo.vida < 0)
            {
                Destroy(enemigo.gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemigo"))
        {
            enemigo = other.GetComponent<EnemigoScript>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemigo"))
        {
            enemigo = null;
        }
    }
}
