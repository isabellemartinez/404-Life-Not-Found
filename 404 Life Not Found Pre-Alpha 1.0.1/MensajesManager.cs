using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class MensajesManager : MonoBehaviour
{
    public static MensajesManager Instance { get; private set; }
    public static MensajesManager Instancia { get { return Instance; } }

    [Header("Referencias UI")]
    [SerializeField] private GameObject _panelMensajeINT;
    [SerializeField] private TMP_Text _textoMensajeINT;

    [Header("Configuración")]
    [SerializeField] private float _duracionDefault = 3f;

    private Coroutine _mensajeActual;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += ManejarEscenaCargada;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ManejarEscenaCargada;
    }

    private void Start()
    {
        BuscarReferenciasUI();

        if (_panelMensajeINT != null)
        {
            _panelMensajeINT.SetActive(false);
        }
    }

    private void ManejarEscenaCargada(Scene escena, LoadSceneMode modo)
    {
        if (_mensajeActual != null)
        {
            StopCoroutine(_mensajeActual);
            _mensajeActual = null;
        }

        BuscarReferenciasUI();

        if (_panelMensajeINT != null)
        {
            _panelMensajeINT.SetActive(false);
        }
    }

    private void BuscarReferenciasUI()
    {
        if (_panelMensajeINT != null && _textoMensajeINT != null)
        {
            return;
        }

        BuscarEnMisHijos();

        if (_panelMensajeINT != null && _textoMensajeINT != null)
        {
            return;
        }

        BuscarEnEscenaActiva();
    }

    private void BuscarEnMisHijos()
    {
        if (_panelMensajeINT == null)
        {
            Transform panelEncontrado = BuscarEnHijosPorNombre(transform, "PanelMensajeINT");

            if (panelEncontrado != null)
            {
                _panelMensajeINT = panelEncontrado.gameObject;
            }
        }

        if (_textoMensajeINT == null)
        {
            Transform textoEncontrado = BuscarEnHijosPorNombre(transform, "TextoMensajeINT");

            if (textoEncontrado != null)
            {
                _textoMensajeINT = textoEncontrado.GetComponent<TMP_Text>();
            }
        }
    }

    private void BuscarEnEscenaActiva()
    {
        Scene escenaActual = SceneManager.GetActiveScene();
        GameObject[] raices = escenaActual.GetRootGameObjects();

        foreach (GameObject raiz in raices)
        {
            if (_panelMensajeINT == null)
            {
                Transform panelEncontrado = BuscarEnHijosPorNombre(raiz.transform, "PanelMensajeINT");

                if (panelEncontrado != null)
                {
                    _panelMensajeINT = panelEncontrado.gameObject;
                }
            }

            if (_textoMensajeINT == null)
            {
                Transform textoEncontrado = BuscarEnHijosPorNombre(raiz.transform, "TextoMensajeINT");

                if (textoEncontrado != null)
                {
                    _textoMensajeINT = textoEncontrado.GetComponent<TMP_Text>();
                }
            }

            if (_panelMensajeINT != null && _textoMensajeINT != null)
            {
                return;
            }
        }
    }

    private Transform BuscarEnHijosPorNombre(Transform padre, string nombre)
    {
        if (padre.name == nombre)
        {
            return padre;
        }

        foreach (Transform hijo in padre)
        {
            Transform resultado = BuscarEnHijosPorNombre(hijo, nombre);

            if (resultado != null)
            {
                return resultado;
            }
        }

        return null;
    }

    public void MostrarMensaje(string mensaje)
    {
        MostrarMensaje(mensaje, _duracionDefault);
    }

    public void MostrarMensaje(string mensaje, float duracion)
    {
        BuscarReferenciasUI();

        if (_panelMensajeINT == null || _textoMensajeINT == null)
        {
            Debug.LogWarning("MensajesManager: no encontró PanelMensajeINT o TextoMensajeINT.");
            Debug.Log(mensaje);
            return;
        }

        if (_mensajeActual != null)
        {
            StopCoroutine(_mensajeActual);
        }

        _mensajeActual = StartCoroutine(MostrarMensajeRutina(mensaje, duracion));
    }

    private IEnumerator MostrarMensajeRutina(string mensaje, float duracion)
    {
        if (_panelMensajeINT == null || _textoMensajeINT == null)
        {
            yield break;
        }

        _textoMensajeINT.text = mensaje;
        _panelMensajeINT.SetActive(true);

        yield return new WaitForSeconds(duracion);

        if (_panelMensajeINT != null)
        {
            _panelMensajeINT.SetActive(false);
        }

        _mensajeActual = null;
    }
}