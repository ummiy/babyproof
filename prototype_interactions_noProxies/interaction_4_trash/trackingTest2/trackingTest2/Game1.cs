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
using Microsoft.Kinect;

namespace trackingTest2
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState scottP;
        Texture2D credo;
        Vector2 credoPos;
        Random rand;
        int counter = 0;

        Texture2D towel;
        Texture2D garbage;
        Vector2 garbagePos;
        Vector2 towelPos;

        //Texture2D[] trash = new Texture2D[5];
        Texture2D trash;

        Boolean inBin;

        Boolean isTouchT;
        Boolean isTouchG;
        Boolean isPutOut;

        KinectSensor kinectSensor;

        Vector3 handPositionR = new Vector3(680, 380, 50);
        Vector3 handPositionL = new Vector3(600, 380, 50);
        Vector3 shoulderRPosition = new Vector3(0, 0, 0);
        Vector2 peePos = new Vector2(0, 0);

        Vector3 hipPosition = new Vector3(600, 420, 50);

        Vector2 headPosition = new Vector2(0, 0);
        Vector3 footRPosition = new Vector3(0, 0, 0);

        //Vector2[] trashPos = new Vector2[5];
        Vector2 trashPos = new Vector2(0, 0);
        Vector2 firePos;
        
        Texture2D cursorL;
        Texture2D cursorR;
        Texture2D hipImg;
        Texture2D headImg;
        Texture2D footR;
        Texture2D elbowImg;

        Vector3 elbowPos;

        float YRange;
        float ZRange;
        float newZ;

        Texture2D pee;
        Texture2D fire;
        Texture2D smoke;

        Texture2D ball;
        Vector2 finalBallPos;
        Boolean ballVisible;
        Vector2 initBallPos;
        Vector2 initBallVel;
        Vector2 ballAcc;
        float time;
        bool ballFlip = false;
        float prevMouseX;
        float curMouseX;
        float throwTime;
        bool startThrow = false;
        bool stopThrow = false;
        float throwVel;
        int balls = 3;

        Texture2D bat;
        Vector3 batPos;
        Texture2D head1;
        Texture2D head2;
        Vector2 head1Pos;
        Vector2 head2Pos;
        int headCount = 0;
        int head1Hit = 0;
        int head2Hit = 0;

        bool head1Visible = true;
        bool head2Visible = true;
        bool batVis = false;
        bool batActive = false;
        float handDistance;

        SpriteFont font;
        int footCount;
        float smokeAlpha;
        int range;
        int maxRange;
        int minRange;

        float armlength;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 840;
            graphics.PreferredBackBufferHeight = 480;
        }

        void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            if (this.kinectSensor == e.Sensor)
            {
                if (e.Status == KinectStatus.Disconnected ||
                    e.Status == KinectStatus.NotPowered)
                {
                    this.kinectSensor = null;
                    this.DiscoverKinectSensor();
                }
            }
        }

        private bool InitializeKinect()
        {
            // Color stream
            kinectSensor.ColorStream.Enable(ColorImageFormat.RgbResolution1280x960Fps12);
            kinectSensor.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(kinectSensor_ColorFrameReady);

            // Skeleton Stream
            kinectSensor.SkeletonStream.Enable(new TransformSmoothParameters()
            {
                Smoothing = 0.5f,
                Correction = 0.5f,
                Prediction = 0.5f,
                JitterRadius = 0.05f,
                MaxDeviationRadius = 0.04f
            });
            kinectSensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinectSensor_SkeletonFrameReady);

            try
            {
                kinectSensor.Start();
            }
            catch
            {
                return false;
            }
            return true;
        }

        void kinectSensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    Skeleton[] skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];

                    skeletonFrame.CopySkeletonDataTo(skeletonData);
                    Skeleton playerSkeleton = (from s in skeletonData where s.TrackingState == SkeletonTrackingState.Tracked select s).FirstOrDefault();
                    if (playerSkeleton != null)
                    {
                        Joint rightHand = playerSkeleton.Joints[JointType.HandRight];
                        handPositionR = new Vector3((((0.5f * rightHand.Position.X) + 0.5f) * (graphics.GraphicsDevice.Viewport.Width)), (((-0.5f * rightHand.Position.Y) + 0.5f) * (graphics.GraphicsDevice.Viewport.Height)), (((-0.5f * rightHand.Position.Z) + 0.5f) * (-50)));

                        Joint leftHand = playerSkeleton.Joints[JointType.HandLeft];
                        handPositionL = new Vector3((((0.5f * leftHand.Position.X) + 0.5f) * (graphics.GraphicsDevice.Viewport.Width)), (((-0.5f * leftHand.Position.Y) + 0.5f) * (graphics.GraphicsDevice.Viewport.Height)), (((-0.5f * leftHand.Position.Z) + 0.5f) * (-50)));

                        Joint hip = playerSkeleton.Joints[JointType.HipCenter];
                        hipPosition = new Vector3((((0.5f * hip.Position.X) + 0.5f) * (graphics.GraphicsDevice.Viewport.Width)), (((-0.5f * hip.Position.Y) + 0.5f) * (graphics.GraphicsDevice.Viewport.Height)), (((-0.5f * hip.Position.Z) + 0.5f) * (-50)));

                        Joint shoulderR = playerSkeleton.Joints[JointType.ShoulderRight];
                        shoulderRPosition = new Vector3((((0.5f * shoulderR.Position.X) + 0.5f) * (graphics.GraphicsDevice.Viewport.Width)), (((-0.5f * shoulderR.Position.Y) + 0.5f) * (graphics.GraphicsDevice.Viewport.Height)), (((-0.5f * shoulderR.Position.Z) + 0.5f) * (-50)));

                        Joint head = playerSkeleton.Joints[JointType.Head];
                        headPosition = new Vector2((((0.5f * head.Position.X) + 0.5f) * (graphics.GraphicsDevice.Viewport.Width)), (((-0.5f * head.Position.Y) + 0.5f) * (graphics.GraphicsDevice.Viewport.Height)));

                        Joint rightFoot = playerSkeleton.Joints[JointType.FootRight];
                        footRPosition = new Vector3((((0.5f * rightFoot.Position.X) + 0.5f) * (graphics.GraphicsDevice.Viewport.Width)), (((-0.5f * rightFoot.Position.Y) + 0.5f) * (graphics.GraphicsDevice.Viewport.Height)), (((-0.5f * rightFoot.Position.Z) + 0.5f) * (-50)));

                        Joint rightElbow = playerSkeleton.Joints[JointType.ElbowRight];
                        elbowPos = new Vector3((((0.5f * rightElbow.Position.X) + 0.5f) * (graphics.GraphicsDevice.Viewport.Width)), (((-0.5f * rightElbow.Position.Y) + 0.5f) * (graphics.GraphicsDevice.Viewport.Height)), (((-0.5f * rightElbow.Position.Z) + 0.5f) * (-50)));
                    
                    }
                }
            }
        }

        void kinectSensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorImageFrame = e.OpenColorImageFrame())
            {
                if (colorImageFrame != null)
                {

                    byte[] pixelsFromFrame = new byte[colorImageFrame.PixelDataLength];

                    colorImageFrame.CopyPixelDataTo(pixelsFromFrame);

                    Color[] color = new Color[colorImageFrame.Height * colorImageFrame.Width];

                    // Go through each pixel and set the bytes correctly
                    // Remember, each pixel got a Rad, Green and Blue
                    int index = 0;
                    for (int y = 0; y < colorImageFrame.Height; y++)
                    {
                        for (int x = 0; x < colorImageFrame.Width; x++, index += 4)
                        {
                            color[y * colorImageFrame.Width + x] = new Color(pixelsFromFrame[index + 2], pixelsFromFrame[index + 1], pixelsFromFrame[index + 0]);
                        }
                    }

                    // Set pixeldata from the ColorImageFrame to a Texture2D
                    //kinectRGBVideo.SetData(color);
                }
            }
        }

        private void DiscoverKinectSensor()
        {
            foreach (KinectSensor sensor in KinectSensor.KinectSensors)
            {
                if (sensor.Status == KinectStatus.Connected)
                {
                    kinectSensor = sensor;
                    break;
                }
            }

            if (this.kinectSensor == null)
            {
                return;
            }

            switch (kinectSensor.Status)
            {
                case KinectStatus.Connected:
                    {
                        break;
                    }
                case KinectStatus.Disconnected:
                    {
                        break;
                    }
                case KinectStatus.NotPowered:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            if (kinectSensor.Status == KinectStatus.Connected)
            {
                InitializeKinect();
            }
        }
                
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            KinectSensor.KinectSensors.StatusChanged += new EventHandler<StatusChangedEventArgs>(KinectSensors_StatusChanged);
            DiscoverKinectSensor();

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            rand = new Random();
            cursorL = Content.Load<Texture2D>("hand");
            cursorR = Content.Load<Texture2D>("hand");
            hipImg = Content.Load<Texture2D>("hip");
            headImg = Content.Load<Texture2D>("head");
            footR = Content.Load<Texture2D>("foot");
            fire = Content.Load<Texture2D>("fire");
            smoke = Content.Load<Texture2D>("smoke");
            bat = Content.Load<Texture2D>("bat");
            elbowImg = Content.Load<Texture2D>("hand");

            credo = Content.Load<Texture2D>("hand");
            pee = Content.Load<Texture2D>("pee");

            firePos = new Vector2((footRPosition.X + 10), (footRPosition.Y + 10));
            head1Pos = new Vector2(rand.Next(0, GraphicsDevice.Viewport.Width), rand.Next(0, GraphicsDevice.Viewport.Height / 2));
            head2Pos = new Vector2(rand.Next(0, GraphicsDevice.Viewport.Width), rand.Next(0, GraphicsDevice.Viewport.Height / 2));

            footCount = 0;
            inBin = false;
            isPutOut = false;

            //for (int i = 0; i<5; i++)
            //{
            //    trash[i] = Content.Load<Texture2D>("trash");
            //    trashPos[i] = new Vector2(rand.Next(0, GraphicsDevice.Viewport.Width), rand.Next(0, GraphicsDevice.Viewport.Height));
            //}

            head1 = Content.Load<Texture2D>("head");
            head2 = Content.Load<Texture2D>("head");

            ball = Content.Load<Texture2D>("ball");
            initBallPos = new Vector2(200, 400);
            initBallVel = new Vector2(50, -50);
            ballAcc = new Vector2(0, -9.8f);
            time = 0;
            finalBallPos = Vector2.Zero;
            ballVisible = false;
            prevMouseX = curMouseX = 0.0f;
            throwTime = 0.0f;
            throwVel = 0.0f;

            trash = Content.Load<Texture2D>("trash");
            trashPos = new Vector2(400,400);

            towel = Content.Load<Texture2D>("towel");
            garbage = Content.Load<Texture2D>("bin");
            towelPos = new Vector2(500, 100);
            garbagePos = new Vector2(800, 100);
            isTouchT = false;
            isTouchG = false;

            handDistance = 144; // (handPositionR.X - elbowPos.X);
           
            range = 12;
            maxRange = range / 2;
            minRange = range / 4;

            ZRange = 74.0f - 0.0f;
            YRange = GraphicsDevice.Viewport.Height - 0.0f;

            font = Content.Load<SpriteFont>("SpriteFont1");
        }

        protected override void UnloadContent()
        {
            kinectSensor.Stop();
            kinectSensor.Dispose();
        }

        void clearTrash()
        {
            if (trashPos.X >= garbagePos.X && trashPos.X <= garbagePos.X + garbage.Width
                && trashPos.Y >= garbagePos.Y && trashPos.Y <= garbagePos.Y + garbage.Height)
            {
                inBin = true;
            }

            if (inBin == false)
            {
                if (trashPos.X >= GraphicsDevice.Viewport.Width - (trash.Width))
                {
                    trashPos.X = GraphicsDevice.Viewport.Width - (trash.Width);
                }
                if (trashPos.X <= 0)
                {
                    trashPos.X = 0;
                }

                 if (trashPos.Y >= GraphicsDevice.Viewport.Height - trash.Height)
                 {
                     trashPos.Y = GraphicsDevice.Viewport.Height - (trash.Height);
                 }
                 if (trashPos.Y <= 0)
                 {
                     trashPos.Y = 0;
                 }

                 if (handPositionR.Y >= trashPos.Y && handPositionR.Y <= trashPos.Y + trash.Height)
                    {
                        if (handPositionR.X >= trashPos.X && handPositionR.X <= (trashPos.X + trash.Width / 2))
                        {
                            trashPos.X++;
                        }

                        if (handPositionR.X <= (trashPos.X + trash.Width) && handPositionR.X >= (trashPos.X + trash.Width / 2))
                        {
                            trashPos.X--;
                        }
                    }

                    if (handPositionR.X >= trashPos.X && handPositionR.X <= trashPos.X + trash.Width)
                    {
                        if (handPositionR.Y >= trashPos.Y && handPositionR.Y <= (trashPos.Y + trash.Height / 2))
                        {
                            trashPos.Y++;
                        }

                        if (handPositionR.Y <= (trashPos.Y + trash.Height) && handPositionR.Y >= (trashPos.Y + trash.Height / 2))
                        {
                            trashPos.Y--;
                        }
                    }
                
        
                //for (int i = 0; i < 5; i++)
                //{
                //    if (trashPos[i].X >= GraphicsDevice.Viewport.Width - (trash[i].Width))
                //    {
                //        trashPos[i].X = GraphicsDevice.Viewport.Width - (trash[i].Width);
                //    }
                //    if (trashPos[i].X <= 0)
                //    {
                //        trashPos[i].X = 0;
                //    }

                //    if (trashPos[i].Y >= GraphicsDevice.Viewport.Height - trash[i].Height)
                //    {
                //        trashPos[i].Y = GraphicsDevice.Viewport.Height - (trash[i].Height);
                //    }
                //    if (trashPos[i].Y <= 0)
                //    {
                //        trashPos[i].Y = 0;
                //    }


                //    if (handPositionR.Y >= trashPos[i].Y && handPositionR.Y <= trashPos[i].Y + trash[i].Height)
                //    {
                //        if (handPositionR.X >= trashPos[i].X && handPositionR.X <= (trashPos[i].X + trash[i].Width / 2))
                //        {
                //            trashPos[i].X++;
                //        }

                //        if (handPositionR.X <= (trashPos[i].X + trash[i].Width) && handPositionR.X >= (trashPos[i].X + trash[i].Width / 2))
                //        {
                //            trashPos[i].X--;
                //        }
                //    }

                //    if (handPositionR.X >= trashPos[i].X && handPositionR.X <= trashPos[i].X + trash[i].Width)
                //    {
                //        if (handPositionR.Y >= trashPos[i].Y && handPositionR.Y <= (trashPos[i].Y + trash[i].Height / 2))
                //        {
                //            trashPos[i].Y++;
                //        }

                //        if (handPositionR.Y <= (trashPos[i].Y + trash[i].Height) && handPositionR.Y >= (trashPos[i].Y + trash[i].Height / 2))
                //        {
                //            trashPos[i].Y--;
                //        }
                //    }
                //}
            }
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) 
                this.Exit();

            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(garbage, garbagePos, Color.White);
            spriteBatch.Draw(cursorR, new Vector2(handPositionR.X, handPositionR.Y), Color.White);
            

            //for (int i = 0; i < 5; i++)
            //{
                //spriteBatch.Draw(trash[i], trashPos[i], Color.White);
            //}

            spriteBatch.Draw(trash, trashPos, Color.White);



            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
