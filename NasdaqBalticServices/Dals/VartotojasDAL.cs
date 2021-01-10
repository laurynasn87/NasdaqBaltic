using Database;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DALs
{
    public class VartotojasDAL
    {

        SQLCommands sQLCommands;
        String DefaultDatabaseConn = "Database";
        const string VartotojuTablePavadinimas = "users";
        public VartotojasDAL()
        {
            sQLCommands = new Database.SQLCommands(DefaultDatabaseConn);
        }
        public VartotojasDAL(string DatbaseConnectionName)
        {
            sQLCommands = new Database.SQLCommands(DatbaseConnectionName);
        }
        public List<Vartotojas> GautiVisus()
        {
            List<Vartotojas> Vartotojai = new List<Vartotojas>();
            List<List<Tuple<string, string>>> result = sQLCommands.GetAll(VartotojuTablePavadinimas);

            foreach (List<Tuple<string, string>> vienasVartotojas in result)
            {
                if (vienasVartotojas.Count > 0)
                {
                    Vartotojai.Add(new Vartotojas().ListToVartotojas(vienasVartotojas));

                }

            }

            return Vartotojai;
        }
        public Vartotojas GautiPagalId(String Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                Vartotojas rezultatas = new Vartotojas();
                List<List<Tuple<string, string>>> result = sQLCommands.GetByCondition(VartotojuTablePavadinimas, new List<Tuple<string, string>>() { new Tuple<string, string>("Id", Id) }, 1);
                foreach (List<Tuple<string, string>> vienasVartotojas in result)
                {
                    if (vienasVartotojas.Count > 0 && rezultatas.Id == 0)
                    {
                        rezultatas = rezultatas.ListToVartotojas(vienasVartotojas);
                    }

                }
                return rezultatas;
            }
            return null;
        }

        public bool Atnaujinti(Vartotojas vartotojas)
        {
            List<Tuple<string, string>> tuples = vartotojas.PaverstVartotojaITupleList();

            return sQLCommands.Update(tuples, VartotojuTablePavadinimas, "Id", vartotojas.Id.ToString());
        }
        public bool AtnaujintiBalansa(Vartotojas vartotojas)
        {
            List<Tuple<string, string>> tuples = vartotojas.PaverstVartotojaITupleList(new List<string>() { "Vardas", "Slaptazodis", "Created_on"});

            return sQLCommands.Update(tuples, VartotojuTablePavadinimas, "Id", vartotojas.Id.ToString());
        }

    }
}
