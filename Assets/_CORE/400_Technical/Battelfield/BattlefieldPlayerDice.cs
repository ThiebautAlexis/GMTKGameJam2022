using System;
using UnityEngine;
using DG.Tweening;

namespace GMTK
{
    public class BattlefieldPlayerDice : BattlefieldDice
    {
        #region Fields and Properties
        [SerializeField] private new BoxCollider2D collider;
        [SerializeField] private LayerMask triggeringLayer = new LayerMask();

        private static readonly Collider2D[] triggerElement = new Collider2D[1];
        private Sequence hornSequence;
        #endregion

        #region Methods 
        protected override void SelectFace()
        {
            base.SelectFace();
            collider.enabled = true;
        }

        public override void DisableVisibility()
        {
            base.DisableVisibility();
            collider.enabled = false;
        }
        #endregion

        #region I Drag And Droppable
        public override void DragUpdate(Vector2 _position)
        {
            transform.position = Vector2.MoveTowards(transform.position, _position, Time.deltaTime * 200);
        }

        public override void Drop()
        {
            // Check if in Battle Horn
            int _amount = Physics2D.OverlapBoxNonAlloc(transform.position, collider.size, 0f, triggerElement, triggeringLayer);
            if(_amount > 0)
            {
                BattleHorn _horn = triggerElement[0].GetComponent<BattleHorn>();
                hornSequence = DOTween.Sequence();
                hornSequence.Join(transform.DOMove(_horn.PlayerDicePosition, attributes.MovementDuration));
                hornSequence.onComplete += CallAction;

                void CallAction()
                {
                    //if (_horn)
                    //_horn.ActivateHorn();
                    // Ask AI 

                    // BattlefieldManager.PlayAction(selectedFace, Army.PlayerArmy.ArmyID, diceType);
                    BattlefieldManager.SelectPlayerAction(selectedFace, diceType);
                }
                
            }
        }

        public override bool StartDrag() => true;
        #endregion
    }
}
