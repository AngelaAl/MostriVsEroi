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
                GiocatoreAssegnato = reader["NomeGiocatore"].ToString(),
                

            };
        }

        public static Mostro ToMostro(this SqlDataReader reader)
        {
            var armaMostro = new Arma()
            {
                NomeArma = reader["Arma"].ToString(),
                PuntiDanno = (int)reader["PuntiDanno"],
                Classe = reader["Classe"].ToString()
            };

            var livello = new Livello()
            {
                Numero = (int)reader["Livello"],
                PuntiVita = (int)reader["PuntiVita"]
            };

            return new Mostro()
            {
                Nome = reader["NomeMostro"].ToString(),
                Classe = reader["Classe"].ToString(),
                ArmaScelta = armaMostro,
                LivelloMostro = livello
            };
        }

        public static Giocatore ToGiocatore(this SqlDataReader reader)
        {
            return new Giocatore()
            {
                Nome = reader["NomeGiocatore"].ToString(),
                Ruolo = reader["Ruolo"].ToString()
            };
        }

        public static Classe ToClasse(this SqlDataReader reader)
        {
            return new Classe()
            {
                nomeClasse = reader["Nome"].ToString()
            };
        }

        public static Arma ToArma(this SqlDataReader reader)
        {
            return new Arma()
            {
                NomeArma = reader["NomeArma"].ToString(),
                PuntiDanno = (int)reader["PuntiDanno"],
                Classe = reader["Classe"].ToString()
            };
        }

        public static Livello ToLivello(this SqlDataReader reader)
        {
            return new Livello()
            {
                Numero = (int)reader["ID"],
                PuntiVita = (int)reader["PuntiVita"],
                PuntiPerPassaggio = (int)reader["PuntiPerPassaggio"]
            };
        }

        public static Statistica ToStatistica(this SqlDataReader reader)
        {
            return new Statistica()
            {
                NomeEroe = reader["NomeEroe"].ToString(),
                TempoTotaleGioco = (int)reader["TempoTotaleGioco"],
                PuntiAccumulati = (int)reader["PuntiAccumulati"],
                GiocatoreAssegnato = reader["NomeGiocatore"].ToString()
            };
        }
    }
}
