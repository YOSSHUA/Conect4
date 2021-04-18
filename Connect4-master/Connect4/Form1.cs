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
        Game game;
        public string FileName { get; set; }
        int time;

        public Form1(int N , int M, int difficulty, Color p1, Color p2)
        {
            InitializeComponent();
            game = new Game(N, M, difficulty, panel1.Height, panel1.Width, p1, p2);
            DoubleBuffered = true;
            g = panel1.CreateGraphics();
            FileName = "Untitled";
            time = 30;
            timer1.Start();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            game.printBoard(g);
            Console.WriteLine("INVALIDATED!");
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (game.playerMove(e.Location)) {
                game.printBoard(game.board); // DEBUGGING
                Invalidate(true);
                int winner = game.checkWinner();
                if (winner == Game.PLAYER)
                {
                    timer1.Stop();
                    MessageBox.Show("Player wins!", "Winner!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                    return;
                }
                else if (winner == Game.COMPUTER)
                {
                    timer1.Stop();
                    MessageBox.Show("Computer wins!", "Winner!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                    return;
                }
                else if (winner == Game.DRAW)
                {
                    timer1.Stop();
                    MessageBox.Show("The board is full!", "Full!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                    return;
                }
                game.computerMove();
                game.printBoard(game.board); // DEBUGGING
                Invalidate(true);
                winner = game.checkWinner();
                if (winner == Game.PLAYER)
                {
                    timer1.Stop();
                    MessageBox.Show("Player wins!", "Winner!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                    return;
                }
                else if (winner == Game.COMPUTER)
                {
                    timer1.Stop();
                    MessageBox.Show("Computer wins!", "Winner!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                    return;
                 
                } else if (winner == Game.DRAW)
                {
                    timer1.Stop();
                    MessageBox.Show("The board is full!", "Full!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                    return;
                }
                time = 30;
                textBox1.Text = time.ToString();
            }
            else
            {
                MessageBox.Show("Invalid move! Make another choice!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void saveFile()
        {
            if (FileName == "Untitled")
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Game file (*.game)|*.game";
                saveFileDialog.Title = "Save game";
                saveFileDialog.FileName = FileName;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FileName = saveFileDialog.FileName;
                }
            }
            if (FileName != null)
            {
                using (FileStream fileStream = new FileStream(FileName, FileMode.Create))
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fileStream, game);
                }
            }
        }
        private void openFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Game file (*.game)|*.game";
            openFileDialog.Title = "Open game";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileName = openFileDialog.FileName;
                try
                {
                    using (FileStream fileStream = new FileStream(FileName, FileMode.Open))
                    {
                        IFormatter formater = new BinaryFormatter();
                        game = (Game)formater.Deserialize(fileStream);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not read file: " + FileName);
                    FileName = null;
                    return;
                }
                Invalidate(true);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.undoMove();
            Invalidate(true);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (game != null)
            {
                panel1.Size = new Size(this.Size.Width - 36, this.Size.Height - 70);
                g = panel1.CreateGraphics();
                game.resize(panel1.Height, panel1.Width);
                Invalidate(true);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            time--;
            textBox1.Text = time.ToString();
            if (time == 0)
            {
                timer1.Stop();
                MessageBox.Show("Game Over!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            this.Close();
            this.Dispose();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            Rules rules = new Rules();
            DialogResult dr = rules.ShowDialog();
            if (dr == DialogResult.Cancel)
                timer1.Start();

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartForm startForm = new StartForm();
            this.Close();
            this.Dispose();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileName = "Untitled";
            saveFile();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
