using System;
using UnityEngine;
using DG.Tweening;

namespace GMTK
{
    public static class BattlefieldManager
    {
        #region Events 
        public static event Action<DiceAsset[]> OnPlayerDicePoolSelected;
        public static event Action<DiceAsset[]> OnOpponentDicePoolSelected;

        public static event Action<int> OnOpponentActionSelected;

        public static event Action<int, int> OnPlayerArmyMove;
        public static event Action<int, int> OnOpponentArmyMove;


        public static event Action<UnitType, int, int, bool> OnPlayerArmyAttack;
        public static event Action<UnitType, int, int, bool> OnOpponentArmyAttack;

        public static event Action OnBattleStart;
        public static event Action OnRoundStart;
        public static event Action OnPlayRoundActions;
        public static event Action OnEndRound;
        #endregion

        #region Fields and Properties
        private static readonly int diceRollCount = 3;
        private static int[] tiles = new int[7];
        private static DiceAsset[] playerDice;
        private static DiceAsset[] opponentDice;
        #endregion

        #region Methods 
        public static void StartBattle()
        {
            GameStatesManager.SetStateActivation(GameStatesManager.InGameState, true);
            tiles[1] = 1;
            OnPlayerArmyMove?.Invoke(0, 1);
            tiles[5] = -1;
            OnOpponentArmyMove?.Invoke(0, 5);
            OnBattleStart?.Invoke();
        }

        public static void StartNewRound()
        {
            playerDice = Army.PlayerArmy.RollDices(diceRollCount);
            OnPlayerDicePoolSelected?.Invoke(playerDice);

            opponentDice = Army.OpponentArmy.RollDices(diceRollCount);
            OnOpponentDicePoolSelected?.Invoke(opponentDice);
        }
        public static void EndRound()
        {
            Army.PlayerArmy.SendToUsedDices(playerDice);
            Army.OpponentArmy.SendToUsedDices(opponentDice);
            OnEndRound?.Invoke();
        }

        internal static void PlayAction(DiceFace selectedFace, int _armyID, UnitType _unitType )
        {
            if(selectedFace != null)
            {
                selectedFace.ApplyBehaviour(ref tiles, _armyID, out int baseTile, out int targetTile, out bool _inflictDamages);
                switch (selectedFace.FaceBehaviour)
                {
                    case DiceFace.Behaviour.Attack:
                        if (_armyID > 0)
                            OnPlayerArmyAttack?.Invoke(_unitType, baseTile, targetTile, _inflictDamages);
                        else if (_armyID < 0)
                            OnOpponentArmyAttack?.Invoke(_unitType, baseTile, targetTile, _inflictDamages);
                        break;
                    case DiceFace.Behaviour.Movement:
                        if (_armyID > 0)
                            OnPlayerArmyMove?.Invoke(baseTile, targetTile);
                        else if (_armyID < 0)
                            OnOpponentArmyMove?.Invoke(baseTile, targetTile);
                        break;
                    default:
                        break;
                }
            }
        }

        private static Sequence roundSequence;
        internal static void SelectPlayerAction(DiceFace selectedFace, UnitType unitType)
        {
            DiceFace _opponentFace = AIController.SelectBestAction(selectedFace, tiles ,out int opponentDiceIndex);
            OnOpponentActionSelected?.Invoke(opponentDiceIndex);

            roundSequence = DOTween.Sequence();
            {
                roundSequence.AppendCallback(() => OnPlayRoundActions?.Invoke());
                roundSequence.AppendInterval(1.0f);
                roundSequence.AppendCallback(() => PlayAction(selectedFace, 1, unitType));
                roundSequence.AppendInterval(1.0f);
                roundSequence.AppendCallback(() => PlayAction(_opponentFace, -1, opponentDice[opponentDiceIndex].UnitType));
                roundSequence.AppendInterval(1.0f);
                roundSequence.onComplete += EndRound;
            }
        }
        #endregion
    }
}
