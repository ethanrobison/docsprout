using Code.Characters.Doods;
using Code.Session;
using UnityEngine;

namespace Code.Interaction
{
    public class Selector : MonoBehaviour
    {
        [SerializeField] private float[] _sizes;
        private int _sizeInd;
        private RaycastHit[] _hits;
        private Vector3 _pos;

        [SerializeField] float _speed = 5f;

        public GameObject Cursor;

        public enum SelectionMode
        {
//            Idle,
            Selecting,
            Deselecting
        }

        public SelectionMode Mode;

        private float _size {
            get { return _sizes[_sizeInd]; }
        }

        // Use this for initialization
        void Start () {
            _pos = new Vector3(0f, 50f, 0f);
            _sizeInd = (_sizes.Length + 1) / 2;
            Cursor.transform.localScale = new Vector3(_size * 2f, Cursor.transform.localScale.y, _size * 2f);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.LeftBumper, DecreaseSize);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.RightBumper, IncreaseSize);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.AButton,
                () => { Mode = SelectionMode.Selecting; });
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.BButton,
                () => { Mode = SelectionMode.Deselecting; });
            _hits = new RaycastHit[50];

            Cursor.GetComponent<Renderer>().enabled = false;
//            _pos = new Vector3(0f, 100f, 0f);
        }

        // Update is called once per frame
        void Update () {
            if (Game.Sesh.Input.Monitor.LT < 0.1f) {
                Cursor.GetComponent<Renderer>().enabled = false;
                Mode = SelectionMode.Selecting;
                return;
            }

            Cursor.GetComponent<Renderer>().enabled = true;
            Vector3 camRight = Camera.main.transform.right;
            Vector3 camForward = Vector3.Cross(Camera.main.transform.up, camRight);
            Vector3 move = camRight * Game.Sesh.Input.Monitor.LeftH - camForward * Game.Sesh.Input.Monitor.LeftV;
            AddPos(move * _speed * Time.deltaTime);
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