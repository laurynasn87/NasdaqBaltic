using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NasdaqBalticGUI
{
    public partial class PagrindinisLangas : Form
    {
        Vartotojas DabartinisVarotojas;
        public PagrindinisLangas(Vartotojas vartotojas)
        {
            InitializeComponent();
            DabartinisVarotojas = vartotojas;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
        }

        private void Search_Enter(object sender, EventArgs e)
        {
            string DabartineValue = ((TextBox)sender).Text;
            if (DabartineValue.Contains("Paieška"))
            {
                ((TextBox)sender).Text = String.Empty;
            }
        }

        private void Search_Leave(object sender, EventArgs e)
        {
            string DabartineValue = ((TextBox)sender).Text;
            if (String.IsNullOrEmpty(DabartineValue))
            {
                ((TextBox)sender).Text = "Paieška";
            }
        }

        private void PagrindinisLangas_Load(object sender, EventArgs e)
        {

        }

        private void Prekiavimas_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Prekyba();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Prekyba();
        }
        void Prekyba()
        {

        }
        void AtidarytosPozicijos()
        {

        }
        void UzdarytosPozicijos()
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            AtidarytosPozicijos();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            AtidarytosPozicijos();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            UzdarytosPozicijos();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            UzdarytosPozicijos();
        }

        private void Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            Paieska();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Paieska();
        }

        void Paieska()
        {

        }
    }
}
