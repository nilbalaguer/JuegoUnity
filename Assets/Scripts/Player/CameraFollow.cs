using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objetivo;
    public Vector3 offset = new Vector3(0, 0, -10);

    [Header("Shake")]
    public float duracionSacudida = 0.2f;
    public float magnitudSacudida = 0.2f;

    private float tiempoSacudidaRestante = 0f;

    [Header("Apuntado Arma")]
    public float maxMouseOffset = 5f;
    public KeyCode shiftKey = KeyCode.LeftShift;

    [Header("Suavizado")]
    public float tiempoSuavizadoNormal = 0.05f;         
    public float tiempoSuavizadoConShift = 0.2f;      
    private Vector3 velocidadSuavizado = Vector3.zero;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (cam == null || objetivo == null) return;

        Vector3 destino = objetivo.position + offset;

        // Apuntar
        if (Input.GetKey(shiftKey))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(offset.z);

            Vector3 mouseWorld = cam.ScreenToWorldPoint(mousePos);
            Vector3 direccion = (mouseWorld - objetivo.position).normalized;
            float distancia = Mathf.Min(Vector3.Distance(mouseWorld, objetivo.position), maxMouseOffset);

            destino += direccion * distancia;
        }

        // Apuntar arma suave
        float suavizadoActual = Input.GetKey(shiftKey) ? tiempoSuavizadoConShift : tiempoSuavizadoNormal;
        Vector3 posicionSuavizada = Vector3.SmoothDamp(transform.position, destino, ref velocidadSuavizado, suavizadoActual);

        if (tiempoSacudidaRestante > 0f)
        {
            float x = Random.Range(-1f, 1f) * magnitudSacudida;
            float y = Random.Range(-1f, 1f) * magnitudSacudida;
            posicionSuavizada += new Vector3(x, y, 0f);
            tiempoSacudidaRestante -= Time.deltaTime;
        }

        transform.position = posicionSuavizada;
    }

    public void Sacudir(float magnitud)
    {
        magnitudSacudida = magnitud;
        tiempoSacudidaRestante = duracionSacudida;
    }
}
