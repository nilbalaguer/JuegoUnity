using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] GameObject pantallaMuerte;
    [SerializeField] GameObject canvaNormal;
    [SerializeField] GameObject menuPausa;
    [SerializeField] GameObject levelClearedMessage;

    [SerializeField] AudioClip wastedSound;

    [SerializeField] GameObject tpFinalEscena;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        pantallaMuerte.SetActive(false);
        canvaNormal.SetActive(true);
        menuPausa.SetActive(false);

        tpFinalEscena.SetActive(false);

        GameObject.Find("DisplayPuntos").GetComponent<TextMeshProUGUI>().text = 0 + " pts";

        if (GameController.Instance == null)
        {
            SceneManager.LoadScene("PantallaInicio");
        }

        if (levelClearedMessage != null)
        {
            levelClearedMessage.SetActive(false);
        }
    }

    public void MenuPausa()
    {
        Debug.Log("Menu de pausa");
        canvaNormal.SetActive(false);
        menuPausa.SetActive(true);

        Time.timeScale = 0f;
    }

    public void JugadorMuerto()
    {
        pantallaMuerte.SetActive(true);
        canvaNormal.SetActive(false);

        Time.timeScale = 0f;

        GameController.Instance.BorrarPuntuacion();

        audioSource.PlayOneShot(wastedSound);
    }

    public void Reintentar()
    {
        GameController.Instance.BorrarPuntuacion();

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Resumen()
    {
        menuPausa.SetActive(false);
        canvaNormal.SetActive(true);
        Time.timeScale = 1f;
    }

    public void MenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("PantallaInicio");
    }

    public void MostrarLevelClearedMessage()
    {
        if (levelClearedMessage != null)
        {
            levelClearedMessage.SetActive(true);
            tpFinalEscena.SetActive(true);

        }
        else
        {
            Debug.LogWarning("LevelClearedMessage no está asignado en el Inspector.");
        }
    }
}
