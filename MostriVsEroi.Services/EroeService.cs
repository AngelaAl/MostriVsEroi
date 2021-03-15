using System;
using MostriVsEroi.Core.Interfaces;

namespace MostriVsEroi.Services
{
    public class EroeService
    {
        private IEroeRepository _repo;

        public EroeService(IEroeRepository repo)
        {
            _repo = repo;
        }


    }
}
