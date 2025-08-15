using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{

    //from conflict branch
    class Progra
    {
        public abstract class LibraryItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public bool IsBorrowed { get; private set; }

            public void Borrow()
            {
                if (IsBorrowed)
                    throw new InvalidOperationException("Item is already borrowed.");
                IsBorrowed = true;
            }

            public void Return()
            {
                if (!IsBorrowed)
                    throw new InvalidOperationException("Item was not borrowed.");
                IsBorrowed = false;
            }

            public abstract string GetInfo();
        }

        // Derived class for Books
        public class Book : LibraryItem
        {
            public string Author { get; set; }

            public override string GetInfo()
            {
                return $"[Book] {Id}: {Title} by {Author} - {(IsBorrowed ? "Borrowed" : "Available")}";
            }
        }

        // Derived class for Magazines
        public class Magazine : LibraryItem
        {
            public int IssueNumber { get; set; }

            public override string GetInfo()
            {
                return $"[Magazine] {Id}: {Title}, Issue #{IssueNumber} - {(IsBorrowed ? "Borrowed" : "Available")}";
            }
        }

        // Library Manager
        public class Library
        {
            private List<LibraryItem> items = new List<LibraryItem>();
            private const string DataFile = "library_data.txt";

            public void AddItem(LibraryItem item)
            {
                items.Add(item);
                SaveToFile();
            }

            public void DisplayItems()
            {
                if (!items.Any())
                {
                    Console.WriteLine("No items in the library.");
                    return;
                }

                foreach (var item in items)
                {
                    Console.WriteLine(item.GetInfo());
                }
            }

            public void BorrowItem(int id)
            {
                var item = items.FirstOrDefault(i => i.Id == id);
                if (item == null) throw new ArgumentException("Item not found.");

                item.Borrow();
                SaveToFile();
            }

            public void ReturnItem(int id)
            {
                var item = items.FirstOrDefault(i => i.Id == id);
                if (item == null) throw new ArgumentException("Item not found.");

                item.Return();
                SaveToFile();
            }

            public void LoadFromFile()
            {
                if (!File.Exists(DataFile)) return;

                var lines = File.ReadAllLines(DataFile);
                items.Clear();

                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length < 5) continue;

                    string type = parts[0];
                    int id = int.Parse(parts[1]);
                    string title = parts[2];
                    bool isBorrowed = bool.Parse(parts[3]);

                    if (type == "Book")
                    {
                        var book = new Book
                        {
                            Id = id,
                            Title = title,
                            Author = parts[4]
                        };
                        if (isBorrowed) book.Borrow();
                        items.Add(book);
                    }
                    else if (type == "Magazine")
                    {
                        var mag = new Magazine
                        {
                            Id = id,
                            Title = title,
                            IssueNumber = int.Parse(parts[4])
                        };
                        if (isBorrowed) mag.Borrow();
                        items.Add(mag);
                    }
                }
            }

            private void SaveToFile()
            {
                var lines = new List<string>();
                foreach (var item in items)
                {
                    if (item is Book b)
                    {
                        lines.Add($"Book|{b.Id}|{b.Title}|{b.IsBorrowed}|{b.Author}");
                    }
                    else if (item is Magazine m)
                    {
                        lines.Add($"Magazine|{m.Id}|{m.Title}|{m.IsBorrowed}|{m.IssueNumber}");
                    }
                }
                File.WriteAllLines(DataFile, lines);
            }
        }

        // Main Program
        class Program
        {
            static void Main()
            {
                Library library = new Library();
                library.LoadFromFile();

                while (true)
                {
                    Console.WriteLine("\n--- Library Menu ---");
                    Console.WriteLine("1. Add Book");
                    Console.WriteLine("2. Add Magazine");
                    Console.WriteLine("3. Display Items");
                    Console.WriteLine("4. Borrow Item");
                    Console.WriteLine("5. Return Item");
                    Console.WriteLine("6. Exit");
                    Console.Write("Enter choice: ");

                    string choice = Console.ReadLine();

                    try
                    {
                        switch (choice)
                        {
                            case "1":
                                Console.Write("Enter ID: ");
                                int bookId = int.Parse(Console.ReadLine());
                                Console.Write("Enter Title: ");
                                string bookTitle = Console.ReadLine();
                                Console.Write("Enter Author: ");
                                string author = Console.ReadLine();

                                library.AddItem(new Book
                                {
                                    Id = bookId,
                                    Title = bookTitle,
                                    Author = author
                                });
                                Console.WriteLine("Book added.");
                                break;

                            case "2":
                                Console.Write("Enter ID: ");
                                int magId = int.Parse(Console.ReadLine());
                                Console.Write("Enter Title: ");
                                string magTitle = Console.ReadLine();
                                Console.Write("Enter Issue Number: ");
                                int issue = int.Parse(Console.ReadLine());

                                library.AddItem(new Magazine
                                {
                                    Id = magId,
                                    Title = magTitle,
                                    IssueNumber = issue
                                });
                                Console.WriteLine("Magazine added.");
                                break;

                            case "3":
                                library.DisplayItems();
                                break;

                            case "4":
                                Console.Write("Enter ID to borrow: ");
                                int borrowId = int.Parse(Console.ReadLine());
                                library.BorrowItem(borrowId);
                                Console.WriteLine("Item borrowed.");
                                break;

                            case "5":
                                Console.Write("Enter ID to return: ");
                                int returnId = int.Parse(Console.ReadLine());
                                library.ReturnItem(returnId);
                                Console.WriteLine("Item returned.");
                                break;

                            case "6":
                                return;

                            default:
                                Console.WriteLine("Invalid choice. Try again.");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
        }
    }
}
