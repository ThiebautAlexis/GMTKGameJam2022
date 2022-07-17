using System;
using UnityEngine;
using DG.Tweening;

namespace GMTK
{
    public class AIController : MonoBehaviour
    {
        #region Fields and Properties
        [SerializeField] private DiceAsset[] dices = new DiceAsset[] { };
        [SerializeField] private BattlefieldDice[] battlefieldDices = new BattlefieldDice[] { };
        [SerializeField] private BattleHorn battleHorn;
        private static DiceFace[] currentFaces = new DiceFace[3];
        #endregion

        #region Methods 
        private void Awake()
        {
            Army.OpponentArmy.InitArmy(dices);
            BattlefieldManager.OnOpponentActionSelected += SelectDice;
        }

        private void OnDisable()
        {
            BattlefieldManager.OnOpponentActionSelected -= SelectDice;
        }

        private Sequence selectionSequence;
        public void SelectDice(int _diceIndex)
        {
            if (selectionSequence.IsActive())
                selectionSequence.Kill();
            selectionSequence = DOTween.Sequence();
            selectionSequence.Join(battlefieldDices[_diceIndex].transform.DOMove(battleHorn.OpponentDicePosition, .25f));
        }

        public static void SetCurrentFaces(DiceFace[] _faces) => currentFaces = _faces;

        internal static DiceFace SelectBestAction(DiceFace playerAction, int[] tiles, out int opponentDiceIndex)
        {
            int[] _tempTiles = new int[9];
            Array.Copy(tiles, _tempTiles, tiles.Length);
            if(playerAction != null && playerAction.FaceBehaviour == DiceFace.Behaviour.Movement)
                playerAction.ApplyBehaviour(ref _tempTiles, 1, out int _playerBasePosition, out int _playerTargetPosition);
            int _bestScore = -9999, _currentScore =0;
            opponentDiceIndex = -1;
            for (int i = 0; i < currentFaces.Length; i++)
            {
                if (currentFaces[i] == null)
                    _currentScore = 0;
                else 
                    currentFaces[i].ComputeScore(_tempTiles);
                if(_currentScore > _bestScore)
                {
                    opponentDiceIndex = i;
                    _bestScore = _currentScore;
                }
            }
            return currentFaces[opponentDiceIndex];
        }


        #endregion
    }
}
