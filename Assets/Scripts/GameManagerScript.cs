using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] GameObject pantallaMuerte;
    [SerializeField] GameObject canvaNormal;
    [SerializeField] GameObject menuPausa;

    [SerializeField] AudioClip wastedSound;

    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        pantallaMuerte.SetActive(false);
        canvaNormal.SetActive(true);
        menuPausa.SetActive(false);

        GameObject.Find("DisplayPuntos").GetComponent<TextMeshProUGUI>().text = 0 + " pts";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MenuPausa() {
        Debug.Log("Menu de pausa");
        canvaNormal.SetActive(false);
        menuPausa.SetActive(true);

        Time.timeScale = 0f;
    }

    public void JugadorMuerto() {
        pantallaMuerte.SetActive(true);
        canvaNormal.SetActive(false);

        Time.timeScale = 0f;

        audioSource.PlayOneShot(wastedSound);
    }

    public void Reintentar() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Resumen() {
        menuPausa.SetActive(false);
        canvaNormal.SetActive(true);
        Time.timeScale = 1f;
    }

    public void MenuPrincipal() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("PantallaInicio");
    }
}
