using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    [Serializable]
    internal class Depositor : Client
    {

        public double DepositAmount { get; }
        public double Deposit { get; }

        public Depositor(string lastName, DateTime startDate, double depositAmount, double deposit) : base(lastName, startDate)
        {
            DepositAmount = depositAmount;
            Deposit = deposit;
        }

        public override string ToString()
        {
            return $"Depositor: {LastName}, Deposit Amount: {DepositAmount}, Deposit Interest: {Deposit}%, time: {StartDate}";
        }


    }
}
