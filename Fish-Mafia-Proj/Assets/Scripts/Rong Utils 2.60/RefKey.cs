using UnityEngine;

namespace Xrofng
{
    public abstract class RefKey : ScriptableObject
    {
        [SerializeField]
        private string debugName;

        public string DebugName => debugName;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (string.IsNullOrEmpty(debugName))
                debugName = name;
        }
#endif
    }
}