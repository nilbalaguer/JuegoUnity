using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    private int puntuacionJuego;

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
        Debug.Log("Juego Instanciado");
    }

    void Update()
    {

    }


    public void SumarPuntuacion(int suma)
    {
        puntuacionJuego += suma;
    }

    public int ObtenerPuntuacion()
    {
        return puntuacionJuego;
    }

    public void BorrarPuntuacion()
    {
        puntuacionJuego = 0;
    }
}
