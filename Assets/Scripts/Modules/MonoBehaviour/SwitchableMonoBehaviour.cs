namespace Modules.MonoBehaviour
{
    public class SwitchableMonoBehaviour: UnityEngine.MonoBehaviour, ISwitchable
    {
        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}