using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransicionEscena : MonoBehaviour
{
    public static TransicionEscena Instancia { get; private set; }

    [Header("Transición")]
    [SerializeField] private Image _imagenTransicion;
    [SerializeField] private float _duracionTransicion = 0.5f;
    [SerializeField] private Color _colorTransicion = Color.black;

    private Coroutine _rutinaTransicion;
    private bool _enTransicion;

    public bool EnTransicion { get { return _enTransicion; } }

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

        BuscarImagenTransicion();
        PrepararImagen();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += ManejarEscenaCargada;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ManejarEscenaCargada;
    }

    private void ManejarEscenaCargada(Scene escena, LoadSceneMode modo)
    {
        BuscarImagenTransicion();
        PrepararImagen();
    }

    private void BuscarImagenTransicion()
    {
        if (_imagenTransicion != null)
        {
            return;
        }

        _imagenTransicion = GetComponentInChildren<Image>(true);
    }

    private void PrepararImagen()
    {
        if (_imagenTransicion == null)
        {
            return;
        }

        _colorTransicion.a = 0f;
        _imagenTransicion.color = _colorTransicion;
        _imagenTransicion.gameObject.SetActive(false);
    }

    public void CambiarEscena(string nombreEscena)
    {
        if (_enTransicion)
        {
            return;
        }

        if (_rutinaTransicion != null)
        {
            StopCoroutine(_rutinaTransicion);
        }

        _rutinaTransicion = StartCoroutine(CambiarEscenaRutina(nombreEscena));
    }

    private IEnumerator CambiarEscenaRutina(string nombreEscena)
    {
        _enTransicion = true;

        yield return StartCoroutine(Abrir());

        SceneManager.LoadScene(nombreEscena);

        yield return null;

        BuscarImagenTransicion();

        yield return StartCoroutine(Cerrar());

        _enTransicion = false;
        _rutinaTransicion = null;
    }

    public IEnumerator Abrir()
    {
        BuscarImagenTransicion();

        if (_imagenTransicion == null)
        {
            yield break;
        }

        _imagenTransicion.gameObject.SetActive(true);

        float tiempo = 0f;

        while (tiempo < _duracionTransicion)
        {
            if (_imagenTransicion == null)
            {
                yield break;
            }

            tiempo += Time.unscaledDeltaTime;

            float progreso = tiempo / _duracionTransicion;
            progreso = Mathf.Clamp01(progreso);

            Color colorActual = _colorTransicion;
            colorActual.a = Mathf.Lerp(0f, 1f, progreso);

            _imagenTransicion.color = colorActual;

            yield return null;
        }

        if (_imagenTransicion != null)
        {
            Color colorFinal = _colorTransicion;
            colorFinal.a = 1f;
            _imagenTransicion.color = colorFinal;
        }
    }

    public IEnumerator Cerrar()
    {
        BuscarImagenTransicion();

        if (_imagenTransicion == null)
        {
            yield break;
        }

        _imagenTransicion.gameObject.SetActive(true);

        float tiempo = 0f;

        while (tiempo < _duracionTransicion)
        {
            if (_imagenTransicion == null)
            {
                yield break;
            }

            tiempo += Time.unscaledDeltaTime;

            float progreso = tiempo / _duracionTransicion;
            progreso = Mathf.Clamp01(progreso);

            Color colorActual = _colorTransicion;
            colorActual.a = Mathf.Lerp(1f, 0f, progreso);

            _imagenTransicion.color = colorActual;

            yield return null;
        }

        if (_imagenTransicion != null)
        {
            Color colorFinal = _colorTransicion;
            colorFinal.a = 0f;
            _imagenTransicion.color = colorFinal;
            _imagenTransicion.gameObject.SetActive(false);
        }
    }
}