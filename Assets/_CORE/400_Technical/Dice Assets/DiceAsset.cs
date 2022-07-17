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
        public ArmyParticleSystem ParticleSystem;
        #endregion

        #region Methods 
        public DiceFace GetRandomFace() => dicefaces[Random.Range(0, dicefaces.Length)];

        private int faceIndex = 0;
        public Sprite GetFaceSprite()
        {
            faceIndex++;
            if (faceIndex >= dicefaces.Length)
                faceIndex = 0;

            if (dicefaces[faceIndex] == null)
                return null;

            return dicefaces[faceIndex].GetFaceSprite();
        }
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
