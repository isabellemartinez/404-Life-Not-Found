using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductoTiendaItem : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private Image _icono;
    [SerializeField] private TextMeshProUGUI _textoNombre;
    [SerializeField] private TextMeshProUGUI _textoPrecio;
    [SerializeField] private Button _botonComprar;

    private ProductoTienda _producto;
    private TiendaUI _tiendaUI;

    public void Configurar(ProductoTienda producto, TiendaUI tiendaUI)
    {
        _producto = producto;
        _tiendaUI = tiendaUI;

        _textoNombre.text = _producto.Nombre;
        _textoPrecio.text = _producto.Precio + " Bits";

        if (_icono != null)
        {
            _icono.sprite = _producto.Icono;
        }

        _botonComprar.onClick.RemoveAllListeners();
        _botonComprar.onClick.AddListener(Comprar);
    }

    private void Comprar()
    {
        _tiendaUI.ComprarProducto(_producto);
    }
}