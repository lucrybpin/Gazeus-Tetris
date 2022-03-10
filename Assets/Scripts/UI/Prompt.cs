using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Text.RegularExpressions;
using Gazeus.Managers;

namespace Gazeus.UI
{
    public class Prompt : MonoBehaviour, IDragHandler, IPointerDownHandler
    {

        [SerializeField] private RectTransform promptRectTransform;
        [SerializeField] private Canvas promptCanvas;
        [SerializeField] private TMP_Text input;
        [SerializeField] private TMP_InputField input_field;
        [SerializeField] private TMP_Text score;

        [SerializeField] InputAction actionProcessCommand;
        public InputAction ActionProcessCommand { get => actionProcessCommand; }

        private void OnEnable()
        {
            actionProcessCommand.Enable();
            actionProcessCommand.performed += (ctx) => { ProcessCommand(); };
        }

        private void Start()
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
            promptRectTransform.anchoredPosition += eventData.delta / promptCanvas.scaleFactor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            promptRectTransform.SetAsLastSibling();
            input_field.Select();
        }
        
        public void ClearScore()
        {
            score.text = "";
        }

        public void UpdateScore()
        {
            score.text = "score\n"+ GameManager.Instance.PlayField.Score;
        }

        private void ProcessCommand()
        {
            if (Regex.IsMatch(input.text, "tetris -new"))
            {
                GameManager.Instance.StartNormalGame();
            }

            if (Regex.IsMatch(input.text, "tetris -crazy"))
            {
                GameManager.Instance.StartCrazyGame();
            }

            if (Regex.IsMatch(input.text, "tetris -menu"))
            {
                ClearScore();
                GameManager.Instance.OpenMainMenu();
            }

            if (Regex.IsMatch(input.text, "tetris -exit"))
            {
                Application.Quit();
            }
        }
    }
}
