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


        public static event Action OnBeforeBattleStart;
        public static event Action OnBattleStart;
        public static event Action OnRoundStart;
        public static event Action OnDiceCanRoll;
        public static event Action OnPlayRoundActions;
        public static event Action OnEndRound;

        public static event Action OnEndBattle;
        public static event Action OnBattleWon;
        public static event Action OnBattleLost;

        public static event Action OnGameWin;
        #endregion

        #region Fields and Properties
        private static readonly int diceRollCount = 3;
        private static int[] tiles = new int[7];
        private static DiceAsset[] playerDice;
        private static DiceAsset[] opponentDice;
        public static RoundState RoundState { get; private set; } = RoundState.NotInRound;
        #endregion

        #region Methods 
        public static void StartBattle()
        {
            tiles[1] = 1;
            OnPlayerArmyMove?.Invoke(0, 1);
            tiles[5] = -1;
            OnOpponentArmyMove?.Invoke(0, 5);
            SetRoundState(RoundState.BattleStarted);
        }

        public static void RollDices()
        {
            SetRoundState(RoundState.WaitingForPlayerInput);

            playerDice = Army.PlayerArmy.RollDices(diceRollCount);
            OnPlayerDicePoolSelected?.Invoke(playerDice);

            opponentDice = Army.OpponentArmy.RollDices(diceRollCount);
            OnOpponentDicePoolSelected?.Invoke(opponentDice);

        }
        private static void EndRound() => OnEndRound?.Invoke();

        internal static void PlayAction(DiceFace selectedFace, int _armyID, UnitType _unitType )
        {
            if(selectedFace != null)
            {
                selectedFace.ApplyBehaviour(ref tiles, _armyID, out int baseTile, out int targetTile, out int _inflictDamages);
                switch (selectedFace.FaceBehaviour)
                {
                    case DiceFace.Behaviour.Attack:
                        if (_armyID > 0)
                        {
                            Army.PlayerArmy.AttackTarget(_unitType, targetTile);
                            Army.OpponentArmy.TakeDamages(_inflictDamages); 
                        }
                        else if (_armyID < 0)
                        {
                            Army.OpponentArmy.AttackTarget(_unitType, targetTile);
                            Army.PlayerArmy.TakeDamages(_inflictDamages);
                        }
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

        internal static void WonGame()
        {
            OnGameWin?.Invoke();
            GameStatesManager.SetStateActivation(GameStatesManager.InGameState, false);
        }

        private static Sequence roundSequence;
        internal static void SelectPlayerAction(DiceFace selectedFace, UnitType unitType)
        {
            DiceFace _opponentFace = AIController.SelectBestAction(selectedFace, tiles, out int opponentDiceIndex);
            OnOpponentActionSelected?.Invoke(opponentDiceIndex);

            SetRoundState(RoundState.PlayingAction);
            Army.PlayerArmy.SendToUsedDices(playerDice);
            Army.OpponentArmy.SendToUsedDices(opponentDice);

            roundSequence = DOTween.Sequence();
            {
                roundSequence.AppendInterval(1.0f);
                roundSequence.AppendCallback(() => PlayAction(selectedFace, 1, unitType));
                roundSequence.AppendInterval(1.0f);
                roundSequence.AppendCallback(() => PlayAction(_opponentFace, -1, opponentDice[opponentDiceIndex].UnitType));
                roundSequence.AppendCallback(EndRound);
                roundSequence.AppendInterval(3f);
                roundSequence.AppendCallback(() => SetRoundState(RoundState.EndingRound));
            }
        }

        public static void SetRoundState(RoundState _state)
        {
            switch (_state)
            {
                case RoundState.NotInRound:
                    break;
                case RoundState.BattleStarted:
                    OnBeforeBattleStart?.Invoke();
                    OnBattleStart?.Invoke();
                    SetRoundState(RoundState.RoundStarted);
                    return;
                case RoundState.RoundStarted:
                    OnRoundStart?.Invoke();
                    SetRoundState(RoundState.WaitingForDiceRoll);
                    return;
                case RoundState.WaitingForDiceRoll:
                    OnDiceCanRoll?.Invoke();
                    break;
                case RoundState.PlayingAction:
                    OnPlayRoundActions?.Invoke();
                    break;
                case RoundState.EndingRound:
                    ProceedToNextRound();
                    return;
                case RoundState.EndingBattle:
                    OnEndBattle?.Invoke();
                    GameStatesManager.SetStateActivation(GameStatesManager.EndOfBattleState, true);
                    break;
                default:
                    break;
            }
            RoundState = _state;
        }

        private static void ProceedToNextRound()
        {
            if(Army.OpponentArmy.diceReserve.Count == 0 &&  Army.OpponentArmy.diceUsed.Count == 0)
            {
                SetRoundState(RoundState.EndingBattle);
                OnBattleWon?.Invoke();
                Debug.Log("Win");
                return;
            }
            if (Army.PlayerArmy.diceReserve.Count == 0 && Army.PlayerArmy.diceUsed.Count == 0)
            {
                SetRoundState(RoundState.EndingBattle);
                OnBattleLost?.Invoke();
                Debug.Log("Loose");
                return;
            }

            SetRoundState(RoundState.RoundStarted);
        }
        #endregion
    }

    public enum RoundState
    {
        NotInRound,
        BattleStarted,
        RoundStarted,
        WaitingForDiceRoll,
        WaitingForPlayerInput,
        PlayingAction, 
        EndingRound,
        EndingBattle
    }
}
