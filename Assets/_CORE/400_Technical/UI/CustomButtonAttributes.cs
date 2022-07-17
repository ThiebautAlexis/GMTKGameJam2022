using System;
using UnityEngine;
using DG.Tweening;

namespace GMTK
{
    [CreateAssetMenu(fileName = "Custom Button Attributes", menuName = "GMTK/Attributes/Custom Button", order = 0)]
    public class CustomButtonAttributes : ScriptableObject
    {
	    #region Fields and Properties
        public float SizeMultiplier = .85f;
        public AnimationCurve SizeEase = new AnimationCurve();
        public float SubmittingDuration = .25f;
        #endregion

        #region Methods 

        #endregion
    }
}
