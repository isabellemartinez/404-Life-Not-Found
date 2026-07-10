using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotInventarioUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image _imagenIcono;
    [SerializeField] private TextMeshProUGUI _textoCantidad;
    [SerializeField] private Button _botonSlot;

    private int _indice;
    private InventarioUI _inventarioUI;

    public void Inicializar(InventarioUI inventarioUI, int indice)
    {
        _inventarioUI = inventarioUI;
        _indice = indice;

        if (_botonSlot == null)
        {
            _botonSlot = GetComponent<Button>();
        }

        if (_botonSlot != null)
        {
            _botonSlot.onClick.RemoveAllListeners();
            _botonSlot.onClick.AddListener(Seleccionar);
        }

        Limpiar();
    }

    public void Mostrar(ItemInventario item)
    {
        if (item == null || item.Consumible == null)
        {
            Limpiar();
            return;
        }

        if (_imagenIcono != null)
        {
            _imagenIcono.enabled = true;
            _imagenIcono.sprite = item.Consumible.Icono;
        }

        if (_textoCantidad != null)
        {
            if (item.Cantidad > 1)
            {
                _textoCantidad.text = item.Cantidad.ToString();
            }
            else
            {
                _textoCantidad.text = "";
            }
        }
    }

    public void Limpiar()
    {
        if (_imagenIcono != null)
        {
            _imagenIcono.enabled = false;
            _imagenIcono.sprite = null;
        }

        if (_textoCantidad != null)
        {
            _textoCantidad.text = "";
        }
    }

    private void Seleccionar()
    {
        if (_inventarioUI != null)
        {
            _inventarioUI.SeleccionarSlot(_indice);
        }
    }
}