using UnityEngine;
using UnityEngine.UI;

public class CambiadorColor : MonoBehaviour
{
    [System.Serializable]
    public class PinturaOficial
    {
        public string nombreComercial;
        public Color colorHex;
    }

    [Header("Mallas de las Carrocerías")]
    public MeshRenderer[] carroceriaJeep;
    public MeshRenderer[] carroceriaAlfa;
    public MeshRenderer[] carroceriaFiat;

    [Header("Catálogos de Color por Marca")]
    public PinturaOficial[] coloresJeep;
    public PinturaOficial[] coloresAlfa;
    public PinturaOficial[] coloresFiat;

    [Header("Canvases de las Fichas Técnicas")]
    public GameObject fichaJeep;
    public GameObject fichaAlfa;
    public GameObject fichaFiat;

    [Header("Submenú de Muestras UI")]
    public GameObject panelColores;
    public GameObject[] muestrasUI;

    private bool fichasVisibles = true;

    // EL CEREBRO DEL SISTEMA: 1 = Jeep, 2 = Alfa, 3 = Fiat
    private int marcaActiva = 1;

    void Awake()
    {
        // MAGIA: Auto-conectamos los 7 botones por código para que sí funcionen al tocarlos
        for (int i = 0; i < muestrasUI.Length; i++)
        {
            if (muestrasUI[i] != null)
            {
                Button btn = muestrasUI[i].GetComponent<Button>();
                if (btn != null)
                {
                    int indiceDinamico = i; // Clave vital para que no se equivoquen de color
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => SeleccionarColorDirecto(indiceDinamico));
                }
            }
        }
    }

    // Vuforia llamará a esta función en cuanto detecte un Target
    public void SetMarcaActiva(int idMarca)
    {
        marcaActiva = idMarca;
    }

    public void SeleccionarColorDirecto(int indiceColor)
    {
        // Cambia el color basándose en la orden explícita de Vuforia
        if (marcaActiva == 1 && indiceColor < coloresJeep.Length)
            AplicarColorMultiMalla(carroceriaJeep, coloresJeep[indiceColor].colorHex);
        else if (marcaActiva == 2 && indiceColor < coloresAlfa.Length)
            AplicarColorMultiMalla(carroceriaAlfa, coloresAlfa[indiceColor].colorHex);
        else if (marcaActiva == 3 && indiceColor < coloresFiat.Length)
            AplicarColorMultiMalla(carroceriaFiat, coloresFiat[indiceColor].colorHex);
    }

    private void AplicarColorMultiMalla(MeshRenderer[] mallas, Color colorObjetivo)
    {
        if (mallas == null) return;
        foreach (MeshRenderer malla in mallas)
        {
            if (malla != null) malla.material.SetColor("_BaseColor", colorObjetivo);
        }
    }

    public void AlternarVisibilidadFichas()
    {
        fichasVisibles = !fichasVisibles;
        if (fichaJeep != null) fichaJeep.SetActive(fichasVisibles);
        if (fichaAlfa != null) fichaAlfa.SetActive(fichasVisibles);
        if (fichaFiat != null) fichaFiat.SetActive(fichasVisibles);
    }

    public void AlternarPanelColores()
    {
        if (panelColores == null) return;

        bool nuevoEstado = !panelColores.activeSelf;
        panelColores.SetActive(nuevoEstado);

        if (nuevoEstado)
        {
            PinturaOficial[] catalogoActivo = coloresJeep; // Fallback

            if (marcaActiva == 1) catalogoActivo = coloresJeep;
            else if (marcaActiva == 2) catalogoActivo = coloresAlfa;
            else if (marcaActiva == 3) catalogoActivo = coloresFiat;

            for (int i = 0; i < muestrasUI.Length; i++)
            {
                if (muestrasUI[i] != null)
                {
                    bool deberiaActivar = i < catalogoActivo.Length;
                    muestrasUI[i].SetActive(deberiaActivar);

                    if (deberiaActivar)
                    {
                        muestrasUI[i].transform.localScale = Vector3.one; // Fuerza tamaño móvil

                        Image componenteImagen = muestrasUI[i].GetComponent<Image>();
                        if (componenteImagen != null)
                        {
                            Color colorSeguro = catalogoActivo[i].colorHex;
                            colorSeguro.a = 1f; // Fuerza opacidad sólida
                            componenteImagen.color = colorSeguro;
                        }
                    }
                }
            }
        }
    }
}