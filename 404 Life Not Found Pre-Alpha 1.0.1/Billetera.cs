using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Billetera : MonoBehaviour
{
    [SerializeField] private float _valorSed = 100f;
    [SerializeField] private float _valorHambre = 100f;
    [SerializeField] private float _valorEstres = 100f;

    public float ValorSed { get { return _valorSed; } }
    public float ValorHambre { get { return _valorHambre; } }
    public float ValorEstres { get { return _valorEstres; } }

    public void CambiarSed(float valor)
    {
        _valorSed = Mathf.Clamp(valor, 0f, 100f);
    }

    public void CambiarHambre(float valor)
    {
        _valorHambre = Mathf.Clamp(valor, 0f, 100f);
    }

    public void CambiarEstres(float valor)
    {
        _valorEstres = Mathf.Clamp(valor, 0f, 100f);
    }

    public static Billetera Instancia { get; private set; }

    private const string CLAVE_BITS = "Bits";
    private const string CLAVE_EXPERIENCIA = "Experiencia";

    private float _bits = 0f;
    private float _maxBits = 999999999f;

    private int _experiencia = 0;
    private int _nivelActual = 1;

    [SerializeField] private TextMeshProUGUI _textoBits;
    [SerializeField] private TextMeshProUGUI _textoExperiencia;
    [SerializeField] private TextMeshProUGUI _textoNivel;

    [Header("Umbrales de XP por nivel")]
    [SerializeField] private List<int> _umbralesNivel = new List<int> { 0, 100, 250, 500, 1000 };

    public float Bits { get { return _bits; } }
    public int Experiencia { get { return _experiencia; } }
    public int NivelActual { get { return _nivelActual; } }

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

        CargarDatos();
        BuscarReferenciasUI(SceneManager.GetActiveScene());
        ActualizarNivel();
        ActualizarUI();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += ManejarEscenaCargada;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ManejarEscenaCargada;
    }

    private void OnApplicationQuit()
    {
        GuardarDatos();
    }

    private void ManejarEscenaCargada(Scene escena, LoadSceneMode modo)
    {
        BuscarReferenciasUI(escena);
        ActualizarUI();
    }

    private void BuscarReferenciasUI(Scene escena)
    {
        if (_textoBits != null && _textoExperiencia != null && _textoNivel != null)
        {
            return;
        }

        GameObject[] raices = escena.GetRootGameObjects();

        foreach (GameObject raiz in raices)
        {
            if (_textoBits == null)
            {
                Transform encontrado = BuscarEnHijos(raiz.transform, "TextoBits");

                if (encontrado != null)
                {
                    _textoBits = encontrado.GetComponent<TextMeshProUGUI>();
                }
            }

            if (_textoExperiencia == null)
            {
                Transform encontrado = BuscarEnHijos(raiz.transform, "TextoExperiencia");

                if (encontrado != null)
                {
                    _textoExperiencia = encontrado.GetComponent<TextMeshProUGUI>();
                }
            }

            if (_textoNivel == null)
            {
                Transform encontrado = BuscarEnHijos(raiz.transform, "TextoNivel");

                if (encontrado != null)
                {
                    _textoNivel = encontrado.GetComponent<TextMeshProUGUI>();
                }
            }
        }
    }

    private Transform BuscarEnHijos(Transform padre, string identificador)
    {
        if (padre.name == identificador || padre.gameObject.tag == identificador)
        {
            return padre;
        }

        foreach (Transform hijo in padre)
        {
            Transform resultado = BuscarEnHijos(hijo, identificador);

            if (resultado != null)
            {
                return resultado;
            }
        }

        return null;
    }

    public bool Gastar(float cantidad)
    {
        if (cantidad <= 0)
        {
            return false;
        }

        if (_bits >= cantidad)
        {
            _bits -= cantidad;
            ActualizarUI();
            GuardarDatos();
            return true;
        }

        return false;
    }

    public void Agregar(float cantidad)
    {
        if (cantidad <= 0)
        {
            return;
        }

        _bits += cantidad;
        _bits = Mathf.Clamp(_bits, 0, _maxBits);

        ActualizarUI();
        GuardarDatos();
    }

    public void AgregarExperiencia(int cantidad)
    {
        if (cantidad == 0)
        {
            return;
        }

        _experiencia += cantidad;

        if (_experiencia < 0)
        {
            _experiencia = 0;
        }

        ActualizarNivel();
        ActualizarUI();
        GuardarDatos();
    }

    private void ActualizarNivel()
    {
        int nivelCalculado = 1;

        for (int i = 0; i < _umbralesNivel.Count; i++)
        {
            if (_experiencia >= _umbralesNivel[i])
            {
                nivelCalculado = i + 1;
            }
        }

        _nivelActual = nivelCalculado;
    }

    private void ActualizarUI()
    {
        if (_textoBits != null)
        {
            _textoBits.text = _bits.ToString("0") + " Bits";
        }

        if (_textoExperiencia != null)
        {
            _textoExperiencia.text = _experiencia + " XP";
        }

        if (_textoNivel != null)
        {
            _textoNivel.text = "Nivel " + _nivelActual;
        }
    }

    public void GuardarDatos()
    {
        PlayerPrefs.SetFloat(CLAVE_BITS, _bits);
        PlayerPrefs.SetInt(CLAVE_EXPERIENCIA, _experiencia);
        PlayerPrefs.Save();
    }

    public void CargarDatos()
    {
        _bits = PlayerPrefs.GetFloat(CLAVE_BITS, 0f);
        _experiencia = PlayerPrefs.GetInt(CLAVE_EXPERIENCIA, 0);

        ActualizarNivel();
    }

    public void BorrarDatosGuardados()
    {
        PlayerPrefs.DeleteKey(CLAVE_BITS);
        PlayerPrefs.DeleteKey(CLAVE_EXPERIENCIA);
        PlayerPrefs.Save();

        _bits = 0f;
        _experiencia = 0;
        _nivelActual = 1;

        ActualizarUI();

        Debug.Log("Datos de billetera borrados.");
    }
}