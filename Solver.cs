using System;
using System.Collections.Generic;
using System.Text;

namespace comandosBin
{
    class Solver
    {
        int[] height;
        int counter;
        long[] bitboard;
        List<int> moves;
        public Solver()
        {
            height = new int[6];
            bitboard = new long[2];
            moves = listMoves();
            counter = 0;
        }
        public static void Main()
        {
            long x = 0b1000000_1000000_1000000_1000000_1000000_1000000_1000000L;
            System.Console.WriteLine(x);
        }
        private void makeMove(int col)
        {
            long move = 1L << height[col]++;
            bitboard[counter & 1] ^= move;
            moves[counter++] = col;
        }
        private void undoMove()
        {
            int col = moves[--counter];
            long move = 1L << --height[col];
            bitboard[counter & 1] ^= move;
        }
        
        private Boolean isWin(long bitboard)
        {
            int[] directions = { 1, 7, 6, 8 };
            for (int i=0; i<directions.Length; i++)
            {
                if ((bitboard & (bitboard >> directions[i]) &
                   (bitboard >> (2 * directions[i])) & (bitboard >> (3 * directions[i]))) != 0)
                    return true;
                
            }
            return false;
        }
        
        List<int> listMoves()
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
    }
}
