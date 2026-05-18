using UnityEngine;

public class BillboardEfecto : MonoBehaviour
{
    private Transform camaraAR;

    void Start()
    {
        // Busca automáticamente la cámara principal de la escena de Realidad Aumentada
        if (Camera.main != null)
        {
            camaraAR = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        if (camaraAR != null)
        {
            // Calculamos la dirección DESDE la cámara HACIA el objeto (invierte el espejo)
            Vector3 direccionObjetivo = transform.position - camaraAR.position;
            direccionObjetivo.y = 0; // Seguimos congelando el eje Y para evitar inclinaciones

            // Si la dirección no es cero, aplicamos la rotación correspondiente
            if (direccionObjetivo != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direccionObjetivo);
            }
        }
    }
}
