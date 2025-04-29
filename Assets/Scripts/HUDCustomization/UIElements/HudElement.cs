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

        public event Action<HudElement> ElementSelected;

        private CanvasGroup elementCanvasGroup;
        private RectTransform elementRectTransform;
        private Image elementSelectedImage;

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

        public bool IsSelected
        {
            set
            {
                if (elementSelectedImage != null)
                    elementSelectedImage.enabled = value;
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
            elementCanvasGroup = GetComponent<CanvasGroup>();
            elementRectTransform = GetComponent<RectTransform>();
            elementSelectedImage = GetComponent<Image>();
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
