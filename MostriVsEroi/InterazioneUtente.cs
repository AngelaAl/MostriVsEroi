using MostriVsEroi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MostriVsEroi
{
    //Ho creato questa classe per raggruppare tutti i metodi che interagiscono con l'utente
    //per mantenere il Program più libero possibile
    public class InterazioneUtente
    {
        //Campo per i livelli (mi servono spesso) per avere un unico accesso al db
        private static List<Livello> livelli = RegoleGioco.ListaLivelli();

        //Nome giocatore e controllo se presente nel db
        public static Giocatore Giocatore()
        {
            Console.WriteLine("Qual è il tuo nome?");
            string nomeGiocatore = Console.ReadLine();
            return RegoleGioco.CheckGiocatore(nomeGiocatore);
        }

        //MENU PRINCIPALE
        public static bool MenuGiocatore(Giocatore giocatore)
        {
            bool check = true;
            do
            {
                Console.WriteLine("\n\n-----------------------------------");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\nMENU PRINCIPALE");
                Console.ResetColor();

                //Giocatore utente
                Console.WriteLine("1 - Creare un nuovo eroe");
                Console.WriteLine("2 - Continuare l'avventura");
                Console.WriteLine("3 - Elimina un eroe");
                Console.WriteLine("4 - Vedere le statistiche di gioco");

                //Giocatore Admin
                if (giocatore.Ruolo == "Admin")
                {
                    Console.WriteLine("5 - Creare un nuovo mostro");
                }

                Console.WriteLine("\nq - Uscire dal gioco\n");
                Console.WriteLine("-----------------------------------");


                string key = Console.ReadLine();

                Console.WriteLine("\n");

                switch (key)
                {
                    case "1":
                        //Creazione eroe
                        var nuovoEroe = CreazioneEroeLocale(giocatore);
                        RegoleGioco.CreaEroe(nuovoEroe);
                        Console.WriteLine("L'avventura ha inizio!");
                        Partita(nuovoEroe);
                        return false;
                    case "2":
                        //Scelta eroe per la partita
                        var eroePartita = SceltaEroe(giocatore);
                        //Se non ci sono eroi, torna al menu principale
                        if(eroePartita == null)
                        {
                            return false;
                        }
                        Console.WriteLine("L'avventura continua!");
                        //Partita
                        Partita(eroePartita);
                        return false;
                    case "3":
                        //Scelta eroe da eliminare
                        var eroeDaEliminare = SceltaEroe(giocatore);
                        //Se non ci sono eroi da eliminare, esce/torna al menu principale
                        if (eroeDaEliminare == null)
                        {
                            break;
                        }
                        RegoleGioco.EliminaEroe(eroeDaEliminare);
                        Console.WriteLine($"{eroeDaEliminare.Nome} è stato eliminato.");
                        return EsciOMenuPrincipale();
                    case "4":
                        //Statistiche
                        if (giocatore.Ruolo == "Admin")
                        {
                            SceltaStatistiche();
                        }
                        else
                        {
                            StatistichePerUtente(giocatore);
                        }
                        return EsciOMenuPrincipale();
                        
                    case "5":
                        //Crea mostro
                        //if-else mi serve per controllare che il 3 sia stato digitato perchè era
                        //uscito come opzione nel menu e non perchè è stato digitato a caso da un utente
                        if(giocatore.Ruolo == "Admin")
                        {
                            var newMostro = CreazioneMostroLocale();
                            RegoleGioco.CreaMostro(newMostro);
                            return EsciOMenuPrincipale();
                        }
                        else
                        {
                            Console.WriteLine("Scelta non valida");
                            check = false;
                        }
                        break;

                    case "q":
                        return true;
                    default:
                        Console.WriteLine("Scelta non valida");
                        check = false;
                        break;


                }
            }
            while (check == false);
            return false;

        }


        //METODI richiamati nel Menu Principale

        public static void Partita(Eroe eroePartita)
        {
            Stopwatch watch = new Stopwatch();
            //Parte il watch
            watch.Start();
            bool continua;
            do
            {
                //Sorteggio mostro per la battaglia
                var mostro = RegoleGioco.SorteggioMostro(eroePartita);
                //Battaglia che mi restituisce l'eroe
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

                //Se il cast è andato a buon fine
                if(check == true)
                {
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
                }

            } while (check == false);

            //NOME EROE
            Console.WriteLine("\nScrivi il nome del tuo eroe");
            string nomeEroe;
            do
            {
                //Senza spazi a inizio e fine nome
                nomeEroe = Console.ReadLine().Trim();

                //Controllo nome unico
                check = RegoleGioco.ControlloNomeEroeUnico(nomeEroe);

                //Caso nomeEroe già esistente
                if(check == false)
                {
                    Console.WriteLine("\nSpiacenti, {0} è già in uso. Per favore, scrivi un altro nome", nomeEroe);
                }
                //Caso nome vuoto (dato che ho usato trim(), questo caso comprende anche: viene inserito solo l'invio o solo spazi)
                else if (nomeEroe.Length == 0)
                {
                    Console.WriteLine("\nNome non valido. Per favore, scrivi un altro nome");
                    check = false;
                }

            } while (check == false);

            //ARMA
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

                //Se il cast è andato bene
                if(check == true)
                {
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
                Console.WriteLine("Questi sono i tuoi eroi:\n ");
                for (int i = 1; i <= eroi.Count; i++)
                {
                    Console.WriteLine(i + " - " + eroi[i - 1].ToString());
                }
                Console.WriteLine("\nPer scegliere un eroe digita il numero corrispondente e premi invio");

                string scelta = Console.ReadLine();
                check = Int32.TryParse(scelta, out int res);
                int indiceEroe = res - 1;

                //Se il cast ha funzionato
                if(check == true)
                {
                    //Potrebbe essere un intero ma non tra gli indici
                    try
                    {
                        eroeScelto = eroi[indiceEroe];
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Scelta non valida");
                        check = false;
                    }
                }
                

            } while (check == false);
            return eroeScelto;
        }

        public static Eroe Battaglia(Eroe eroe, Mostro mostro)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"\nUn {mostro.Classe} ti blocca la strada!");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nInizio battaglia!");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{eroe.Nome}, {eroe.Classe}, Livello: {eroe.Livello} \tVS \t{mostro.Nome}, {mostro.Classe}, Livello: {mostro.LivelloMostro.Numero}");
            Console.ResetColor();

            bool fuga;
            do
            {
                Console.WriteLine($"\n{eroe.Nome} PV: {eroe.PuntiVita} \t\tVS\t\t {mostro.Nome} PV: {mostro.LivelloMostro.PuntiVita}");
                fuga = SceltaTurno(eroe, mostro);

                //Se non riesce la fuga/eroe attacca e se il mostro è sopravvissuto all'attacco dell'eroe
                if(fuga==false && mostro.LivelloMostro.PuntiVita > 0)
                {
                    //Attacca il mostro
                    int attaccoMostro = mostro.Attacca();
                    eroe.PuntiVita -= attaccoMostro;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{mostro.Nome} attacca con {mostro.ArmaScelta.NomeArma}! Danno: {mostro.ArmaScelta.PuntiDanno}");
                    Console.ResetColor();
                }

            }//Si ripete finchè l'eroe e il mostro sono vivi e l'eroe non riesce nella fuga
            while ((fuga == false) && (eroe.PuntiVita > 0 && mostro.LivelloMostro.PuntiVita > 0));

            //Se l'eroe è stato sconfitto
            if(eroe.PuntiVita <= 0)
            {
                Console.WriteLine("Il tuo eroe è stato scofitto da " + mostro.Nome);
            }
            //Se il mostro è stato sconfitto
            else if(mostro.LivelloMostro.PuntiVita<= 0)
            {
                Console.WriteLine("Complimenti! Hai sconfitto " + mostro.Nome);
                //L'eroe vince punti
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
                //Scelte possibili
                Console.WriteLine("Cosa vuoi fare?");

                Console.WriteLine("1 - Attacco");
                Console.WriteLine("2 - Tento la fuga");

                string key = Console.ReadLine();

                Console.WriteLine("\n");

                switch (key)
                {
                    case "1":
                        //Attacca l'eroe
                        int attaccoEroe = eroe.Attacca();
                        //Tolgo punti vita al mostro (in locale)
                        mostro.LivelloMostro.PuntiVita -= attaccoEroe;
                        //Comunico l'attacco all'utente
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{eroe.Nome} attacca con {eroe.ArmaScelta.NomeArma}! Danno: {eroe.ArmaScelta.PuntiDanno}");
                        Console.ResetColor();
                        check = true;
                        break;

                    case "2":
                        //Tenta la fuga
                        var fuga = eroe.Fuga();
                        if (fuga == true)
                        {
                            Console.WriteLine("Sei riuscito a scappare!");
                            //Eroe perde i punti della fuga
                            int puntiPersi = mostro.LivelloMostro.Numero * 5;
                            eroe.PuntiAccumulati -= puntiPersi;
                            Console.WriteLine("La fuga ti è costata {0} punti!", puntiPersi);
                            Console.WriteLine("Ora hai {0} punti accumulati", eroe.PuntiAccumulati);

                            //Finisce la battaglia
                            return true;
                        }
                        else
                        {
                            //Fuga non riuscita, continua la battaglia
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
                //Scelte possibili
                Console.WriteLine("\nCosa vuoi fare?");
                Console.WriteLine("1 - Salvare");
                Console.WriteLine("2 - Continuare");
                Console.WriteLine("3 - Tornare al Menù Principale (SENZA SALVARE)");

                char key = (char)Console.ReadKey().Key;

                switch (key)
                {
                    case '1':
                        //Salva
                        RegoleGioco.SalvaEroe(eroe);
                        break;
                    case '2':
                        //Continua
                        return true;
                    case '3':
                        break;
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

            //SCELTA CLASSE
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
            
            //SCELTA NOME
            Console.WriteLine("\nScrivi il nome del nuovo {0}", classeScelta.nomeClasse);
            string nomeMostro;

            do
            {
                nomeMostro = Console.ReadLine().Trim();

                //Caso nome vuoto (dato che ho usato trim(), questo caso comprende anche: viene inserito solo l'invio o solo spazi)
                if (nomeMostro.Length == 0)
                {
                    Console.WriteLine("\nNome non valido. Per favore, scrivi un altro nome");
                    check = false;
                }

            } while (check == false);
            

            //SCELTA ARMA
            var armi = RegoleGioco.ArmiPerClasse(classeScelta);

            do
            {
                Console.WriteLine("Ecco le armi disponibili per {0}:", nomeMostro);

                for (int i = 1; i <= armi.Count; i++)
                {
                    Console.WriteLine(i + " - " + armi[i - 1].NomeArma + " Punti Danno: " + armi[i - 1].PuntiDanno);
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
                        Console.WriteLine("\nSTATISTICHE RELATIVE A TUTTI I GIOCATORI:\n");
                        var statistiche = RegoleGioco.AllStatistiche();
                        if (statistiche.Count == 0)
                        {
                            Console.WriteLine("\nNon sono presenti statistiche");
                        }
                        foreach (Statistica statistica in statistiche)
                        {
                            Console.WriteLine("Giocatore: " + statistica.GiocatoreAssegnato + "\t" + statistica.ToString());
                        }
                        break;
                    case '2':
                        //Statistiche per giocatore
                        var giocatore = SceltaGiocatore();
                        Console.WriteLine("\nSTATISTICHE di {0}:\n", giocatore.Nome);
                        var statisticheGiocatore = RegoleGioco.StatisticheByGiocatore(giocatore);
                        if (statisticheGiocatore.Count == 0)
                        {
                            Console.WriteLine("Non sono presenti statistiche per questo giocatore");
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

        public static void StatistichePerUtente(Giocatore giocatore)
        {
            Console.WriteLine("\nSTATISTICHE di {0}:", giocatore.Nome);
            var statisticheGiocatore = RegoleGioco.StatisticheByGiocatore(giocatore);
            if (statisticheGiocatore.Count == 0)
            {
                Console.WriteLine("\nNon sono presenti statistiche");
            }
            foreach (Statistica statistica in statisticheGiocatore)
            {
                Console.WriteLine(statistica.ToString());
            }
        }

        public static Giocatore SceltaGiocatore()
        {
            var giocatoreScelto = new Giocatore() { };
            var giocatori = RegoleGioco.AllGiocatori();
            bool check;
            do
            {
                Console.WriteLine("\nDi quale giocatore vuoi vedere le statistiche?");
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

        public static bool EsciOMenuPrincipale()
        {
            Console.WriteLine("\n\nSe vuoi uscire dal gioco premi q, altrimenti premi un altro tasto per tornare al Menù Principale");
            Console.WriteLine("\n");
            char scelta = Console.ReadKey().KeyChar;
            switch (scelta)
            {
                case 'q':
                    return true;
                default:
                    return false;
            }
        }
        

       

    }
}
