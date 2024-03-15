namespace FreedomLogic.Infrastructure
{
    public class AppConfiguration
    {
        public LinksConfiguration Links { get; set; }
        public TrinityCoreConfiguration TrinityCore { get; set; }   
        public DbogConfiguration Dbo { get; set; }
        public SmtpConfiguration Smtp { get; set; }
    }

    public class LinksConfiguration
    {
        public string ForumUrl { get; set; }
        public string WikiUrl { get; set; }
        public string ConnectionGuideUrl { get; set; }
    }

    public class TrinityCoreConfiguration
    {
        public int ProcessStartSessionId { get; set; }
        public int RealmId { get; set; }
        public string WorldServerPidFilename { get; set; }
        public string BnetServerPidFilename { get; set; }
        public string ServerDir { get; set; }
        public string SoapAddress { get; set; }
        public string SoapPort { get; set; }
        public string SoapUser { get; set; }
        public string SoapPassword { get; set; } 
    }

    public class SmtpConfiguration
    {
        public string FromAddress { get; set; }
        public string DisplayName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

    }

    public class DbogConfiguration
    {
        public string RootPath { get; set; }
        public string MasterPidFileName { get; set; }
        public string QueryPidFileName { get; set; }
        public string CharPidFileName { get; set; }
        public string ChatPidFileName { get; set; }
        public string GamePidFileName { get; set; }
        public string AuthPidFileName { get; set; }
    }
}
