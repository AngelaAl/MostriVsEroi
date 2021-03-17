using MostriVsEroi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MostriVsEroi.ADORepository.Extensions
{
    public static class SqlDataReaderExtension
    {
        public static Eroe ToEroe(this SqlDataReader reader)
        {
            var armaEroe = new Arma()
            {
                NomeArma = reader["Arma"].ToString(),
                PuntiDanno = (int)reader["PuntiDanno"],
                Classe = reader["Classe"].ToString()

            };

            

            return new Eroe()
            {
                Nome = reader["NomeEroe"].ToString(),
                Classe = reader["Classe"].ToString(),
                ArmaScelta = armaEroe,
                Livello = (int)reader["Livello"],
                PuntiVita = (int)reader["PuntiVita"],
                PuntiAccumulati = (int)reader["PuntiAccumulati"],
                GiocatoreAssegnato = reader["NomeGiocatore"].ToString()

            };
        }
    }
}
