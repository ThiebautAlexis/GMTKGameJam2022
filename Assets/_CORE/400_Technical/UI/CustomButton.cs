using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

namespace GMTK
{
    public class CustomButton : MonoBehaviour, ISubmitHandler, IPointerClickHandler
    {
        #region Global Members
        [SerializeField] private CustomButtonAttributes attributes = null;
        [SerializeField] private RectTransform rectTransform = null;
        [Space(10f)]

        [SerializeField] private UnityEvent onClick = null;
        #endregion

        #region Behaviour
        private static bool isSubmitting = false;

        private Sequence sequence = null;

        private Vector3 baseScale = Vector3.one;
        private bool isSelected = false;

        // ---------------

        protected void OnEnable()
        {
            baseScale = rectTransform.localScale;
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (isSubmitting)
                return;

            isSubmitting = true;
            // Animation.
            if (sequence.IsActive())
            {
                sequence.Kill(true);
            }

            sequence = DOTween.Sequence();
            {
                sequence.Join(rectTransform.DOScale(baseScale * attributes.SizeMultiplier, attributes.SubmittingDuration).SetEase(attributes.SizeEase));
                sequence.OnComplete(OnComplete);

                sequence.SetUpdate(true);
            }

            // ----- Local Method ----- \\

            void OnComplete()
            {
                onClick.Invoke();

                isSubmitting = false;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isSubmitting)
                return;

            isSubmitting = true;
            // Animation.
            if (sequence.IsActive())
            {
                sequence.Kill(true);
            }

            sequence = DOTween.Sequence();
            {
                sequence.Join(rectTransform.DOScale(baseScale * attributes.SizeMultiplier, attributes.SubmittingDuration).SetEase(attributes.SizeEase));
                sequence.OnComplete(OnComplete);

                sequence.SetUpdate(true);
            }

            // ----- Local Method ----- \\

            void OnComplete()
            {
                onClick.Invoke();

                isSubmitting = false;
            }
        }
        #endregion
    }
}
