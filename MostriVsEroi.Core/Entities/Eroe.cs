using System;
using System.Collections.Generic;
using System.Text;
using MostriVsEroi.Core.Entities.Abstract;

namespace MostriVsEroi.Core.Entities
{
    public class Eroe : Personaggio
    {
        //Campo
        //private List<Livello> Livelli = new List<Livello>();

        //Proprietà
        public int PuntiVita { get; set; }

        public int PuntiAccumulati { get; set; }

        public string GiocatoreAssegnato { get; set; }

        public int Livello { get; set; }

       

        //Costruttore
        public Eroe(string nome, string classe, Arma arma, string giocatore)
        {
            Nome = nome;
            Classe = classe;
            ArmaScelta = arma;
            Livello = 1;
            PuntiVita = 20;
            PuntiAccumulati = 0;
            GiocatoreAssegnato = giocatore;
            
        }

        public Eroe() { }

        //Override ToString
        public override string ToString()
        {
            return $"{Nome} \t Classe: {Classe} \t Arma: {ArmaScelta.NomeArma} \t Livello: {Livello} \t PV: {PuntiVita} \t Punti accumulati: {PuntiAccumulati} \t Giocatore: {GiocatoreAssegnato}";
        }

        //Fuga
        public bool Fuga()
        {
            Random x = new Random();
            int numero = x.Next(1, 3);
            if(numero%2 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

       
    }
}
