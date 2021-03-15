using System;
using System.Collections.Generic;
using System.Text;
using MostriVsEroi.Core.Entities.Abstract;

namespace MostriVsEroi.Core.Entities
{
    public class Mostro : Personaggio
    {

        //Override ToString
        public override string ToString()
        {
            return $"{Nome} \t Classe: {Classe} \t Arma: {ArmaScelta} \t Livello: {LivelloPersonaggio.Numero}";
        }
    }
}
