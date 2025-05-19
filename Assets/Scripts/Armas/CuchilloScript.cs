using UnityEngine;

public class CuchilloScript : MonoBehaviour
{

    private EnemigoScript enemigo = null;
    private EnemigoEscopetaScript enemigoEscopeta = null;

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
                enemigo.RecivirDano(100);

                enemigo.CooldownAtaque = Time.time + 2;

                audioSource.PlayOneShot(sonidoCuchilloMatando);
            }
            else if (enemigoEscopeta != null)
            {
                enemigoEscopeta.RecivirDano(100);

                enemigoEscopeta.CooldownAtaque = Time.time + 2;

                audioSource.PlayOneShot(sonidoCuchilloMatando);
            }
            else
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
            enemigoEscopeta = other.GetComponent<EnemigoEscopetaScript>();
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
