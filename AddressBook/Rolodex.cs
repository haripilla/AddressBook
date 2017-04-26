using System;
using System.Collections.Generic;

namespace AddressBook
{
    public class Rolodex
    {
        public Rolodex()
        {
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
            Console.Clear();
            Console.WriteLine("RECIPES!");
            
            foreach (RecipeType recipeType in _recipes.Keys)
            {
                Console.WriteLine(recipeType);
                List<Recipe> specificRecipes = _recipes[recipeType];
                foreach (Recipe recipe in specificRecipes)
                {
                    Console.WriteLine($"\t{recipe}");
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
            matchables.AddRange(_recipes[RecipeType.Appetzers]);
            matchables.AddRange(_recipes[RecipeType.Entrees]);
            matchables.AddRange(_recipes[RecipeType.Desserts]);

            foreach (IMatchable matcher in matchables)
            {
                if (matcher.Matches(term))
                {
                    Console.WriteLine($" > {matcher}");
                }
            }

            Console.WriteLine("Press Enter to continue...");
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

            RecipeType choice = (RecipeType)int.Parse(Console.ReadLine());

            List<Recipe> specificRecipes = _recipes[choice];
            specificRecipes.Add(recipe);

            //_recipes.Add(recipe);

            //_recipes.Add(RecipeType.Appetzers, new List<Recipe>());

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
            
            foreach (Contact contact in _contacts)
            {
                Console.WriteLine($"> {contact}");
            }

            Console.ReadLine();
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

                finally
                {
                    Console.WriteLine("THIS WILL ALWAYS PRINTED");
                    Console.ReadLine();
                }
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

        private List<Contact> _contacts;
        //private List<Recipe> _recipes;
        private Dictionary<RecipeType, List<Recipe>> _recipes;
    }
}

