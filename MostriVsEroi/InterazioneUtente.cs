using MostriVsEroi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MostriVsEroi
{
    public class InterazioneUtente
    {
        private static List<Livello> livelli = RegoleGioco.ListaLivelli();
        public static Giocatore Giocatore()
        {
            Console.WriteLine("Qual è il tuo nome?");
            string nomeGiocatore = Console.ReadLine();
            return RegoleGioco.CheckGiocatore(nomeGiocatore);
        }

        //MENU PRINCIPALE
        public static void MenuGiocatore(Giocatore giocatore)
        {
            bool check = true;
            do
            {
                Console.WriteLine("\nCosa vuoi fare?");

                //Giocatore utente
                Console.WriteLine("1 - Creare un nuovo eroe");
                Console.WriteLine("2 - Iniziare l'avventura");
                Console.WriteLine("3 - Elimina un eroe");

                //Giocatore Admin
                if (giocatore.Ruolo == "Admin")
                {
                    Console.WriteLine("4 - Creare un nuovo mostro");
                    Console.WriteLine("5 - Vedere le statistiche di gioco");
                }

                string key = Console.ReadLine();

                Console.WriteLine("\n");

                switch (key)
                {
                    case "1":
                        //Creazione eroe
                        var nuovoEroe = CreazioneEroeLocale(giocatore);
                        RegoleGioco.CreaEroe(nuovoEroe);
                        Partita(nuovoEroe);
                        break;
                    case "2":
                        //Scelta eroe per la partita
                        var eroePartita = SceltaEroe(giocatore);
                        if(eroePartita == null)
                        {
                            break;
                        }
                        Console.WriteLine("L'avventura ha inizio!");
                        //Partita
                        Partita(eroePartita);
                        break;
                    case "3":
                        //Scelta eroe da eliminare
                        var eroeDaEliminare = SceltaEroe(giocatore);
                        if (eroeDaEliminare == null)
                        {
                            break;
                        }
                        RegoleGioco.EliminaEroe(eroeDaEliminare);
                        Console.WriteLine($"{eroeDaEliminare.Nome} è stato eliminato.");
                        break;
                    case "4":
                        //Crea mostro
                        //if-else mi serve per controllare che il 3 sia stato digitato perchè era
                        //uscito come opzione nel menu e non perchè è stato digitato a caso da un utente
                        if(giocatore.Ruolo == "Admin")
                        {
                            var newMostro = CreazioneMostroLocale();
                            RegoleGioco.CreaMostro(newMostro);
                        }
                        else
                        {
                            Console.WriteLine("Scelta non valida");
                            check = false;
                        }
                        break;
                    case "5":
                        //Statistiche
                        //if-else mi serve per controllare che il 4 sia stato digitato perchè era
                        //uscito come opzione nel menu e non perchè è stato digitato a caso da un utente
                        if (giocatore.Ruolo == "Admin")
                        {
                            SceltaStatistiche();
                        }
                        else
                        {
                            Console.WriteLine("Scelta non valida");
                            check = false;
                        }
                        break;
                    default:
                        Console.WriteLine("Scelta non valida");
                        check = false;
                        break;


                }
            }
            while (check == false);

        }


        //METODI 

        public static void Partita(Eroe eroePartita)
        {
            Stopwatch watch = new Stopwatch();
            //Parte il watch
            watch.Start();
            bool continua;
            do
            {
                var mostro = RegoleGioco.SorteggioMostro(eroePartita);
                eroePartita = Battaglia(eroePartita, mostro);

                //Se l'eroe è morto, lo elimino dal database e termina la partita
                if (eroePartita.PuntiVita <= 0)
                {
                    RegoleGioco.EliminaEroe(eroePartita);
                    break;
                }

                //Salvo l'attuale livello dell'eroe
                int livello = eroePartita.Livello;
                //Check per il passaggio di livello viene fatto solo se il livello attuale è diverso
                //dal massimo livello disponibile (questo copre anche il caso in cui un eroe abbia già vinto)
                if (livello != livelli[livelli.Count - 1].Numero)
                {
                    eroePartita = RegoleGioco.CheckPassaggioDiLivello(eroePartita, livelli);
                }
                if (livello != eroePartita.Livello)
                {
                    Console.WriteLine("Complimenti! Sei passato al livello {0}! \n Ora hai {1} punti vita!", eroePartita.Livello, eroePartita.PuntiVita);
                }

                if (eroePartita.PuntiAccumulati >= 200)
                {
                    Console.WriteLine("Complimenti!! HAI VINTO!!!");
                    Console.WriteLine("Puoi continuare a giocare con il tuo eroe, ma non potrai più salire di livello");
                }

                //Menu fine battaglia
                continua = ContinuaPartita(eroePartita);
            }
            while (continua == true);
            watch.Stop();
            int milliSecondi = (int)watch.ElapsedMilliseconds;
            if (eroePartita.PuntiVita > 0)
            {
                RegoleGioco.AggiornaStatistica(eroePartita, milliSecondi);
            }
        }
        public static Eroe CreazioneEroeLocale(Giocatore giocatore)
        {
            var classi = RegoleGioco.ClassiPerEroe();
            var classeScelta = new Classe() { };
            var armaScelta = new Arma() { };
            bool check;

            do
            {
                Console.WriteLine("Ecco le classi disponibili: ");
                for (int i = 1; i <= classi.Count; i++)
                {
                    Console.WriteLine(i + " - " + classi[i - 1].ToString());
                }
                Console.WriteLine("Per scegliere una classe digita il numero corrispondente");

                string scelta = Console.ReadLine();
                check = Int32.TryParse(scelta, out int res);
                int indiceClasse = res - 1;

                //Try-Catch mi serve nel caso venga inserito un intero non valido (non tra le opzioni)
                try
                {
                    classeScelta = classi[indiceClasse];
                }
                catch (Exception)
                {
                    Console.WriteLine("Scelta non valida");
                    check = false;
                }

            } while (check == false);

            Console.WriteLine("\nScrivi il nome del tuo eroe");

            string nomeEroe = Console.ReadLine();

            do
            {
                Console.WriteLine("Ecco le armi disponibili per il tuo eroe:");

                var armi = RegoleGioco.ArmiPerClasse(classeScelta);

                for (int i = 1; i <= armi.Count; i++)
                {
                    Console.WriteLine(i + " - " + armi[i - 1].NomeArma);
                }
                Console.WriteLine("Per scegliere un'arma digita il numero corrispondente");

                string scelta = Console.ReadLine();
                check = Int32.TryParse(scelta, out int res);
                int indiceArma = res - 1;

                //Try-Catch mi serve nel caso venga inserito un intero non valido (non tra le opzioni)
                try
                {
                    armaScelta = armi[indiceArma];
                }
                catch (Exception)
                {
                    Console.WriteLine("Scelta non valida");
                    check = false;
                }

            } while (check == false);

            var eroe = new Eroe(nomeEroe, classeScelta.nomeClasse, armaScelta, giocatore.Nome) { };
            Console.WriteLine("Hai creato un eroe:");
            Console.WriteLine(eroe.ToString());

            return eroe;

        }

        public static Eroe SceltaEroe(Giocatore giocatore)
        {
            var eroi = RegoleGioco.EroiDelGiocatore(giocatore);
            var eroeScelto = new Eroe() { };
            if (eroi.Count == 0)
            {
                Console.WriteLine("Non hai ancora eroi! Torna al menù principale e creane uno!");
                return null;
            }
            bool check;
            do
            {
                Console.WriteLine("Questi sono i tuoi eroi: ");
                for (int i = 1; i <= eroi.Count; i++)
                {
                    Console.WriteLine(i + " - " + eroi[i - 1].ToString());
                }
                Console.WriteLine("Per scegliere un eroe digita il numero corrispondente e premi invio");

                string scelta = Console.ReadLine();
                check = Int32.TryParse(scelta, out int res);
                int indiceEroe = res - 1;

                try
                {
                    eroeScelto = eroi[indiceEroe];
                }
                catch (Exception)
                {
                    Console.WriteLine("Scelta non valida");
                    check = false;
                }
                

            } while (check == false);
            return eroeScelto;
        }

        public static Eroe Battaglia(Eroe eroe, Mostro mostro)
        {
            Console.WriteLine($"\nUn {mostro.Classe} ti blocca la strada!");
            Console.WriteLine("\nInizio battaglia!");
            Console.WriteLine($"{eroe.Nome}, {eroe.Classe}, Livello: {eroe.Livello} \tVS \t{mostro.Nome}, {mostro.Classe}, Livello: {mostro.LivelloMostro.Numero}");

            bool fuga;
            do
            {
                Console.WriteLine($"\n{eroe.Nome} PV: {eroe.PuntiVita} \t\tVS\t\t {mostro.Nome} PV: {mostro.LivelloMostro.PuntiVita}");
                fuga = SceltaTurno(eroe, mostro);
                int attaccoMostro = mostro.Attacca();
                if(fuga==false && mostro.LivelloMostro.PuntiVita > 0)
                {
                    eroe.PuntiVita -= attaccoMostro;
                    Console.WriteLine($"{mostro.Nome} attacca con {mostro.ArmaScelta.NomeArma}! Danno: {mostro.ArmaScelta.PuntiDanno}");

                }

            }
            while ((fuga == false) && (eroe.PuntiVita > 0 && mostro.LivelloMostro.PuntiVita > 0));
            if(eroe.PuntiVita <= 0)
            {
                Console.WriteLine("Il tuo eroe è stato scofitto da " + mostro.Nome);
            }
            else if(mostro.LivelloMostro.PuntiVita<= 0)
            {
                Console.WriteLine("Complimenti! Hai sconfitto " + mostro.Nome);
                int puntiVinti = mostro.LivelloMostro.Numero * 10;
                Console.WriteLine("Hai guadagnato {0} punti", puntiVinti);
                eroe.PuntiAccumulati += puntiVinti;
                Console.WriteLine("Ora hai {0} punti accumulati", eroe.PuntiAccumulati);
            }
            return eroe;
        }

        public static bool SceltaTurno(Eroe eroe, Mostro mostro)
        {
            bool check = true;
            do
            {
                Console.WriteLine("Cosa vuoi fare?");

                Console.WriteLine("1 - Attacco");
                Console.WriteLine("2 - Tento la fuga");

                string key = Console.ReadLine();

                Console.WriteLine("\n");

                switch (key)
                {
                    case "1":
                        int attaccoEroe = eroe.Attacca();
                        mostro.LivelloMostro.PuntiVita -= attaccoEroe;
                        Console.WriteLine($"{eroe.Nome} attacca con {eroe.ArmaScelta.NomeArma}! Danno: {eroe.ArmaScelta.PuntiDanno}");
                        break;

                    case "2":
                        var fuga = eroe.Fuga();
                        if (fuga == true)
                        {
                            Console.WriteLine("Sei riuscito a scappare!");
                            eroe.PuntiAccumulati -= mostro.LivelloMostro.Numero * 5;
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("La fuga non è riuscita!");
                            break;
                        }
                    default:
                        Console.WriteLine("Scelta non valida\n");
                        check = false;
                        break;

                } 

            } while (check == false);

            return false;
        }


        public static bool ContinuaPartita(Eroe eroe)
        {
            bool check = true;
            do
            {
                Console.WriteLine("Cosa vuoi fare?");
                Console.WriteLine("1 - Salvare");
                Console.WriteLine("2 - Continuare");

                char key = (char)Console.ReadKey().Key;

                switch (key)
                {
                    case '1':
                        RegoleGioco.SalvaEroe(eroe);
                        break;
                    case '2':
                        return true;
                    default:
                        Console.WriteLine("Scelta non valida\n");
                        check = false;
                        break;
                }
            } while (check == false);
            
            return false;


        }

        public static Mostro CreazioneMostroLocale()
        {
            bool check;
            var classeScelta = new Classe() { };
            var armaScelta = new Arma() { };
            var livelloScelto = new Livello() { };
            var classi = RegoleGioco.ClassiPerMostro();

            //SCELTA ARMA
            do
            {
                Console.WriteLine("Ecco le classi disponibili: ");
                for (int i = 1; i <= classi.Count; i++)
                {
                    Console.WriteLine(i + " - " + classi[i - 1].ToString());
                }
                Console.WriteLine("Per scegliere una classe digita il numero corrispondente");

                string scelta = Console.ReadLine();
                check = Int32.TryParse(scelta, out int res);
                int indiceClasse = res - 1;

                //Try-Catch mi serve nel caso venga inserito un intero non valido (non tra le opzioni)
                try
                {
                    classeScelta = classi[indiceClasse];
                }
                catch (Exception)
                {
                    Console.WriteLine("Scelta non valida");
                    check = false;
                }


            } while (check == false);
            

            Console.WriteLine("\nScrivi il nome del nuovo {0}", classeScelta.nomeClasse);

            string nomeMostro = Console.ReadLine();

            //SCELTA ARMA
            var armi = RegoleGioco.ArmiPerClasse(classeScelta);

            do
            {
                Console.WriteLine("Ecco le armi disponibili per {0}:", nomeMostro);

                for (int i = 1; i <= armi.Count; i++)
                {
                    Console.WriteLine(i + " - " + armi[i - 1].NomeArma + "Punti Danno: " + armi[i - 1].PuntiDanno);
                }
                Console.WriteLine("Per scegliere un'arma digita il numero corrispondente");

                string scelta = Console.ReadLine();
                check = Int32.TryParse(scelta, out int res);
                int indiceArma = res - 1;

                //Try-Catch mi serve nel caso venga inserito un intero non valido (non tra le opzioni)
                try
                {
                    armaScelta = armi[indiceArma];
                }
                catch (Exception)
                {
                    Console.WriteLine("Scelta non valida");
                    check = false;
                }

            } while (check == false);

            //SCELTA LIVELLO
            do
            {
                Console.WriteLine("Quale livello vuoi assegnare?");

                for (int i = 1; i <= livelli.Count; i++)
                {
                    Console.WriteLine(i + " - Livello " + livelli[i - 1].Numero + "\tPunti Vita: " + livelli[i - 1].PuntiVita);
                }
                Console.WriteLine("Per scegliere un livello digita il numero corrispondente");

                string scelta = Console.ReadLine();
                check = Int32.TryParse(scelta, out int res);
                int indiceLivello = res - 1;

                //Try-Catch mi serve nel caso venga inserito un intero non valido (non tra le opzioni)
                try
                {
                    livelloScelto = livelli[indiceLivello];
                }
                catch (Exception)
                {
                    Console.WriteLine("Scelta non valida");
                    check = false;
                }

            } while (check == false);
            

            var mostro = new Mostro(nomeMostro, classeScelta.nomeClasse, armaScelta, livelloScelto) { };
            Console.WriteLine("Hai creato un mostro:");
            Console.WriteLine(mostro.ToString());

            return mostro;

        }

        public static void SceltaStatistiche()
        {
            bool check = true;
            do
            {
                char key;
                Console.WriteLine("Quali statistiche vuoi vedere?");
                Console.WriteLine("1 - Statistiche di tutti i giocatori");
                Console.WriteLine("2 - Statistiche filtrate per giocatore");

                key = Console.ReadKey().KeyChar;
                switch (key)
                {
                    case '1':
                        //Tutte le statistiche
                        Console.WriteLine("\nSTATISTICHE:");
                        var statistiche = RegoleGioco.AllStatistiche();
                        foreach (Statistica statistica in statistiche)
                        {
                            Console.WriteLine(statistica.ToString());
                        }
                        break;
                    case '2':
                        //Statistiche per giocatore
                        var giocatore = SceltaGiocatore();
                        Console.WriteLine("\nSTATISTICHE di {0}:", giocatore.Nome);
                        var statisticheGiocatore = RegoleGioco.StatisticheByGiocatore(giocatore);
                        if (statisticheGiocatore.Count == 0)
                        {
                            Console.WriteLine("\nNon sono presenti statistiche per questo giocatore");
                        }
                        foreach (Statistica statistica in statisticheGiocatore)
                        {
                            Console.WriteLine(statistica.ToString());
                        }
                        break;
                    default:
                        Console.WriteLine("Scelta non valida");
                        check = false;
                        break;
                }

            } while (check == false);
            
        }

        public static Giocatore SceltaGiocatore()
        {
            var giocatoreScelto = new Giocatore() { };
            var giocatori = RegoleGioco.AllGiocatori();
            bool check;
            do
            {
                Console.WriteLine("Di quale giocatore vuoi vedere le statistiche?");
                for (int i = 1; i <= giocatori.Count; i++)
                {
                    Console.WriteLine(i + " - " + giocatori[i - 1].ToString());
                }
                Console.WriteLine("Per scegliere un giocatore digita il numero corrispondentee premi invio");

                string scelta = Console.ReadLine();
                check = Int32.TryParse(scelta, out int res);
                int indiceGiocatore = res - 1;

                //Try-Catch mi serve nel caso venga inserito un intero non valido (non tra le opzioni)
                try
                {
                    giocatoreScelto = giocatori[indiceGiocatore];
                }
                catch (Exception)
                {
                    Console.WriteLine("Scelta non valida");
                    check = false;
                }
                

            } while (check == false);

            return giocatoreScelto;
        }

    }
}
