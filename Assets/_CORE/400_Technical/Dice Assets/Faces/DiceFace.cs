using System;
using UnityEngine;

namespace GMTK
{
    // [CreateAssetMenu(fileName = "FaceAsset_", menuName = "GMTK/Assets/Face Asset", order = 0)]
    public abstract class DiceFace : ScriptableObject
    {
        #region Fields and Properties
        [Header("Base Face")]
        [SerializeField] protected Sprite faceSprite = null;
        [Header("Upgraded Face")]
        [SerializeField] protected bool isUpgraded = false;
        [SerializeField] protected Sprite uprgadedFaceSprite = null;

        public virtual Behaviour FaceBehaviour => Behaviour.Empty;

        #endregion

        #region Methods 
        public abstract void ApplyBehaviour();

        public Sprite GetFaceSprite()
        {
            if (isUpgraded)
                return uprgadedFaceSprite;
            return faceSprite;
        }
        #endregion

        public enum Behaviour
        {
            Empty,
            Attack,
            Movement
        }
    }
}
