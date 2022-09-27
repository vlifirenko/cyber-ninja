using CyberNinja.Models.Enums;

namespace CyberNinja.Services
{
    public interface IAiService : IDestroyable
    {
        public void InitUnit(int entity);

        public void ReplaceAiTask(int entity, EAiTaskType task);

        public void TryAttack(int unitEntity);
    }
}