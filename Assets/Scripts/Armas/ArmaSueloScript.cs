using UnityEngine;

public class ArmaSueloScript : MonoBehaviour
{
    public int tipoArma = 0;
    public Sprite[] spritesArmas;
    [SerializeField] SpriteRenderer spriteRenderer;

    void Start()
    {
        ActualizarSprite();
    }

    public void ActualizarSprite()
    {
        if (spritesArmas != null && tipoArma >= 0 && tipoArma < spritesArmas.Length)
        {
            spriteRenderer.sprite = spritesArmas[tipoArma];

            transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        }
        else
        {
            Debug.Log("Tipo de arma fuera de rango o sprites no asignados.");
        }
    }
}
