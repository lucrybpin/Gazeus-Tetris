using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.Utils {

    public class Rotate : MonoBehaviour {

        [SerializeField] float rotationSpeed;
        private float initialRotationSpeed;

        
        private void Setup ()
        {
            initialRotationSpeed = rotationSpeed;
        }

        private void Start ()
        {
            Setup();
        }

        private void Update ()
        {
            transform.Rotate( 0, rotationSpeed * Time.deltaTime, 0 );
        }

        [ContextMenu( "AccelerateAndDecelerate" )]
        public void AccelerateAndDecelerate()
        {
            StartCoroutine( AccelerateAndDecelerateCo() );
        }

        public IEnumerator AccelerateAndDecelerateCo ()
        {
            yield return AccelerateCo();
            yield return DecelerateCo();
        }

        private IEnumerator AccelerateCo()
        {
            float totalTime = 1f;
            float elapsedTime = 0f;
            float finalRotationSpeed = 700f;
            while (elapsedTime < totalTime)
            {
                elapsedTime += Time.deltaTime;
                rotationSpeed = Mathf.Lerp( initialRotationSpeed, finalRotationSpeed, elapsedTime / totalTime );
                yield return null;
            }
            rotationSpeed = finalRotationSpeed;
        }

        private IEnumerator DecelerateCo ()
        {
            float totalTime = 3f;
            float elapsedTime = 0f;
            float currentStartingSpeed = rotationSpeed;
            while (elapsedTime < totalTime)
            {
                elapsedTime += Time.deltaTime;
                rotationSpeed = Mathf.Lerp( currentStartingSpeed, initialRotationSpeed, elapsedTime / totalTime );
                yield return null;
            }
            rotationSpeed = initialRotationSpeed;
        }
    }

}