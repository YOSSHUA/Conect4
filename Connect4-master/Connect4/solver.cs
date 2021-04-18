﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
namespace con4
{
    class Solver
    {
        long[] bitboard;    //Tablero
        int counter = 0;    //Contador para saber quien es el tirador
        int[] height;       //Indica siguiente posición donde puedes poner fichar
        int [] moves;       //Hace movimiento y los guarda
        
        public const int PLAYER = 1;
        public const int COMPUTER = 2;
        public const int DRAW = 3;
        public  int N;
        public  int M;
        public  int maxDepth;
        public HashSet<String> increment;
        public HashSet<String> decrement;
        public Dictionary<string, int> visited;
        public int[,] board;
        private int panelWidth;
        private int panelHeight;
        private int circleWidth;
        private int circleHeight;
        public int Radius { get; set; }
        Stack<int> undo;
        Color player1;
        Color player2;

        public Solver(int N, int M, int maxDepth, int panelHeight, int panelWidth, Color p1, Color p2)
        {
            player1 = p1;
            player2 = p2;                        
            this.N = 6; // Filas
            this.M = 7; // Columnas
            this.maxDepth = maxDepth;
            resize(panelHeight, panelWidth);
            undo = new Stack<int>();
            bitboard = new long[2];
            height = new int[10];
            moves = new int[500];
            player1 = p1;
            player2 = p2;
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
            for (int i = 0; i <  M ; ++i)
            {
                g.DrawLine(pen, new Point(i * circleWidth, 0), new Point(i * circleWidth, panelHeight));
            }
            for (int i = 0; i < N; ++i) // Filas
            {
                for (int j = 0; j < M; ++j) // Columnas
                {
                    //
                    Color color = Color.White;                    
                    if( ( bitboard[0] & (1<< 7*j+i))   == 1 )                    
                        color = player1;
                    if ((bitboard[1] & (1 << 7 * j + i)) == 1)
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


        private void makeMove(int col) //Hacen movimiento
        {
            long move = 1L << height[col]++;
            bitboard[counter & 1] ^= move;
            moves[counter++] = col;
        }
        private void undoMove()//Deshacen movimiento
        {
            int col = moves[--counter];
            long move = 1L << --height[col];
            bitboard[counter & 1] ^= move;
        }

        private Boolean isWin(long bitboard)//Indica si la posición actual es ganadora
        {
            int[] directions = { 1, 7, 6, 8 };
            for (int i = 0; i < directions.Length; i++)
            {
                if ((bitboard & (bitboard >> directions[i]) &
                   (bitboard >> (2 * directions[i])) & (bitboard >> (3 * directions[i]))) != 0)
                    return true;

            }
            return false;
        }

        List<int> validMoves()//Lista de de movimientos validos
        {
            List<int> moves = new List<int>();
            long TOP = 0b1000000_1000000_1000000_1000000_1000000_1000000_1000000L;
            for (int col = 0; col <= 6; col++)
            {
                if ((TOP & (1L << height[col])) == 0)
                    moves.Add(col);

            }
            return moves;
        }
        int evalPosition(long pos)  //
        {
            List<int> nextMov = validMoves();
            int winningMovs = 0;
            foreach(int col in nextMov)
            {
                makeMove(col);
                if(isWin(bitboard[counter & 1]))
                {
                    winningMovs++;
                }
                if (isWin(bitboard[counter ^ 1]))
                {
                    winningMovs--;
                }
                undoMove();                
            }
            return winningMovs;

        }
        int maxVal = 500;
        int bestMov;
        void applyMove(int player, int depth)
        {
            int ans = minimax(-maxVal, maxVal, depth);

        }
        
        
        int minimax(int alpha, int beta, int depth)
        {
            if(isWin(bitboard[counter & 1]))
            {
                if ((counter & 1) == 1)
                    return -maxVal;
                else
                    return maxVal;
            }
            if (depth == 0)
            {
                if ((counter & 1) == 1)
                    return evalPosition(bitboard[counter & 1]);
                else
                    return evalPosition(bitboard[counter & 1]);
            }
            if ((counter & 1) == 0)
            {
                List<int> listMov = validMoves();
                int bestValue = -maxVal;
                int len = listMov.Count;
                for (int i = 0; i < len; i++)
                {
                    makeMove(listMov[i]);
                    int value = minimax(alpha, beta, depth - 1);
                    undoMove();
                    bestValue = Math.Max(bestValue, value);
                    if (maxDepth == depth && bestValue == value)
                        bestMov = listMov[i];
                    alpha = Math.Max(alpha, bestValue);
                    if (alpha >= beta)
                        break;
                }
                return bestValue;
            }
            else
            {
                List<int> listMov = validMoves();
                int bestValue = maxVal;
                int len = listMov.Count;
                for (int i = 0; i < len; i++)
                {
                    makeMove(listMov[i]);
                    int value = minimax(alpha, beta, depth - 1);
                    undoMove();
                    bestValue = Math.Min(bestValue, value);
                    alpha = Math.Min(alpha, bestValue);
                    if (alpha >= beta)
                        break;
                }
                return bestValue;
            }
        }
    }

}
