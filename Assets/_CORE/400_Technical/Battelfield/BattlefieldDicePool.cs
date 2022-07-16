using System;
using UnityEngine;

namespace GMTK
{
    public class BattlefieldDicePool : MonoBehaviour
    {
        #region Fields and Properties
        [SerializeField] private Owner owner = Owner.None;
        [SerializeField] private BattlefieldDice[] dices = new BattlefieldDice[] { };
        [SerializeField] private Vector2[] dicesPosition = new Vector2[] { };
        #endregion

        #region Methods 
        private void Awake()
        {
            switch (owner)
            {
                case Owner.Player:
                    BattlefieldManager.OnPlayerDicePoolSelected += RollDicePool;
                    break;
                case Owner.Opponent:
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
                case Owner.Player:
                    BattlefieldManager.OnPlayerDicePoolSelected -= RollDicePool;
                    break;
                case Owner.Opponent:
                    BattlefieldManager.OnOpponentDicePoolSelected -= RollDicePool;
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

    }
}
