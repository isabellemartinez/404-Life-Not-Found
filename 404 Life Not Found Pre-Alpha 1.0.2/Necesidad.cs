using UnityEngine;
using UnityEngine.UI;

public class Necesidad : MonoBehaviour
{
    [SerializeField] protected Image icono;
    [SerializeField] protected Sprite nivel8;
    [SerializeField] protected Sprite nivel7;
    [SerializeField] protected Sprite nivel6;
    [SerializeField] protected Sprite nivel5;
    [SerializeField] protected Sprite nivel4;
    [SerializeField] protected Sprite nivel3;
    [SerializeField] protected Sprite nivel2;
    [SerializeField] protected Sprite nivel1;

    protected float _valor = 100f;
    protected float _velocidadBase = 0.3f;

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
        if (icono == null)
        {
            return;
        }

        if (_valor > 87.5f)
        {
            icono.sprite = nivel8;
        }
        else if (_valor > 75f)
        {
            icono.sprite = nivel7;
        }
        else if (_valor > 62.5f)
        {
            icono.sprite = nivel6;
        }
        else if (_valor > 50f)
        {
            icono.sprite = nivel5;
        }
        else if (_valor > 37.5f)
        {
            icono.sprite = nivel4;
        }
        else if (_valor > 25f)
        {
            icono.sprite = nivel3;
        }
        else if (_valor > 12.5f)
        {
            icono.sprite = nivel2;
        }
        else
        {
            icono.sprite = nivel1;
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