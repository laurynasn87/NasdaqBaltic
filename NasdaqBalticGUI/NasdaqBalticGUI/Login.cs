using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NasdaqBalticGUI
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(PrisijungimoVardas.Text) && !String.IsNullOrEmpty(Slaptazodis.Text))
            {
                LoginAndRegistracija login = new LoginAndRegistracija(PrisijungimoVardas.Text, CreateMD5(Slaptazodis.Text));
                Vartotojas dabartinisVartotojas;
                if (login.BandytiPrisijungti(out dabartinisVartotojas) && dabartinisVartotojas != null)
                {
                    PagrindinisLangas pagrindinisLangas = new PagrindinisLangas(dabartinisVartotojas);
                    pagrindinisLangas.Visible = true;

                    this.Visible = false;

                }
                else
                {
                    ErrorLabel.Text = "Neteisingi Prisijungimo duomenys";
                    ErrorLabel.Visible = true;
                    Slaptazodis.Text = String.Empty;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(PrisijungimoVardas.Text) && !String.IsNullOrEmpty(Slaptazodis.Text))
            {
                LoginAndRegistracija registracija = new LoginAndRegistracija(PrisijungimoVardas.Text, CreateMD5(Slaptazodis.Text));
                if (registracija.BandytiRegistruoti())
                {
                    ErrorLabel.Visible = true;
                    PrisijungimoVardas.Text = String.Empty;
                    Slaptazodis.Text = String.Empty;
                }
                else
                {
                    ErrorLabel.Text = "Prisijungimo Vardas uzimtas";
                    ErrorLabel.Visible = true;
                    Slaptazodis.Text = String.Empty;
                }
            }
        }

        private void PrisijungimoVardas_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                button1_Click(null, null);
            }
        }

        private void Slaptazodis_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                button1_Click(null, null);
            }
        }
        public string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
