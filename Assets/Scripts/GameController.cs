using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    private int puntuacionJuego;

    private float tiempoParaContarEnemigos = -1f;
    private bool comprobarEnemigos = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        puntuacionJuego = 0;
        Debug.Log("GameController Instanciado");
    }

    void Update()
    {
        if (comprobarEnemigos && Time.time >= tiempoParaContarEnemigos)
        {
            comprobarEnemigos = false;

            int cantidadEnemigos = GameObject.FindGameObjectsWithTag("Enemigo").Length;
            Debug.Log("Cantidad enemigos: " + cantidadEnemigos);

            if (cantidadEnemigos <= 0)
            {
                Debug.Log("Cambiar de escena");
            }
        }
    }

    public void SumarPuntuacion(int suma)
    {
        puntuacionJuego += suma;

        GameObject.Find("DisplayPuntos").GetComponent<TextMeshProUGUI>().text = puntuacionJuego + " pts";

        tiempoParaContarEnemigos = Time.time + 1f;
        comprobarEnemigos = true;
    }

    public int ObtenerPuntuacion()
    {
        return puntuacionJuego;
    }

    public void BorrarPuntuacion()
    {
        puntuacionJuego = 0;

        GameObject.Find("DisplayPuntos").GetComponent<TextMeshProUGUI>().text = puntuacionJuego + " pts";
    }
}
