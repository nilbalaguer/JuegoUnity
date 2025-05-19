using System.Runtime.CompilerServices;
using UnityEngine;

public class BalaScript : MonoBehaviour
{
    [SerializeField] AudioClip reboteBala;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pared"))
        {
            audioSource.PlayOneShot(reboteBala);

            Destroy(gameObject);
        }
    }
}