namespace RaceDay.MVC.Models
{
    public class EnrolmentViewModel
    {
        public int EnrolmentId { get; set; }
        public DateTime EnrolmentDate { get; set; }
        public EventViewModel Event { get; set; } = new EventViewModel();
        public CategoryViewModel Category { get; set; } = new CategoryViewModel();
    }

    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}