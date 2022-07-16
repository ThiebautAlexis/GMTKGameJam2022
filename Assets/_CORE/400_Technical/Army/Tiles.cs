using System;
using UnityEngine;

namespace GMTK
{
    public class Tiles : MonoBehaviour
    {
        public Vector2[] TilesPosition = new Vector2[9];

        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < TilesPosition.Length; i++)
            {
                Gizmos.DrawSphere((Vector2)transform.position + TilesPosition[i], .1f);
            }
        }
    }
}
