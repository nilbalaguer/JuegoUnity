using UnityEngine;
using TMPro;
using System.Net;

/*
    AÑADIR PUNTUACION
*/

public class PlayerMovement : MonoBehaviour
{
    public float speed = 4f;
    public int vida = 100;
    private Rigidbody2D rb;
    private EscopetaScript escopeta;
    private CuchilloScript cuchillo;
    [SerializeField] GameObject gameManager;
    public int armaSeleccionada;
    private ArmaSueloScript armaCercana = null;

    [SerializeField] Transform puntoRespawn;

    [SerializeField] Collider2D cuchilloColision;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        escopeta = GetComponentInChildren<EscopetaScript>();
        cuchillo = GetComponentInChildren<CuchilloScript>();

        //Arma seleccionada por defecto
        armaSeleccionada = 0;

        GameObject.Find("ArmaSeleccionada").GetComponent<TextMeshProUGUI>().text = "Arma: " + armaSeleccionada;
    }

    void Update()
    {
        switch (armaSeleccionada)
        {
            case 1:
                escopeta.Disparar();
                break;

            case 0:
            default:
                cuchillo.Disparar();
                break;
        }

        //cambiar por la arma que esta en el suelo
        if (armaCercana != null && Input.GetButtonDown("Fire2"))
        {
            CambiarArma(armaCercana);
            armaCercana = null;
        }

        //Menu de pausa
        if(Input.GetKeyDown("escape")) {
            gameManager.GetComponent<GameManagerScript>().MenuPausa();
        }
    }

    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 velocidad = new Vector2(x, y) * speed;
        rb.linearVelocity = velocidad;

        MirarCursor();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pincho"))
        {
            vida -= 1;
        }
        else if (collision.gameObject.CompareTag("Municion"))
        {
            escopeta.municion += 20;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ArmaSuelo"))
        {
            armaCercana = other.GetComponent<ArmaSueloScript>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ArmaSuelo"))
        {
            if (other.GetComponent<ArmaSueloScript>() == armaCercana)
            {
                armaCercana = null;
            }
        }

        if (other.CompareTag("Bala"))
        {
            RecivirDano(100);

            Destroy(other.gameObject);
        }
    }

    void CambiarArma(ArmaSueloScript armasuelo)
    {
        int armaTemp = armaSeleccionada;
        armaSeleccionada = armasuelo.tipoArma;
        armasuelo.tipoArma = armaTemp;

        GameObject.Find("ArmaSeleccionada").GetComponent<TextMeshProUGUI>().text = "Arma: " + armaSeleccionada;

        // Activar o desactivar colisión del cuchillo según el arma equipada
        cuchilloColision.enabled = (armaSeleccionada == 0);
    }

    void MirarCursor()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 direccion = mousePos - transform.position;
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angulo);
    }

    public void RecivirDano(int dano)
    {
        vida -= dano;

        if (vida <= 0)
        {
            gameManager.GetComponent<GameManagerScript>().JugadorMuerto();
        }

    }

    public void RespawnPlayer() {
        //Hace TP al punto de respawn
        gameObject.transform.position = puntoRespawn.position;

        vida = 100;
    }
}
