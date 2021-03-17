using System;
using System.Collections.Generic;
using System.Text;

namespace MostriVsEroi.Core.Entities.Abstract
{
    public abstract class Personaggio
    {
        public string Nome { get; set; }

        public string Classe { get; set; }

        public Arma ArmaScelta{ get; set; }

        //public Livello LivelloPersonaggio { get; set; }


        //METODI
        abstract public override string ToString();

        //Attacca
        public int Attacca()
        {
            return ArmaScelta.PuntiDanno;
        }
        
    }
}
