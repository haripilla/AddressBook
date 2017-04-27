using System;
using System.Configuration;

namespace AddressBook
{
    class Program
    {
        static void Main(string[] args)
        {
            // Databse Connection Start//
            string connectionString = ConfigurationManager.ConnectionStrings["AddressBook"].ConnectionString;
            // Databse Connection End//

            string name = ConfigurationManager.AppSettings["ApplicationName"];
            Console.WriteLine("WELCOME TO" + name);
            Console.WriteLine(new string('-', Console.WindowWidth - 50));
            Console.WriteLine();
            //Console.WriteLine("Press Enter to continue");
            //Console.ReadLine();
            Rolodex rolodex = new Rolodex(connectionString);
            rolodex.DoStuff();
        }
    }
}
