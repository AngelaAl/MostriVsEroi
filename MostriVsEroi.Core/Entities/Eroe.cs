using System;
using System.Collections.Generic;
using System.Text;
using MostriVsEroi.Core.Entities.Abstract;

namespace MostriVsEroi.Core.Entities
{
    public class Eroe : Personaggio
    {
        //Campo
        private List<Livello> Livelli = new List<Livello>();

        //Proprietà
        public int PuntiVita { get; set; }

        public int PuntiAccumulati { get; set; }

        //Costruttore
        public Eroe(string nome, string classe, Arma arma)
        {
            Nome = nome;
            Classe = classe;
            ArmaScelta = arma;
            LivelloPersonaggio = Livelli[0];
            PuntiVita = Livelli[0].PuntiVita;
            PuntiAccumulati = 0;
        }

        //Override ToString
        public override string ToString()
        {
            return $"{Nome} \t Classe: {Classe} \t Arma: {ArmaScelta} \t Livello: {LivelloPersonaggio.Numero} \t PV: {PuntiVita} \t Punti accumulati: {PuntiAccumulati}";
        }
    }
}
