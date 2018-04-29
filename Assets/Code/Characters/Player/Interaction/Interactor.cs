using Code.Environment;
using Code.Session;
using UnityEngine;

namespace Code.Characters.Player.Interaction
{
    /// <inheritdoc />
    /// <summary>
    ///  The interactor that approaches or departs IApproachable's.
    /// </summary>
    public class Interactor : MonoBehaviour
    {
        private IApproachable _target;

        private void Start () { Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.XButton, Interact); }

        private void OnTriggerEnter (Collider other) {
            if (_target != null) { return; }

            var approachable = other.GetComponent<IApproachable>();
            if (approachable == null) { return; }

            _target = approachable;
            _target.OnApproach();
        }

        private void OnTriggerExit (Collider other) {
            if (_target == null) { return; }

            var approachable = other.GetComponent<IApproachable>();
            if (approachable == null || approachable != _target) { return; }

            _target.OnDepart();
            _target = null;
        }

        private void Interact () {
            if (_target != null) { _target.Interact(); }
        }
    }
}