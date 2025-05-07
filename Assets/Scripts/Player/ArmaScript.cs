using UnityEngine;
using TMPro;

public class ArmaScript : MonoBehaviour
{
    public UnityEngine.UI.Image barraCooldown;
    public int municion = 20;
    public GameObject balaPrefab;
    public float fuerzaDisparo = 10f;
    public Transform puntoDisparo;

    public float tiempoEntreAtaques = 2f;
    private float proximoAtaque = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ActualizarBarraCooldown();
    }

    //Se dispara hacia donde mira el jugador
    public void Disparar()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (municion > 0 && Time.time >= proximoAtaque)
            {
                municion --;
                GameObject.Find("Municion").GetComponent<TextMeshProUGUI>().text = "" + municion;

                for (int i = 0; i < 6; i++)
                {
                    float anguloAleatorio = Random.Range(-10f, 10f);
                    Quaternion rotacionDisparo = Quaternion.Euler(0, 0, anguloAleatorio);
                    Vector2 direccion = rotacionDisparo * transform.right;

                    GameObject bala = Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
                    Rigidbody2D rbBala = bala.GetComponent<Rigidbody2D>();
                    rbBala.linearVelocity = direccion.normalized * fuerzaDisparo;
                }

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
