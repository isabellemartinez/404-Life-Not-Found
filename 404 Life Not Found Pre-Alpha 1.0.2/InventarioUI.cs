using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventarioUI : MonoBehaviour
{
    [Header("Slots")]
    [SerializeField] private SlotInventarioUI[] _slots;

    [Header("Panel de detalles")]
    [SerializeField] private Image _imagenObjetoSeleccionado;
    [SerializeField] private TextMeshProUGUI _textoNombreObjeto;
    [SerializeField] private TextMeshProUGUI _textoCategoriaObjeto;
    [SerializeField] private TextMeshProUGUI _textoCantidadObjeto;
    [SerializeField] private TextMeshProUGUI _textoDescripcionObjeto;

    [Header("Botones")]
    [SerializeField] private Button _botonUsar;
    [SerializeField] private Button _botonEliminar;

    private int _indiceSeleccionado = -1;

    private void Awake()
    {
        InicializarSlots();
        ConfigurarBotones();
        LimpiarDetalles();
    }

    private void OnEnable()
    {
        if (Inventario.Instancia != null)
        {
            Inventario.Instancia.InventarioActualizado += ActualizarInventario;
        }

        ActualizarInventario();
    }

    private void OnDisable()
    {
        if (Inventario.Instancia != null)
        {
            Inventario.Instancia.InventarioActualizado -= ActualizarInventario;
        }
    }

    private void InicializarSlots()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] != null)
            {
                _slots[i].Inicializar(this, i);
            }
        }
    }

    private void ConfigurarBotones()
    {
        if (_botonUsar != null)
        {
            _botonUsar.onClick.RemoveAllListeners();
            _botonUsar.onClick.AddListener(UsarObjetoSeleccionado);
        }

        if (_botonEliminar != null)
        {
            _botonEliminar.onClick.RemoveAllListeners();
            _botonEliminar.onClick.AddListener(EliminarObjetoSeleccionado);
        }
    }

    public void ActualizarInventario()
    {
        LimpiarSlots();

        if (Inventario.Instancia == null)
        {
            LimpiarDetalles();
            return;
        }

        for (int i = 0; i < _slots.Length; i++)
        {
            ItemInventario item = Inventario.Instancia.ObtenerItem(i);

            if (_slots[i] != null)
            {
                _slots[i].Mostrar(item);
            }
        }

        if (_indiceSeleccionado >= Inventario.Instancia.Items.Count)
        {
            LimpiarDetalles();
        }
        else if (_indiceSeleccionado >= 0)
        {
            MostrarDetalles(_indiceSeleccionado);
        }
    }

    private void LimpiarSlots()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] != null)
            {
                _slots[i].Limpiar();
            }
        }
    }

    public void SeleccionarSlot(int indice)
    {
        if (Inventario.Instancia == null)
        {
            return;
        }

        ItemInventario item = Inventario.Instancia.ObtenerItem(indice);

        if (item == null || item.Consumible == null)
        {
            LimpiarDetalles();
            return;
        }

        _indiceSeleccionado = indice;
        MostrarDetalles(indice);
    }

    private void MostrarDetalles(int indice)
    {
        if (Inventario.Instancia == null)
        {
            return;
        }

        ItemInventario item = Inventario.Instancia.ObtenerItem(indice);

        if (item == null || item.Consumible == null)
        {
            LimpiarDetalles();
            return;
        }

        Consumible consumible = item.Consumible;

        if (_imagenObjetoSeleccionado != null)
        {
            _imagenObjetoSeleccionado.enabled = true;
            _imagenObjetoSeleccionado.sprite = consumible.Icono;
        }

        if (_textoNombreObjeto != null)
        {
            _textoNombreObjeto.text = consumible.Nombre;
        }

        if (_textoCategoriaObjeto != null)
        {
            _textoCategoriaObjeto.text = consumible.Categoria;
        }

        if (_textoCantidadObjeto != null)
        {
            _textoCantidadObjeto.text = "Cantidad: " + item.Cantidad;
        }

        if (_textoDescripcionObjeto != null)
        {
            _textoDescripcionObjeto.text = consumible.Descripcion;
        }

        if (_botonUsar != null)
        {
            _botonUsar.interactable = true;
        }

        if (_botonEliminar != null)
        {
            _botonEliminar.interactable = true;
        }
    }

    private void UsarObjetoSeleccionado()
    {
        if (_indiceSeleccionado < 0)
        {
            return;
        }

        if (Inventario.Instancia == null)
        {
            return;
        }

        Inventario.Instancia.UsarItem(_indiceSeleccionado);
        ActualizarInventario();
    }

    private void EliminarObjetoSeleccionado()
    {
        if (_indiceSeleccionado < 0)
        {
            return;
        }

        if (Inventario.Instancia == null)
        {
            return;
        }

        Inventario.Instancia.EliminarItem(_indiceSeleccionado);
        LimpiarDetalles();
        ActualizarInventario();
    }

    private void LimpiarDetalles()
    {
        _indiceSeleccionado = -1;

        if (_imagenObjetoSeleccionado != null)
        {
            _imagenObjetoSeleccionado.enabled = false;
            _imagenObjetoSeleccionado.sprite = null;
        }

        if (_textoNombreObjeto != null)
        {
            _textoNombreObjeto.text = "";
        }

        if (_textoCategoriaObjeto != null)
        {
            _textoCategoriaObjeto.text = "";
        }

        if (_textoCantidadObjeto != null)
        {
            _textoCantidadObjeto.text = "";
        }

        if (_textoDescripcionObjeto != null)
        {
            _textoDescripcionObjeto.text = "";
        }

        if (_botonUsar != null)
        {
            _botonUsar.interactable = false;
        }

        if (_botonEliminar != null)
        {
            _botonEliminar.interactable = false;
        }
    }
}