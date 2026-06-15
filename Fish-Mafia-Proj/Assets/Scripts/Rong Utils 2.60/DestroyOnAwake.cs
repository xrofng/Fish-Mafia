using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xrofng
{
    public class DestroyOnAwake : BetterMonoBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            Destroy(gameObject);
        }
    }
}