using UnityEngine;
using UnityEngine.SceneManagement;

public class TpFinalEscenaScript : MonoBehaviour
{
    [SerializeField] string EscenaParaTp;
    private Transform spriteFlecha;

    [Header("Movimiento de la flecha")]
    [SerializeField] float velocidad = 1f;
    [SerializeField] float amplitud = 0.5f;

    private Vector3 posicionInicial;

    void Start()
    {
        // Buscar el primer hijo como sprite de la flecha
        spriteFlecha = transform.GetChild(0); 
        posicionInicial = spriteFlecha.localPosition;
    }

    void Update()
    {
        if (spriteFlecha != null)
        {
            float offsetY = Mathf.Sin(Time.time * velocidad) * amplitud;
            spriteFlecha.localPosition = new Vector3(posicionInicial.x, posicionInicial.y + offsetY, posicionInicial.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(EscenaParaTp);
        }
    }
}
