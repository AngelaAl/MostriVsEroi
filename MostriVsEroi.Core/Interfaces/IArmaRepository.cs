using MostriVsEroi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MostriVsEroi.Core.Interfaces
{
    public interface IArmaRepository : IRepository<Arma>
    {
        IEnumerable<Arma> GetByClasse(Classe classe);
    }
}
