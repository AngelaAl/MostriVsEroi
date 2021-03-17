using System;
using System.Collections.Generic;
using MostriVsEroi.Core.Entities;
using MostriVsEroi.Core.Interfaces;

namespace MostriVsEroi.Services
{
    public class EroeService
    {
        private IEroeRepository _repo;

        public EroeService(IEroeRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Eroe> GetAllEroiByGiocatori(Giocatore giocatore)
        {
            return _repo.GetByGiocatore(giocatore);
        }

        public void CreateNewEroe(Eroe eroe)
        {
            _repo.Create(eroe);
        }

        public bool DeleteEroe(Eroe eroe)
        {
            return _repo.Delete(eroe);
        }

        


    }
}
