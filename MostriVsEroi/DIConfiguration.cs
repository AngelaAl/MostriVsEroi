using Microsoft.Extensions.DependencyInjection;
using MostriVsEroi.ADORepository;
using MostriVsEroi.Core.Interfaces;
using MostriVsEroi.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MostriVsEroi
{
    //Provider di Servizi
    public class DIConfiguration
    {
        public static ServiceProvider Configurazione()
        {
            return new ServiceCollection()
                .AddScoped<EroeService>()
                .AddScoped<IEroeRepository, ADOEroeRepository>()
                
                .AddScoped<ArmaService>()
                .AddScoped<IArmaRepository, ADOArmiRepository>()

                .AddScoped<ClasseService>()
                .AddScoped<IClasseRepository, ADOClassiRepository>()

                .AddScoped<GiocatoreService>()
                .AddScoped<IGiocatoreRepository, ADOGiocatoreRepository>()

                .AddScoped<MostroService>()
                .AddScoped<IMostroRepository, ADOMostroRepository>()

                .AddScoped<LivelloService>()
                .AddScoped<ILivelloRepository, ADOLivelloRepository>()

                .AddScoped<StatisticaService>()
                .AddScoped<IStatisticaRepository, ADOStatisticaRepository>()

                .BuildServiceProvider();
        }
    }
}
