using System.Collections;
using UnityEngine;

namespace Xrofng
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BaseFadeView : BaseAlphaView
    {
        [SerializeField] private float FadeDuration = 0.3f;
        public bool AlwaysShowThenHide = false;

        private Coroutine fadeCoroutine;


        protected override void Start()
        {
            base.Start();
        }

        protected override void OnShowing()
        {
            IsVisible = true;
            FadeToAlpha(1f);
        }

        protected override void OnHiding()
        {
            IsVisible = false;
            FadeToAlpha(0f);
        }

        protected void FadeToAlpha(float targetAlpha)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine = StartCoroutine(FadeCanvas(targetAlpha));
        }

        private IEnumerator FadeCanvas(float targetAlpha)
        {
            float startAlpha = CanvasGroup.alpha;
            float timeElapsed = 0f;

            while (timeElapsed < FadeDuration)
            {
                Debug.Log("fade" + timeElapsed);
                CanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / FadeDuration);
                timeElapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            CanvasGroup.alpha = targetAlpha;
            CanvasGroup.interactable = targetAlpha > 0.95f;
            CanvasGroup.blocksRaycasts = targetAlpha > 0.95f;

            if (AlwaysShowThenHide && targetAlpha > 0)
            {
                fadeCoroutine = StartCoroutine(FadeCanvas(0));
            }
        }
    }
}