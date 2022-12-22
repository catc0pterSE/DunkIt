namespace Modules.MonoBehaviour
{
    using MonoBehaviour = UnityEngine.MonoBehaviour;
    public abstract class SwitchableComponent: MonoBehaviour
    {
        public void Enable()
        {
            enabled = true;
        }

        public void Disable()
        {
            enabled = false;
        }
    }
}