using System;
using UnityEngine;
using DG.Tweening;

namespace GMTK
{
    public class BattleHorn : MonoBehaviour
    {
        #region Fields and Properties
        [SerializeField] private Vector2 playerDicePosition; 
        [SerializeField] private Vector2 opponentDicePosition;

        public Vector2 PlayerDicePosition => playerDicePosition;
        public Vector2 OpponentDicePosition => opponentDicePosition;

        private Sequence hornSequence;
        #endregion

        #region Methods 
        public void ActivateHorn()
        {
            Debug.Log("Poueeeet");
            if (hornSequence.IsActive())
                hornSequence.Kill(true);
            hornSequence = DOTween.Sequence();
            hornSequence.Join(transform.DOShakePosition(1.5f, .15f, 10, 90, false, false));
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(playerDicePosition, .25f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(opponentDicePosition, .25f);
        }
        #endregion
    }
}
