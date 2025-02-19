using InmindLab3LibraryDB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InmindLab3LibraryDB.Data
{
    public static class DbInitializer
    {
        public static void Initialize(LibraryContext context)
        {
            context.Database.EnsureCreated();

            if (context.Authors.Any())
                return;

            // Authors
            var authors = new[]
            {
                new Author { Name = "Rowling", BirthDate = new DateTime(1965, 7, 31), Country = "UK" },
                new Author { Name = "George", BirthDate = new DateTime(1948, 9, 20), Country = "USA" }
            };
            context.Authors.AddRange(authors);
            context.SaveChanges();

            // Books (use AuthorId with exact casing from your model)
            var books = new[]
            {
                new Book { 
                    Title = "Harry Potter", 
                    AuthorId = authors[0].AuthorId, // Match property casing
                    ISBN = "9780747532699", 
                    PublishedYear = 1997 
                },
                new Book { 
                    Title = "A Game of Thrones", 
                    AuthorId = authors[1].AuthorId, // Match property casing
                    ISBN = "9780553103540", 
                    PublishedYear = 1996 
                }
            };
            context.Books.AddRange(books);
            context.SaveChanges();

            // Borrowers (don't set borrower_id explicitly)
            var borrowers = new[]
            {
                new Borrowers { 
                    name = "John", 
                    email = "john@example.com", 
                    phone = 1234567890 
                },
                new Borrowers { 
                    name = "Smith", 
                    email = "smith@example.com", 
                    phone = 987654321 
                }
            };
            context.Borrowers.AddRange(borrowers);
            context.SaveChanges();

            // Loans
            var loans = new[]
            {
                new Loan { 
                    BookId = books[0].BookId, 
                    BorrowerId = borrowers[0].borrower_id, 
                    LoanDate = DateTime.Now.AddDays(-10), 
                    ReturnDate = DateTime.Now.AddDays(20), 
                    IsReturned = false 
                },
                new Loan { 
                    BookId = books[1].BookId, 
                    BorrowerId = borrowers[1].borrower_id, 
                    LoanDate = DateTime.Now.AddDays(-5), 
                    ReturnDate = DateTime.Now.AddDays(25), 
                    IsReturned = false 
                }
            };
            context.Loans.AddRange(loans);
            context.SaveChanges();
        }
    }
}