using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    public Animator animator;

    public int puntosNivel;

    [SerializeField] Transform puntoRespawn;
    [SerializeField] Collider2D cuchilloColision;
    [SerializeField] Sprite[] ArmasEnHud;

    [SerializeField] Sprite[] SpritesJugador;
    private SpriteRenderer spriteRenderer;

    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();

        escopeta = GetComponentInChildren<EscopetaScript>();
        cuchillo = GetComponentInChildren<CuchilloScript>();
        carabinaM4 = GetComponentInChildren<CarbinaM4Script>();
        pistolaGlock = GetComponentInChildren<PistolaGlockScript>();

        //Obtener Animator desde el objeto hijo "Legs"
        animator = transform.Find("Legs").GetComponent<Animator>();


        armaSeleccionada = 0;
        puntosNivel = 0;

        int armaController = GameController.Instance.armaSeleccionada;
        if (armaController != -1)
        {
            armaSeleccionada = armaController;
        }

        GameObject.Find("ArmaSeleccionada").GetComponent<UnityEngine.UI.Image>().sprite = ArmasEnHud[armaSeleccionada];

        ActualizarSpriteArmaPlayer();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        bool estaMoviendose = (x != 0 || y != 0);
        animator.SetBool("isMoving", estaMoviendose);

        if (Time.timeScale > 0)
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
        }

        if (armaCercana != null && Input.GetButtonDown("Fire2"))
        {
            CambiarArma(armaCercana);
            armaCercana = null;
        }

        if (Input.GetKeyDown("escape"))
        {
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
        GameController.Instance.armaSeleccionada = armaSeleccionada;

        cuchilloColision.enabled = (armaSeleccionada == 0);

        switch (armaSeleccionada)
        {
            case 1:
                escopeta.ActualizarMunicion();
                spriteRenderer.sprite = SpritesJugador[armaSeleccionada];
                break;
            case 2:
                carabinaM4.ActualizarMunicion();
                spriteRenderer.sprite = SpritesJugador[armaSeleccionada];
                break;
            case 3:
                pistolaGlock.ActualizarMunicion();
                spriteRenderer.sprite = SpritesJugador[armaSeleccionada];
                break;
            case 0:
            default:
                cuchillo.ActualizarMunicion();
                spriteRenderer.sprite = SpritesJugador[armaSeleccionada];
                break;
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

        if (vida <= 0)
        {
            gameManager.GetComponent<GameManagerScript>().JugadorMuerto();
        }
    }

    public void RespawnPlayer()
    {
        gameObject.transform.position = puntoRespawn.position;
        vida = 100;
    }

    public void ActualizarSpriteArmaPlayer()
    {
        switch (armaSeleccionada)
        {
            case 1:
                escopeta.ActualizarMunicion();
                spriteRenderer.sprite = SpritesJugador[armaSeleccionada];
                break;
            case 2:
                carabinaM4.ActualizarMunicion();
                spriteRenderer.sprite = SpritesJugador[armaSeleccionada];
                break;
            case 3:
                pistolaGlock.ActualizarMunicion();
                spriteRenderer.sprite = SpritesJugador[armaSeleccionada];
                break;
            case 0:
            default:
                cuchillo.ActualizarMunicion();
                spriteRenderer.sprite = SpritesJugador[armaSeleccionada];
                break;
        }
    }
}
