using UnityEngine;

public class EnemigoScript : MonoBehaviour
{

    public Collider2D hitEnemigo;
    public Collider2D colliderAtaque;
    public Collider2D colliderVision;
    public int speed = 4;
    public int vida = 100;

    public float tiempoEntreAtaques = 2f;
    private float proximoAtaque = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bala"))
        {
            if (other.IsTouching(hitEnemigo))
            {
                vida -= 50; //ce muere
            }
        }

        if (vida <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.IsTouching(colliderAtaque))
            {
                if (Time.time >= proximoAtaque)
                {
                    PlayerMovement player = other.GetComponent<PlayerMovement>();
                    player.RecivirDano(20);
                    proximoAtaque = Time.time + tiempoEntreAtaques;
                }
            }

            if (other.IsTouching(colliderVision))
            {
                PlayerMovement player = other.GetComponent<PlayerMovement>();

                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

                Vector2 direccion = player.transform.position - transform.position;
                float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angulo);
            }
        }
    }
}
