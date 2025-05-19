using UnityEngine;
using TMPro;

public class CarbinaM4Script : MonoBehaviour
{
    public UnityEngine.UI.Image barraCooldown;
    public int municion = 120;
    public GameObject balaPrefab;
    public float fuerzaDisparo = 10f;
    public Transform puntoDisparo;

    public float tiempoEntreAtaques = 0.3f;
    private float proximoAtaque = 0.3f;

    public AudioClip sonido;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ActualizarBarraCooldown();
    }

    //Se dispara hacia donde mira el jugador
    public void Disparar()
    {
        if (Input.GetButton("Fire1"))
        {
            if (municion > 0 && Time.time >= proximoAtaque)
            {
                municion --;
                GameObject.Find("Municion").GetComponent<TextMeshProUGUI>().text = "" + municion;

                Vector2 direccion = transform.right;

                GameObject bala = Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
                Rigidbody2D rbBala = bala.GetComponent<Rigidbody2D>();
                rbBala.linearVelocity = direccion.normalized * fuerzaDisparo;

                audioSource.PlayOneShot(sonido);

                proximoAtaque = Time.time + tiempoEntreAtaques;
            }
        }
    }

    void ActualizarBarraCooldown()
    {
        float progreso = Mathf.Clamp01((Time.time - (proximoAtaque - tiempoEntreAtaques)) / tiempoEntreAtaques);
        barraCooldown.fillAmount = progreso;
    }
}
