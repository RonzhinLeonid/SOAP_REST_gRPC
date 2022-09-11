using LibraryService.Models;
using System.Collections.Generic;

namespace LibraryService.Services
{
    public interface ILibraryDatabaseContextService
    {
        IList<Book> Books { get; }
    }
}
