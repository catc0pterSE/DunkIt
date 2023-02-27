using System;
using System.Collections;
using UnityEngine;

namespace z_Test
{
    public class Switcher: MonoBehaviour
    {
        [SerializeField] private GameObject obj;

        private void OnEnable()
        {
            StartCoroutine(TurnOffTurnO());
        }

        private IEnumerator TurnOffTurnO()
        {
            yield return new WaitForSeconds(2);
            obj.SetActive(false);
            yield return new WaitForSeconds(1);
            obj.SetActive(true);
        }
    }
}