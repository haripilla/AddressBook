using System;

namespace AddressBook
{
    public class ConsoleReader : IGetInputFromUsers
    {
        public string GetNonEmptyString()
        {
            string input = Console.ReadLine();
            while (input.Length == 0)
            {
                Console.WriteLine("That is not valid.");
                input = Console.ReadLine();
            }
            return input;
        }

        public int GetNumber()
        {
            int v;
            string input = Console.ReadLine();
            while (!int.TryParse(input, out v))
            {
                if (v == 1)
                {
                    Console.WriteLine("You should type a number.");
                    input = Console.ReadLine();
                }
            }
            return (v);
        }

        public void WaitForEnterKey()
        {
            Console.WriteLine("Press ENTER to Continue...");
            Console.ReadLine();
        }
    }
}