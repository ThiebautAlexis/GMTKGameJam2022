using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GMTK
{
    public class ArmyManager : MonoBehaviour
    {
        #region Fields and Properties
        [SerializeField] private Owner owner = Owner.None;
        [SerializeField] private Tiles tiles;
        [SerializeField] private Vector2 tileRange = Vector2.zero;

        private List<ArmyParticleSystem> systems = new List<ArmyParticleSystem>();
        private Sequence armySequence;
        #endregion

        #region Methods 
        private void OnEnable()
        {
            switch (owner)
            {
                case Owner.Player:
                    BattlefieldManager.OnPlayerArmyMove += MoveArmy;
                    Army.PlayerArmy.OnArmyAttackTarget += ArmyAttack;
                    Army.PlayerArmy.OnArmyTakeDamages += TakeDamages;
                    break;
                case Owner.Opponent:
                    BattlefieldManager.OnOpponentArmyMove += MoveArmy;
                    Army.OpponentArmy.OnArmyAttackTarget += ArmyAttack;
                    Army.OpponentArmy.OnArmyTakeDamages += TakeDamages;
                    break;
                default:
                    break;
            }
            BattlefieldManager.OnBattleStart += PopulateArmy;
            BattlefieldManager.OnEndBattle += ClearArmy;
        }

        private void OnDisable()
        {
            switch (owner)
            {
                case Owner.Player:
                    BattlefieldManager.OnPlayerArmyMove -= MoveArmy;
                    Army.PlayerArmy.OnArmyAttackTarget -= ArmyAttack;
                    Army.PlayerArmy.OnArmyTakeDamages -= TakeDamages;

                    break;
                case Owner.Opponent:
                    BattlefieldManager.OnOpponentArmyMove -= MoveArmy;
                    Army.OpponentArmy.OnArmyAttackTarget -= ArmyAttack;
                    Army.OpponentArmy.OnArmyTakeDamages -= TakeDamages;
                    break;
                default:
                    break;
            }
            BattlefieldManager.OnBattleStart -= PopulateArmy;
            BattlefieldManager.OnEndBattle -= ClearArmy;
        }

        private void ClearArmy()
        {
            for (int i = 0; i < systems.Count; i++)
            {
                DestroyImmediate(systems[i].gameObject);
            }
            systems.Clear();
        }

        private void PopulateArmy()
        {
            List<DiceAsset> _dices;
            switch (owner)
            {
                case Owner.Player:
                    _dices = new List<DiceAsset>(Army.PlayerArmy.diceReserve);
                    break;
                case Owner.Opponent:
                    _dices = new List<DiceAsset>(Army.OpponentArmy.diceReserve);
                    break;
                default:
                    return;
            }
            systems = new List<ArmyParticleSystem>();
            Vector3 _invertedScale = new Vector3(-1,1,1);
            for (int i = 0; i <_dices.Count; i++)
            {
                systems.Add(Instantiate(_dices[i].ParticleSystem, transform));
                systems[i].transform.localPosition = Vector2.right * Random.Range(tileRange.x, tileRange.y);
                if (owner == Owner.Opponent)
                    systems[i].System.transform.localScale = _invertedScale;
            }
        }

        private void ArmyAttack(UnitType _unitType, int _targetIndex)
        {
            for (int i = 0; i < systems.Count; i++)
            {
                if(systems[i].UnitType == _unitType)
                {
                    var _mainModule = systems[i].System.main;
                    _mainModule.startSpeed = 1;
                    if(armySequence.IsActive())
                    {
                        armySequence.Kill();
                    }
                    armySequence = DOTween.Sequence();
                    {
                        armySequence.AppendInterval(.45f);
                        armySequence.AppendCallback(() => _mainModule.startSpeed = 0);
                    };
                    break;
                }
            }
        }

        private void MoveArmy(int _baseTile, int _targetTile)
        {
            if (armySequence.IsActive())
                armySequence.Kill(true);

            armySequence = DOTween.Sequence();
            {
                armySequence.Append(transform.DOMoveX(tiles.TilesPosition[_targetTile].x, 1.0f));
            }
        }

        private void TakeDamages(UnitType _unitType)
        {
            for (int i = 0; i < systems.Count; i++)
            {
                if(systems[i].UnitType == _unitType)
                {
                    DestroyImmediate(systems[i].gameObject);
                    systems.RemoveAt(i);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine((Vector2)transform.position + Vector2.right * tileRange.x, (Vector2)transform.position + Vector2.right * tileRange.y);
        }
        #endregion
    }

}
