using Database;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DALs
{
   public class LoginIrRegistracijosDAL
    {
        SQLCommands sQLCommands;
        String DefaultDatabaseConn = "Database";
        const string LoginIrRegistracijosTablePavadinimas = "users";
        public LoginIrRegistracijosDAL()
        {
            sQLCommands = new Database.SQLCommands(DefaultDatabaseConn);
        }
        public LoginIrRegistracijosDAL(string DatbaseConnectionName)
        {
            sQLCommands = new Database.SQLCommands(DatbaseConnectionName);
        }
        public bool Ivesti(Vartotojas vartotojas)
        {
            List<string> IgnoreColumns = new List<string>();
            IgnoreColumns.Add("Created_on");
            IgnoreColumns.Add("Id");

            List<Tuple<string, string>> tuples = vartotojas.PaverstVartotojaITupleList(IgnoreColumns);

            return sQLCommands.Insert(tuples, LoginIrRegistracijosTablePavadinimas);
        }
        public bool ArTeisingiLogin(Vartotojas PrisijungimoDuomenys, out Vartotojas gautasVarototjas)
        {
            gautasVarototjas = new Vartotojas();
            if (!String.IsNullOrEmpty(PrisijungimoDuomenys.Vardas) && !String.IsNullOrEmpty(PrisijungimoDuomenys.Slaptazodis))
            {
                List<List<Tuple<string, string>>> result = sQLCommands.GetByCondition(LoginIrRegistracijosTablePavadinimas, new List<Tuple<string, string>>() { new Tuple<string, string>("Vardas", PrisijungimoDuomenys.Vardas), new Tuple<string, string>("Slaptazodis", PrisijungimoDuomenys.Slaptazodis) }, 1);
                Vartotojas rezultatas = new Vartotojas();
                if (result!= null && result.Count>0)
                foreach (List<Tuple<string, string>> vienasVarototjas in result)
                {
                    if (vienasVarototjas.Count > 0 && rezultatas.Id == 0)
                    {
                        rezultatas = rezultatas.ListToVartotojas(vienasVarototjas);
                    }
                }
                if (rezultatas != null && rezultatas.Id != 0)
                {
                    gautasVarototjas = rezultatas;
                    gautasVarototjas.Slaptazodis = String.Empty;
                    return true;
                }
            }
            return false;
        }
    }
}
