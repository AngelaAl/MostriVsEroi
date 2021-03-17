using System;
using System.Collections.Generic;
using System.Text;

namespace MostriVsEroi.Core.Entities
{
    public class Classe
    {
        public string nomeClasse { get; set; }

        //Override toString
        public override string ToString()
        {
            return nomeClasse;
        }
    }
}
