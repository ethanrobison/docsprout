using Code.Characters.Doods;
using Code.Environment;
using Code.Environment.Advertising;
using Code.Utils;
using UnityEngine;

namespace Code.environment
{
    public class Beachball : MonoBehaviour, IApproachable
    {
        public Vector3 Force = new Vector3(0f, 5f, 5f);

        private Rigidbody _rig;
        private float _cooldown;
        private const float COOLDOWN_TIME = .1f;
        private AudioSource _audioSource;
        private InteractionHighlight _highlight;

        private void Start () {
            _rig = gameObject.GetRequiredComponent<Rigidbody>();
            _highlight = gameObject.GetRequiredComponent<InteractionHighlight>();
            _audioSource = gameObject.GetRequiredComponent<AudioSource>();
            gameObject.GetRequiredComponentInChildren<Satisfier>().OnInteract += dood => { Kick(dood.transform); };
        }

        private void Update () {
            if (_cooldown > 0f) {
                _cooldown -= Time.deltaTime;
            }
        }


        private void Kick (Transform tran) {
            if (_cooldown > 0f) return;
            _cooldown += COOLDOWN_TIME;
            _audioSource.Play();
            _rig.AddForce(tran.TransformVector(Force), ForceMode.VelocityChange);
        }

        public void Interact () { Kick(Game.Ctx.Player.transform); }

        public void OnApproach () { _highlight.IsHighlighted = true; }
        public void OnDepart () { _highlight.IsHighlighted = false; }
    }
}