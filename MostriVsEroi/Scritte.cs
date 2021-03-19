using System;
using System.Collections.Generic;
using System.Text;

namespace MostriVsEroi
{
    public class Scritte
    {
        //Fatta un po' per gioco, si legge bene a schermo intero.
        public static void TitoloGioco()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" __   __  _______  _______  _______  ______    ___  \t\t __   __  _______ \t\t _______  ______    _______  ___  ");
            Console.WriteLine("|  |_|  ||       ||       ||       ||    _ |  |   | \t\t|  | |  ||       |\t\t|       ||    _ |  |       ||   | ");
            Console.WriteLine("|       ||   _   ||  _____||_     _||   | ||  |   | \t\t|  |_|  ||  _____|\t\t|    ___||   | ||  |   _   ||   | ");
            Console.WriteLine("|       ||  | |  || |_____   |   |  |   |_||_ |   | \t\t|       || |_____ \t\t|   |___ |   |_||_ |  | |  ||   | ");
            Console.WriteLine("|       ||  |_|  ||_____  |  |   |  |    __  ||   | \t\t|       ||_____  |\t\t|    ___||    __  ||  |_|  ||   | ");
            Console.WriteLine("| ||_|| ||       | _____| |  |   |  |   |  | ||   | \t\t |     |  _____| |\t\t|   |___ |   |  | ||       ||   | ");
            Console.WriteLine("|_|   |_||_______||_______|  |___|  |___|  |_||___| \t\t  |___|  |_______|\t\t|_______||___|  |_||_______||___|\n\n\n ");
            Console.ResetColor();
        }
    }
}
