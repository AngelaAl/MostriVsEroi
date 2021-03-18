using MostriVsEroi.ADORepository;
using MostriVsEroi.Core.Entities;
using MostriVsEroi.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace MostriVsEroi
{

    public static class RegoleGioco
    {
        private static ServiceProvider serviceProvider = DIConfiguration.Configurazione();
        public static Giocatore CheckGiocatore(string nomeGiocatore)
        {
            GiocatoreService giocatoreService = serviceProvider.GetService<GiocatoreService>();
            var giocatori = giocatoreService.GetAllGiocatori();
            foreach(var giocatore in giocatori)
            {
                if (giocatore.Nome == nomeGiocatore)
                {
                    Console.WriteLine("Bentornato " + nomeGiocatore);
                    return giocatore;
                }

            }
            var nuovoGiocatore = new Giocatore(nomeGiocatore) { };
            giocatoreService.CreateGiocatore(nuovoGiocatore);
            Console.WriteLine("Benvenuto " + nomeGiocatore);
            return nuovoGiocatore;
            
        }

        //PASSAGGIO DI LIVELLO
        public static List<Livello> ListaLivelli()
        {
            LivelloService livelloService = serviceProvider.GetService<LivelloService>();
            var livelli = livelloService.GetAllLivelli().ToList();
            return livelli;
        }
        

        public static Eroe CheckPassaggioDiLivello(Eroe eroe, List<Livello> livelli)
        {
            foreach(Livello livello in livelli)
            {
                if(livello.Numero > eroe.Livello && eroe.PuntiAccumulati>= livello.PuntiPerPassaggio)
                {
                    eroe.Livello = livello.Numero;
                    eroe.PuntiVita = livello.PuntiVita;
                    
                }
            }
            return eroe;

        }

        public static List<Classe> ClassiPerEroe()
        {
            ClasseService classeService = serviceProvider.GetService<ClasseService>();
            var classi = classeService.GetClassiFiltrate(1).ToList();
            return classi;
        }

        public static List<Classe> ClassiPerMostro()
        {
            ClasseService classeService = serviceProvider.GetService<ClasseService>();
            var classi = classeService.GetClassiFiltrate(0).ToList();
            return classi;
        }

        public static List<Arma> ArmiPerClasse(Classe classe)
        {
            ArmaService armaService = serviceProvider.GetService<ArmaService>();
            var armi = armaService.GetArmiByClasse(classe).ToList();
            return armi;
        }

        public static void CreaEroe(Eroe eroe)
        {
            EroeService eroeService = serviceProvider.GetService<EroeService>();
            StatisticaService statisticaService = serviceProvider.GetService<StatisticaService>();
            eroeService.CreateNewEroe(eroe);
            var statistica = new Statistica(eroe) { };
            statisticaService.CreateNewStatistica(statistica);
        }

        public static void EliminaEroe(Eroe eroe)
        {
            EroeService eroeService = serviceProvider.GetService<EroeService>();
            eroeService.DeleteEroe(eroe);
        }

        public static void SalvaEroe(Eroe eroe)
        {
            EroeService eroeService = serviceProvider.GetService<EroeService>();
            eroeService.UpdateEroe(eroe);
        }

        public static List<Eroe> EroiDelGiocatore(Giocatore giocatore)
        {
            EroeService eroeService = serviceProvider.GetService<EroeService>();
            var eroi = eroeService.GetAllEroiByGiocatori(giocatore).ToList();
            return eroi;
        }

        public static Mostro SorteggioMostro(Eroe eroe)
        {
            MostroService mostroService = serviceProvider.GetService<MostroService>();
            var mostri = mostroService.GetMostriByLivello(eroe.Livello).ToList();
            Random x = new Random();
            int indiceEstratto = x.Next(0, mostri.Count());
            return mostri[indiceEstratto];
        }

        public static void AggiornaStatistica(Eroe eroe, int millisecondi)
        {
            StatisticaService statisticaService = serviceProvider.GetService<StatisticaService>();
            statisticaService.UpdateStatistica(eroe, millisecondi);
        }

        public static void CreaMostro(Mostro mostro)
        {
            MostroService mostroService = serviceProvider.GetService<MostroService>();
            mostroService.CreateNewMostro(mostro);
        }

        public static List<Statistica> AllStatistiche()
        {
            StatisticaService statisticaService = serviceProvider.GetService<StatisticaService>();
            return statisticaService.GetStatistiche().ToList();
        }

        public static List<Statistica> StatisticheByGiocatore(Giocatore giocatore)
        {
            StatisticaService statisticaService = serviceProvider.GetService<StatisticaService>();
            return statisticaService.GetStatisticheByGiocatore(giocatore).ToList();
        }

        public static List<Giocatore> AllGiocatori()
        {
            GiocatoreService giocatoreService = serviceProvider.GetService<GiocatoreService>();
            return giocatoreService.GetAllGiocatori().ToList();
        }
    }
}
