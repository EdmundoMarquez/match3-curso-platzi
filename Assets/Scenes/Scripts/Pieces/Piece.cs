namespace Match3.Pieces
{
    using UnityEngine;
    
    public class Piece : MonoBehaviour 
    {
        [SerializeField] private PieceTypes _pieceType;
        private int _x, _y;

        public void Init(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }
}