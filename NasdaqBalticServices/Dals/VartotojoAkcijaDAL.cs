using Database;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
namespace DALs
{
   public class VartotojoAkcijaDAL
    {
        SQLCommands sQLCommands;
        String DefaultDatabaseConn = "Database";
        AkcijuDAL akcijuDal = new AkcijuDAL();
        const string VarotojoAkcijuTablePavadinimas = "vartotojuakcijos";
        public void Dispose()
        {

        }
        public VartotojoAkcijaDAL()
        {
            sQLCommands = new Database.SQLCommands(DefaultDatabaseConn);
        }
        public VartotojoAkcijaDAL(string DatbaseConnectionName)
        {
            sQLCommands = new Database.SQLCommands(DatbaseConnectionName);
        }
        public bool Ivesti(VartotojoAkcija akcija)
        {
            List<Tuple<string, string>> tuples = akcija.PaverstAkcijaITupleList( new List<string>() { "akcija","vartotojas", "AtidarymoData", "UzdarymoData", "Pirkimas" });
            tuples.Add(new Tuple<string, string>("AkcijosKodas", akcija.akcija.AkcijosKodas));
            tuples.Add(new Tuple<string, string>("VartotojoId", akcija.vartotojas.Id.ToString()));
            if (akcija.Pirkimas)
                tuples.Add(new Tuple<string, string>("Pirkimas", "1"));
            else
                tuples.Add(new Tuple<string, string>("Pirkimas", "0"));

            return sQLCommands.Insert(tuples, VarotojoAkcijuTablePavadinimas);
        }

        public List<VartotojoAkcija> GautiVisus()
        {
            List<VartotojoAkcija> akcijos = new List<VartotojoAkcija>();
            List<List<Tuple<string, string>>> result = sQLCommands.GetAll(VarotojoAkcijuTablePavadinimas);

            if (result != null) { 
            foreach (List<Tuple<string, string>> vienaAkcija in result)
            {
                if (vienaAkcija.Count > 0)
                {
                    akcijos.Add(new VartotojoAkcija().ListToVartotojoAkcija(vienaAkcija));
                        akcijos[akcijos.Count - 1] = NustatytiAkcijosObjektiniusKintamiuosius(akcijos[akcijos.Count - 1], vienaAkcija);
                }

            }
            }

            return akcijos;
        }

        public List<VartotojoAkcija> GautiVartotojoAkcijas(int vartotojoId)
        {
            List<VartotojoAkcija> VisosAkcijos = new List<VartotojoAkcija>();
            if (vartotojoId > 0)
            {
                List<VartotojoAkcija> akcijos = new List<VartotojoAkcija>();
                List<List<Tuple<string, string>>> result = sQLCommands.GetByCondition(VarotojoAkcijuTablePavadinimas, new List<Tuple<string, string>>() { new Tuple<string, string>("VartotojoId", vartotojoId.ToString()) });
                if (result != null && result.Count>0)
                foreach (List<Tuple<string, string>> vienaAkcija in result)
                {

                    if (vienaAkcija.Count > 0)
                    {
                        VartotojoAkcija akcija = new VartotojoAkcija();
                        akcija = akcija.ListToVartotojoAkcija(vienaAkcija);
                        if (akcija != null)
                        {
                            akcija = NustatytiAkcijosObjektiniusKintamiuosius(akcija, vienaAkcija);
                            VisosAkcijos.Add(akcija);
                        }
                    }
                }
            }
            return VisosAkcijos;
        }
        VartotojoAkcija NustatytiAkcijosObjektiniusKintamiuosius(VartotojoAkcija akcija, List<Tuple<string, string>> vienaAkcijaList)
        {
            VartotojoAkcija vartotojoAkcija = akcija;

            if (vartotojoAkcija != null)
            {
                string dbakcijaName = new VartotojoAkcija().KonvertuotiDbVardaIObjektoVarda("akcija.AkcijosKodas", false);
                string dbVarotojoName = new VartotojoAkcija().KonvertuotiDbVardaIObjektoVarda("vartotojas.Id", false);
                if (!String.IsNullOrEmpty(dbakcijaName))
                {
                    Tuple<string, string> dbAkcijosValueID = vienaAkcijaList.Find(x => x.Item1.Equals(dbakcijaName));
                    if (dbAkcijosValueID != null)
                    {
                        vartotojoAkcija.akcija = new Akcijos(dbAkcijosValueID.Item2);
                    }
                }
                if (!String.IsNullOrEmpty(dbVarotojoName))
                {
                    Tuple<string, string> dbVartotojoValueID = vienaAkcijaList.Find(x => x.Item1.Equals(dbVarotojoName));
                    if (dbVartotojoValueID != null)
                    {
                        vartotojoAkcija.vartotojas = new Vartotojas(int.Parse(dbVartotojoValueID.Item2));
                    }
                }
            }

            return vartotojoAkcija;
        }

        public bool Atnaujinti(VartotojoAkcija akcija)
        {
            List<Tuple<string, string>> tuples = akcija.PaverstAkcijaITupleList(new List<string>() { "akcija", "vartotojas", "AtidarymoData", "Pirkimas" });
            tuples.Add(new Tuple<string, string>("AkcijosKodas", akcija.akcija.AkcijosKodas));
            tuples.Add(new Tuple<string, string>("VartotojoId", akcija.vartotojas.Id.ToString()));
            if (akcija.Pirkimas)
                tuples.Add(new Tuple<string, string>("Pirkimas", "1"));
            else
                tuples.Add(new Tuple<string, string>("Pirkimas", "0"));

            return sQLCommands.Update(tuples, VarotojoAkcijuTablePavadinimas, "Id", akcija.Id.ToString());
        }
    }
}
