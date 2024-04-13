namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class NotificationViewModel
    {
        public string Message { get; set; }
        public string Date { get; set; }
        public int NotificationId { get; set; }
        public bool IsRead { get; set; }
        public string SenderName { get; set; }
        public NotificationViewModel()
        {
        }
        public NotificationViewModel(string message, string date)
        {
            Message = message;
            Date = date;
        }
    }
}