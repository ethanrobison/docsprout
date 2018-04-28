using Code.Characters.Doods;
using Code.Characters.Doods.Needs;
using UnityEngine;

namespace Code.Environment.Advertising
{
    public class FunSatisfier : Satisfier
    {
        public Vector3 AppliedForce;
        private Rigidbody _rig;
        private void Start () { _rig = transform.parent.GetComponent<Rigidbody>(); }

        public override void InteractWith (Dood dood) {
            
            _rig.AddForce(dood.transform.TransformVector(AppliedForce), ForceMode.VelocityChange);
        }

        public override NeedType Satisfies () { return NeedType.Fun; }
    }
}