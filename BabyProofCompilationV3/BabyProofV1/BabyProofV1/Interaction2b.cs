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
    class Interaction2b //picking up trash
    {
        int trashSize = 5;

        float gameTimer;

        Random rand;

        Texture2D[] trash = new Texture2D[5];
        Texture2D garbage;

        bool[] inBin = new bool[5];

        Vector2[] trashPos = new Vector2[5];
        Vector2 garbagePos;

        Texture2D handImg;
        Vector2 handImgPos;

        bool gameOver = false;
        bool win = false;
        int inBinCounter = 0;
        bool[] isAddedToBinCounter = new bool[5];

        public void Load(ContentManager content, GraphicsDevice graphicsDevice)
        {
            handImg = content.Load<Texture2D>("Interaction2b/hand");
            handImgPos = new Vector2(0, 0);

            rand = new Random();
            garbage = content.Load<Texture2D>("Interaction2b/bin");

            for (int i = 0; i < 5; i++)
            {
                inBin[i] = false;
                trash[i] = content.Load<Texture2D>("Interaction2b/trash");
                trashPos[i] = new Vector2(rand.Next(0, graphicsDevice.Viewport.Width), rand.Next(0, graphicsDevice.Viewport.Height));
                isAddedToBinCounter[i] = false;
            }
        }

        public void Update(GraphicsDevice graphicsDevice, GameTime gameTime, Vector3 handRPosition)
        {
            gameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            handImgPos = new Vector2(handRPosition.X, handRPosition.Y);

            if (gameTimer <= 70)
            {
                for (int i = 0; i < trashSize; i++)
                {
                    if (trashPos[i].X >= garbagePos.X && trashPos[i].X <= garbagePos.X + garbage.Width
                    && trashPos[i].Y >= garbagePos.Y && trashPos[i].Y <= garbagePos.Y + garbage.Height)
                    {
                        inBin[i] = true;
                    }

                    if (inBin[i] == false)
                    {
                        if (trashPos[i].X >= graphicsDevice.Viewport.Width - (trash[i].Width))
                        {
                            trashPos[i].X = graphicsDevice.Viewport.Width - (trash[i].Width);
                        }
                        if (trashPos[i].X <= 0)
                        {
                            trashPos[i].X = 0;
                        }

                        if (trashPos[i].Y >= graphicsDevice.Viewport.Height - trash[i].Height)
                        {
                            trashPos[i].Y = graphicsDevice.Viewport.Height - (trash[i].Height);
                        }
                        if (trashPos[i].Y <= 0)
                        {
                            trashPos[i].Y = 0;
                        }

                        if (handRPosition.Y >= trashPos[i].Y && handRPosition.Y <= trashPos[i].Y + trash[i].Height)
                        {
                            if ((handRPosition.X + handImg.Width) >= trashPos[i].X && (handRPosition.X + handImg.Width) <= (trashPos[i].X + trash[i].Width / 2))
                            {
                                trashPos[i].X++;
                            }

                            if (handRPosition.X <= (trashPos[i].X + trash[i].Width) && handRPosition.X >= (trashPos[i].X + trash[i].Width / 2))
                            {
                                trashPos[i].X--;
                            }
                        }

                        if (handRPosition.X >= trashPos[i].X && handRPosition.X <= trashPos[i].X + trash[i].Width)
                        {
                            if ((handRPosition.Y + handImg.Width) >= trashPos[i].Y && (handRPosition.Y + handImg.Width) <= (trashPos[i].Y + trash[i].Height / 2))
                            {
                                trashPos[i].Y++;
                            }

                            if (handRPosition.Y <= (trashPos[i].Y + trash[i].Height) && handRPosition.Y >= (trashPos[i].Y + trash[i].Height / 2))
                            {
                                trashPos[i].Y--;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < trashSize; i++)
                {
                    trashPos[i] = new Vector2(-100, -100);
                }
                garbagePos = new Vector2(-200, -200);

                if (win == false)
                    gameOver = true;
            }

            for (int i = 0; i < trashSize; i++)
            {
                if (inBin[i] == true && isAddedToBinCounter[i] == false)
                {
                    inBinCounter++;
                    isAddedToBinCounter[i] = true;
                }
            }

            if (inBinCounter >= trashSize) 
            {
                for (int i = 0; i < trashSize; i++)
                {
                    trashPos[i] = new Vector2(-100, -100);
                }
                garbagePos = new Vector2(-200, -200);

                if (gameOver == false)
                    win = true;
            }
        }

        public void Draw( SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(garbage, garbagePos, Color.White);
            spriteBatch.Draw(handImg, handImgPos, Color.White);


            for (int i = 0; i < 5; i++)
            {
                spriteBatch.Draw(trash[i], trashPos[i], Color.White);
            }

            spriteBatch.End();
        }
        
    }
}
