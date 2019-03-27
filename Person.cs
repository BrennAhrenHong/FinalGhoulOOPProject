using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OOP_Project.Person
{
    public class PersonClass
    {
        public string FirstName;
        public string MiddleName;
        public string LastName;
        public string BirthDate;
        public string Address;
        public int Age;

        public PersonClass(string firstName, string lastName, string middleName = "")
        {
            FirstName = FormatName(firstName);
            LastName = FormatName(lastName);
            MiddleName = FormatName(middleName);
        }

        public string GetFullName()
        {
            return String.Format("{0} {1}. {2}", FirstName, MiddleName[0], LastName);
        }
        private string FormatName(string Name)
        {
            Name = Name.ToLower();
            string[] names = Name.Split(' ');
            string formattedName = "";
            for (int counter = 0; counter < names.Length; counter++)
                formattedName = formattedName + char.ToUpper(names[counter][0]) + names[counter].Substring(1) + " ";
            return formattedName.Remove(formattedName.Length - 1);
        }
        public string GetBirthDate()
        {
            return BirthDate;
        }
        public string GetAddress()
        {
            return (char.ToUpper(Address[0]) + Address.Substring(1));
        }
        public static int GetAge(string birthDate)
        {
            return 1;//CalculationsClass.CalculateAge(birthDate);
        }
    }
}
