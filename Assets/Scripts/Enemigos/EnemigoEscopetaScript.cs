using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemigoEscopetaScript : MonoBehaviour
{
    [Header("Patrulla")]
    [SerializeField] Transform puntoA;
    [SerializeField] Transform puntoB;
    private Transform destinoActual;

    [Header("Combate")]
    public Collider2D hitEnemigo;
    public Collider2D colliderAtaque;
    public int speed = 4;
    public int vida = 100;
    public float tiempoEntreAtaques = 2f;
    public float CooldownAtaque = 0f;
    private bool jugadorDentro = false;
    [SerializeField] GameObject balaPrefab;
    [SerializeField] Transform puntoDisparo;

    [Header("Persecución")]
    [SerializeField] Transform target;
    [SerializeField] LayerMask obstacleMask;
    public float visionRange = 10f;
    [Tooltip("Ángulo total del cono de visión en grados")]
    [SerializeField] float campoVision = 60f;

    [Header("Memoria de visión")]
    [Tooltip("Segundos que sigue persiguiendo tras perder de vista al jugador")]
    [SerializeField] float tiempoPerdidoMax = 3f;
    private float ultimoVistoTime = Mathf.NegativeInfinity;
    private bool persiguiendo = false;

    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        destinoActual = puntoA;

        agent.speed = speed;

        agent.acceleration = 100f;

        agent.angularSpeed = 20f;
    }

    void Update()
    {
        Vector2 dirAlJugador = target != null
            ? (Vector2)(target.position - transform.position)
            : Vector2.zero;

        bool puedeVer = target != null && TieneVisionDirecta() && EstaEnCampoDeVision(dirAlJugador);

        if (puedeVer)
        {
            persiguiendo = true;
            ultimoVistoTime = Time.time;
        }
        else if (persiguiendo && Time.time - ultimoVistoTime <= tiempoPerdidoMax)
        {
            puedeVer = true;
        }
        else
        {
            persiguiendo = false;
            puedeVer = false;
        }

        if (persiguiendo)
        {
            float distanciaJugador = Vector2.Distance(transform.position, target.position);

            if (distanciaJugador > 4)
            {
                agent.SetDestination(target.position);
                agent.speed = speed;
            } else {
                agent.ResetPath();
            }
        }
        else
        {
            agent.speed =  speed/2.5f;
            agent.SetDestination(destinoActual.position);
            float distancia = Vector2.Distance(transform.position, destinoActual.position);
            if (distancia < 1.7f)
                destinoActual = destinoActual == puntoA ? puntoB : puntoA;
        }

        Vector2 movimiento = agent.velocity;
        if (movimiento.sqrMagnitude > 0.01f)
        {
            float angulo = Mathf.Atan2(movimiento.y, movimiento.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angulo);
        }
    }

    bool TieneVisionDirecta()
    {
        Vector2 direction = target.position - transform.position;
        float distance = direction.magnitude;
        if (distance > visionRange) return false;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction.normalized,
            distance,
            obstacleMask
        );
        return hit.collider == null;
    }

    bool EstaEnCampoDeVision(Vector2 direccionJugador)
    {
        Vector2 forward = transform.right;
        float angle = Vector2.Angle(forward, direccionJugador.normalized);
        return angle <= campoVision * 0.5f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bala") && other.IsTouching(hitEnemigo))
        {
            vida -= 100;
            if (vida <= 0)
                Destroy(gameObject);

            Destroy(other.gameObject);
        }

        if (other.CompareTag("Player") && other.IsTouching(colliderAtaque))
        {
            jugadorDentro = true;
            // Espera 1 segundo antes de permitir el primer ataque
            CooldownAtaque = Time.time + 0.5f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (jugadorDentro && other.CompareTag("Player") && other.IsTouching(colliderAtaque) && persiguiendo)
        {
            if (Time.time >= CooldownAtaque)
            {
                for (int i = 0; i < 6; i++)
                {
                    float anguloAleatorio = UnityEngine.Random.Range(-20f, 20f);
                    Quaternion rotacionDisparo = Quaternion.Euler(0, 0, anguloAleatorio);
                    Vector2 direccion = rotacionDisparo * transform.right;

                    GameObject bala = Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
                    Rigidbody2D rbBala = bala.GetComponent<Rigidbody2D>();
                    rbBala.linearVelocity = direccion.normalized * 30; //El ultimo valor es la velocidad del disparo
                }

                CooldownAtaque = Time.time + tiempoEntreAtaques;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position);
        }

        Gizmos.color = new Color(1, 1, 0, 0.3f);
        Vector3 leftDir = Quaternion.Euler(0, 0, -campoVision * 0.5f) * transform.right;
        Vector3 rightDir = Quaternion.Euler(0, 0, campoVision * 0.5f) * transform.right;
        Gizmos.DrawRay(transform.position, leftDir.normalized * visionRange);
        Gizmos.DrawRay(transform.position, rightDir.normalized * visionRange);
    }
}
