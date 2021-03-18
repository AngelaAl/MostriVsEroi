using MostriVsEroi.Core.Entities;
using MostriVsEroi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MostriVsEroi.Services
{
    public class StatisticaService
    {
        private IStatisticaRepository _repo;

        public StatisticaService(IStatisticaRepository repo)
        {
            _repo = repo;
        }

        public void CreateNewStatistica(Statistica statistica)
        {
            _repo.Create(statistica);
        }

        public void UpdateStatistica(Eroe eroe, int millisecondi)
        {
            _repo.UpdateTempo(eroe, millisecondi);
        }

        public IEnumerable<Statistica> GetStatistiche()
        {
            return _repo.GetAll();
        }

        public IEnumerable<Statistica> GetStatisticheByGiocatore(Giocatore giocatore)
        {
            return _repo.GetAllByGiocatore(giocatore);
        }
    }
}
