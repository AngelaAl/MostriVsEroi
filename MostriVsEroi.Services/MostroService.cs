using MostriVsEroi.Core.Entities;
using MostriVsEroi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MostriVsEroi.Services
{
    public class MostroService 
    {
        private IMostroRepository _repo;

        public MostroService(IMostroRepository repo)
        {
            _repo = repo;
        }

        public void CreateNewMostro(Mostro mostro)
        {
            _repo.Create(mostro);
        }

        public IEnumerable<Mostro> GetMostriByLivello(int numeroLivello)
        {
            return _repo.GetByLivello(numeroLivello);
        }
    }
}
