using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetConsole
{
    public class Budget
    {
        public List<LineItem> items {  get; set; }

        public Budget()
        {
            this.items = new List<LineItem>();
        }

        public LineItem Get(int index)
        {
            return items[index];
        }

        public int Add(LineItem item)
        {
            items.Add(item);
            return items.IndexOf(item);
        }

        public void Remove(LineItem item)
        {
            items.Remove(item);
        }

        public void RemoveAll()
        {
            items.Clear();
        }

        public List<LineItem> GetExpense()
        {
            return this.items.FindAll(item => item.Type == Type.EXPENSE);
        }

        public List<LineItem> GetIncome()
        {
            return this.items.FindAll(item => item.Type == Type.INCOME);
        }

        public Tally Totals()
        {
            Tally tally = new();
            foreach(LineItem i in GetExpense())
            {
                tally.expenseAmountTotal += i.Amount;
                tally.expenseRemainingTotal += i.Remaining;
            }

            tally.expenseAmountTotal = Math.Ceiling(tally.expenseAmountTotal);
            tally.expenseRemainingTotal = Math.Ceiling(tally.expenseRemainingTotal);

            foreach(LineItem i in GetIncome())
            {
                tally.incomeAmountTotal += i.Amount;
                tally.incomeRemainingTotal += i.Remaining;
            }

            tally.incomeAmountTotal = Math.Floor(tally.incomeAmountTotal);
            tally.incomeRemainingTotal = Math.Floor(tally.incomeRemainingTotal);

            tally.balanceTotal = tally.incomeAmountTotal - tally.expenseAmountTotal;
            tally.balanceRemaining = tally.incomeRemainingTotal - tally.expenseRemainingTotal;

            return tally;
        }
    }

    public class Tally
    {
        public double expenseAmountTotal = 0.0;
        public double expenseRemainingTotal = 0.0;
        public double incomeAmountTotal = 0.0;
        public double incomeRemainingTotal = 0.0;
        public double balanceTotal = 0.0;
        public double balanceRemaining = 0.0;
    }

    public enum Type
    {
        INCOME,
        EXPENSE
    }

    public enum Status
    {
        NONE,
        IGNORED,
        PENDING,
        CLEARED
    }

    public enum Category
    {
        NONE,
        BILLS,
        DEBT,
        LIVING,
        CHILD,
        OTHER,
        OVERFLOW
    }

    public class LineItem
    {
        public int DueDay { get; set; }
        public Status Status { get; set; }
        public string Name { get; set; }
        public double Remaining { get; set; }
        public double Amount { get; set; }
        public Category Category { get; set; }
        public Type Type { get; set; }

        public LineItem()
        {
            this.DueDay = 1;
            this.Status = Status.NONE;
            this.Name = "";
            this.Remaining = 0.0;
            this.Amount = 0.0;
            this.Category = Category.BILLS;
            this.Type = Type.EXPENSE;
        }

        public static LineItem Create(
            string name,
            double amount,
            int dueDay,
            Category category = Category.BILLS,
            Type type = Type.EXPENSE
        )
        {
            LineItem li = new();
            li.DueDay = dueDay;
            li.Status = Status.NONE;
            li.Name = name;
            li.Remaining = amount;
            li.Amount = amount;
            li.Category = category;
            li.Type = type;
            return li;
        }
    }
}
