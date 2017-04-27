using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
namespace AddressBook
{
    public class Rolodex
    {
        public Rolodex(string connectionString)
        {
            _connectionString = connectionString; // Databse Connection 

            _contacts = new List<Contact>();
            _recipes = new Dictionary<RecipeType, List<Recipe>>();

            _recipes.Add(RecipeType.Appetzers, new List<Recipe>());
            _recipes[RecipeType.Entrees] = new List<Recipe>();
            _recipes.Add(RecipeType.Desserts, new List<Recipe>());
        }

        public void DoStuff()
        {
            // Print a menu
            ShowMenu();
            // Get the user's choice
            MenuOption choice = GetMenuOption();
            
            // while the user does not want to exit
            while (choice != MenuOption.Exit)
            {
                // figure out what they want to do
                // get information
                // do stuff
                switch(choice)
                {
                    case MenuOption.Addperson:
                        //DoFileOpen();
                        DoAddPerson();
                        break;
                    case MenuOption.Addcompany:
                        DoAddCompany();
                        break;
                    case MenuOption.Listcontacts:
                        DoListContacts();
                        break;
                    case MenuOption.Searchcontacts:
                        DoSearchContacts();
                        break;
                    case MenuOption.Removecontact:
                        DoRemoveContact();
                        break;
                    case MenuOption.Recipe:
                        DoAddReceipe();                         
                        break;
                    case MenuOption.SearchAll:
                        DoSearchAll();
                        break;
                    case MenuOption.ListofReceipe:
                        DoListofRecipe();
                        break;
                }
                ShowMenu();
                choice = GetMenuOption();
            }
        }



        private void DoListofRecipe()
        {
            //SqlConnection connection;
            //connection = new SqlConnection(connectionstring);
            Console.Clear();
            Console.WriteLine("RECIPES!");
            Console.WriteLine("--------");

            //SqlConnection connection = new SqlConnection(_connectionString);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command = connection.CreateCommand();
                command.CommandText = $@"
                        SELECT RecipeName, Recipetype
                        FROM Recipe  
                        Order by Recipetype,RecipeName
                        ";
                //command.Parameters.AddWithValue("@Recipetype", recipeType);
                SqlDataReader reader = command.ExecuteReader();
                int currentRecipeTypeId = -1;
                Console.ReadLine();

                while (reader.Read())
                {
                    //int rowID = reader.GetInt32(0);
                    string recipeName = reader.GetString(0);
                    int recipetype = int.Parse(reader.GetString(1));

                    if (recipetype != currentRecipeTypeId)
                    {
                        currentRecipeTypeId = recipetype;
                        RecipeType pretty = (RecipeType)currentRecipeTypeId;
                        Console.WriteLine(pretty.ToString().ToUpper());
                    }
                    Console.WriteLine("   "+ recipeName.ToUpper());
                }
            }
            Console.ReadLine();
        }


