using System;
using System.Collections.Generic;
using System.Text;

namespace MostriVsEroi.Core.Entities
{
    public class Statistica
    {
        public string NomeEroe { get; set; }

        public int TempoTotaleGioco { get; set; }

        public int PuntiAccumulati { get; set; }

        public string GiocatoreAssegnato { get; set; }

        public Statistica(Eroe eroe)
        {
            NomeEroe = eroe.Nome;
            TempoTotaleGioco = 0;
            PuntiAccumulati = 0;
            GiocatoreAssegnato = eroe.GiocatoreAssegnato;

        }

        public Statistica() { }

        //Override ToString
        public override string ToString()
        {
            return $"Eroe: {NomeEroe} \t PuntiAccumulati: {PuntiAccumulati} \t Tempo Totale di Gioco: {TempoTotaleGioco / 60000} minuti";
        }

        
    }
}
