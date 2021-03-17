using MostriVsEroi.Core.Entities;
using MostriVsEroi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MostriVsEroi.Services
{
    public class GiocatoreService
    {
        private IGiocatoreRepository _repo;

        public GiocatoreService(IGiocatoreRepository repo)
        {
            _repo = repo;
        }

        public void CreateGiocatore(Giocatore giocatore)
        {
            _repo.Create(giocatore);
        }

        public IEnumerable<Giocatore> GetAllGiocatori()
        {
            return _repo.GetAll();
        }
    }
}
