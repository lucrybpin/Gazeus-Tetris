using Gazeus.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus {

    [CreateAssetMenu( fileName = "Tetrominos", menuName = "Kokku/Tetris/Tetrominos", order = 1 )]
    public class Tetrominos : ScriptableObject {
        public List<Tetromino> tetrominos = new List<Tetromino>();

        public Tetromino GetRandomTetromino()
        {
            return tetrominos [ Random.Range( 0, tetrominos.Count ) ];
        }
    }

    [System.Serializable]
    public class Tetromino {

        [SerializeField] string name;
        [SerializeField] CellColor color;

        [SerializeField] List<Vector2Int> cellsRotation0;
        [SerializeField] List<Vector2Int> cellsRotation1;
        [SerializeField] List<Vector2Int> cellsRotation2;
        [SerializeField] List<Vector2Int> cellsRotation3;

        public string Name { get => name; }
        public CellColor Color { get => color; }
        public List<Vector2Int> CellsRotation0 { get => cellsRotation0; }
        public List<Vector2Int> CellsRotation1 { get => cellsRotation1; }
        public List<Vector2Int> CellsRotation2 { get => cellsRotation2; }
        public List<Vector2Int> CellsRotation3 { get => cellsRotation3; }
        
    }

}