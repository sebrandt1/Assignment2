using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Assignment_2
{
    public class Expense
    {
        public string Name { get; private set; }
        public Categories Category { get; private set; }
        public decimal Price { get; private set; }
        public static List<Expense> TotalExpenses = new List<Expense>();
        public enum Categories
        {
            Food,
            Entertainment,
            Other,
            NONE
        }

        //Constructor for our Expense class where we set our instance properties
        public Expense(string name, Categories category, decimal price)
        {
            if (!string.IsNullOrEmpty(name) && category != Categories.NONE && price > 0)
            {
                Name = name;
                Category = category;
                Price = price;
                TotalExpenses.Add(this);
                return;
            }
            Console.WriteLine("One of more values were empty when trying to create expense, try again.");
        }

        /// <summary>
        /// If argument #2 is empty on call, we want to return the total sum of ALL expenses.
        /// If argument #2 contains a specified category we want to return the sum of all expenses of that single category.
        /// </summary>
        public static decimal SumExpenses(List<Expense> expenses, Categories category = Categories.NONE)
        {
            decimal sum = 0;

            if (category != Categories.NONE)
            {
                for (int i = 0; i < expenses.Count; i++)
                {
                    if (expenses[i].Category == category)
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

            //Since an expense cannot be a negative value (that would be an income) we want to return here and skip the rest of the method if its negative
            if (price < 0)
            {
                Console.WriteLine($"Price was {price}, will not add to list.");
                return;
            }

            Categories category = CategoryMenu();

            Console.Clear();
            new Expense(name, category, price);
        }

        /// <summary>
        /// Create and print our Category selection menu and return the selected value as string (so we know what category was selected).
        /// </summary>
        public static Categories CategoryMenu()
        {
            int selected = Program.ShowMenu("Category: ", new string[]
                {
                    "Food",
                    "Entertainment",
                    "Other"
                });

            switch (selected)
            {
                case 0:
                    return Categories.Food;

                case 1:
                    return Categories.Entertainment;

                case 2:
                    return Categories.Other;

                //There are only 3 possible values since we only input 3 indexes
                //But if for some reason some kind of programming voodoo happens we want to default it to something
                default:
                    return Categories.NONE;
            }
        }

        /// <summary>
        /// If no argument is passed, all expenses will be printed.
        /// </summary>
        public static void ShowExpenses(Categories category = Categories.NONE)
        {
            Console.Clear();

            void PrintExpense(Expense expense)
            {
                Console.WriteLine($"[Category: {expense.Category}]");
                Console.WriteLine($"[Name: {expense.Name}, Price: {expense.Price}]");
                Console.WriteLine();
                //items.Add($"[{expense.Name}, {expense.Price}, {expense.Category}]");
            }

            if (category != Categories.NONE || TotalExpenses.Count == 0) //if count is = 0 we want to print that there are no expenses
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
        }

        public static void ExpenseMenu()
        {
            Console.Clear();
            //If total expenses count is less than 1 we want to return here so we dont pass an empty string array into the showmenu which will cause the program to crash
            if (TotalExpenses.Count < 1)
            {
                Console.WriteLine("You have no expenses to remove!");
                return;
            }

            //Initialize a string array of the same size as our expense array
            string[] items = new string[TotalExpenses.Count];

            for (int i = 0; i < items.Length; i++)
            {
                //Create a string in the items array at the equivalent index of the expense
                //So we can use this in our ShowMenu call (since it only accepts string arrays as parameter).
                items[i] = ($"[Name: {TotalExpenses[i].Name}, [Price: {TotalExpenses[i].Price}]");
            }

            int selected = Program.ShowMenu("Select an item to remove.", items);
            RemoveExpense(TotalExpenses[selected]);
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
                        Expense.Categories category = Expense.CategoryMenu();
                        Expense.ShowExpenses(category);
                        break;

                    case 3:
                        Expense.ExpenseMenu();
                        break;

                    case 4:
                        Expense.RemoveExpense();
                        break;

                    case 5:
                        Console.WriteLine("Exiting.");
                        runMenu = false;
                        break;

                    default:
                        throw new Exception("Invalid menu option selected.");
                }
            }
            Console.ReadKey();
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
        public void SumAllExpenses()
        {
            new Expense("Apple", Expense.Categories.Food, 5.90M);
            new Expense("Tv", Expense.Categories.Entertainment, 2499.90M);
            new Expense("Car insurance", Expense.Categories.Other, 1700M);
            decimal sum = Expense.SumExpenses(Expense.TotalExpenses);

            Assert.AreEqual(4205.80M, sum);
            Expense.TotalExpenses.Clear();
        }

        [TestMethod]
        public void SingleCategory()
        {
            new Expense("Apple", Expense.Categories.Food, 5.90M);
            new Expense("Tv", Expense.Categories.Entertainment, 2499.90M);
            new Expense("Netflix subscription", Expense.Categories.Entertainment, 159M);
            new Expense("Car insurance", Expense.Categories.Other, 1700M);
            Expense.Categories category = Expense.Categories.Entertainment;
            decimal sum = Expense.SumExpenses(Expense.TotalExpenses, category);

            Assert.AreEqual(2658.90M, sum);
            Expense.TotalExpenses.Clear();
        }
        [TestMethod]
        public void SingleIncorrectDeclare()
        {
            // If price is 0 or lower the product does not get added to the Expense list
            new Expense("Apple", Expense.Categories.Food, -1700);
            new Expense("Tv", Expense.Categories.Entertainment, 2499.90M);
            new Expense("Car insurance", Expense.Categories.Other, 1700M);
            decimal sum = Expense.SumExpenses(Expense.TotalExpenses);

            Assert.AreEqual(4199.90M, sum);
            Expense.TotalExpenses.Clear();
        }
        [TestMethod]
        public void ZeroExpenses()
        {
            new Expense("T-shirt", Expense.Categories.Other, 0);
            new Expense("Tv", Expense.Categories.Entertainment, 0);
            new Expense("Car insurance", Expense.Categories.Other, 0);
            new Expense("Cellphone", Expense.Categories.Other, 0);
            decimal sum = Expense.SumExpenses(Expense.TotalExpenses);

            Assert.AreEqual(0, sum);
            Expense.TotalExpenses.Clear();
        }
        [TestMethod]
        public void AddSameExpense()
        {
            Expense.Categories category = Expense.Categories.Other;
            new Expense("T-shirt", category, 249.90M);
            new Expense("T-shirt", category, 249.90M);
            new Expense("Car insurance", category, 1800M);
            new Expense("Cellphone", category, 4799.90M);
            decimal sum = Expense.SumExpenses(Expense.TotalExpenses);

            Assert.AreEqual(7099.70M, sum);
            Expense.TotalExpenses.Clear();
        }
        [TestMethod]
        public void MultipleNegativeValues()
        {
            new Expense("Exhaust pipe", Expense.Categories.Other, -999.90M);
            new Expense("Dirt bike", Expense.Categories.Entertainment, -14900.00M);
            new Expense("Tentipi Onyx 9", Expense.Categories.Other, -12399.00M);
            new Expense("Canned soup", Expense.Categories.Food, -49.90M);
            decimal sum = Expense.SumExpenses(Expense.TotalExpenses);

            Assert.AreEqual(0, sum);
            Expense.TotalExpenses.Clear();
        }
        [TestMethod]
        public void TestAllCategories()
        {
            new Expense("Exhaust pipe", Expense.Categories.Other, 999.90M);
            new Expense("Dirt bike", Expense.Categories.Entertainment, 14900.00M);
            new Expense("Tentipi Onyx 9", Expense.Categories.Other, 12399.00M);
            new Expense("Canned soup", Expense.Categories.Food, 49.90M);
            new Expense("Canned tuna", Expense.Categories.Food, 15.90M);
            new Expense("Movie tickets", Expense.Categories.Entertainment, 250.00M);

            Expense.Categories categoryOther = Expense.Categories.Other;
            decimal sumOther = Expense.SumExpenses(Expense.TotalExpenses, categoryOther);
            Assert.AreEqual(13398.90M, sumOther);

            Expense.Categories categoryFood = Expense.Categories.Food;
            decimal sumFood = Expense.SumExpenses(Expense.TotalExpenses, categoryFood);
            Assert.AreEqual(65.80M, sumFood);

            Expense.Categories categoryEntertainment = Expense.Categories.Entertainment;
            decimal sumEntertainment = Expense.SumExpenses(Expense.TotalExpenses, categoryEntertainment);
            Assert.AreEqual(15150M, sumEntertainment);

            decimal sum = Expense.SumExpenses(Expense.TotalExpenses);
            Assert.AreEqual(28614.70M, sum);

            Expense.TotalExpenses.Clear();
        }
    }
}
