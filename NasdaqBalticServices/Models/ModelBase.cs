using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Models
{
    public class ModelBase
    {
        protected List<Tuple<string, string>> KintamujuMapping = new List<Tuple<string, string>>();
        
        public ModelBase()
        {
        }

        protected void SetObjectProperty(string propertyName, string value, object obj)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
            var convertedValue = new object();
            // make sure object has the property we are after
            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                var converter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
                if (!propertyInfo.PropertyType.Name.Contains("String"))
                {
                    convertedValue = converter.ConvertFromString(value);
                }
                else
                {
                    convertedValue = value;
                }

                propertyInfo.SetValue(obj, convertedValue, null);

            }
        }
        protected T ListToObject<T>(List<Tuple<string, string>> vienasElementas)
        {
            T Objetkas = (T)Activator.CreateInstance(typeof(T));
            foreach (Tuple<string, string> kintamasisi in vienasElementas)
            {
                if (!String.IsNullOrEmpty(kintamasisi.Item1) && !String.IsNullOrEmpty(kintamasisi.Item2))
                {
                    Tuple<string, string> Mapping = KintamujuMapping.Find(x => x.Item1.Equals(kintamasisi.Item1));
                    if (Mapping != null)
                    {
                        SetObjectProperty(Mapping.Item2, kintamasisi.Item2, Objetkas);
                    }
                        
                }
            }
            return Objetkas;
        }
        protected List<Tuple<string, string>> PaverstObjektaITupleList(Object Objektas, List<string> IgnoravimoSar = null)
        {
            List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();
            foreach (PropertyInfo prop in Objektas.GetType().GetProperties())
            {
                if (prop.Name.Equals("Id"))
                    continue;
                if (IgnoravimoSar != null && IgnoravimoSar.Count > 0 && IgnoravimoSar.Contains(prop.Name))
                    continue;
                try
                {
                    string pav = prop.Name;
                    var value = prop.GetValue(Objektas);
                    if (!String.IsNullOrEmpty(pav) && value != null)
                    {
                        if (prop.PropertyType == typeof(DateTime))
                        {
                            DateTime date = (DateTime)value;
                            if (date.Year == 1) value = DateTime.Now;
                            value = date.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        pav = this.KonvertuotiDbVardaIObjektoVarda(pav, false);
                        tuples.Add(new Tuple<string, string>(pav, value.ToString()));
                    }
                }
                catch(System.Reflection.TargetException err)
                {

                }
            }
            return tuples;
        }

        public string KonvertuotiDbVardaIObjektoVarda(string vardas, bool DbVardasIObjektoVarda = true)
        {
            Tuple<string, string> Mapping;
            string result = String.Empty;
            if (DbVardasIObjektoVarda)
            {
                Mapping = KintamujuMapping.Find(x => x.Item1.Equals(vardas));
                if (Mapping != null)
                {
                    result = Mapping.Item2;
                }
            }
            else
            {
                Mapping = KintamujuMapping.Find(x => x.Item2.Equals(vardas));
                if (Mapping != null)
                {
                    result = Mapping.Item1;
                }
            }
            return result;
        }
        public bool ArIdentiski(Object v1, Object v2, List<string> IgnoreList = null)
        {
            foreach (PropertyInfo prop in v1.GetType().GetProperties())
            {
                if (IgnoreList != null && IgnoreList.Count > 0)
                {
                    if (IgnoreList.Contains(prop.Name))
                        continue;
                }

                var v1Value = prop.GetValue(v1);
                var v2Value = prop.GetValue(v2);
                if (v1Value != v2Value)
                {
                    return false;
                }

            }
            return true;
        }
    }
}
