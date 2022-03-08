using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Gazeus {

    [System.Serializable]
    public class Grid {

        [SerializeField] private int [ , ] grid;
        [SerializeField] private int sizeX;
        [SerializeField] private int sizeY;
        [SerializeField] private Vector2Int spawnPoint;
        [SerializeField] public UnityEvent OnFinishInstantiating;

        Playfield playField;
        Transform gridParent;
        [SerializeField] List<CellCube> cellCubes = new List<CellCube>();
        GameObject cellPrefab;

        public int SizeX { get => sizeX; }
        public int SizeY { get => sizeY; }
        public Vector2Int SpawnPoint { get => spawnPoint; }

        public void Setup (Playfield playfield)
        {
            playField = playfield;
            this.cellPrefab = Utils.GetPrefab( "Cell Cube" );
            this.gridParent = playfield.transform;
            OnFinishInstantiating.AddListener(playfield.InputHandler.Enable);
        }

        public Grid (int sizeX, int sizeY)
        {
            grid = new int [ sizeX, sizeY ];
            Clear();
        }

        public bool SpawnPiece (Piece piece)
        {
            foreach (Vector2Int cell in piece.Cells)
            {
                if (GetCellValue( cell.x, cell.y ) != -1)
                    return false;
                else
                {
                    SetCellValue( cell.x, cell.y, ( int ) piece.Tetromino.Color );
                    Draw();
                }
            }
            return true;
        }

        public void StartGrid ()
        {
            DeleteGrid();
            playField.StartCoroutine( StartGridCo() );
        }

        private IEnumerator StartGridCo ()
        {
            if (cellCubes.Count == 0)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    for (int i = 0; i < SizeX; i++)
                    {
                        CellCube cellCube
                        = GameObject.Instantiate(
                            cellPrefab,
                            new Vector3( i, j, 0 ),
                            Quaternion.identity,
                            gridParent
                        ).GetComponent<CellCube>();

                        int cellValue = GetCellValue( i, j );

                        cellCube.GetComponent<MeshRenderer>().material = Utils.GetMaterial( CellColor.CellWhite );
                        cellCube.value = cellValue;
                        cellCube.position = new Vector2Int( i, j );
                        cellCubes.Add( cellCube );

                        yield return new WaitForSecondsRealtime( 0.0125f );

                    }
                }
            }
            yield return new WaitForSecondsRealtime( 0.43f );
            OnFinishInstantiating.Invoke();
        }

        public void DeleteGrid()
        {
            foreach (CellCube cellCube in cellCubes)
                GameObject.Destroy( cellCube.gameObject );
            cellCubes.Clear();
        }

        public void Draw (TMP_Text dos_text = null)
        {
            string charactere   = "";
            string linePrint    = "";
            //for (int j = 0; j < SizeY; j++)
            //{
            //    linePrint = linePrint + j.ToString("D2");
            //    for (int i = 0; i < SizeX; i++)
            //    {
            //        int cellValue = GetCellValue( i, j );
            //        CellCube cellCubeFound = null;

            //        for (int n = 0; n < cellCubes.Count; n++)
            //            if (cellCubes[n].position == new Vector2Int(i, j))
            //                cellCubeFound = cellCubes[n];

            //        if (cellCubeFound != null)
            //        {
            //            cellCubeFound.SetMaterial( ( CellColor ) cellValue );
            //            cellCubeFound.value = cellValue;
            //            charactere = (cellValue == -1) ? " " : "X"; 
            //        }
            //        linePrint = linePrint + charactere;
            //    }
            //    linePrint = linePrint + '|' + '\n';
            //}

            for (int j = sizeY-1; j >= 0; j--)
            {
                linePrint = linePrint + j.ToString("D2") + "|";
                for (int i = 0; i < SizeX; i++)
                {
                    int cellValue = GetCellValue(i, j);
                    CellCube cellCubeFound = null;

                    for (int n = 0; n < cellCubes.Count; n++)
                        if (cellCubes[n].position == new Vector2Int(i, j))
                            cellCubeFound = cellCubes[n];

                    if (cellCubeFound != null)
                    {
                        cellCubeFound.SetMaterial((CellColor)cellValue);
                        cellCubeFound.value = cellValue;
                        charactere = (cellValue == -1) ? " " : "<color=" + GetColorConverted((CellColor)cellValue) + ">X</color>";
                    }
                    linePrint = linePrint + charactere;
                }
                linePrint = linePrint + '|' + '\n';
            }

            Debug.Log(linePrint);
            if (dos_text != null)
            {
                dos_text.text = linePrint;
            }
            
        }

        public string GetColorConverted(CellColor color)
        {
            switch (color)
            {
                case CellColor.CellWhite:
                    return "\"white\"";
                case CellColor.CellBlue:
                    return "\"blue\"";
                case CellColor.CellCyan:
                    return "#00EAFF";
                case CellColor.CellGreen:
                    return "\"green\"";
                case CellColor.CellOrange:
                    return "\"orange\"";
                case CellColor.CellPurple:
                    return "\"purple\"";
                case CellColor.CellRed:
                    return "\"red\"";
                case CellColor.CellYellow:
                    return "\"yellow\"";
                default:
                    return "\"white\"";
            }
        }

        public int GetCellValue (int x, int y)
        {
            if (!IsCellInsideTheGrid( x, y ))
                Debug.LogError( "Trying to get a cell value out of the grid range" );

            return grid [ x, y ];
        }

        public int GetCellValue (Vector2Int cellCoordinates)
        {
            if (!IsCellInsideTheGrid( cellCoordinates.x, cellCoordinates.y ))
                Debug.LogError( "Trying to get a cell value out of the grid range" );

            return grid [ cellCoordinates.x, cellCoordinates.y ];
        }

        public void SetCellValue (int x, int y, int value)
        {
            if (!IsCellInsideTheGrid( x, y ))
                Debug.LogError( "Trying to set a cell value out of the grid range" );

            grid [ x, y ] = value;
        }

        public void SetCellValue (Vector2Int cellCoordinates, int value)
        {
            if (!IsCellInsideTheGrid( cellCoordinates.x, cellCoordinates.y ))
                Debug.LogError( "Trying to set a cell value out of the grid range" );

            grid [ cellCoordinates.x, cellCoordinates.y ] = value;
        }

        public bool IsCellInsideTheGrid (int x, int y)
        {
            return ( ( x < 0 || x >= sizeX ) || ( y < 0 || y >= sizeY ) ) ? false : true;
        }

        public bool IsCellInsideTheGrid (Vector2Int cellCoordinates)
        {
            return ( ( cellCoordinates.x < 0 || cellCoordinates.x >= sizeX ) || ( cellCoordinates.y < 0 || cellCoordinates.y >= sizeY ) ) ? false : true;
        }

        public bool IsCellFree (int x, int y)
        {
            return ( grid [ x, y ] == -1 ) ? true : false;
        }

        public bool IsCellFree (Vector2Int cellCoordinates)
        {
            return ( grid [ cellCoordinates.x, cellCoordinates.y ] == -1 ) ? true : false;
        }

        public bool IsValidCellsIgnoringPiece (List<Vector2Int> cells, Piece piece)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                if (!IsCellInsideTheGrid( cells [ i ] ) ||
                    ( !IsCellFree( cells [ i ] ) && !piece.Cells.Contains( cells [ i ] ) )) //Consider only cells that are not from this piece
                {
                    //Debug.Log( "Cell :" + piece.Cells [ i ] + " can't move to " + cells [ i ] );
                    return false;
                }

            }
            return true;
        }

        public static Vector3 GetWorldPosition (int x, int y)
        {
            return new Vector3( x + 0.5f, y + 0.5f, 0 );
        }

        public void ClearRow (int row)
        {
            for (int column = 0; column < sizeX; column++)
                SetCellValue( column, row, -1 );
        }

        internal void ShiftDownCells (int startingRow)
        {
            for (int j = startingRow; j < sizeY; j++)
            {
                for (int i = 0; i < sizeX; i++)
                {
                    int cellValue = GetCellValue( i, j );
                    SetCellValue( i, j - 1, cellValue );
                    SetCellValue( i, j, -1 );
                }
            }
        }

        internal void Clear ()
        {
            for (int j = 0; j < sizeY; j++)
                for (int i = 0; i < sizeX; i++)
                    grid [ i, j ] = -1;

            Draw();
        }

        public bool RotatePiece (Piece pieceToRotate, bool clockwise = false)
        {
            List<Vector2Int> nextCellsPosition = pieceToRotate.GetNextRotationCells(clockwise);
            Utils.AddOffset( nextCellsPosition, pieceToRotate.Offset );

            if (IsValidCellsIgnoringPiece( nextCellsPosition, pieceToRotate ))
            {
                //Clear last cells
                for (int i = 0; i < pieceToRotate.Cells.Count; i++)
                    SetCellValue( pieceToRotate.Cells [ i ], -1 );

                pieceToRotate.Cells = nextCellsPosition;

                //Paint new cells
                for (int i = 0; i < pieceToRotate.Cells.Count; i++)
                    SetCellValue( pieceToRotate.Cells [ i ], ( int ) pieceToRotate.Tetromino.Color );

                pieceToRotate.IncreateCurrentRotation();
            }

            return true;
        }

        public bool MovePiece (Piece pieceToMove, Vector2Int moveDirection)
        {
            Vector2Int nextCellPosition;

            //Try
            for (int i = 0; i < pieceToMove.Cells.Count; i++)
            {
                nextCellPosition = pieceToMove.Cells [ i ] + moveDirection;

                if (!IsCellInsideTheGrid( nextCellPosition ) ||
                    ( !IsCellFree( nextCellPosition ) && !pieceToMove.Cells.Contains( nextCellPosition ) )) //Consider only cells that are not from this piece
                {
                    //Debug.Log( "Can't Move cell:" + pieceToMove.Cells [ i ] + " to " + nextCellPosition );
                    return false;
                }

            }

            //Move
            for (int i = 0; i < pieceToMove.Cells.Count; i++)
            {
                SetCellValue( pieceToMove.Cells [ i ], -1 ); //Clear last cells
                pieceToMove.Cells [ i ] = pieceToMove.Cells [ i ] + moveDirection;
            }

            for (int i = 0; i < pieceToMove.Cells.Count; i++)
                SetCellValue( pieceToMove.Cells [ i ], ( int ) pieceToMove.Tetromino.Color ); //Paint new cells

            pieceToMove.Offset += moveDirection;

            return true;
        }

        public List<int> ResolveCompletedRows ()
        {
            List<int> clearRows = new List<int>();
            bool isClear = true;
            for (int j = 0; j < SizeY; j++)
            {
                isClear = true;
                for (int i = 0; i < SizeX; i++)
                {
                    if (GetCellValue( i, j ) == -1)
                    {
                        isClear = false;
                        continue;
                    }
                }

                if (isClear)
                    clearRows.Add( j );
            }

            clearRows?.Reverse();
            return clearRows;
        }

        public void AddEffect ()
        {
            playField.StartCoroutine( AddWaveEffect() );
        }

        public IEnumerator AddWaveEffect ()
        {
            for (int i = 0; i < cellCubes.Count; i++)
            {
                cellCubes [ i ].WaveEffect();
                yield return null;
            }

        }
    }

}