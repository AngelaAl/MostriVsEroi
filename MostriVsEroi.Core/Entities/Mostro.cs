using System;
using System.Collections.Generic;
using System.Text;
using MostriVsEroi.Core.Entities.Abstract;

namespace MostriVsEroi.Core.Entities
{
    public class Mostro : Personaggio
    {
        public Livello LivelloMostro { get; set; }

        public Mostro() { }

        public Mostro(string nome, string classe, Arma armaScelta, Livello livello)
        {
            Nome = nome;
            Classe = classe;
            ArmaScelta = armaScelta;
            LivelloMostro = livello;
        }

        //Override ToString
        public override string ToString()
        {
            return $"{Nome} \t Classe: {Classe} \t Arma: {ArmaScelta.NomeArma} \t Livello: {LivelloMostro.Numero} \t Punti Vita: {LivelloMostro.PuntiVita}";
        }

        
    }
}
