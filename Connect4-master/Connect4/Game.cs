using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connect4
{
    class Move
    {
        public int move;
        public int score;

        public Move(int m, int s)
        {
            move = m;
            score = s;
        }
    }
    [Serializable]
    class Game
    {
        public const int PLAYER = 1;
        public const int COMPUTER = 2;
        public const int DRAW = 3;
        public readonly int N;
        public readonly int M;
        public readonly int maxDepth;
        public HashSet<String> increment;
        public HashSet<String> decrement;
        public Dictionary<string, int> visited;
        public int[,] board;
        private int panelWidth;
        private int panelHeight;
        private int circleWidth;
        private int circleHeight;
        public int Radius { get; set;  }
        Stack<int> undo;
        Color player1;
        Color player2;

        public Game(int N, int M, int maxDepth, int panelHeight, int panelWidth, Color p1, Color p2)
        {
            player1 = p1;
            player2 = p2;
            increment = new HashSet<String>();
            decrement = new HashSet<String>();
            visited = new Dictionary<string, int>();
            System.IO.StreamReader incFile = new System.IO.StreamReader("increment.txt");
            System.IO.StreamReader decFile = new System.IO.StreamReader("decrement.txt");
            string line;
            while ((line = incFile.ReadLine()) != null)
            {
                increment.Add(line);
            }
            while ((line = decFile.ReadLine()) != null)
            {
                decrement.Add(line);
            }
            board = new int[N, M];
            this.N = N;
            this.M = M;
            this.maxDepth = maxDepth;
            resize(panelHeight, panelWidth);
            undo = new Stack<int>();
        }

        public void resize(int panelHeight, int panelWidth)
        {
            this.panelHeight = panelHeight;
            this.panelWidth = panelWidth;
            circleWidth = panelWidth / M;
            circleHeight = panelHeight / N;
            Radius = Math.Min(circleWidth, circleHeight) / 2;
            Radius -= 5;
        }

        public void printBoard(Graphics g)
        {
            Pen pen = new Pen(Color.Black, 2);
            g.Clear(Color.Blue);
            for (int i = 1; i <= M - 1; ++i)
            {
                g.DrawLine(pen, new Point(i * circleWidth, 0), new Point(i * circleWidth, panelHeight));
            }
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < M; ++j)
                {
                    Color color = Color.White;
                    if (board[i, j] == 1)
                        color = player1;
                    if (board[i, j] == 2)
                        color = player2;
                    Brush brush = new SolidBrush(color);
                    int x = (j * circleWidth) + (circleWidth / 2) - Radius;
                    int y = (i * circleHeight) + (circleHeight / 2) - Radius;
                    g.FillEllipse(brush, x, y, 2 * Radius, 2 * Radius);
                    brush.Dispose();
                }
            }
            pen.Dispose();
        }

        public bool checkDraw()
        {
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < M; ++j)
                {
                    if (board[i, j] == 0) return false;
                }
            }
            return true;
        }

        // proveruva dali ima pobednik vo momentalnata sostojba i go vrakja pobednikot ako ima
        public int checkWinner()
        {
            if (checkDraw()) return 3;

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j <= M - 4; j++)
                {
                    int t = board[i, j];
                    if (t != 0 && board[i, j + 1] == t && board[i, j + 2] == t && board[i, j + 3] == t)
                    {
                        return t;
                    }
                }
            }

            for (int i = 0; i <= N - 4; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    int t = board[i, j];
                    if (t != 0 && board[i + 1, j] == t && board[i + 2, j] == t && board[i + 3, j] == t)
                    {
                        return t;
                    }
                }
            }

            for (int i = 0; i <= N - 4; i++)
            {
                for (int j = 0; j <= M - 4; j++)
                {
                    int t = board[i, j];
                    if (t != 0 && board[i + 1, j + 1] == t && board[i + 2, j + 2] == t && board[i + 3, j + 3] == t)
                    {
                        return t;
                    }
                }
            }

            for (int i = 0; i <= N - 4; i++)
            {
                for (int j = 3; j < M; j++)
                {
                    int t = board[i, j];
                    if (t != 0 && board[i + 1, j - 1] == t && board[i + 2, j - 2] == t && board[i + 3, j - 3] == t)
                    {
                        return t;
                    }
                }
            }
            return -1;
        }

        public int checkWinner(int[,] board)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j <= M - 4; j++)
                {
                    int t = board[i, j];
                    if (t != 0 && board[i, j + 1] == t && board[i, j + 2] == t && board[i, j + 3] == t)
                    {
                        return t;
                    }
                }
            }

            for (int i = 0; i <= N - 4; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    int t = board[i, j];
                    if (t != 0 && board[i + 1, j] == t && board[i + 2, j] == t && board[i + 3, j] == t)
                    {
                        return t;
                    }
                }
            }

            for (int i = 0; i <= N - 4; i++)
            {
                for (int j = 0; j <= M - 4; j++)
                {
                    int t = board[i, j];
                    if (t != 0 && board[i + 1, j + 1] == t && board[i + 2, j + 2] == t && board[i + 3, j + 3] == t)
                    {
                        return t;
                    }
                }
            }

            for (int i = 0; i <= N - 4; i++)
            {
                for (int j = 3; j < M; j++)
                {
                    int t = board[i, j];
                    if (t != 0 && board[i + 1, j - 1] == t && board[i + 2, j - 2] == t && board[i + 3, j - 3] == t)
                    {
                        return t;
                    }
                }
            }
            return -1;

        }






        // DEBUGGING
        public void printBoard(int[,] board)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    Console.Write(String.Format("{0} ", board[i, j]));
                }
                Console.WriteLine();
            }
        }


        
        public bool playerMove(Point p)
        {
            int choice = p.X / circleWidth;
            if(!validMoves(board).Contains(choice))
                return false;
            Console.WriteLine("Choice: " + choice); //DEBUGGING!
            // Treba da ja prima kolonata sto ja kliknal igracot vo "choice" i ostanata logika
            // moze da ostane
            for (int i = N - 1; i >= 0; i--)
            {
                if (board[i, choice] == 0)
                {
                    board[i, choice] = 1;
                    break;
                }
            }
            undo.Push(choice);
            return true;
        }
        public void computerMove()
        {
          // Ova moze da ostane nepromeneto ama da se povikuva posle sekoj poteg na igracot
            Move m = minimax(board, 0, 2, int.MinValue, int.MaxValue);
            
            if (m.move != -1 && m.move != -2)
            {
                for (int i = N - 1; i >= 0; i--)
                {
                    if (board[i, m.move] == 0)
                    {
                        board[i, m.move] = 2;
                        break;
                    }
                }
                undo.Push(m.move);
            }
        }



        // gi vrakja kolonite sto ne se polni 
        private List<int> validMoves(int[,] board)
        {
            List<int> moves = new List<int>();
            for (int i = 0; i < M; i++)
            {
                if (board[0, i] == 0) moves.Add(i);
            }
            return moves;
        }


        // ja prima momentalnata sostojba, koj igrac go napravil potegot i vo koja kolona
        // i go boi prvoto prazno mesto vo taa kolona
        private int[,] applyMove(int[,] board, int column, int player)
        {
            int[,] newBoard = new int[N, M];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    newBoard[i, j] = board[i, j];
                }
            }
            for (int i = N - 1; i >= 0; i--)
            {
                if (newBoard[i, column] == 0)
                {
                    newBoard[i, column] = player;
                    break;
                }
            }
            return newBoard;
        }

        //Minimax so alfa beta kastrenje
        public Move minimax(int[,] board, int depth, int player, int a, int b)
        {
            int alpha = a;
            int beta = b;
            int winner = checkWinner(board);
            if (depth == maxDepth)
            {
                return new Move(-1, getHeuristic(board));

            }
            else if (winner != -1)
            {
                return new Move(-1, winner == 1 ? -1 * int.MaxValue : int.MaxValue);
            }
            else
            {

                List<int> moves = validMoves(board);


                if (moves.Count == 0)
                {
                    return new Move(-2, getHeuristic(board));
                }
                else
                {

                    //min node
                    if (player == 1)
                    {
                        int best_score = int.MaxValue, best_move = -1;
                        for (int i = 0; i < moves.Count; i++)
                        {
                            int[,] newBoard = applyMove(board, moves[i], player);
                            Move m = minimax(newBoard, depth + 1, 2, alpha, beta);
                            if (m.score <= best_score)
                            {
                                best_score = m.score;
                                best_move = moves.ElementAt(i);
                            }
                            beta = Math.Min(beta, best_score);
                            if (beta < alpha) break;
                        }


                        //Console.WriteLine(String.Format("Depth: {0} Pleyer: {1} Move: {2} Score: {3}", depth, player, best_move, best_score));
                        //printBoard(board);

                        return new Move(best_move, best_score);
                    }
                    //max node
                    else
                    {
                        int best_score = int.MinValue, best_move = -1;
                        for (int i = 0; i < moves.Count; i++)
                        {
                            int[,] newBoard = applyMove(board, moves.ElementAt(i), player);
                            Move m = minimax(newBoard, depth + 1, 1, alpha, beta);
                            if (m.score >= best_score)
                            {
                                best_score = m.score;
                                best_move = moves.ElementAt(i);
                            }
                            alpha = Math.Max(alpha, best_score);
                            if (beta < alpha) break;
                        }

                        //Console.WriteLine(String.Format("Depth: {0} Pleyer: {1} Move: {2} Score: {3}", depth, player, best_move, best_score));
                        //printBoard(board);

                        return new Move(best_move, best_score);
                    }

                }

            }

        }


        // Funkcija za evaluacija
        public int getHeuristic(int[,] board)
        {
            int winner = checkWinner(board);
            if (winner != -1)
            {
                int m = winner == 1 ? -1 : 1;
                return m * int.MaxValue;
            }
            int count = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j <= M - 4; j++)
                {
                    string s = String.Format("{0}{1}{2}{3}", board[i, j], board[i, j + 1], board[i, j + 2], board[i, j + 3]);
                    if (increment.Contains(s))
                    {
                        count++;
                    }
                    else if (decrement.Contains(s))
                    {
                        count--;
                    }
                }
            }
            for (int i = 0; i <= N - 4; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    string s = String.Format("{0}{1}{2}{3}", board[i, j], board[i + 1, j], board[i + 2, j], board[i + 3, j]);
                    if (increment.Contains(s))
                    {
                        count++;
                    }
                    else if (decrement.Contains(s))
                    {
                        count--;
                    }
                }
            }
            for (int i = 0; i <= N - 4; i++)
            {
                for (int j = 0; j <= M - 4; j++)
                {
                    string s = String.Format("{0}{1}{2}{3}", board[i, j], board[i + 1, j + 1], board[i + 2, j + 2], board[i + 3, j + 3]);
                    if (increment.Contains(s))
                    {
                        count++;
                    }
                    else if (decrement.Contains(s))
                    {
                        count--;
                    }
                }
            }

            for (int i = 0; i <= N - 4; i++)
            {
                for (int j = 3; j < M; j++)
                {
                    string s = String.Format("{0}{1}{2}{3}", board[i, j], board[i + 1, j - 1], board[i + 2, j - 2], board[i + 3, j - 3]);
                    if (increment.Contains(s))
                    {
                        count++;
                    }
                    else if (decrement.Contains(s))
                    {
                        count--;
                    }
                }
            }
            return count;
        }

        public void undoMove()
        {
            if(undo.Count == 0)
            {
                return;
            }
            int p = undo.Pop();
            for(int i =0;i< N; i++)
            {
                if(board[i, p] != 0)
                {
                    board[i, p] = 0;
                    break;
                }
            }
            int c = undo.Pop();
            for (int i = 0; i < N; i++)
            {
                if (board[i, c] != 0)
                {
                    board[i, c] = 0;
                    break;
                }
            }
        }

    }
}