        private void DoSearchAll()
        {
            Console.Clear();
            Console.WriteLine("SEARCH!");
            Console.Write("Please enter a search item: ");
            string term = GetNonEmptyStringFromUser();

            List<IMatchable> matchables = new List<IMatchable>();
            matchables.AddRange(_contacts);
            //matchables.AddRange(_recipes[RecipeType.Appetzers]);
            //matchables.AddRange(_recipes[RecipeType.Entrees]);
            //matchables.AddRange(_recipes[RecipeType.Desserts]);

            //foreach (IMatchable matcher in matchables)
            //{
            //    if (matcher.Matches(term))
            //    {
            //        Console.WriteLine($" > {matcher}");
            //    }
            //}

            //Console.WriteLine("Press Enter to continue...");
            //Console.ReadLine();


            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command = connection.CreateCommand();
                command.CommandText = $@"
                        SELECT RecipeName, Recipetype
                        FROM Recipe  
                        Order by Recipetype,RecipeName
                        ";
                //command.Parameters.AddWithValue("@Recipetype", recipeType);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string recipeName = reader.GetString(0);
                    int recipetype = int.Parse(reader.GetString(1));
                    Recipe recipe = new Recipe(recipeName);
                    matchables.Add(recipe);
                }
            }

            foreach (IMatchable matcher in matchables)
            {
                if (matcher.Matches(term))
                {
                    Console.WriteLine($" > {matcher}");
                }
            }

            Console.ReadLine();
        }

        public void DoAddReceipe()
        {
            Console.Clear();
            Console.WriteLine("Please enter your Receipe title");
            string title = GetNonEmptyStringFromUser();
            Recipe recipe = new Recipe(title);
            Console.WriteLine("what Kind of Recipe is this ?");

            for (int i = 0; i < (int)RecipeType.UPPER_LIMIT; i++)
            {
                Console.WriteLine($"{i}.{(RecipeType)i}");
            }

            //string input = Console.ReadLine();
            //int num = int.Parse(input);
            //RecipeType choice = (RecipeType)num;
            RecipeType choice = (RecipeType)int.Parse(Console.ReadLine());

            List<Recipe> specificRecipes = _recipes[choice];
            specificRecipes.Add(recipe);

            //Console.WriteLine(recipe);
            //Console.WriteLine(choice);
            //Console.WriteLine(_recipes[choice]);
            //Console.WriteLine(specificRecipes);
            //Console.ReadLine();

            //string connectionstring;
            
            SqlConnection connection = new SqlConnection(_connectionString);
                        
            try
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = $@"
                        insert into Recipe (RecipeName, Recipetype)
                        values (@recipe,@choice)
                        ";
                command.Parameters.AddWithValue("@recipe", title);
                command.Parameters.AddWithValue("@choice", choice);
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Dispose();
            }
            
        }
        private void DoRemoveContact()
        {
            Console.Clear();
            Console.WriteLine("REMOVE A CONTACT!");
            Console.Write("Search for a contact: ");
            string term = GetNonEmptyStringFromUser();

            foreach (Contact contact in _contacts)
            {
                if (contact.Matches(term))
                {
                    Console.Write($"Remove {contact}? (y/N)");
                    if (Console.ReadLine().ToLower() == "y")
                    {
                        _contacts.Remove(contact);
                        return;
                    }
                }
            }

            Console.WriteLine("No more contacts found.");
            Console.WriteLine("Press Enter to return to the menu...");
            Console.ReadLine();
        }

        private void DoSearchContacts()
        {
            Console.Clear();
            Console.WriteLine("SEARCH!");
            Console.Write("Please enter a search term: ");
            string term = GetNonEmptyStringFromUser();

            DoSearchLoad();
            foreach (Contact contact in _contacts)
            {
                if (contact.Matches(term))
                {
                    Console.WriteLine($"> {contact}");
                }
            }

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        private void DoListContacts()
        {
            Console.Clear();
            Console.WriteLine("YOUR CONTACTS");

            //_contacts.Add(new Person(firstName, lastName, phoneNumber));
            DoSearchLoad();
            foreach (Contact contact in _contacts)
            {
                Console.WriteLine($"> {contact}");
            }

            Console.ReadLine();
        }
        private void DoSearchLoad()
        {
            _contacts.Clear();
            string desktoppath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filename = "DATA.DAT";
            string fullpath = Path.Combine(desktoppath, filename);

            using (StreamReader reader = File.OpenText(fullpath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] parts = line.Split('|');
                    //Console.WriteLine(string.Join("\t", parts));
                    //Console.WriteLine(line);

                    if (parts[0] == "P")
                    {
                        string firstName = parts[1];
                        string lastName = parts[2];
                        string phoneNumber = parts[3];
                        _contacts.Add(new Person(firstName, lastName, phoneNumber));
                    }

                    if (parts[0] == "C")
                    {
                        string firstName = parts[1];
                        string lastName = parts[2];
                        string phoneNumber = parts[3];
                        _contacts.Add(new Company(lastName, phoneNumber));
                    }
                }
            }
        }
        private void DoAddCompany()
        {
            Console.Clear();
            Console.WriteLine("Please enter information about the company.");
            Console.Write("Company name: ");
            string name = Console.ReadLine();

            Console.Write("Phone number: ");
            string phoneNumber = GetNonEmptyStringFromUser();

            _contacts.Add(new Company(name, phoneNumber));

            string desktoppath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filename = "DATA.DAT";
            string fullpath = Path.Combine(desktoppath, filename);

            using (StreamWriter writer = File.AppendText(fullpath))
            {
                writer.WriteLine($"C| |{name}|{phoneNumber}");
            }


        }

        private void DoAddPerson()
        {
            Console.Clear();
            Console.WriteLine("Please enter information about the person.");
            Console.Write("First name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last name: ");
            string lastName = GetNonEmptyStringFromUser();

            Console.Write("Phone number: ");
            string phoneNumber = GetNonEmptyStringFromUser();

            _contacts.Add(new Person(firstName, lastName, phoneNumber));
            string desktoppath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filename = "DATA.DAT";
            string fullpath = Path.Combine(desktoppath, filename);

            using (StreamWriter writer = File.AppendText(fullpath))
            {
                writer.WriteLine($"P|{firstName}|{lastName}|{phoneNumber}");
            }
        }
        private string GetNonEmptyStringFromUser()
        {
            string input = Console.ReadLine();
            while (input.Length == 0)
            {
                Console.WriteLine("That is not valid.");
                input = Console.ReadLine();
            }
            return input;
        }

        private int GetNumberUSer()
        {
            while (true)
            {
                try
                {
                    string input = Console.ReadLine();
                    return int.Parse(input);
                }
                catch (FormatException)
                {
                    Console.WriteLine("your should enter the number <9");
                    Console.ReadLine();
                }

                //finally
                //{
                //    Console.WriteLine("THIS WILL ALWAYS PRINTED");
                //    Console.ReadLine();
                //}
            }

        }
        private MenuOption GetMenuOption()
        {
            int choice = GetNumberUSer();

            while (choice < 0 || choice >= (int)MenuOption.UPPER_LIMIT)
            {
                Console.WriteLine("That is not valid.");
                choice = GetNumberUSer();
            }

            return (MenuOption)choice;
        }

        private void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine($"ROLODEX! ({_contacts.Count})");
            Console.WriteLine("1. Add a person");
            Console.WriteLine("2. Add a company");
            Console.WriteLine("3. List all contacts");
            Console.WriteLine("4. Search contacts");
            Console.WriteLine("5. Remove a contact");
            Console.WriteLine("6. Add Recipe ");
            Console.WriteLine("7. Search All !!!");
            Console.WriteLine("8. List of Receipe !!!");

            Console.WriteLine();
            Console.WriteLine("0. Exit");
            Console.WriteLine();
            Console.Write("What would you like to do? ");
        }

        private readonly List<Contact> _contacts;
        private Dictionary<RecipeType, List<Recipe>> _recipes;
        private readonly string _connectionString;
    }
}

