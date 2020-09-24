using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/*
    Skriv ett interaktivt konsolprogram som låter användaren hålla koll på sina utgifter (expenses). Programmet ska innehålla följande funktionalitet:

    Lägga till utgifter
    Varje utgift består av namn, pris och kategori (Food, Entertainment eller Other)
    Visa alla utgifter och totalsumman av dessa utgifter
    Visa totalsumman av alla utgifter per kategori
    Ta bort en specifik utgift
    Ta bort alla utgifter

 */
namespace Assignment_2
{
    public class Expense
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public static List<Expense> expenses = new List<Expense>();


        //Constructor for our Expense class where we set our instance properties
        public Expense(string name, string category, decimal price)
        {
            Name = name;
            Category = category;
            Price = price;
            expenses.Add(this);
        }

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
        
        public static void RemoveExpense(Expense toRemove = null)
        {
            if (toRemove == null)
            {
                expenses.Clear();
                return;
            }

            expenses.Remove(toRemove);
        }

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

            if (price == -1) return;

            string category = CategoryMenu();

            Expense exp = new Expense(name, category, price);
            Console.Clear();
            Console.WriteLine($"Added expense: {name} with price {price} and category {category}");
        }

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

                default:
                    return "Invalid Option.";
            }
        }
        
        /// <summary>
        /// If no argument is passed, all expenses will be printed.
        /// </summary>
        public static string[] ShowExpenses(string category = null)
        {
            List<string> items = new List<string>();

            Console.Clear();
            void Print(Expense expense)
            {
                Console.WriteLine($"[Category: {expense.Category}]");
                Console.WriteLine($"[Name: {expense.Name}, [Price: {expense.Price}]");
                Console.WriteLine();
                items.Add($"[{expense.Name}, {expense.Price}, {expense.Category}]");
            }

            if (category != null)
            {
                for (int i = 0; i < expenses.Count; i++)
                {
                    if (expenses[i].Category == category)
                    {
                        Print(expenses[i]);
                    }
                }
                Console.WriteLine(SumExpenses(expenses, category));
            }
            else
            {
                for (int i = 0; i < expenses.Count; i++)
                {
                    Print(expenses[i]);
                }
                Console.WriteLine(SumExpenses(expenses, category));
            }
            return items.ToArray();
        }

        public static string ExpenseMenu()
        {
            Console.Clear();
            string[] exp = ShowExpenses();
            int selected = Program.ShowMenu("Select an item to remove.", ShowExpenses());

            

            return string.Empty;
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
                        Expense.RemoveExpense();
                        break;

                    case 4:
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
