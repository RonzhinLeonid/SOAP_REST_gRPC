using System.ComponentModel.DataAnnotations;

namespace LibraryService.Web.Models
{
    public enum SearchType
    {
        [Display(Name = "Идентификатор")]
        Id,
        [Display(Name = "Заголовок")]
        Title,
        [Display(Name = "Автор")]
        Author,
        [Display(Name = "Категория")]
        Category,
        [Display(Name = "Все книги")]
        All
    }
}
