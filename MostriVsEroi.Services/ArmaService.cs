using MostriVsEroi.Core.Entities;
using MostriVsEroi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MostriVsEroi.Services
{
    public class ArmaService
    {
        private IArmaRepository _repo;

        public ArmaService(IArmaRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Arma> GetArmiByClasse(Classe classe)
        {
            return _repo.GetByClasse(classe);
        }

    }
}
