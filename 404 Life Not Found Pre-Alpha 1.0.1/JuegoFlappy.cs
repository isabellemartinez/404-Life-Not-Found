using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class JuegoFlappy : Minijuego
{
    [Header("Jugador")]
    [SerializeField] private Rigidbody2D _rbJugador;
    [SerializeField] private Transform _posicionInicialJugador;
    [SerializeField] private float _fuerzaSalto = 6f;

    [Header("Obstáculos")]
    [SerializeField] private GameObject _prefabObstaculo;
    [SerializeField] private Transform _puntoSpawnObstaculo;
    [SerializeField] private Transform _contenedorObstaculos;
    [SerializeField] private float _rangoAleatorioY = 2f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _textoNivel;
    [SerializeField] private TextMeshProUGUI _textoPuntaje;
    [SerializeField] private TextMeshProUGUI _textoResultado;

    private int _puntosActuales;
    private int _puntosObjetivo;
    private float _velocidadObstaculos;
    private float _tiempoEntreObstaculos;

    private bool _esperandoInicio;

    private Coroutine _rutinaObstaculos;
    private List<GameObject> _obstaculosActivos = new List<GameObject>();

    public override void IniciarJuego(int nivel)
    {
        base.IniciarJuego(nivel);

        ConfigurarDificultad(nivel);

        _puntosActuales = 0;
        _esperandoInicio = true;

        if (_textoResultado != null)
        {
            _textoResultado.text = "Presiona ESPACIO para empezar";
        }

        if (_textoNivel != null)
        {
            _textoNivel.text = "Nivel " + nivel;
        }

        ActualizarTextoPuntaje();

        ReiniciarJugadorCongelado();
        LimpiarObstaculos();
    }

    private void Update()
    {
        if (!_juegoActivo)
        {
            return;
        }

        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            if (_esperandoInicio)
            {
                EmpezarMovimiento();
            }

            Saltar();
        }
    }

    private void EmpezarMovimiento()
    {
        _esperandoInicio = false;

        if (_textoResultado != null)
        {
            _textoResultado.text = "";
        }

        if (_rbJugador != null)
        {
            _rbJugador.simulated = true;
            _rbJugador.linearVelocity = Vector2.zero;
            _rbJugador.angularVelocity = 0f;
        }

        if (_rutinaObstaculos != null)
        {
            StopCoroutine(_rutinaObstaculos);
        }

        _rutinaObstaculos = StartCoroutine(GenerarObstaculos());
    }

    private void Saltar()
    {
        if (_rbJugador == null)
        {
            return;
        }

        if (!_rbJugador.simulated)
        {
            return;
        }

        _rbJugador.linearVelocity = Vector2.zero;
        _rbJugador.AddForce(Vector2.up * _fuerzaSalto, ForceMode2D.Impulse);
    }

    private void ReiniciarJugadorCongelado()
    {
        if (_rbJugador == null)
        {
            return;
        }

        _rbJugador.simulated = false;
        _rbJugador.linearVelocity = Vector2.zero;
        _rbJugador.angularVelocity = 0f;

        if (_posicionInicialJugador != null)
        {
            _rbJugador.position = _posicionInicialJugador.position;
            _rbJugador.transform.position = _posicionInicialJugador.position;
        }
    }

    private void ConfigurarDificultad(int nivel)
    {
        if (nivel == 1)
        {
            _puntosObjetivo = 5;
            _velocidadObstaculos = 3f;
            _tiempoEntreObstaculos = 2f;
        }
        else if (nivel == 2)
        {
            _puntosObjetivo = 8;
            _velocidadObstaculos = 4f;
            _tiempoEntreObstaculos = 1.6f;
        }
        else
        {
            _puntosObjetivo = 12;
            _velocidadObstaculos = 5f;
            _tiempoEntreObstaculos = 1.3f;
        }
    }

    private IEnumerator GenerarObstaculos()
    {
        yield return new WaitForSeconds(1f);

        while (_juegoActivo && !_esperandoInicio)
        {
            CrearObstaculo();
            yield return new WaitForSeconds(_tiempoEntreObstaculos);
        }
    }

    private void CrearObstaculo()
    {
        if (_prefabObstaculo == null || _puntoSpawnObstaculo == null)
        {
            return;
        }

        Vector3 posicion = _puntoSpawnObstaculo.position;
        posicion.y += Random.Range(-_rangoAleatorioY, _rangoAleatorioY);

        GameObject obstaculo = Instantiate(_prefabObstaculo, posicion, Quaternion.identity, _contenedorObstaculos);

        ObstaculoFlappy scriptObstaculo = obstaculo.GetComponent<ObstaculoFlappy>();

        if (scriptObstaculo != null)
        {
            scriptObstaculo.Inicializar(_velocidadObstaculos);
        }

        _obstaculosActivos.Add(obstaculo);
    }

    public void SumarPunto()
    {
        if (!_juegoActivo || _esperandoInicio)
        {
            return;
        }

        _puntosActuales++;
        ActualizarTextoPuntaje();

        if (_puntosActuales >= _puntosObjetivo)
        {
            TerminarJuego(true);
        }
    }

    public void Perder()
    {
        if (!_juegoActivo || _esperandoInicio)
        {
            return;
        }

        TerminarJuego(false);
    }

    private void ActualizarTextoPuntaje()
    {
        if (_textoPuntaje != null)
        {
            _textoPuntaje.text = _puntosActuales + " / " + _puntosObjetivo;
        }
    }

    public override void TerminarJuego(bool gano)
    {
        if (!_juegoActivo)
        {
            return;
        }

        _esperandoInicio = false;

        if (_rutinaObstaculos != null)
        {
            StopCoroutine(_rutinaObstaculos);
            _rutinaObstaculos = null;
        }

        DetenerObstaculos();

        if (_rbJugador != null)
        {
            _rbJugador.linearVelocity = Vector2.zero;
            _rbJugador.angularVelocity = 0f;
            _rbJugador.simulated = false;
        }

        base.TerminarJuego(gano);

        if (_textoResultado != null)
        {
            if (gano)
            {
                _textoResultado.text = "Ganaste! +" + _recompensaBits + " Bits, +" + _recompensaXP + " XP";
            }
            else
            {
                _textoResultado.text = "Perdiste";
            }
        }
    }

    private void DetenerObstaculos()
    {
        foreach (GameObject obstaculo in _obstaculosActivos)
        {
            if (obstaculo == null)
            {
                continue;
            }

            ObstaculoFlappy scriptObstaculo = obstaculo.GetComponent<ObstaculoFlappy>();

            if (scriptObstaculo != null)
            {
                scriptObstaculo.Detener();
            }
        }
    }

    private void LimpiarObstaculos()
    {
        foreach (GameObject obstaculo in _obstaculosActivos)
        {
            if (obstaculo != null)
            {
                Destroy(obstaculo);
            }
        }

        _obstaculosActivos.Clear();
    }
}