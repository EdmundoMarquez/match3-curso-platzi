namespace Match3.Grid
{
    using UnityEngine;
    
    public class Tile : MonoBehaviour 
    {
        public int _x {get; private set;}
        public int _y {get; private set;}
        private PiecesSwaper _piecesSwaper;

        public void Init(int x, int y, PiecesSwaper piecesSwaper)
        {
            _x = x;
            _y = y;
            _piecesSwaper = piecesSwaper;
        }

        private void OnMouseDown() 
        {
            _piecesSwaper.OnTileDown(this);
        }

        private void OnMouseEnter()
        {
            _piecesSwaper.OnTileOver(this);
        }
        
        private void OnMouseUp()
        {
            _piecesSwaper.OnTileUp(this);
        }
    }
}