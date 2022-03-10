using Gazeus.GameComponents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gazeus.Utils {

    public class BackgroundPieces : MonoBehaviour {
        List<Rotate> rotatingPieces = new List<Rotate>();

        private void Start ()
        {
            Setup();
        }

        private void Setup ()
        {
            Rotate [ ] rotatingPiecesFound = FindObjectsOfType<Rotate>();
            foreach (Rotate rotatingPiece in rotatingPiecesFound)
                rotatingPieces.Add( rotatingPiece );

            Playfield playField = FindObjectOfType<Playfield>();
            playField.OnScoreChanged.AddListener( RotateOnScore );
        }

        public void RotateOnScore(int newScore)
        {
            RotateRandomPiece();
        }

        public void RotateAllPieces()
        {
            for (int i = 0; i < rotatingPieces.Count; i++)
                rotatingPieces [ i ].AccelerateAndDecelerate();
        }

        public void RotateRandomPiece()
        {
            rotatingPieces [ Random.Range( 0, rotatingPieces.Count ) ].AccelerateAndDecelerate();
        }
    }

}