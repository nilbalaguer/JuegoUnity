using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemigoScript : MonoBehaviour
{
    [Header("Instanciar al morir")]
    [SerializeField] GameObject sangre;
    [SerializeField] GameObject armaSuelo;

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

        // agent.angularSpeed = 0.1f;
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
            agent.SetDestination(target.position);
            agent.speed = speed;
        }
        else
        {
            agent.speed = speed / 2.5f;
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
            RecivirDano(100);

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
        if (jugadorDentro && other.CompareTag("Player") && other.IsTouching(colliderAtaque))
        {
            if (Time.time >= CooldownAtaque)
            {
                PlayerMovement player = other.GetComponent<PlayerMovement>();
                player.RecivirDano(100);
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
    
    public void RecivirDano(int reduccion)
    {
        vida -= reduccion;

        if (vida <= 0)
        {
            Instantiate(sangre, transform.position, Quaternion.identity);


            GameObject armaGO = Instantiate(armaSuelo, transform.position, Quaternion.identity);
            ArmaSueloScript armaScript = armaGO.GetComponent<ArmaSueloScript>();

            if (armaScript != null)
            {
                armaScript.tipoArma = 0;
            }

            Destroy(gameObject);
            
            GameController.Instance.SumarPuntuacion(40);
        }
    }
}
