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

        public static event Action<UnitType, int> OnArmyTakeDamages;

        #region Fields and Properties
        public int ArmyID = 0;
        public List<DiceAsset> diceReserve = new List<DiceAsset>();
        public List<DiceAsset> diceUsed = new List<DiceAsset>();
        public List<DiceAsset> diceUnavailable = new List<DiceAsset>();
        #endregion

        #region Methods 
        public void InitArmy(DiceAsset[] _dices)
        {
            diceReserve = new List<DiceAsset>(_dices);
            diceUsed.Clear();
            diceUnavailable.Clear();
        }

        public DiceAsset[] RollDices(int _rollLength)
        {
            DiceAsset[] _roll = new DiceAsset[_rollLength];
            if (diceReserve.Count == 0 && diceUsed.Count == 0) return _roll;
            for (int i = 0; i < _rollLength; i++)
            {
                if (diceReserve.Count == 0)
                    RefreshReserve();

                int _index = Random.Range(0, diceReserve.Count);
                _roll[i] = diceReserve[_index];

                diceReserve.RemoveAt(_index);
            }
            return _roll;
        }

        internal void SendToUsedDices(DiceAsset[] _usedDices) => diceUsed.AddRange(_usedDices);

        private void RefreshReserve()
        {
            diceReserve = new List<DiceAsset>(diceUsed);
            diceUsed.Clear();
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
                    OnArmyTakeDamages?.Invoke(_type, ArmyID);
                }
                else if(diceReserve.Count > 0)
                {
                    int _index = Random.Range(0, diceReserve.Count);
                    _type = diceReserve[_index].UnitType;
                    diceUnavailable.Add(diceReserve[_index]);
                    diceReserve.RemoveAt(_index);
                    OnArmyTakeDamages?.Invoke(_type, ArmyID);
                }

                if(diceUsed.Count == 0 && diceReserve.Count == 0)
                {
                    OnDie(ArmyID);
                    break;
                }
            }

        }

        public void OnDie(int _armyID)
        {
            if (_armyID > 0)
                Debug.Log("Victory!");
            else
                Debug.Log("Defeat");
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
