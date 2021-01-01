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
                LoginAndRegistracija login = new LoginAndRegistracija(PrisijungimoVardas.Text, Slaptazodis.Text);
                if (login.BandytiPrisijungti())
                {
                    // create form ir paduoti prisjungusi
                }
                else
                {
                    ErrorLabel.Text = "Neteisingi Prisijungimo duomenys";
                    ErrorLabel.Visible = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(PrisijungimoVardas.Text) && !String.IsNullOrEmpty(Slaptazodis.Text))
            {
                LoginAndRegistracija registracija = new LoginAndRegistracija(PrisijungimoVardas.Text, Slaptazodis.Text);
                if (registracija.BandytiRegistruoti())
                {
                    // create form ir paduoti prisjungusi
                }
                else
                {
                    ErrorLabel.Text = "Prisijungimo Vardas uzimtas";
                    ErrorLabel.Visible = true;
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
    }
}
