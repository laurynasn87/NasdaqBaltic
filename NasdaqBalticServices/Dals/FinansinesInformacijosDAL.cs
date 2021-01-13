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
        public List<FinansineInformacija> GautiVisus()
        {
            List<FinansineInformacija> finansinesInformacijos = new List<FinansineInformacija>();
            List<List<Tuple<string, string>>> result = sQLCommands.GetAll(FinansinesInformacijosTablePavadinimas);
            if (result != null && result.Count > 0)
                foreach (List<Tuple<string, string>> vienasFI in result)
                {
                    if (vienasFI.Count > 0)
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
                List<List<Tuple<string, string>>> result = sQLCommands.GetByCondition(FinansinesInformacijosTablePavadinimas, new List<Tuple<string, string>>() { new Tuple<string, string>("AkcijosKodas", AkcijosKodas) }, 1, "Id", true);
                FinansineInformacija rezultatas = new FinansineInformacija();
                if (result != null && result.Count > 0)
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
        public Dictionary<string, Dictionary<DateTime, double>> GautiStatistikas(string Akcijoskodas)
        {
            if (!string.IsNullOrEmpty(Akcijoskodas))
            {
                Dictionary<DateTime, List<FinansineInformacija>> FinansineInformacijaGrupuotaPagalDatas = new Dictionary<DateTime, List<FinansineInformacija>>();
                FinansineInformacija temp = new FinansineInformacija();
                List<FinansineInformacija> VisosAkcijosFinansinesInformacijos = new List<FinansineInformacija>();
                List<List<Tuple<string, string>>> result = sQLCommands.GetByCondition(FinansinesInformacijosTablePavadinimas, new List<Tuple<string, string>>() { new Tuple<string, string>("AkcijosKodas", Akcijoskodas) });
                foreach (List<Tuple<string, string>> vienaFiN in result)
                {
                    if (vienaFiN.Count > 0)
                    {
                        VisosAkcijosFinansinesInformacijos.Add(temp.ListToFinansineInformacija(vienaFiN));
                    }

                }


                foreach (FinansineInformacija finansineInformacija in VisosAkcijosFinansinesInformacijos)
                {
                    KeyValuePair<DateTime, List<FinansineInformacija>> dienosFinansineInformacija = FinansineInformacijaGrupuotaPagalDatas.FirstOrDefault(x => x.Key.Month == finansineInformacija.Timestamp.Month && x.Key.Day == finansineInformacija.Timestamp.Day);
                    if (!dienosFinansineInformacija.Equals(new KeyValuePair<DateTime, List<FinansineInformacija>>()))
                    {
                        dienosFinansineInformacija.Value.Add(finansineInformacija);
                    }
                    else
                    {
                        if (finansineInformacija.Timestamp.DayOfWeek != DayOfWeek.Saturday && finansineInformacija.Timestamp.DayOfWeek != DayOfWeek.Sunday)
                            FinansineInformacijaGrupuotaPagalDatas.Add(finansineInformacija.Timestamp, new List<FinansineInformacija>() { finansineInformacija });
                    }

                }
                Dictionary<DateTime, double> MenesineStatistikaPirkimas = new Dictionary<DateTime, double>();
                Dictionary<DateTime, double> MenesineStatistikaPardavimas = new Dictionary<DateTime, double>();

                foreach (KeyValuePair<DateTime, List<FinansineInformacija>> VienaDiena in FinansineInformacijaGrupuotaPagalDatas)
                {
                    double KainaPirkimas = 0;
                    int KainaPirkimasCount = 0;
                    double KainaPardvimas = 0;
                    int KainaPardvimasCount = 0;

                    foreach (FinansineInformacija finansineInformacija in VienaDiena.Value)
                    {
                        if (finansineInformacija.PirkimoKaina > 0)
                        {
                            KainaPirkimas += finansineInformacija.PirkimoKaina;
                            KainaPirkimasCount++;
                        }
                        if (finansineInformacija.PardavimoKaina > 0)
                        {
                            KainaPardvimas += finansineInformacija.PardavimoKaina;
                            KainaPardvimasCount++;
                        }
                    }
                    if (KainaPirkimas==0 || KainaPirkimasCount==0)
                        KainaPirkimas = 0;
                    else
                    KainaPirkimas = KainaPirkimas / KainaPirkimasCount;
                    if (KainaPardvimas == 0 || KainaPardvimasCount == 0)
                        KainaPardvimas = 0;
                    else
                        KainaPardvimas = KainaPardvimas / KainaPardvimasCount;

                    MenesineStatistikaPirkimas.Add(VienaDiena.Key, KainaPirkimas);
                    MenesineStatistikaPardavimas.Add(VienaDiena.Key, KainaPardvimas);
                }

                Dictionary<DateTime, double> DienaStatistikaPirkimas = new Dictionary<DateTime, double>();
                Dictionary<DateTime, double> DienaStatistikaPardavimas = new Dictionary<DateTime, double>();

                var cutoff = DateTime.Now.Subtract(new TimeSpan(24, 0, 0));

                foreach (FinansineInformacija finansineInformacija in VisosAkcijosFinansinesInformacijos.Where(x => x.Timestamp > cutoff).OrderBy(x => x.Timestamp))
                {
                    if (!DienaStatistikaPirkimas.Any(x => x.Key.Hour == finansineInformacija.Timestamp.Hour))
                    {
                        DienaStatistikaPirkimas.Add(finansineInformacija.Timestamp, finansineInformacija.PirkimoKaina);
                        DienaStatistikaPardavimas.Add(finansineInformacija.Timestamp, finansineInformacija.PardavimoKaina);
                    }

                }

                Dictionary<string, Dictionary<DateTime, double>> StatistikosInfo = new Dictionary<string, Dictionary<DateTime, double>>();
                StatistikosInfo.Add("Menesis-Pirkimas", MenesineStatistikaPirkimas);
                StatistikosInfo.Add("Menesis-Pardavimas", MenesineStatistikaPardavimas);
                StatistikosInfo.Add("Diena-Pirkimas", DienaStatistikaPirkimas);
                StatistikosInfo.Add("Diena-Pardavimas", DienaStatistikaPardavimas);

                return StatistikosInfo;
            }
            return null;
        }

    }
}
