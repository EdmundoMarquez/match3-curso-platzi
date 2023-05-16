namespace Match3.Grid
{
    using UnityEngine;
    using Match3.Pieces;
    
    public class GridController : MonoBehaviour 
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;  
        [SerializeField] private GameObject _tileTemplate; 
        [SerializeField] private GameObject[] _availablePieces;
        [Space(10)] [Header("Camera Configurations")]
        [SerializeField] private float _cameraSizeOffset;
        [SerializeField] private float _cameraVerticalOffset;

        private void Start() 
        {
            GenerateBoard();
            CenterCamera();
            GeneratePieces();
        }

        private void GenerateBoard()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var tile = Instantiate(_tileTemplate, new Vector2(x,y), Quaternion.identity, transform);
                    tile.GetComponent<Tile>()?.Init(x,y,this);
                }
            }
        }

        private void CenterCamera()
        {
            float newPosX = (float)_width / 2f;
            float newPosY = (float)_height / 2f;

            float horizontal = _width + 1;
            float vertical = (_height/2) + 1;

            Camera mainCamera = Camera.main;
            mainCamera.transform.position = new Vector3(newPosX - 0.5f, newPosY - 0.5f + _cameraVerticalOffset, -10f);
            mainCamera.orthographicSize = ((horizontal > vertical) ? horizontal : vertical) + _cameraSizeOffset;
        }

        private void GeneratePieces()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var pieceToGenerate = _availablePieces[Random.Range(0, _availablePieces.Length)];
                    var tile = Instantiate(pieceToGenerate, new Vector2(x,y), Quaternion.identity, transform);
                    tile.GetComponent<Piece>()?.Init(x,y);
                }
            }
        }

        #if UNITY_EDITOR
        private void Update() 
        {
            CenterCamera();
        }
        #endif
    }
}