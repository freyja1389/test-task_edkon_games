using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EdCon.MiniGameTemplate
{
    public class HudElement : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private String elementName;
        [SerializeField] private HUDHighlighter highlighter;
        [SerializeField] private CanvasGroup elementCanvasGroup;

        public event Action<HudElement> ElementSelected;

        private RectTransform elementRectTransform;

        #region Public

        public float Opacity
        {
            get => elementCanvasGroup != null ? elementCanvasGroup.alpha : 1f;
            set
            {
                if (elementCanvasGroup != null)
                    elementCanvasGroup.alpha = value;
            }
        }

        public Vector2 Scale
        {
            get => elementRectTransform != null ? elementRectTransform.sizeDelta : Vector2.zero;
            set
            {
                if (elementRectTransform != null)
                {
                    elementRectTransform.sizeDelta = new Vector2(value.x, value.y);
                }
            }
        }

        public Vector2 Position
        {
            get => elementRectTransform != null ? elementRectTransform.anchoredPosition : Vector2.zero;
            set
            {
                if (elementRectTransform != null)
                    elementRectTransform.anchoredPosition = value;
            }
        }

        public void SetSelected(bool isSelected)
        {

            if (highlighter == null) return;

            if (isSelected)
            {
                highlighter.SetHighlight(elementRectTransform);
            }
            else
            {
                highlighter.RemoveHighlight(elementRectTransform);
            }
        }

        public string Name
        {
            get => elementName;
        }

        #endregion

        #region LifeCycle

        private void Awake()
        {
            elementRectTransform = GetComponent<RectTransform>();
        }

        #endregion

        #region IPointerDownHandler

        public void OnPointerDown(PointerEventData eventData)
        {
            ElementSelected?.Invoke(this);
        }

        #endregion

        #region IDragHandler

        public void OnDrag(PointerEventData eventData)
        {
            elementRectTransform.anchoredPosition += eventData.delta / GetCanvasScaleFactor();
        }

        #endregion

        #region Private

        private float GetCanvasScaleFactor()
        {
            return canvas ? canvas.scaleFactor : 1f;
        }

        #endregion
    }
}
