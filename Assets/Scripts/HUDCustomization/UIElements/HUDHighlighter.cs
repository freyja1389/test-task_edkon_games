using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EdCon.MiniGameTemplate
{
    public class HUDHighlighter : MonoBehaviour
    {
        [SerializeField] private RectTransform hudHighlighter;

        private RectTransform currentHighlightedTarget;
        private RectTransform defaultHighlighterParent;

        #region Public

        public void SetHighlight(RectTransform target)
        {
            if (target == null || hudHighlighter == null) return;

            if (hudHighlighter != null && currentHighlightedTarget == target)
                return;

            hudHighlighter.SetParent(target);
            hudHighlighter.SetSiblingIndex(0);
            SetTargetRectTransformSettings(target);
            currentHighlightedTarget = target;
            hudHighlighter.gameObject.SetActive(true);
        }

        public void RemoveHighlight(RectTransform target)
        {
            if (hudHighlighter != null && currentHighlightedTarget == target)
            {
                hudHighlighter.gameObject.SetActive(false);
                hudHighlighter.SetParent(defaultHighlighterParent);
                currentHighlightedTarget = null;
            }
        }

        #endregion

        #region LifeCycle

        private void Awake()
        {
            defaultHighlighterParent = GetComponent<RectTransform>();
        }

        #endregion

        #region Private

        private void SetTargetRectTransformSettings(RectTransform target)
        {
            hudHighlighter.anchorMin = new Vector2(0f, 0f);
            hudHighlighter.anchorMax = new Vector2(1f, 1f);
            hudHighlighter.offsetMin = Vector2.zero;
            hudHighlighter.offsetMax = Vector2.zero;
        }

        #endregion
    }
}
