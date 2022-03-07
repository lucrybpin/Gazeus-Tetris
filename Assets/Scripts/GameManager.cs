using Gazeus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gazeus
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        [SerializeField] private Playfield playField;
        [SerializeField] private BlackFader blackFader;

        public static GameManager Instance { get; private set; }
        public Playfield PlayField { get => playField; }

        private void Awake()
        {
            SetupSingleton();
        }

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            playField = GameObject.FindObjectOfType<Playfield>();
            blackFader = GameObject.FindObjectOfType<BlackFader>();
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

        public void StartNormalGame()
        {
            AudioManager.Instance.EndCurrentSound();
            playField.gameObject.SetActive(true);
            blackFader.FadeInOut(1);
            UIManager.Instance.EnableAfterTime(UIManager.Instance.ButtonNewGame.gameObject);
            UIManager.Instance.DisableAfterTime(UIManager.Instance.PanelMainMenu.gameObject);
            AudioManager.Instance.PlaySongAfterTime(1);
            playField.SetClassicMode();
            playField.StartGameAfterTime(2);
        }

        public void StartCrazyGame()
        {
            AudioManager.Instance.EndCurrentSound();
            playField.gameObject.SetActive(true);
            blackFader.FadeInOut(1);
            UIManager.Instance.EnableAfterTime(UIManager.Instance.ButtonNewGame.gameObject);
            UIManager.Instance.DisableAfterTime(UIManager.Instance.PanelMainMenu.gameObject);
            AudioManager.Instance.PlaySongAfterTime(1);
            playField.SetCrazyMode();
            playField.StartGameAfterTime(2);
        }

        public void ShowControls()
        {
            UIManager.Instance.ShowControls();
        }

        public void OpenMainMenu()
        {
            AudioManager.Instance.EndCurrentSound();
            blackFader.FadeInOut(1);
            UIManager.Instance.EnableAfterTime(UIManager.Instance.PanelMainMenu.gameObject);
            AudioManager.Instance.PlaySongAfterTime(0);
            playField.SetTimeScale(1);
            UIManager.Instance.HideScore();
            playField.ClearGrid3D();
            UIManager.Instance.HidePauseResume();
            playField.StopGameLoopTick();
        }
    }
}
