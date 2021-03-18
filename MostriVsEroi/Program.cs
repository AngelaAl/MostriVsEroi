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
            var mostroService = new MostroService(new ADOMostroRepository());
            var giocatoreService = new GiocatoreService(new ADOGiocatoreRepository());
            var statisticaService = new StatisticaService(new ADOStatisticaRepository());

            //var giocatore = new Giocatore(){Nome = "Angela", Ruolo = "Admin" };

            //--------------------------------------------------------
            //FUNZIONA GET
            //var eroi = eroeService.GetAllEroiByGiocatori(giocatore);

            //foreach (var eroe in eroi)
            //{
            //    Console.WriteLine(eroe.ToString());
            //}

            //var arma = new Arma() { NomeArma = "Spada", Classe = "Guerriero", PuntiDanno = 10 };
            //var eroe = new Eroe("Pina", "Guerriero", arma, "Angela") { };

            //------------------------------------------------------
            //FUNZIONA CREATE
            //eroeService.CreateNewEroe(eroe);

            //----------------------------------------------------------
            //FUNZIONA DELETE
            //var b = eroeService.DeleteEroe(eroe);

            //------------------------------------------------------------
            //FUNZIONA UPDATE
            //eroe.PuntiAccumulati = 40;
            //var b = eroeService.UpdateEroe(eroe);

            //-------------------------------------------------------
            //CREATE MOSTRO
            //var livello = new Livello() { Numero = 2, PuntiVita = 40 };
            //var arma = new Arma() { NomeArma = "Clava", Classe = "Orco", PuntiDanno = 5 };
            //var mostro = new Mostro("Zuru", "Orco", arma, livello);
            //mostroService.CreateNewMostro(mostro);


            //----------------------------------------------------------
            //FUNZIONA GET
            //var mostri = mostroService.GetMostriByLivello(2);

            //foreach (var mostro in mostri)
            //{
            //    Console.WriteLine(mostro.ToString());
            //}

            //--------------------------------------------------------
            //FUNZIONA GET UTENTi
            //var giocatori = giocatoreService.GetAllGiocatori();

            //foreach (var giocatore in giocatori)
            //{
            //    Console.WriteLine(giocatore.ToString());
            //}

            //---------------------------------------------------------
            //FUNZOINA CREATE statistica
            //var statistica = new Statistica(eroe) { };
            //statisticaService.CreateNewStatistica(statistica);




            char key;

            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("BENVENUTO A MOSTRI VS EROI!");
            Console.ResetColor();
            var giocatore = InterazioneUtente.Giocatore();
            


            do
            {
                InterazioneUtente.MenuGiocatore(giocatore);
                
                Console.WriteLine("\nSe vuoi uscire dal gioco premi q, altrimenti premi un altro tasto per tornare al Menù Principale");
                Console.WriteLine("\n");
                key = Console.ReadKey().KeyChar;
            }
            while (key != 'q');

        }
    }
}
