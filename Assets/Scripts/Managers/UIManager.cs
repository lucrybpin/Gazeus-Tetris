using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

namespace Gazeus {

    public class UIManager : MonoBehaviour {

        private static UIManager instance;

        [SerializeField] CanvasGroup blackFader;
        [SerializeField] TMP_Text scoreText;
        [SerializeField] GameObject scorePanel;

        [SerializeField] GameObject panelMainMenu;
        [SerializeField] GameObject panelPauseResume;
        [SerializeField] GameObject panelControls;
        [SerializeField] Button buttonNewGame;

        public static UIManager Instance { get; private set; }
        public Button ButtonNewGame { get => buttonNewGame; }
        public GameObject PanelMainMenu { get => panelMainMenu; }

        private void Awake()
        {
            SetupSingleton();
        }

        private void Start ()
        {
            Setup();
        }

        private void Setup ()
        {
            Playfield playField = FindObjectOfType<Playfield>();
            playField.OnScoreChanged.AddListener( SetScore );
        }

        public void EnableAfterTime (GameObject gameObject)
        {
            StartCoroutine( EnableAfterTimeCo( gameObject ) );
        }

        private IEnumerator EnableAfterTimeCo (GameObject gameObject)
        {
            yield return new WaitForSecondsRealtime( 1f );
            gameObject.SetActive( true );
        }

        public void DisableAfterTime (GameObject gameObject)
        {
            StartCoroutine( DisableAfterTimeCo( gameObject ) );
        }

        private IEnumerator DisableAfterTimeCo (GameObject gameObject)
        {
            yield return new WaitForSecondsRealtime( 1f );
            gameObject.SetActive( false );
        }


        public void ShowScore ()
        {
            scorePanel.SetActive( true );
        }

        public void HideScore ()
        {
            scorePanel.SetActive( false );
        }

        public void ShowControls()
        {
            panelControls.SetActive(true);
        }

        public void HidePauseResume()
        {
            panelPauseResume.SetActive(false);
        }

        public void SetScore (int newScore)
        {
            string prefix;
            string posfix = "</color>";

            if (newScore >= 0 && newScore < 200)
                prefix = "<color=white>";
            else if (newScore >= 200 && newScore < 300)
                prefix = "<color=#00AAFF>";
            else if (newScore >= 300 && newScore < 400)
                prefix = "<color=#8C78AA>";
            else if (newScore >= 400 && newScore < 500)
                prefix = "<color=#348C46>";
            else if (newScore >= 500 && newScore < 600)
                prefix = "<color=#FAFA78>";
            else if (newScore >= 600 && newScore < 700)
                prefix = "<color=#FF8C34>";
            else
                prefix = "<color=#FA7878>";

            scoreText.text = prefix + newScore.ToString() + posfix;
        }

        private void SetupSingleton()
        {
            if (Instance != null && Instance != this)
                Destroy(this.gameObject);
            else
                Instance = this;

            if (transform.parent != null)
                DontDestroyOnLoad(transform.parent);
            else
                DontDestroyOnLoad(this);
        }

    }

}