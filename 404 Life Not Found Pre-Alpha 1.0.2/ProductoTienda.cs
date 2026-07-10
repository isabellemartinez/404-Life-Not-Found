using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo Producto Tienda", menuName = "Juego/Tienda/Producto")]
public class ProductoTienda : ScriptableObject
{
    [Header("Datos del producto")]
    [SerializeField] private string _nombre;
    [SerializeField] private Sprite _icono;
    [SerializeField] private Consumible _consumible;

    [Header("Compra")]
    [SerializeField] private int _precio;
    [SerializeField] private int _cantidad = 1;

    public string Nombre { get { return _nombre; } }
    public Sprite Icono { get { return _icono; } }
    public Consumible Consumible { get { return _consumible; } }
    public int Precio { get { return _precio; } }
    public int Cantidad { get { return _cantidad; } }
}