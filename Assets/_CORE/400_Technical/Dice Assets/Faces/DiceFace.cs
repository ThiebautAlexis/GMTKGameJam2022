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

        public virtual Behaviour FaceBehaviour => Behaviour.Attack;

        #endregion

        #region Methods 
        public abstract void ApplyBehaviour(ref int[] tiles, int _armyID, out int _basePosition, out int _targetPosition);

        public Sprite GetFaceSprite()
        {
            if (isUpgraded)
                return uprgadedFaceSprite;
            return faceSprite;
        }
        #endregion

        [Flags]
        public enum Behaviour
        {
            Attack,
            Movement
        }
    }
}
