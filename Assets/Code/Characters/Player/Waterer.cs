using Code.Characters.Doods.Needs;
using Code.Session;
using Code.Utils;
using UnityEngine;

namespace Code.Characters.Player
{
    public class Waterer : MonoBehaviour
    {
        public Transform SpherePos;

        private float _radius;

        private void Start () {
            var sphere = gameObject.GetRequiredComponent<SphereCollider>();
            _radius = sphere.radius;
            Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.XButton, WaterNearby);
        }

        private void WaterNearby () {
            var cols = Physics.OverlapSphere(SpherePos.position, _radius);
            foreach (var col in cols) {
                var waterable = col.gameObject.GetComponent<Waterable>();
                if (waterable != null) { waterable.Satisfy(); }
            }
        }
    }
}