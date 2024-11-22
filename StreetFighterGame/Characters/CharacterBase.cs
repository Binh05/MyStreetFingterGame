using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace StreetFighterGame.Characters
{
    public enum ActionState
    {
        MoveForward,
        MoveBack,
        Jumping,
        Standing,
    }
    public class CharacterBase
    {
        public int Healt { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public bool isSitDown { get; set; }
        public bool isAttacking { get; set; }

        public int width = 64;
        public int height = 134;
        public float Speed = 6;

        private float gravity = 1;
        private float air = 0.5f;
        private float step = 5;

        private bool isFacingRight = true;
        public Image CurrentImage { get; private set; }

        private Dictionary<ActionState, List<Image>> animations;
        private ActionState currentState = ActionState.Standing;
        private int currentFrame = 0;
        private float jumpSpeed = -10;

        public CharacterBase(int healt, float positionX, float positionY)
        {
            Healt = healt;
            PositionX = positionX;
            PositionY = positionY;
            isSitDown = false;
            isAttacking = false;

            animations = LoadAnimations();
            CurrentImage = animations[ActionState.Standing][0];
        }

        private Dictionary<ActionState, List<Image>> LoadAnimations()
        {
            var animatePlayer = new Dictionary<ActionState, List<Image>>();
            animatePlayer[ActionState.Standing] = LoadImage("..\\..\\Assets\\Images\\GokuMui", "GokuMui_0", 4);
            animatePlayer[ActionState.Jumping] = LoadImage("..\\..\\Assets\\Images\\GokuMui", "GokuMui_40", 5);
            animatePlayer[ActionState.MoveForward] = LoadImage("..\\..\\Assets\\Images\\GokuMui", "GokuMui_20", 4);
            animatePlayer[ActionState.MoveBack] = LoadImage("..\\..\\Assets\\Images\\GokuMui", "GokuMui_20", 4);

            return animatePlayer;
        }

        private List<Image> LoadImage(string folderPath, string filePrefix, int count)
        {
            var image = new List<Image>();
            for (int i=0; i < count; i++)
            {
                string filePath = $"{folderPath}\\{filePrefix}-{i}.png";
                if (File.Exists(filePath))
                {
                    image.Add(Image.FromFile(filePath));
                }
            }

            return image;
        }

        public void Update()
        {
            UpdateFrame();
        }

        public void UpdateAction()
        {
            if ( currentState == ActionState.Jumping )
            {
                Jump();
            }else if ( currentState == ActionState.MoveForward) 
            {
                moveRight();
            }else if ( currentState == ActionState.MoveBack)
            {
                moveLeft();
            }
        }

        private void UpdateFrame()
        {
            var frames = animations[currentState];
            currentFrame = (currentFrame + 1) % frames.Count;
            CurrentImage = frames[currentFrame];
        }

        private void ChangeState(ActionState newState)
        {
            // Ngăn chặn thay đổi trạng thái khi đang nhảy (trừ khi nhảy xong hoặc đặc biệt như tấn công trên không)
            /*if (currentState == ActionState.Jumping && newState != ActionState.Standing) return;

            if (currentState == newState) return;

            currentState = newState;
            currentFrame = 0;

            if (animations.ContainsKey(newState))
            {
                CurrentImage = animations[newState][0];
            }
            else
            {
                CurrentImage = animations[ActionState.Standing][0];
            }*/
            /*
            if (currentState == newState) return;
            if (currentState == ActionState.Jumping && newState != ActionState.Standing) return;

            currentState = newState;
            currentFrame = 0;
            CurrentImage = animations.ContainsKey(newState) ? animations[newState][0] : animations[ActionState.Standing][0];*/

            // Ngăn chặn thay đổi trạng thái khi đang nhảy
            if (currentState == ActionState.Jumping && newState != ActionState.Standing) return;

            if (currentState == newState) return;

            currentState = newState;
            currentFrame = 0;

            if (animations.ContainsKey(newState))
            {
                CurrentImage = animations[newState][currentFrame];
            }
            else
            {
                CurrentImage = animations[ActionState.Standing][0];
            }
        }

        private void Jump()
        {
            PositionY += this.jumpSpeed;
            jumpSpeed += gravity;

           /* if (PositionY >= 200)
            {
                PositionY = 200;
                ChangeState(ActionState.Standing);
                jumpSpeed = -10;
            }*/

            if (PositionY < 200) // Đang nhảy
            {
                currentFrame = Math.Min(currentFrame + 1, animations[ActionState.Jumping].Count - 2);
                CurrentImage = animations[currentState][currentFrame];
            }
            else // Đã tiếp đất
            {
                PositionY = 200;
                jumpSpeed = -10;

                // Chuyển sang frame cuối cùng
                currentFrame = animations[ActionState.Jumping].Count - 1;
                CurrentImage = animations[ActionState.Jumping][currentFrame];

                // Trở lại trạng thái đứng
                ChangeState(ActionState.Standing);
            }
        }

        public void handleJump()
        {
            if (currentState != ActionState.Jumping)
            {
                ChangeState(ActionState.Jumping);
                currentFrame = 0;
                CurrentImage = animations[ActionState.Jumping][currentFrame];
            }
        }

        public void moveRight ()
        {

            if (currentState != ActionState.MoveForward)
            {
                ChangeState(ActionState.MoveForward);
                PositionY += height - 84;
                this.width = 110;
                this.height = 84;

            }
            this.PositionX += step;
        }
        public void moveLeft ()
        {
            if (isFacingRight)
            {
                FlipImageHorizontally();
                isFacingRight = false;
            }
            if (currentState != ActionState.MoveBack)
            {
                ChangeState(ActionState.MoveBack);
                PositionY += height - 84;
                this.width = 110;
                this.height = 84;

            }
            this.PositionX -= step;
        }
        private void FlipImageHorizontally()
        {
            if (CurrentImage != null)
            {
                // Tạo một bản sao của hình ảnh hiện tại
                Bitmap flippedImage = new Bitmap(CurrentImage);

                // Lật hình ảnh theo chiều ngang
                flippedImage.RotateFlip(RotateFlipType.RotateNoneFlipX);

                // Cập nhật hình ảnh hiện tại
                CurrentImage = flippedImage;
            }
        }
        public void stopMove()
        {
            this.width = 64;
            this.height = 134;
            PositionY = 200;
            ChangeState(ActionState.Standing);
        }
    }
}
