using UnityEngine;

namespace Utility.Extensions
{
    public static class CharacterControllerExtensions
    {
        public static void Enable(this CharacterController characterController) =>
            characterController.enabled = true;
        
        public static void Disable(this CharacterController characterController) =>
            characterController.enabled = false;
    }
}