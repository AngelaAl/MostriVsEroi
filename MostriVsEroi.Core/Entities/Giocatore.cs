using System;
using System.Collections.Generic;
using System.Text;

namespace MostriVsEroi.Core.Entities
{
    public class Giocatore
    {
        public string Nome { get; set; }
        public string Ruolo { get; set; }

        //Costruttore default
        public Giocatore(string nome)
        {
            Nome = nome;
            Ruolo = "Utente";
        }

        public Giocatore() { }

        //Override ToString
        public override string ToString()
        {
            return $"{Nome}";
        }
    }
}
