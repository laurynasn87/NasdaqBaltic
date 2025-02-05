﻿using Database;
using Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DALs
{
    public class AkcijuDAL
    {
        SQLCommands sQLCommands;
        String DefaultDatabaseConn = "Database";
        FinansinesInformacijosDAL finansinisDal = new FinansinesInformacijosDAL();
        PapildomosInformacijosDAL papildomosInfDal = new PapildomosInformacijosDAL();
        const string AkcijuTablePavadinimas = "akcijos";
        public AkcijuDAL()
        {
            sQLCommands = new Database.SQLCommands(DefaultDatabaseConn);
        }
        public AkcijuDAL(string DatbaseConnectionName)
        {
            sQLCommands = new Database.SQLCommands(DatbaseConnectionName);
        }
        public bool Ivesti(Akcijos akcija)
        {
            List<string> IgnoreColumns = new List<string>();
            IgnoreColumns.Add("finansineInformacija");
            IgnoreColumns.Add("papildomaInformacija");

            List<Tuple<string, string>> tuples = akcija.PaverstAkcijaITupleList(IgnoreColumns);

            return sQLCommands.Insert(tuples, AkcijuTablePavadinimas);
        }

        public List <Akcijos> GautiVisus()
        {
            List<Akcijos> akcijos = new List<Akcijos>();
            List<List<Tuple<string, string>>> result = sQLCommands.GetAll(AkcijuTablePavadinimas);

            foreach(List<Tuple<string, string>> vienaAkcija in result)
            {
                if (vienaAkcija.Count > 0)
                {
                    akcijos.Add(new Akcijos().ListToAkcija(vienaAkcija));
                    akcijos.Last().finansineInformacija = finansinisDal.GautiPagalKodaNaujausia(akcijos.Last().AkcijosKodas);
                    akcijos.Last().papildomaInformacija = papildomosInfDal.GautiPagalKoda(akcijos.Last().AkcijosKodas);
                }

            }

            return akcijos;
        }
        public Akcijos GautiPagalKoda(String AkcijosKodas)
        {
            if (!string.IsNullOrEmpty(AkcijosKodas))
            {
                Akcijos rezultatas = new Akcijos();
                List<List<Tuple<string, string>>> result = sQLCommands.GetByCondition(AkcijuTablePavadinimas, new List<Tuple<string, string>>() { new Tuple<string, string>("AkcijosKodas", AkcijosKodas) }, 1);
                foreach (List<Tuple<string, string>> vienaAkcija in result)
                {
                    if (vienaAkcija.Count > 0 && String.IsNullOrEmpty(rezultatas.AkcijosKodas))
                    {
                        rezultatas = rezultatas.ListToAkcija(vienaAkcija);
                        rezultatas.finansineInformacija = finansinisDal.GautiPagalKodaNaujausia(rezultatas.AkcijosKodas);
                        rezultatas.papildomaInformacija = papildomosInfDal.GautiPagalKoda(rezultatas.AkcijosKodas);
                    }

                }
                return rezultatas;
            }
            return null;
        }

        public bool Atnaujinti(Akcijos akcija)
        {
            List<string> IgnoreColumns = new List<string>();
            IgnoreColumns.Add("finansineInformacija");
            IgnoreColumns.Add("papildomaInformacija");

            List<Tuple<string, string>> tuples = akcija.PaverstAkcijaITupleList(IgnoreColumns);

            return sQLCommands.Update(tuples, AkcijuTablePavadinimas, "AkcijosKodas", akcija.AkcijosKodas);
        }

        public bool Istrinti(Akcijos akcija)
        {
            return sQLCommands.Delete(AkcijuTablePavadinimas, "AkcijosKodas", akcija.AkcijosKodas);
        }
    }
}
