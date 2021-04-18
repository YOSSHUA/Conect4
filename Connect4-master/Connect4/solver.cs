using System;
using System.Collections.Generic;
using System.Text;

namespace con4
{
    class solver
    {
        long[] bitboard;    //Tablero
        int counter = 0;    //Contador para saber quien es el tirador
        int[] height;       //Indica siguiente posición donde puedes poner fichar
        int [] moves;       //Hace movimiento y los guarda
        public solver() //Constructor del solver
        {
            bitboard = new long[2];
            height = new int[10];
            moves = new int[500];
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
                if(isWin(bitboard[counter ^ 1]))
                {
                    winningMovs++;
                }            
                undoMove();
            }
            return winningMovs;

        }
        int maxVal = 500;
        int bestMov;
        int maxDepth=100;
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
