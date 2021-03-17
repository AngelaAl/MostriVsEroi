using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using MostriVsEroi.ADORepository;
using MostriVsEroi.Core.Interfaces;
using MostriVsEroi.Services;

namespace MostriVsEroi
{
    //Provider generico di servizi
    public class DIConfiguration
    {
        public static ServiceProvider Configurazione()
        {
            return new ServiceCollection()
                .AddTransient<EroeService>()

                .AddTransient<IEroeRepository, ADOEroeRepository>()

                .BuildServiceProvider();
        }
    }
}
