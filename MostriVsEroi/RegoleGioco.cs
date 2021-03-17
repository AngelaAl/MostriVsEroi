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
                if(eroe.PuntiAccumulati>= livello.PuntiPerPassaggio)
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

        public static List<Arma> ArmiPerClasse(Classe classe)
        {
            ArmaService armaService = serviceProvider.GetService<ArmaService>();
            var armi = armaService.GetArmiByClasse(classe).ToList();
            return armi;
        }

        public static void CreaEroe(Eroe eroe)
        {
            EroeService eroeService = serviceProvider.GetService<EroeService>();
            eroeService.CreateNewEroe(eroe);
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

        
    }
}
