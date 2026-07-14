using UnityEngine;
using UnityEngine.UI;

public class Necesidad : MonoBehaviour
{
    [Header("Icono de necesidad")]
    [SerializeField] protected Image _icono;

    [Header("Sprites por nivel")]
    [SerializeField] protected Sprite _nivel8;
    [SerializeField] protected Sprite _nivel7;
    [SerializeField] protected Sprite _nivel6;
    [SerializeField] protected Sprite _nivel5;
    [SerializeField] protected Sprite _nivel4;
    [SerializeField] protected Sprite _nivel3;
    [SerializeField] protected Sprite _nivel2;
    [SerializeField] protected Sprite _nivel1;

    protected float _valor = 100f;
    protected float _velocidadBase = 0.3f;

    public float Valor { get { return _valor; } }

    protected virtual void Start()
    {
        _valor = LeerValorDeGestor();
        _valor = Mathf.Clamp(_valor, 0f, 100f);
        GuardarValorEnGestor(_valor);
        ActualizarIcono();
    }

    protected virtual void Update()
    {
        _valor = LeerValorDeGestor();
        _valor = Mathf.Clamp(_valor, 0f, 100f);

        float velocidad = ObtenerVelocidad();
        _valor -= velocidad * Time.deltaTime;
        _valor = Mathf.Clamp(_valor, 0f, 100f);

        GuardarValorEnGestor(_valor);
        ActualizarIcono();
    }

    protected virtual float ObtenerVelocidad()
    {
        return _velocidadBase;
    }

    protected virtual float LeerValorDeGestor()
    {
        return 100f;
    }

    protected virtual void GuardarValorEnGestor(float valor)
    {
    }

    protected void ActualizarIcono()
    {
        if (_icono == null)
        {
            return;
        }

        if (_valor > 87.5f)
        {
            _icono.sprite = _nivel8;
        }
        else if (_valor > 75f)
        {
            _icono.sprite = _nivel7;
        }
        else if (_valor > 62.5f)
        {
            _icono.sprite = _nivel6;
        }
        else if (_valor > 50f)
        {
            _icono.sprite = _nivel5;
        }
        else if (_valor > 37.5f)
        {
            _icono.sprite = _nivel4;
        }
        else if (_valor > 25f)
        {
            _icono.sprite = _nivel3;
        }
        else if (_valor > 12.5f)
        {
            _icono.sprite = _nivel2;
        }
        else
        {
            _icono.sprite = _nivel1;
        }
    }

    public void ReducirValor(float cantidad)
    {
        _valor = LeerValorDeGestor();
        _valor -= cantidad;
        _valor = Mathf.Clamp(_valor, 0f, 100f);
        GuardarValorEnGestor(_valor);
        ActualizarIcono();
    }

    public void AumentarValor(float cantidad)
    {
        _valor = LeerValorDeGestor();
        _valor += cantidad;
        _valor = Mathf.Clamp(_valor, 0f, 100f);
        GuardarValorEnGestor(_valor);
        ActualizarIcono();
    }
}
