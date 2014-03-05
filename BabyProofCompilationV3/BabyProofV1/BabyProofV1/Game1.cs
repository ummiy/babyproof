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
//using Microsoft.Xna.Framework.Video;
using Microsoft.Kinect;

namespace BabyProofV1
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KinectSensor kinectSensor;

        Vector3 handRPosition = new Vector3( 0.0f, 0.0f, 0.0f);
        Vector3 shoulderRPosition = new Vector3 (0.0f, 0.0f, 0.0f);
        Vector3 headPosition = new Vector3(0, 0, 0);
        Vector3 elbowPosition = new Vector3(0, 0, 0);
        Vector3 hipPosition = new Vector3(0, 0, 0);
        Vector3 footRPosition = new Vector3(0, 0, 0);

        Interaction1 pulling;
        Interaction2a putOut;
        Interaction2b takeOut;
        Interaction3 swingAt;
        Interaction4 throwAt;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
                        handRPosition = new Vector3((((0.5f * rightHand.Position.X) + 0.5f) * (graphics.GraphicsDevice.Viewport.Width)), (((-0.5f * rightHand.Position.Y) + 0.5f) * (graphics.GraphicsDevice.Viewport.Height)), (((-0.5f * rightHand.Position.Z) + 0.5f) * (-50)));

                        Joint shoulderR = playerSkeleton.Joints[JointType.ShoulderRight];
                        shoulderRPosition = new Vector3((((0.5f * shoulderR.Position.X) + 0.5f) * (graphics.GraphicsDevice.Viewport.Width)), (((-0.5f * shoulderR.Position.Y) + 0.5f) * (graphics.GraphicsDevice.Viewport.Height)), (((-0.5f * shoulderR.Position.Z) + 0.5f) * (-50)));

                        Joint head = playerSkeleton.Joints[JointType.Head];
                        headPosition = new Vector3((((0.5f * head.Position.X) + 0.5f) * (graphics.GraphicsDevice.Viewport.Width)), (((-0.5f * head.Position.Y) + 0.5f) * (graphics.GraphicsDevice.Viewport.Height)), (((-0.5f * head.Position.Z) + 0.5f) * (-50)));

                        Joint hip = playerSkeleton.Joints[JointType.HipCenter];
                        hipPosition = new Vector3((((0.5f * hip.Position.X) + 0.5f) * (graphics.GraphicsDevice.Viewport.Width)), (((-0.5f * hip.Position.Y) + 0.5f) * (graphics.GraphicsDevice.Viewport.Height)), (((-0.5f * hip.Position.Z) + 0.5f) * (-50)));

                        Joint elbow = playerSkeleton.Joints[JointType.ElbowRight];
                        elbowPosition = new Vector3((((0.5f * elbow.Position.X) + 0.5f) * (graphics.GraphicsDevice.Viewport.Width)), (((-0.5f * elbow.Position.Y) + 0.5f) * (graphics.GraphicsDevice.Viewport.Height)), (((-0.5f * elbow.Position.Z) + 0.5f) * (-50)));

                        Joint footR = playerSkeleton.Joints[JointType.FootRight];
                        footRPosition = new Vector3((((0.5f * footR.Position.X) + 0.5f) * (graphics.GraphicsDevice.Viewport.Width)), (((-0.5f * footR.Position.Y) + 0.5f) * (graphics.GraphicsDevice.Viewport.Height)), (((-0.5f * footR.Position.Z) + 0.5f) * (-50)));
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
            KinectSensor.KinectSensors.StatusChanged += new EventHandler<StatusChangedEventArgs>(KinectSensors_StatusChanged);
            DiscoverKinectSensor();

            pulling = new Interaction1();
            putOut = new Interaction2a();
            takeOut = new Interaction2b();
            swingAt = new Interaction3();
            throwAt = new Interaction4();

            base.Initialize();
        }

       protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            pulling.Load(this.Content);
            putOut.Load(this.Content, this.GraphicsDevice);
            takeOut.Load(this.Content, this.GraphicsDevice);
            swingAt.Load(this.Content, this.GraphicsDevice);
            throwAt.Load(this.Content, this.GraphicsDevice);
        }

       protected override void UnloadContent()
       {
           kinectSensor.Stop();
           kinectSensor.Dispose();
       }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            pulling.Update(this.handRPosition, this.shoulderRPosition);
            //putOut.Update(gameTime, this.GraphicsDevice, this.footRPosition);
            //takeOut.Update(this.GraphicsDevice, gameTime, this.handRPosition);
            //swingAt.Update(this.GraphicsDevice, this.handRPosition, this.headPosition, this.elbowPosition, gameTime, this.shoulderRPosition);
            //throwAt.Update(gameTime, this.handRPosition, this.hipPosition, this.shoulderRPosition, this.headPosition, this.GraphicsDevice);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            pulling.Draw(this.spriteBatch, this.handRPosition);
            //putOut.Draw(this.spriteBatch, this.footRPosition);
            //takeOut.Draw(this.spriteBatch);
            //swingAt.Draw(this.spriteBatch, this.handRPosition);
            //throwAt.Draw(this.spriteBatch, this.handRPosition);

            base.Draw(gameTime);
        }
    }
}