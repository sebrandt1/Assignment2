using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Assignment_2
{
    public class Expense
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public static List<Expense> TotalExpenses = new List<Expense>();
        
        //Constructor for our Expense class where we set our instance properties and add the created instance to the static total list
        public Expense(string name, string category, decimal price)
        {
            Name = name;
            Category = category;
            Price = price;
            TotalExpenses.Add(this);
        }

        /// <summary>
        /// If argument #2 is empty on call, we want to return the total sum of ALL expenses.
        /// If argument #2 contains a specified category we want to return the sum of all expenses of that single category.
        /// </summary>
        public static decimal SumExpenses(List<Expense> expenses, string category = null)
        {
            decimal sum = 0;

            if(category != null)
            {
                for(int i = 0; i < expenses.Count; i++)
                {
                    if(expenses[i].Category == category)
                    {
                        sum += expenses[i].Price;
                    }
                }
            }
            else
            {
                for (int i = 0; i < expenses.Count; i++)
                {
                    sum += expenses[i].Price;
                }
            }
            return sum;
        }
        
        /// <summary>
        /// If argument is empty we should clear all expenses from our list.
        /// If argument contains an expense instance we should removed that specified instance alone from our total list.
        /// </summary>
        public static void RemoveExpense(Expense toRemove = null)
        {
            if (toRemove == null)
            {
                TotalExpenses.Clear();
                Console.Clear();
                Console.WriteLine("All entries were removed.");
                return;
            }

            TotalExpenses.Remove(toRemove);
            Console.Clear();
            Console.WriteLine($"{toRemove.Name} was removed from expenses and {toRemove.Price} was subtracted from total sum.");
        }

        /// <summary>
        /// Create and print our menu for adding an expense to our total list.
        /// </summary>
        public static void AddExpenseMenu()
        {
            string[] expenseMenu =
            {
                "Food",
                "Entertainment",
                "Other"
            };
            decimal price = -1;

            Console.Clear();

            Console.WriteLine("[Add Expense]");
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Price: ");

            decimal.TryParse(Console.ReadLine(), out price);

            //We declared price as -1, if price is still -1 by this point we can assume that our TryParse failed for whatever reason
            //Since an expense cannot be a negative value (that would be an income) we want to return here and skip the rest of the method
            if (price == -1) return;

            string category = CategoryMenu();

            Expense exp = new Expense(name, category, price);
            Console.Clear();
            Console.WriteLine($"Added expense: {name} with price {price} and category {category}");
        }

        /// <summary>
        /// Create and print our Category selection menu and return the selected value as string (so we know what category was selected).
        /// </summary>
        public static string CategoryMenu()
        {
            int selected = Program.ShowMenu("Category: ", new string[]
                {
                    "Food",
                    "Entertainment",
                    "Other"
                });

            switch(selected)
            {
                case 0:
                    return "Food";

                case 1:
                    return "Entertainment";

                case 2:
                    return "Other";

                    //There are only 3 possible values since we only input 3 indexes
                    //But if for some reason some kind of programming voodoo happens we want to default it to something
                default:
                    return "Invalid Option.";
            }
        }
        
        /// <summary>
        /// If no argument is passed, all expenses will be printed.
        /// </summary>
        public static Expense[] ShowExpenses(string category = null)
        {
            List<Expense> items = new List<Expense>();

            Console.Clear();
            void PrintExpense(Expense expense)
            {
                Console.WriteLine($"[Category: {expense.Category}]");
                Console.WriteLine($"[Name: {expense.Name}, Price: {expense.Price}]");
                Console.WriteLine();
                //items.Add($"[{expense.Name}, {expense.Price}, {expense.Category}]");
                items.Add(expense);
            }

            if (category != null || TotalExpenses.Count == 0) //if count is = 0 we want to print that there are no expenses
            {                                                 //this will happen after sum of 0 expense objects is calculated below the for loop
                for (int i = 0; i < TotalExpenses.Count; i++)
                {
                    if (TotalExpenses[i].Category == category)
                    {
                        PrintExpense(TotalExpenses[i]);
                    }
                }

                decimal sum = SumExpenses(TotalExpenses, category);
                string message = sum != 0 ? $"Total sum of expenses for category: {category}: {sum}" : "You have no expenses.";
                Console.WriteLine(message);
            }
            else
            {
                for (int i = 0; i < TotalExpenses.Count; i++)
                {
                    PrintExpense(TotalExpenses[i]);
                }
                Console.WriteLine($"Total expenses: {SumExpenses(TotalExpenses, category)}");
            }
            return items.ToArray();
        }

        public static void ExpenseMenu()
        {
            Console.Clear();
            Expense[] exp = ShowExpenses();
            //Initialize a string array of the same size as our expense array
            string[] items = new string[exp.Length];

            for(int i = 0; i < exp.Length; i++)
            {
                //Create a string in the items array at the equivalent index of the expense
                //So we can use this in our ShowMenu call (since it only accepts string arrays as parameter).
                items[i] = ($"[Name: {exp[i].Name}, [Price: {exp[i].Price}]");
            }

            int selected = Program.ShowMenu("Select an item to remove.", items);

            RemoveExpense(exp[selected]);
        }
    }

    public class Program
    {
        private static bool runMenu = true;

        public static void Main()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            while (runMenu)
            {
                int selected = ShowMenu("Navigate up and down with the arrow keys and select by pressing enter.", new string[]
                    {
                    "[Add Expense]",
                    "[Show All Expenses]",
                    "[Show Sum by Category]",
                    "[Remove Expense]",
                    "[Remove All Expenses]",
                    "[Exit]",
                    });


                switch (selected)
                {
                    case 0:
                        Expense.AddExpenseMenu();
                        break;

                    case 1:
                        Expense.ShowExpenses();
                        break;

                    case 2:
                        string category = Expense.CategoryMenu();
                        Expense.ShowExpenses(category);
                        break;

                    case 3:
                        Expense.ExpenseMenu();
                        break;

                    case 4:
                        Expense.RemoveExpense();
                        break;

                    case 5:
                        runMenu = false;
                        break;

                    default:
                        break;
                }

            }
            Console.ReadKey();
            Console.WriteLine("Hello!");
        }

        // Don't change this method.
        public static int ShowMenu(string prompt, string[] options)
        {
            if (options == null || options.Length == 0)
            {
                throw new ArgumentException("Cannot show a menu for an empty array of options.");
            }

            Console.WriteLine(prompt);

            int selected = 0;

            // Hide the cursor that will blink after calling ReadKey.
            Console.CursorVisible = false;

            ConsoleKey? key = null;
            while (key != ConsoleKey.Enter)
            {
                // If this is not the first iteration, move the cursor to the first line of the menu.
                if (key != null)
                {
                    Console.CursorLeft = 0;
                    Console.CursorTop = Console.CursorTop - options.Length;
                }

                // Print all the options, highlighting the selected one.
                for (int i = 0; i < options.Length; i++)
                {
                    var option = options[i];
                    if (i == selected)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine("- " + option);
                    Console.ResetColor();
                }

                // Read another key and adjust the selected value before looping to repeat all of this.
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.DownArrow)
                {
                    selected = Math.Min(selected + 1, options.Length - 1);
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    selected = Math.Max(selected - 1, 0);
                }
            }

            // Reset the cursor and return the selected option.
            Console.CursorVisible = true;
            return selected;
        }
    }

        [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void ExampleTest()
        {
            FakeConsole console = new FakeConsole("");
            Program.Main();
            Assert.AreEqual("Hello!", console.Output);
            console.Dispose();
        }
    }
}
