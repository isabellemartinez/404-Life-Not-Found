using TMPro;
using UnityEngine;

public class CelularUI : MonoBehaviour
{
    [Header("Pantallas del celular")]
    [SerializeField] private GameObject _pantallaInicio;
    [SerializeField] private GameObject _pantallaPedidos;
    [SerializeField] private GameObject _pantallaTrabajo;
    [SerializeField] private GameObject _pantallaContactos;

    [Header("Texto superior")]
    [SerializeField] private TextMeshProUGUI _textoTituloApp;

    private void Start()
    {
        VolverInicio();
    }

    public void AbrirAppPedidos()
    {
        OcultarTodasLasPantallas();

        if (_pantallaPedidos != null)
        {
            _pantallaPedidos.SetActive(true);
        }

        CambiarTitulo("Pedidos");
    }

    public void AbrirAppTrabajo()
    {
        OcultarTodasLasPantallas();

        if (_pantallaTrabajo != null)
        {
            _pantallaTrabajo.SetActive(true);
        }

        CambiarTitulo("Trabajos");
    }

    public void AbrirAppContactos()
    {
        OcultarTodasLasPantallas();

        if (_pantallaContactos != null)
        {
            _pantallaContactos.SetActive(true);
        }

        CambiarTitulo("Contactos");
    }

    public void VolverInicio()
    {
        OcultarTodasLasPantallas();

        if (_pantallaInicio != null)
        {
            _pantallaInicio.SetActive(true);
        }

        CambiarTitulo("Inicio");
    }

    private void OcultarTodasLasPantallas()
    {
        if (_pantallaInicio != null)
        {
            _pantallaInicio.SetActive(false);
        }

        if (_pantallaPedidos != null)
        {
            _pantallaPedidos.SetActive(false);
        }

        if (_pantallaTrabajo != null)
        {
            _pantallaTrabajo.SetActive(false);
        }

        if (_pantallaContactos != null)
        {
            _pantallaContactos.SetActive(false);
        }
    }

    private void CambiarTitulo(string titulo)
    {
        if (_textoTituloApp != null)
        {
            _textoTituloApp.text = titulo;
        }
    }
}