using DALs;
using HtmlAgilityPack;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace FoniniaiProcesai
{
   public class InformacijosGavimoFunkcijos
    {
        const string RootUrl = "https://nasdaqbaltic.com";
        const string MainUrl = RootUrl + "/statistics/lt/shares";
        List<Akcijos> visosAkcijos = new List<Akcijos>();
        AkcijuDAL dAL = new AkcijuDAL();
        PapildomosInformacijosDAL papildomosInformacijosDal = new PapildomosInformacijosDAL();
        FinansinesInformacijosDAL finansinesInfDal = new FinansinesInformacijosDAL();
        readonly List<Tuple<string, string>> EiluciuMapping = new List<Tuple<string, string>>()
        {
            new Tuple<string, string>("Bendrovė","Pavadinimas"),
            new Tuple<string, string>("Trumpinys","AkcijosKodas"),
            new Tuple<string, string>("Pask.kaina €","finansineInformacija.Paskutine_Kaina"),
            new Tuple<string, string>("+/-","finansineInformacija.Pokytis"),
            new Tuple<string, string>("%","finansineInformacija.PokytisProcentais"),
            new Tuple<string, string>("Prk. €","finansineInformacija.PirkimoKaina"),
            new Tuple<string, string>("Prd. €","finansineInformacija.PardavimoKaina"),
            new Tuple<string, string>("Sand.","finansineInformacija.Sandoriai"),
            new Tuple<string, string>("Kiekis","finansineInformacija.Kiekis"),
            new Tuple<string, string>("Apyvarta €","finansineInformacija.Apyvarta"),
            new Tuple<string, string>("Didžiausia kaina","papildomaInformacija.Didziausia_Kaina"),
            new Tuple<string, string>("Mažiausia kaina","papildomaInformacija.Maziausia_Kaina"),
            new Tuple<string, string>("Vid.","papildomaInformacija.Vidutine_Kaina"),
            new Tuple<string, string>("Atidarymo kaina","papildomaInformacija.Atidarymo_Kaina"),
            new Tuple<string, string>("FI","papildomaInformacija.AtaskaitosURL")
        };
        List<Tuple<string, string>> PapildomosInfoUrl = new List<Tuple<string, string>>();
        public InformacijosGavimoFunkcijos()
        {
        }
       public void NuskaitytiInformacija(bool AtnaujintiAkcijas = false, bool AtnaujintiFinansineInformacija = false, bool AtnaujintiPapildomaInformacija = false)
        {
            visosAkcijos = GautiAkcijuSarasa();
            if (AtnaujintiAkcijas)
            {
                IrasytiNaujasAkcijas(visosAkcijos);
            }
            if (AtnaujintiFinansineInformacija)
            {
                IrasytiNaujaFinansineInformacija(visosAkcijos);
            }
            if (AtnaujintiPapildomaInformacija)
            {
                List<PapildomaInformacija> papildomaInformacija = GautiPapildomaInformacija();
                PapildomosInformacijosAtnaujinimas(papildomaInformacija);
            }
        }
        public void PapildomosInformacijosAtnaujinimas(List<PapildomaInformacija> NaujPapildomaInformacija)
        {
            if (NaujPapildomaInformacija == null && NaujPapildomaInformacija.Count < 1)
                return;

            List<PapildomaInformacija> DbPapildomaInformacija = papildomosInformacijosDal.GautiVisus();
            List<PapildomaInformacija> AtnaujintiPapildomaInformacija = new List<PapildomaInformacija>();
            List<PapildomaInformacija> SukurtiPapildomaInformacija = new List<PapildomaInformacija>();

                foreach (PapildomaInformacija papildomainf in NaujPapildomaInformacija)
                {
                    PapildomaInformacija DbInformacija = DbPapildomaInformacija.Find(x => papildomainf.AkcijosKodas.Equals(x.AkcijosKodas));
                    if (DbInformacija != null)
                    {
                    
                        if (!DbInformacija.ArIdentiski(DbInformacija, papildomainf, new List<string>() {"Id"}))
                        {
                            papildomainf.Id = DbInformacija.Id;
                            AtnaujintiPapildomaInformacija.Add(papildomainf);
                        }
                           
                    }
                    else
                    {
                        SukurtiPapildomaInformacija.Add(papildomainf);
                    }
                }
            

            foreach (Akcijos akcijos in visosAkcijos)
            {
                PapildomaInformacija papildomaInformacija = NaujPapildomaInformacija.Find(x => akcijos.AkcijosKodas.Equals(x.AkcijosKodas));
                if (papildomaInformacija != null) akcijos.papildomaInformacija = papildomaInformacija;
            }

            foreach (PapildomaInformacija papildomainf in SukurtiPapildomaInformacija)
            {
                papildomosInformacijosDal.Ivesti(papildomainf);
            }
            foreach (PapildomaInformacija papildomainf in AtnaujintiPapildomaInformacija)
            {
                papildomosInformacijosDal.Atnaujinti(papildomainf);
            }

        }

        public List<PapildomaInformacija> GautiPapildomaInformacija()
        {
            string informacijosDivKlase = "tradinginfo-numbers";
            string informacijosSubDivKlase = "smaller-nrs";
            string imonesAprasymoDivKlase = "mobile-content-accordion";

            string[] VadybaSekcijosPavadinimas = { "Valdyba", "Management Board", "Management Board", "Management Board", "Management Board" };
            string[] ApieSekcijosPavadinimas = { "Trumpai apie bendrovę", "Background Information", "Company Description", "Brief overview of the enterprise", "About" };
            string[] KontaktaiSekcijosPavadinimas = { "Kontaktai", "Contact Details", "Contacts", "Contact information", "Contacts" };
            string[] Kalbos = {"lt", "en", "en", "en"};

            List<PapildomaInformacija> NaujiIrasai = new List<PapildomaInformacija>();
            if (PapildomosInfoUrl == null || PapildomosInfoUrl.Count < 1)
                visosAkcijos = GautiAkcijuSarasa();

            foreach (Tuple<String,String> Urls in PapildomosInfoUrl)
            {
                PapildomaInformacija papildomaInformacija = new PapildomaInformacija();
                Akcijos akcija = visosAkcijos.Find(x => String.Equals(x.Pavadinimas,Urls.Item1, StringComparison.OrdinalIgnoreCase));

                if (akcija == null)
                    continue;

                papildomaInformacija.AkcijosKodas = akcija.AkcijosKodas;

                string HTMLKodas = GautiHTML(Urls.Item2);

                if (!string.IsNullOrEmpty(HTMLKodas))
                {
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(HTMLKodas);
                    HtmlNodeCollection InfoDiv = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, '" + informacijosDivKlase + "')]");
                    if (InfoDiv[0].InnerHtml != null)
                    {
                        HtmlNode InfoSubDiv = InfoDiv[0].SelectSingleNode(InfoDiv[0].XPath + "//div[contains(@class, '" + informacijosSubDivKlase + "')]");
                        if (InfoSubDiv.InnerHtml != null)
                        {
                            foreach (HtmlNode ul in InfoSubDiv.SelectNodes(InfoSubDiv.XPath + "//ul"))
                            {
                                foreach (HtmlNode li in ul.SelectNodes( ul.XPath + "//li"))
                                {
                                    string name = li.InnerText;
                                    string value = li.SelectSingleNode( li.XPath + "//span").InnerText;
                                    if (!String.IsNullOrEmpty(value))
                                    {
                                        name = name.Replace(value, "");
                                    }
                                    Tuple<string, string> mapping = EiluciuMapping.Find(x => string.Equals(x.Item1, name));
                                    if (mapping != null)
                                    {
                                        name = mapping.Item2.Replace("papildomaInformacija.", "");
                                        SetObjectProperty(name,value,papildomaInformacija);
                                    }

                                }
                            }
                        }
                    }

                }
                HTMLKodas = GautiHTML(Urls.Item2.Replace("trading", "company"));
                if (!string.IsNullOrEmpty(HTMLKodas))
                {
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(HTMLKodas);
                    HtmlNode InfoDiv = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, '" + imonesAprasymoDivKlase + "')]");
                    if (InfoDiv.InnerHtml != null)
                    {
                        HtmlNodeCollection Tekstai = InfoDiv.SelectNodes("//p");
                        foreach (HtmlNode tekstoNode in Tekstai)
                        {
                            string vadybaName = String.Empty;
                            string ApieName = String.Empty;
                            string KontaktaiName = String.Empty;

                            for (int i = 0; i < Kalbos.Length; i++)
                            {
                                if (!String.IsNullOrEmpty(papildomaInformacija.Vadyba) && !String.IsNullOrEmpty(papildomaInformacija.Apie) && !String.IsNullOrEmpty(papildomaInformacija.Kontaktai))
                                    break;
                                String VisasTekstas = PasalintiNereikalingusSimbolius(HttpUtility.HtmlDecode(tekstoNode.InnerText));
                                int ApieIndex = VisasTekstas.IndexOf(ApieSekcijosPavadinimas[i], StringComparison.OrdinalIgnoreCase);
                                int KontaktaiIndex = VisasTekstas.IndexOf(KontaktaiSekcijosPavadinimas[i], StringComparison.OrdinalIgnoreCase);
                                int VadybaIndex = VisasTekstas.IndexOf(VadybaSekcijosPavadinimas[i], StringComparison.OrdinalIgnoreCase);

                                if (ApieIndex>=0 || KontaktaiIndex>=0 || VadybaIndex>=0)
                                {
                                    papildomaInformacija.Kalba = Kalbos[i];
                                    if (ApieIndex > 0 && KontaktaiIndex > 0 && VadybaIndex > 0)
                                    {
                                    papildomaInformacija.Vadyba = VisasTekstas.Substring(0, VisasTekstas.Length - ApieIndex).Replace(VadybaSekcijosPavadinimas[i], "");
                                    papildomaInformacija.Apie = VisasTekstas.Substring(ApieIndex, KontaktaiIndex - ApieIndex).Replace(ApieSekcijosPavadinimas[i], "");
                                    papildomaInformacija.Kontaktai = VisasTekstas.Substring(KontaktaiIndex).Replace(KontaktaiSekcijosPavadinimas[i], "");

                                    }
                                    else
                                    {

                                        if (String.IsNullOrEmpty(papildomaInformacija.Vadyba) && VadybaIndex >= 0)
                                        {
                                            papildomaInformacija.Vadyba = VisasTekstas.Substring(VadybaIndex);
                                            vadybaName = VadybaSekcijosPavadinimas[i];
                                        }
                                        if (String.IsNullOrEmpty(papildomaInformacija.Apie) && ApieIndex >= 0)
                                        {
                                            papildomaInformacija.Apie = VisasTekstas.Substring(ApieIndex);
                                            ApieName = ApieSekcijosPavadinimas[i];
                                        }
                                        if (String.IsNullOrEmpty(papildomaInformacija.Kontaktai) && KontaktaiIndex >= 0)
                                        {
                                            papildomaInformacija.Kontaktai = VisasTekstas.Substring(KontaktaiIndex);
                                            KontaktaiName = KontaktaiSekcijosPavadinimas[i];

                                        }
                                    }

                                }

                            }
                            string tempVadyba = papildomaInformacija.Vadyba;
                            string tempApie = papildomaInformacija.Apie;
                            string tempKontaktai = papildomaInformacija.Kontaktai;
                            if (!String.IsNullOrEmpty(papildomaInformacija.Apie))
                            {
                                papildomaInformacija.Apie = IstrintiPapInformacijaKitosPapInformacijos(papildomaInformacija.Apie, ApieName, new string[] { tempVadyba, tempKontaktai });
                                papildomaInformacija.Apie= Regex.Replace(papildomaInformacija.Apie, ApieName, "", RegexOptions.IgnoreCase);

                                       if (!String.IsNullOrEmpty(papildomaInformacija.Apie) && papildomaInformacija.Apie[0] == ':')
                                            papildomaInformacija.Apie = papildomaInformacija.Apie.Remove(0, 1);
                            }
                            if (!String.IsNullOrEmpty(papildomaInformacija.Vadyba)) 
                            {
                                papildomaInformacija.Vadyba = IstrintiPapInformacijaKitosPapInformacijos(papildomaInformacija.Vadyba,vadybaName, new string[] { tempApie, tempKontaktai });
                                papildomaInformacija.Vadyba= Regex.Replace(papildomaInformacija.Vadyba, vadybaName, "", RegexOptions.IgnoreCase);

                                  if (!String.IsNullOrEmpty(papildomaInformacija.Vadyba) && papildomaInformacija.Vadyba[0] == ':')
                                      papildomaInformacija.Vadyba = papildomaInformacija.Vadyba.Remove(0, 1);
                            }
                            if (!String.IsNullOrEmpty(papildomaInformacija.Kontaktai))
                            {
                                papildomaInformacija.Kontaktai = IstrintiPapInformacijaKitosPapInformacijos(papildomaInformacija.Kontaktai, KontaktaiName, new string[] { tempApie, tempVadyba });
                                papildomaInformacija.Kontaktai = Regex.Replace(papildomaInformacija.Kontaktai, KontaktaiName, "", RegexOptions.IgnoreCase);
                                    
                                if (!String.IsNullOrEmpty(papildomaInformacija.Kontaktai) && papildomaInformacija.Kontaktai[0] == ':')
                                    papildomaInformacija.Kontaktai = papildomaInformacija.Kontaktai.Remove(0, 1);
                            }
                        }

                    }
                }
                papildomaInformacija.AtaskaitosURL = akcija.papildomaInformacija.AtaskaitosURL;
                NaujiIrasai.Add(papildomaInformacija);

            }

            return NaujiIrasai;
        }

     public void IrasytiNaujasAkcijas(List<Akcijos> NuskaitytosAkcijos)
        {
            if (NuskaitytosAkcijos == null || NuskaitytosAkcijos.Count < 1)
                return;

            List<Akcijos> DBazesAkcijos = dAL.GautiVisus();
            List<Akcijos> NaujosAkcijos = new List<Akcijos>();
            List<Akcijos> AtnaujintosAkcijos = new List<Akcijos>();
                foreach (Akcijos akcija in NuskaitytosAkcijos)
                {
                    Akcijos arYraAkcijaDb = DBazesAkcijos.Find(x => x.AkcijosKodas == akcija.AkcijosKodas);
                    if (arYraAkcijaDb != null)
                    {
                        if (!arYraAkcijaDb.Pavadinimas.Equals(akcija.Pavadinimas))
                        {
                            AtnaujintosAkcijos.Add(akcija);
                        }
                    }
                    else
                    {
                        NaujosAkcijos.Add(akcija);
                    }
                }
            foreach (Akcijos akcija in DBazesAkcijos)
            {
                Akcijos arYraAkcija = NuskaitytosAkcijos.Find(x => x.AkcijosKodas == akcija.AkcijosKodas);
                if (arYraAkcija == null)
                    dAL.Istrinti(akcija);
            }
            foreach (Akcijos akcija in NaujosAkcijos)
            {
                dAL.Ivesti(akcija);
            }
            foreach (Akcijos akcija in AtnaujintosAkcijos)
            {
                dAL.Atnaujinti(akcija);
            }
        }
        public void IrasytiNaujaFinansineInformacija(List<Akcijos> NuskaitytosAkcijos)
        {
            if (NuskaitytosAkcijos != null && NuskaitytosAkcijos.Count > 0)
            foreach (Akcijos akcija in NuskaitytosAkcijos)
            {
                if (akcija.finansineInformacija.Apyvarta != 0 && akcija.finansineInformacija.Kiekis != 0 && akcija.finansineInformacija.Sandoriai != 0 && akcija.finansineInformacija.PirkimoKaina != 0 && akcija.finansineInformacija.PardavimoKaina != 0)
                finansinesInfDal.Ivesti(akcija.finansineInformacija);
            }
            
        }
        public List<Akcijos> GautiAkcijuSarasa()
        {
            List<Akcijos> NaujosAkcijos = new List<Akcijos>();
            string HTMLKodas = GautiHTML(MainUrl);
            if (!String.IsNullOrEmpty(HTMLKodas))
            {
                List<HtmlNode> akcijuLenteles = new List<HtmlNode>();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(HTMLKodas);


                HtmlNodeCollection visosLenteles = htmlDoc.DocumentNode.SelectNodes("//table");
                foreach (HtmlNode node in visosLenteles)
                {
                    bool arTuriClass = false;
                    foreach (HtmlAttribute atributai in node.Attributes)
                    {
                        if (atributai.Value.ToString().ToUpper().Contains("tablesaw".ToUpper()))
                            arTuriClass = true;
                    }
                    if (arTuriClass)
                        akcijuLenteles.Add(node);

                }
                foreach (HtmlNode akcijosLentele in akcijuLenteles)
                {
                    List<Tuple<int, string>> eiliskumas;
                    eiliskumas = GautiEiliskuma(akcijosLentele);

                    HtmlNode tBody = akcijosLentele.SelectSingleNode(akcijosLentele.XPath + "//tbody");
                    HtmlNodeCollection eilutes = tBody.SelectNodes(tBody.XPath + "//tr");
                    List<Tuple<string, string>> Reiksmes;
                    foreach (HtmlNode eilute in eilutes)
                    {
                        Reiksmes = new List<Tuple<string, string>>();
                        HtmlNodeCollection stulpeliai = eilute.SelectNodes(eilute.XPath + "//td");
                        foreach (HtmlNode stulpelis in stulpeliai)
                        {
                            int i = stulpeliai.IndexOf(stulpelis);
                            string InnerText = PasalintiNereikalingusSimbolius(stulpelis.InnerText);
                            string InnerHtml = PasalintiNereikalingusSimbolius(stulpelis.InnerHtml);
                            if (!String.IsNullOrEmpty(InnerText) || !String.IsNullOrEmpty(InnerHtml))
                            {
                                if (String.IsNullOrEmpty(InnerText))
                                {
                                    HtmlNode href = stulpelis.SelectSingleNode(stulpelis.XPath + "//a[@href]");
                                    if (href != null)
                                    {
                                        InnerText = href.GetAttributeValue("href", string.Empty);
                                        InnerText = InnerText.Replace("&amp;", "&");
                                        if (InnerText == null) break;
                                    }
                                }
                                Tuple<int, string> surastasEiliskumas = eiliskumas.Find(x => x.Item1 == i);
                                if (surastasEiliskumas != null && !String.IsNullOrEmpty(surastasEiliskumas.Item2))
                                {
                                    InnerText = InnerText.Trim();

                                    if (InnerText.Equals(String.Empty) || InnerText.Replace("-","").Equals(String.Empty)) InnerText = "0";
                                    if (InnerText[InnerText.Length - 1] == 'P' && InnerText[InnerText.Length - 2] == 'L') InnerText = InnerText.Remove(InnerText.Length-2,2);
                                    Reiksmes.Add(new Tuple<string, string>(surastasEiliskumas.Item2, InnerText));

                                    if (surastasEiliskumas.Item2.Equals("Pavadinimas"))
                                    {
                                        HtmlNode href = stulpelis.SelectSingleNode(stulpelis.XPath + "//a[@href]");
                                        if (href != null)
                                        {
                                            InnerHtml = href.GetAttributeValue("href", string.Empty);
                                            InnerHtml = InnerHtml.Replace("&amp;", "&");
                                            PapildomosInfoUrl.Add(new Tuple<string, string>(InnerText, RootUrl + InnerHtml));

                                        }
                                    }
                                }

                            }

                        }
                        Akcijos akcija = ListToAkcija(Reiksmes);
                        if (akcija.AkcijosKodas != String.Empty)
                        {
                            if (akcija.finansineInformacija != null)
                            {
                                akcija.finansineInformacija.AkcijosKodas = akcija.AkcijosKodas;
                            }
                            if (akcija.papildomaInformacija != null)
                            {
                                akcija.papildomaInformacija.AkcijosKodas = akcija.AkcijosKodas;
                            }
                            NaujosAkcijos.Add(akcija);
                        }
                    }
                }
            }
            return NaujosAkcijos;
        }

        public string GautiHTML(string url)
        {
            String code = String.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (String.IsNullOrWhiteSpace(response.CharacterSet))
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.UTF8);

                code = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
            }
            return code;
        }
        private List<Tuple<int, string>> GautiEiliskuma(HtmlNode htmlNode)
        {
            int i = 0;
            List <Tuple<int, string>> tuple = new List<Tuple<int, string>>();
            foreach (HtmlNode stulpeliai in htmlNode.SelectNodes(htmlNode.XPath + "//th"))
            {
                string innerText = stulpeliai.InnerText;
                innerText = PasalintiNereikalingusSimbolius(innerText);
                foreach (Tuple<string,string> mapping in EiluciuMapping)
                {
                    if (innerText.Contains(mapping.Item1))
                    {
                        tuple.Add(new Tuple<int, string>(i, mapping.Item2));
                        
                        break;
                    }
                        
                }
                i++;
            }
            return tuple;
        }
        private string PasalintiNereikalingusSimbolius(String st)
        {
            if (!string.IsNullOrEmpty(st))
            return st.Replace("\n", "").Replace("\r", "").Replace("!", "").Replace("&amp", "&").Replace("quot;", "'").Replace("&nbsp;", " ").Trim();
            return st;
        }
        private Akcijos ListToAkcija(List<Tuple<string, string>> tuples)
        {
            Akcijos akcija = new Akcijos();
            foreach (Tuple<string,string> irasas in tuples)
            {
                if (irasas.Item1.Contains("finansineInformacija"))
                {
                    if (akcija.finansineInformacija == null)
                        akcija.finansineInformacija = new Models.FinansineInformacija();

                    SetObjectProperty(irasas.Item1.Replace("finansineInformacija.",""), irasas.Item2, akcija.finansineInformacija);
                }
                else if(irasas.Item1.Contains("papildomaInformacija"))
                {
                    if (akcija.papildomaInformacija == null)
                        akcija.papildomaInformacija = new Models.PapildomaInformacija();

                    SetObjectProperty(irasas.Item1.Replace("papildomaInformacija.", ""), irasas.Item2, akcija.papildomaInformacija);
                }
                else
                {
                    SetObjectProperty(irasas.Item1, irasas.Item2, akcija);
                }
                
            }

            return akcija;
        }

        private void SetObjectProperty(string propertyName, string value, object obj)
        {
            var convertedValue = new object();
            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
            // make sure object has the property we are after
            if (propertyInfo != null)
            {
                var converter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
                if (String.Equals(propertyInfo.PropertyType.Name, "double", StringComparison.OrdinalIgnoreCase))
                    value = value.Replace(",", ".");

                if (!String.Equals(propertyInfo.PropertyType.Name, "string", StringComparison.OrdinalIgnoreCase))
                {
                    value = value.Replace(" ", "");
                    if (string.IsNullOrEmpty(value)) value = "0";
                    convertedValue = converter.ConvertFromString(value);
                }
                    
                else
                    convertedValue = value;


                propertyInfo.SetValue(obj, convertedValue, null);
            }
        }
        string IstrintiPapInformacijaKitosPapInformacijos(string informacija, string informacijosPav, string[] kitaInformaicja)
        {
            if (string.IsNullOrEmpty(informacija))
                return String.Empty;

            for (int i=0; i<kitaInformaicja.Length; i++)
            {
                if (!String.IsNullOrEmpty(kitaInformaicja[i]) && informacija.Contains(kitaInformaicja[i]))
                {
                    int indexNereikalingaInfo = informacija.IndexOf(kitaInformaicja[i]);
                    int indexReikalingaInfo = informacija.IndexOf(informacijosPav);

                    if (indexReikalingaInfo>=0)
                    {
                        if (indexNereikalingaInfo > indexReikalingaInfo)
                        {
                            informacija = informacija.Remove(indexNereikalingaInfo);
                        }
                        else
                        {
                            informacija = informacija.Substring(indexReikalingaInfo);
                        }
                    }


                }
            }
            return informacija;
        }

    }


}
