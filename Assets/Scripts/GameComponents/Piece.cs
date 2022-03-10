using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.GameComponents {

    [System.Serializable]
    public class Piece {

        [SerializeField] Tetromino tetromino;
        [SerializeField] int currentRotation;
        [SerializeField] Vector2Int offset;
        [SerializeField] private List<Vector2Int> cells;

        public Piece (Tetromino tetromino, Vector2Int offset)
        {
            if (tetromino == null)
                Debug.LogError( "Trying to create a piece with no Tetromino (Tetromino parameter is null)" );

            this.tetromino = tetromino;
            this.cells = new List<Vector2Int>();
            this.cells = Utils.Utils.CopyCells( tetromino.CellsRotation0 );
            Utils.Utils.AddOffset( this.cells, offset );
            this.currentRotation = 0;
            this.offset = offset;
        }

        public void IncreateCurrentRotation ()
        {
            currentRotation = ( currentRotation + 1 ) % 4;
        }

        public List<Vector2Int> GetNextRotationCells (bool clockwiwe = false)
        {
            List<Vector2Int> cellsToCopy = null;
            if (clockwiwe == true)
            {
                if (currentRotation == 0)
                    cellsToCopy = Utils.Utils.CopyCells(Tetromino.CellsRotation1);
                if (currentRotation == 1)
                    cellsToCopy = Utils.Utils.CopyCells(Tetromino.CellsRotation2);
                if (currentRotation == 2)
                    cellsToCopy = Utils.Utils.CopyCells(Tetromino.CellsRotation3);
                if (currentRotation == 3)
                    cellsToCopy = Utils.Utils.CopyCells(Tetromino.CellsRotation0);
            } else
            {
                if (currentRotation == 0)
                    cellsToCopy = Utils.Utils.CopyCells(Tetromino.CellsRotation3);
                if (currentRotation == 1)
                    cellsToCopy = Utils.Utils.CopyCells(Tetromino.CellsRotation2);
                if (currentRotation == 2)
                    cellsToCopy = Utils.Utils.CopyCells(Tetromino.CellsRotation1);
                if (currentRotation == 3)
                    cellsToCopy = Utils.Utils.CopyCells(Tetromino.CellsRotation0);
            }

            return cellsToCopy;
        }

        public List<Vector2Int> Cells { get => cells; set => cells = value; }
        public Vector2Int Offset { get => offset; set => offset =  value ; }
        public Tetromino Tetromino { get => tetromino; }
    }

}