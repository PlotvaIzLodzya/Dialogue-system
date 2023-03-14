using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueAnimator
    {
        private ScrollRect _scrollRect;
        private Coroutine _animation;

        public DialogueAnimator(ScrollRect scrollRect)
        {
            _scrollRect = scrollRect;
        }

        public void Scroll(float time, float targetNormalizedPosition)
        {
            UnityTranslator.CoroutineProvider.Stop(_animation);
            _animation = UnityTranslator.CoroutineProvider.RunCoroutine(ScrollDown(time, targetNormalizedPosition));
        }

        private IEnumerator ScrollDown(float time, float targetNormalizedPosition)
        {
            yield return null;
            float speed = Mathf.Abs(_scrollRect.verticalNormalizedPosition - targetNormalizedPosition) / time;
            float elapsedTime = 0;

            while (Mathf.Abs(_scrollRect.verticalNormalizedPosition - targetNormalizedPosition) > 0.01f)
            {
                elapsedTime += Time.deltaTime;

                _scrollRect.verticalNormalizedPosition = Mathf.MoveTowards(_scrollRect.verticalNormalizedPosition, targetNormalizedPosition, speed * Time.deltaTime);
                yield return null;
            }

            _scrollRect.verticalNormalizedPosition = targetNormalizedPosition;
        }
    }
}
