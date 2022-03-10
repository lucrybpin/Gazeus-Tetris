using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Gazeus.GameComponents;

namespace Gazeus.Managers {

    public class UIManager : MonoBehaviour {

        private static UIManager instance;

        //[SerializeField] CanvasGroup blackFader;
        [SerializeField] TMP_Text scoreText;
        [SerializeField] GameObject scorePanel;

        [SerializeField] GameObject panelMainMenu;
        [SerializeField] GameObject panelPauseResume;
        [SerializeField] GameObject panelControls;
        [SerializeField] Button buttonNewGame;
        [SerializeField] TMP_Text dos_text;

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

        public void ClearDosText()
        {
            dos_text.text = "";
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