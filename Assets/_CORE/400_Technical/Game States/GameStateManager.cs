using System;
using UnityEngine;

namespace GMTK
{
    public static class GameStatesManager
    {
        public static event Action<Type> OnChangeState;


        public static readonly Type InMenuState = typeof(InMenuSate);              // 0
        public static readonly Type InGameState = typeof(InBattleState);           // 1
        public static readonly Type PauseState = typeof(PauseState);               // 2



        private static GameState[] gameStates = new GameState[]{new InMenuSate(),
                                                                new InBattleState(),
                                                                new PauseState()};

        public static Type currentGameState;

        public static void SetStateActivation(Type _t, bool _isActive)
        {
            for (int i = 0; i < gameStates.Length; i++)
            {
                if (gameStates[i].GetType() == _t)
                {
                    gameStates[i].isActive = _isActive;
                }
            }
            SelectNewState();
        }

        private static void SelectNewState()
        {
            int _bestPriority = -99;
            for (int i = 0; i < gameStates.Length; i++)
            {
                if (gameStates[i].isActive && gameStates[i].Priority > _bestPriority)
                {
                    currentGameState = gameStates[i].GetType();
                    _bestPriority = gameStates[i].Priority;
                }
            }
            OnChangeState?.Invoke(currentGameState);
        }
    }
}
