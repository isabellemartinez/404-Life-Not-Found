using UnityEngine;
using System;

public abstract class Minijuego : MonoBehaviour
{
    [Header("Multiplicadores de recompensa")]
    [SerializeField] protected int _multiplicadorBits = 20;
    [SerializeField] protected int _multiplicadorXP = 5;

    protected int _nivelActual;
    protected int _recompensaBits;
    protected int _recompensaXP;
    protected bool _juegoActivo;

    public event Action<bool, int, int> OnJuegoTerminado;

    public virtual void IniciarJuego(int nivel)
    {
        _nivelActual = nivel;
        _juegoActivo = true;
        _recompensaBits = CalcularRecompensaBits();
        _recompensaXP = CalcularRecompensaXP();
    }

    public virtual void TerminarJuego(bool gano)
    {
        _juegoActivo = false;

        if (gano)
        {
            OnJuegoTerminado?.Invoke(true, _recompensaBits, _recompensaXP);
        }
        else
        {
            OnJuegoTerminado?.Invoke(false, 0, 0);
        }
    }

    protected virtual int CalcularRecompensaBits()
    {
        return _nivelActual * _multiplicadorBits;
    }

    protected virtual int CalcularRecompensaXP()
    {
        return _nivelActual * _multiplicadorXP;
    }
}