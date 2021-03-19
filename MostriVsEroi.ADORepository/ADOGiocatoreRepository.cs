using MostriVsEroi.ADORepository.Extensions;
using MostriVsEroi.Core.Entities;
using MostriVsEroi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MostriVsEroi.ADORepository
{
    public class ADOGiocatoreRepository : IGiocatoreRepository
    {
        const string connectionString = @"Persist Security Info = False; Integrated Security = true; Initial Catalog = MostriVsEroi; Server = .\SQLEXPRESS";

        public void Create(Giocatore obj)
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
                    command.CommandText = "INSERT INTO Giocatori (NomeGiocatore, Ruolo) VALUES (@nome, @ruolo)";

                    //Valori
                    command.Parameters.AddWithValue("@nome", obj.Nome);
                    command.Parameters.AddWithValue("@ruolo", obj.Ruolo);

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

        public bool Delete(Giocatore obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Giocatore> GetAll()
        {
            List<Giocatore> giocatori = new List<Giocatore>();

           
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
                    command.CommandText = "SELECT * FROM Giocatori";


                    //Esecuzione
                    SqlDataReader reader = command.ExecuteReader();

                    //Lettura dati
                    while (reader.Read())
                    {
                        giocatori.Add(reader.ToGiocatore());
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
            return giocatori;
        }

        public bool Update(Giocatore obj)
        {
            throw new NotImplementedException();
        }
    }
}
