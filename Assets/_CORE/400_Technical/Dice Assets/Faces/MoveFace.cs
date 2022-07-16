using System;
using UnityEngine;

namespace GMTK
{
    [CreateAssetMenu(fileName = "MoveFaceAsset_", menuName = "GMTK/Faces/Movement Face Asset", order = 0)]
    public class MoveFace : DiceFace
    {
        public override Behaviour FaceBehaviour => Behaviour.Movement;
        #region Fields and Properties
        [Header("Mouvement Face")]
        [SerializeField, Range(-4, 4)] private int range = 1;
        [SerializeField, Range(-4, 4)] private int rangeUpgraded = 2;
        #endregion

        #region Methods 

        public override void ApplyBehaviour(ref int[] tiles, int _armyID, out int _basePosition, out int _targetPosition)
        {
            _basePosition = 0;
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i] == _armyID)
                {
                    _basePosition = i;
                    break;
                }
            }

            _targetPosition = _basePosition + ((isUpgraded ? rangeUpgraded : range) * _armyID);
            if(_targetPosition > _basePosition)
            {
                for (int i = _basePosition; i < _targetPosition; i++)
                {
                    if (i + 1 < tiles.Length && tiles[i + 1] == 0)
                        continue;
                    _targetPosition = i;
                    break;
                }
            }
            else
            {
                for (int i = _basePosition; i > _targetPosition; i--)
                {
                    if (i - 1 >= 0 && tiles[i - 1] == 0)
                        continue;
                    _targetPosition = i;
                    break;
                }
            }
            tiles[_basePosition] = 0;
            tiles[_targetPosition] = _armyID;
        }
        #endregion
    }
}
