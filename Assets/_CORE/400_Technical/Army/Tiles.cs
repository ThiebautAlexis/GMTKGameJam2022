using System;
using UnityEngine;

namespace GMTK
{
    public class Tiles : MonoBehaviour
    {
        public Vector2[] TilesPosition = new Vector2[9];
        [SerializeField] private ParticleSystem playerSlash;
        [SerializeField] private ParticleSystem playerImpact;

        [SerializeField] private ParticleSystem opponentSlash;
        [SerializeField] private ParticleSystem opponentImpact;

        [SerializeField] private ParticleSystem playerArrows;
        [SerializeField] private ParticleSystem opponentsArrows;

        private void Awake()
        {
            Army.PlayerArmy.OnArmyAttackTarget += DisplayPlayerVFX;
            Army.OpponentArmy.OnArmyAttackTarget += DisplayOpponentVFX;
        }

        private void OnDisable()
        {
            Army.PlayerArmy.OnArmyAttackTarget -= DisplayPlayerVFX;
            Army.OpponentArmy.OnArmyAttackTarget -= DisplayOpponentVFX;

        }

        private void DisplayPlayerVFX(UnitType _type, int _index)
        {
            
            switch (_type)
            {

                case UnitType.Swordsmen:
                    playerSlash.transform.position = TilesPosition[_index] + Vector2.up;
                    playerSlash.Play();
                    break;
                case UnitType.Spearmen:
                    playerImpact.transform.position = TilesPosition[_index] + Vector2.up;
                    playerImpact.Play();
                    break;
                case UnitType.Archers:
                    playerArrows.transform.position = TilesPosition[Mathf.Max(0, _index - 2)] + Vector2.up;
                    playerArrows.Play();
                    break;
                default:
                    break;
            }
        }

        private void DisplayOpponentVFX(UnitType _type, int _index)
        {
            switch (_type)
            {

                case UnitType.Swordsmen:
                    opponentSlash.transform.position = TilesPosition[_index] + Vector2.up;
                    opponentSlash.Play();
                    break;
                case UnitType.Spearmen:
                    opponentImpact.transform.position = TilesPosition[_index] + Vector2.up;
                    opponentImpact.Play();
                    break;
                case UnitType.Archers:
                    opponentsArrows.transform.position = TilesPosition[Mathf.Min(TilesPosition.Length - 1, _index + 2)] + Vector2.up;
                    opponentsArrows.Play();
                    break;
                default:
                    break;
            }
        }
        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < TilesPosition.Length; i++)
            {
                Gizmos.DrawSphere(TilesPosition[i], .1f);
            }
        }
    }
}
