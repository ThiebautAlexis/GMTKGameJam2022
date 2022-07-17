using System;
using UnityEngine;
using DG.Tweening;

namespace GMTK
{
    [CreateAssetMenu(fileName = "BattlefieldDice Dice Attributes", menuName = "GMTK/Attributes/Battlefield Dice", order = 0)]
    public class BattlefieldDiceAttributes : ScriptableObject
    {
        #region Fields and Properties
        public float MovementDuration = 1.0f;
        public float MovementInterval = .25f;
        public Ease MovementEase = Ease.OutBounce;
        public float RefreshInterval = .05f;
        public int RefreshCount = 12;
        #endregion
    }
}
