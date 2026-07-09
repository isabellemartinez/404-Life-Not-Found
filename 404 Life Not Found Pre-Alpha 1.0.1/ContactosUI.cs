using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ContactosUI : MonoBehaviour
{
    [Header("Lista de misiones")]
    [SerializeField] private List<MisionesContacto> _misionesContactos = new List<MisionesContacto>();

    [Header("Clonado de contactos")]
    [SerializeField] private Transform _contenedorContactos;
    [SerializeField] private GameObject _prefabContactoListItem;

    [Header("Panel detalle")]
    [SerializeField] private GameObject _panelDetalleMision;
    [SerializeField] private Image _imagenContactoDetalle;
    [SerializeField] private TextMeshProUGUI _textoNombreContacto;
    [SerializeField] private TextMeshProUGUI _textoMensajeContacto;
    [SerializeField] private TextMeshProUGUI _textoNombreTrabajo;
    [SerializeField] private TextMeshProUGUI _textoDescripcionTrabajo;
    [SerializeField] private TextMeshProUGUI _textoPagoTrabajo;
    [SerializeField] private TextMeshProUGUI _textoNivelTrabajo;
    [SerializeField] private TextMeshProUGUI _textoEstadoMision;
    [SerializeField] private TextMeshProUGUI _textoBotonAceptar;
    [SerializeField] private Button _botonAceptarMision;

    private MisionesContacto _misionSeleccionada;

    private void OnEnable()
    {
        Debug.Log("ContactosUI activado.");

        if (GestorMisiones.Instancia != null)
        {
            GestorMisiones.Instancia.RegistrarMisionesDisponibles(_misionesContactos);
        }

        CrearListaContactos();

        if (_panelDetalleMision != null)
        {
            _panelDetalleMision.SetActive(false);
        }
    }

    private void CrearListaContactos()
    {
        Debug.Log("Intentando crear lista de contactos...");

        if (_contenedorContactos == null)
        {
            Debug.LogError("Falta asignar ContenedorContactos en ContactosUI.");
            return;
        }

        if (_prefabContactoListItem == null)
        {
            Debug.LogError("Falta asignar PrefabContactoListItem en ContactosUI.");
            return;
        }

        Debug.Log("Cantidad de misiones: " + _misionesContactos.Count);

        LimpiarContenedor();

        for (int i = 0; i < _misionesContactos.Count; i++)
        {
            if (_misionesContactos[i] == null)
            {
                Debug.LogWarning("La misión en la posición " + i + " está vacía.");
                continue;
            }

            Debug.Log("Creando contacto: " + _misionesContactos[i].NombreContacto);

            GameObject nuevoObjeto = Instantiate(_prefabContactoListItem, _contenedorContactos);
            nuevoObjeto.SetActive(true);

            RectTransform rectTransform = nuevoObjeto.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                rectTransform.localScale = Vector3.one;
                rectTransform.anchorMin = new Vector2(0f, 1f);
                rectTransform.anchorMax = new Vector2(1f, 1f);
                rectTransform.pivot = new Vector2(0.5f, 1f);
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.sizeDelta = new Vector2(0f, 90f);
            }

            ContactoListItem nuevoContacto = nuevoObjeto.GetComponent<ContactoListItem>();

            if (nuevoContacto == null)
            {
                Debug.LogError("El prefab no tiene el script ContactoListItem en el objeto principal.");
                return;
            }

            nuevoContacto.Configurar(_misionesContactos[i], this);
        }

        Canvas.ForceUpdateCanvases();

        RectTransform rectContenedor = _contenedorContactos.GetComponent<RectTransform>();

        if (rectContenedor != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectContenedor);
        }
    }

    private void LimpiarContenedor()
    {
        for (int i = _contenedorContactos.childCount - 1; i >= 0; i--)
        {
            Destroy(_contenedorContactos.GetChild(i).gameObject);
        }
    }

    public void MostrarDetalleMision(MisionesContacto misionContacto)
    {
        if (misionContacto == null)
        {
            MostrarMensaje("Esta misión no existe.");
            return;
        }

        _misionSeleccionada = misionContacto;

        if (_imagenContactoDetalle != null)
        {
            _imagenContactoDetalle.sprite = _misionSeleccionada.FotoContacto;
            _imagenContactoDetalle.enabled = _misionSeleccionada.FotoContacto != null;
        }

        if (_textoNombreContacto != null)
        {
            _textoNombreContacto.text = _misionSeleccionada.NombreContacto;
        }

        if (_textoMensajeContacto != null)
        {
            _textoMensajeContacto.text = _misionSeleccionada.MensajeContacto;
        }

        if (_textoNombreTrabajo != null)
        {
            _textoNombreTrabajo.text = _misionSeleccionada.NombreMision;
        }

        if (_textoDescripcionTrabajo != null)
        {
            _textoDescripcionTrabajo.text = _misionSeleccionada.DescripcionMision;
        }

        if (_textoPagoTrabajo != null)
        {
            _textoPagoTrabajo.text = _misionSeleccionada.RecompensaBits + " Bits";
        }

        if (_textoNivelTrabajo != null)
        {
            _textoNivelTrabajo.text = "Tiempo: " + _misionSeleccionada.TiempoParaCompletar + " segundos";
        }

        ActualizarEstadoDetalle();

        if (_botonAceptarMision != null)
        {
            _botonAceptarMision.onClick.RemoveAllListeners();
            _botonAceptarMision.onClick.AddListener(AceptarMisionSeleccionada);
            _botonAceptarMision.interactable = !_misionSeleccionada.Aceptada && !_misionSeleccionada.Completada;
        }

        if (_panelDetalleMision != null)
        {
            _panelDetalleMision.SetActive(true);
        }
    }

    public void AceptarMisionSeleccionada()
    {
        if (_misionSeleccionada == null)
        {
            MostrarMensaje("No hay misión seleccionada.");
            Debug.LogWarning("ContactosUI: no hay misión seleccionada.");
            return;
        }

        if (_misionSeleccionada.Completada)
        {
            MostrarMensaje("Esta misión ya fue completada.");
            Debug.LogWarning("ContactosUI: misión ya completada.");
            return;
        }

        if (_misionSeleccionada.Aceptada)
        {
            MostrarMensaje("Ya aceptaste esta misión.");
            Debug.LogWarning("ContactosUI: misión ya aceptada.");
            return;
        }

        if (GestorMisiones.Instancia == null)
        {
            MostrarMensaje("No existe GestorMisiones.");
            Debug.LogWarning("ContactosUI: no existe GestorMisiones.");
            return;
        }

        bool aceptada = GestorMisiones.Instancia.AceptarMision(_misionSeleccionada);

        if (!aceptada)
        {
            MostrarMensaje("No se pudo aceptar la misión.");
            Debug.LogWarning("ContactosUI: no se pudo aceptar la misión.");
            return;
        }

        ActualizarEstadoDetalle();
        CrearListaContactos();

        MostrarMensaje("Misión aceptada: " + _misionSeleccionada.NombreMision);
        Debug.Log("ContactosUI: misión enviada al GestorMisiones.");
    }

    private void ActualizarEstadoDetalle()
    {
        string estado = ObtenerEstadoMision(_misionSeleccionada);

        if (_textoEstadoMision != null)
        {
            _textoEstadoMision.text = "Estado: " + estado;
        }

        if (_textoBotonAceptar != null)
        {
            _textoBotonAceptar.text = ObtenerTextoBoton(_misionSeleccionada);
        }

        if (_botonAceptarMision != null)
        {
            _botonAceptarMision.interactable = !_misionSeleccionada.Aceptada && !_misionSeleccionada.Completada;
        }
    }

    public string ObtenerEstadoMision(MisionesContacto mision)
    {
        if (mision == null)
        {
            return "Desconocida";
        }

        if (mision.Completada)
        {
            return "Completada";
        }

        if (mision.Aceptada)
        {
            return "Aceptada";
        }

        return "Nueva";
    }

    public string ObtenerTextoBoton(MisionesContacto mision)
    {
        if (mision == null)
        {
            return "Aceptar misión";
        }

        if (mision.Completada)
        {
            return "Completada";
        }

        if (mision.Aceptada)
        {
            return "Aceptada";
        }

        return "Aceptar misión";
    }

    public void CerrarDetalleMision()
    {
        if (_panelDetalleMision != null)
        {
            _panelDetalleMision.SetActive(false);
        }
    }

    private void MostrarMensaje(string mensaje)
    {
        MensajesManager mensajesManager = FindAnyObjectByType<MensajesManager>();

        if (mensajesManager != null)
        {
            mensajesManager.MostrarMensaje(mensaje);
        }
        else
        {
            Debug.Log(mensaje);
        }
    }
}