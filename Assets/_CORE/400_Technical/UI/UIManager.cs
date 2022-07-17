using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

namespace GMTK
{
    public class UIManager : MonoBehaviour
    {
        #region Fields and Properties
        [SerializeField] private CanvasGroup mainMenuCanvasGroup;
        [SerializeField] private CanvasGroup inGameCanvasGroup;
        [SerializeField] private CanvasGroup winCanvasGroup;
        [SerializeField] private CanvasGroup looseCanvasGroup;
        [SerializeField] private CanvasGroup WinGameCanvasGroup;
        [SerializeField] private CanvasGroup fadingScreen;
        [SerializeField] private FadeInOutAttributes attributes;

        [Space]
        [SerializeField] private CustomButton rollDiceButton;
        #endregion

        #region Methods 
        private void Awake()
        {
            GameStatesManager.OnChangeState += FadeTransition;
            BattlefieldManager.OnBattleWon += DisplayWinPanel;
            BattlefieldManager.OnBattleLost += DisplayLoosePanel;
            BattlefieldManager.OnDiceCanRoll += EnableRoll;
            BattlefieldManager.OnGameWin += DisplayEndGamePanel;

            GameStatesManager.SetStateActivation(GameStatesManager.InMenuState, true);
        }

        private void OnDisable()
        {
            GameStatesManager.OnChangeState -= FadeTransition;
            BattlefieldManager.OnBattleWon -= DisplayWinPanel;
            BattlefieldManager.OnBattleLost -= DisplayLoosePanel;
            BattlefieldManager.OnDiceCanRoll -= EnableRoll;
            BattlefieldManager.OnGameWin -= DisplayEndGamePanel;


        }

        private void DisplayEndGamePanel()
        {
            Sequence miniSeqiuencce = DOTween.Sequence();
            miniSeqiuencce.AppendInterval(attributes.fadeInDuration);
            miniSeqiuencce.AppendCallback(DisplayPanel);

            void DisplayPanel()
            {
                WinGameCanvasGroup.gameObject.SetActive(true);
                winCanvasGroup.gameObject.SetActive(false);
            }
        }

        private void EnableRoll() => rollDiceButton.SetInteractible(true);

        private void DisplayLoosePanel()
        {
            Sequence miniSeqiuencce = DOTween.Sequence();
            miniSeqiuencce.AppendInterval(attributes.fadeInDuration);
            miniSeqiuencce.AppendCallback(DisplayPanel);

            void DisplayPanel()
            {
                looseCanvasGroup.gameObject.SetActive(true);
            }
        }

        private void DisplayWinPanel()
        {
            Sequence miniSeqiuencce = DOTween.Sequence();
            miniSeqiuencce.AppendInterval(attributes.fadeInDuration + .1f);
            miniSeqiuencce.AppendCallback(DisplayPanel);

            void DisplayPanel()
            {
                if(!WinGameCanvasGroup.gameObject.activeInHierarchy)
                    winCanvasGroup.gameObject.SetActive(true);
            }
        }



        private Sequence fadingSequence; 
        public void FadeTransition(Type _type)
        {
            if(fadingSequence.IsActive())
            {
                fadingSequence.Kill();
            }
            fadingSequence = DOTween.Sequence();
            fadingSequence.Append(DOTween.To(a => fadingScreen.alpha = a, fadingScreen.alpha, 1, attributes.fadeInDuration).SetEase(attributes.fadeInEase));

            if (_type == typeof(InMenuSate))
                fadingSequence.AppendCallback(DisplayMainMenu);
            else if (_type == typeof(InBattleState))
                fadingSequence.AppendCallback(DisplayInGame);
            else fadingSequence.AppendInterval(.25f);

            fadingSequence.Append(DOTween.To(a => fadingScreen.alpha = a, 1, 0, attributes.fadeOutDuration).SetEase(attributes.fadeOutEase));
        }

        private void DisplayMainMenu()
        {
            mainMenuCanvasGroup.alpha = 1;
            inGameCanvasGroup.alpha = 0;
            mainMenuCanvasGroup.blocksRaycasts = true;
            inGameCanvasGroup.blocksRaycasts = false;
        }

        private void DisplayInGame()
        {
            mainMenuCanvasGroup.alpha = 0;
            inGameCanvasGroup.alpha = 1;
            inGameCanvasGroup.blocksRaycasts = true;
            mainMenuCanvasGroup.blocksRaycasts = false;
        }

        public void StartGame()
        {
            GameStatesManager.SetStateActivation(GameStatesManager.InGameState, true);
        }

        public void ProceedToNextBattle()
        {
            GameStatesManager.SetStateActivation(GameStatesManager.EndOfBattleState, false);
        }

        public void GoBackToMainMenu()
        {
            GameStatesManager.SetStateActivation(GameStatesManager.InGameState, false);

        }

        public void QuitGame() => Application.Quit();
        #endregion
    }
}
