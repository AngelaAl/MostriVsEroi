using MostriVsEroi.Core.Entities;
using MostriVsEroi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MostriVsEroi.Services
{
    public class ClasseService
    {
        private IClasseRepository _repo;

        public ClasseService(IClasseRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Classe> GetClassiFiltrate(int filter)
        {
            return _repo.GetByFilter(filter);
        }
    }
}
