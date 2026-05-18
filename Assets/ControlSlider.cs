using UnityEngine;

public class ControlSlider : MonoBehaviour
{
    [Header("Lista de Modelos")]
    // Ahora es un arreglo [] para aceptar varios objetos
    public GameObject[] modelosA_Escalar;

    [Header("Rangos de Escala")]
    public float escalaMinima = 0.1f;
    public float escalaMaxima = 3.0f;

    public void CambiarEscala(float valorSlider)
    {
        // Calculamos la escala basada en el slider
        float nuevaEscala = Mathf.Lerp(escalaMinima, escalaMaxima, valorSlider);
        Vector3 escalaVector = new Vector3(nuevaEscala, nuevaEscala, nuevaEscala);

        // Recorremos la lista y aplicamos la escala a cada uno
        foreach (GameObject modelo in modelosA_Escalar)
        {
            if (modelo != null)
            {
                modelo.transform.localScale = escalaVector;
            }
        }
    }
}