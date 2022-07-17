using System;
using UnityEngine;
using DG.Tweening;

namespace GMTK
{
    [CreateAssetMenu(fileName = "Fade in out Attributes", menuName = "GMTK/Attributes/Fade in out", order = 0)]
    public class FadeInOutAttributes : ScriptableObject
    {
        #region Fields and Properties
        public float fadeInDuration = .5f;
        public Ease fadeInEase = Ease.OutSine;

        public float fadeOutDuration = .5f;
        public Ease fadeOutEase = Ease.OutSine;
        #endregion

        #region Methods 

        #endregion
    }
}
