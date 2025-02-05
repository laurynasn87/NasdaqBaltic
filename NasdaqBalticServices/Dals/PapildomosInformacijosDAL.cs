﻿using Database;
using Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DALs
{
    public class PapildomosInformacijosDAL
    {
        SQLCommands sQLCommands;
        String DefaultDatabaseConn = "Database";
        const string PapildomosInformacijosTablePavadinimas = "papildomainformacija";
        public PapildomosInformacijosDAL()
        {
            sQLCommands = new Database.SQLCommands(DefaultDatabaseConn);
        }
        public PapildomosInformacijosDAL(string DatbaseConnectionName)
        {
            sQLCommands = new Database.SQLCommands(DatbaseConnectionName);
        }
        public List<PapildomaInformacija> GautiVisus()
        {
            List<PapildomaInformacija> papildomaInformacija = new List<PapildomaInformacija>();
            List<List<Tuple<string, string>>> result = sQLCommands.GetAll(PapildomosInformacijosTablePavadinimas);
            foreach (List<Tuple<string, string>> PI in result)
            {
                if (PI.Count > 0)
                {
                    papildomaInformacija.Add(new PapildomaInformacija().ListToPapildomaInformacija(PI));
                }
            }

            return papildomaInformacija;
        }
        public bool Ivesti(PapildomaInformacija papildomaInformacija)
        {
            List<Tuple<string, string>> tuples = papildomaInformacija.PaverstPapildomaInformacijaITupleList();

            return sQLCommands.Insert(tuples, PapildomosInformacijosTablePavadinimas);
        }
        public bool Atnaujinti(PapildomaInformacija papildomaInformacija)
        {
            List<Tuple<string, string>> tuples = papildomaInformacija.PaverstPapildomaInformacijaITupleList(new List<string>() { "AkcijosKodas"});
            
            return sQLCommands.Update(tuples, PapildomosInformacijosTablePavadinimas, "AkcijosKodas", papildomaInformacija.AkcijosKodas);
        }

        public bool Istrinti(PapildomaInformacija papildomaInformacija)
        {
            return sQLCommands.Delete(PapildomosInformacijosTablePavadinimas, "Id", papildomaInformacija.Id.ToString());
        }
        public PapildomaInformacija GautiPagalKoda(String AkcijosKodas)
        {
            if (!String.IsNullOrEmpty(AkcijosKodas))
            {
                List<List<Tuple<string, string>>> result = sQLCommands.GetByCondition(PapildomosInformacijosTablePavadinimas, new List<Tuple<string, string>>() { new Tuple<string, string>("AkcijosKodas", AkcijosKodas) }, 1);
                PapildomaInformacija rezultatas = new PapildomaInformacija();
                foreach (List<Tuple<string, string>> vienasFI in result)
                {
                if (vienasFI.Count > 0 && String.IsNullOrEmpty(rezultatas.AkcijosKodas))
                {
                    rezultatas = rezultatas.ListToPapildomaInformacija(vienasFI);
                }
            }
            return rezultatas;
            }
            return null;
        }
        public PapildomaInformacija GautiPagalId(String id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                PapildomaInformacija rezultatas = new PapildomaInformacija();
                List<List<Tuple<string, string>>> result = sQLCommands.GetByCondition(PapildomosInformacijosTablePavadinimas, new List<Tuple<string, string>>() { new Tuple<string, string>("Id", id) }, 1);
                foreach (List<Tuple<string, string>> vienaaPiN in result)
                {
                    if (vienaaPiN.Count > 0 && rezultatas.Id == 0)
                    {
                        rezultatas = rezultatas.ListToPapildomaInformacija(vienaaPiN);
                    }

                }
                return rezultatas;
            }
            return null;
        }

    }
}
