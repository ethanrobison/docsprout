using System.Runtime.InteropServices;
using Code.environment;
using Code.Utils;
using UnityEngine;

namespace Code.Environment
{
    public class GateDoor : MonoBehaviour, IApproachable
    {
        private Gate _gate;

        // Use this for initialization
        void Start () { _gate = GetComponentInParent<Gate>(); }

        public void Interact () { _gate.Interact(); }

        public void SecondaryInteract () { }

        public void OnApproach () { _gate.OnApproach(); }

        public void OnDepart () { _gate.OnDepart(); }
    }
}