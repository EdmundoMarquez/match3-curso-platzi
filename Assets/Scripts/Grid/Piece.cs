namespace Match3.Grid
{
    using UnityEngine;
    using DG.Tweening;
    
    public class Piece : MonoBehaviour 
    {
        [SerializeField] private PieceTypes _pieceType;
        public int _x {get; private set;}
        public int _y {get; private set;}
        private GridController _gridController;
        

        public void Init(int x, int y, GridController gridController)
        {
            _x = x;
            _y = y;
            _gridController = gridController;
        }

        public void Move(int x, int y)
        {
            transform.DOMove(new Vector2(x,y), 0.25f).SetEase(Ease.InOutCubic).onComplete = () =>
            {
                _x = x;
                _y = y;
            };
        }

        public PieceTypes GetType()
        {
            return _pieceType;
        }
    }
}