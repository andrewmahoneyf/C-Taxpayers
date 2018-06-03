/* Andrew Mahoney-Fernandes */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMahoneyFernandes_FinalApp
{
    class Taxpayer
    {
        // fields
        private string name;
        private decimal salary, investment, taxableIncome, income, taxRate;
        private int exemptions;
        private bool isMarried;

        // static fields
        private static int countMarried, countUnMarried;
        private static decimal totalTaxDec;

        public Taxpayer(string name, bool isMarried, decimal salary, decimal investment, int exemptions)
        {
            this.name = name;
            this.isMarried = isMarried;
            this.salary = salary;
            this.investment = investment;
            this.exemptions = exemptions;
            this.income = salary + investment;
            decimal deduction = CalculateDeduction(isMarried, exemptions);
            totalTaxDec += CalculateTax(deduction);
        }


        private decimal CalculateDeduction(bool isMarried, int exemptions)
        {
            decimal deduction;

            if (isMarried) {
                deduction = 600 + (400 * exemptions);
                countMarried++;
            } else {
                deduction = 300 + (225 * exemptions);
                countUnMarried++;
            }

            return deduction;
        }
        
        private decimal CalculateTaxRate(bool isMarried, decimal taxableIncome) {
            if (isMarried) {
                if (taxableIncome > 100000) {
                    return .2m;
                } else {
                    return .15m;
                }
            } else {
                if (taxableIncome > 70000) {
                    return .15m;
                } else {
                    return .10m;
                }
            }
        }

        private decimal CalculateTax(decimal deduction)
        {
            decimal tax;
            if (deduction > income)
            {
                this.taxableIncome = 0;
                tax = 0m;
            }
            else
            {
                this.taxableIncome = income - deduction;
                this.taxRate = CalculateTaxRate(isMarried, taxableIncome);
                tax = taxableIncome * taxRate;
            }
            return tax;
        }

        public string GetInfo() 
        {
            return this.name + "\t" + 
                   this.salary.ToString("C") + "\t" + 
                   this.investment.ToString("C") + "\t" + 
                   this.exemptions.ToString() + "\t" + 
                   this.GetMaritalStatus();
        }

        public string Display()
        {
            decimal tax = taxableIncome * taxRate;
            return this.name + 
                   "\t Income: " + this.income.ToString("C") + 
                   "\t Tax: " + tax.ToString("C");
        }

        private string GetMaritalStatus()
        {
            if (this.isMarried) {
                return "Married";
            }

            return "UnMarried";
        }

        public static string GetSummary()
        {
            decimal averageTax = totalTaxDec / (countMarried + countUnMarried);
            return "Total Married Taxpayers: " + countMarried.ToString() +
                   "\t Total UnMarried Taxpayers: " + countUnMarried.ToString() +
                   "\n\r Average Tax: " + averageTax.ToString("C");
        }

        public string Save() 
        {
            int taxPercent = Convert.ToInt32(this.taxRate * 100);
            decimal tax = taxableIncome * taxRate;
            return this.name + ", " +
                   this.salary.ToString("C") + ", " +
                   this.investment.ToString("C") + ", " +
                   this.taxableIncome.ToString("C") + ", " +
                   taxPercent.ToString() + "%, " +
                   tax.ToString("C") + ", ";
        }

        public static void ResetData()
        {
            countMarried = 0;
            countUnMarried = 0;
            totalTaxDec = 0m;
        }
    }
}
