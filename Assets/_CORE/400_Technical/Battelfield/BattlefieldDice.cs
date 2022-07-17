using System;
using UnityEngine;
using DG.Tweening;

namespace GMTK
{
    public class BattlefieldDice : MonoBehaviour, IDragAndDroppable
    {
        #region Fields and Properties
        [SerializeField] private SpriteRenderer diceRenderer;
        [SerializeField] private SpriteRenderer faceRenderer;
        [SerializeField] protected BattlefieldDiceAttributes attributes;


        protected UnitType diceType = UnitType.Default;
        protected DiceFace selectedFace = null;
        private Sequence rollSequence;

        public DiceFace SelectedFace => selectedFace;
        #endregion

        #region Methods 
        public virtual void RollDice(DiceAsset diceAsset, Vector2 _position, int _index)
        {
            diceType = diceAsset.UnitType;
            diceRenderer.sprite = diceAsset.DiceSprite;
            selectedFace = diceAsset.GetRandomFace();
            diceRenderer.enabled = true;
            faceRenderer.enabled = true;

            // Sequence to roll dice here
            if (rollSequence.IsActive())
            {
                rollSequence.Kill(true);
            }
            rollSequence = DOTween.Sequence();

            /// Here add the rolling Dice sequence
            rollSequence.AppendInterval(attributes.MovementInterval *  (1 +_index));
            rollSequence.Append(transform.DOMove(_position, attributes.MovementDuration).SetEase(attributes.MovementEase));
            
            for (int i = 0; i < attributes.RefreshCount; i++)
            {
                rollSequence.AppendInterval(attributes.RefreshInterval);
                rollSequence.AppendCallback(() => faceRenderer.sprite = diceAsset.GetFaceSprite());
            }
            
            rollSequence.onComplete += SelectFace;
        }

        protected virtual void SelectFace()
        {
            if (selectedFace)
                faceRenderer.sprite = selectedFace.GetFaceSprite();
            else faceRenderer.sprite = null;
        }
        public virtual void DisableVisibility()
        {
            diceRenderer.enabled = false;
            faceRenderer.enabled = false;
        }
        #endregion

        #region Drag and Drop 

        public virtual void DragUpdate(Vector2 _position){}

        public virtual void Drop(){}

        public virtual bool StartDrag() => false;


        #endregion
    }
}
