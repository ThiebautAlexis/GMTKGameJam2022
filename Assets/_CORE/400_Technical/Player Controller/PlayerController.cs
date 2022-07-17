using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GMTK
{
    public class PlayerController : MonoBehaviour
    {
        #region Fields and Properties
        // [Header("Base Settings")]
        // [SerializeField] private DiceAsset[] baseDices = new DiceAsset[] { };
        [Header("Inputs")]
        [SerializeField] private PlayerInputs playerInputs = null;
        [Header("Camera & Interactions")]
        [SerializeField] private new Camera camera = null;
        [SerializeField] private LayerMask interactibleMask = new LayerMask();
        private IDragAndDroppable dragAndDroppable = null;
        private bool hasDraggable = false;
        private bool canInteract = false;
        [SerializeField] private DiceDatabase diceBase;
        private List<DiceAsset> diceArmy;

        [SerializeField] private AudioSource source;
        #endregion

        #region Methods 
        private void Awake()
        {
            playerInputs.Init(this);
            DiceAsset[] _temp = new DiceAsset[diceBase.dice.Length];
            Array.Copy(diceBase.dice, _temp, diceBase.dice.Length);
            diceArmy = new List<DiceAsset>(_temp);

            GameStatesManager.OnChangeState += SetActivity;
            GameStatesManager.OnChangeState += StartBattle;
            

        }
        private void OnDisable()
        {
            GameStatesManager.OnChangeState -= SetActivity;
            GameStatesManager.OnChangeState -= StartBattle;
        }



        private void StartBattle(Type obj)
        {
            if(obj == GameStatesManager.InGameState)
            {
                Army.PlayerArmy.InitArmy(diceArmy.ToArray());
                BattlefieldManager.StartBattle();
            }
        }

        RaycastHit2D[] hit = new RaycastHit2D[1];
        internal void OnInputPerformed(InputAction.CallbackContext context)
        {
            Debug.Log(BattlefieldManager.RoundState);
            if (!canInteract ||  BattlefieldManager.RoundState != RoundState.WaitingForPlayerInput) return;
            if (Physics2D.RaycastNonAlloc(camera.ScreenToWorldPoint(playerInputs.MousePosition.ReadValue<Vector2>()), Vector3.forward, hit, camera.farClipPlane, interactibleMask) > 0)
            {
                if(hit[0].collider.TryGetComponent(out dragAndDroppable))
                {
                    hasDraggable = dragAndDroppable.StartDrag();
                }
            }
        }

        internal void OnInputCanceled(InputAction.CallbackContext context)
        {
            if(hasDraggable && canInteract && BattlefieldManager.RoundState == RoundState.WaitingForPlayerInput)
            {
                dragAndDroppable.Drop();
                hasDraggable = false;
            }
        }
        internal void UpdateMousePosition(InputAction.CallbackContext obj)
        {
            if (hasDraggable && canInteract && BattlefieldManager.RoundState == RoundState.WaitingForPlayerInput)
            {
                dragAndDroppable.DragUpdate(camera.ScreenToWorldPoint(obj.action.ReadValue<Vector2>()));
            }
        }

        public void RollTheDices()
        {
            if (canInteract && BattlefieldManager.RoundState == RoundState.WaitingForDiceRoll)
            {
                source.Play();
                BattlefieldManager.RollDices();
            }
        }


        private void SetActivity(Type gameState)
        {
            canInteract = gameState == GameStatesManager.InGameState;
        }
        #endregion
    }
}
