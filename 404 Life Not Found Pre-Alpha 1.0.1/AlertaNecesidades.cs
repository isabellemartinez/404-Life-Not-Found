using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class AlertaNecesidades : MonoBehaviour
{
    [Header("Mensaje")]
    [SerializeField] private GameObject _panelMensaje;
    [SerializeField] private TextMeshProUGUI _textoMensaje;

    [Header("Configuración")]
    [SerializeField] private float _tiempoParaRecuperarse = 120f;
    [SerializeField] private bool _mostrarContador = true;

    [Header("Mensajes")]
    [SerializeField] private string _mensajeSedCero = "Fren, busca agua antes que te apagues.";
    [SerializeField] private string _mensajeHambreCero = "Mi rey, ese estómago está pidiendo auxilio. Busca algo pa' comer.";
    [SerializeField] private string _mensajeEstresCero = "Respira, compa. Estás más cargado que Diablo Rojo en hora pico.";

    private bool _sedCritica;
    private bool _hambreCritica;
    private bool _estresCritico;

    private float _tiempoSed;
    private float _tiempoHambre;
    private float _tiempoEstres;

    private void Awake()
    {
        _tiempoSed = _tiempoParaRecuperarse;
        _tiempoHambre = _tiempoParaRecuperarse;
        _tiempoEstres = _tiempoParaRecuperarse;

        OcultarMensaje();

        Debug.Log("AlertaNecesidades inició correctamente.");
    }

    private void Update()
    {
        if (Billetera.Instancia == null)
        {
            Debug.LogWarning("No existe Billetera.Instancia.");
            return;
        }

        RevisarNecesidades();
        ActualizarMensaje();
    }



    private void RevisarNecesidades()
    {
        RevisarSed();
        RevisarHambre();
        RevisarEstres();
    }

    private void RevisarSed()
    {
        if (Billetera.Instancia.ValorSed <= 0f)
        {
            if (!_sedCritica)
            {
                _sedCritica = true;
                _tiempoSed = _tiempoParaRecuperarse;
                Debug.Log("Sed crítica iniciada.");
            }

            _tiempoSed -= Time.deltaTime;

            if (_tiempoSed <= 0f)
            {
                _tiempoSed = 0f;
                Debug.LogWarning("El jugador no recuperó la sed a tiempo.");
            }
        }
        else
        {
            if (_sedCritica)
            {
                Debug.Log("Sed recuperada.");
            }

            _sedCritica = false;
            _tiempoSed = _tiempoParaRecuperarse;
        }
    }

    private void RevisarHambre()
    {
        if (Billetera.Instancia.ValorHambre <= 0f)
        {
            if (!_hambreCritica)
            {
                _hambreCritica = true;
                _tiempoHambre = _tiempoParaRecuperarse;
                Debug.Log("Hambre crítica iniciada.");
            }

            _tiempoHambre -= Time.deltaTime;

            if (_tiempoHambre <= 0f)
            {
                _tiempoHambre = 0f;
                Debug.LogWarning("El jugador no recuperó el hambre a tiempo.");
            }
        }
        else
        {
            if (_hambreCritica)
            {
                Debug.Log("Hambre recuperada.");
            }

            _hambreCritica = false;
            _tiempoHambre = _tiempoParaRecuperarse;
        }
    }

    private void RevisarEstres()
    {
        if (Billetera.Instancia.ValorEstres <= 0f)
        {
            if (!_estresCritico)
            {
                _estresCritico = true;
                _tiempoEstres = _tiempoParaRecuperarse;
                Debug.Log("Estrés crítico iniciado.");
            }

            _tiempoEstres -= Time.deltaTime;

            if (_tiempoEstres <= 0f)
            {
                _tiempoEstres = 0f;
                Debug.LogWarning("El jugador no recuperó el estrés a tiempo.");
            }
        }
        else
        {
            if (_estresCritico)
            {
                Debug.Log("Estrés recuperado.");
            }

            _estresCritico = false;
            _tiempoEstres = _tiempoParaRecuperarse;
        }
    }

    private void ActualizarMensaje()
    {
        if (_sedCritica)
        {
            MostrarMensaje(_mensajeSedCero, _tiempoSed);
            return;
        }

        if (_hambreCritica)
        {
            MostrarMensaje(_mensajeHambreCero, _tiempoHambre);
            return;
        }

        if (_estresCritico)
        {
            MostrarMensaje(_mensajeEstresCero, _tiempoEstres);
            return;
        }

        OcultarMensaje();
    }

    private void MostrarMensaje(string mensaje, float tiempoRestante)
    {
        if (_panelMensaje != null)
        {
            _panelMensaje.SetActive(true);
        }

        if (_textoMensaje != null)
        {
            if (_mostrarContador)
            {
                int segundos = Mathf.CeilToInt(tiempoRestante);
                _textoMensaje.text = mensaje + "\n" + "\nTiempo restante: " +  segundos + " s";
            }
            else
            {
                _textoMensaje.text = mensaje;
            }
        }
    }

    private void OcultarMensaje()
    {
        if (_panelMensaje != null)
        {
            _panelMensaje.SetActive(false);
        }

        if (_textoMensaje != null)
        {
            _textoMensaje.text = "";
        }
    }
}