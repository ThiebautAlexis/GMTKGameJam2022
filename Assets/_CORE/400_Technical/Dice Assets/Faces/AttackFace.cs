using System;
using UnityEngine;

namespace GMTK
{
    [CreateAssetMenu(fileName = "AttackFaceAsset_", menuName = "GMTK/Faces/Attack Face Asset", order = 0)]
    public class AttackFace : DiceFace
    {
        public override Behaviour FaceBehaviour => Behaviour.Attack;
        
        #region Fields and Properties
        [Header("Attack Face")]
        [SerializeField, Range(1,4)] private int range = 1;
        [SerializeField, Range(1,4)] private int rangeUpgraded = 2;

        [SerializeField, Range(1, 4)] private int damages = 1;
        [SerializeField, Range(1, 4)] private int damagesUpgraded = 1;
        
        #endregion

        #region Methods 
        public override void ApplyBehaviour(ref int[] tiles, int _armyID, out int _basePosition, out int _targetPosition)
        {
            _basePosition = 0;
            for (int i = 0; i < tiles.Length; i++)
            {
                if(tiles[i] == _armyID)
                {
                    _basePosition = i;
                    break;
                }
            }

            _targetPosition = _basePosition + ((isUpgraded ? rangeUpgraded : range) * _armyID) ;
            if (_targetPosition < 0 || _targetPosition >= tiles.Length)
                return;
            if(tiles[_targetPosition] > 0)
                Army.PlayerArmy.TakeDamages(isUpgraded ? damagesUpgraded : damages);
            else if(tiles[_targetPosition] < 0)
                Army.OpponentArmy.TakeDamages(isUpgraded ? damagesUpgraded : damages);
            Debug.Log("Attack!");
        }
        #endregion
    }
}
