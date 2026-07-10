using UnityEngine;

[CreateAssetMenu(fileName = "NuevoConsumible", menuName = "Inventario/Consumible")]
public class Consumible : ScriptableObject
{
    [Header("Identificador único")]
    [SerializeField] private string _id;

    [Header("Datos básicos")]
    [SerializeField] private string _nombre;
    [SerializeField] private string _categoria;
    [SerializeField] private float _precio;
    [SerializeField] private Sprite _icono;

    [TextArea]
    [SerializeField] private string _descripcion;

    [Header("Efectos")]
    [SerializeField] private float _puntosSed;
    [SerializeField] private float _puntosHambre;
    [SerializeField] private float _puntosEstres;

    public string ID { get { return _id; } }

    public string Nombre { get { return _nombre; } }
    public string Categoria { get { return _categoria; } }
    public float Precio { get { return _precio; } }
    public Sprite Icono { get { return _icono; } }
    public string Descripcion { get { return _descripcion; } }

    public float PuntosSed { get { return _puntosSed; } }
    public float PuntosHambre { get { return _puntosHambre; } }
    public float PuntosEstres { get { return _puntosEstres; } }

    public void Usar()
    {
        if (Billetera.Instancia == null)
        {
            return;
        }

        Billetera.Instancia.CambiarSed(Billetera.Instancia.ValorSed + _puntosSed);
        Billetera.Instancia.CambiarHambre(Billetera.Instancia.ValorHambre + _puntosHambre);
        Billetera.Instancia.CambiarEstres(Billetera.Instancia.ValorEstres + _puntosEstres);
    }
}