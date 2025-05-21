using System.Collections;
using UnityEngine;

public class SangreScript : MonoBehaviour
{
    [SerializeField] private Sprite[] SpritesSangre;
    [SerializeField] private GameObject sangrePrefab;
    [SerializeField] private int probabilidadSpawn = 10;
    [SerializeField] private int totalProbabilidad = 30;
    [SerializeField] private float rangoAleatorio = 0.5f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = SpritesSangre[Random.Range(0, SpritesSangre.Length)];

        // Probabilidad de spawn
        int dado = Random.Range(1, totalProbabilidad + 1); // [1, 30]

        if (dado <= probabilidadSpawn)
        {
            StartCoroutine(SpawnSangreConRetraso(0.06f)); // Ajusta el tiempo si quieres
        }

    }

    private IEnumerator SpawnSangreConRetraso(float retraso)
    {
        yield return new WaitForSeconds(retraso);

        Vector2 offset = new Vector2(
            Random.Range(-rangoAleatorio, rangoAleatorio),
            Random.Range(-rangoAleatorio, rangoAleatorio)
        );
        Vector3 nuevaPosicion = transform.position + (Vector3)offset;

        Quaternion rotacionAleatoria = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        Instantiate(sangrePrefab, nuevaPosicion, rotacionAleatoria);
    }

}
