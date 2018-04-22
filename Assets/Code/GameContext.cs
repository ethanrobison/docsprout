﻿using Code.Characters.Doods;
using Code.Characters.Player;
using Code.UI;
using UnityEngine.SceneManagement;

namespace Code
{
    public class GameContext
    {
        public DialogManager Dialogs { get; private set; }
        public DoodManager Doods { get; private set; }
        public Player Player { get; private set; }

        public void StartGame (int scene, bool testscene = false) {
            if (testscene) {
                Initialize();
                return;
            }

            SceneManager.sceneLoaded += (foo, bar) => { Initialize(); };
            SceneManager.LoadScene(scene);

            Initialize();
        }

        public void SetPlayer (Player player) { Player = player; }

        private void Initialize () {
            Dialogs = new DialogManager();
            Doods = new DoodManager();

            Dialogs.Initialize();
            Doods.Initialize();
        }

        void ShutDown () {
            Doods.ShutDown();
            Dialogs.ShutDown();

            Doods = null;
            Dialogs = null;
        }

        // todo exit a game and return to the main menu
    }

    public interface IContextManager
    {
        void Initialize ();
        void ShutDown ();
    }
}