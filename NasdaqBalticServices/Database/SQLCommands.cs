using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Xml.Linq;

namespace Database
{
    public class SQLCommands
    {
        MySqlConnection databaseConnection;
        TraceLogger logger = new TraceLogger();
        public static IDictionary<string, string> ConnStrings = new Dictionary<string, string>();
        public SQLCommands(String ConnName)
        {
            ConnStrings = ReadConfig(XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "app.xml"));
            this.databaseConnection = new MySqlConnection(ConnStrings[ConnName]);
            databaseConnection.Open();
        }
        public List<List<Tuple<string, string>>> GetAll(string TableName)
        {
            List<List<Tuple<string, string>>> results = new List<List<Tuple<string, string>>>();
            try
            {
                MySqlCommand command = new MySqlCommand("Select * From " + TableName);
                command.Connection = databaseConnection;
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        List<Tuple<string, string>> VienasIrasas = new List<Tuple<string, string>>();
                        for (int i=0; i< reader.FieldCount; i++)
                        {
                            if (!reader.IsDBNull(i))
                            VienasIrasas.Add(new Tuple<string, string>(reader.GetName(i), reader.GetString(i)));
                        }
                        results.Add(VienasIrasas);
                    }
                }
                reader.Close();
            }
            catch (Exception e)
            {
                logger.Log("Database-SQL-Error: " + e.Message);
                return null;

            }
            return results;
        }
        public List<List<Tuple<string, string>>> GetByCondition(string TableName, List<Tuple<string, string>> SulpeliaiIrReiksmes, int Kiekis = 0, string Order = null, bool DESC = false)
        {
           List<List<Tuple<string, string>>> results = new List<List<Tuple<string, string>>>();
            try
            {
                string Komanda = $"Select * FROM `{TableName}` ";
                string stulpeliai = String.Empty;

                if (SulpeliaiIrReiksmes.Count > 0)
                {
                    stulpeliai = stulpeliai + "Where ";
                    foreach (Tuple<string, string> tuple in SulpeliaiIrReiksmes)
                    {
                        stulpeliai = stulpeliai + tuple.Item1 + " = " + "'" + tuple.Item2 + "' AND ";
                    }
                    stulpeliai = stulpeliai.Remove(stulpeliai.Length - 4);
                }
                if (!String.IsNullOrEmpty(Order))
                {
                    stulpeliai = stulpeliai + $" ORDER BY `{TableName}`.`" + Order + "`";
                    if (DESC) stulpeliai = stulpeliai + " DESC ";
                }

                if (Kiekis > 0)
                {
                    stulpeliai = stulpeliai + " Limit " + Kiekis;
                }


                MySqlCommand command = new MySqlCommand(Komanda + stulpeliai);
                command.Connection = databaseConnection;
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        List<Tuple<string, string>> VienasIrasas = new List<Tuple<string, string>>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (!reader.IsDBNull(i))
                                VienasIrasas.Add(new Tuple<string, string>(reader.GetName(i), reader.GetString(i)));
                        }
                        results.Add(VienasIrasas);
                    }
                }
                reader.Close();
            }
            catch (Exception e)
            {
                logger.Log("Database-SQL-Error: " + e.Message);
                return null;

            }
            return results;
        }
        public bool Insert(List <Tuple <string,string>> SulpeliaiIrReiksmes, string LentelsPav)
        {
            if (String.IsNullOrEmpty(LentelsPav) || SulpeliaiIrReiksmes.Count == 0)
                return false;
            try
            {
                MySqlCommand comm = databaseConnection.CreateCommand();
                string CommandText = $"INSERT INTO {LentelsPav}";
                string stulpeliai = "(";
                string stulpeliai2 = " VALUES(";
                foreach (Tuple<string, string> tuple in SulpeliaiIrReiksmes)
                {
                    stulpeliai = stulpeliai + tuple.Item1 + ',';
                    stulpeliai2 = stulpeliai2 + '@' + tuple.Item1 + ", ";
                    comm.Parameters.AddWithValue('@' + tuple.Item1, tuple.Item2);
                }
                stulpeliai = stulpeliai.Remove(stulpeliai.Length - 1, 1) + ") ";
                stulpeliai2 = stulpeliai2.Remove(stulpeliai2.Length - 2, 2) + ")";

                comm.CommandText = CommandText + stulpeliai + stulpeliai2;
                comm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                logger.Log("Database-SQL-Error: " + e.Message);
                return false;
            }
            return true;
        }

        public bool Delete(string LentelesPav, string StulpelioPavadinimas, string identifikatorius)
        {
            if (String.IsNullOrEmpty(LentelesPav) || String.IsNullOrEmpty(identifikatorius))
                return false;
            try
            {
                MySqlCommand comm = databaseConnection.CreateCommand();
                comm.CommandText = $"DELETE  from {LentelesPav} where {StulpelioPavadinimas} = '{identifikatorius}'";
                comm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                logger.Log("Database-SQL-Error: " + e.Message);
                return false;
            }
            return true;

        }

        public bool Update(List<Tuple<string, string>> NustatomiSulpeliaiIrReiksmes, string LentelsPav, string IdentifikavimoStulpelioPavadinimas, string IdentifikavimoReiksme)
        {
            if (String.IsNullOrEmpty(LentelsPav) || NustatomiSulpeliaiIrReiksmes.Count == 0)
                return false;
            try
            {
                MySqlCommand comm = databaseConnection.CreateCommand();
                string CommandText = $"UPDATE {LentelsPav} SET ";
                string stulpeliai = "";

                NustatomiSulpeliaiIrReiksmes.RemoveAll(x => x.Item1.Equals(IdentifikavimoStulpelioPavadinimas));
                foreach (Tuple<string, string> tuple in NustatomiSulpeliaiIrReiksmes)
                {
                    stulpeliai = stulpeliai + tuple.Item1 + "=@" + tuple.Item1 + ",";
                    comm.Parameters.AddWithValue('@' + tuple.Item1, tuple.Item2);
                }
                comm.Parameters.AddWithValue('@' + IdentifikavimoStulpelioPavadinimas, IdentifikavimoReiksme);
                stulpeliai = stulpeliai.Remove(stulpeliai.Length - 1, 1) + " WHERE " + IdentifikavimoStulpelioPavadinimas + "=@" + IdentifikavimoStulpelioPavadinimas;

                comm.CommandText = CommandText + stulpeliai;
                comm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                logger.Log("Database-SQL-Error: " + e.Message);
                return false;
            }
            return true;
        }

        public void Dispose()
        {
            databaseConnection.Close();
        }
        public IDictionary<string, string> ReadConfig(XDocument myxml)
        {
            IDictionary<string, string> valuePairs = new Dictionary<string, string>();
            IEnumerable<XElement> query = from c in myxml.Descendants("add") select c;
            foreach (var item in query)
            {
                valuePairs.Add(item.FirstAttribute.Value, item.LastAttribute.Value);
            }
            return valuePairs;
        }
    }
}
