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
        private Sequence rollSequence;

        #endregion

        #region Methods 
        int randomIndex = 0;
        internal void RollDice(DiceAsset diceAsset)
        {
            randomIndex = 0;
            diceType = diceAsset.UnitType;
            diceRenderer.sprite = diceAsset.DiceSprite;
            selectedFace = diceAsset.GetRandomFace();
            gameObject.SetActive(true);

            // Sequence to roll dice here
            if (rollSequence.IsActive())
                rollSequence.Kill();
            rollSequence = DOTween.Sequence();

            /// Here add the rolling Dice sequence

            rollSequence.onComplete += SelectFace;
            void SelectFace()
            {
                if (selectedFace)
                    faceRenderer.sprite = selectedFace.GetFaceSprite();
                else faceRenderer.sprite = null;
            }
        }
        #endregion

        #region Drag and Drop 

        public virtual void DragUpdate(Vector2 _position){}

        public virtual void Drop(){}

        public virtual bool StartDrag() => false;

        #endregion
    }
}
