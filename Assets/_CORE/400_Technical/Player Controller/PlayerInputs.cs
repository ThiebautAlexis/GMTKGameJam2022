using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GMTK
{
    [CreateAssetMenu(fileName = "PlayerInputs", menuName = "GMTK/Player Inputs", order = 0)]
    public class PlayerInputs : ScriptableObject
    {
        #region Fields and Properties
        [SerializeField] private InputAction clickAction = new InputAction();
        [SerializeField] private InputAction mousePosition = new InputAction();

        public InputAction MousePosition => mousePosition;
        #endregion

        #region Methods 
        public void Init(PlayerController _controller)
        {
            clickAction.performed += _controller.OnInputPerformed;
            clickAction.canceled += _controller.OnInputCanceled;
            clickAction.Enable();

            mousePosition.performed += _controller.UpdateMousePosition;
            mousePosition.Enable();
        }

        #endregion
    }
}
