using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 4f;
    public int vida = 100;
    public TMP_Text TextComponent;
    private Rigidbody2D rb;

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
        transform.Translate(velocidad * Time.deltaTime);
    }

    void Disparar()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direccion = (mousePos - transform.position).normalized;

            GameObject bala = Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
            Rigidbody2D rbBala = bala.GetComponent<Rigidbody2D>();
            rbBala.linearVelocity = direccion * fuerzaDisparo;
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
}
