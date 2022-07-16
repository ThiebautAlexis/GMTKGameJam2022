using System;
using UnityEngine;

namespace GMTK
{
    [CreateAssetMenu(fileName = "AttackFaceAsset_", menuName = "GMTK/Faces/Attack Face Asset", order = 0)]
    public class AttackFace : DiceFace
    {
        public override Behaviour FaceBehaviour => Behaviour.Attack;
        
        #region Fields and Properties
        [Header("Attack Face")]
        [SerializeField, Range(1,4)] private int range = 1;
        [SerializeField, Range(1,4)] private int rangeUpgraded = 2;

        [SerializeField, Range(1, 4)] private int damages = 1;
        [SerializeField, Range(1, 4)] private int damagesUpgraded = 1;
        #endregion

        #region Methods 
        public override void ApplyBehaviour()
        {
            Debug.Log("Et vlan!");
            throw new NotImplementedException();
        }
        #endregion
    }
}
