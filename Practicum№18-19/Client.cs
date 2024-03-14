using System;
using System.ComponentModel.Design;
using System.Net.NetworkInformation;
using System.Text;
using System.IO;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace ConsoleApp1
{
    [Serializable]
    internal abstract class Client : IComparable<Client>
    {

        public string LastName { get; }
        public DateTime StartDate { get; }

        public Client(string lastName, DateTime startDate)
        {
            LastName = lastName;
            StartDate = startDate;
        }

        public bool IsMatch(DateTime targetDate)
        {
            return StartDate >= targetDate;
        }


        public int CompareTo(Client other)
        {
            return StartDate.CompareTo(other.StartDate);
        }
    }
}
