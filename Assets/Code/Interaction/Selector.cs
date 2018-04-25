using System;
using Code.Characters.Doods;
using Code.Characters.Player;
using Code.Session;
using Code.Utils;
using UnityEngine;

namespace Code.Interaction
{
    public class Selector : MonoBehaviour
    {
        private enum SelectionMode
        {
            Off,
            Selecting,
            Deselecting
        }

        private const float MAX_SPEED = 8f;
        private const float MIN_SPEED = 3f;
        private const float MIN_DIST = 2f;
        private const float MAX_DIST = 15f;
        private const float SPEED_CONSTANT = 10f;
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
            if (Game.Ctx != null) {
                RegisterMappings();
            }
        }

        private void Update () {
            if (_mode == SelectionMode.Off) {
                if (Game.Sesh.Input.Monitor.Rt == PressType.ButtonUp) {
                    TransitionToMode(SelectionMode.Selecting);
                }
                else if (Game.Sesh.Input.Monitor.Lt == PressType.ButtonUp) {
                    TransitionToMode(SelectionMode.Deselecting);
                }

                return;
            }

            var selecting = _mode == SelectionMode.Selecting && Game.Sesh.Input.Monitor.Rt == PressType.Hold;
            var deselecting = _mode == SelectionMode.Deselecting && Game.Sesh.Input.Monitor.Lt == PressType.Hold;

            if (!selecting && !deselecting) { return; }

            CastAtPosition(_pos + transform.position + Vector3.up * 50f, Size, _mode);
        }

        private void FixedUpdate () {
            if (_mode == SelectionMode.Off) { return; }

            float curSpeed = _pos.magnitude;
            curSpeed = MIN_SPEED + (MAX_SPEED - MIN_SPEED) * curSpeed / (curSpeed + SPEED_CONSTANT);
            Vector3 camRight = Camera.main.transform.right;
            Vector3 camForward = Vector3.Cross(Camera.main.transform.up, camRight);
            Vector3 move = camRight * Game.Sesh.Input.Monitor.RightH - camForward * Game.Sesh.Input.Monitor.RightV;
            AddPos(move * curSpeed * Time.deltaTime);
            _pos.y = 0f;
            if (_pos.sqrMagnitude < MIN_DIST * MIN_DIST) {
                _pos.Normalize();
                _pos *= MIN_DIST;
            }
            else {
                _pos = Vector3.ClampMagnitude(_pos, MAX_DIST);
            }
        }


        private void RegisterMappings () {
            // B Button to DeselectAll
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.BButton, OnBPress);

            // Y Button to PikminWhistle
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.YButton, PikminWhistle);

            // bumbers to change size
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.LeftBumper, () => { ChangeSize(-1); });
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.RightBumper, () => { ChangeSize(1); });
        }

        private void PikminWhistle () {
            CastAtPosition(transform.position + Vector3.up * 50f, WHISTLE_RADIUS, SelectionMode.Selecting);
        }

        private void OnBPress () {
            if (_mode == SelectionMode.Off) {
                Game.Ctx.Doods.DeselectAll();
            }
            else {
                TransitionToMode(SelectionMode.Off);
            }
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
                    _renderer.enabled = true;
                    Vector3 camForward = Vector3.Cross(Camera.main.transform.up, Camera.main.transform.right);
                    SetPos(-camForward * MIN_DIST);
                    _cameraController.enabled = false;
                    _multiFocusCamera.enabled = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode", mode, null);
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

        private void SetPos (Vector3 pos) {
            _pos = pos;
            Cursor.transform.position = _pos + transform.position;
        }

        private void AddPos (Vector3 pos) {
            _pos += pos;
            Cursor.transform.position = _pos + transform.position;
        }

        public void SetPos (Vector2 pos) {
            _pos.x = pos.x;
            _pos.z = pos.y;
            Cursor.transform.position = _pos + transform.position;
        }

        public void AddPos (Vector2 pos) {
            _pos.x += pos.x;
            _pos.z += pos.y;
            Cursor.transform.position = _pos + transform.position;
        }

        private void ChangeSize (int delta) {
            _sizeInd = Mathf.Clamp(_sizeInd + delta, 0, _sizes.Length - 1);
            Cursor.transform.localScale = new Vector3(Size * 2f, Cursor.transform.localScale.y, Size * 2f);
        }
    }
}