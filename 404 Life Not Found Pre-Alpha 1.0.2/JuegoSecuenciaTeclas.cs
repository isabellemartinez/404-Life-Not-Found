using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class SpriteTecla
{
    public Key tecla;
    public Sprite sprite;
}

public class JuegoSecuenciaTeclas : Minijuego
{
    [SerializeField] private List<Key> _teclasDisponibles;
    [SerializeField] private List<SpriteTecla> _spritesPorTecla;
    [SerializeField] private GameObject _prefabIcono;
    [SerializeField] private Transform _contenedorIconos;
    [SerializeField] private TextMeshProUGUI _textoNivel;
    [SerializeField] private TextMeshProUGUI _textoResultado;

    private List<Key> _secuenciaGenerada;
    private List<GameObject> _iconosInstanciados;
    private int _indiceActual;

    public override void IniciarJuego(int nivel)
    {
        base.IniciarJuego(nivel);

        if (_textoResultado != null)
        {
            _textoResultado.text = "";
        }

        if (_textoNivel != null)
        {
            _textoNivel.text = "Nivel " + nivel;
        }

        int cantidadTeclas = 3;

        if (nivel == 1)
        {
            cantidadTeclas = 3;
        }
        else if (nivel == 2)
        {
            cantidadTeclas = 4;
        }
        else if (nivel == 3)
        {
            cantidadTeclas = 6;
        }

        GenerarSecuencia(cantidadTeclas);
        InstanciarIconos();
        _indiceActual = 0;
    }

    private void GenerarSecuencia(int cantidad)
    {
        _secuenciaGenerada = new List<Key>();

        for (int i = 0; i < cantidad; i++)
        {
            int indiceAleatorio = UnityEngine.Random.Range(0, _teclasDisponibles.Count);
            Key teclaElegida = _teclasDisponibles[indiceAleatorio];
            _secuenciaGenerada.Add(teclaElegida);
        }

        MostrarSecuenciaEnConsola();
    }

    private void MostrarSecuenciaEnConsola()
    {
        string secuenciaTexto = "Secuencia generada: ";

        foreach (Key tecla in _secuenciaGenerada)
        {
            secuenciaTexto += tecla.ToString() + " ";
        }

        Debug.Log(secuenciaTexto);
    }

    private void InstanciarIconos()
    {
        LimpiarIconos();

        _iconosInstanciados = new List<GameObject>();

        foreach (Key tecla in _secuenciaGenerada)
        {
            GameObject icono = Instantiate(_prefabIcono, _contenedorIconos);
            Image imagenIcono = icono.GetComponent<Image>();

            Sprite spriteEncontrado = BuscarSpritePorTecla(tecla);

            if (imagenIcono != null)
            {
                if (spriteEncontrado != null)
                {
                    imagenIcono.sprite = spriteEncontrado;
                }

                imagenIcono.color = Color.white;
            }

            _iconosInstanciados.Add(icono);
        }
    }

    private Sprite BuscarSpritePorTecla(Key tecla)
    {
        foreach (SpriteTecla entrada in _spritesPorTecla)
        {
            if (entrada.tecla == tecla)
            {
                return entrada.sprite;
            }
        }

        return null;
    }

    private void LimpiarIconos()
    {
        if (_iconosInstanciados == null)
        {
            return;
        }

        foreach (GameObject icono in _iconosInstanciados)
        {
            Destroy(icono);
        }

        _iconosInstanciados.Clear();
    }

    private void Update()
    {
        if (!_juegoActivo)
        {
            return;
        }

        if (_indiceActual >= _secuenciaGenerada.Count)
        {
            return;
        }

        Key teclaEsperada = _secuenciaGenerada[_indiceActual];

        if (Keyboard.current[teclaEsperada].wasPressedThisFrame)
        {
            MarcarIconoCorrecto(_indiceActual);
            _indiceActual++;

            if (_indiceActual >= _secuenciaGenerada.Count)
            {
                TerminarJuego(true);
            }
        }
        else
        {
            VerificarTeclaIncorrecta();
        }
    }

    private void MarcarIconoCorrecto(int indice)
    {
        Image imagenIcono = _iconosInstanciados[indice].GetComponent<Image>();

        if (imagenIcono != null)
        {
            imagenIcono.color = Color.green;
        }
    }

    private void MarcarIconoIncorrecto(int indice)
    {
        Image imagenIcono = _iconosInstanciados[indice].GetComponent<Image>();

        if (imagenIcono != null)
        {
            imagenIcono.color = Color.red;
        }
    }

    private void VerificarTeclaIncorrecta()
    {
        foreach (Key tecla in _teclasDisponibles)
        {
            if (Keyboard.current[tecla].wasPressedThisFrame)
            {
                if (tecla != _secuenciaGenerada[_indiceActual])
                {
                    MarcarIconoIncorrecto(_indiceActual);
                    TerminarJuego(false);
                }
            }
        }
    }

    public override void TerminarJuego(bool gano)
    {
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

    private void LimpiarIconosDiferido()
    {
        LimpiarIconos();
    }
}