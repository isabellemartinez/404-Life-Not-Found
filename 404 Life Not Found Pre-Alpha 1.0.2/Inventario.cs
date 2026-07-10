using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    public static Inventario Instancia { get; private set; }

    public event Action InventarioActualizado;

    [Header("Items")]
    [SerializeField] private List<ItemInventario> _items = new List<ItemInventario>();

    [Header("Base de datos de consumibles")]
    [SerializeField] private Consumible[] _consumiblesDisponibles;

    private const string ClaveInventario = "Inventario_Datos";

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

        CargarInventario();
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

        GuardarInventario();
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

        GuardarInventario();
        AvisarInventarioActualizado();
    }

    public void EliminarItem(int indice)
    {
        if (indice < 0 || indice >= _items.Count)
        {
            return;
        }

        _items.RemoveAt(indice);

        GuardarInventario();
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

    public void GuardarInventario()
    {
        DatosInventario datos = new DatosInventario();
        List<ItemInventarioGuardado> itemsGuardados = new List<ItemInventarioGuardado>();

        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i] == null || _items[i].Consumible == null)
            {
                continue;
            }

            if (string.IsNullOrEmpty(_items[i].Consumible.ID))
            {
                Debug.LogWarning("Hay un consumible sin ID: " + _items[i].Consumible.Nombre);
                continue;
            }

            ItemInventarioGuardado itemGuardado = new ItemInventarioGuardado(
                _items[i].Consumible.ID,
                _items[i].Cantidad
            );

            itemsGuardados.Add(itemGuardado);
        }

        datos.items = itemsGuardados.ToArray();

        string json = JsonUtility.ToJson(datos);
        PlayerPrefs.SetString(ClaveInventario, json);
        PlayerPrefs.Save();
    }

    public void CargarInventario()
    {
        if (PlayerPrefs.HasKey(ClaveInventario) == false)
        {
            return;
        }

        string json = PlayerPrefs.GetString(ClaveInventario);

        if (string.IsNullOrEmpty(json))
        {
            return;
        }

        DatosInventario datos = JsonUtility.FromJson<DatosInventario>(json);

        if (datos == null || datos.items == null)
        {
            return;
        }

        _items.Clear();

        for (int i = 0; i < datos.items.Length; i++)
        {
            Consumible consumible = BuscarConsumiblePorID(datos.items[i].idConsumible);

            if (consumible == null)
            {
                Debug.LogWarning("No se encontró consumible con ID: " + datos.items[i].idConsumible);
                continue;
            }

            if (datos.items[i].cantidad <= 0)
            {
                continue;
            }

            ItemInventario nuevoItem = new ItemInventario(consumible, datos.items[i].cantidad);
            _items.Add(nuevoItem);
        }

        AvisarInventarioActualizado();
    }

    private Consumible BuscarConsumiblePorID(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return null;
        }

        for (int i = 0; i < _consumiblesDisponibles.Length; i++)
        {
            if (_consumiblesDisponibles[i] == null)
            {
                continue;
            }

            if (_consumiblesDisponibles[i].ID == id)
            {
                return _consumiblesDisponibles[i];
            }
        }

        return null;
    }

    private ItemInventario BuscarItem(Consumible consumible)
    {
        if (consumible == null)
        {
            return null;
        }

        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i] == null || _items[i].Consumible == null)
            {
                continue;
            }

            if (_items[i].Consumible == consumible)
            {
                return _items[i];
            }

            if (string.IsNullOrEmpty(consumible.ID) == false &&
                _items[i].Consumible.ID == consumible.ID)
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

[Serializable]
public class DatosInventario
{
    public ItemInventarioGuardado[] items;
}

[Serializable]
public class ItemInventarioGuardado
{
    public string idConsumible;
    public int cantidad;

    public ItemInventarioGuardado(string idConsumible, int cantidad)
    {
        this.idConsumible = idConsumible;
        this.cantidad = cantidad;
    }
}