using UnityEngine;

public class CuchilloScript : MonoBehaviour
{

    private EnemigoScript enemigo = null;

    [SerializeField] AudioClip sonidoCuchillo;
    [SerializeField] AudioClip sonidoCuchilloMatando;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Disparar()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (enemigo != null)
            {
                enemigo.vida -= 100;

                enemigo.CooldownAtaque = Time.time + 2;

                audioSource.PlayOneShot(sonidoCuchilloMatando);

                if (enemigo.vida < 0)
                {
                    Destroy(enemigo.gameObject);
                }
            } else
            {
                audioSource.PlayOneShot(sonidoCuchillo);
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
