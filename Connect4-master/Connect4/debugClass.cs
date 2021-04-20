using con4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    class debugClass
    {
        static void main(string[] args)
        {
            int dif = Int32.Parse(Console.ReadLine());
            Solver s = new Solver(6, 7, dif, 10, 10, Color.White, Color.Black);
            int moves = 0;
            while (true && moves < 42)
            {
                s.printBoard();
                int col = Int32.Parse(Console.ReadLine());
                int result = s.applyMove(0, col);
                if (result == 1)
                {
                    Console.WriteLine("Gano la persona");
                    break;
                }
                moves++;
                if (moves >= 42)
                    break;
                s.printBoard();
                result = s.applyMove(1);
                if (result == 2)
                {
                    s.printBoard();
                    Console.WriteLine("Gano la compu");
                    break;
                }
            }
            Console.ReadLine();

        }
    }
}
