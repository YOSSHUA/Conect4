using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connect4
{
    public partial class StartForm : Form
    {
        Color player1;
        Color player2;
        int difficulty;
        public StartForm()
        {
            InitializeComponent();
            player1 = Color.Yellow;
            player2 = Color.Red;
            button1.BackColor = player1;
            button2.BackColor = player2;
            difficulty = 3;
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (p1colorDialog.ShowDialog() == DialogResult.OK)
            {
                player1 = p1colorDialog.Color;
                button1.BackColor = p1colorDialog.Color;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (p2colorDialog.ShowDialog() == DialogResult.OK)
            {
                player2 = p2colorDialog.Color;
                button2.BackColor = p2colorDialog.Color;
            }
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1( difficulty, player1, player2);
            this.Visible = false;
            DialogResult dr= form.ShowDialog();
            if(dr == DialogResult.Cancel)
            {
                this.Visible = true;
            }
            if (dr == DialogResult.Abort)
                Close();
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Rules rules = new Rules();
            this.Visible = false;
            DialogResult dr = rules.ShowDialog();
            if (dr == DialogResult.Cancel)
            {
                this.Visible = true;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == 0)
            {
                difficulty = 3;
            } else
            {
                difficulty = 6;
            }
        }

        private void StartForm_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
