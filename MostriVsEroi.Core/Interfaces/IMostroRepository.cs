using MostriVsEroi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MostriVsEroi.Core.Interfaces
{
    public interface IMostroRepository : IRepository<Mostro>
    {
        IEnumerable<Mostro> GetByLivello(int numeroLivello);
    }
}
