using Infrastructure.ServiceManagement;

namespace Infrastructure.Input
{
    public interface IUIInputController: IService
    {
        public void OnUIThrowButtonDown();
        public void OnUIPassButtonDown();
        public void OnUIDunkButtonDown();
        public void OnUIChangePlayerButtonDown();
        
        public void OnUIThrowButtonUp();
        public void OnUIPassButtonUp();
        public void OnUIDunkButtonUp();
        public void OnUIChangePlayerButtonUp();
        public void OnUIJumpButtonUP();
        public void OnUIJumpButtonDown();
    }
}