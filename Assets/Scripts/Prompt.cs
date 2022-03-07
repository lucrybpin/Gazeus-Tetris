using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Text.RegularExpressions;

namespace gazeus
{
    public class Prompt : MonoBehaviour, IDragHandler, IPointerDownHandler
    {

        [SerializeField] private RectTransform promptRectTransform;
        [SerializeField] private Canvas promptCanvas;
        [SerializeField] private TMP_Text input;

        [SerializeField] InputAction actionProcessCommand;
        public InputAction ActionProcessCommand { get => actionProcessCommand; }

        private void OnEnable()
        {
            actionProcessCommand.Enable();
            actionProcessCommand.performed += (ctx) => { ProcessCommand(); };
        }

        public void OnDrag(PointerEventData eventData)
        {
            promptRectTransform.anchoredPosition += eventData.delta / promptCanvas.scaleFactor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            promptRectTransform.SetAsLastSibling();
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
                GameManager.Instance.OpenMainMenu();
            }

            if (Regex.IsMatch(input.text, "tetris -commands"))
            {
                GameManager.Instance.ShowControls();
            }

            if (Regex.IsMatch(input.text, "tetris -exit"))
            {
                Application.Quit();
            }
            //Debug.Log("Input: " + input.text);
        }
    }
}
