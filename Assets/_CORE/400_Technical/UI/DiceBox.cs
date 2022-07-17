using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GMTK
{
    public class DiceBox : MonoBehaviour
    {
        #region Fields and Properties
        [SerializeField] private Owner owner = Owner.None;
        [Header("Army")]
        [SerializeField] private TMP_Text swordsTextArmy;
        [SerializeField] private TMP_Text spearTextArmy;
        [SerializeField] private TMP_Text bowTextArmy;
        [Header("Exhausted")]
        [SerializeField] private TMP_Text swordsTextExhausted;
        [SerializeField] private TMP_Text spearTextExhausted;
        [SerializeField] private TMP_Text bowTextExhausted;
        [Header("Wounded")]      
        [SerializeField] private TMP_Text swordsTextWounded;
        [SerializeField] private TMP_Text spearTextWounded;
        [SerializeField] private TMP_Text bowTextWounded;
        #endregion

        #region Methods 
        private void Awake()
        {
            switch (owner)
            {
                case Owner.Player:
                    Army.PlayerArmy.OnArmyModified += UpdateDiceBox;
                    break;
                case Owner.Opponent:
                    Army.OpponentArmy.OnArmyModified += UpdateDiceBox;
                    break;
                default:
                    break;
            }
        }

        private void OnDisable()
        {
            switch (owner)
            {
                case Owner.Player:
                    Army.PlayerArmy.OnArmyModified -= UpdateDiceBox;
                    break;
                case Owner.Opponent:
                    Army.OpponentArmy.OnArmyModified -= UpdateDiceBox;
                    break;
                default:
                    break;
            }
        }

        private void UpdateDiceBox()
        {
            List<DiceAsset> armyDices;
            List<DiceAsset> exhaustedDices;
            List<DiceAsset> woundedDices;
            switch (owner)
            {
                case Owner.Player:
                    armyDices = Army.PlayerArmy.diceReserve;
                    exhaustedDices = Army.PlayerArmy.diceUsed;
                    woundedDices = Army.PlayerArmy.diceUnavailable;
                    break;
                case Owner.Opponent:
                   armyDices = Army.OpponentArmy.diceReserve;
                   exhaustedDices = Army.OpponentArmy.diceUsed;
                   woundedDices = Army.OpponentArmy.diceUnavailable;
                    break;
                default:
                    return;
            }
            int _bowCount = 0, _swordCount = 0, _spearCount = 0;
            for (int i = 0; i < armyDices.Count; i++)
            {
                if (armyDices[i].UnitType == UnitType.Archers)
                    _bowCount++;
                else if (armyDices[i].UnitType == UnitType.Swordsmen)
                    _swordCount++;
                else if (armyDices[i].UnitType == UnitType.Spearmen)
                    _spearCount++;
            }
            swordsTextArmy.text = _swordCount.ToString();
            spearTextArmy.text = _spearCount.ToString();
            bowTextArmy.text = _bowCount.ToString();

            _bowCount  = _swordCount = _spearCount = 0;
            for (int i = 0; i < exhaustedDices.Count; i++)
            {
                if (exhaustedDices[i].UnitType == UnitType.Archers)
                    _bowCount++;
                else if (exhaustedDices[i].UnitType == UnitType.Swordsmen)
                    _swordCount++;
                else if (exhaustedDices[i].UnitType == UnitType.Spearmen)
                    _spearCount++;
            }
            swordsTextExhausted.text = _swordCount.ToString();
            spearTextExhausted.text = _spearCount.ToString();
            bowTextExhausted.text = _bowCount.ToString();

            _bowCount = _swordCount = _spearCount = 0;
            for (int i = 0; i < woundedDices.Count; i++)
            {
                if (woundedDices[i].UnitType == UnitType.Archers)
                    _bowCount++;
                else if (woundedDices[i].UnitType == UnitType.Swordsmen)
                    _swordCount++;
                else if (woundedDices[i].UnitType == UnitType.Spearmen)
                    _spearCount++;
            }
            swordsTextWounded.text = _swordCount.ToString();
            spearTextWounded.text = _spearCount.ToString();
            bowTextWounded.text = _bowCount.ToString();
        }


        #endregion
    }
}
