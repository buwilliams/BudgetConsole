using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace BudgetConsole
{
    class ConsoleBudget : DiskBudget
    {
        public enum COLUMNS
        {
            INDEX = 10,
            STATUS = 10,
            NAME = 40,
            CATEGORY = 10,
            AMOUNT = 20,
            REMAINING = 20 
        };

        private const char STAR = '*';
        private const char DASH = '-';

        private int Month = 1;
        private int Year = 2022;

        public ConsoleBudget(int month, int year) : base()
        {
            this.Month = month;
            this.Year = year;
        }

        /*
         * I may want want to use a template method here
         * to make formatting a visual experience
         */
        public void Print()
        {
            Console.SetWindowSize(200, 80);

            while(true)
            {
                Clear();
                ChangeColor(true);
                RenderBlank();
                RenderCenteredLine(GetLine(STAR, 0.5));
                RenderCenteredLine("Welcome to Budgetting in your Console!");
                RenderCenteredLine(String.Format("Budget date: {0}/{1} {2}", Month, Year, Saved ? "" : "***Unsaved"));
                RenderCenteredLine(GetLine(STAR, 0.5));

                RenderBlank();
                RenderLine(GetLine(DASH));
                RenderLine("EXPENSES");
                RenderLine(GetLine(DASH));
                RenderLine(PrintableHeaders());
                RenderLine(GetLine(DASH));

                ChangeColor();

                
                foreach(LineItem i in GetExpense())
                {
                    RenderLine(PrintableLineItem(i));
                }

                ChangeColor(true);
                RenderBlank();
                RenderBlank();
                RenderLine(GetLine(DASH));
                RenderLine("INCOME");
                RenderLine(GetLine(DASH));
                RenderLine(PrintableHeaders());
                RenderLine(GetLine(DASH));

                ChangeColor();

                foreach(LineItem i in GetIncome())
                {
                    RenderLine(PrintableLineItem(i));
                }

                ChangeColor(true);
                RenderBlank();
                RenderBlank();
                RenderLine(GetLine(DASH));
                RenderLine("TOTALS");
                RenderLine(GetLine(DASH));
                RenderLine(PrintableTotalsHeaders());
                RenderLine(GetLine(DASH));
                ChangeColor();
                RenderTotals();
                ChangeColor(true);
                RenderLine(GetLine(DASH));
                ChangeColor();

                RenderBlank();
                RenderBlank();
                RenderMenu();
                if(!HandleMenu()) break;
            }
        }

        private void ChangeColor(bool highlight = false)
        {
            if(highlight)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            } else
            {
                Console.ResetColor();
            }

        }

        private bool HandleMenu()
        {
            if(!Saved)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(string.Format("Status: changes have not been saved to {0}/{1}", Month, Year));
                Console.ResetColor();
            } else {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(string.Format("Status: saved in {0}/{1}", Month, Year));
                Console.ResetColor();
            }
            string option = Ask("Enter option: ", true);
            switch(option)
            {
                case "q":
                    return false;
                case "a":
                    return HandleAdd();
                case "e":
                    return HandleEdit();
                case "d":
                    return HandleDelete();
                case "t":
                    return HandleStatus();
                case "r":
                    return HandleRemoveAll();
                case "n":
                    return HandleNew();
                case "l":
                    return HandleLoad().Result;
                case "s":
                    return HandleSave().Result;
                default:
                    return true;
            }
        }

        private bool HandleNew()
        {
            int month = AskMonth();
            int year = AskYear();

            Month = month;
            Year = year;

            Saved = false;

            return true;
        }

        private async Task<bool> HandleLoad()
        {
            int month = AskMonth();
            int year = AskYear();
            Budget budget = await Load(month, year);
            items.Clear();
            Month = month;
            Year = year;
            items = budget.items;
            return true;
        }

        private bool HandleRemoveAll()
        {
            Saved = false;
            items.Clear();
            return true;
        }

        private bool HandleStatus()
        {
            Saved = false;
            int index = AskIndex();
            LineItem item = items[index];
            Status status = AskStatus(item.Status);
            item.Status = status;
            return true;
        }

        private bool HandleDelete()
        {
            Saved = false;
            int index = AskIndex();
            items.RemoveAt(index);
            return true;
        }

        private bool HandleEdit()
        {
            Saved = false;
            int index = AskIndex();

            LineItem li = items[index];

            li.Name = AskName(li.Name);
            double amount = AskAmount(li.Amount);
            li.Amount = amount;
            double remaining = AskRemaining(li.Remaining);
            li.Remaining = remaining;
            Type type = AskType(li.Type);
            li.Type = type;
            if(li.Type == Type.INCOME)
            {
                li.Category = Category.OTHER;
            }
            else
            {
                li.Category = AskCategory(li.Category);
            }

            return true;
        }

        private async Task<bool> HandleSave()
        {
            await Save(ToJson(), Month, Year);
            return true;
        }

        private bool HandleAdd()
        {
            Saved = false;
            LineItem li = new();
            li.Name = AskName(null);
            double amount = AskAmount(null);
            li.Amount = amount;
            li.Remaining = amount;
            Type type = AskType(null);
            li.Type = type;
            if(li.Type == Type.INCOME)
            {
                li.Category = Category.OTHER;
            }
            else
            {
                li.Category = AskCategory(null);
            }

            Add(li);

            return true;
        }

        private int AskIndex()
        {
            string index = Ask("Enter index: ", false);
            return Convert.ToInt32(index);
        }

        private int AskMonth()
        {
            string month = Ask("Enter month: ", false);
            return Convert.ToInt32(month);
        }

        private int AskYear()
        {
            string year = Ask("Enter year: ", false);
            return Convert.ToInt32(year);
        }

        private string AskName(string? current)
        {
            if(current != null)
            {
                RenderLine("Current budgetted item name: " + current);
            }
            return Ask("Enter budgetted item name: ", false);
        }

        private double AskAmount(double? current)
        {
            if(current != null)
            {
                RenderLine("Current amount: " + current);
            }
            string stringAmount = Ask("Enter amount: ", false);
            double amount = Convert.ToDouble(stringAmount);
            return amount;
        }

        private double AskRemaining(double? current)
        {
            if(current != null)
            {
                RenderLine("Current remaining: " + current);
            }
            string stringAmount = Ask("Enter remaining: ", false);
            double remaining = Convert.ToDouble(stringAmount);
            return remaining;
        }
        private Type AskType(Type? current)
        {
            if(current != null)
            {
                RenderLine("Current type: " + current);
            }
            string stringAmount = Ask("Enter type, (I)ncome, (E)xpense: ", true);
            if(stringAmount == "i")
            {
                return Type.INCOME;
            } else
            {
                return Type.EXPENSE;
            }
        }

        private Category AskCategory(Category? current)
        {
            if(current != null)
            {
                RenderLine("Current category: " + CategoryToString(current));
            }
            string stringAmount = Ask("Enter category, (N)one, (B)ills, (D)ebt, (L)iving, (C)hild, (O)ther, OVER(F)LOW: ", true);
            return StringToCategory(stringAmount);
        }

        private Status AskStatus(Status? current)
        {
            if(current != null)
            {
                RenderLine("Current status: " + current);
            }
            
            string stringAmount = Ask("Enter status, (N)one, (I)gnore, (P)ending, (C)lear:", true);
            return StringToStatus(stringAmount);
        }

        private string CategoryToString(Category? category)
        {
            switch(category)
            {
                case null:
                    return "None";
                case Category.BILLS:
                    return "Bills";
                case Category.DEBT:
                    return "Debt";
                case Category.LIVING:
                    return "Living";
                case Category.CHILD:
                    return "Childcare";
                case Category.OVERFLOW:
                    return "Overflow";
                case Category.NONE:
                    return "None";
                default:
                    return "Other";
            }
        }

        private Category StringToCategory(string category)
        {
            switch(category)
            {
                case "":
                    return Category.NONE;
                case "n":
                    return Category.NONE;
                case "b":
                    return Category.BILLS;
                case "d":
                    return Category.DEBT;
                case "c":
                    return Category.CHILD;
                case "f":
                    return Category.OVERFLOW;
                default:
                    return Category.OTHER;
            }
        }

        private Status StringToStatus(string status)
        {
            switch(status)
            {
                case "c":
                    return Status.CLEARED;
                case "i":
                    return Status.IGNORED;
                case "p":
                    return Status.PENDING;
                default:
                    return Status.NONE;
            }
        }

        private string Ask(string prompt, bool lowerCase = false)
        {
            Console.Write(prompt);
            string? value = Console.ReadLine();
            value = value == null ? "" : value.Trim();
            return lowerCase ? value.ToLower() : value;
        }

        private string PrintableTotalsHeaders()
        {
            string line = string.Format(
                "{0} {1} {2}",
                Pad("Name", (int) COLUMNS.INDEX + (int) COLUMNS.STATUS + (int) COLUMNS.NAME + (int) COLUMNS.CATEGORY),
                Pad("Amount", (int) COLUMNS.AMOUNT),
                Pad("Remaining", (int) COLUMNS.REMAINING)
            );

            return line;
        }

        private void RenderTotals()
        {
            Tally tally = Totals();

            string income = string.Format(
                "{0} ${1} ${2}",
                Pad("Income", (int)COLUMNS.INDEX + (int)COLUMNS.STATUS + (int)COLUMNS.NAME + (int)COLUMNS.CATEGORY),
                Pad(FormatMoney(tally.incomeAmountTotal), (int)COLUMNS.AMOUNT),
                Pad(FormatMoney(tally.incomeRemainingTotal), (int) COLUMNS.REMAINING)
            );

            string expenses = string.Format(
                "{0} ${1} ${2}",
                Pad("Expenses", (int)COLUMNS.INDEX + (int)COLUMNS.STATUS + (int)COLUMNS.NAME + (int)COLUMNS.CATEGORY),
                Pad(FormatMoney(tally.expenseAmountTotal), (int)COLUMNS.AMOUNT),
                Pad(FormatMoney(tally.expenseRemainingTotal), (int)COLUMNS.REMAINING)
            );

            string balance = string.Format(
                "{0} ${1} ${2}",
                Pad("Balance", (int)COLUMNS.INDEX + (int)COLUMNS.STATUS + (int)COLUMNS.NAME + (int)COLUMNS.CATEGORY),
                Pad(FormatMoney(tally.balanceTotal), (int)COLUMNS.AMOUNT),
                Pad(FormatMoney(tally.balanceRemaining), (int)COLUMNS.REMAINING)
            );

            RenderLine(income);
            RenderLine(expenses);
            RenderLine(balance);
        }

        private string PrintableHeaders()
        {
            string line = string.Format(
                "{0} {1} {2} {3} {4} {5}",
                Pad("Index", (int) COLUMNS.INDEX),
                Pad("Status", (int) COLUMNS.STATUS),
                Pad("Name", (int) COLUMNS.NAME),
                Pad("Category", (int) COLUMNS.CATEGORY),
                Pad("Amount", (int) COLUMNS.AMOUNT),
                Pad("Remaining", (int) COLUMNS.REMAINING)
            );

            return line;
        }

        private string PrintableLineItem(LineItem i)
        {
            string index = string.Format("({0})", items.IndexOf(i));
            string status;
            switch(i.Status)
            {
                case Status.CLEARED:
                    status = "[X]";
                    break;
                case Status.PENDING:
                    status = "[P]";
                    break;
                case Status.IGNORED:
                    status = "[-]";
                    break;
                default:
                    status = "[ ]";
                    break;
            }

            string line = string.Format(
                "{0} {1} {2} {3} ${4} ${5}",
                Pad(index, (int) COLUMNS.INDEX),
                Pad(status, (int) COLUMNS.STATUS),
                Pad(i.Name, (int) COLUMNS.NAME),
                Pad(CategoryToString(i.Category), (int) COLUMNS.CATEGORY),
                Pad(FormatMoney(Math.Ceiling(i.Amount)), (int) COLUMNS.AMOUNT),
                Pad(FormatMoney(Math.Ceiling(i.Remaining)), (int) COLUMNS.REMAINING)
            );

            return line;
        }

        private string GetLine(char c, double percent = 1.0)
        {
            string line = "";
            for(int i=0; i < Math.Ceiling(Console.WindowWidth * percent); i++)
            {
                line += c;
            }

            return line;
        }

        private void Clear()
        {
            Console.Clear();
        }

        private void RenderMenu()
        {
            RenderLine("Options: (A)dd (E)dit, (D)elete, S(t)atus, (R)emove All, (N)ew, (L)oad, (S)ave, (Q)uit");
        }

        private void RenderLine(string line)
        {
            Console.WriteLine(line);
        }

        private void RenderBlank()
        {
            Console.WriteLine("");
        }

        private void RenderCenteredLine(string line)
        {
            string centeredLine = "";
            int leftpad = Console.WindowWidth / 2 - line.Length / 2;
            for(int i=0; i<leftpad; i++)
            {
                centeredLine += " ";
            }
            centeredLine += line;
            RenderLine(centeredLine);
        }

        private string Pad(string value, int length)
        {
            return value.PadRight(length).Substring(0, length);
        }

        private string FormatMoney(double value)
        {
            return value.ToString("0,0.00", new CultureInfo("en-US", false));
        }
    }
}
