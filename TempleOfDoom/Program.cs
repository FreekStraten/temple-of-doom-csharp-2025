using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleOfDoom
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");


            int x = 10;
            int y = 10;
            ConsoleKeyInfo keyInfo;

            Console.CursorVisible = false;

            while (true)
            {
                Console.Clear();
                Console.SetCursorPosition(x, y);
                Console.Write("X");

                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (y > 0) y--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (y < Console.WindowHeight - 1) y++;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (x > 0) x--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (x < Console.WindowWidth - 1) x++;
                        break;
                }
            }
        }
    }
}
