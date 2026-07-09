using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instancia { get; private set; }

    [Header("Paneles")]
    [SerializeField] private GameObject _panelCelular;
    [SerializeField] private GameObject _panelInventario;
    [SerializeField] private CelularUI _celularUI;

    [Header("Canvas Global")]
    [SerializeField] private Canvas _canvasGlobal;

    [Header("Escenas donde NO aparece la UI")]
    [SerializeField]
    private List<string> _escenasSinUI = new List<string>
    {
        "JuegoFlappy",
        "JuegoSecuenciaTeclas",
        "MiniJuego",
        "Minijuego"
    };

    [Header("Animación celular")]
    [SerializeField] private RectTransform _rectCelular;
    [SerializeField] private CanvasGroup _canvasGroupCelular;
    [SerializeField] private float _duracionAnimacionCelular = 0.2f;
    [SerializeField] private Vector2 _posicionCelularAbierto = Vector2.zero;
    [SerializeField] private Vector2 _posicionCelularCerrado = new Vector2(0f, -900f);

    private Coroutine _rutinaCelular;
    private bool _celularAbierto;
    private bool _inventarioAbierto;
    private bool _uiPermitida = true;

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Debug.LogWarning("UIManager duplicado detectado. Se eliminó solo el componente, NO el Canvas: " + gameObject.name);
            Destroy(this);
            return;
        }

        Instancia = this;
        BuscarReferenciasGlobales();
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
        BuscarReferenciasGlobales();
        AplicarEstadoUI(SceneManager.GetActiveScene());
    }

    private void ManejarEscenaCargada(Scene escena, LoadSceneMode modo)
    {
        BuscarReferenciasGlobales();
        AplicarEstadoUI(escena);
    }

    private void AplicarEstadoUI(Scene escena)
    {
        _uiPermitida = !EsEscenaSinUI(escena.name);

        CerrarTodo();

        if (_canvasGlobal != null)
        {
            _canvasGlobal.enabled = _uiPermitida;
        }

        if (_uiPermitida)
        {
            BloquearCursor();
        }
        else
        {
            LiberarCursor();
        }
    }

    private bool EsEscenaSinUI(string nombreEscena)
    {
        for (int i = 0; i < _escenasSinUI.Count; i++)
        {
            if (nombreEscena == _escenasSinUI[i])
            {
                return true;
            }
        }

        return false;
    }

    private void BuscarReferenciasGlobales()
    {
        if (_canvasGlobal == null)
        {
            _canvasGlobal = GetComponentInChildren<Canvas>(true);
        }

        if (_canvasGlobal == null && GlobalUI.Instancia != null)
        {
            _canvasGlobal = GlobalUI.Instancia.GetComponentInChildren<Canvas>(true);
        }

        if (_panelCelular == null)
        {
            _panelCelular = BuscarObjetoPorNombreEnGlobalUI("PanelCelular");
        }

        if (_panelInventario == null)
        {
            _panelInventario = BuscarObjetoPorNombreEnGlobalUI("PanelInventario");
        }

        if (_celularUI == null && _panelCelular != null)
        {
            _celularUI = _panelCelular.GetComponent<CelularUI>();

            if (_celularUI == null)
            {
                _celularUI = _panelCelular.GetComponentInChildren<CelularUI>(true);
            }
        }

        if (_rectCelular == null && _panelCelular != null)
        {
            _rectCelular = _panelCelular.GetComponent<RectTransform>();
        }

        if (_canvasGroupCelular == null && _panelCelular != null)
        {
            _canvasGroupCelular = _panelCelular.GetComponent<CanvasGroup>();

            if (_canvasGroupCelular == null)
            {
                _canvasGroupCelular = _panelCelular.AddComponent<CanvasGroup>();
            }
        }
    }

    private GameObject BuscarObjetoPorNombreEnGlobalUI(string nombre)
    {
        Transform raiz = null;

        if (GlobalUI.Instancia != null)
        {
            raiz = GlobalUI.Instancia.transform;
        }
        else
        {
            raiz = transform.root;
        }

        Transform encontrado = BuscarEnHijos(raiz, nombre);

        if (encontrado != null)
        {
            return encontrado.gameObject;
        }

        return null;
    }

    private Transform BuscarEnHijos(Transform padre, string nombre)
    {
        if (padre == null)
        {
            return null;
        }

        if (padre.name == nombre)
        {
            return padre;
        }

        foreach (Transform hijo in padre)
        {
            Transform resultado = BuscarEnHijos(hijo, nombre);

            if (resultado != null)
            {
                return resultado;
            }
        }

        return null;
    }

    private void Update()
    {
        if (!_uiPermitida)
        {
            return;
        }

        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            AlternarCelular();
        }

        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            AlternarInventario();
        }
    }

    private void AlternarCelular()
    {
        if (_panelCelular == null)
        {
            BuscarReferenciasGlobales();
        }

        if (_panelCelular == null)
        {
            Debug.LogWarning("No se encontró PanelCelular. El objeto debe llamarse exactamente PanelCelular y estar dentro de GlobalUI.");
            return;
        }

        if (_celularAbierto)
        {
            CerrarCelularAnimado();
        }
        else
        {
            AbrirCelularAnimado();
        }
    }

    private void AbrirCelularAnimado()
    {
        _celularAbierto = true;
        _inventarioAbierto = false;

        if (_canvasGlobal != null)
        {
            _canvasGlobal.enabled = true;
        }

        if (_panelCelular != null)
        {
            _panelCelular.SetActive(true);
        }

        if (_celularUI != null)
        {
            _celularUI.VolverInicio();
        }

        if (_panelInventario != null)
        {
            _panelInventario.SetActive(false);
        }

        LiberarCursor();

        IniciarAnimacionCelular(
            _posicionCelularCerrado,
            _posicionCelularAbierto,
            0f,
            1f,
            true
        );
    }

    private void CerrarCelularAnimado()
    {
        _celularAbierto = false;

        IniciarAnimacionCelular(
            _posicionCelularAbierto,
            _posicionCelularCerrado,
            1f,
            0f,
            false
        );

        if (!_inventarioAbierto)
        {
            BloquearCursor();
        }
    }

    private void PrepararCelularCerrado()
    {
        _celularAbierto = false;

        if (_rutinaCelular != null)
        {
            StopCoroutine(_rutinaCelular);
            _rutinaCelular = null;
        }

        if (_rectCelular != null)
        {
            _rectCelular.anchoredPosition = _posicionCelularCerrado;
        }

        if (_canvasGroupCelular != null)
        {
            _canvasGroupCelular.alpha = 0f;
            _canvasGroupCelular.interactable = false;
            _canvasGroupCelular.blocksRaycasts = false;
        }

        if (_panelCelular != null)
        {
            _panelCelular.SetActive(false);
        }
    }

    private void PrepararInventarioCerrado()
    {
        _inventarioAbierto = false;

        if (_panelInventario != null)
        {
            _panelInventario.SetActive(false);
        }
    }

    private void CerrarTodo()
    {
        PrepararCelularCerrado();
        PrepararInventarioCerrado();
    }

    private void IniciarAnimacionCelular(
        Vector2 posicionInicial,
        Vector2 posicionFinal,
        float alphaInicial,
        float alphaFinal,
        bool dejarActivoAlFinal)
    {
        if (_rutinaCelular != null)
        {
            StopCoroutine(_rutinaCelular);
        }

        _rutinaCelular = StartCoroutine(AnimarCelular(
            posicionInicial,
            posicionFinal,
            alphaInicial,
            alphaFinal,
            dejarActivoAlFinal
        ));
    }

    private IEnumerator AnimarCelular(
        Vector2 posicionInicial,
        Vector2 posicionFinal,
        float alphaInicial,
        float alphaFinal,
        bool dejarActivoAlFinal)
    {
        float tiempo = 0f;

        if (_canvasGroupCelular != null)
        {
            _canvasGroupCelular.interactable = false;
            _canvasGroupCelular.blocksRaycasts = false;
        }

        while (tiempo < _duracionAnimacionCelular)
        {
            tiempo += Time.unscaledDeltaTime;

            float progreso = tiempo / _duracionAnimacionCelular;
            progreso = Mathf.Clamp01(progreso);
            progreso = progreso * progreso * (3f - 2f * progreso);

            if (_rectCelular != null)
            {
                _rectCelular.anchoredPosition = Vector2.Lerp(posicionInicial, posicionFinal, progreso);
            }

            if (_canvasGroupCelular != null)
            {
                _canvasGroupCelular.alpha = Mathf.Lerp(alphaInicial, alphaFinal, progreso);
            }

            yield return null;
        }

        if (_rectCelular != null)
        {
            _rectCelular.anchoredPosition = posicionFinal;
        }

        if (_canvasGroupCelular != null)
        {
            _canvasGroupCelular.alpha = alphaFinal;
            _canvasGroupCelular.interactable = dejarActivoAlFinal;
            _canvasGroupCelular.blocksRaycasts = dejarActivoAlFinal;
        }

        if (_panelCelular != null && !dejarActivoAlFinal)
        {
            _panelCelular.SetActive(false);
        }

        _rutinaCelular = null;
    }

    private void AlternarInventario()
    {
        if (_panelInventario == null)
        {
            BuscarReferenciasGlobales();
        }

        if (_panelInventario == null)
        {
            Debug.LogWarning("No se encontró PanelInventario. El objeto debe llamarse exactamente PanelInventario y estar dentro de GlobalUI.");
            return;
        }

        _inventarioAbierto = !_inventarioAbierto;
        _panelInventario.SetActive(_inventarioAbierto);

        if (_inventarioAbierto)
        {
            if (_panelCelular != null)
            {
                if (_rutinaCelular != null)
                {
                    StopCoroutine(_rutinaCelular);
                    _rutinaCelular = null;
                }

                _panelCelular.SetActive(false);
                _celularAbierto = false;
            }

            if (_canvasGroupCelular != null)
            {
                _canvasGroupCelular.alpha = 0f;
                _canvasGroupCelular.interactable = false;
                _canvasGroupCelular.blocksRaycasts = false;
            }

            LiberarCursor();
        }
        else
        {
            if (!_celularAbierto)
            {
                BloquearCursor();
            }
        }
    }

    private void LiberarCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void BloquearCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}