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
    class Interaction2a //putting out fire
    {
        Texture2D footRImg;

        float gameTimer;

        int fireSize = 5;

        int[] counter = new int[5]; 

        bool[] isPutOut = new bool[5];

        Random rand;

        Vector2[] firePos = new Vector2[5];

        Texture2D[] fire = new Texture2D[5];
        Texture2D[] smoke = new Texture2D[5];

        int[] footCount = new int[5];
        float[] smokeAlpha = new float[5];

        float YRange;
        float ZRange;
        float newZ;

        int extinguishedCounter = 0;
        bool gameOver = false;
        bool win = false;
        bool[] isAddedToECounter = new bool[5];

        public void Load(ContentManager content, GraphicsDevice graphicsDevice)
        {
            for (int i = 0; i < fireSize; i++)
            {
                fire[i] = content.Load<Texture2D>("Interaction2a/fire");
                smoke[i] = content.Load<Texture2D>("Interaction2a/smoke");

                firePos[i] = new Vector2(-100, -100);

                footCount[i] = 0;
                isPutOut[i] = false;

                counter[i] = 0;
            }

            rand = new Random();

            footRImg = content.Load<Texture2D>("Interaction2a/foot");

            ZRange = 74.0f - 0.0f;
            YRange = graphicsDevice.Viewport.Height - 0.0f;
        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice, Vector3 footRPosition)
        {
            gameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (gameTimer <= 100)
            {
                for (int i = 0; i < fireSize; i++)
                {
                    smokeAlpha[i] = 1.0f;

                    counter[i]++;

                    if (isPutOut[i] == false)
                    {
                        if (counter[i] == 150)
                        {
                            firePos[i].X = rand.Next(0, (graphicsDevice.Viewport.Width - fire[i].Width));
                            firePos[i].Y = rand.Next(graphicsDevice.Viewport.Height / 2, (graphicsDevice.Viewport.Height - fire[i].Height));

                            counter[i] = 0;
                        }
                    }

                    if (footRPosition.X >= firePos[i].X && footRPosition.X <= (firePos[i].X + fire[i].Width)
                        && newZ >= firePos[i].Y && newZ <= (firePos[i].Y + fire[i].Height))
                    {
                        if (footCount[i] < 2)
                        {
                            footCount[i]++;
                            firePos[i].X = rand.Next(0, (graphicsDevice.Viewport.Width - fire[i].Width));
                            firePos[i].Y = rand.Next(graphicsDevice.Viewport.Height / 2, (graphicsDevice.Viewport.Height - fire[i].Height));
                        }
                        else
                        {
                            footCount[i] = 0;
                            isPutOut[i] = true;
                        }
                    }
                }

                newZ = (((footRPosition.Z - 0) * YRange) / ZRange) + 0.0f;
            }
            else
            {
                for (int i = 0; i < fireSize; i++)
                {
                    firePos[i] = new Vector2(-100, -100);
                }
                footRPosition = new Vector3(-100, -100, 0);

                if (win == false)
                    gameOver = true;
            }

            for (int i = 0; i < fireSize; i++)
            {
                if (isPutOut[i] == true && isAddedToECounter[i] == false)
                {
                    extinguishedCounter++;
                    isAddedToECounter[i] = true;
                }
            }
            
            if (extinguishedCounter >= fireSize)
            {
                Console.WriteLine("win");
                for (int i = 0; i < fireSize; i++)
                {
                    firePos[i] = new Vector2(-100, -100);
                }
                footRPosition = new Vector3(-100, -100, 0);

                if (gameOver == false)
                    win = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector3 footRPosition)
        {
            spriteBatch.Begin();

            for (int i = 0; i < fireSize; i++)
            {
                if (isPutOut[i] == true)
                {
                    spriteBatch.Draw(smoke[i], firePos[i], new Color(255, 255, 255, smokeAlpha[i]));
                }
                else
                {
                    spriteBatch.Draw(fire[i], firePos[i], Color.White);
                }

                spriteBatch.Draw(footRImg, new Vector2(footRPosition.X, newZ), new Color(255, 255, 255, smokeAlpha[i]));
            }

            spriteBatch.End();
        }
    }
}

