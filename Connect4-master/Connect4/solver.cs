using System;
using System.Collections.Generic;
using System.Text;

namespace con4
{
    class solver
    {
        long[] bitboard;
        int counter = 0;
        int[] height;
        int [] moves;
        public solver()
        {
            bitboard = new long[2];
            height = new int[10];
            moves = new int[500];
        }

        void makeMove(int col)
        {            
            long move = 1 << height[col]++; // (1)            
            bitboard[counter & 1] ^= move;  // (2)
            moves[counter++] = col;         // (3)
        }
        void undoMove()
        {
            int col = moves[--counter];     // reverses (3)
            long move = 1L << --height[col]; // reverses (1)
            bitboard[counter & 1] ^= move;  // reverses (2)
        }
        List<int> listMoves()
        {
            List<int> moves = new List<int>();
            long TOP = 0b1000000_1000000_1000000_1000000_1000000_1000000_1000000L;
            for (int col = 0; col <= 6; col++)
            {
                if ((TOP & (1L << height[col])) == 0) moves.Add(col);
            }
            return moves;
        }
        bool isWin(long bitboard)
        {            

            return false;
        }
        int evalPosition(long pos)
        {
            List<int> nextMov = listMoves();
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
    }

}
