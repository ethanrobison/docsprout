using UnityEngine;

namespace Code.Environment
{
    public class SineWiggle : MonoBehaviour
    {
        public float Period;
        public Transform Bone;
        public float Magnitude = 1f;

        private float _time;
        private Quaternion _rot;

        private void Start () {
            _time = Random.value;
            _rot = Bone.localRotation;

            // check period
            if (Period < 0.01f) { Period = 0.01f; }
        }

        private void Update () {
            _time += Time.deltaTime / Period;
            Bone.localRotation =
                _rot * Quaternion.AngleAxis(Magnitude * Mathf.Sin(_time * 2f * Mathf.PI), Vector3.right);
        }
    }
}