using Database;
using Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Dals
{
    public class PapildomosInformacijosDAL : IDisposable
    {
        SQLCommands sQLCommands = new Database.SQLCommands();
        const string PapildomosInformacijosTablePavadinimas = "papildomainformacija";
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
            List<Tuple<string, string>> tuples = papildomaInformacija.PaverstPapildomaInformacijaITupleList();
            
            return sQLCommands.Update(tuples, PapildomosInformacijosTablePavadinimas, "Id", papildomaInformacija.Id.ToString());
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
        public FinansineInformacija GautiPagalId(String AkcijosKodas)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
