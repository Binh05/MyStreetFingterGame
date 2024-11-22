using StreetFighterGame.Characters;
using StreetFighterGame.GameEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreetFighterGame
{
    public partial class MainForm : Form
    {
        private LogicGame logicGame;
        private Timer animationTimer = new Timer();
        public MainForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            SetUpForm();
        }
        private void SetUpForm()
        {
            float gameWidth = this.ClientSize.Width;
            float gameHeight = this.ClientSize.Height;

            logicGame = new LogicGame(gameWidth, gameHeight);

            gameTimer.Interval = 32;
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            animationTimer.Interval = 100;
            animationTimer.Tick += animatePlayer;
            animationTimer.Start();

            this.KeyDown += onKeyDown;
            this.KeyUp += onKeyUp;
        }
        private void animatePlayer(object sender, EventArgs e)
        {
            logicGame.player.Update();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics Canvas = e.Graphics;

            var player = logicGame.player;

            Canvas.DrawImage(player.CurrentImage, new Rectangle((int)player.PositionX, (int)player.PositionY, player.width, player.height));
        }
        private void GameLoop(object sender, EventArgs e)
        {
            logicGame.Update();
            Invalidate();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {

        }
        private void onKeyDown(object sender, KeyEventArgs e)
        {
            logicGame.onKeyDown(e.KeyCode);
        }
        private void onKeyUp(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.D:
                    logicGame.player.stopMove();
                    break;
                case Keys.A:
                    logicGame.player.stopMove();
                    break;
            }
            logicGame.onKeyUp(e.KeyCode);
        }
    }
}
