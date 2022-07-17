using System;
using UnityEngine;

namespace GMTK
{
    public abstract class GameState
    {
        public abstract int Priority { get; protected set; }
        public abstract bool isActive { get; set; }
    }

    public class InMenuSate : GameState
    {
        public override int Priority { get; protected set; } = 0;
        public override bool isActive { get; set; } = false;
    }

    public class InBattleState : GameState
    {
        public override int Priority { get; protected set; } = 1;
        public override bool isActive { get; set; } = false;

        #region Methods

        #endregion 
    }

    public class EndOfBattleState : GameState
    {
        public override int Priority { get; protected set; } = 2;
        public override bool isActive { get; set; } = false;

        #region Methods

        #endregion
    }

    public class PauseState : GameState
    {
        public override int Priority { get; protected set; } = 3;
        public override bool isActive { get; set; } = false;

        #region Methods

        #endregion
    }
}
