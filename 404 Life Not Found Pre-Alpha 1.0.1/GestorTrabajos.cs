using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GestorTrabajos : MonoBehaviour
{
    public static GestorTrabajos Instancia { get; private set; }

    [SerializeField] private List<Trabajo> _trabajosDisponibles;
    [SerializeField] private string _nombreEscenaPrincipal = "SampleScene";
    [SerializeField] private float _tiempoEsperaParaVolver = 2f;

    [Header("Cooldown de trabajos")]
    [SerializeField] private float _tiempoEsperaDespuesDeTrabajo = 120f; // 2 minutos

    private Trabajo _trabajoActivo;
    private bool _enEsperaDespuesDeTrabajo = false;
    private Coroutine _corutinaCooldown;

    public Trabajo TrabajoActivo
    {
        get { return _trabajoActivo; }
    }

    public List<Trabajo> TrabajosDisponibles
    {
        get { return _trabajosDisponibles; }
    }

    public bool EnEsperaDespuesDeTrabajo
    {
        get { return _enEsperaDespuesDeTrabajo; }
    }

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool PuedeAceptarTrabajo()
    {
        return _trabajoActivo == null && !_enEsperaDespuesDeTrabajo;
    }

    public bool AceptarTrabajo(Trabajo trabajo)
    {
        if (trabajo == null)
        {
            Debug.LogWarning("El trabajo es null.");
            return false;
        }

        if (!PuedeAceptarTrabajo())
        {
            if (_trabajoActivo != null)
            {
                Debug.Log("No puedes aceptar otro trabajo. Ya tienes uno activo: " + _trabajoActivo.Nombre);
            }
            else if (_enEsperaDespuesDeTrabajo)
            {
                Debug.Log("Debes esperar antes de aceptar otro trabajo.");
            }

            return false;
        }

        if (string.IsNullOrEmpty(trabajo.NombreEscenaMinijuego))
        {
            Debug.LogWarning("El trabajo no tiene escena de minijuego asignada: " + trabajo.Nombre);
            return false;
        }

        _trabajoActivo = trabajo;

        Debug.Log("Trabajo aceptado: " + trabajo.Nombre);
        Debug.Log("Cargando minijuego: " + trabajo.NombreEscenaMinijuego);


        return true;
    }

    public void IniciarMinijuegoTrabajoActivo()
    {
        if (_trabajoActivo == null)
        {
            Debug.Log("No tienes ningún trabajo activo.");
            return;
        }

        if (string.IsNullOrEmpty(_trabajoActivo.NombreEscenaMinijuego))
        {
            Debug.LogWarning("El trabajo activo no tiene escena de minijuego asignada.");
            return;
        }

        Debug.Log("Iniciando minijuego: " + _trabajoActivo.NombreEscenaMinijuego);
        SceneManager.LoadScene(_trabajoActivo.NombreEscenaMinijuego);
    }

    public void FinalizarTrabajoActivo()
    {
        if (_trabajoActivo == null)
        {
            Debug.Log("No hay trabajo activo para finalizar.");
            return;
        }

        Debug.Log("Trabajo finalizado: " + _trabajoActivo.Nombre);

        _trabajoActivo = null;

        if (_corutinaCooldown != null)
        {
            StopCoroutine(_corutinaCooldown);
        }

        _corutinaCooldown = StartCoroutine(EsperarParaAceptarOtroTrabajo());
    }

    private IEnumerator EsperarParaAceptarOtroTrabajo()
    {
        _enEsperaDespuesDeTrabajo = true;

        Debug.Log("Espera 2 minutos para aceptar otro trabajo.");

        yield return new WaitForSeconds(_tiempoEsperaDespuesDeTrabajo);

        _enEsperaDespuesDeTrabajo = false;
        _corutinaCooldown = null;

        Debug.Log("Ya puedes aceptar otro trabajo.");
    }

    public bool TrabajoEstaDesbloqueado(Trabajo trabajo)
    {
        if (trabajo == null)
        {
            return false;
        }

        if (Billetera.Instancia == null)
        {
            return false;
        }

        return Billetera.Instancia.NivelActual >= trabajo.NivelRequerido;
    }

    public void VolverAEscenaPrincipal()
    {
        StartCoroutine(EsperarYVolver());
    }

    private IEnumerator EsperarYVolver()
    {
        yield return new WaitForSeconds(_tiempoEsperaParaVolver);
        SceneManager.LoadScene(_nombreEscenaPrincipal);
    }
}