namespace RestApiApp.Dtos
{
    public class EmailContent
    {
        public string EmailRecipient { get; set; }
        public string VerifyLink { get; set; }
        public DateTime JoinDate { get; set; }
    }
}