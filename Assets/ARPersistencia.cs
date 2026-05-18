using UnityEngine;
using Vuforia;

public class ARPersistencia : MonoBehaviour
{
    private ObserverBehaviour mObserverBehaviour;

    [Header("Configuración de UI")]
    public GameObject panelAviso; // El panel 'AvisoTarget' de tu Canvas

    [Header("Contenedor Modular")]
    public GameObject[] modelosAutos; // Aquí arrastrarás únicamente el objeto 'Acomodo' de este Target

    void Start()
    {
        mObserverBehaviour = GetComponent<ObserverBehaviour>();
        if (mObserverBehaviour)
        {
            mObserverBehaviour.OnTargetStatusChanged += OnStatusChanged;
        }

        // ==========================================
        // OPTIMIZACIÓN DE INICIO (EVITA AUTOS FLOTANDO)
        // ==========================================
        SetModelos(false);
        if (panelAviso != null)
        {
            panelAviso.SetActive(false);
        }
    }

    private void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        // Caso 1: El papel se ve o se calcula por giroscopio con buena precisión
        if (targetStatus.Status == Status.TRACKED || targetStatus.Status == Status.EXTENDED_TRACKED)
        {
            if (panelAviso != null) panelAviso.SetActive(false);
            SetModelos(true);
        }
        // Caso 2: Se perdió el papel pero el teléfono estima la posición (Persistencia activa)
        else if (targetStatus.Status == Status.LIMITED)
        {
            if (panelAviso != null) panelAviso.SetActive(true);
            SetModelos(true); // Mantenemos el auto visible en la mesa física
        }
        // Caso 3: Pérdida total de tracking (Cámara tapada o error crítico de sensores)
        else
        {
            if (panelAviso != null) panelAviso.SetActive(true);
            SetModelos(false); // Apagamos para evitar que el modelo salga disparado al cielo
        }
    }

    void SetModelos(bool estado)
    {
        foreach (var auto in modelosAutos)
        {
            if (auto != null) auto.SetActive(estado);
        }
    }

    // Función que llamará tu botón "Nuevo Escaneo"
    public void LimpiarEscenaParaNuevoEscaneo()
    {
        // 1. Apagamos el contenedor 'Acomodo' (oculta auto y sombras de golpe)
        SetModelos(false);

        // 2. Ocultamos el letrero de advertencia
        if (panelAviso != null)
        {
            panelAviso.SetActive(false);
        }

        // 3. Regresamos la escala al valor por defecto (1,1,1) para el siguiente escaneo
        foreach (GameObject auto in modelosAutos)
        {
            if (auto != null) auto.transform.localScale = Vector3.one;
        }
    }
}