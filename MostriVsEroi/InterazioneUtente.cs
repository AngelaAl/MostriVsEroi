using MostriVsEroi.Core.Entities;
using System;
using System.Collections.Generic;
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
            Console.WriteLine("\nCosa vuoi fare?");
            
            //Giocatore utente
            Console.WriteLine("1 - Creare un nuovo eroe");
            Console.WriteLine("2 - Iniziare l'avventura");

            //Giocatore Admin
            if(giocatore.Ruolo == "Admin")
            {
                Console.WriteLine("3 - Creare un nuovo mostro");
            }

            char key = (char)Console.ReadKey().Key;

            Console.WriteLine("\n");

            switch (key)
            {
                case '1':
                    //Creazione eroe
                    var nuovoEroe = CreazioneEroeLocale(giocatore);
                    RegoleGioco.CreaEroe(nuovoEroe);
                    break;
                case '2':
                    //Inizia partita
                    //Scelta eroe
                    var eroePartita = SceltaEroe(giocatore);
                    Console.WriteLine("L'avventura ha inizio!");
                    bool continua;
                    do
                    {
                        var mostro = RegoleGioco.SorteggioMostro(eroePartita);
                        eroePartita = Battaglia(eroePartita, mostro);

                        //Se l'eroe è morto, lo elimino dal database
                        if (eroePartita.PuntiVita <= 0)
                        {
                            RegoleGioco.EliminaEroe(eroePartita);
                            break;
                        }

                        int livello = eroePartita.Livello;
                        eroePartita = RegoleGioco.CheckPassaggioDiLivello(eroePartita, livelli);
                        if(livello != eroePartita.Livello)
                        {
                            Console.WriteLine("Complimenti! Sei passato al livello {0}! \n Ora hai {1} punti vita!", eroePartita.Livello, eroePartita.PuntiVita);
                        }

                        if(eroePartita.PuntiAccumulati >= 200)
                        {
                            Console.WriteLine("Complimenti!! HAI VINTO!!!");
                            Console.WriteLine("Puoi continuare a giocare con il tuo eroe, ma non potrai più salire di livello");
                        }

                        //Menu fine battaglia
                        continua = ContinuaPartita(eroePartita);
                    }
                    while (continua == true);
                    break;
                case '3':
                    //Crea mostro
                    break;
                default:
                    Console.WriteLine("Scelta non valida");
                    break;


            }

        }


        //METODI 
        public static Eroe CreazioneEroeLocale(Giocatore giocatore)
        {
            Console.WriteLine("Ecco le classi disponibili: ");
            var classi = RegoleGioco.ClassiPerEroe();
            for(int i =1; i<= classi.Count; i++)
            {
                Console.WriteLine(i + " - " + classi[i - 1].ToString());
            }
            Console.WriteLine("Per scegliere una classe digita il numero corrispondente");
            
            int indiceClasse = Convert.ToInt32(Console.ReadLine()) - 1;

            var classeScelta = classi[indiceClasse];

            Console.WriteLine("\nScrivi il nome del tuo eroe");

            string nomeEroe = Console.ReadLine();

            Console.WriteLine("Ecco le armi disponibili per il tuo eroe:");

            var armi = RegoleGioco.ArmiPerClasse(classeScelta);

            for (int i = 1; i <= armi.Count; i++)
            {
                Console.WriteLine(i + " - " + armi[i - 1].NomeArma);
            }
            Console.WriteLine("Per scegliere un'arma digita il numero corrispondente");

            int indiceArma = Convert.ToInt32(Console.ReadLine()) - 1;

            var armaScelta = armi[indiceArma];

            var eroe = new Eroe(nomeEroe, classeScelta.nomeClasse, armaScelta, giocatore.Nome) { };

            return eroe;

        }

        public static Eroe SceltaEroe(Giocatore giocatore)
        {
            Console.WriteLine("Questi sono i tuoi eroi: ");
            var eroi = RegoleGioco.EroiDelGiocatore(giocatore);
            for (int i = 1; i <= eroi.Count; i++)
            {
                Console.WriteLine(i + " - " + eroi[i - 1].ToString());
            }
            Console.WriteLine("Per scegliere un eroe digita il numero corrispondente");

            int indiceEroe = Convert.ToInt32(Console.ReadLine()) - 1;

            var eroeScelto = eroi[indiceEroe];
            return eroeScelto;
        }

        public static Eroe Battaglia(Eroe eroe, Mostro mostro)
        {
            Console.WriteLine($"\nUn {mostro.Classe} ti blocca la strada!");
            Console.WriteLine("\nInizio battaglia!");
            Console.WriteLine($"{eroe.Nome}, {eroe.Classe}, Livello: {eroe.Livello} \tVS \t{mostro.Nome}, {mostro.Classe}, Livello: {mostro.LivelloMostro.Numero}");

            bool fuga = false;
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
            Console.WriteLine("Cosa vuoi fare?");

            Console.WriteLine("1 - Attacco");
            Console.WriteLine("2 - Tento la fuga");

            char key = (char)Console.ReadKey().Key;

            Console.WriteLine("\n");

            switch (key)
            {
                case '1':
                    int attaccoEroe = eroe.Attacca();
                    mostro.LivelloMostro.PuntiVita -= attaccoEroe;
                    Console.WriteLine($"{eroe.Nome} attacca con {eroe.ArmaScelta.NomeArma}! Danno: {eroe.ArmaScelta.PuntiDanno}");
                    break;
                    
                case '2':
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
                   
                    

            }
            return false;
        }


        public static bool ContinuaPartita(Eroe eroe)
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
            }
            return false;


        }
    }
}
