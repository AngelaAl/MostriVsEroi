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
    //Ho creato questa classe per raggruppare tutti i metodi che utilizzano il Service Provider
    //Ho cercato di non inserire interazione con l'utente
    //Tutti questi metodi sono utilizzati nella classe InterazioneUtente
    public static class RegoleGioco
    {
        //Campo per il service provider
        private static ServiceProvider serviceProvider = DIConfiguration.Configurazione();


        //Metodi


        //Controllo se il nome del giocatore è già presente nel db
        //Restituisce l'oggetto giocatore
        public static Giocatore CheckGiocatore(string nomeGiocatore)
        {
            GiocatoreService giocatoreService = serviceProvider.GetService<GiocatoreService>();
            var giocatori = giocatoreService.GetAllGiocatori();
            foreach(var giocatore in giocatori)
            {
                if (giocatore.Nome == nomeGiocatore)
                {
                    Console.WriteLine("Bentornato " + nomeGiocatore +"!!");
                    return giocatore;
                }

            }
            var nuovoGiocatore = new Giocatore(nomeGiocatore) { };
            giocatoreService.CreateGiocatore(nuovoGiocatore);
            Console.WriteLine("Benvenuto " + nomeGiocatore + "!!");
            return nuovoGiocatore;
            
        }


        //Funzione che restituisce la lista completa di livelli dal db
        public static List<Livello> ListaLivelli()
        {
            LivelloService livelloService = serviceProvider.GetService<LivelloService>();
            var livelli = livelloService.GetAllLivelli().ToList();
            return livelli;
        }
        

        //Dato l'eroe (e quindi il suo livello) e la lista totale di livelli
        //controllo se l'eroe ha abbastanza punti accumulati per il passaggio di livello
        //Restituisce l'eroe con il passaggio di livello e il refill di punti vita 
        //(se non c'è stato il passaggio di livello, restituisce l'eroe che c'è come parametro)
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


        //Restituisce la lista delle classi disponibili per il personaggio eroe
        public static List<Classe> ClassiPerEroe()
        {
            ClasseService classeService = serviceProvider.GetService<ClasseService>();
            var classi = classeService.GetClassiFiltrate(1).ToList();
            return classi;
        }


        //Restituisce la lista delle classi disponibili per il personaggio mostro
        public static List<Classe> ClassiPerMostro()
        {
            ClasseService classeService = serviceProvider.GetService<ClasseService>();
            var classi = classeService.GetClassiFiltrate(0).ToList();
            return classi;
        }


        //Data una classe come parametro
        //Restituisce tutte le armi disponibili per quella classe
        public static List<Arma> ArmiPerClasse(Classe classe)
        {
            ArmaService armaService = serviceProvider.GetService<ArmaService>();
            var armi = armaService.GetArmiByClasse(classe).ToList();
            return armi;
        }


        //Dato in input il nome inserito dall'utente per un nuovo eroe
        //controlla se il nome è già presente nel db
        //restituisce true se si può procedere con la creazione di un nuovo eroe
        //false se è già presente un eroe con quel nome
        public static bool ControlloNomeEroeUnico(string nomeNuovoEroe)
        {
            EroeService eroeService = serviceProvider.GetService<EroeService>();
            var nomiEroiEsistenti = eroeService.GetAllNomiEroi();
            foreach(string nome in nomiEroiEsistenti)
            {
                if(nomeNuovoEroe == nome)
                {
                    return false;
                }
            }
            return true;

        }


        //Dato un oggetto eroe come parametro
        //Crea l'eroe nel db
        public static void CreaEroe(Eroe eroe)
        {
            EroeService eroeService = serviceProvider.GetService<EroeService>();
            StatisticaService statisticaService = serviceProvider.GetService<StatisticaService>();
            eroeService.CreateNewEroe(eroe);
            var statistica = new Statistica(eroe) { };
            statisticaService.CreateNewStatistica(statistica);
        }


        //Dato un oggetto eroe come parametro
        //Elimina l'eroe dal database (lo elimina da Eroi e da Statistiche)
        public static void EliminaEroe(Eroe eroe)
        {
            EroeService eroeService = serviceProvider.GetService<EroeService>();
            eroeService.DeleteEroe(eroe);
        }


        //Dato un oggetto eroe come parametro
        //Fa l'update nel database di quell'eroe
        public static void SalvaEroe(Eroe eroe)
        {
            EroeService eroeService = serviceProvider.GetService<EroeService>();
            eroeService.UpdateEroe(eroe);
        }


        //Dato un giocatore come parametro
        //Restituisce la lista di tutti gli eroi di quel giocatore (dal db)
        public static List<Eroe> EroiDelGiocatore(Giocatore giocatore)
        {
            EroeService eroeService = serviceProvider.GetService<EroeService>();
            var eroi = eroeService.GetAllEroiByGiocatori(giocatore).ToList();
            return eroi;
        }


        //Dato un eroe (e quindi il suo livello)
        //prende la lista completa di tutti i mostri con il livello minore o ugale a quello dell'eroe e sorteggia un mostro 
        //Restituisceil mostro sorteggiato
        public static Mostro SorteggioMostro(Eroe eroe)
        {
            MostroService mostroService = serviceProvider.GetService<MostroService>();
            var mostri = mostroService.GetMostriByLivello(eroe.Livello).ToList();
            Random x = new Random();
            int indiceEstratto = x.Next(0, mostri.Count());
            return mostri[indiceEstratto];
        }



        //Dato eroe e i millisecondi che ha giocato nella partita
        //Aggiorna la statistica sul db aggiungento i millisecondi
        public static void AggiornaStatistica(Eroe eroe, int millisecondi)
        {
            StatisticaService statisticaService = serviceProvider.GetService<StatisticaService>();
            statisticaService.UpdateStatistica(eroe, millisecondi);
        }


        //Dato un oggetto mostro
        //Lo crea nel db
        public static void CreaMostro(Mostro mostro)
        {
            MostroService mostroService = serviceProvider.GetService<MostroService>();
            mostroService.CreateNewMostro(mostro);
        }

        //Restituisce la lista di tutte le statistiche dal db
        public static List<Statistica> AllStatistiche()
        {
            StatisticaService statisticaService = serviceProvider.GetService<StatisticaService>();
            return statisticaService.GetStatistiche().ToList();
        }


        //Dato un giocatore come parametro
        //Restituisce la lista delle statistiche di quel giocatore
        public static List<Statistica> StatisticheByGiocatore(Giocatore giocatore)
        {
            StatisticaService statisticaService = serviceProvider.GetService<StatisticaService>();
            return statisticaService.GetStatisticheByGiocatore(giocatore).ToList();
        }


        //Restituisce la lista di tutti i giocatori dal db
        public static List<Giocatore> AllGiocatori()
        {
            GiocatoreService giocatoreService = serviceProvider.GetService<GiocatoreService>();
            return giocatoreService.GetAllGiocatori().ToList();
        }
    }
}
