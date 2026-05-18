using UnityEngine;
using UnityEngine.EventSystems; // Evita que el zoom se active al tocar botones o el Slider

public class PinchZoom : MonoBehaviour
{
    [Header("Configuración de Objetos")]
    public GameObject[] objetosA_Escalar; // Aquí arrastrarás tus 3 objetos 'Acomodo'

    [Header("Ajustes de Zoom")]
    public float sensibilidad = 2.0f; // Sensibilidad calibrada para pantallas de alta densidad (DPI)
    public float minEscala = 0.1f;
    public float maxEscala = 10.0f;

    void Update()
    {
        // 1. Solo actuar si hay exactamente 2 dedos en la pantalla
        if (Input.touchCount == 2)
        {
            Touch dedo1 = Input.GetTouch(0);
            Touch dedo2 = Input.GetTouch(1);

            // 2. FILTRO DE UI: Si el usuario está moviendo el Slider, bloqueamos el pellizco para que no se peleen
            if (EventSystem.current != null)
            {
                if (EventSystem.current.IsPointerOverGameObject(dedo1.fingerId) ||
                    EventSystem.current.IsPointerOverGameObject(dedo2.fingerId))
                {
                    return; // Ignora el código de abajo si tocan la UI
                }
            }

            // 3. Solo calculamos si al menos uno de los dedos se está moviendo realmente
            if (dedo1.phase == TouchPhase.Moved || dedo2.phase == TouchPhase.Moved)
            {
                // Posición de los dedos en el frame anterior
                Vector2 dedo1PrevPos = dedo1.position - dedo1.deltaPosition;
                Vector2 dedo2PrevPos = dedo2.position - dedo2.deltaPosition;

                // Magnitud (distancia en píxeles) entre dedos: frame anterior vs actual
                float prevMag = (dedo1PrevPos - dedo2PrevPos).magnitude;
                float currentMag = (dedo1.position - dedo2.position).magnitude;

                // La diferencia neta de movimiento
                float diferenciaPixeles = currentMag - prevMag;

                // DIVISION DE INGENIERÍA: Dividimos entre el ancho de la pantalla para que la 
                // sensibilidad responda igual de bien en tu Legion, en un teléfono 4K o uno HD.
                float factorEscala = (diferenciaPixeles / Screen.width) * sensibilidad;

                AplicarEscala(factorEscala);
            }
        }
    }

    void AplicarEscala(float incremento)
    {
        foreach (GameObject obj in objetosA_Escalar)
        {
            if (obj != null)
            {
                // Tomamos la escala actual del eje X como base y sumamos el incremento
                float nuevaEscala = Mathf.Clamp(obj.transform.localScale.x + incremento, minEscala, maxEscala);
                obj.transform.localScale = new Vector3(nuevaEscala, nuevaEscala, nuevaEscala);
            }
        }
    }
}