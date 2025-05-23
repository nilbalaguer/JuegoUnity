using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HistoriaScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        try
        {

            GameObject.Find("Puntuacion").GetComponent<TextMeshProUGUI>().text = GameController.Instance.puntuacionJuego + " pts totales";
        }
        catch (System.Exception)
        {
            Debug.Log("No se encontro puntuacion");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jugar();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("PantallaInicio");
        }
    }
    
    public void Jugar(){
        //SceneManager.LoadScene("NIVELANIOL_prueba");
        //SceneManager.LoadScene("NIVELANIOL");
        SceneManager.LoadScene("Nivel1_Final");
    }
}
