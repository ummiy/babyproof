using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Linq;
using System.Text;

namespace BabyProofV1
{
    class Interaction3 //swinging bat
    {
        Texture2D cursorR;

        Random rand;

        float gameTimer;

        Texture2D bat;
        Vector2 batPos;
        Texture2D head1;
        Texture2D head2;
        Vector2 head1Pos;
        Vector2 head2Pos;
        int head1MoveTimer;
        int head2MoveTimer;
        int head1HitCount;
        int head2HitCount;

        float armLength;

        bool head1Visible;
        bool head2Visible;
        bool batVisible;

        public void Load(ContentManager content, GraphicsDevice graphicsDevice)
        {
            rand = new Random();

            cursorR = content.Load<Texture2D>("Interaction3/hand");
            bat = content.Load<Texture2D>("Interaction3/bat");
            head1 = content.Load<Texture2D>("Interaction3/head");
            head2 = content.Load<Texture2D>("Interaction3/head");

            head1Pos = new Vector2(rand.Next(0, graphicsDevice.Viewport.Width), rand.Next(0, graphicsDevice.Viewport.Height / 2));
            head2Pos = new Vector2(rand.Next(0, graphicsDevice.Viewport.Width), rand.Next(0, graphicsDevice.Viewport.Height / 2));

            head1MoveTimer = 0;
            head2MoveTimer = 75;
            head1HitCount = 0;
            head2HitCount = 0;

            head1Visible = true;
            head2Visible = true;
            batVisible = false;
        }

        public void Update(GraphicsDevice graphicsDevice, Vector3 handRPosition, Vector3 headPosition, Vector3 elbowPosition, GameTime gameTime, Vector3 shoulderRPosition)
        {
            gameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            batPos = new Vector2(handRPosition.X, handRPosition.Y);

            if (gameTimer <= 30)
            {
                batVisible = true;

                if (head1Visible == true)
                {
                    head1MoveTimer += rand.Next(1, 3);
                    if (head1MoveTimer >= 150)
                    {
                        if (head1Visible == true)
                            head1Pos = new Vector2(rand.Next(0, (graphicsDevice.Viewport.Width - head1.Width)),
                                rand.Next((int)headPosition.Y, (int)elbowPosition.Y));
                        head1MoveTimer = 0;
                    }
                }

                if (head2Visible == true)
                {
                    head2MoveTimer += rand.Next(1, 3);
                    if (head2MoveTimer >= 150)
                    {
                        if (head2Visible == true)
                            head2Pos = new Vector2(rand.Next(0, (graphicsDevice.Viewport.Width - head1.Width)),
                                rand.Next((int)headPosition.Y, (int)elbowPosition.Y));
                        head2MoveTimer = 0;
                    }
                }

                armLength = (shoulderRPosition.Z - handRPosition.Z);

                if (head1Visible == true && batVisible == true)
                {
                    if ((batPos.X + ((bat.Width / 2) - 10)) >= head1Pos.X && (batPos.X + ((bat.Width / 2) + 10)) <= (head1Pos.X + head1.Width) && batPos.Y >= head1Pos.Y && batPos.Y <= (head1Pos.Y + head1.Height))// && armLength >= 3 && armLength <= 12)
                    {
                        head1HitCount++;
                        if (head1HitCount == 10)
                            head1Visible = false;
                        head1Pos = new Vector2(rand.Next(0, graphicsDevice.Viewport.Width - head1.Width),
                            rand.Next((int)headPosition.Y, graphicsDevice.Viewport.Height / 2));
                        head1MoveTimer = 0;
                    }
                }

                if (head2Visible == true && batVisible == true)
                {
                    if ((batPos.X + ((bat.Width / 2) - 10)) >= head2Pos.X && (batPos.X + ((bat.Width / 2) + 10)) <= (head2Pos.X + head2.Width) && batPos.Y >= head2Pos.Y && batPos.Y <= (head2Pos.Y + head2.Height))// && armLength >= 3 && armLength <= 12)
                    {
                        head2HitCount++;
                        if (head2HitCount == 10)
                            head2Visible = false;
                        head2Pos = new Vector2(rand.Next(0, graphicsDevice.Viewport.Width - head2.Width),
                            rand.Next((int)headPosition.Y, graphicsDevice.Viewport.Height / 2));
                        head2MoveTimer = 0;
                    }
                }
            }
            else
            {
                batVisible = false;
                head1Visible = false;
                head2Visible = false;
                head1Pos = head2Pos = new Vector2(-100, -100);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector3 handRPosition)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(cursorR, new Vector2(handRPosition.X, handRPosition.Y), Color.White);

            if (head1Visible == true)
                spriteBatch.Draw(head1, head1Pos, Color.White);
            if (head2Visible == true)
                spriteBatch.Draw(head2, head2Pos, Color.White);
            if (batVisible == true) 
                spriteBatch.Draw(bat, new Vector2(batPos.X, batPos.Y), Color.White);


            spriteBatch.End();
        }
    }
}
