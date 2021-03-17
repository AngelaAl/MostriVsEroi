using MostriVsEroi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MostriVsEroi.Core.Interfaces
{
    public interface IClasseRepository : IRepository<Classe>
    {
        IEnumerable<Classe> GetByFilter(int filter);
    }
}
