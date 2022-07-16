namespace GMTK
{
    public interface IDragAndDroppable
    {
        #region Fields and Properties
        #endregion

        #region Methods 
        public void StartDrag();
        void DragUpdate(UnityEngine.Vector2 _position);
        public void Drop();
        #endregion
    }
}
