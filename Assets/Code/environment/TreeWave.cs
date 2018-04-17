using UnityEngine;

namespace Code.Environment
{
    public class TreeWave : MonoBehaviour
    {
        public float Period1;
        public Transform Bone1;
        public float Period2;
        public Transform Bone2;

        private float _time1;
        private float _time2;

        private Quaternion _rot1;
        private Quaternion _rot2;

        private void Start () {
            _time1 = Random.value;
            _time2 = Random.value;
            _rot1 = Bone1.localRotation;
            _rot2 = Bone2.localRotation;
        }

        // Update is called once per frame
        private void Update () {
            _time1 += Time.deltaTime / Period1;
            _time1 %= 1f;
            _time2 += Time.deltaTime / Period2;
            _time2 %= 1f;
            Bone1.localRotation = _rot1 * Quaternion.AngleAxis(Mathf.Sin(_time1 * 2f * Mathf.PI), Vector3.right);
            Bone2.localRotation = _rot2 * Quaternion.AngleAxis(Mathf.Sin(_time2 * 2f * Mathf.PI), Vector3.right);
        }
    }
}