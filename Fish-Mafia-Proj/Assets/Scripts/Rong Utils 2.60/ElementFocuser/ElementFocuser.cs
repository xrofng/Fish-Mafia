using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xrofng
{
    struct EvsFocuscerChanged
    {
        public string Id;
        public int IndexChangedTo;
        public int IndexChangedFrom;

        public EvsFocuscerChanged(string id, int indexChangedTo, int indexChangedFrom)
        {
            Id = id;
            IndexChangedTo = indexChangedTo;
            IndexChangedFrom = indexChangedFrom;
        }
    }

    public class ElementFocuser : BetterMonoBehaviour
    {
        [SerializeField] string Id;
        public FocusableElement[] Elements;
        [SerializeField] bool IsLoop;
        [SerializeField] bool UnFocusAllOnStart = false;
        [SerializeField] bool FocusFirstElementOnStart = true;
        [SerializeField] bool IsFixLength;
        [SerializeField] [ShowIf("IsFixLength")] int FixedElementLength = 1;
        [InfoBox("When Focus First Element")]
        [SerializeField] bool BypassChangeCondition = false;

        private FocusableElement _focusing => Elements[currIndex];
        private int _prevIndex = 666;
        protected int currIndex = -6666;

        public int FocusingElementIndex => currIndex;
        public int ElementCount
        {
            get
            {
                if (IsFixLength)
                {
                    return FixedElementLength;
                }
                return Elements.Length;
            }
        }
        public FocusableElement FocusingElement => _focusing;


        private Action _onFocusChanged;

        protected override void OnEnable()
        {
            OnObjectEnabled();
        }

        protected virtual void OnObjectEnabled()
        {
            
        }

        protected override void OnDisable()
        {
            OnObjectDisabled();
        }

        protected virtual void OnObjectDisabled()
        {
            
        }

        protected override void Awake()
        {

        }

        protected override void Start()
        {
            OnEarlyStart();
            if (UnFocusAllOnStart)
            {
                UnFocusAll();
            }
            if (FocusFirstElementOnStart)
            {
                StartCoroutine(FocusFirstElementRoutine());
            }
            for (int i = 0; i < Elements.Length; i++)
            {
                Elements[i].SetPositionIndex(i);
            }
        }

        private void UnFocusAll()
        {
            foreach (FocusableElement element in Elements)
            {
                element.UnFocus();
            }
        }

        protected virtual void OnEarlyStart()
        {

        }

        public void SetFocusIndex(int index)
        {
            if (index < 0 || index >= ElementCount)
            {
                return;
            }
            if (CheckFocusChangedCondition(index))
            {
                _prevIndex = currIndex;
                currIndex = index;
                InternalFocusChanged();    
            }
        }

        protected virtual bool CheckFocusChangedCondition(int newIndex)
        {
            if (BypassChangeCondition && newIndex == 0)
            {
                return true;
            }
            return  newIndex != currIndex;
        }

        public FocusableElement GetElement(int index)
        {
            return Elements[index];
        }

        private void InternalFocusChanged()
        {
            if (_prevIndex >= 0 && _prevIndex < ElementCount)
            {
                OnBeforeFocusChanged(Elements[_prevIndex], _focusing);
                Elements[_prevIndex].UnFocus();
            }
            else
            {
                OnBeforeFocusChanged(null, _focusing);
            }
            _focusing.Focus();
            OnAfterFocusChanged(_prevIndex, currIndex);
            _onFocusChanged?.Invoke();
            EventBus.TriggerEvent(new EvsFocuscerChanged(Id, currIndex,_prevIndex));
        }

        protected virtual void OnAfterFocusChanged(int prevIndex, int newIndex)
        {

        }

        protected virtual void OnBeforeFocusChanged(FocusableElement prevFocusing, FocusableElement focusing)
        {

        }

        /// <summary>
        /// Use in Invoke on Start 
        /// </summary>
        IEnumerator FocusFirstElementRoutine()
        {
            _prevIndex = 666;
            yield return null;
            FocusFirstElement();
            _prevIndex = 666;
        }

        public void FocusFirstElement()
        {
            SetFocusIndex(0);
        }

        /// <summary>
        /// Add value to _currIndex
        /// </summary>
        /// <param name="increment"></param>
        public void AddFocusIndex(int increment)
        {
            SetFocusIndex(CalculateNextIndex(increment));
        }

        /// <summary>
        /// Calculate next index from fromIndex
        /// </summary>
        public int CalculateIndexNextTo(int nextRange, int fromIndex)
        {
            int _focusingIndex = fromIndex + nextRange;
            if (IsLoop)
            {
                _focusingIndex = (_focusingIndex + ElementCount) % ElementCount;
            }
            else
            {
                _focusingIndex = Mathf.Clamp(_focusingIndex, 0, ElementCount);
            }
            return _focusingIndex;
        }

        /// <summary>
        /// Calculate next index from CurrentIndex
        /// </summary>
        public int CalculateNextIndex(int nextRange)
        {
            return CalculateIndexNextTo(nextRange, currIndex);
        }

        /// <summary>
        /// Try to get element from next Index
        /// if current is not init before, we just get next to 0
        /// </summary>
        public FocusableElement GetNextElement(int nextRange, int fromIndex = -66)
        {
            if (fromIndex < 0)
            {
                if (currIndex < 0)
                {
                    return Elements[CalculateIndexNextTo(nextRange, 0)];
                }
                return Elements[CalculateIndexNextTo(nextRange, currIndex)];
            }
            return Elements[CalculateIndexNextTo(nextRange, fromIndex)];
        }

        public virtual void SetFixedElementLength(int length)
        {
            FixedElementLength = length;
        }

        public void AddFocusChangedCallback(System.Action action)
        {
            _onFocusChanged += action;
        }
    }
}
