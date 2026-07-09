using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgresoMisionUI : MonoBehaviour
{
    [SerializeField] private GameObject _panelProgreso;
    [SerializeField] private Slider _barraProgreso;
    [SerializeField] private TMP_Text _textoProgreso;

    private void Awake()
    {
        Ocultar();
    }

    public void Mostrar(string mensaje)
    {
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

        if (_textoProgreso != null)
        {
            _textoProgreso.text = mensaje;
        }
    }

    public void Actualizar(float progreso)
    {
        if (_barraProgreso != null)
        {
            _barraProgreso.value = Mathf.Clamp01(progreso);
        }
    }

    public void Ocultar()
    {
        if (_barraProgreso != null)
        {
            _barraProgreso.value = 0f;
        }

        if (_panelProgreso != null)
        {
            _panelProgreso.SetActive(false);
        }
    }
}