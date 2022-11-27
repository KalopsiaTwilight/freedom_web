namespace FreedomLogic.Infrastructure
{
    public class AppConfiguration
    {
        public LinksConfiguration Links { get; set; }
        public TrinityCoreConfiguration TrinityCore { get; set; }
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
    }
}
