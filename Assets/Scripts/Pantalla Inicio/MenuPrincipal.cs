using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuPrincipal : MonoBehaviour
{
    [SerializeField] GameObject panelControlesBool;
    void Start(){
        panelControlesBool.SetActive(false);
    }
    public void Salir(){
        Debug.Log("Salir...");
        Application.Quit();
    }
    public void Jugar(){
        SceneManager.LoadScene("Piso1NivelAniol");
    }
    public void Controles(){
        Debug.Log("Controles");
        panelControlesBool.SetActive(true);
    }
    public void salirControles(){
        Debug.Log("Salir Controles");
        panelControlesBool.SetActive(false);
    }
}