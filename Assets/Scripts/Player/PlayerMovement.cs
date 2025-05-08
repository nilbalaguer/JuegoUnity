using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 4f;
    public int vida = 100;
    public TMP_Text TextComponent;
    private Rigidbody2D rb;
    private ArmaScript arma;

    private EnemigoScript enemigoEnRango;
    private bool enContactoConEnemigo = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        arma = GetComponentInChildren<ArmaScript>();
    }

    void Update()
    {
        arma.Disparar();

        if (Input.GetButtonDown("Jump") && enContactoConEnemigo && enemigoEnRango != null)
        {
            enemigoEnRango.vida -= 70;
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
            arma.municion += 20;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemigo"))
        {
            enemigoEnRango = other.GetComponent<EnemigoScript>();
            enContactoConEnemigo = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemigo"))
        {
            enemigoEnRango = null;
            enContactoConEnemigo = false;
        }
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
