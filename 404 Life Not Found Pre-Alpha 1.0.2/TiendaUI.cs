using UnityEngine;

public class TiendaUI : MonoBehaviour
{
    [Header("Productos")]
    [SerializeField] private ProductoTienda[] _productos;

    [Header("Referencias UI")]
    [SerializeField] private Transform _contenedorProductos;
    [SerializeField] private ProductoTiendaItem _prefabProducto;

    private void Start()
    {
        CrearProductos();
    }

    private void CrearProductos()
    {
        foreach (Transform hijo in _contenedorProductos)
        {
            Destroy(hijo.gameObject);
        }

        for (int i = 0; i < _productos.Length; i++)
        {
            ProductoTiendaItem item = Instantiate(_prefabProducto, _contenedorProductos);
            item.Configurar(_productos[i], this);
        }
    }

    public void ComprarProducto(ProductoTienda producto)
    {
        if (producto == null)
        {
            return;
        }

        if (producto.Consumible == null)
        {
            MostrarMensaje("Este producto no tiene consumible asignado.");
            return;
        }

        if (Billetera.Instancia == null)
        {
            MostrarMensaje("No se encontró la billetera.");
            return;
        }

        if (Inventario.Instancia == null)
        {
            MostrarMensaje("No se encontró el inventario.");
            return;
        }

        bool compraExitosa = Billetera.Instancia.Gastar(producto.Precio);

        if (compraExitosa == false)
        {
            MostrarMensaje("No tienes suficientes Bits.");
            return;
        }

        Inventario.Instancia.AgregarItem(producto.Consumible, producto.Cantidad);

        MostrarMensaje("Compraste " + producto.Nombre + " x" + producto.Cantidad + ".");
    }

    private void MostrarMensaje(string mensaje)
    {
        if (MensajesManager.Instancia != null)
        {
            MensajesManager.Instancia.MostrarMensaje(mensaje);
        }
        else
        {
            Debug.Log(mensaje);
        }
    }
}