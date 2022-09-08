using LibraryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryService.Services.Impl
{
    public class LibraryRepository : ILibraryRepositoryService, IRepository<Book, string>
    {
        private readonly ILibraryDatabaseContextService _dbContext;


        public LibraryRepository(ILibraryDatabaseContextService dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<Book> GetByAuthor(string authorName)
        {
            return _dbContext.Books.Where(book =>
                book.Authors.Where(author =>
                    author.Name.ToUpper().Contains(authorName.ToUpper())).Count() > 0).ToList();
        }

        public IList<Book> GetByCategory(string category)
        {
            try
            {
                return _dbContext.Books.Where(book =>
                    book.Category.ToUpper().Contains(category.ToUpper())).ToList();
            }
            catch (Exception e)
            {
                return new List<Book>();
            }
        }

        public IList<Book> GetByTitle(string title)
        {
            try
            {
                return _dbContext.Books.Where(book =>
                    book.Title.ToUpper().Contains(title.ToUpper())).ToList();
            }
            catch (Exception e)
            {
                return new List<Book>();
            }
        }

        public string Add(Book item)
        {
            throw new NotImplementedException();
        }

        public int Delete(Book item)
        {
            throw new NotImplementedException();
        }

        public IList<Book> GetAll()
        {
            try
            {
                return _dbContext.Books;
            }
            catch (Exception e)
            {
                return new List<Book>();
            }
        }

        public Book GetById(string id)
        {
            try
            {
                return _dbContext.Books.Where(book =>
                    book.Id.ToUpper().Contains(id.ToUpper())).FirstOrDefault();
            }
            catch (Exception e)
            {
                return new Book();
            }
        }

        public int Update(Book item)
        {
            throw new NotImplementedException();
        }
    }
}