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
        public override void ApplyBehaviour(ref int[] tiles, int _armyID, out int _basePosition, out int _targetPosition, out bool _inflictDamages)
        {
            _inflictDamages = false;
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
            {
                _inflictDamages = true;
                Army.PlayerArmy.TakeDamages(isUpgraded ? damagesUpgraded : damages);
            }
            else if(tiles[_targetPosition] < 0)
            {
                _inflictDamages = true;
                Army.OpponentArmy.TakeDamages(isUpgraded ? damagesUpgraded : damages);
            }
        }


        public override int ComputeScore(int[] tiles)
        {
            int _baseIndex = -1;
            for (int i = 0; i < tiles.Length; i++)
            {
                if(tiles[i] == -1)
                {
                    _baseIndex = i;
                    break;
                }
            }
            int _targetIndex = _baseIndex + ((isUpgraded ? rangeUpgraded : range) * -1);
            if (_targetIndex < 0 || _targetIndex >= tiles.Length)
                return -999;
            if (tiles[_targetIndex] == 1)
                return isUpgraded ? damagesUpgraded : damages;
            return 0;
        }
        #endregion
    }
}
