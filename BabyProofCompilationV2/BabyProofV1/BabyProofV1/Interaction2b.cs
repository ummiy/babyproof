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
    class Interaction2b
    {
        MouseState mouse;

        int trashSize = 5;

        Random rand;

        Texture2D[] trash = new Texture2D[5];
        Texture2D garbage;
        //Texture2D trash;

        bool[] inBin = new bool[5];

        Vector3 handPositionR = new Vector3(0, 0, 0);

        Vector2[] trashPos = new Vector2[5];
        Vector2 garbagePos;
        //Vector2 trashPos = new Vector2(0, 0);

        Texture2D cursorR;

        public void Load(ContentManager content, GraphicsDevice graphicsDevice)
        {
            cursorR = content.Load<Texture2D>("Interaction2b/hand");

            rand = new Random();
            garbage = content.Load<Texture2D>("Interaction2b/bin");

            for (int i = 0; i < 5; i++)
            {
                inBin[i] = false;
                trash[i] = content.Load<Texture2D>("Interaction2b/trash");
                trashPos[i] = new Vector2(rand.Next(0, graphicsDevice.Viewport.Width), rand.Next(0, graphicsDevice.Viewport.Height));
            }
        }

        public void Update(GraphicsDevice graphicsDevice)
        {
            mouse = Mouse.GetState();

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

                    if (mouse.Y >= trashPos[i].Y && mouse.Y <= trashPos[i].Y + trash[i].Height)
                    //(handPositionR.Y >= trashPos[i].Y && handPositionR.Y <= trashPos[i].Y + trash[i].Height)
                    {
                        if ((mouse.X + cursorR.Width) >= trashPos[i].X && (mouse.X + cursorR.Width) <= (trashPos[i].X + trash[i].Width / 2))
                        //((handPositionR.X + cursorR.Width) >= trashPos[i].X && (handPositionR.X + cursorR.Width) <= (trashPos[i].X + trash[i].Width / 2))
                        {
                            trashPos[i].X++;
                        }

                        if (mouse.X <= (trashPos[i].X + trash[i].Width) && mouse.X >= (trashPos[i].X + trash[i].Width / 2))
                        //(handPositionR.X <= (trashPos[i].X + trash[i].Width) && handPositionR.X >= (trashPos[i].X + trash[i].Width / 2))
                        {
                            trashPos[i].X--;
                        }
                    }

                    if (mouse.X >= trashPos[i].X && mouse.X <= trashPos[i].X + trash[i].Width)
                    //(handPositionR.X >= trashPos[i].X && handPositionR.X <= trashPos[i].X + trash[i].Width)
                    {
                        if ((mouse.Y + cursorR.Width) >= trashPos[i].Y && (mouse.Y + cursorR.Width) <= (trashPos[i].Y + trash[i].Height / 2))
                        //((handPositionR.Y+ cursorR.Width) >= trashPos[i].Y && (handPositionR.Y + cursorR.Width) <= (trashPos[i].Y + trash[i].Height / 2))
                        {
                            trashPos[i].Y++;
                        }

                        if (mouse.Y <= (trashPos[i].Y + trash[i].Height) && mouse.Y >= (trashPos[i].Y + trash[i].Height / 2))
                        //(handPositionR.Y <= (trashPos[i].Y + trash[i].Height) && handPositionR.Y >= (trashPos[i].Y + trash[i].Height / 2))
                        {
                            trashPos[i].Y--;
                        }
                    }
                }
            }
        }

        public void Draw( SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(garbage, garbagePos, Color.White);
            spriteBatch.Draw(cursorR, new Vector2(mouse.X, mouse.Y), Color.White);
            //spriteBatch.Draw(cursorR, new Vector2(handPositionR.X, handPositionR.Y), Color.White);


            for (int i = 0; i < 5; i++)
            {
                spriteBatch.Draw(trash[i], trashPos[i], Color.White);
            }

            spriteBatch.End();
        }
        
    }
}
