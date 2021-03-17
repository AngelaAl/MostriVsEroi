using MostriVsEroi.ADORepository.Extensions;
using MostriVsEroi.Core.Entities;
using MostriVsEroi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MostriVsEroi.ADORepository
{
    public class ADOLivelloRepository : ILivelloRepository
    {
        const string connectionString = @"Persist Security Info = False; Integrated Security = true; Initial Catalog = MostriVsEroi; Server = .\SQLEXPRESS";

        public void Create(Livello obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Livello obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Livello> GetAll()
        {
            List<Livello> livelli = new List<Livello>();

            //ADO
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Apro la connessione
                connection.Open();

                //Comando
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT * FROM Livelli";

                //Esecuzione
                SqlDataReader reader = command.ExecuteReader();

                //Lettura dati
                while (reader.Read())
                {
                    livelli.Add(reader.ToLivello());
                }

                //Chiudo connessione
                reader.Close();
                connection.Close();
            }
            return livelli;
        }

        public bool Update(Livello obj)
        {
            throw new NotImplementedException();
        }
    }
}
