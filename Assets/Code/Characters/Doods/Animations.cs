using UnityEngine;

namespace Code.Characters.Doods
{
    public class Animations : MonoBehaviour
    {
        private Rigidbody _rb;
        public Animator Anim;

        private void Start () { _rb = GetComponent<Rigidbody>(); }

        private void Update () { Anim.SetBool("isWalking", _rb.velocity.sqrMagnitude >= 0.01f); }
    }
}