using Code.Characters.Doods;
using Code.Session;
using Code.Utils;
using UnityEngine;

namespace Code.Characters.Player.Interaction
{
    public class Selector : MonoBehaviour
    {
        private const float MIN_RADIUS = 5f, MAX_RADIUS = 10F, CHARGING_RATE = 3F;

        private readonly RaycastHit[] _hits = new RaycastHit[50];

        private Renderer _renderer;
        private Vector3 _pos = new Vector3(0f, 50f, 0f);

        private void Start () {
//            _renderer = Cursor.GetRequiredComponent<Renderer>();
//            _renderer.enabled = false;
            // todo a more graceful way to check if the game has started
            if (Game.Ctx != null) { RegisterMappings(); }
        }

        private void RegisterMappings () {
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.BButton, OnBPress);
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.YButton, () => { _chargingWhistle = true; });
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.YButton, PikminWhistle, PressType.ButtonUp);
        }

        private bool _chargingWhistle;
        private float _whistleRadius = MIN_RADIUS;

        private void Update () {
            if (_chargingWhistle) {
                _whistleRadius = Mathf.Min(_whistleRadius + CHARGING_RATE * Time.deltaTime, MAX_RADIUS);
            }
        }

        private void PikminWhistle () {
            CastAtPosition(transform.position + Vector3.up * 50f, _whistleRadius);
            _chargingWhistle = false;
            _whistleRadius = MIN_RADIUS;
        }

        private static void OnBPress () { Game.Ctx.Doods.DeselectAll(); }

        private void CastAtPosition (Vector3 position, float size) {
            var n = Physics.SphereCastNonAlloc(position, size, Vector3.down, _hits);
            for (var i = 0; i < n; ++i) {
                var dood = _hits[i].transform.GetComponent<Dood>();
                if (dood != null) { dood.OnSelect(); }
            }
        }
    }
}