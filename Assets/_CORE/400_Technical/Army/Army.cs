using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GMTK
{
    public class Army
    {
        public static Army PlayerArmy = new Army() { ArmyID = 1};
        public static Army OpponentArmy = new Army() { ArmyID = -1};

        public event Action<UnitType> OnArmyTakeDamages;
        public event Action<UnitType, int> OnArmyAttackTarget;
        public event Action OnArmyModified;


        #region Fields and Properties
        public int ArmyID = 0;
        public List<DiceAsset> diceReserve = new List<DiceAsset>();
        public List<DiceAsset> diceUsed = new List<DiceAsset>();
        public List<DiceAsset> diceUnavailable = new List<DiceAsset>();

        public bool IsArmyAlive => ((diceReserve.Count > 0) && (diceUsed.Count > 0));
        #endregion

        #region Methods 
        public void InitArmy(DiceAsset[] _dices)
        {
            diceReserve = new List<DiceAsset>(_dices);
            diceUsed.Clear();
            diceUnavailable.Clear();
            OnArmyModified?.Invoke();
        }

        public DiceAsset[] RollDices(int _rollLength)
        {
            DiceAsset[] _roll = new DiceAsset[_rollLength];
            for (int i = 0; i < _rollLength; i++)
            {
                if (diceReserve.Count == 0)
                    RefreshReserve();
                if (diceReserve.Count == 0 && diceUsed.Count == 0) return _roll;

                int _index = Random.Range(0, diceReserve.Count);
                _roll[i] = diceReserve[_index];

                diceReserve.RemoveAt(_index);
            }
            OnArmyModified?.Invoke();
            return _roll;
        }

        internal void SendToUsedDices(DiceAsset[] _usedDices)
        {
            for (int i = 0; i < _usedDices.Length; i++)
            {
                if (_usedDices[i] == null) continue;
                diceUsed.Add(_usedDices[i]);
            }
            OnArmyModified?.Invoke();
        }

        private void RefreshReserve()
        {
            diceReserve = new List<DiceAsset>(diceUsed);
            diceUsed.Clear();
            OnArmyModified?.Invoke();
        }

        public void TakeDamages(int _damages)
        {
            UnitType _type = UnitType.Default;
            for (int i = 0; i < _damages; i++)
            {
                if(diceUsed.Count > 0)
                {
                    int _index = Random.Range(0, diceUsed.Count);
                    _type = diceUsed[_index].UnitType;
                    diceUnavailable.Add(diceUsed[_index]);
                    diceUsed.RemoveAt(_index);

                    OnArmyTakeDamages?.Invoke(_type);
                    OnArmyModified?.Invoke();
                }
                else if(diceReserve.Count > 0)
                {
                    int _index = Random.Range(0, diceReserve.Count);
                    _type = diceReserve[_index].UnitType;
                    diceUnavailable.Add(diceReserve[_index]);
                    diceReserve.RemoveAt(_index);

                    OnArmyTakeDamages?.Invoke(_type);
                    OnArmyModified?.Invoke();
                }

                if (diceUsed.Count == 0 && diceReserve.Count == 0)
                {
                    OnArmyModified?.Invoke();
                    break;
                }
            }

        }

        public void AttackTarget(UnitType _type, int _targetIndex)
        {
            OnArmyAttackTarget?.Invoke(_type, _targetIndex);
        }
        #endregion

    }
        public enum Owner
        {
            None = 0,
            Player = 1,
            Opponent = -1
        }
}
