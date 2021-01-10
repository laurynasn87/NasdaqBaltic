using Database;
using Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DALs
{
    public class FinansinesInformacijosDAL
    {
        SQLCommands sQLCommands;
        String DefaultDatabaseConn = "Database";
        const string FinansinesInformacijosTablePavadinimas = "finansineinformacija";
        public FinansinesInformacijosDAL()
        {
            sQLCommands = new Database.SQLCommands(DefaultDatabaseConn);
        }
        public FinansinesInformacijosDAL(string DatbaseConnectionName)
        {
            sQLCommands = new Database.SQLCommands(DatbaseConnectionName);
        }
        public List <FinansineInformacija> GautiVisus()
        {
            List<FinansineInformacija> finansinesInformacijos = new List<FinansineInformacija>();
            List<List<Tuple<string, string>>> result = sQLCommands.GetAll(FinansinesInformacijosTablePavadinimas);
            if (result != null && result.Count > 0)
            foreach (List<Tuple<string, string>> vienasFI in result)
            {
                if (vienasFI.Count>0)
                {
                    finansinesInformacijos.Add(new FinansineInformacija().ListToFinansineInformacija(vienasFI));
                }

            }

            return finansinesInformacijos;
        }
        public bool Ivesti(FinansineInformacija finansineInformacija)
        {
            List<string> IgnoreColumns = new List<string>();
            IgnoreColumns.Add("Timestamp");

            List<Tuple<string, string>> tuples = finansineInformacija.PaverstFinansineinformacijaITupleList(IgnoreColumns);

            return sQLCommands.Insert(tuples, FinansinesInformacijosTablePavadinimas);
        }
        public bool Atnaujinti(FinansineInformacija finansineInformacija)
        {
            List<string> IgnoreColumns = new List<string>();
            IgnoreColumns.Add("Id");
            IgnoreColumns.Add("Timestamp");

            List<Tuple<string, string>> tuples = finansineInformacija.PaverstFinansineinformacijaITupleList(IgnoreColumns);

            return sQLCommands.Update(tuples, FinansinesInformacijosTablePavadinimas, "Id", finansineInformacija.Id.ToString());
        }

        public bool Istrinti(FinansineInformacija finansineInformacija)
        {
            return sQLCommands.Delete(FinansinesInformacijosTablePavadinimas, "Id", finansineInformacija.Id.ToString());
        }
        public FinansineInformacija GautiPagalKodaNaujausia(String AkcijosKodas)
        {
            if (!String.IsNullOrEmpty(AkcijosKodas))
            { 
                List<List<Tuple<string, string>>> result = sQLCommands.GetByCondition(FinansinesInformacijosTablePavadinimas, new List<Tuple<string, string>>(){ new Tuple<string, string>("AkcijosKodas", AkcijosKodas) },1,"Id",true);
                FinansineInformacija rezultatas = new FinansineInformacija();
                if (result !=  null && result.Count>0)
                foreach (List<Tuple<string, string>> vienasFI in result)
                {
                    if (vienasFI.Count > 0 && String.IsNullOrEmpty(rezultatas.AkcijosKodas))
                    {
                        rezultatas = rezultatas.ListToFinansineInformacija(vienasFI);
                    }

                }
            return rezultatas;
            }
            return null;
        }
        public FinansineInformacija GautiPagalId(String Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                FinansineInformacija rezultatas = new FinansineInformacija();
                List<List<Tuple<string, string>>> result = sQLCommands.GetByCondition(FinansinesInformacijosTablePavadinimas, new List<Tuple<string, string>>() { new Tuple<string, string>("Id", Id) }, 1);
                foreach (List<Tuple<string, string>> vienaFiN in result)
                {
                    if (vienaFiN.Count > 0 && rezultatas.Id == 0)
                    {
                        rezultatas = rezultatas.ListToFinansineInformacija(vienaFiN);
                    }

                }
                return rezultatas;
            }
            return null;
        }

    }
}
