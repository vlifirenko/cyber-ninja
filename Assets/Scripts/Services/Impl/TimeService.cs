namespace CyberNinja.Services.Impl
{
    public class TimeService : ITimeService
    {
        public float Time { get; set; }
        public float DeltaTime { get; set; }
        public float UnscaledDeltaTime { get; set; }
        public float UnscaledTime { get; set; }
    }
}