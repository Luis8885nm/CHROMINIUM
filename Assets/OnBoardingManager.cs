using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class OnboardingManager : MonoBehaviour
{
    [Header("Ajustes de Transición")]
    public float duracionDesvanecido = 0.5f;

    private CanvasGroup canvasGroup;
    private bool interactuado = false;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void IniciarShowroom()
    {
        if (interactuado) return;
        interactuado = true;
        StartCoroutine(DesvanecerPantalla());
    }

    private IEnumerator DesvanecerPantalla()
    {
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionDesvanecido)
        {
            tiempoTranscurrido += Time.deltaTime;
            // Va reduciendo la opacidad gradualmente
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, tiempoTranscurrido / duracionDesvanecido);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false); // Apaga el panel por completo al terminar
    }
}