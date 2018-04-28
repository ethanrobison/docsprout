using Code.Characters.Doods;
using Code.Session;
using UnityEngine;

namespace Code.Characters.Player.Interaction
{
    public class Selector : MonoBehaviour
    {
        private const float MIN_RADIUS = 5f, MAX_RADIUS = 10F, CHARGING_RATE = 3F;

        private readonly RaycastHit[] _hits = new RaycastHit[50];

        private Renderer _renderer;
        private Transform _cursor;

        private void Start () {
            _cursor = transform.Find("Cursor");
            // todo a more graceful way to check if the game has started
            if (Game.Ctx != null) { RegisterMappings(); }
        }

        private void RegisterMappings () {
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.BButton, OnBPress);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.YButton, () => {
                SetWhistlingState(true);
            });
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.YButton, PikminWhistle, PressType.ButtonUp);
        }

        private bool _chargingWhistle;
        private float _whistleRadius = MIN_RADIUS;

        private void Update () {
            if (!_chargingWhistle) { return; }

            _whistleRadius = Mathf.Min(_whistleRadius + CHARGING_RATE * Time.deltaTime, MAX_RADIUS);
            SetCursorScale();
        }

        private void SetWhistlingState (bool state) {
            if (!state) { _whistleRadius = MIN_RADIUS; }

            SetCursorScale();
            _chargingWhistle = state;
            _cursor.gameObject.SetActive(state);
        }

        private void SetCursorScale () {
            var size = _whistleRadius * 2f;
            _cursor.localScale = new Vector3(size, size / 1.5f, size);
        }

        private void PikminWhistle () {
            if (!_chargingWhistle) { return; }

            CastAtPosition(transform.position + Vector3.up * 50f, _whistleRadius);
            SetWhistlingState(false);
        }

        private void OnBPress () {
            if (_chargingWhistle) { SetWhistlingState(false); }
            else { Game.Ctx.Doods.DeselectAll(); }
        }

        private void CastAtPosition (Vector3 position, float size) {
            var n = Physics.SphereCastNonAlloc(position, size, Vector3.down, _hits);
            for (var i = 0; i < n; ++i) {
                var dood = _hits[i].transform.GetComponent<Dood>();
                if (dood != null) { dood.OnSelect(); }
            }
        }
    }
}