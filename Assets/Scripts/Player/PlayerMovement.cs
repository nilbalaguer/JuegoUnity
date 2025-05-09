using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 4f;
    public int vida = 100;
    public TMP_Text TextComponent;
    private Rigidbody2D rb;
    private EscopetaScript escopeta;
    private CuchilloScript cuchillo;
    public int armaSeleccionada;
    private ArmaSueloScript armaCercana = null;

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
            ActualizarVida();
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
    }

    void CambiarArma(ArmaSueloScript armasuelo)
    {
        int armaTemp = armaSeleccionada;
        armaSeleccionada = armasuelo.tipoArma;
        armasuelo.tipoArma = armaTemp;

        GameObject.Find("ArmaSeleccionada").GetComponent<TextMeshProUGUI>().text = "Arma: " + armaSeleccionada;

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
        ActualizarVida();
    }

    void ActualizarVida()
    {
        GameObject.Find("Vidas").GetComponent<TextMeshProUGUI>().text = "" + vida;
    }
}
