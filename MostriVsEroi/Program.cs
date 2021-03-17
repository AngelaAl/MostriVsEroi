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
            //var serviceProvider = DIConfiguration.Configurazione();

            //EroeService eroeService = serviceProvider.GetService<EroeService>();



            
            var eroeService = new EroeService(new ADOEroeRepository());

            var giocatore = new Giocatore(){Nome = "Angela", Ruolo = "Admin" };

            //--------------------------------------------------------
            //FUNZIONA GET
            //var eroi = eroeService.GetAllEroiByGiocatori(giocatore);

            //foreach(var eroe in eroi)
            //{
            //    Console.WriteLine(eroe.ToString());
            //}

            var arma = new Arma() { NomeArma = "Bacchetta", Classe = "Mago", PuntiDanno = 5 };
            var eroe = new Eroe("Merlino", "Mago", arma, giocatore.Nome) { };

            //------------------------------------------------------
            //FUNZIONA CREATE
            //eroeService.CreateNewEroe(eroe);

            //----------------------------------------------------------
            //
            var b = eroeService.DeleteEroe(eroe);
        }
    }
}
