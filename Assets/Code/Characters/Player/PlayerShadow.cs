using UnityEngine;

namespace Code.Characters.Player
{
    public class PlayerShadow : MonoBehaviour
    {
        void Update () { transform.rotation = Quaternion.identity; }
    }
}