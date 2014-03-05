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
    class Interaction1 // pulling towel or gargbage bag
    {
        Texture2D hand;

        Texture2D garbage;
        Vector2 garbagePos;
        Boolean isTouchG;

        Texture2D towel;
        Vector2 towelPos;
        Boolean isTouchT;

        float armlength;
        float range;
        float maxRange;
        float minRange;

        public void Load(ContentManager content)
        {
            hand = content.Load<Texture2D>("hand");

            towel = content.Load<Texture2D>("towel");
            towelPos = new Vector2(500, 100);
            isTouchT = false;

            garbage = content.Load<Texture2D>("bin");
            garbagePos = new Vector2(800, 100);
            isTouchG = false;

            range = 12;
            maxRange = range / 2;
            minRange = range / 4;
        }

        public void Update(Vector3 handRPosition, Vector3 shoulderRPosition)
        {
            armlength = handRPosition.X - shoulderRPosition.X;

            if (armlength < 100)
            {

                if (handRPosition.X >= towelPos.X && handRPosition.X <= (towelPos.X + towel.Width)
                    && handRPosition.Y >= towelPos.Y && handRPosition.Y <= (towelPos.Y + towel.Height))
                {
                    isTouchT = true;
                }

                if (isTouchT == true)
                {
                    if (((shoulderRPosition.Z - handRPosition.Z) <= maxRange) && ((shoulderRPosition.Z - handRPosition.Z) >= minRange))
                    {
                        towelPos.X = 400;
                    }
                }

                if (handRPosition.X >= garbagePos.X && handRPosition.X <= (garbagePos.X + garbage.Width)
                    && handRPosition.Y >= garbagePos.Y && handRPosition.Y <= (garbagePos.Y + garbage.Height))
                {
                    isTouchG = true;
                }

                if (isTouchG == true)
                {
                    if (((shoulderRPosition.Z - handRPosition.Z) <= maxRange) && ((shoulderRPosition.Z - handRPosition.Z) >= minRange))
                    {
                        garbagePos.X = 1000;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector3 handRPosition)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(towel, towelPos, Color.White);
            spriteBatch.Draw(garbage, garbagePos, Color.White);
            spriteBatch.Draw(hand, new Vector2(handRPosition.X, handRPosition.Y), Color.White);

            spriteBatch.End();
        }

    }
}
