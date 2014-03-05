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
    class Interaction4 //throwing balls
    {
        Texture2D[] ball = new Texture2D[4];
        Rectangle[] ballPosition = new Rectangle[4];

        float prevHandPos;
        float curHandPos;
        bool[] throwStarted = new bool[4];
        bool[] throwStopped = new bool[4];
        bool[] allowThrow = new bool[4];
        bool[] ballVisible = new bool[4];
        int ballNum = 4;

        Texture2D hand;
        Random rand;

        Texture2D head1;
        Texture2D head2;
        Vector2 head1Pos;
        Vector2 head2Pos;
        int head1MoveTimer = 0;
        int head2MoveTimer = 75;
        int head1HitCount = 0;
        int head2HitCount = 0;

        int i = 0;

        float gameTimer;

        bool head1Visible = true;
        bool head2Visible = true;

        float armSpan;

        public void Load(ContentManager content, GraphicsDevice graphicsDevice)
        {
            rand = new Random();

            hand = content.Load<Texture2D>("Interaction4/hand");
            head1 = content.Load<Texture2D>("Interaction4/head");
            head2 = content.Load<Texture2D>("Interaction4/head");

            for (int k = 0; k < ballNum; k++)
            {
                ball[k] = content.Load<Texture2D>("Interaction4/ball");
                ballVisible[k] = true;
                throwStopped[k] = false;
                throwStarted[k] = false;
                allowThrow[k] = false;
            }

            prevHandPos = curHandPos = 0.0f;

            head1Pos = new Vector2(rand.Next(0, graphicsDevice.Viewport.Width), rand.Next(0, graphicsDevice.Viewport.Height / 2));
            head2Pos = new Vector2(rand.Next(0, graphicsDevice.Viewport.Width), rand.Next(0, graphicsDevice.Viewport.Height / 2));
        }

        public void Update(GameTime gameTime, Vector3 handRPosition, Vector3 hipPosition, Vector3 shoulderRPosition, Vector3 headPosition, GraphicsDevice graphicsDevice)
        {
            gameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            armSpan = handRPosition.X - shoulderRPosition.X;

            if (headPosition.Y < 0)
                headPosition.Y = 0;

            if (gameTimer <= 30)
            {
                if (head1Visible == true)
                {
                    head1MoveTimer += rand.Next(1, 3);
                    if (head1MoveTimer >= 150)
                    {
                        if (head1Visible == true)
                            head1Pos = new Vector2(rand.Next(0, (graphicsDevice.Viewport.Width - head1.Width)),
                                rand.Next((int)headPosition.Y, graphicsDevice.Viewport.Height / 2));
                        head1MoveTimer = 0;
                    }
                }

                if (head2Visible == true)
                {
                    head2MoveTimer += rand.Next(1, 3);
                    if (head2MoveTimer >= 150)
                    {
                        if (head2Visible == true)
                            head2Pos = new Vector2(rand.Next(0, (graphicsDevice.Viewport.Width - head2.Width)),
                                rand.Next((int)headPosition.Y, (int)hipPosition.Y));
                        head2MoveTimer = 0;
                    }
                }

                if (head1Visible == true)
                {
                    if (((ballPosition[i].X + (ball[i].Width / 2)) >= head1Pos.X) && (ballPosition[i].X + (ball[i].Width / 2)) <= (head1Pos.X + head1.Width) &&
                        ((ballPosition[i].Y + (ball[i].Height / 2)) >= head1Pos.Y) && (ballPosition[i].Y + (ball[i].Height / 2)) <= (head1Pos.Y + head1.Height) &&
                        (ballPosition[i].Width <= ball[i].Width / 2) && (ballPosition[i].Height <= ball[i].Height / 2))
                    {
                        head1HitCount++;
                        ballVisible[i] = false;
                        if (head1HitCount == 10)
                            head1Visible = false;
                        head1Pos = new Vector2(rand.Next(0, graphicsDevice.Viewport.Width - head1.Width),
                            rand.Next((int)headPosition.Y, graphicsDevice.Viewport.Height / 2));
                        head1MoveTimer = 0;
                    }
                }

                if (head2Visible == true)
                {
                    if (((ballPosition[i].X + (ball[i].Width / 2)) >= head2Pos.X) && (ballPosition[i].X + (ball[i].Width / 2)) <= (head2Pos.X + head2.Width) &&
                        ((ballPosition[i].Y + (ball[i].Height / 2)) >= head2Pos.Y) && (ballPosition[i].Y + (ball[i].Height / 2)) <= (head2Pos.Y + head2.Height) &&
                        (ballPosition[i].Width <= ball[i].Width / 2) && (ballPosition[i].Height <= ball[i].Height / 2))
                    {
                        head2HitCount++;
                        ballVisible[i] = false;
                        if (head2HitCount == 10)
                            head2Visible = false;
                        head2Pos = new Vector2(rand.Next(0, graphicsDevice.Viewport.Width - head2.Width),
                            rand.Next((int)headPosition.Y, graphicsDevice.Viewport.Height / 2));
                        head2MoveTimer = 0;
                    }
                }

                if (throwStopped[i] == false) 
                {
                    if (armSpan < 100)
                    {
                        if (handRPosition.Z >= (shoulderRPosition.Z - 3))
                            allowThrow[i] = true;
                    }

                    if (throwStopped[i] == false)
                        ballPosition[i] = new Rectangle((int)(handRPosition.X), (int)(handRPosition.Y), ball[i].Width, ball[i].Height);

                    if (allowThrow[i] == true)
                    {
                        prevHandPos = curHandPos;
                        curHandPos = handRPosition.Z;

                        float diff = shoulderRPosition.Z - handRPosition.Z;

                        if (prevHandPos > curHandPos)
                        {
                            throwStarted[i] = true;

                            if (diff > 9)
                                throwStopped[i] = true;
                        }
                    }
                }
                else
                {
                    if (i < ballNum - 1)
                        i++;
                }

                for (int t = 0; t < ballNum; t++)
                {
                    if (throwStopped[t] == true)
                    {
                        if (ballPosition[t].Width >= 0)
                            ballPosition[t].Width--;

                        if (ballPosition[t].Height >= 0)
                            ballPosition[t].Height--;
                    }
                }
            }

            else
            {
                head1Visible = false;
                head2Visible = false;
                head1Pos = head2Pos = new Vector2(-100, -100);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector3 handPositionR)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(hand, new Vector2(handPositionR.X, handPositionR.Y), Color.White);

            for (int i = 0; i < ballNum; i++)
            {
                if (ballVisible[i] == true)
                    spriteBatch.Draw(ball[i], ballPosition[i], Color.White);
            }

            if (head1Visible == true)
                spriteBatch.Draw(head1, head1Pos, Color.White);

            if (head2Visible == true)
                spriteBatch.Draw(head2, head2Pos, Color.White);

            spriteBatch.End();
        }
    }
}
