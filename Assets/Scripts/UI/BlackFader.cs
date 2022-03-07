using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus {

    [RequireComponent(typeof(CanvasGroup))]
    public class BlackFader : MonoBehaviour {

        [SerializeField] CanvasGroup canvasGroup;

        IEnumerator fadeCo;

        private void Setup ()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            fadeCo = Fade( 0.52f, 1f );
        }

        private void Start ()
        {
            Setup();
        }

        public void FadeInOut(float timeInterval)
        {
            StartCoroutine( FadeInOutCo( timeInterval ) );
        }

        public IEnumerator FadeInOutCo (float timeInterval)
        {
            StopCoroutine( fadeCo );
            fadeCo = Fade( 0.52f, 1f );
            StartCoroutine( fadeCo );
            yield return new WaitForSecondsRealtime( timeInterval );
            StopCoroutine( fadeCo );
            fadeCo = Fade( 0.25f, 0f );
            StartCoroutine( fadeCo );
        }

        public void FadeIn()
        {
            StopCoroutine( fadeCo );
            fadeCo = Fade( 0.52f, 1f );
            StartCoroutine( fadeCo );
        }

        public void FadeOut ()
        {
            StopCoroutine( fadeCo );
            fadeCo = Fade( 0.52f, 0f );
            StartCoroutine( fadeCo );
        }

        private IEnumerator Fade (float totalTime, float finalAlpha)
        {
            float elapsedTime = 0f;
            float startingAlpha = canvasGroup.alpha;
            while (elapsedTime < totalTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.Lerp( startingAlpha, finalAlpha, elapsedTime / totalTime );
                yield return null;
            }
            canvasGroup.alpha = finalAlpha;
        }

    }

}