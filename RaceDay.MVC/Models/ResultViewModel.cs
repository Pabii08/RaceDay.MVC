namespace RaceDay.MVC.Models
{
    public class ResultViewModel
    {
        public int ResultId { get; set; }
        public string Time { get; set; } = string.Empty;
        public int Position { get; set; }
        public DateTime ResultDate { get; set; }
        public EnrolmentDetailsViewModel Enrolment { get; set; } = new EnrolmentDetailsViewModel();
    }

    public class EnrolmentDetailsViewModel
    {
        public EventViewModel Event { get; set; } = new EventViewModel();
        public CategoryViewModel Category { get; set; } = new CategoryViewModel();
    }
}