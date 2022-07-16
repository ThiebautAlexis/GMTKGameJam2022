using System;
using UnityEngine;
using DG.Tweening;

namespace GMTK
{
    public class BattlefieldDicePool : MonoBehaviour
    {
        #region Fields and Properties
        [SerializeField] private Owner owner = Owner.None;
        [SerializeField] private BattlefieldDice[] dices = new BattlefieldDice[] { };
        [SerializeField] private Vector2[] dicesPosition = new Vector2[] { };
        [SerializeField] private Vector2 resetPosition;

        private Sequence dicesSequence;
        #endregion

        #region Methods 
        private void Awake()
        {
            switch (owner)
            {
                case Owner.Player:
                    BattlefieldManager.OnPlayerDicePoolSelected += RollDicePool;
                    break;
                case Owner.Opponent:
                    BattlefieldManager.OnOpponentDicePoolSelected += RollDicePool;
                    break;
                default:
                    break;
            }
            BattlefieldManager.OnPlayerAction += ResetDices;

        }

        private void OnDisable()
        {
            switch (owner)
            {
                case Owner.Player:
                    BattlefieldManager.OnPlayerDicePoolSelected -= RollDicePool;
                    break;
                case Owner.Opponent:
                    BattlefieldManager.OnOpponentDicePoolSelected -= RollDicePool;
                    break;
                default:
                    break;
            }
            BattlefieldManager.OnPlayerAction -= ResetDices;

        }

        private void ResetDices()
        {
            if (dicesSequence.IsActive())
                dicesSequence.Kill(true);
            dicesSequence = DOTween.Sequence();
            for (int i = 0; i < dices.Length; i++)
            {
                dicesSequence.AppendInterval(.25f);
                dicesSequence.Join(dices[i].transform.DOMove(resetPosition, .70f));
            }
            dicesSequence.onComplete += ResetDices;

            void ResetDices()
            {
                for (int i = 0; i < dices.Length; i++)
                {
                    dices[i].gameObject.SetActive(false); 
                }
            }
        }

        private void RollDicePool(DiceAsset[] _dicePool)
        {
            for (int i = 0; i < _dicePool.Length; i++)
            {
                dices[i].RollDice(_dicePool[i]);
            }
            if (dicesSequence.IsActive())
                dicesSequence.Kill(true);

            dicesSequence = DOTween.Sequence();
            for (int i = 0; i < dices.Length; i++)
            {
                dicesSequence.AppendInterval(.25f);
                dicesSequence.Join(dices[i].transform.DOMove(resetPosition, .70f));

            }
        }
        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < dicesPosition.Length; i++)
            {
                Gizmos.DrawSphere(dicesPosition[i], .1f);
            }
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(resetPosition, .1f);
        }
    }
}
