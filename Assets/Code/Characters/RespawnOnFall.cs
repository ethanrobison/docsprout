using UnityEngine;

namespace Code.Characters
{
    public class RespawnOnFall : MonoBehaviour
    {
        public Vector3 StartPos;
        public float Thresh;

        private void Update () {
            if (transform.position.y < Thresh) { Respawn(); }
        }

        private void Respawn () { transform.position = StartPos; }
    }
}