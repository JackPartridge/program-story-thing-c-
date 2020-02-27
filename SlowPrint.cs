using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Text_based_game_oofer
{
    public class SlowPrint
    {
        public static void SlowPrinting(String text)
        {
            foreach (char d in text)
            {
                Console.Write(d);
                Thread.Sleep(1);
            }
        }
    }
}
