using System;
using Modules.MonoBehaviour;
using UnityEngine;

namespace z_Test
{
    public class TriggerZone : SwitchableMonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collider)
        {
            Debug.Log(collider.name);
        }
    }
}