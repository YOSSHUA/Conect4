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

        void makeMove(int col) //Hacen movimiento
        {            
            long move = 1 << height[col]++; // (1)            
            bitboard[counter & 1] ^= move;  // (2)
            moves[counter++] = col;         // (3)
        }
        void undoMove() //Deshacen movimiento
        {
            int col = moves[--counter];     // reverses (3)
            long move = 1L << --height[col]; // reverses (1)
            bitboard[counter & 1] ^= move;  // reverses (2)
        }
        List<int> listMoves()   //Lista de de movimientos validos
        {
            List<int> moves = new List<int>();
            long TOP = 0b1000000_1000000_1000000_1000000_1000000_1000000_1000000L;
            for (int col = 0; col <= 6; col++)
            {
                if ((TOP & (1L << height[col])) == 0) moves.Add(col);
            }
            return moves;
        }
        bool isWin(long bitboard)   //Indica si la posición actual es ganadora
        {            
            return false;
        }
        int evalPosition(long pos)  //
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
