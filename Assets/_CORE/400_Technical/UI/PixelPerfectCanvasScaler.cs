using System;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK
{
    [ExecuteInEditMode]
    public class PixelPerfectCanvasScaler : CanvasScaler
    {
        #region Fields and Properties
        [SerializeField, HideInInspector] private Canvas rootCanvas = null;
        private static readonly int kLogBase = 2;
        #endregion

        #region Methods
        protected override void OnEnable()
        {
            rootCanvas = GetComponent<Canvas>();
            base.OnEnable();
        }

        protected override void HandleScaleWithScreenSize()
        {
            Vector2 _screenSize;
            if (rootCanvas.worldCamera != null)
            {
                _screenSize = new Vector2(rootCanvas.worldCamera.pixelWidth, rootCanvas.worldCamera.pixelHeight);
            }
            else
            {
                _screenSize = new Vector2(Screen.width, Screen.height);
            }

            // Multiple display support only when not the main display. For display 0 the reported
            // resolution is always the desktops resolution since its part of the display API,
            // so we use the standard none multiple display method. (case 741751)
            int displayIndex = rootCanvas.targetDisplay;
            if (displayIndex > 0 && displayIndex < Display.displays.Length)
            {
                Display disp = Display.displays[displayIndex];
                _screenSize = new Vector2(disp.renderingWidth, disp.renderingHeight);
            }

            float scaleFactor = 0;
            switch (m_ScreenMatchMode)
            {
                case ScreenMatchMode.MatchWidthOrHeight:
                    {
                        // We take the log of the relative width and height before taking the average.
                        // Then we transform it back in the original space.
                        // the reason to transform in and out of logarithmic space is to have better behavior.
                        // If one axis has twice resolution and the other has half, it should even out if widthOrHeight value is at 0.5.
                        // In normal space the average would be (0.5 + 2) / 2 = 1.25
                        // In logarithmic space the average is (-1 + 1) / 2 = 0
                        float logWidth = Mathf.Log(_screenSize.x / m_ReferenceResolution.x, kLogBase);
                        float logHeight = Mathf.Log(_screenSize.y / m_ReferenceResolution.y, kLogBase);
                        float logWeightedAverage = Mathf.Lerp(logWidth, logHeight, m_MatchWidthOrHeight);
                        scaleFactor = Mathf.Pow(kLogBase, logWeightedAverage);
                        break;
                    }
                case ScreenMatchMode.Expand:
                    {
                        scaleFactor = Mathf.Min(_screenSize.x / m_ReferenceResolution.x, _screenSize.y / m_ReferenceResolution.y);
                        break;
                    }
                case ScreenMatchMode.Shrink:
                    {
                        scaleFactor = Mathf.Max(_screenSize.x / m_ReferenceResolution.x, _screenSize.y / m_ReferenceResolution.y);
                        break;
                    }
            }

            SetScaleFactor(scaleFactor);
            SetReferencePixelsPerUnit(m_ReferencePixelsPerUnit);
        }

#if UNITY_EDITOR
        private void OnGUI()
        {
            Vector2 _screenSize;
            if (rootCanvas.worldCamera != null)
            {
                _screenSize = new Vector2(rootCanvas.worldCamera.pixelWidth, rootCanvas.worldCamera.pixelHeight);
            }
            else
            {
                _screenSize = new Vector2(Screen.width, Screen.height);
            }
            Rect _r = new Rect(0, 0, 250, 50);
            GUI.Label(_r, $"{_screenSize.x} x {_screenSize.y}");
        }
#endif
#endregion
    }
}
