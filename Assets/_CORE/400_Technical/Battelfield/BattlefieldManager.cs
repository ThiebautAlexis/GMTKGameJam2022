using System;
using UnityEngine;

namespace GMTK
{
    public static class BattlefieldManager
    {
        #region Events 
        public static event Action<DiceAsset[]> OnPlayerDicePoolSelected;
        public static event Action<DiceAsset[]> OnOpponentDicePoolSelected;
        #endregion

        #region Fields and Properties
        private static readonly int diceRollCount = 3;
        private static int[] tiles = new int[9];
        private static DiceAsset[] playerDice;
        private static DiceAsset[] opponentDice;
        #endregion

        #region Methods 


        public static void StartNewRound()
        {
            playerDice = Army.PlayerArmy.RollDices(diceRollCount);
            OnPlayerDicePoolSelected?.Invoke(playerDice);

            opponentDice = Army.OpponentArmy.RollDices(diceRollCount);
            OnOpponentDicePoolSelected?.Invoke(opponentDice);
        }


        public static void EndRound()
        {
            Army.PlayerArmy.SendToUnusedDices(playerDice);
            Army.OpponentArmy.SendToUnusedDices(opponentDice);

        }
        #endregion
    }
}
