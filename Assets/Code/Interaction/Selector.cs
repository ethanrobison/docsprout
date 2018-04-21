using System.Security.Cryptography.X509Certificates;
using Code.Characters.Doods;
using Code.Characters.Player;
using Code.Session;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace Code.Interaction
{
    public class Selector : MonoBehaviour
    {
        public enum ControlConfiguration
        {
            LeftStick,
            RightStick
        }

        public static ControlConfiguration Configuration = ControlConfiguration.RightStick;
        [SerializeField] private float[] _sizes;
        private int _sizeInd;
        private RaycastHit[] _hits;
        private Vector3 _pos;

        [SerializeField] float _maxSpeed = 8f;
        [SerializeField] float _minSpeed = 3f;
        [SerializeField] float _speedConstant = 10f;

        Renderer _renderer;

        public GameObject Cursor;

        public float MinDist = 2f;
        public float MaxDist = 15f;

        public MultiFocusCam Cam;

        public enum SelectionMode
        {
            Off,
            Idle,
            Selecting,
            Deselecting
        }

        [HideInInspector] public SelectionMode Mode;

        private float _size {
            get { return _sizes[_sizeInd]; }
        }

        // Use this for initialization
        void Start () {
            _pos = new Vector3(0f, 50f, 0f);
            _sizeInd = ((_sizes.Length + 1) >> 1) - 1;
            _hits = new RaycastHit[50];
            Cursor.transform.localScale = new Vector3(_size * 2f, Cursor.transform.localScale.y, _size * 2f);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.LeftBumper, DecreaseSize);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.RightBumper, IncreaseSize);
            if (Configuration == ControlConfiguration.LeftStick) {
                Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.AButton,
                    () => {
                        if (Mode == SelectionMode.Idle) Mode = SelectionMode.Selecting;
                    });
                Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.BButton,
                    () => {
                        if (Mode == SelectionMode.Idle) Mode = SelectionMode.Deselecting;
                    });
                Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.AButton,
                    () => { Mode = SelectionMode.Idle; }, InputMonitor.PressType.ButtonUp);
                Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.BButton,
                    () => { Mode = SelectionMode.Idle; }, InputMonitor.PressType.ButtonUp);
            }

            if (Configuration == ControlConfiguration.RightStick) {
                Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.YButton, () => {
                    if (Mode == SelectionMode.Off) {
                        Mode = SelectionMode.Idle;
                        _renderer.enabled = true;
                        Vector3 camForward = Vector3.Cross(Camera.main.transform.up, Camera.main.transform.right);
                        SetPos(-camForward * MinDist);
                        GetComponent<CameraController>().enabled = false;
                        Cam.enabled = true;
                    }
                    else {
                        Mode = SelectionMode.Off;
                        _renderer.enabled = false;
                        GetComponent<CameraController>().enabled = true;
                        Cam.enabled = false;
                        Cam.RelinquishControl(GetComponent<CameraController>());
                    }
                });

                Cam.enabled = false;
                GetComponent<CameraController>().enabled = true;
            }


            _renderer = Cursor.GetComponent<Renderer>();
            _renderer.enabled = false;
//            _pos = new Vector3(0f, 100f, 0f);
        }

        // Update is called once per frame
        void Update () {
            if (Configuration == ControlConfiguration.LeftStick) {
                if (Game.Sesh.Input.Monitor.LT < 0.1f) {
                    Cursor.GetComponent<Renderer>().enabled = false;
                    Mode = SelectionMode.Off;
                    return;
                }

                if (Mode == SelectionMode.Off) {
                    Mode = SelectionMode.Idle;
                    SetPos(Vector2.zero);
                }


                _renderer.enabled = true;
                Vector3 camRight = Camera.main.transform.right;
                Vector3 camForward = Vector3.Cross(Camera.main.transform.up, camRight);
                Vector3 move = camRight * Game.Sesh.Input.Monitor.LeftH - camForward * Game.Sesh.Input.Monitor.LeftV;
                AddPos(move * _maxSpeed * Time.deltaTime);
                if (Mode == SelectionMode.Idle) return;
                int n = Physics.SphereCastNonAlloc(_pos, _size, Vector3.down, _hits);
                for (int i = 0; i < n; ++i) {
                    if (!_hits[i].transform.GetComponent<Dood>()) {
                        continue;
                    }

                    if (Mode == SelectionMode.Selecting) {
                        _hits[i].transform.GetComponent<ISelectable>().OnSelect();
                    }
                    else if (Mode == SelectionMode.Deselecting) {
                        _hits[i].transform.GetComponent<ISelectable>().OnDeselect();
                    }
                }
            }
            else {
                if (Mode == SelectionMode.Selecting) {
                    if (Game.Sesh.Input.Monitor.RT < 0.1f) {
                        Mode = SelectionMode.Idle;
                    }
                }
                else if (Mode == SelectionMode.Deselecting) {
                    if (Game.Sesh.Input.Monitor.LT < 0.1f) {
                        Mode = SelectionMode.Idle;
                    }
                }

                if (Mode == SelectionMode.Idle) {
                    if (Game.Sesh.Input.Monitor.RT > 0.1f) {
                        Mode = SelectionMode.Selecting;
                    }
                    else if (Game.Sesh.Input.Monitor.LT > 0.1f) {
                        Mode = SelectionMode.Deselecting;
                    }
                    else {
                        return;
                    }
                }

                int n = Physics.SphereCastNonAlloc(_pos + transform.position + Vector3.up * 50f, _size, Vector3.down,
                    _hits);
                for (int i = 0; i < n; ++i) {
                    if (!_hits[i].transform.GetComponent<Dood>()) {
                        continue;
                    }

                    if (Mode == SelectionMode.Selecting) {
                        _hits[i].transform.GetComponent<ISelectable>().OnSelect();
                    }
                    else if (Mode == SelectionMode.Deselecting) {
                        _hits[i].transform.GetComponent<ISelectable>().OnDeselect();
                    }
                }
            }
        }

        void FixedUpdate () {
            if (Mode == SelectionMode.Off) {
                return;
            }

            float curSpeed = _pos.magnitude;
            curSpeed = _minSpeed + (_maxSpeed - _minSpeed) * curSpeed / (curSpeed + _speedConstant);
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

        void IncreaseSize () {
            _sizeInd = Mathf.Min(_sizeInd + 1, _sizes.Length - 1);
            Cursor.transform.localScale = new Vector3(_size * 2f, Cursor.transform.localScale.y, _size * 2f);
        }

        void DecreaseSize () {
            _sizeInd = Mathf.Max(_sizeInd - 1, 0);
            Cursor.transform.localScale = new Vector3(_size * 2f, Cursor.transform.localScale.y, _size * 2f);
        }
    }
}