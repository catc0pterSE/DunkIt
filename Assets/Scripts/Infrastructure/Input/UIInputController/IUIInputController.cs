using System;
using Infrastructure.ServiceManagement;

namespace Infrastructure.Input
{
    public interface IUIInputController: IService
    {
        public void OnUIThrowButtonClicked();
        public void OnUIPassButtonClicked();
        public void OnUIDunkButtonClicked();
        public void OnUIChangePlayerButtonClicked();
    }
}