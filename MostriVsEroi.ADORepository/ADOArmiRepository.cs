using MostriVsEroi.ADORepository.Extensions;
using MostriVsEroi.Core.Entities;
using MostriVsEroi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MostriVsEroi.ADORepository
{
    public class ADOArmiRepository : IArmaRepository
    {
        const string connectionString = @"Persist Security Info = False; Integrated Security = true; Initial Catalog = MostriVsEroi; Server = .\SQLEXPRESS";

        public void Create(Arma obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Arma obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Arma> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Arma> GetByClasse(Classe classe)
        {
            List<Arma> armi = new List<Arma>();

            //ADO
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Apro la connessione
                connection.Open();

                //Comando
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT * FROM Armi WHERE Classe = @nomeClasse";

                //Parametro
                //SqlParameter nomeParam = new SqlParameter();
                command.Parameters.AddWithValue("@nomeClasse", classe.nomeClasse);

                //Esecuzione
                SqlDataReader reader = command.ExecuteReader();

                //Lettura dati
                while (reader.Read())
                {
                    armi.Add(reader.ToArma());
                }

                //Chiudo connessione
                reader.Close();
                connection.Close();
            }
            return armi;
        }

        public bool Update(Arma obj)
        {
            throw new NotImplementedException();
        }
    }
}
