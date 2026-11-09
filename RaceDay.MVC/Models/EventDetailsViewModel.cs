namespace RaceDay.MVC.Models
{
    public class EventDetailsViewModel
    {
        public string EventName { get; set; } = string.Empty;
        public List<ParticipantEnrolmentViewModel> Enrolments { get; set; } = new List<ParticipantEnrolmentViewModel>();
    }

    public class ParticipantEnrolmentViewModel
    {
        public int EnrolmentId { get; set; }
        public string ParticipantName { get; set; } = string.Empty;
        public string ParticipantEmail { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public DateTime EnrolmentDate { get; set; }
    }
}