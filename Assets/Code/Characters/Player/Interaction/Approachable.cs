using UnityEngine;

namespace Code.Characters.Player.Interaction
{
    public class Approachable : MonoBehaviour, IApproachable
    {
        void IApproachable.OnApproach () { Utils.Logging.Log("Hi"); }

        void IApproachable.OnDepart () { Utils.Logging.Log("Bye"); }
    }

    public interface IApproachable
    {
        void OnApproach ();
        void OnDepart ();
    }
}