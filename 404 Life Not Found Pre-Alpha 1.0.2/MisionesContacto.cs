using UnityEngine;

[System.Serializable]
public class MisionesContacto
{
    [Header("Datos del contacto")]
    [SerializeField] private string _nombreContacto;

    [TextArea(2, 4)]
    [SerializeField] private string _mensajeContacto;

    [SerializeField] private Sprite _fotoContacto;

    [Header("Datos de la misión")]
    [SerializeField] private string _idMision;
    [SerializeField] private string _nombreMision;

    [TextArea(2, 4)]
    [SerializeField] private string _descripcionMision;

    [SerializeField] private string _idPuntoMision;
    [SerializeField] private float _tiempoParaCompletar = 3f;
    [SerializeField] private float _recompensaBits;
    [SerializeField] private int _recompensaXP;

    private bool _aceptada;
    private bool _completada;

    public string NombreContacto { get { return _nombreContacto; } }
    public string MensajeContacto { get { return _mensajeContacto; } }
    public Sprite FotoContacto { get { return _fotoContacto; } }

    public string IdMision { get { return _idMision; } }
    public string NombreMision { get { return _nombreMision; } }
    public string DescripcionMision { get { return _descripcionMision; } }
    public string IdPuntoMision { get { return _idPuntoMision; } }
    public float TiempoParaCompletar { get { return _tiempoParaCompletar; } }
    public float RecompensaBits { get { return _recompensaBits; } }
    public int RecompensaXP { get { return _recompensaXP; } }

    public bool Aceptada { get { return _aceptada; } }
    public bool Completada { get { return _completada; } }

    public MisionesContacto(
        string nombreContacto,
        string mensajeContacto,
        Sprite fotoContacto,
        string idMision,
        string nombreMision,
        string descripcionMision,
        string idPuntoMision,
        float tiempoParaCompletar,
        float recompensaBits,
        int recompensaXP)
    {
        _nombreContacto = nombreContacto;
        _mensajeContacto = mensajeContacto;
        _fotoContacto = fotoContacto;
        _idMision = idMision;
        _nombreMision = nombreMision;
        _descripcionMision = descripcionMision;
        _idPuntoMision = idPuntoMision;
        _tiempoParaCompletar = tiempoParaCompletar;
        _recompensaBits = recompensaBits;
        _recompensaXP = recompensaXP;
        _aceptada = false;
        _completada = false;
    }

    public void Aceptar()
    {
        if (_completada)
        {
            return;
        }

        _aceptada = true;
    }

    public void Completar()
    {
        _completada = true;
        _aceptada = false;
    }
}