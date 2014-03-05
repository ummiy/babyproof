using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Babyproof_CompositionTesting
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        MouseState mouse;

        Boolean isScene1, isScene2, isScene3;
        Boolean isS1Play, isS2Play, isS3Play;

        Boolean isInteraction1, isInteraction2;

        Boolean isInt1Done, isInt2Done;

        Video scene1, scene2, scene3;
        VideoPlayer vPlayer;
        Texture2D vTexture;

        Texture2D testBG;
        Texture2D testIMG;
        Texture2D testIMG2;
        Vector2 testImgPos;
        Vector2 testImgPos2;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            isScene1 = true;
            isScene2 = false;
            isScene3 = false;

            isS1Play = true;
            isS2Play = true;
            isS3Play = true;

            isInteraction1 = false;
            isInteraction2 = false;

            isInt1Done = false;
            isInt2Done = false;

            scene1 = Content.Load<Video>("Scene1");
            scene2 = Content.Load<Video>("Scene2");
            scene3 = Content.Load<Video>("Scene3");
            
            vPlayer = new VideoPlayer();

            testBG = Content.Load<Texture2D>("canalBN");
            testIMG = Content.Load<Texture2D>("exitH");
            testIMG2 = Content.Load<Texture2D>("inventLocketActive");
            testImgPos = new Vector2(0, 0);
            testImgPos2 = new Vector2(0, 0);
        }

        protected override void UnloadContent()
        {
        }

        private void startScene1(Video video)
        {
            vPlayer.Play(video);
            isS1Play = false;
        }

        private void startScene2(Video video)
        {
            vPlayer.Play(video);
            isS2Play = false;
        }

        private void startScene3(Video video)
        {
            vPlayer.Play(video);
            isS3Play = false;
        }

        protected override void Update(GameTime gameTime)
        {
            //Console.WriteLine(isScene1 + " " + isScene2 + " " + isScene3);
            Console.WriteLine(isScene2 + " " + isS2Play);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (isScene1 == true)
            {
                if(isS1Play == true)
                    startScene1(scene1);
            }

            if (isScene2 == true)
            {
                if (isS2Play == true)
                    startScene2(scene2);
            }

            if (isScene3 == true)
            {
                if (isS3Play == true)
                    startScene3(scene3);
            }

            if (isS1Play == false && isScene1 == false)
            {
                isInteraction1 = true;
            }

            if (isInteraction1 == true)
            {
                mouse = Mouse.GetState();

                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    isInt1Done = true;
                }
            }
            
            if (isInt1Done == true)
            {
                isScene2 = true;
                isInt1Done = false;
            }

            if (isS2Play == false && isScene2 == false)
            {
                isInteraction2 = true;
            }

            if (isInteraction2 == true)
            {
                mouse = Mouse.GetState();

                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    isInt2Done = true;
                }
            }

            if (isInt2Done == true)
            {
                isScene3 = true;
                isInt2Done = false;
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            if (isScene1 == true)
            {
                if (vPlayer.State != MediaState.Stopped) 
                    vTexture = vPlayer.GetTexture();

                if (vPlayer.State == MediaState.Stopped)
                {
                    isScene1 = false;
                    //isScene2 = true;
                }

                Rectangle screen = new Rectangle(0, 0, 800, 600);

                if (vTexture != null)
                {
                    spriteBatch.Draw(vTexture, screen, Color.White);
                }
            }
            else if (isScene2 == true)
            {
                if (vPlayer.State != MediaState.Stopped)
                    vTexture = vPlayer.GetTexture();

                if (vPlayer.State == MediaState.Stopped)
                {
                    isScene2 = false;
                    //isScene3 = true;
                }

                Rectangle screen = new Rectangle(0, 0, 800, 600);

                if (vTexture != null)
                {
                    spriteBatch.Draw(vTexture, screen, Color.White);
                }
            }
            else if (isScene3 == true)
            {
                if (vPlayer.State != MediaState.Stopped)
                    vTexture = vPlayer.GetTexture();

                if (vPlayer.State == MediaState.Stopped)
                {
                    isScene3 = false;
                }

                Rectangle screen = new Rectangle(0, 0, 800, 600);

                if (vTexture != null)
                {
                    spriteBatch.Draw(vTexture, screen, Color.White);
                }
            }
            else
            {
                spriteBatch.Draw(testBG, new Rectangle(0, 0, 800, 600), Color.White);

                if (isInteraction1 == true)
                    spriteBatch.Draw(testIMG, testImgPos, Color.White);
                else if (isInteraction2 == true)
                    spriteBatch.Draw(testIMG2, testImgPos2, Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
