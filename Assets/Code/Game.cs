using Code.Characters.Doods.LifeCycle;
using Code.Session;
using Code.Utils;
using UnityEngine;

namespace Code
{
    public class Game : MonoBehaviour
    {
        public bool TestScene;

        public static GameContext Ctx;
        public static GameSession Sesh;
        public static GameObject GO;

        private void Awake () {
            if (GO != null) { // I already exist; destroy me
                Destroy(gameObject);
                return;
            }

            GO = gameObject;

            DontDestroyOnLoad(gameObject); // Please keep me kthxbai.
            DontDestroyOnLoad(UIUtils.GetCanvas().gameObject);
            DontDestroyOnLoad(GameObject.Find("EventSystem")); // hard-coding is best coding

            Sesh = new GameSession();
            Sesh.Initialize();
            
            if (TestScene) { Game.Sesh.StartTestGame(); }
            
            Doodopedia.LoadSpecies();
        }

        public static void SetContext (GameSession sesh, GameContext ctx) {
            Logging.Assert(sesh == Sesh, "Wrong session trying to add context?");
            Ctx = ctx;
        }
    }
}