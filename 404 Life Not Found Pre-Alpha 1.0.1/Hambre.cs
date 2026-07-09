using UnityEngine;

public class Hambre : Necesidad
{
    protected override float LeerValorDeGestor()
    {
        if (Billetera.Instancia == null)
        {
            return 100f;
        }

        return Billetera.Instancia.ValorHambre;
    }

    protected override void GuardarValorEnGestor(float valor)
    {
        if (Billetera.Instancia == null)
        {
            return;
        }

        Billetera.Instancia.CambiarHambre(valor);
    }
}