using MostriVsEroi.ADORepository.Extensions;
using MostriVsEroi.Core.Entities;
using MostriVsEroi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MostriVsEroi.ADORepository
{
    public class ADOMostroRepository : IMostroRepository
    {
        const string connectionString = @"Persist Security Info = False; Integrated Security = true; Initial Catalog = MostriVsEroi; Server = .\SQLEXPRESS";

        public void Create(Mostro obj)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Aprire la connessione
                connection.Open();

                //Creo il comando
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "INSERT INTO Mostri (NomeMostro, Classe, Arma, Livello) VALUES (@nome, @classe, @arma, @livello)";

                //Valori
                command.Parameters.AddWithValue("@nome", obj.Nome);
                command.Parameters.AddWithValue("@classe", obj.Classe);
                command.Parameters.AddWithValue("@arma", obj.ArmaScelta.NomeArma);
                command.Parameters.AddWithValue("@livello", obj.LivelloMostro.Numero);

                command.ExecuteNonQuery();
            }
        }

        public bool Delete(Mostro obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Mostro> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Mostro> GetByLivello(int numeroLivello)
        {
            List<Mostro> mostri = new List<Mostro>();

            //ADO
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Apro la connessione
                connection.Open();

                //Comando
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT * FROM MostriConPuntiDannoEPuntiVita WHERE Livello <= @livello";

                //Parametro
                //SqlParameter nomeParam = new SqlParameter();
                command.Parameters.AddWithValue("@livello", numeroLivello);

                //Esecuzione
                SqlDataReader reader = command.ExecuteReader();

                //Lettura dati
                while (reader.Read())
                {
                    mostri.Add(reader.ToMostro());
                }

                //Chiudo connessione
                reader.Close();
                connection.Close();
            }
            return mostri;
        }

        public bool Update(Mostro obj)
        {
            throw new NotImplementedException();
        }
    }
}
