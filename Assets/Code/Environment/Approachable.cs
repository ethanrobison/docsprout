using UnityEngine;

namespace Code.Environment
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