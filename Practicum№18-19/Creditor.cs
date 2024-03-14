using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    [Serializable]
    internal class Creditor : Client
    {

        public double CreditAmount { get; }
        public double Credit { get; }
        public double RemainingCredit { get; }

        public Creditor(string lastName, DateTime startDate, double creditAmount, double credit, double remainingCredit) : base(lastName, startDate)
        {
            CreditAmount = creditAmount;
            Credit = credit;
            RemainingCredit = remainingCredit;
        }

        public override string ToString()
        {
            return $"Creditor: {LastName}, Credit Amount: {CreditAmount}, Credit: {Credit}%, Remaining Credit: {RemainingCredit}";
        }

    }
}
