using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Xrofng
{
    public class FocusableElement : BetterMonoBehaviour
    {
        [FoldoutGroup("Focusable")]
        [ChildGameObjectsOnly]
        //public MMFeedbacks OnFocusEnterFB;
        [FoldoutGroup("Focusable")]
        //[ChildGameObjectsOnly]
        //public MMFeedbacks OnFocusExitFB;
        public bool AllowSimulFB = false;

        private int _posId;
        public int PositionIndex => _posId;


        public void Focus()
        {
            OnFocus();
            //if (OnFocusEnterFB)
            //{
            //    if (AllowSimulFB == false && OnFocusExitFB)
            //    {
            //        StopFeedbacks(OnFocusExitFB);
            //    }
            //    OnFocusEnterFB.Direction = MMFeedbacks.Directions.TopToBottom;
            //    OnFocusEnterFB.PlayFeedbacks();
            //}
        }

        public void UnFocus()
        {
            OnUnFocus();
            //if (OnFocusExitFB)
            //{
            //    if (AllowSimulFB == false)
            //    {
            //        StopFeedbacks(OnFocusEnterFB);
            //    }
            //    OnFocusExitFB.PlayFeedbacks();
            //    return;
            //}
            //if (OnFocusEnterFB)
            //{
            //    OnFocusEnterFB.Direction = MMFeedbacks.Directions.BottomToTop;
            //    OnFocusEnterFB.PlayFeedbacks();
            //}
        }



        protected virtual void OnFocus()
        {

        }

        protected virtual void OnUnFocus()
        {

        }

        //private void StopFeedbacks(MMFeedbacks mMFeedbacks)
        //{
        //    if (mMFeedbacks.IsPlaying)
        //    {
        //        mMFeedbacks.StopFeedbacks();
        //    }
        //}

        public void SetPositionIndex(int i)
        {
            _posId = i;
        }

        public void SetShowVisibility(bool v)
        {
            if (v)
            {
                OnElementShowing();
            }
            else
            {
                OnElementHiding();
            }
        }

        protected virtual void OnElementShowing()
        {

        }

        protected virtual void OnElementHiding()
        {
            
        }
    }
}
