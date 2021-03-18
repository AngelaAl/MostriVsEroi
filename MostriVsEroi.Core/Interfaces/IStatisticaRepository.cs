using MostriVsEroi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MostriVsEroi.Core.Interfaces
{
    public interface IStatisticaRepository : IRepository<Statistica>
    {
        bool UpdateTempo(Eroe eroe, int millisecondi);

        IEnumerable<Statistica> GetAllByGiocatore(Giocatore giocatore);
    }
}
