using MostriVsEroi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MostriVsEroi
{
    public class InterazioneUtente
    {
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
                    RegoleGioco.SalvaEroe(nuovoEroe);
                    break;
                case '2':
                    //Inizia partita
                    //Scelta eroe
                    var eroePartita = SceltaEroe(giocatore);
                    break;
                case '3':
                    //Crea mostro
                    break;
                default:
                    Console.WriteLine("Scelta non valida");
                    break;


            }

        }

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

            var armaScelta = armi[indiceClasse];

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
    }
}
