using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Microsoft.Unity.VisualStudio.Editor;

/*
    AÑADIR PUNTUACION
*/

public class PlayerMovement : MonoBehaviour
{
    public float speed = 4f;
    public int vida = 100;
    public Rigidbody2D rb;
    private EscopetaScript escopeta;
    private CuchilloScript cuchillo;
    private CarbinaM4Script carabinaM4;
    private PistolaGlockScript pistolaGlock;
    [SerializeField] GameObject gameManager;
    public int armaSeleccionada;
    private ArmaSueloScript armaCercana = null;

    public int puntosNivel;

    [SerializeField] Transform puntoRespawn;

    [SerializeField] Collider2D cuchilloColision;

    [SerializeField] Sprite[] ArmasEnHud;

    void Start()
    {
        escopeta = GetComponentInChildren<EscopetaScript>();
        cuchillo = GetComponentInChildren<CuchilloScript>();
        carabinaM4 = GetComponentInChildren<CarbinaM4Script>();
        pistolaGlock = GetComponentInChildren<PistolaGlockScript>();

        //Arma seleccionada por defecto
        armaSeleccionada = 0;

        //Poner puntos a 0
        puntosNivel = 0;
    }

    void Update()
    {
        switch (armaSeleccionada)
        {
            case 1:
                escopeta.Disparar();
                break;

            case 2:
                carabinaM4.Disparar();
                break;

            case 3:
                pistolaGlock.Disparar();
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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ArmaSuelo"))
        {
            armaCercana = other.GetComponent<ArmaSueloScript>();

        }
        else if (other.CompareTag("Municion"))
        {
            escopeta.municion += 10;
            pistolaGlock.municion += 10;
            carabinaM4.municion += 15;

            Destroy(other.gameObject);
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

        GameObject.Find("ArmaSeleccionada").GetComponent<UnityEngine.UI.Image>().sprite = ArmasEnHud[armaSeleccionada];

        armasuelo.ActualizarSprite();

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
