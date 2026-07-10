using System.Collections.Generic;
using UnityEngine;

public class GestorMisiones : MonoBehaviour
{
    public static GestorMisiones Instancia { get; private set; }

    private const string CLAVE_MISIONES_ACEPTADAS = "MisionesAceptadas";
    private const string CLAVE_MISIONES_COMPLETADAS = "MisionesCompletadas";

    [SerializeField] private Billetera _billetera;

    private List<MisionesContacto> _misionesAceptadas = new List<MisionesContacto>();
    private List<MisionesContacto> _misionesCompletadas = new List<MisionesContacto>();

    private List<string> _idsMisionesAceptadasGuardadas = new List<string>();
    private List<string> _idsMisionesCompletadasGuardadas = new List<string>();

    public List<MisionesContacto> MisionesAceptadas { get { return _misionesAceptadas; } }
    public List<MisionesContacto> MisionesCompletadas { get { return _misionesCompletadas; } }

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;
        DontDestroyOnLoad(gameObject);

        CargarDatos();
    }

    public void RegistrarMisionesDisponibles(List<MisionesContacto> misionesDisponibles)
    {
        if (misionesDisponibles == null)
        {
            return;
        }

        for (int i = 0; i < misionesDisponibles.Count; i++)
        {
            MisionesContacto mision = misionesDisponibles[i];

            if (mision == null)
            {
                continue;
            }

            if (string.IsNullOrEmpty(mision.IdMision))
            {
                Debug.LogWarning("Hay una misión sin IdMision: " + mision.NombreMision);
                continue;
            }

            if (_idsMisionesCompletadasGuardadas.Contains(mision.IdMision))
            {
                mision.Completar();

                if (!MisionEstaCompletada(mision.IdMision))
                {
                    _misionesCompletadas.Add(mision);
                }

                continue;
            }

            if (_idsMisionesAceptadasGuardadas.Contains(mision.IdMision))
            {
                mision.Aceptar();

                if (!MisionEstaAceptada(mision.IdMision))
                {
                    _misionesAceptadas.Add(mision);
                }
            }
        }

        Debug.Log("Misiones registradas. Aceptadas: " + _misionesAceptadas.Count + " | Completadas: " + _misionesCompletadas.Count);
    }

    public bool AceptarMision(MisionesContacto mision)
    {
        if (mision == null)
        {
            Debug.LogWarning("La misión enviada al GestorMisiones es null.");
            return false;
        }

        if (string.IsNullOrEmpty(mision.IdMision))
        {
            Debug.LogWarning("La misión no tiene IdMision: " + mision.NombreMision);
            return false;
        }

        if (MisionEstaCompletada(mision.IdMision))
        {
            MostrarMensaje("Esta misión ya fue completada.");
            return false;
        }

        if (MisionEstaAceptada(mision.IdMision))
        {
            MostrarMensaje("Esta misión ya está aceptada.");
            return false;
        }

        mision.Aceptar();
        _misionesAceptadas.Add(mision);

        GuardarDatos();

        MostrarMensaje("Misión aceptada: " + mision.NombreMision);

        return true;
    }

    public void CompletarMision(string idMision)
    {
        MisionesContacto mision = ObtenerMisionAceptadaPorId(idMision);

        if (mision == null)
        {
            Debug.LogWarning("No se encontró una misión aceptada con el ID: " + idMision);
            return;
        }

        mision.Completar();

        _misionesAceptadas.Remove(mision);

        if (!MisionEstaCompletada(mision.IdMision))
        {
            _misionesCompletadas.Add(mision);
        }

        DarRecompensa(mision);
        GuardarDatos();

        MostrarMensaje("Misión completada: " + mision.NombreMision);
    }

    public MisionesContacto ObtenerMisionAceptadaPorPunto(string idPuntoMision)
    {
        for (int i = 0; i < _misionesAceptadas.Count; i++)
        {
            if (_misionesAceptadas[i].IdPuntoMision == idPuntoMision)
            {
                return _misionesAceptadas[i];
            }
        }

        return null;
    }

    public MisionesContacto ObtenerMisionAceptadaPorId(string idMision)
    {
        for (int i = 0; i < _misionesAceptadas.Count; i++)
        {
            if (_misionesAceptadas[i].IdMision == idMision)
            {
                return _misionesAceptadas[i];
            }
        }

        return null;
    }

    public bool MisionEstaAceptada(string idMision)
    {
        for (int i = 0; i < _misionesAceptadas.Count; i++)
        {
            if (_misionesAceptadas[i].IdMision == idMision)
            {
                return true;
            }
        }

        return false;
    }

    public bool MisionEstaCompletada(string idMision)
    {
        for (int i = 0; i < _misionesCompletadas.Count; i++)
        {
            if (_misionesCompletadas[i].IdMision == idMision)
            {
                return true;
            }
        }

        return false;
    }

    private void DarRecompensa(MisionesContacto mision)
    {
        if (_billetera == null)
        {
            _billetera = FindAnyObjectByType<Billetera>();
        }

        if (_billetera == null)
        {
            Debug.LogWarning("No se encontró Billetera en la escena.");
            return;
        }

        _billetera.Agregar(mision.RecompensaBits);
        _billetera.AgregarExperiencia(mision.RecompensaXP);
    }

    private void GuardarDatos()
    {
        string idsAceptadas = ConvertirListaMisionesATexto(_misionesAceptadas);
        string idsCompletadas = ConvertirListaMisionesATexto(_misionesCompletadas);

        PlayerPrefs.SetString(CLAVE_MISIONES_ACEPTADAS, idsAceptadas);
        PlayerPrefs.SetString(CLAVE_MISIONES_COMPLETADAS, idsCompletadas);
        PlayerPrefs.Save();

        Debug.Log("Misiones guardadas. Aceptadas: " + idsAceptadas + " | Completadas: " + idsCompletadas);
    }

    private void CargarDatos()
    {
        string idsAceptadas = PlayerPrefs.GetString(CLAVE_MISIONES_ACEPTADAS, "");
        string idsCompletadas = PlayerPrefs.GetString(CLAVE_MISIONES_COMPLETADAS, "");

        _idsMisionesAceptadasGuardadas = ConvertirTextoALista(idsAceptadas);
        _idsMisionesCompletadasGuardadas = ConvertirTextoALista(idsCompletadas);

        Debug.Log("Misiones cargadas. Aceptadas: " + idsAceptadas + " | Completadas: " + idsCompletadas);
    }

    private string ConvertirListaMisionesATexto(List<MisionesContacto> misiones)
    {
        List<string> ids = new List<string>();

        for (int i = 0; i < misiones.Count; i++)
        {
            if (misiones[i] == null)
            {
                continue;
            }

            if (string.IsNullOrEmpty(misiones[i].IdMision))
            {
                continue;
            }

            if (!ids.Contains(misiones[i].IdMision))
            {
                ids.Add(misiones[i].IdMision);
            }
        }

        return string.Join(",", ids);
    }

    private List<string> ConvertirTextoALista(string texto)
    {
        List<string> lista = new List<string>();

        if (string.IsNullOrEmpty(texto))
        {
            return lista;
        }

        string[] partes = texto.Split(',');

        for (int i = 0; i < partes.Length; i++)
        {
            string id = partes[i].Trim();

            if (!string.IsNullOrEmpty(id) && !lista.Contains(id))
            {
                lista.Add(id);
            }
        }

        return lista;
    }

    public void BorrarDatosGuardados()
    {
        PlayerPrefs.DeleteKey(CLAVE_MISIONES_ACEPTADAS);
        PlayerPrefs.DeleteKey(CLAVE_MISIONES_COMPLETADAS);
        PlayerPrefs.Save();

        _misionesAceptadas.Clear();
        _misionesCompletadas.Clear();
        _idsMisionesAceptadasGuardadas.Clear();
        _idsMisionesCompletadasGuardadas.Clear();

        Debug.Log("Datos guardados de misiones borrados.");
    }

    private void MostrarMensaje(string mensaje)
    {
        MensajesManager mensajesManager = FindAnyObjectByType<MensajesManager>();

        if (mensajesManager != null)
        {
            mensajesManager.MostrarMensaje(mensaje);
        }
        else
        {
            Debug.Log(mensaje);
        }
    }
}