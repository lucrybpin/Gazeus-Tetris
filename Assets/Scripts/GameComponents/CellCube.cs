using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Gazeus.Utils;

namespace Gazeus.GameComponents {

    public class CellCube : MonoBehaviour {
        [SerializeField] public Vector2Int position;
        [SerializeField] public int value;
        [SerializeField] Ease easeType;

        public void OnEnable ()
        {
            Expand();
        }

        [ContextMenu( "Shrink" )]
        private void Shrink ()
        {
            transform
                .DOScale( Vector3.zero, 0.52f )
                .SetEase( easeType )
                .SetUpdate( true );
        }

        [ContextMenu( "Expand" )]
        private void Expand ()
        {
            transform
                .DOScale( Vector3.one, 0.52f )
                .SetEase( easeType )
                .SetUpdate( true );
        }

        public void SetMaterial (CellColor cellColor)
        {
            GetComponent<MeshRenderer>().material = Utils.Utils.GetMaterial( ( CellColor ) cellColor );
        }

        public void WaveEffect ()
        {
            transform
                //.DOLocalMoveX( transform.position.x + 1.4f, 1f )
                .DOScale(Vector3.zero, 0.25f)
                .SetEase( Ease.Linear )
                .SetUpdate( true )
                .OnComplete(
                    () => {

                        transform
                        //.DOLocalMoveX( transform.position.x - 1.4f, 1f )
                        .DOScale( Vector3.one, 0.25f )
                        .SetEase( Ease.Linear )
                        .SetUpdate( true );

                    }
                );
        }
    }

}