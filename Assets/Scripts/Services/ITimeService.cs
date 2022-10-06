namespace CyberNinja.Services
{
    public interface ITimeService
    {
        public float Time { get; set; }
        public float DeltaTime { get; set; }
        public float UnscaledDeltaTime { get; set; }
        public float UnscaledTime { get; set; }
    }
}