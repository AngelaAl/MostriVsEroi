using MostriVsEroi.Core.Entities;
using MostriVsEroi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MostriVsEroi.Services
{
    public class LivelloService
    {
        private ILivelloRepository _repo;

        public LivelloService(ILivelloRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Livello> GetAllLivelli()
        {
            return _repo.GetAll();
        }

    }
}
