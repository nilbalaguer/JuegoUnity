using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuPrincipal : MonoBehaviour
{
    public void Salir(){
        Debug.Log("Salir...");
        Application.Quit();

    }
    public void Jugar(){
        SceneManager.LoadScene("SampleScene");
    }
}
