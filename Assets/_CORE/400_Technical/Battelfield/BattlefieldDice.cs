using System;
using UnityEngine;
using DG.Tweening;

namespace GMTK
{
    public class BattlefieldDice : MonoBehaviour, IDragAndDroppable
    {
        #region Fields and Properties
        [SerializeField] private     SpriteRenderer diceRenderer;
        [SerializeField] private     SpriteRenderer faceRenderer;
        [SerializeField] private new BoxCollider2D collider;

        private DiceFace selectedFace = null;
        private int diceIndex = -1;
        #endregion

        #region Methods 
        internal void RollDice(DiceAsset diceAsset)
        {
            diceRenderer.sprite = diceAsset.DiceSprite;
            selectedFace = diceAsset.GetRandomFace();
            // Sequence to roll dice here
            if (selectedFace)
                faceRenderer.sprite = selectedFace.GetFaceSprite();
            else faceRenderer.sprite = null;

            gameObject.SetActive(true);
        }
        #endregion

        #region I Drag And Droppable
        public void DragUpdate(Vector2 _position)
        {
            transform.position = Vector2.MoveTowards(transform.position, _position, Time.deltaTime * 200);
        }

        public void Drop()
        {
            // Check if in Battle Horn

        }

        public void StartDrag()
        {

        }
        #endregion
    }
}
