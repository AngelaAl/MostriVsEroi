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
                try
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
                catch (SqlException)
                {
                    Console.WriteLine("Siamo spiacenti, è stato rilevato un errore");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public bool Delete(Eroe obj)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    //Aprire connessione
                    connection.Open();

                    //Comando
                    string deleteFromStatistiche = "DELETE FROM Statistiche WHERE NomeEroe = @nomeEroe";
                    string deleteFromEroe = " DELETE FROM Eroi WHERE NomeEroe = @nomeEroe";
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = deleteFromStatistiche + deleteFromEroe;

                    command.Parameters.AddWithValue("@nomeEroe", obj.Nome);

                    //Eseguo
                
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (SqlException)
                {
                    Console.WriteLine("Siamo spiacenti, è stato rilevato un errore");
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
            List<Eroe> eroi = new List<Eroe>();

            //ADO
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    //Apro la connessione
                    connection.Open();

                    //Comando
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT * FROM Eroi";


                    //Esecuzione
                    SqlDataReader reader = command.ExecuteReader();

                    //Lettura dati
                    while (reader.Read())
                    {
                        eroi.Add(reader.ToEroe());
                    }

                    //Chiudo il reader
                    reader.Close();
                }
                catch (SqlException)
                {
                    Console.WriteLine("Siamo spiacenti, è stato rilevato un errore");
                }
                finally
                {
                    connection.Close();
                }
            }
            return eroi;
        }

        public IEnumerable<Eroe> GetByGiocatore(Giocatore giocatore)
        {
            string nomeGiocatore = giocatore.Nome;
            List<Eroe> eroi = new List<Eroe>();

            //ADO
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                try
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
                    while (reader.Read())
                    {
                        eroi.Add(reader.ToEroe());
                    }

                    //Chiudo il reader
                    reader.Close();
                }
                catch (SqlException)
                {
                    Console.WriteLine("Siamo spiacenti, è stato rilevato un errore");
                }
                finally
                {
                    connection.Close();
                }
            }
            return eroi;
        }

        public IEnumerable<string> GetNomiEroi()
        {
            List<string> nomiEroi = new List<string>();

            //ADO
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    //Apro la connessione
                    connection.Open();

                    //Comando
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT NomeEroe FROM Eroi";


                    //Esecuzione
                    SqlDataReader reader = command.ExecuteReader();

                    //Lettura dati
                    while (reader.Read())
                    {
                        nomiEroi.Add(reader.ToNomeEroe());
                    }

                    //Chiudo il reader
                    reader.Close();
                }
                catch (SqlException)
                {
                    Console.WriteLine("Siamo spiacenti, è stato rilevato un errore");
                }
                finally
                {
                    connection.Close();
                }
            }
            return nomiEroi;
        }

        public bool Update(Eroe obj)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
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

                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Siamo spiacenti, è stato rilevato un errore");
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
