using Code.Session;
using UnityEngine;

namespace Code.Characters.Player
{
    public class TreatGiver : MonoBehaviour
    {
        private Object _treat;

        private void Start () {
            _treat = Resources.Load("Environment/Donut");
        }

        public void GiveTreat () {
            var treatPos = transform.position + transform.forward * 2f + transform.up;
            var treatRot = Quaternion.Euler(-60f, 0f, 0f);
            Instantiate(_treat, treatPos, treatRot);
        }
    }
}