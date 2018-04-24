using System.Security.Cryptography.X509Certificates;
using Code.Characters.Doods;
using Code.Characters.Player;
using Code.Session;
using Code.Utils;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace Code.Interaction
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

        private const float MAX_SPEED = 8f;
        private const float MIN_SPEED = 3f;
        private const float SPEED_CONSTANT = 10f;

        public GameObject Cursor;

        public float MinDist = 2f;
        public float MaxDist = 15f;

        private MultiFocusCam _multiFocusCamera;
        private CameraController _cameraController;
        private SelectionMode _mode;

        [SerializeField] private float[] _sizes;
        private int _sizeInd;
        private RaycastHit[] _hits;
        private Vector3 _pos;

        private Renderer _renderer;

        private float Size {
            get { return _sizes[_sizeInd]; }
        }

        private void Start () {
            _multiFocusCamera = gameObject.GetRequiredComponent<MultiFocusCam>();
            _cameraController = gameObject.GetRequiredComponent<CameraController>();

            _pos = new Vector3(0f, 50f, 0f);
            _sizeInd = ((_sizes.Length + 1) >> 1) - 1;
            _hits = new RaycastHit[50];
            Cursor.transform.localScale = new Vector3(Size * 2f, Cursor.transform.localScale.y, Size * 2f);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.LeftBumper, DecreaseSize);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.RightBumper, IncreaseSize);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.BButton, () => {
                if (_mode != SelectionMode.Off) Game.Ctx.Doods.DeselectAll();
            });

            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.YButton, () => {
                if (_mode == SelectionMode.Off) {
                    _mode = SelectionMode.Idle;
                    _renderer.enabled = true;
                    Vector3 camForward = Vector3.Cross(Camera.main.transform.up, Camera.main.transform.right);
                    SetPos(-camForward * MinDist);
                    _cameraController.enabled = false;
                    _multiFocusCamera.enabled = true;
                }
                else {
                    _mode = SelectionMode.Off;
                    _renderer.enabled = false;
                    _cameraController.enabled = true;
                    _multiFocusCamera.enabled = false;
                    _multiFocusCamera.RelinquishControl(_cameraController);
                }
            });

            _multiFocusCamera.enabled = false;
            _cameraController.enabled = true;

            _renderer = Cursor.GetComponent<Renderer>();
            _renderer.enabled = false;
        }

        private void Update () {
            if (_mode == SelectionMode.Off) return;
            if (_mode == SelectionMode.Selecting) {
                if (Game.Sesh.Input.Monitor.RT < 0.1f) {
                    _mode = SelectionMode.Idle;
                }
            }
            else if (_mode == SelectionMode.Deselecting) {
                if (Game.Sesh.Input.Monitor.LT < 0.1f) {
                    _mode = SelectionMode.Idle;
                }
            }

            if (_mode == SelectionMode.Idle) {
                if (Game.Sesh.Input.Monitor.RT > 0.1f) {
                    _mode = SelectionMode.Selecting;
                }
                else if (Game.Sesh.Input.Monitor.LT > 0.1f) {
                    _mode = SelectionMode.Deselecting;
                }
                else {
                    return;
                }
            }

            int n = Physics.SphereCastNonAlloc(_pos + transform.position + Vector3.up * 50f, Size, Vector3.down,
                _hits);
            for (int i = 0; i < n; ++i) {
                Dood dood = _hits[i].transform.GetComponent<Dood>();
                if (!dood) {
                    continue;
                }

                if (_mode == SelectionMode.Selecting) {
                    dood.OnSelect();
                }
                else if (_mode == SelectionMode.Deselecting) {
                    dood.OnDeselect();
                }
            }
        }

        private void FixedUpdate () {
            if (_mode == SelectionMode.Off) {
                return;
            }

            float curSpeed = _pos.magnitude;
            curSpeed = MIN_SPEED + (MAX_SPEED - MIN_SPEED) * curSpeed / (curSpeed + SPEED_CONSTANT);
            Vector3 camRight = Camera.main.transform.right;
            Vector3 camForward = Vector3.Cross(Camera.main.transform.up, camRight);
            Vector3 move = camRight * Game.Sesh.Input.Monitor.RightH - camForward * Game.Sesh.Input.Monitor.RightV;
            AddPos(move * curSpeed * Time.deltaTime);
            _pos.y = 0f;
            if (_pos.sqrMagnitude < MinDist * MinDist) {
                _pos.Normalize();
                _pos *= MinDist;
            }
            else {
                _pos = Vector3.ClampMagnitude(_pos, MaxDist);
            }
        }

        public void SetPos (Vector3 pos) {
            _pos = pos;
            Cursor.transform.position = _pos + transform.position;
        }

        public void AddPos (Vector3 pos) {
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

        private void IncreaseSize () {
            _sizeInd = Mathf.Min(_sizeInd + 1, _sizes.Length - 1);
            Cursor.transform.localScale = new Vector3(Size * 2f, Cursor.transform.localScale.y, Size * 2f);
        }

        private void DecreaseSize () {
            _sizeInd = Mathf.Max(_sizeInd - 1, 0);
            Cursor.transform.localScale = new Vector3(Size * 2f, Cursor.transform.localScale.y, Size * 2f);
        }
    }
}