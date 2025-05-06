using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 4f;
    public int vida = 100;
    public TMP_Text TextComponent;
    private Rigidbody2D rb;
    public int municion = 20;
    public GameObject balaPrefab;
    public float fuerzaDisparo = 10f;
    public Transform puntoDisparo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Disparar();
    }

    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 velocidad = new Vector2(x, y) * speed;
        rb.linearVelocity = velocidad;

        MirarCursor();
    }

    //Se dispara hacia donde mira el jugador
    void Disparar()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (municion > 0)
            {
                municion --;

                for (int i = 0; i < 6; i++)
                {
                    float anguloAleatorio = Random.Range(-10f, 10f);
                    Quaternion rotacionDisparo = Quaternion.Euler(0, 0, anguloAleatorio);
                    Vector2 direccion = rotacionDisparo * transform.right;

                    GameObject bala = Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
                    Rigidbody2D rbBala = bala.GetComponent<Rigidbody2D>();
                    rbBala.linearVelocity = direccion.normalized * fuerzaDisparo;
                }
            }
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pincho"))
        {
            vida -= 1;
            GameObject.Find("Vidas").GetComponent<TextMeshProUGUI>().text = "" + vida;
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
}
