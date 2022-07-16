using UnityEngine;

namespace GMTK
{
    [CreateAssetMenu(fileName = "DiceAsset_", menuName = "GMTK/Dice Asset", order = 0)]
    public class DiceAsset : ScriptableObject
    {
        #region Fields and Properties
        [SerializeField] private DiceFace[] dicefaces = new DiceFace[6];
        public UnitType UnitType = UnitType.Default;
        public Sprite DiceSprite; 
        #endregion

        #region Methods 
        public DiceFace GetRandomFace() => dicefaces[Random.Range(0, dicefaces.Length)];
        #endregion
    }

    public enum UnitType
    {
        Default,
        Swordsmen,
        Spearmen,
        Archers
    }
}
