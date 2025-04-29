using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace EdCon.MiniGameTemplate
{
    public class ToastController : MonoBehaviour
    {
        private const float ANIM_TIME = 0.3f;
        private const float SHOW_DURATION = 2f;
        private const float VISIBLE_POSITION = -130;
        private const float INVISIBLE_POSITION = 0;

        [SerializeField] private Text toastText;
        [SerializeField] RectTransform toastRectTransform;

        private Vector2 hiddenPosition;
        private Vector2 visiblePosition;

        #region Public

        public void ShowToast(string text)
        {
            toastText.text = text;
            toastRectTransform.DOKill();
            toastRectTransform.DOAnchorPos(visiblePosition, ANIM_TIME).SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                DOVirtual.DelayedCall(SHOW_DURATION, HideToast);
                });
        }

        public void HideToast()
        {
            toastRectTransform.DOKill();
            toastRectTransform.DOAnchorPos(hiddenPosition, ANIM_TIME).SetEase(Ease.InQuad);
        }

        #endregion

        #region LifeCycle

        private void Start()
        {
            visiblePosition = new Vector2(toastRectTransform.anchoredPosition.x, VISIBLE_POSITION);
            hiddenPosition = new Vector2(toastRectTransform.anchoredPosition.x, INVISIBLE_POSITION);
            toastRectTransform.anchoredPosition = hiddenPosition;
        }

        #endregion
    }
}
