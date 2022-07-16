using System;
using UnityEngine;
using DG.Tweening;

namespace GMTK
{
    public class ArmyManager : MonoBehaviour
    {
        #region Fields and Properties
        [SerializeField] private Owner owner = Owner.None;
        [SerializeField] private Tiles tiles;

        private Sequence armySequence;
        #endregion

        #region Methods 
        private void OnEnable()
        {
            switch (owner)
            {
                case Owner.Player:
                    BattlefieldManager.OnPlayerArmyMove += MoveArmy;
                    BattlefieldManager.OnPlayerArmyAttack += ArmyAttack;
                    break;
                case Owner.Opponent:
                    BattlefieldManager.OnOpponentArmyMove += MoveArmy;
                    BattlefieldManager.OnOpponentArmyAttack += ArmyAttack;
                    break;
                default:
                    break;
            }
        }
        private void OnDisable()
        {
            switch (owner)
            {
                case Owner.Player:
                    BattlefieldManager.OnPlayerArmyMove -= MoveArmy;
                    BattlefieldManager.OnPlayerArmyAttack -= ArmyAttack;
                    break;
                case Owner.Opponent:
                    BattlefieldManager.OnPlayerArmyMove -= MoveArmy;
                    BattlefieldManager.OnPlayerArmyAttack -= ArmyAttack;
                    break;
                default:
                    break;
            }
        }

        private void ArmyAttack(UnitType _unitType, int _baseTile, int _targetTile)
        {
            switch (_unitType)
            {
                case UnitType.Swordsmen:
                    break;
                case UnitType.Spearmen:
                    break;
                case UnitType.Archers:
                    break;
                default:
                    break;
            }
        }

        private void MoveArmy(int _baseTile, int _targetTile)
        {
            if (armySequence.IsActive())
                armySequence.Kill(true);

            armySequence = DOTween.Sequence();
            {
                armySequence.Append(transform.DOLocalMove(tiles.TilesPosition[_targetTile], 1.0f));
            }
        }

  
        #endregion
    }

}
