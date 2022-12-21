using System.Threading.Tasks;
using UnityEngine;

namespace z_Test
{
    public class AsyncMovement : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _target2;

        private void Start()
        {
            Move();
        }

        private async void Move()
        {
            await GoTo();
            await GoTo2();
        }

        private async Task GoTo()
        {
            while (transform.position != _target.position)
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * 10);
                Debug.Log("GoTO1");
                await Task.Yield();
            }
        }

        private async Task GoTo2()
        {
            while (transform.position != _target.position)
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, _target2.transform.position, Time.deltaTime * 10);
                Debug.Log("GOTO2");
                await Task.Yield();
            }
        }
    }
}