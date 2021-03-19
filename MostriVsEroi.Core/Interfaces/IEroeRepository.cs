using System;
using System.Collections.Generic;
using System.Text;
using MostriVsEroi.Core.Entities;

namespace MostriVsEroi.Core.Interfaces
{
    public interface IEroeRepository : IRepository<Eroe>
    {
        IEnumerable<Eroe> GetByGiocatore(Giocatore giocatore);

        IEnumerable<string> GetNomiEroi();
    }
}
