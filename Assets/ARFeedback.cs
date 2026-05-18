using UnityEngine;

public class ARFeedback : MonoBehaviour
{
    private AudioSource audioSource;
    private bool yaDetectado = false;

    void Awake()
    {
        // Consigue automáticamente el componente de audio del mismo Image Target
        audioSource = GetComponent<AudioSource>();
    }

    public void EjecutarFeedbackDeteccion()
    {
        // Evita que el sonido y la vibración se activen muchas veces seguidas
        if (yaDetectado) return;
        yaDetectado = true;

        // 1. Retroalimentación Háptica (Vibración física en el teléfono)
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        Handheld.Vibrate();
#endif

        // 2. Retroalimentación Auditiva (Sonido de éxito)
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }

        Debug.Log("Feedback háptico y auditivo ejecutado con éxito.");
    }

    public void RestablecerDeteccion()
    {
        // Permite volver a sonar/vibrar si el usuario pierde el papel y lo vuelve a enfocar
        yaDetectado = false;
    }
}