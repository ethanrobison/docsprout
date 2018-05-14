﻿using Code.Session;
using Code.Utils;
using UnityEngine;

namespace Code
{
    public class Game : MonoBehaviour
    {
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

            Sesh = new GameSession();
            Sesh.Initialize();
        }

        public static void SetContext (GameSession sesh, GameContext ctx) {
            Logging.Assert(sesh == Sesh, "Wrong session trying to add context?");
            Ctx = ctx;
        }
    }
}