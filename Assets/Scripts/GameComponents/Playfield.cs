using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Gazeus.GameComponents {

    [System.Serializable]
    public class UnityEventScore : UnityEvent<int> {
    }

    public class Playfield : MonoBehaviour {

        [SerializeField] int score;

        [SerializeField] Tetrominos tetrominosClassic;
        [SerializeField] Tetrominos tetrominosCrazyMode;
        [SerializeField] Tetrominos tetrominos;
        [SerializeField] TMP_Text dos_text;
        [SerializeField] InputHandler inputHandler;

        public UnityEvent OnGameStart;
        public UnityEvent OnGameOver;

        public UnityEvent OnPause;
        public UnityEvent OnResume;

        public UnityEventScore OnScoreChanged;

        [SerializeField] Grid grid = new Grid( 10, 24 );
        [SerializeField] Piece currentPiece;

        IEnumerator gameLoopTickCo;

        [Header( "Game Status" )]
        [SerializeField] bool isPaused = false;

        public Grid Grid { get => grid; }
        public Piece CurrentPiece { get => currentPiece; }
        public InputHandler InputHandler { get => inputHandler; }
        public int Score { get => score; }

        private void OnEnable ()
        {
            Setup();

            inputHandler.ActionMoveLeft.performed += (ctx) => { MoveLeft(); };
            inputHandler.ActionMoveRight.performed += (ctx) => { MoveRight(); };
            inputHandler.ActionMoveDown.performed += (ctx) => { MoveDown(); };
            inputHandler.ActionRotateLeft.performed += (ctx) => { RotateLeft(); };
            inputHandler.ActionRotateRight.performed += (ctx) => { RotateRight(); };
            inputHandler.ActionPauseResume.performed += (ctx) => { HandlePauseResume(); };
            inputHandler.ActionHardDrop.performed += (ctx) => { HardDrop(); };
        }

        private void Setup ()
        {
            grid.Setup( this );
            inputHandler.Setup( this );
            OnGameOver.AddListener( GameOver );
            grid.OnFinishInstantiating.AddListener( StartNewGame );
        }

        public void StartGameAfterTime(float time)
        {
            StartCoroutine( StartGameAfterTimeCo( time ) );
        }

        public IEnumerator StartGameAfterTimeCo (float time)
        {
            yield return new WaitForSecondsRealtime( time );
            PrepareGame();
        }

        public void RestartGame ()
        {
            PrepareGame();
        }

        [ContextMenu( "OnPrepareGame" )]
        public void PrepareGame ()
        {
            isPaused = true;
            grid.StartGrid();
        }

        public void SetClassicMode()
        {
            tetrominos = tetrominosClassic;
        }

        public void SetCrazyMode ()
        {
            tetrominos = tetrominosCrazyMode;
        }

        private void SpawnRandomPiece ()
        {
            Piece newPiece = new Piece( tetrominos.GetRandomTetromino(), grid.SpawnPoint );
            if (grid.SpawnPiece( newPiece ))
                currentPiece = newPiece;
            else
                OnGameOver.Invoke();
        }

        private void MoveLeft ()
        {
            if (!inputHandler.MovementsEnabled) return;
            if (grid.MovePiece( currentPiece, Vector2Int.left ))
                grid.Draw(dos_text);
        }

        private void MoveRight ()
        {
            if (!inputHandler.MovementsEnabled) return;
            if (grid.MovePiece( currentPiece, Vector2Int.right ))
                grid.Draw(dos_text);
        }

        private void MoveDown ()
        {
            if (!inputHandler.MovementsEnabled) return;
            if (grid.MovePiece( currentPiece, Vector2Int.down ))
                grid.Draw(dos_text);
        }

        private void RotateLeft ()
        {
            if (!inputHandler.MovementsEnabled) return;
            if (grid.RotatePiece( currentPiece ))
                grid.Draw(dos_text);
        }

        private void RotateRight()
        {
            if (!inputHandler.MovementsEnabled) return;
            if (grid.RotatePiece( currentPiece, true ))
                grid.Draw(dos_text);
        }

        private void HandlePauseResume ()
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }

        private void HardDrop ()
        {
            if (!inputHandler.MovementsEnabled) return;
            do { } while (grid.MovePiece( currentPiece, Vector2Int.down ));
            grid.Draw(dos_text);
        }

        private void PauseGame ()
        {
            isPaused = true;
            SetTimeScale( 0 );
            inputHandler.MovementsEnabled = false;
            OnPause.Invoke();
        }

        public void ResumeGame ()
        {
            isPaused = false;
            SetTimeScale( 1 );
            inputHandler.MovementsEnabled = true;
            OnResume.Invoke();
        }

        private IEnumerator GameLoopTick ()
        {
            while (true)
            {
                while (isPaused)
                    yield return null;

                yield return new WaitForSeconds( 0.25f );

                if (grid.MovePiece( currentPiece, Vector2Int.down ))
                {
                    grid.Draw(dos_text);
                }
                else
                {
                    ResolveCompletedRows();
                    SpawnRandomPiece();
                }
                yield return null;
            }
        }

        private void ResolveCompletedRows ()
        {
            List<int> clearRows = new List<int>();

            clearRows = grid.ResolveCompletedRows();
            for (int n = 0; n < clearRows.Count; n++)
            {
                grid.ClearRow( clearRows [ n ] );
                grid.ShiftDownCells( clearRows [ n ] + 1 );
                score += 100;
                OnScoreChanged.Invoke( score );
            }
        }

        public void StartNewGame ()
        {
            score = 0;
            OnScoreChanged.Invoke( 0 );
            OnGameStart.Invoke();
            grid.Clear();
            SpawnRandomPiece();

            gameLoopTickCo = GameLoopTick();
            StartCoroutine( gameLoopTickCo );
            ResumeGame();
        }

        public void StopGameLoopTick()
        {
            StopCoroutine(gameLoopTickCo);
        }

        private void GameOver ()
        {
            WaveEffect();
            StopCoroutine( gameLoopTickCo );
            currentPiece = null;
            PauseGame();
        }

        public void EndGame ()
        {
            StopCoroutine( gameLoopTickCo );
            currentPiece = null;
            isPaused = true;
            ClearGrid3D();
        }

        public void ClearGrid3D ()
        {
            grid.DeleteGrid();
        }

        public void SetTimeScale (float newTimeScale)
        {
            Time.timeScale = newTimeScale;
        }

        public void WaveEffect ()
        {
            grid.AddEffect();
        }


    }

}