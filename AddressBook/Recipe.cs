using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AddressBook
{
    public class Recipe :IMatchable
    {
        public Recipe(string title)
        {
            _title = title;
        }

        public bool Matches(string term)
        {

            return _title.ToLower().StartsWith(term.ToLower());

        }

        public override string ToString()
        {
            return _title;
        }


        private string _title;
    }

}
