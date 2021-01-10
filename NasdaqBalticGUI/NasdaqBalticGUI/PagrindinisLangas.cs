using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NasdaqBalticGUI
{
    public partial class PagrindinisLangas : Form
    {
        Vartotojas DabartinisVarotojas;
        PriekiautiLogika PriekiautiLangas = new PriekiautiLogika();
        AtidarytosIrUzdarytosPozicijosLogika UzsakymuLogika = new AtidarytosIrUzdarytosPozicijosLogika();
        List<Akcijos> DabartinesPirkimoAkcijos = new List<Akcijos>();
        List<VartotojoAkcija> VartotojoAtidarytosPozicijos = new List<VartotojoAkcija>();
        List<Dictionary<string, string>> CustomLaukaiPozicijom = new List<Dictionary<string, string>>();
        List<Dictionary<string, string>> CustomLaukaiUzdarytomPozicijom = new List<Dictionary<string, string>>();
        List<VartotojoAkcija> VartotojoUzdarytosPozicijos = new List<VartotojoAkcija>();
        Akcijos PasirinktaAkcija = new Akcijos();
        VartotojoAkcija AtidarytaVartotojoAkcija = new VartotojoAkcija();
        bool PauseAtnaujinima = false;
        int selected = 0;
        int ListviewPlotisSumazinus = 1070;
        int ListviewPlotisPilnas = 1352;
        public PagrindinisLangas(Vartotojas vartotojas)
        {
            InitializeComponent();
            DabartinisVarotojas = vartotojas;
            DabartinesPirkimoAkcijos = PriekiautiLangas.GautiVisasAkcijas();
            UzsakymuLogika.GautiAtidarytasIrUzdarytasPozicijas(DabartinisVarotojas.Id, DabartinesPirkimoAkcijos, out VartotojoAtidarytosPozicijos,out CustomLaukaiPozicijom, out VartotojoUzdarytosPozicijos, out CustomLaukaiUzdarytomPozicijom);
            NustatytiAntrastes(DabartinisVarotojas.Balansas, VartotojoAtidarytosPozicijos);
            Thread thread = new Thread(() => AtnaujintiReiksmesTimer());
            thread.IsBackground = true;
            thread.Start();
        }

        private void AtnaujintiReiksmesTimer(bool Amzinai = true)
        {
            int SKSenu = 0;
            do
            {
                if (selected > 0 && !PauseAtnaujinima)
                {
                    if (listView1.InvokeRequired)
                    {
                        switch (selected)
                        {
                            case 1:
                                SKSenu = DabartinesPirkimoAkcijos.Count;
                                DabartinesPirkimoAkcijos = PriekiautiLangas.GautiVisasAkcijas();
                                if (SKSenu == DabartinesPirkimoAkcijos.Count)
                                    listView1.Invoke(new MethodInvoker(delegate { AtnaujintiReiksmes(PriekiautiLangas.LentelesStulpeliaiMapping, DabartinesPirkimoAkcijos); }));
                                else
                                    listView1.Invoke(new MethodInvoker(delegate { Prekyba(null, null); }));
                                UzsakymuLogika.GautiAtidarytasIrUzdarytasPozicijas(DabartinisVarotojas.Id, DabartinesPirkimoAkcijos, out VartotojoAtidarytosPozicijos, out CustomLaukaiPozicijom, out VartotojoUzdarytosPozicijos, out CustomLaukaiUzdarytomPozicijom);
                                break;
                            case 2:
                                SKSenu = VartotojoAtidarytosPozicijos.Count;
                                UzsakymuLogika.GautiAtidarytasIrUzdarytasPozicijas(DabartinisVarotojas.Id, DabartinesPirkimoAkcijos, out VartotojoAtidarytosPozicijos, out CustomLaukaiPozicijom, out VartotojoUzdarytosPozicijos, out CustomLaukaiUzdarytomPozicijom);
                                if (SKSenu == VartotojoAtidarytosPozicijos.Count)
                                    listView1.Invoke(new MethodInvoker(delegate { AtnaujintiReiksmes(UzsakymuLogika.AtidarytuAkcijuUzsakymuLentelesStulpeliaiMapping, VartotojoAtidarytosPozicijos, CustomLaukaiPozicijom); }));
                                listView1.Invoke(new MethodInvoker(delegate { AtidarytosPozicijos(null, null); }));
                                DabartinesPirkimoAkcijos = PriekiautiLangas.GautiVisasAkcijas();
                                break;
                            case 3:
                                SKSenu = VartotojoUzdarytosPozicijos.Count;
                                UzsakymuLogika.GautiAtidarytasIrUzdarytasPozicijas(DabartinisVarotojas.Id, DabartinesPirkimoAkcijos, out VartotojoAtidarytosPozicijos, out CustomLaukaiPozicijom, out VartotojoUzdarytosPozicijos, out CustomLaukaiUzdarytomPozicijom);
                                if (SKSenu == VartotojoUzdarytosPozicijos.Count)
                                    listView1.Invoke(new MethodInvoker(delegate { AtnaujintiReiksmes(UzsakymuLogika.UzdarytuAkcijuUzsakymuLentelesStulpeliaiMapping, VartotojoUzdarytosPozicijos, CustomLaukaiUzdarytomPozicijom); }));
                                listView1.Invoke(new MethodInvoker(delegate { UzdarytosPozicijos(null, null); }));
                                DabartinesPirkimoAkcijos = PriekiautiLangas.GautiVisasAkcijas();
                                break;
                        }
                    }
                }
                if (!String.IsNullOrEmpty(PaieskosLaukas.Text) && !PaieskosLaukas.Text.Equals("Paieška"))
                {
                    listView1.Invoke(new MethodInvoker(delegate {
                        Paieska(null, null);
                    }));
                }
                Thread.Sleep(60000);
            }while(Amzinai);
        }
        public void AtnaujintiReiksmes<T>(List<Tuple<string, string, int>> Mappingai, List<T> Duomenys, List<Dictionary<string,string>> CustomLaukai = null)
        {
                foreach (ListViewItem Item in listView1.Items)
                {
                    int i = listView1.Items.IndexOf(Item);
                    string Pavadinimas = Item.SubItems[0].Text;
                    T akcija = (T)Activator.CreateInstance(typeof(T));
                    if (typeof(T) == typeof(Akcijos))
                    {
                    List<Akcijos> Temp = (List<Akcijos>)Convert.ChangeType(Duomenys, typeof(List<Akcijos>));
                    akcija = (T)Convert.ChangeType(Temp.Find(x => x.Pavadinimas.Equals(Pavadinimas)), typeof(T)) ;
                    }
                if (typeof(T) == typeof(VartotojoAkcija))
                {
                    List<VartotojoAkcija> Temp = (List<VartotojoAkcija>)Convert.ChangeType(Duomenys, typeof(List<VartotojoAkcija>));
                    akcija = (T)Convert.ChangeType(Temp.Find(x => x.akcija.Pavadinimas.Equals(Pavadinimas)), typeof(T));
                }

                    if (akcija != null)
                    {
                        foreach (ListViewItem.ListViewSubItem langelis in Item.SubItems)
                        {
                            int j = Item.SubItems.IndexOf(langelis);
                            if (j == 0)
                                continue;
                            String LangelioValue = langelis.Text;
                            String NaujaValue = String.Empty;
                            Object Objektas = GetPropValue(akcija, Mappingai[j].Item2);
                            if (Objektas != null)
                                NaujaValue = Objektas.ToString();
                            else if (CustomLaukai != null && CustomLaukai.Count > 0)
                            {
                                CustomLaukai[i].TryGetValue(Mappingai[j].Item2, out NaujaValue);
                            }

                            if (!String.IsNullOrEmpty(NaujaValue) && NaujaValue.Equals("True")) NaujaValue = "Pirkimas";
                            if (!String.IsNullOrEmpty(NaujaValue) && NaujaValue.Equals("False")) NaujaValue = "Pardavimas";

                            if (!String.IsNullOrEmpty(LangelioValue) && !String.IsNullOrEmpty(LangelioValue) && !LangelioValue.Equals(NaujaValue))
                            {
                                langelis.Text = NaujaValue;
                            }

                        }

                    }
                }
            NustatytiAntrastes(DabartinisVarotojas.Balansas, VartotojoAtidarytosPozicijos);
            SpalvuNustatymasPokyciams(Mappingai);
            AtnaujintiData();


        }

        void NustatytiAntrastes(Double VartotojoBalansas, List<VartotojoAkcija> AkcijosVartotojo)
        {
            DabartinisVarotojas = PriekiautiLangas.GautiVartotoja(DabartinisVarotojas.Id);
            Balansas.Text = VartotojoBalansas + " €";
            double portfolioVerte;
            double PandN = UzsakymuLogika.GautiPelnaNuostoli(AkcijosVartotojo, out portfolioVerte);
            if (PandN > 0) PN.ForeColor = Color.Green;
            if (PandN < 0) PN.ForeColor = Color.Red;

            PN.Text = PandN.ToString("0.##") + " €";
            Kapitalas.Text = VartotojoBalansas + PandN + " €";
            VerteValue.Text = portfolioVerte.ToString("0.##") + " €";
        } 
        void AtnaujintiData()
        {
            if (!AtnaujintaData.Visible)
                AtnaujintaData.Visible = true;
            DateTime Dabar = DateTime.Now;
            String Data = Dabar.ToString("yyyy/dd/MM HH:mm");
            AtnaujintaData.Text = Data;
        }

        void Prekyba(object sender, EventArgs e)
        {
            Prekiauti.Width = 1346;
            selected = 1;
            AkcijosInformacija.Visible = false;
            pictureBox6.Visible = false;
            label10.Text = "Akcijų Prekyba";
            label10.Visible = true;
            uzdaryta.Visible = !PriekiautiLangas.ArDirbaAkcijuBirza(DabartinesPirkimoAkcijos);
            InicijuotiAtvaizdzioPakeitima(PriekiautiLangas.LentelesStulpeliaiMapping, DabartinesPirkimoAkcijos);
        }

        void InicijuotiAtvaizdzioPakeitima<T>(List<Tuple<string,string,int>> Mappingas, List<T> Duomenys, List<Dictionary<string, string>> customMapping = null)
        {
            KurtiLentele(Mappingas);
            SupildytiDuomenis<T>(Duomenys, Mappingas, customMapping);
            NustatytiDynaminiusStulpeliuDydzius(Mappingas);
            SpalvuNustatymasPokyciams(Mappingas);
            AtnaujintiData();
        }
        private void SpalvuNustatymasPokyciams(List<Tuple<string, string, int>> lentelesStulpeliaiMapping)
        {
            List<int> PokycioNr = new List<int>();
            foreach (ColumnHeader header in listView1.Columns)
            {
                if (header.Text.Contains("+/-") || header.Text.Contains("Pokytis") || header.Text.Contains("P&N"))
                {
                    PokycioNr.Add(listView1.Columns.IndexOf(header));
                }
            }
            ListViewItem[] VisiLentelesItemai = new ListViewItem[listView1.Items.Count];
            listView1.Items.CopyTo(VisiLentelesItemai, 0);
            foreach (ListViewItem item in listView1.Items)
            {
                item.UseItemStyleForSubItems = false;
                foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                {
                    int i = item.SubItems.IndexOf(subItem);
                    if (PokycioNr.Contains(i))
                    {
                        Double value;
                        Double.TryParse(subItem.Text.Replace("€",""), out value);
                        if (value == 0)
                        {
                            subItem.ForeColor = SystemColors.GrayText;
                        }
                        else if (value > 0)
                        {
                            subItem.ForeColor = Color.Green;
                        }
                        else if (value < 0)
                        {
                            subItem.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }
        }

        private void SupildytiDuomenis<T>(List<T> Duomenys, List<Tuple<string, string, int>> mapping, List<Dictionary<string, string>> customMapping = null )
        {
            listView1.Items.Clear();
            foreach (T VienasIrasas in Duomenys)
            {
                int i = Duomenys.IndexOf(VienasIrasas);
                List<string> row = new List<string>();
                foreach (Tuple<string, string, int> item in mapping)
                {
                    String propertyValue = String.Empty;
                    try
                    {
                        Object PropValueObject = GetPropValue(VienasIrasas, item.Item2);
                        if (PropValueObject != null)
                        propertyValue = GetPropValue(VienasIrasas, item.Item2).ToString().Trim();
                        else
                        {
                            if (customMapping != null)
                            {
                                String value;
                                customMapping[i].TryGetValue(item.Item2, out value);
                                if (!String.IsNullOrEmpty(value))
                                    propertyValue = value.Trim();
                            }
                        }
                    }
                    catch (Exception e)
                    { }
                    if (!String.IsNullOrEmpty(propertyValue))
                    {
                        if (propertyValue.Equals("True"))
                            propertyValue = "Pirkimas";
                        if (propertyValue.Equals("False"))
                            propertyValue = "Pardavimas";

                        row.Add(propertyValue);
                    }
                    else
                        row.Add(" ");
                }
                var listViewItem = new ListViewItem(row.ToArray());
                listView1.Items.Add(listViewItem);
            }
        }

        private void KurtiLentele(List<Tuple<string, string,int>> lentelesStulpeliaiMapping)
        {
            List<string> Columns = new List<string>();
            foreach(Tuple<string, string, int>  item in lentelesStulpeliaiMapping)
            {
                Columns.Add(item.Item1);
            }
            KurtiLentele(Columns);
        }
        private void KurtiLentele(List<string> Stulpeliai)
        {
            if (listView1.Columns.Count > 0)
                listView1.Columns.Clear();

            foreach (string columnName in Stulpeliai)
            {
                ColumnHeader stulpelis = new ColumnHeader();
                stulpelis.Text = columnName;
                stulpelis.TextAlign = HorizontalAlignment.Center;
                listView1.Columns.Add(stulpelis);
            }

            listView1.Visible = true;
        }
        void NustatytiDynaminiusStulpeliuDydzius(List<Tuple<string, string, int>> lentelesStulpeliaiMapping)
        {
            foreach (ColumnHeader header in listView1.Columns)
            {
                int i = listView1.Columns.IndexOf(header);
                header.Width = lentelesStulpeliaiMapping[i].Item3;
            }
        }
        void AtidarytosPozicijos(object sender, EventArgs e)
        {
            Prekiauti.Width = ListviewPlotisPilnas;
            selected = 2;
            PauseAtnaujinima = false;
            AkcijosInformacija.Visible = false;
            pictureBox6.Visible = false;
            label10.Text = "Atidarytos Pozicijos";
            label10.Visible = true;

            InicijuotiAtvaizdzioPakeitima(UzsakymuLogika.AtidarytuAkcijuUzsakymuLentelesStulpeliaiMapping, VartotojoAtidarytosPozicijos, CustomLaukaiPozicijom);

        }
        void UzdarytosPozicijos(object sender, EventArgs e)
        {
            Prekiauti.Width = ListviewPlotisPilnas;
            selected = 3;
            PauseAtnaujinima = false;
            AkcijosInformacija.Visible = false;
            pictureBox6.Visible = false;
            label10.Text = "Uzdarytos Pozicijos";
            label10.Visible = true;

            InicijuotiAtvaizdzioPakeitima(UzsakymuLogika.UzdarytuAkcijuUzsakymuLentelesStulpeliaiMapping, VartotojoUzdarytosPozicijos, CustomLaukaiUzdarytomPozicijom);
        }
        void Paieska(object sender, EventArgs e)
        {
            string Paieska = PaieskosLaukas.Text;
            if(!Paieska.Equals("Paieška"))
                switch (selected)
                {
                    case 1:
                        List<Akcijos> Akcijos = new List<Akcijos>();
                        DabartinesPirkimoAkcijos.ForEach(x => { if (x.Pavadinimas.IndexOf(Paieska, 0, StringComparison.OrdinalIgnoreCase) >= 0) Akcijos.Add(x);  });
                        InicijuotiAtvaizdzioPakeitima(PriekiautiLangas.LentelesStulpeliaiMapping, Akcijos);
                        break;
                    case 2:
                        List<VartotojoAkcija> vartotojoAkcijas = new List<VartotojoAkcija>();
                        VartotojoAtidarytosPozicijos.ForEach(x => { if (x.akcija.Pavadinimas.IndexOf(Paieska, 0, StringComparison.OrdinalIgnoreCase) >= 0) vartotojoAkcijas.Add(x); });
                        InicijuotiAtvaizdzioPakeitima(UzsakymuLogika.AtidarytuAkcijuUzsakymuLentelesStulpeliaiMapping, vartotojoAkcijas, CustomLaukaiPozicijom);
                        break;
                    case 3:
                        List<VartotojoAkcija> vartotojoAkcijos = new List<VartotojoAkcija>();
                        VartotojoUzdarytosPozicijos.ForEach(x => { if (x.akcija.Pavadinimas.IndexOf(Paieska, 0, StringComparison.OrdinalIgnoreCase) >= 0) vartotojoAkcijos.Add(x); });
                        InicijuotiAtvaizdzioPakeitima(UzsakymuLogika.UzdarytuAkcijuUzsakymuLentelesStulpeliaiMapping, vartotojoAkcijos, CustomLaukaiUzdarytomPozicijom);
                        break;
                }
            
        }
        private void PagrindinisLangas_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void LogOut_Click(object sender, EventArgs e)
        {

        }
        public object GetPropValue(object src, string propName)
        {
            Type tipas = src.GetType();
            PropertyInfo propertyInfo;

                if (propName.Contains("FinansineInformacija."))
                {
                    propName = propName.Replace("FinansineInformacija.","");
                    src = GetPropValue(src,"finansineInformacija"); // finansine informacija
                    tipas = src.GetType();
                }
                else if (propName.Contains("PapildomaInformacija"))
                {
                    propName = propName.Replace("PapildomaInformacija.", "");
                    src = GetPropValue(src, "papildomaInformacija");
                    tipas = src.GetType();
                }
                else if (propName.Contains("Akcija"))
                {
                    propName = propName.Replace("Akcija.", "");
                    src = GetPropValue(src, "akcija");
                    tipas = src.GetType();
                }
                else if (propName.Contains("Vartotojas"))
                {
                    propName = propName.Replace("Vartotojas.", "");
                    src = GetPropValue(src, "vartotojas");
                    tipas = src.GetType();
                }
            
            propertyInfo = tipas.GetProperty(propName);
            if (propertyInfo != null && src != null)
                return propertyInfo.GetValue(src, null);
            else
                return null;
        }

        private void PaieskosLaukas_Enter(object sender, EventArgs e)
        {
            string DabartineValue = ((TextBox)sender).Text;
            if (DabartineValue.Contains("Paieška"))
            {
                ((TextBox)sender).Text = String.Empty;
            }
        }

        private void PaieskosLaukas_Leave(object sender, EventArgs e)
        {
            string DabartineValue = ((TextBox)sender).Text;
            if (String.IsNullOrEmpty(DabartineValue))
            {
                ((TextBox)sender).Text = "Paieška";
            }
        }
        private void Pirkti_Click(object sender, EventArgs e)
        {
            if(!PirktiKiekis.Visible)
            {
                PirktiKiekis.Visible = true;
            }
            else if (PirktiKiekis.Value == 0)
            {
                PirktiKiekis.Visible = false;
                PirkimoKiekisKaina.Visible = false;
            }
            else if (PirktiKiekis.Value > 0)
            {
                SutikimasIrUzsakymoDarymas((int)PirktiKiekis.Value, PasirinktaAkcija.finansineInformacija.PirkimoKaina, PasirinktaAkcija, true, DabartinisVarotojas);
               
            }
            
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem PasirinktasElementas = listView1.SelectedItems[0];
            if (selected == 1)
            {
                Akcijos PasirinktaAkcija = DabartinesPirkimoAkcijos.Find(x => x.Pavadinimas.Trim().Equals(PasirinktasElementas.SubItems[0].Text));
                if (PasirinktaAkcija != null)
                {
                    AtidarytiInformacijosLanga(PasirinktaAkcija, PriekiautiLangas.ArDirbaAkcijuBirza(DabartinesPirkimoAkcijos));
                }
            }
            if (selected == 2)
            {
                PauseAtnaujinima = true;
                VartotojoAkcija PasirinktaVartotojoAkcija = VartotojoAtidarytosPozicijos.Find(x => x.akcija.Pavadinimas.Trim().Equals(PasirinktasElementas.SubItems[0].Text) && x.AtidarymoData.ToString().Equals(PasirinktasElementas.SubItems[PasirinktasElementas.SubItems.Count-1].Text));
                if (PasirinktaVartotojoAkcija != null)
                {
                    AtidarytaVartotojoAkcija = PasirinktaVartotojoAkcija;
                    AtidarytiUzsakymoLanga(true, PasirinktaVartotojoAkcija);
                }
            }
            if (selected == 3)
            {
                PauseAtnaujinima = true;
                String Trukme = PasirinktasElementas.SubItems[PasirinktasElementas.SubItems.Count - 2].Text;
                String[] Padalintas = Trukme.Split('-');
                String PradziosData = Padalintas[0].Trim();
                String PabaigosData = Padalintas[1].Trim();
                VartotojoAkcija PasirinktaVartotojoAkcija = VartotojoUzdarytosPozicijos.Find(x => x.akcija.Pavadinimas.Trim().Equals(PasirinktasElementas.SubItems[0].Text) && x.AtidarymoData.ToString().Equals(PradziosData) && x.UzdarymoData.ToString().Equals(PabaigosData));
                if (PasirinktaVartotojoAkcija != null)
                {
                    AtidarytaVartotojoAkcija = PasirinktaVartotojoAkcija;
                    AtidarytiUzsakymoLanga(false, PasirinktaVartotojoAkcija);
                }
            }
        }

        private void AtidarytiUzsakymoLanga(bool Aktyvus, VartotojoAkcija pasirinktaVartotojoAkcija)
        {
            InfoUzsakymo.Visible = true;
            InfoUzsakymo.BringToFront();
            Prekiauti.Width = ListviewPlotisSumazinus;
            string Pelnas = String.Empty;
            string DabartinisPelnas = String.Empty;
            if (pasirinktaVartotojoAkcija.Pirkimas)
            {
                PirkimasPardavimas.Text = "Pirkimas";
                DabartinisPelnas = (pasirinktaVartotojoAkcija.akcija.finansineInformacija.PirkimoKaina * pasirinktaVartotojoAkcija.Kiekis - pasirinktaVartotojoAkcija.PirkimoKaina * pasirinktaVartotojoAkcija.Kiekis).ToString("0.##") + " €";
                Pelnas = (pasirinktaVartotojoAkcija.UzdarymoKaina * pasirinktaVartotojoAkcija.Kiekis - pasirinktaVartotojoAkcija.PirkimoKaina * pasirinktaVartotojoAkcija.Kiekis).ToString("0.##") + " €";
            }
            else
            { 
                Pelnas = (pasirinktaVartotojoAkcija.PirkimoKaina * pasirinktaVartotojoAkcija.Kiekis - pasirinktaVartotojoAkcija.UzdarymoKaina * pasirinktaVartotojoAkcija.Kiekis).ToString("0.##") + " €";
                DabartinisPelnas = (pasirinktaVartotojoAkcija.PirkimoKaina * pasirinktaVartotojoAkcija.Kiekis - pasirinktaVartotojoAkcija.akcija.finansineInformacija.PardavimoKaina * pasirinktaVartotojoAkcija.Kiekis).ToString("0.##") + " €";
                PirkimasPardavimas.Text = "Pardavimas";
            }

            Priezastis.Text = "Priežastis: " + pasirinktaVartotojoAkcija.Priezastis;
            PelnasGautas.Text = Pelnas;
            if (Aktyvus)
            PelnasAndN.Text = DabartinisPelnas;
            else
                PelnasAndN.Text = Pelnas;

            Suma.Text = pasirinktaVartotojoAkcija.Kiekis.ToString();
            AVerte.Text =(pasirinktaVartotojoAkcija.PirkimoKaina*pasirinktaVartotojoAkcija.Kiekis).ToString() + " €";
            UVerte.Text =(pasirinktaVartotojoAkcija.UzdarymoKaina * pasirinktaVartotojoAkcija.Kiekis).ToString() + " €";

            AKaina.Text = pasirinktaVartotojoAkcija.PirkimoKaina.ToString() + " €";
            Ukaina.Text = pasirinktaVartotojoAkcija.UzdarymoKaina.ToString() + " €";

            ALaikas.Text = pasirinktaVartotojoAkcija.AtidarymoData.ToString("yyyy-MM-dd HH:mm");
            ULaikas.Text = pasirinktaVartotojoAkcija.UzdarymoData.ToString("yyyy-MM-dd HH:mm");

            UID.Text = pasirinktaVartotojoAkcija.Id.ToString();

            Pavadinimas.Text = pasirinktaVartotojoAkcija.akcija.Pavadinimas;


            if (Aktyvus)
            {
                Priezastis.Visible = false;
                PelnasGautas.Visible = false;
                label14.Visible = false;
                UVerte.Visible = false;
                label18.Visible = false;
                Ukaina.Visible = false;
                label15.Visible = false;
                ULaikas.Visible = false;
                uzdaryti.Visible = true;

            }
            else
            {
                Priezastis.Visible = true;
                PelnasGautas.Visible = true;
                label14.Visible = true;
                UVerte.Visible = true;
                label18.Visible = true;
                Ukaina.Visible = true;
                label15.Visible = true;
                ULaikas.Visible = true;
                uzdaryti.Visible = false;
            }


        }
        void AtidarytiInformacijosLanga(Akcijos Akcija, bool BirzaAtidaryta)
        {
            PasirinktaAkcija = Akcija;
            PavadinimasImones.Text = Akcija.Pavadinimas;
            AkcijosKodas.Text = Akcija.AkcijosKodas;

            AkcijosInformacija.BringToFront();
            AkcijosInformacija.Visible = true;
            PirktiKiekis.Visible = BirzaAtidaryta;
            ParduotiKiekis.Visible = BirzaAtidaryta;
            PardavimoKiekisKaina.Visible = BirzaAtidaryta;
            PirkimoKiekisKaina.Visible = BirzaAtidaryta;


            DataInformacijos.Text = Akcija.finansineInformacija.Timestamp.ToString("yyyy-MM-dd HH:mm");

            PaskKaina.Text = Akcija.finansineInformacija.Paskutine_Kaina.ToString() + " EUR";
            Pokytis.Text = Akcija.finansineInformacija.PokytisProcentais.ToString() + "%";
            PrkKaina.Text = Akcija.finansineInformacija.PirkimoKaina.ToString();
            PrdKaina.Text = Akcija.finansineInformacija.PardavimoKaina.ToString();

            DidzKaina.Text = Akcija.papildomaInformacija.Didziausia_Kaina.ToString();
            MaziKaina.Text = Akcija.papildomaInformacija.Maziausia_Kaina.ToString();
            VidutKaina.Text = Akcija.papildomaInformacija.Vidutine_Kaina.ToString();
            AtidayrmoKaina.Text = Akcija.papildomaInformacija.Atidarymo_Kaina.ToString();

            Sando.Text = Akcija.finansineInformacija.Sandoriai.ToString();
            Kiek.Text = Akcija.finansineInformacija.Kiekis.ToString();
            Apyv.Text = Akcija.finansineInformacija.Apyvarta.ToString();

            VaddybaKontentas.Text = SutvarkytiSimbolius(Akcija.papildomaInformacija.Vadyba);
            ApieKontent.Text = SutvarkytiSimbolius(Akcija.papildomaInformacija.Apie);
            KontaktaiKontent.Text = SutvarkytiSimbolius(Akcija.papildomaInformacija.Kontaktai);

            if (Akcija.papildomaInformacija.Kalba.Equals("lt"))
            {
                Apie.Text = "Apie";
                Vadyba.Text = "Vadyba";
                Kontaktai.Text = "Kontaktai";
            }

            if (Akcija.papildomaInformacija.Kalba.Equals("en"))
            {
                Apie.Text = "About";
                Vadyba.Text = "Management Board";
                Kontaktai.Text = "Contacts";
            }


            Ataskaita.Links.Clear();
            Ataskaita.Links.Add(0, 0, Akcija.papildomaInformacija.AtaskaitosURL);

        }
        private void Ataskaita_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Ataskaita.LinkVisited = true;
            LinkLabel Dabartinis = (LinkLabel)sender;
            // Navigate to a URL.
            System.Diagnostics.Process.Start("https:" + Dabartinis.Links[0].LinkData.ToString());
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            this.Ataskaita.LinkVisited = true;
            // Navigate to a URL.
            System.Diagnostics.Process.Start("https:" + Ataskaita.Links[0].LinkData.ToString());
        }
        private string SutvarkytiSimbolius (String input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                input = input.Replace("bdquo;", "„");
                input = input.Replace("ldquo;", "”");
                input = input.Replace("scaron;", "š");
                input = input.Replace("Scaron;", "Š");
                input = input.Replace("ndash;", "–");
                input = input.Replace("rdquo;", "”");
                input = input.Replace("nbsp;", " ");
            }
            return input;
        }

        private void PirktiKiekis_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numericUp = (NumericUpDown)sender;
            if (numericUp.Value>0)
            {
                Double Value = Double.Parse(numericUp.Value.ToString());
                PirkimoKiekisKaina.Text = $"{PasirinktaAkcija.finansineInformacija.PirkimoKaina} * {numericUp.Value} = € {PasirinktaAkcija.finansineInformacija.PirkimoKaina * Value} ";
                PirkimoKiekisKaina.Visible = true;
            }
            else
            {
                PirkimoKiekisKaina.Visible = false;

            }
        }

        private void ParduotiKiekis_ValueChanged(object sender, EventArgs e)
        {
             NumericUpDown numericUp = (NumericUpDown)sender;
            if (numericUp.Value > 0)
            {
                Double Value = Double.Parse(numericUp.Value.ToString());
                PardavimoKiekisKaina.Text = $"{PasirinktaAkcija.finansineInformacija.PardavimoKaina} * {numericUp.Value} = € {PasirinktaAkcija.finansineInformacija.PardavimoKaina * Value} ";
                PardavimoKiekisKaina.Visible = true;
            }
            else
            {
                PardavimoKiekisKaina.Visible = false;

            }
        }

        private void Parduoti_Click(object sender, EventArgs e)
        {
            if (!ParduotiKiekis.Visible)
            {
                ParduotiKiekis.Visible = true;
            }
            else if (ParduotiKiekis.Value == 0)
            {
                ParduotiKiekis.Visible = false;
                PardavimoKiekisKaina.Visible = false;
            }
            else if (ParduotiKiekis.Value > 0)
            {
                SutikimasIrUzsakymoDarymas((int)ParduotiKiekis.Value, PasirinktaAkcija.finansineInformacija.PardavimoKaina, PasirinktaAkcija, false, DabartinisVarotojas);
            }
        }

       void SutikimasIrUzsakymoDarymas(int kiekis, double Kaina, Akcijos akcija, bool Pirkimas, Vartotojas Pirkejas)
       {
            double PilnaKaina = kiekis * Kaina;
            if (Kaina <= Pirkejas.Balansas)
            {
                String Tekstas = String.Empty;
                if (Pirkimas)
                Tekstas = $"Ar tikrai norite pirkimas {akcija.Pavadinimas} {Kaina} akcijas už  {PilnaKaina} € ?";
                else
                Tekstas = $"Ar tikrai norite pardavimas {akcija.Pavadinimas} {Kaina} akcijas už  {PilnaKaina} € ?";
                DialogResult dialogResult = MessageBox.Show(Tekstas, "Ar tikrai?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    if (UzsakymuLogika.KurtiUzsakyma(Pirkimas, akcija, Pirkejas, kiekis))
                    {
                        ParduotiKiekis.Value = 0;
                        ParduotiKiekis.Visible = false;
                        MessageBox.Show("Sekmingai nupirkta");
                        UzsakymuLogika.GautiAtidarytasIrUzdarytasPozicijas(DabartinisVarotojas.Id, DabartinesPirkimoAkcijos, out VartotojoAtidarytosPozicijos, out CustomLaukaiPozicijom, out VartotojoUzdarytosPozicijos, out CustomLaukaiUzdarytomPozicijom);
                        
                    }
                    else
                        MessageBox.Show("Nepavyko nupirkti");
                }
            }
            else
            {
                MessageBox.Show("Neuztenka Pinigu");
            }
       }


        private void pictureBox5_Click(object sender, EventArgs e)
        {
            InfoUzsakymo.Visible = false;
            PauseAtnaujinima = false;
            Prekiauti.Width = ListviewPlotisPilnas;
        }

        private void uzdaryti_Click(object sender, EventArgs e)
        {
           if(AtidarytaVartotojoAkcija != null)
            {
                bool result = UzsakymuLogika.UzdarytiAkcija(AtidarytaVartotojoAkcija, DabartinisVarotojas, DabartinesPirkimoAkcijos);
                if (result)
                {
                    MessageBox.Show("Sekmingai uždaryta");
                    InfoUzsakymo.Visible = false;
                    PauseAtnaujinima = false;
                    Prekiauti.Width = ListviewPlotisPilnas;
                    UzsakymuLogika.GautiAtidarytasIrUzdarytasPozicijas(DabartinisVarotojas.Id, DabartinesPirkimoAkcijos, out VartotojoAtidarytosPozicijos, out CustomLaukaiPozicijom, out VartotojoUzdarytosPozicijos, out CustomLaukaiUzdarytomPozicijom);
                    InicijuotiAtvaizdzioPakeitima(UzsakymuLogika.AtidarytuAkcijuUzsakymuLentelesStulpeliaiMapping, VartotojoAtidarytosPozicijos, CustomLaukaiPozicijom);
                    PauseAtnaujinima = false;
                }
                else
                {
                    MessageBox.Show("Nepavyko uždaryti");
                }
            }
        }
    }
}
