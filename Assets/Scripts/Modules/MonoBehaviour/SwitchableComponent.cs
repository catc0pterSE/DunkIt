namespace Modules.MonoBehaviour
{
    using MonoBehaviour = UnityEngine.MonoBehaviour;
    public abstract class SwitchableComponent: MonoBehaviour, ISwitchable
    {
        public virtual void Enable()
        {
            enabled = true;
        }

        public virtual void Disable()
        {
            enabled = false;
        }
    }
}