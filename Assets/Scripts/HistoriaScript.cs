using UnityEngine;
using UnityEngine.SceneManagement;

public class HistoriaScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jugar();
        }

        if (Input.GetKeyDown(KeyCode.A))
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
