using UnityEngine;

public class Sed : Necesidad
{
    protected override float LeerValorDeGestor()
    {
        if (Billetera.Instancia == null)
        {
            return 100f;
        }

        return Billetera.Instancia.ValorSed;
    }

    protected override void GuardarValorEnGestor(float valor)
    {
        if (Billetera.Instancia == null)
        {
            return;
        }

        Billetera.Instancia.CambiarSed(valor);
    }
}