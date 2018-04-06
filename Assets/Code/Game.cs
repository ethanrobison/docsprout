using Code.Session;
using Code.Utils;
using UnityEngine;

namespace Code
{
    public class Game : MonoBehaviour
    {
        public static GameContext Ctx;
        public static GameSession Sesh;

        private void Start () {
            // todo I am a singleton; if I already exist, kill the Buddha

            DontDestroyOnLoad(gameObject); // Please keep me kthxbai.
            DontDestroyOnLoad(UIUtils.GetCanvas().gameObject);
            DontDestroyOnLoad(GameObject.Find("EventSystem")); // hard-coding is best coding

            Sesh = new GameSession();
            Sesh.Initialize();
        }

        public static void SetContext (GameSession sesh, GameContext ctx) {
            Debug.Assert(sesh == Sesh, "Wrong session trying to add context?");
            Ctx = ctx;
        }
    }
}