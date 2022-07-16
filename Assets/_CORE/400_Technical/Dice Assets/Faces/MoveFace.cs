using System;
using UnityEngine;

namespace GMTK
{
    [CreateAssetMenu(fileName = "MoveFaceAsset_", menuName = "GMTK/Faces/Movement Face Asset", order = 0)]
    public class MoveFace : DiceFace
    {
        public override Behaviour FaceBehaviour => Behaviour.Movement;
        #region Fields and Properties
        [Header("Mouvement Face")]
        [SerializeField, Range(-4, 4)] private int range = 1;
        [SerializeField, Range(-4, 4)] private int rangeUpgraded = 2;
        #endregion

        #region Methods 

        public override void ApplyBehaviour()
        {
            Debug.Log("Hop hop hop!");
            throw new NotImplementedException();
        }
        #endregion
    }
}
