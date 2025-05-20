using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform objetivo;
    public Vector3 offset = new Vector3(0, 0, -10);

    [Header("Shake")]
    public float duracionSacudida = 0.2f;
    public float magnitudSacudida = 0.2f;

    private float tiempoSacudidaRestante = 0f;

    void LateUpdate()
    {
        if (objetivo != null)
        {
            // Posición base (seguimiento)
            Vector3 destino = objetivo.position + offset;

            // Si hay sacudida activa, le sumamos un pequeño desplazamiento aleatorio
            if (tiempoSacudidaRestante > 0f)
            {
                float x = Random.Range(-1f, 1f) * magnitudSacudida;
                float y = Random.Range(-1f, 1f) * magnitudSacudida;

                destino += new Vector3(x, y, 0f);

                tiempoSacudidaRestante -= Time.deltaTime;
            }

            transform.position = destino;
        }
    }

    public void Sacudir(float magnitud)
    {
        magnitudSacudida = magnitud;
        tiempoSacudidaRestante = duracionSacudida;
    }
}
