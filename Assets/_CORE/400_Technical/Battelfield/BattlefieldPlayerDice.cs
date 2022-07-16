using System;
using UnityEngine;

namespace GMTK
{
    public class BattlefieldPlayerDice : BattlefieldDice
    {
        #region Fields and Properties
        [SerializeField] private new BoxCollider2D collider;
        [SerializeField] private LayerMask triggeringLayer = new LayerMask();

        private static readonly Collider2D[] triggerElement = new Collider2D[1];

        #endregion

        #region Methods 
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
                BattlefieldManager.PlayAction(selectedFace, Army.PlayerArmy.ArmyID, diceType);
            }
        }

        public override bool StartDrag() => true;
        #endregion
    }
}
