using System;
using UnityEngine;

namespace GMTK
{
    public class BattlefieldDicePool : MonoBehaviour
    {
        #region Fields and Properties
        [SerializeField] private PoolOwner owner = PoolOwner.None;
        [SerializeField] private BattlefieldDice[] dices = new BattlefieldDice[] { };
        #endregion

        #region Methods 
        private void Awake()
        {
            switch (owner)
            {
                case PoolOwner.Player:
                    BattlefieldManager.OnPlayerDicePoolSelected += RollDicePool;
                    break;
                case PoolOwner.Opponent:
                    BattlefieldManager.OnOpponentDicePoolSelected += RollDicePool;
                    break;
                default:
                    break;
            }
        }

        private void OnDisable()
        {
            switch (owner)
            {
                case PoolOwner.Player:
                    BattlefieldManager.OnPlayerDicePoolSelected -= RollDicePool;
                    break;
                case PoolOwner.Opponent:
                    // BattlefieldManager.OnOpponentDicePoolSelected -= RollDicePool;
                    break;
                default:
                    break;
            }
        }

        private void RollDicePool(DiceAsset[] _dicePool)
        {
            for (int i = 0; i < _dicePool.Length; i++)
            {
                dices[i].RollDice(_dicePool[i]);
            }
        }
        #endregion

        private enum PoolOwner
        {
            None,
            Player, 
            Opponent
        }
    }
}
