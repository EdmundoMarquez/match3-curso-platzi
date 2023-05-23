namespace Match3.Grid
{
    using UnityEngine;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    
    public class PiecesSwaper : MonoBehaviour 
    {
        private Tile _startTile, _endTile;
        private int _width, _height;
        public Piece[,] _pieces;

        public void Init(Piece[,] pieces, int width, int height)
        {
            _pieces = pieces;
            _width = width;
            _height = height;
        }

        public void OnTileDown(Tile tile) { _startTile = tile; }

        public void OnTileOver(Tile tile) { _endTile = tile; }

        public void OnTileUp(Tile tile) { SwapPieces(_startTile, _endTile); }

        private bool IsCloseTo(Tile startTile, Tile endTile)
        {
            if(Mathf.Abs(startTile._x - endTile._x) == 1 && startTile._y == endTile._y) return true;
            if(Mathf.Abs(startTile._y - endTile._y) == 1 && startTile._x == endTile._x) return true;

            return false;
        }

        public List<Piece> GetMatchesByDirection(int x, int y, Vector2 direction, int minPiecesToMatch = 3)
        {
            var match = new List<Piece>();
            Piece startPiece = _pieces[x,y];
            match.Add(startPiece);

            Vector2Int move = new Vector2Int(0,0);
            int maxValue = _width > _height ? _width : _height;

            for (int i = 1; i < maxValue; i++)
            {
                move.x = x + ((int)direction.x * i);
                move.y = y + ((int)direction.y * i);

                if(move.x >= 0 && move.x < _width && move.y >= 0 && move.y < _height)
                {
                    var nextPiece = _pieces[move.x, move.y];

                    if(nextPiece != null && nextPiece.GetType() == startPiece.GetType())
                    {
                        match.Add(nextPiece);
                        continue;
                    }

                    break;
                }
            }

            return match.Count >= minPiecesToMatch ? match :  null;
        }

        public List<Piece> GetMatchByPiece(int xpos, int ypos, int minPieces = 3)
        {
            var upMatchs = GetMatchesByDirection(xpos, ypos, Vector2.up, 2);
            var downMatchs = GetMatchesByDirection(xpos, ypos, Vector2.down, 2);
            var rightMatchs = GetMatchesByDirection(xpos, ypos, Vector2.right, 2);
            var leftMatchs = GetMatchesByDirection(xpos, ypos, Vector2.left, 2);

            if (upMatchs == null) upMatchs = new List<Piece>();
            if (downMatchs == null) downMatchs = new List<Piece>();
            if (rightMatchs == null) rightMatchs = new List<Piece>();
            if (leftMatchs == null) leftMatchs = new List<Piece>();

            var verticalMatches = upMatchs.Union(downMatchs).ToList();
            var horizontalMatches = leftMatchs.Union(rightMatchs).ToList();

            var foundMatches = new List<Piece>();

            if (verticalMatches.Count >= minPieces)
            {
                foundMatches = foundMatches.Union(verticalMatches).ToList();
            }
            if (horizontalMatches.Count >= minPieces)
            {
                foundMatches = foundMatches.Union(horizontalMatches).ToList();
            }

            return foundMatches;
        }

        private IEnumerator FindMatches(Tile startTile, Tile endTile, Piece startPiece, Piece endPiece)
        {
            yield return new WaitForSeconds(0.6f);

            bool foundMatch = false;
            var startMatches = GetMatchByPiece(_startTile._x, _startTile._y);
            var endMatches = GetMatchByPiece(_endTile._x, _endTile._y);

            startMatches.ForEach(piece =>
            {
                foundMatch = true;
                _pieces[piece._x, piece._y] = null;
                Destroy(piece.gameObject);
            });

            endMatches.ForEach(piece =>
            {
                foundMatch = true;
                _pieces[piece._x, piece._y] = null;
                Destroy(piece.gameObject);
            });

            if(!foundMatch)
            {
                startPiece?.Move(startTile._x, startTile._y);
                endPiece?.Move(endTile._x, endTile._y);

                _pieces[startTile._x, startTile._y] = startPiece;
                _pieces[endTile._x, endTile._y] = endPiece;
            }

            _startTile = null;
            _endTile = null;
            yield return null;
        }

        private void SwapPieces(Tile startTile, Tile endTile)
        {
            if(!IsCloseTo(startTile, endTile)) return;
            if(startTile == null || endTile == null) return;

            var startPiece = _pieces[startTile._x, startTile._y];
            var endPiece = _pieces[endTile._x, endTile._y];   

            startPiece?.Move(endTile._x, endTile._y);
            endPiece?.Move(startTile._x, startTile._y);

            _pieces[startTile._x, startTile._y] = endPiece;
            _pieces[endTile._x, endTile._y] = startPiece;

            StartCoroutine(FindMatches(startTile, endTile, startPiece, endPiece));
        }
    }
}