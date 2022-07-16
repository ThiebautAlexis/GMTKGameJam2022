using System;
using UnityEngine;

namespace GMTK
{
    public class AIController : MonoBehaviour
    {
        #region Fields and Properties
        [SerializeField] private DiceAsset[] dices = new DiceAsset[] { };
        #endregion

        #region Methods 
        private void Awake()
        {
            Army.OpponentArmy.InitArmy(dices);
        }
        #endregion
    }
}
