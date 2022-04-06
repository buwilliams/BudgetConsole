using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetConsole
{
    public class App
    {
        public App()
        {
            ConsoleBudget sb = SampleBudget();
            sb.Print();
        }

        static ConsoleBudget SampleBudget()
        {
            ConsoleBudget budget = new(4, 2022);

            // Expenses
            budget.Add(LineItem.Create("Child Support", 1702, 1));
            budget.Add(LineItem.Create("Daily #1", 900, 1));
            budget.Add(LineItem.Create("Save #1", 0, 1));
            budget.Add(LineItem.Create("Debt #1", 0, 30));
            budget.Add(LineItem.Create("Buffer #1", 100, 1));
            budget.Add(LineItem.Create("Carry Over #1", 0, 1));
            budget.Add(LineItem.Create("Counseling", 150, 1));
            budget.Add(LineItem.Create("Charity", 40.00, 1));
            budget.Add(LineItem.Create("Mortgage", 1544, 1));
            budget.Add(LineItem.Create("News", 4.99, 3));
            budget.Add(LineItem.Create("Disney+", 8, 3));
            budget.Add(LineItem.Create("Term Life", 43, 7));
            budget.Add(LineItem.Create("At&t", 91, 9));
            budget.Add(LineItem.Create("Daily #2", 900, 15));
            budget.Add(LineItem.Create("Car Insurance", 111, 15));
            budget.Add(LineItem.Create("Save #2", 0, 15));
            budget.Add(LineItem.Create("Debt #2", 0, 15));
            budget.Add(LineItem.Create("Carry Over #2", 1200, 15));
            budget.Add(LineItem.Create("Buffer #2", 100, 1));
            budget.Add(LineItem.Create("Loan", 312, 15));
            budget.Add(LineItem.Create("Spotify", 10, 16));
            budget.Add(LineItem.Create("YouTube Premium", 10, 17));
            budget.Add(LineItem.Create("HBO Max", 15, 21));
            budget.Add(LineItem.Create("Xbox", 10, 25));
            budget.Add(LineItem.Create("Sports", 0, 26));
            budget.Add(LineItem.Create("Protein Powder", 0, 26));
            budget.Add(LineItem.Create("Fed Taxes", 763, 28));
            budget.Add(LineItem.Create("Netlify", 9, 29));
            budget.Add(LineItem.Create("Github", 4.99, 30));
            budget.Add(LineItem.Create("Netflix", 14.99, 30));

            // Income
            budget.Add(LineItem.Create("Balance #1", 1200, 1, Category.OTHER, Type.INCOME));
            budget.Add(LineItem.Create("My Company #1", 2543, 1, Category.OTHER, Type.INCOME));
            budget.Add(LineItem.Create("Balance #2", 0, 15, Category.OTHER, Type.INCOME));
            budget.Add(LineItem.Create("My Company #2", 1265, 15, Category.OTHER, Type.INCOME));

            return budget;
        }

    }
}
