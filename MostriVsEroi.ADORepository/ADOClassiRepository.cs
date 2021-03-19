using MostriVsEroi.ADORepository.Extensions;
using MostriVsEroi.Core.Entities;
using MostriVsEroi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MostriVsEroi.ADORepository
{
    public class ADOClassiRepository : IClasseRepository
    {
        const string connectionString = @"Persist Security Info = False; Integrated Security = true; Initial Catalog = MostriVsEroi; Server = .\SQLEXPRESS";

        public void Create(Classe obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Classe obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Classe> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Classe> GetByFilter(int filter)
        {
            List<Classe> classi = new List<Classe>();

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
                    command.CommandText = "SELECT * FROM Classi WHERE Eroe = @filter";

                    //Parametro
                    //SqlParameter nomeParam = new SqlParameter();
                    command.Parameters.AddWithValue("@filter", filter);

                    //Esecuzione
                    SqlDataReader reader = command.ExecuteReader();

                    //Lettura dati
                    while (reader.Read())
                    {
                        classi.Add(reader.ToClasse());
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
            return classi;

        }

        public bool Update(Classe obj)
        {
            throw new NotImplementedException();
        }
    }
}
