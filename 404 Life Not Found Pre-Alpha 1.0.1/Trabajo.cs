using UnityEngine;

[System.Serializable]
public class Trabajo
{
    [SerializeField] private string _idTrabajo;
    [SerializeField] private string _nombre;
    [SerializeField] private string _descripcion;
    [SerializeField] private int _nivelRequerido;
    [SerializeField] private int _nivelMinijuego;
    [SerializeField] private float _pagoBits;
    [SerializeField] private int _pagoXP;

    [Header("Escena del minijuego")]
    [SerializeField] private string _nombreEscenaMinijuego;

    public string IdTrabajo { get { return _idTrabajo; } }
    public string Nombre { get { return _nombre; } }
    public string Descripcion { get { return _descripcion; } }
    public int NivelRequerido { get { return _nivelRequerido; } }
    public int NivelMinijuego { get { return _nivelMinijuego; } }
    public float PagoBits { get { return _pagoBits; } }
    public int PagoXP { get { return _pagoXP; } }
    public string NombreEscenaMinijuego { get { return _nombreEscenaMinijuego; } }

    public Trabajo(
        string idTrabajo,
        string nombre,
        string descripcion,
        int nivelRequerido,
        int nivelMinijuego,
        float pagoBits,
        int pagoXP,
        string nombreEscenaMinijuego)
    {
        _idTrabajo = idTrabajo;
        _nombre = nombre;
        _descripcion = descripcion;
        _nivelRequerido = nivelRequerido;
        _nivelMinijuego = nivelMinijuego;
        _pagoBits = pagoBits;
        _pagoXP = pagoXP;
        _nombreEscenaMinijuego = nombreEscenaMinijuego;
    }
}