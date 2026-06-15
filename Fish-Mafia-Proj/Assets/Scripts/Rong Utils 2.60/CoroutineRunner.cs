using System.Collections;
using UnityEngine;

namespace Xrofng
{
    public class CoroutineRunner : MonoBehaviour
    {
        public void Run(IEnumerator routine)
        {
            StartCoroutine(routine);
        }
    }
}
