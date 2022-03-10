using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.Managers {

    public enum Songs {
        MainMenu,
        NormalMode,
        CrazyMode
    }

    [RequireComponent( typeof( AudioSource ) )]
    public class AudioManager : MonoBehaviour {

        [SerializeField] List<AudioClip> audioClips;
        Songs currentSong;
        private AudioSource audioSource;
        private IEnumerator soundFadeCo;

        public static AudioManager Instance { get; private set; } = null;

        private void SetupSingleton ()
        {
            if (Instance != null && Instance != this)
                Destroy( this.gameObject );
            else
                Instance = this;

            if (transform.parent != null)
                DontDestroyOnLoad( transform.parent );
            else
                DontDestroyOnLoad( this );
        }

        private void Setup ()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Awake ()
        {
            SetupSingleton();
        }

        private void Start ()
        {
            Setup();
            PlaySound( Songs.MainMenu );
            StartCoroutine( RepeatSongOverTime() );
        }

        public void PlaySongAfterTime(int song)
        {
            StartCoroutine( PlaySongAfterTimeCo( (int)song ) );
        }

        private IEnumerator PlaySongAfterTimeCo (int song)
        {
            yield return new WaitForSecondsRealtime( 1f );
            PlaySound( ( Songs ) song );
        }


        public void PlaySound (Songs song)
        {
            currentSong = song;
            audioSource.clip = audioClips [ ( int ) song ];
            audioSource.Play();
            soundFadeCo = FadeVolume( 10f, 0.1f );
            StartCoroutine( soundFadeCo );
        }

        public void EndCurrentSound()
        {
            StopCoroutine( soundFadeCo );
            soundFadeCo = FadeVolume( 1f, 0f );
            StartCoroutine( soundFadeCo );
        }

        private IEnumerator FadeVolume (float totalTime, float finalVolume = 1)
        {
            float elapsedTime = 0f;
            float startingVolume = audioSource.volume;
            while (elapsedTime < totalTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                audioSource.volume = Mathf.Lerp( startingVolume, finalVolume, elapsedTime / totalTime );
                yield return null;
            }
            audioSource.volume = finalVolume;
        }

        private IEnumerator RepeatSongOverTime()
        {
            while (true)
            {
                yield return new WaitForSeconds( 10f );
                if (!audioSource.isPlaying)
                {
                    PlaySound( currentSong );
                }
            }
        }


    }

}