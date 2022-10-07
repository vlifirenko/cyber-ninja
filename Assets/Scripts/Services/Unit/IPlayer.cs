namespace CyberNinja.Services.Unit
{
    public interface IPlayer
    {
        public bool IsPlayer(int entity);

        public int GetPlayerEntity();
    }
}