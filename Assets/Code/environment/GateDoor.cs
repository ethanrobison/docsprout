using System.Runtime.InteropServices;
using Code.Utils;
using UnityEngine;

namespace Code.Environment
{
    public class GateDoor : MonoBehaviour, IApproachable
    {
        private Gate _gate;

        private void Start () { _gate = GetComponentInParent<Gate>(); }

        public void Interact () { _gate.Interact(); }

        public void OnApproach () { _gate.OnApproach(); }

        public void OnDepart () { _gate.OnDepart(); }
    }
}