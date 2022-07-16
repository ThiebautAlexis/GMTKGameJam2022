using System;
using UnityEngine;
using DG.Tweening;

namespace GMTK
{
    public class BattlefieldDice : MonoBehaviour, IDragAndDroppable
    {
        #region Fields and Properties
        [SerializeField] private     SpriteRenderer diceRenderer;
        [SerializeField] private     SpriteRenderer faceRenderer;
        protected UnitType diceType = UnitType.Default;
        protected DiceFace selectedFace = null;
        #endregion

        #region Methods 
        internal void RollDice(DiceAsset diceAsset)
        {
            diceType = diceAsset.UnitType;
            diceRenderer.sprite = diceAsset.DiceSprite;
            selectedFace = diceAsset.GetRandomFace();
            // Sequence to roll dice here
            if (selectedFace)
                faceRenderer.sprite = selectedFace.GetFaceSprite();
            else faceRenderer.sprite = null;

            gameObject.SetActive(true);
        }
        #endregion

        #region Drag and Drop 

        public virtual void DragUpdate(Vector2 _position){}

        public virtual void Drop(){}

        public virtual bool StartDrag() => false;

        #endregion
    }
}
