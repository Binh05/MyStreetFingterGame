using StreetFighterGame.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreetFighterGame.GameEngine
{
    public class LogicGame
    {
        public CharacterBase player {  get; private set; }
        private float gameWidth;
        private float gameHeight;

        private HashSet<Keys> pressedKey = new HashSet<Keys>();

        public LogicGame(float Width, float Height)
        {
            gameWidth = Width;
            gameHeight = Height;
            InitializeGame();
        }

        private void InitializeGame()
        {
            player = new CharacterBase(100, 50, 200);
        }

        public void onKeyDown(Keys key)
        {
            pressedKey.Add(key);
        }
        public void onKeyUp(Keys key)
        {
            pressedKey.Remove(key);
        }

        public void Update()
        {
            ProcessInput();

            player.UpdateAction();
        }

        private void ProcessInput()
        {
            if (pressedKey.Contains(Keys.D)) player.moveRight();
            if (pressedKey.Contains(Keys.K)) player.handleJump();
            if (pressedKey.Contains(Keys.A)) player.moveLeft();
        }
    }
}
