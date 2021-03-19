using MostriVsEroi.ADORepository;
using MostriVsEroi.Core.Entities;
using MostriVsEroi.Services;
using System;

namespace MostriVsEroi
{
    class Program
    {
        static void Main(string[] args)
        {
            bool quit;

            //Titolo
            Scritte.TitoloGioco();

            //Nome giocatore e controllo se già presente nel db
            var giocatore = InterazioneUtente.Giocatore();

            //Partita
            do
            {
                quit = InterazioneUtente.MenuGiocatore(giocatore);
            }
            while (quit == false);

        }
    }
}
