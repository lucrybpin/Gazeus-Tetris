using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gazeus {
    [System.Serializable]
    public class InputHandler {
        [SerializeField] bool movementsEnabled;
        [SerializeField] InputAction actionMoveLeft;
        [SerializeField] InputAction actionMoveRight;
        [SerializeField] InputAction actionMoveDown;
        [SerializeField] InputAction actionRotate;
        [SerializeField] InputAction actionPauseResume;
        [SerializeField] InputAction actionHardDrop;

        Playfield playField;

        public bool MovementsEnabled { get => movementsEnabled; set => movementsEnabled = value; }
        public InputAction ActionMoveLeft { get => actionMoveLeft; }
        public InputAction ActionMoveRight { get => actionMoveRight; }
        public InputAction ActionMoveDown { get => actionMoveDown; }
        public InputAction ActionRotate { get => actionRotate; }
        public InputAction ActionPauseResume { get => actionPauseResume; }
        public InputAction ActionHardDrop { get => actionHardDrop; }
        

        public void Setup(Playfield playField)
        {
            this.playField = playField;
        }

        public void Enable()
        {
            movementsEnabled = true;
            actionMoveLeft.Enable();
            actionMoveRight.Enable();
            actionMoveDown.Enable();
            actionRotate.Enable();
            actionPauseResume.Enable();
            actionHardDrop.Enable();
        }

        public void Disable()
        {
            movementsEnabled = false;
            actionMoveLeft.Disable();
            actionMoveRight.Disable();
            actionMoveDown.Disable();
            actionRotate.Disable();
            actionPauseResume.Disable();
            actionHardDrop.Disable();
        }
    }

}