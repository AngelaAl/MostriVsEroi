using MostriVsEroi.ADORepository.Extensions;
using MostriVsEroi.Core.Entities;
using MostriVsEroi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MostriVsEroi.ADORepository
{
    public class ADOStatisticaRepository : IStatisticaRepository
    {
        const string connectionString = @"Persist Security Info = False; Integrated Security = true; Initial Catalog = MostriVsEroi; Server = .\SQLEXPRESS";

        public void Create(Statistica obj)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Aprire la connessione
                connection.Open();

                //Creo il comando
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "INSERT INTO Statistiche (NomeEroe, TempoTotaleGioco) VALUES (@nome, @tempoTotaleGioco)";

                //Valori
                command.Parameters.AddWithValue("@nome", obj.NomeEroe);
                command.Parameters.AddWithValue("@tempoTotaleGioco", obj.TempoTotaleGioco);

                command.ExecuteNonQuery();
            }
        }

        public bool Delete(Statistica obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Statistica> GetAll()
        {
            List<Statistica> statistiche = new List<Statistica>();

            //ADO
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Apro la connessione
                connection.Open();

                //Comando
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT * FROM StatistichePuntiAccumulatiGiocatore";

                //Esecuzione
                SqlDataReader reader = command.ExecuteReader();

                //Lettura dati
                while (reader.Read())
                {
                    statistiche.Add(reader.ToStatistica());
                }

                //Chiudo connessione
                reader.Close();
                connection.Close();
            }
            return statistiche;
        }

        public IEnumerable<Statistica> GetAllByGiocatore(Giocatore giocatore)
        {
            List<Statistica> statistiche = new List<Statistica>();

            //ADO
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Apro la connessione
                connection.Open();

                //Comando
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT * FROM StatistichePuntiAccumulatiGiocatore WHERE NomeGiocatore = @nome";

                //Parametro
                command.Parameters.AddWithValue("@nome", giocatore.Nome);

                //Esecuzione
                SqlDataReader reader = command.ExecuteReader();

                //Lettura dati
                while (reader.Read())
                {
                    statistiche.Add(reader.ToStatistica());
                }

                //Chiudo connessione
                reader.Close();
                connection.Close();
            }
            return statistiche;
        }

        public bool Update(Statistica obj)
        {
            throw new NotImplementedException();
        }

        public bool UpdateTempo(Eroe eroe, int millisecondi)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Aprire la connessione
                connection.Open();

                //Creo il comando
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "UPDATE Statistiche SET TempoTotaleGioco += @millisecondi WHERE NomeEroe = @nome";

                //Valori
                command.Parameters.AddWithValue("@millisecondi", millisecondi);
                command.Parameters.AddWithValue("@nome", eroe.Nome);

                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

        }
    }
}
