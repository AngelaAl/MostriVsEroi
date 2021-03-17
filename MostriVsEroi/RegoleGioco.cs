using MostriVsEroi.ADORepository;
using MostriVsEroi.Core.Entities;
using MostriVsEroi.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MostriVsEroi
{

    public static class RegoleGioco
    {
        public static Giocatore CheckGiocatore(string nomeGiocatore)
        {
            GiocatoreService giocatoreService = new GiocatoreService(new ADOGiocatoreRepository());
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

        public static List<Classe> ClassiPerEroe()
        {
            ClasseService classeService = new ClasseService(new ADOClassiRepository());
            var classi = classeService.GetClassiFiltrate(1).ToList();
            return classi;
        }

        public static List<Arma> ArmiPerClasse(Classe classe)
        {
            ArmaService armaService = new ArmaService(new ADOArmiRepository());
            var armi = armaService.GetArmiByClasse(classe).ToList();
            return armi;
        }

        public static void SalvaEroe(Eroe eroe)
        {
            EroeService eroeService = new EroeService(new ADOEroeRepository());
            eroeService.CreateNewEroe(eroe);
        }

        public static List<Eroe> EroiDelGiocatore(Giocatore giocatore)
        {
            EroeService eroeService = new EroeService(new ADOEroeRepository());
            var eroi = eroeService.GetAllEroiByGiocatori(giocatore).ToList();
            return eroi;
        }
    }
}
