namespace Match3.Grid
{
    using UnityEngine;
    
    public class Tile : MonoBehaviour 
    {
        private int _x, _y;
        private GridController _gridController;

        public void Init(int x, int y, GridController gridController)
        {
            _x = x;
            _y = y;
            _gridController = gridController;
        }
    }
}