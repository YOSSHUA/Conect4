using con4;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connect4
{
    public partial class Form1 : Form
    {        
        private Graphics g;
        Solver solver;
        //Game game;
        public string FileName { get; set; }        

        public Form1( int difficulty, Color p1, Color p2)
        {
            InitializeComponent();
            solver = new Solver(difficulty, panel1.Height, panel1.Width, p1, p2);
            //game = new Game(difficulty, panel1.Height, panel1.Width, p1, p2);            
            g = panel1.CreateGraphics();
            FileName = "Untitled";            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            solver.printBoard(g);
            //game.printBoard(g);            
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            int play = solver.playerMove(e.Location);
            if (play == 0 || play == 1 || play == 2 || play == 3) {
                solver.printBoard(); // Debug
                //game.printBoard(game.board); // DEBUGGING
                Invalidate(true);                
                if (play == 1)
                {                    
                    MessageBox.Show("Player wins!", "Winner!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                    return;
                }
                else if (play == 2)
                {                    
                    MessageBox.Show("Computer wins!", "Winner!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                    return;
                }
                else if (play == 3)
                {                    
                    MessageBox.Show("The board is full!", "Full!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                    return;
                }
                play = solver.applyMove(1);
                //game.computerMove();
                solver.printBoard();
                //game.printBoard(game.board); // DEBUGGING
                Invalidate(true);
                if (play == 1)
                {                    
                    MessageBox.Show("Player wins!", "Winner!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                    return;
                }
                else if (play == 2)
                {                    
                    MessageBox.Show("Computer wins!", "Winner!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                    return;
                }
                else if (play == 3)
                {
                    MessageBox.Show("The board is full!", "Full!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                    return;
                }

            }
            else
            {
                MessageBox.Show("Invalid move! Make another choice!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (solver != null)
            {
                panel1.Size = new Size(this.Size.Width - 36, this.Size.Height - 70);
                g = panel1.CreateGraphics();
                solver.resize(panel1.Height, panel1.Width);
                Invalidate(true);
            }
        }
      

        


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
