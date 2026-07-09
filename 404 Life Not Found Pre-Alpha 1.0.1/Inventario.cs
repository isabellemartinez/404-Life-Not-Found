using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    public static Inventario Instancia { get; private set; }

    public event Action InventarioActualizado;

    [Header("Items")]
    [SerializeField] private List<ItemInventario> _items = new List<ItemInventario>();

    public List<ItemInventario> Items { get { return _items; } }

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;

        if (transform.parent != null)
        {
            transform.SetParent(null);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void AgregarItem(Consumible consumible)
    {
        AgregarItem(consumible, 1);
    }

    public void AgregarItem(Consumible consumible, int cantidad)
    {
        if (consumible == null)
        {
            return;
        }

        if (cantidad <= 0)
        {
            return;
        }

        ItemInventario itemExistente = BuscarItem(consumible);

        if (itemExistente != null)
        {
            itemExistente.AumentarCantidad(cantidad);
        }
        else
        {
            ItemInventario nuevoItem = new ItemInventario(consumible, cantidad);
            _items.Add(nuevoItem);
        }

        AvisarInventarioActualizado();
    }

    public void UsarItem(int indice)
    {
        if (indice < 0 || indice >= _items.Count)
        {
            return;
        }

        ItemInventario item = _items[indice];

        if (item == null || item.Consumible == null)
        {
            return;
        }

        item.Consumible.Usar();
        item.DisminuirCantidad(1);

        if (item.Cantidad <= 0)
        {
            _items.RemoveAt(indice);
        }

        AvisarInventarioActualizado();
    }

    public void EliminarItem(int indice)
    {
        if (indice < 0 || indice >= _items.Count)
        {
            return;
        }

        _items.RemoveAt(indice);
        AvisarInventarioActualizado();
    }

    public ItemInventario ObtenerItem(int indice)
    {
        if (indice < 0 || indice >= _items.Count)
        {
            return null;
        }

        return _items[indice];
    }

    private ItemInventario BuscarItem(Consumible consumible)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i] != null && _items[i].Consumible == consumible)
            {
                return _items[i];
            }
        }

        return null;
    }

    private void AvisarInventarioActualizado()
    {
        if (InventarioActualizado != null)
        {
            InventarioActualizado.Invoke();
        }
    }
}