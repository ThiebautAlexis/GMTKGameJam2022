using System;
using UnityEngine;

namespace GMTK
{
    [CreateAssetMenu(fileName = "DB_", menuName = "GMTK/Dice Database", order = 0)]

    public class DiceDatabase : ScriptableObject
    {
        public DiceAsset[] dice = new DiceAsset[] { };
    }
}
