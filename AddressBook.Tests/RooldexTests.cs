using NSubstitute;
using NUnit.Framework;
using System;


namespace AddressBook.Tests
{
    [TestFixture]
    public class RooldexTests
    {
        private IGetInputFromUsers _input;
        private IHandleContacts _contacts;
        private IHandleRecipes _recipes;
        private Rolodex _rolodex;

        [SetUp]
        public void BeforeEachTest()
        {
            _input      = Substitute.For<IGetInputFromUsers>();
            _contacts   = Substitute.For<IHandleContacts>();
            _recipes    = Substitute.For<IHandleRecipes>();
            _rolodex    = new Rolodex(_contacts,_recipes,_input);
        }
        [Test]
        public void ExitJustDoesNothing()
        {
            //Arrange 
            _input.GetNumber().Returns(0);

            //Act
            _rolodex.DoStuff();
            //Assert
            _input.Received().GetNumber();
            _contacts.DidNotReceive().GetAllContacts();
            _recipes.DidNotReceive().GetAllRecipes();
            _contacts.DidNotReceive().CreateCompany(null,null);
        }
        [Test]
        public void AddPerson()
        {
            //Arrange 

            _input.GetNumber().Returns(1,0);
            _input.GetNonEmptyString().Returns("Bob", "Marley","555-555-9999");
            
            //Act
            _rolodex.DoStuff();
            //Assert
            _input.Received(2).GetNumber();
            _contacts.DidNotReceive().GetAllContacts();
            _recipes.DidNotReceive().GetAllRecipes();
            _contacts.DidNotReceive().CreateCompany(null, null);
            _contacts.Received().CreatePerson("Bob", "Marley", "555-555-9999");

        }

        public void AddRecipe()
        {
            _input.GetNumber().Returns(6,0);
            _input.GetNonEmptyString().Returns("Chicken");

            _rolodex.DoStuff();

            _input.Received(2).GetNumber();
            _contacts.DidNotReceive().GetAllContacts();
            _recipes.DidNotReceive().GetAllRecipes();
            _contacts.DidNotReceive().CreateCompany(null, null);
            _contacts.Received().CreatePerson(null,null,null);
            _recipes.Received().Create("Chicken",RecipeType.Appetizers);
            
        }

    }
}
