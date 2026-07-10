using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgresoMisionUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject _panelProgreso;
    [SerializeField] private Slider _barraProgreso;
    [SerializeField] private TMP_Text _textoProgreso;

    private string _mensajeActual;

    private void Awake()
    {
        Ocultar();
    }

    public void Mostrar(string mensaje)
    {
        _mensajeActual = mensaje;

        if (_panelProgreso != null)
        {
            _panelProgreso.SetActive(true);
        }

        if (_barraProgreso != null)
        {
            _barraProgreso.minValue = 0f;
            _barraProgreso.maxValue = 1f;
            _barraProgreso.value = 0f;
        }

        Actualizar(0f);
    }

    public void Actualizar(float progreso)
    {
        progreso = Mathf.Clamp01(progreso);

        if (_barraProgreso != null)
        {
            _barraProgreso.value = progreso;
        }

        if (_textoProgreso != null)
        {
            int porcentaje = Mathf.RoundToInt(progreso * 100f);
            _textoProgreso.text = _mensajeActual + "\n" + porcentaje + "%";
        }
    }

    public void Ocultar()
    {
        if (_barraProgreso != null)
        {
            _barraProgreso.value = 0f;
        }

        if (_textoProgreso != null)
        {
            _textoProgreso.text = "";
        }

        if (_panelProgreso != null)
        {
            _panelProgreso.SetActive(false);
        }
    }
}