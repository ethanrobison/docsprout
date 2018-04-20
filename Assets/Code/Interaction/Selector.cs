using Code.Characters.Doods;
using Code.Characters.Player;
using Code.Session;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace Code.Interaction
{
    [RequireComponent(typeof(Player))]
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

        [SerializeField] float _speed = 5f;

        Renderer _renderer;

        public GameObject Cursor;

        public float minDist = 1f;
        
        public MultiFocusCam Cam;

        public enum SelectionMode
        {
            Off,
            Idle,
            Selecting,
            Deselecting
        }

        [HideInInspector]public SelectionMode Mode;

        private float _size {
            get { return _sizes[_sizeInd]; }
        }

        // Use this for initialization
        void Start () {
            _pos = new Vector3(0f, 50f, 0f);
            _sizeInd = ((_sizes.Length + 1) >> 1) - 1;
            Cursor.transform.localScale = new Vector3(_size * 2f, Cursor.transform.localScale.y, _size * 2f);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.LeftBumper, DecreaseSize);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.RightBumper, IncreaseSize);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.AButton,
                () => {
                    if (Mode == SelectionMode.Idle) Mode = SelectionMode.Selecting;
                });
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.BButton,
                () => {
                    if (Mode == SelectionMode.Idle) Mode = SelectionMode.Deselecting;
                });
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.AButton,
                () => { Mode = SelectionMode.Idle; }, ActionType.onRelease);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.BButton,
                () => { Mode = SelectionMode.Idle; }, ActionType.onRelease);
            _hits = new RaycastHit[50];
            if (Configuration == ControlConfiguration.RightStick) {
                Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.YButton, () => {
                    if (Mode == SelectionMode.Off) {
                        Mode = SelectionMode.Idle;
                        _renderer.enabled = true;
                        Cursor.transform.position = Camera.main.transform.forward + transform.position;
                        GetComponent<CameraController>().enabled = false;
                        Cam.enabled = true;
                    }
                    else {
                        Mode = SelectionMode.Off;
                        _renderer.enabled = false;
                        GetComponent<CameraController>().enabled = true;
                        Cam.enabled = false;
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
                    Mode = SelectionMode.Idle;
                    return;
                }


                _renderer.enabled = true;
                Vector3 camRight = Camera.main.transform.right;
                Vector3 camForward = Vector3.Cross(Camera.main.transform.up, camRight);
                Vector3 move = camRight * Game.Sesh.Input.Monitor.LeftH - camForward * Game.Sesh.Input.Monitor.LeftV;
                AddPos(move * _speed * Time.deltaTime);
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
                

                if(Mode == SelectionMode.Selecting) {
                    if(Game.Sesh.Input.Monitor.LT < 0.1f) {
                        Mode = SelectionMode.Idle;
                    }
                }
                else if (Mode == SelectionMode.Deselecting) {
                    if(Game.Sesh.Input.Monitor.RT < 0.1f) {
                        Mode = SelectionMode.Idle;
                    }
                }
                
                if(Mode == SelectionMode.Idle) {
                    if(Game.Sesh.Input.Monitor.LT > 0.1f) {
                        Mode = SelectionMode.Selecting;
                    }
                    else if(Game.Sesh.Input.Monitor.RT > 0.1f) {
                        Mode = SelectionMode.Deselecting;
                    }
                    else {
                        return;
                    }
                }
                
                int n = Physics.SphereCastNonAlloc(_pos + Vector3.up*50f, _size, Vector3.down, _hits);
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
        
        void FixedUpdate() {
            if (Mode == SelectionMode.Off) {
                return;
            }

            Vector3 camRight = Camera.main.transform.right;
            Vector3 camForward = Vector3.Cross(Camera.main.transform.up, camRight);
            Vector3 move = camRight * Game.Sesh.Input.Monitor.RightH - camForward * Game.Sesh.Input.Monitor.RightV;
            AddPos(move * _speed * Time.deltaTime);
            _pos.y = transform.position.y;
            Vector2 fromPlayer = new Vector2(_pos.x - transform.position.x,
                _pos.z - transform.position.z);
                
            if(fromPlayer.sqrMagnitude < minDist*minDist) {
                fromPlayer.Normalize();
                _pos.x =  transform.position.x + fromPlayer.x*minDist;
                _pos.z =  transform.position.z + fromPlayer.y*minDist;
            }
        }

        public void SetPos (Vector3 pos) {
            _pos = pos;
            Cursor.transform.position = _pos;
        }

        public void AddPos (Vector3 pos) {
            _pos += pos;
            Cursor.transform.position = _pos;
        }

        public void SetPos (Vector2 pos) {
            _pos.x = pos.x;
            _pos.z = pos.y;
            Cursor.transform.position = _pos;
        }

        public void AddPos (Vector2 pos) {
            _pos.x += pos.x;
            _pos.z += pos.y;
            Cursor.transform.position = _pos;
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