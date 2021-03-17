using MostriVsEroi.ADORepository.Extensions;
using MostriVsEroi.Core.Entities;
using MostriVsEroi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MostriVsEroi.ADORepository
{
    public class ADOEroeRepository : IEroeRepository
    {
        const string connectionString = @"Persist Security Info = False; Integrated Security = true; Initial Catalog = MostriVsEroi; Server = .\SQLEXPRESS";
        public void Create(Eroe obj)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                //Aprire la connessione
                connection.Open();

                //Creo il comando
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "INSERT INTO Eroi (NomeEroe, Classe, Arma, Livello, PuntiVita, PuntiAccumulati, NomeGiocatore) VALUES (@nome, @classe, @arma, @livello, @puntiVita, @puntiAccumulati, @giocatore)";

                //Valori
                command.Parameters.AddWithValue("@nome", obj.Nome);
                command.Parameters.AddWithValue("@classe", obj.Classe);
                command.Parameters.AddWithValue("@arma", obj.ArmaScelta.NomeArma);
                command.Parameters.AddWithValue("@livello", obj.Livello);
                command.Parameters.AddWithValue("@puntiVita", obj.PuntiVita);
                command.Parameters.AddWithValue("@puntiAccumulati", obj.PuntiAccumulati);
                command.Parameters.AddWithValue("@giocatore", obj.GiocatoreAssegnato);

                command.ExecuteNonQuery();
            }
        }

        public bool Delete(Eroe obj)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                //Aprire connessione
                connection.Open();

                //Comando
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "DELETE FROM Eroi WHERE NomeEroe = @nomeEroe";

                command.Parameters.AddWithValue("@nomeEroe", obj.Nome);

                //Eseguo
                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public IEnumerable<Eroe> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Eroe> GetByGiocatore(Giocatore giocatore)
        {
            string nomeGiocatore = giocatore.Nome;
            List<Eroe> eroi = new List<Eroe>();

            //ADO
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                //Apro la connessione
                connection.Open();

                //Comando
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT * FROM EroiConPuntiDanno WHERE NomeGiocatore = @nomeGiocatore";

                //Parametro
                //SqlParameter nomeParam = new SqlParameter();
                command.Parameters.AddWithValue("@nomeGiocatore", nomeGiocatore);

                //Esecuzione
                SqlDataReader reader = command.ExecuteReader();

                //Lettura dati
                while(reader.Read())
                {
                    eroi.Add(reader.ToEroe());
                }

                //Chiudo connessione
                reader.Close();
                connection.Close();
            }
            return eroi;
        }

        public bool Update(Eroe obj)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Aprire la connessione
                connection.Open();

                //Creo il comando
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "UPDATE Eroi SET Livello = @livello, PuntiVita = @puntiVita, PuntiAccumulati = @puntiAccumulati WHERE NomeEroe = @nome";

                //Valori
                command.Parameters.AddWithValue("@nome", obj.Nome);
                command.Parameters.AddWithValue("@livello", obj.Livello);
                command.Parameters.AddWithValue("@puntiVita", obj.PuntiVita);
                command.Parameters.AddWithValue("@puntiAccumulati", obj.PuntiAccumulati);

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
