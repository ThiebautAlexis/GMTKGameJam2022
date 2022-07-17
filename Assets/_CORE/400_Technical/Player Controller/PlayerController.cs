using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GMTK
{
    public class PlayerController : MonoBehaviour
    {
        #region Fields and Properties
        [Header("Base Settings")]
        [SerializeField] private DiceAsset[] baseDices = new DiceAsset[] { };
        [Header("Inputs")]
        [SerializeField] private PlayerInputs playerInputs = null;
        [Header("Camera & Interactions")]
        [SerializeField] private new Camera camera = null;
        [SerializeField] private LayerMask interactibleMask = new LayerMask();
        private IDragAndDroppable dragAndDroppable = null;
        private bool hasDraggable = false;
        #endregion

        #region Methods 
        private void Start()
        {
            Army.PlayerArmy.InitArmy(baseDices);
            playerInputs.Init(this);

            BattlefieldManager.StartBattle();
        }

        RaycastHit2D[] hit = new RaycastHit2D[1];
        internal void OnInputPerformed(InputAction.CallbackContext context)
        {
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
            if(hasDraggable)
            {
                dragAndDroppable.Drop();
                hasDraggable = false;
            }
        }
        internal void UpdateMousePosition(InputAction.CallbackContext obj)
        {
            if (hasDraggable)
            {
                dragAndDroppable.DragUpdate(camera.ScreenToWorldPoint(obj.action.ReadValue<Vector2>()));
            }
        }

        public void RollTheDices() => BattlefieldManager.StartNewRound();
        #endregion
    }
}
