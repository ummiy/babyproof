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

namespace BabyProofV1
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KinectSensor kinectSensor;

        Vector3 handRPosition;
        Vector3 shoulderRPosition;

        Interaction1 pulling;

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

            base.Initialize();
        }

       protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            pulling.Load(this.Content);
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            pulling.Draw(this.spriteBatch, this.handRPosition);

            base.Draw(gameTime);
        }
    }
}
