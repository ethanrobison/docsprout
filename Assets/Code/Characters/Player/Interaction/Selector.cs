﻿using System;
using Code.Characters.Doods;
using Code.Session;
using Code.Utils;
using UnityEngine;

namespace Code.Characters.Player.Interaction
{
    public class Selector : MonoBehaviour
    {
        private enum SelectionMode
        {
            Off,
            Idle,
            Selecting,
            Deselecting
        }

        private const float MIN_SPEED = 8f, MAX_SPEED = 14f, SPEED_CONSTANT = 10f;
        private const float MIN_DIST = 2f, MAX_DIST = 15f;
        private const float WHISTLE_RADIUS = 5f;

        public GameObject Cursor;

        private readonly float[] _sizes = { 0.5f, 1.5f, 2.5f, 4f, 5.5f };
        private readonly RaycastHit[] _hits = new RaycastHit[50];

        private MultiFocusCam _multiFocusCamera;
        private CameraController _cameraController;
        private Renderer _renderer;

        private Vector3 _pos = new Vector3(0f, 50f, 0f);
        private SelectionMode _mode;

        private int _sizeInd;

        private float Size {
            get { return _sizes[_sizeInd]; }
        }


        private void Start () {
            _multiFocusCamera = gameObject.GetRequiredComponent<MultiFocusCam>();
            _cameraController = gameObject.GetRequiredComponent<CameraController>();
            _renderer = Cursor.GetRequiredComponent<Renderer>();

            _multiFocusCamera.enabled = false;
            _cameraController.enabled = true;
            _renderer.enabled = false;

            _sizeInd = ((_sizes.Length + 1) >> 1) - 1;
            Cursor.transform.localScale = new Vector3(Size * 2f, Cursor.transform.localScale.y, Size * 2f);

            // todo a more graceful way to check if the game has started
            if (Game.Ctx != null) { RegisterMappings(); }
        }

        private void RegisterMappings () {
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.BButton, OnBPress);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.YButton, PikminWhistle);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.LeftBumper, () => { ChangeSize(-1); });
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.RightBumper, () => { ChangeSize(1); });
        }


        private void Update () {
            CalculateMode();
            if (_mode == SelectionMode.Off || _mode == SelectionMode.Idle) { return; }

            CastAtPosition(_pos + transform.position + Vector3.up * 50f, Size, _mode);
        }

        private void CalculateMode () {
            PressType rt = Game.Sesh.Input.Monitor.Rt, lt = Game.Sesh.Input.Monitor.Lt;
            switch (_mode) {
                case SelectionMode.Off:
                    if (rt == PressType.ButtonUp || lt == PressType.ButtonUp) { TransitionToMode(SelectionMode.Idle); }

                    break;
                case SelectionMode.Idle:
                    if (rt == PressType.Hold) { _mode = SelectionMode.Selecting; }
                    else if (lt == PressType.Hold) { _mode = SelectionMode.Deselecting; }

                    break;
                case SelectionMode.Selecting:
                    if (rt == PressType.ButtonUp) { _mode = SelectionMode.Idle; }

                    break;
                case SelectionMode.Deselecting:
                    if (lt == PressType.ButtonUp) { _mode = SelectionMode.Idle; }

                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }


        private void FixedUpdate () {
            if (_mode == SelectionMode.Off) { return; }

            MoveCamera();
            ClampPosition();
        }

        private void MoveCamera () {
            var speed = _pos.magnitude;
            speed = MIN_SPEED + (MAX_SPEED - MIN_SPEED) * speed / (speed + SPEED_CONSTANT);

            var camright = Camera.main.transform.right;
            var camforward = Vector3.Cross(Camera.main.transform.up, camright);
            var move = camright * Game.Sesh.Input.Monitor.RightH - camforward * Game.Sesh.Input.Monitor.RightV;

            MoveCursorPosition(move * speed * Time.deltaTime);
        }

        private void ClampPosition () {
            _pos = _pos.sqrMagnitude < MIN_DIST * MIN_DIST
                ? _pos.normalized * MIN_DIST
                : Vector3.ClampMagnitude(_pos, MAX_DIST);
        }

        private void PikminWhistle () {
            CastAtPosition(transform.position + Vector3.up * 50f, WHISTLE_RADIUS, SelectionMode.Selecting);
        }

        private void OnBPress () {
            if (_mode == SelectionMode.Off) { Game.Ctx.Doods.DeselectAll(); }
            else { TransitionToMode(SelectionMode.Off); }
        }

        private void TransitionToMode (SelectionMode mode) {
            switch (mode) {
                case SelectionMode.Off:
                    _renderer.enabled = false;
                    _cameraController.enabled = true;
                    _multiFocusCamera.enabled = false;
                    _multiFocusCamera.RelinquishControl(_cameraController);
                    break;
                case SelectionMode.Selecting:
                case SelectionMode.Deselecting:
                case SelectionMode.Idle:
                    _renderer.enabled = true;
                    var camForward = Vector3.Cross(Camera.main.transform.up, Camera.main.transform.right);
                    SetCursorPosition(-camForward * MIN_DIST);
                    _cameraController.enabled = false;
                    _multiFocusCamera.enabled = true;
                    break;
                default: throw new ArgumentOutOfRangeException("mode", mode, null);
            }

            _mode = mode;
        }

        private void CastAtPosition (Vector3 position, float size, SelectionMode mode) {
            var n = Physics.SphereCastNonAlloc(position, size, Vector3.down, _hits);
            for (var i = 0; i < n; ++i) {
                var dood = _hits[i].transform.GetComponent<Dood>();
                if (dood != null) { HandleDood(dood, mode); }
            }
        }

        private static void HandleDood (ISelectable dood, SelectionMode mode) {
            if (mode == SelectionMode.Selecting) { dood.OnSelect(); }
            else if (mode == SelectionMode.Deselecting) { dood.OnDeselect(); }
        }

        private void SetCursorPosition (Vector3 pos) {
            _pos = pos;
            Cursor.transform.position = _pos + transform.position;
        }

        private void MoveCursorPosition (Vector3 pos) {
            _pos += pos;
            _pos.y = 0f;
            Cursor.transform.position = _pos + transform.position;
        }

        private void ChangeSize (int delta) {
            _sizeInd = Mathf.Clamp(_sizeInd + delta, 0, _sizes.Length - 1);
            Cursor.transform.localScale = new Vector3(Size * 2f, Cursor.transform.localScale.y, Size * 2f);
        }
    }
}