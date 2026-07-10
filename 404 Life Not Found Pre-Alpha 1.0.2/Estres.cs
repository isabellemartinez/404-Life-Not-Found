using UnityEngine;

public class Estres : Necesidad
{
    protected override float ObtenerVelocidad()
    {
        if (Billetera.Instancia == null)
        {
            return _velocidadBase;
        }

        if (Billetera.Instancia.Bits < 25f)
        {
            return _velocidadBase * 3.5f;
        }
        else if (Billetera.Instancia.Bits < 50f)
        {
            return _velocidadBase * 2.5f;
        }
        else if (Billetera.Instancia.Bits < 100f)
        {
            return _velocidadBase * 1.7f;
        }
        else
        {
            return _velocidadBase;
        }
    }

    protected override float LeerValorDeGestor()
    {
        if (Billetera.Instancia == null)
        {
            return 100f;
        }

        return Billetera.Instancia.ValorEstres;
    }

    protected override void GuardarValorEnGestor(float valor)
    {
        if (Billetera.Instancia == null)
        {
            return;
        }

        Billetera.Instancia.CambiarEstres(valor);
    }
}