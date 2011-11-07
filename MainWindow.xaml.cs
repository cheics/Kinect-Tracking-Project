/////////////////////////////////////////////////////////////////////////
//
// This module contains code to do Kinect NUI initialization,
// processing, displaying players on screen, and sending updated player
// positions to the game portion for hit testing.
//
// Copyright © Microsoft Corporation.  All rights reserved.  
// This code is licensed under the terms of the 
// Microsoft Kinect for Windows SDK (Beta) 
// License Agreement: http://kinectforwindows.org/KinectSDK-ToU
//
/////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Research.Kinect.Audio;
using Microsoft.Research.Kinect.Nui;
using ShapeGame.Speech;
using ShapeGame.Utils;


namespace ShapeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        #region Private State
        const int TimerResolution = 2;  // ms
        const int NumIntraFrames = 3;
        const int MaxShapes = 80;
        const double MaxFramerate = 70;
        const double MinFramerate = 15;
        const double MinShapeSize = 12;
        const double MaxShapeSize = 90;
        const double DefaultDropRate = 2.5;
        const double DefaultDropSize = 32.0;
        const double DefaultDropGravity = 1.0;

        Dictionary<int, Player> players = new Dictionary<int, Player>();

        double dropRate = DefaultDropRate;
        double dropSize = DefaultDropSize;
        double dropGravity = DefaultDropGravity;
        DateTime lastFrameDrawn = DateTime.MinValue;
        DateTime predNextFrame = DateTime.MinValue;
        double actualFrameTime = 0;

        // Player(s) placement in scene (z collapsed):
        Rect playerBounds;
        Rect screenRect;

        double targetFramerate = MaxFramerate;
        int frameCount = 0;
        bool runningGameThread = false;
        FallingThings fallingThings = null;
        int playersAlive = 0;
        SoundPlayer popSound = new SoundPlayer();
        SoundPlayer hitSound = new SoundPlayer();
        SoundPlayer squeezeSound = new SoundPlayer();

        RuntimeOptions runtimeOptions;
        SpeechRecognizer speechRecognizer = null;
        #endregion Private State

        #region ctor + Window Events
        public MainWindow()
        {
            InitializeComponent();
            RestoreWindowState();
        }

        private void RestoreWindowState()
        {
            // Restore window state to that last used
            Rect bounds = Properties.Settings.Default.PrevWinPosition;
            if (bounds.Right != bounds.Left)
            {
                this.Top = bounds.Top;
                this.Left = bounds.Left;
                this.Height = bounds.Height;
                this.Width = bounds.Width;
            }
            this.WindowState = (WindowState)Properties.Settings.Default.WindowState;
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            playfield.ClipToBounds = true;

            fallingThings = new FallingThings(MaxShapes, targetFramerate, NumIntraFrames);

            UpdatePlayfieldSize();

            fallingThings.SetGravity(dropGravity);
            fallingThings.SetDropRate(dropRate);
            fallingThings.SetSize(dropSize);
            fallingThings.SetPolies(PolyType.All);
            fallingThings.SetGameMode(FallingThings.GameMode.Off);

            KinectStart();

            popSound.Stream = Properties.Resources.Pop_5;
            hitSound.Stream = Properties.Resources.Hit_2;
            squeezeSound.Stream = Properties.Resources.Squeeze;

            popSound.Play();

            Win32Timer.timeBeginPeriod(TimerResolution);
            var gameThread = new Thread(GameThread);
            gameThread.SetApartmentState(ApartmentState.STA);
            gameThread.Start();

            FlyingText.NewFlyingText(screenRect.Width / 30, new Point(screenRect.Width / 2, screenRect.Height / 2), "Shapes!");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            runningGameThread = false;
            Properties.Settings.Default.PrevWinPosition = this.RestoreBounds;
            Properties.Settings.Default.WindowState = (int)this.WindowState;
            Properties.Settings.Default.Save();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            KinectStop();
        }
        #endregion ctor + Window Events

        #region Kinect discovery + setup

        /* To add Kinect support, an app can:
         *   copy the entire "Kinect discovery + setup" region into your app's code file.
         *   call KinectStart() from Window loaded.
         *   call KinectStop() from Window closed.
         *   modify ShowStatus method to display app specific feedback to the user for each of the conditions.
         *   modify Initialize/UnitializeKinectServices for your app.
         */

        //Kinect enabled apps should customize each message and replace the technique of displaying the message.
        private void ShowStatus(ErrorCondition errorCondition)
        {
            string statusMessage;
            switch (errorCondition)
            {
                case ErrorCondition.None:
                    statusMessage = null;
                    break;
                case ErrorCondition.NoKinect:
                    statusMessage = Properties.Resources.NoKinectError;
                    break;
                case ErrorCondition.NoPower:
                    statusMessage = Properties.Resources.NoPowerError;
                    break;
                case ErrorCondition.NoSpeech:
                    statusMessage = Properties.Resources.NoSpeechError;
                    break;
                case ErrorCondition.NotReady:
                    statusMessage = Properties.Resources.NotReady;
                    break;
                case ErrorCondition.KinectAppConflict:
                    statusMessage = Properties.Resources.KinectAppConflict;
                    break;
                default:
                    throw new NotImplementedException("ErrorCondition." + errorCondition.ToString() + " needs a handler in ShowStatus()");
            }
            BannerText.NewBanner(statusMessage, screenRect, false, Color.FromArgb(90, 255, 255, 255));
            currentErrorCondition = errorCondition;
        }

        //Kinect enabled apps should customize which Kinect services it initializes here.
        private void InitializeKinectServices(Runtime runtime)
        {
            runtimeOptions = RuntimeOptions.UseDepthAndPlayerIndex | RuntimeOptions.UseSkeletalTracking | RuntimeOptions.UseColor;

            //KinectSDK TODO: should be able to understand a Kinect used by another app without having to try/catch.
            try
            {
                Kinect.Initialize(runtimeOptions);
            }
            catch (COMException comException)
            {
                //TODO: make CONST
                if (comException.ErrorCode == -2147220947)  //Runtime is being used by another app.
                {
                    Kinect = null;
                    ShowStatus(ErrorCondition.KinectAppConflict);
                    return;
                }
                else
                {
                    throw comException;
                }
            }

            kinectViewer.RuntimeOptions = runtimeOptions;
            kinectViewer.Kinect = Kinect;

            Kinect.SkeletonEngine.TransformSmooth = true;
            Kinect.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(SkeletonsReady);

            speechRecognizer = SpeechRecognizer.Create();         //returns null if problem with speech prereqs or instantiation.
            if (speechRecognizer != null)
            {
                speechRecognizer.Start(new KinectAudioSource());  //KinectSDK TODO: expose Runtime.AudioSource to return correct audiosource.
                speechRecognizer.SaidSomething += new EventHandler<SpeechRecognizer.SaidSomethingEventArgs>(recognizer_SaidSomething);
            }
            else
            {
                ShowStatus(ErrorCondition.NoSpeech);
                speechRecognizer = null;
            }
        }

        //Kinect enabled apps should uninitialize all Kinect services that were initialized in InitializeKinectServices() here.
        private void UninitializeKinectServices(Runtime runtime)
        {
            Kinect.Uninitialize();

            kinectViewer.Kinect = null;

            Kinect.SkeletonFrameReady -= new EventHandler<SkeletonFrameReadyEventArgs>(SkeletonsReady);

            if (speechRecognizer != null)
            {
                speechRecognizer.Stop();
                speechRecognizer.SaidSomething -= new EventHandler<SpeechRecognizer.SaidSomethingEventArgs>(recognizer_SaidSomething);
                speechRecognizer = null;
            }
        }

        #region Most apps won't modify this code
        private void KinectStart()
        {
            KinectDiscovery();
            if (Kinect == null)
            {
                if (Runtime.Kinects.Count == 0)
                {
                    ShowStatus(ErrorCondition.NoKinect);
                }
                else
                {
                    if (Runtime.Kinects[0].Status == KinectStatus.NotPowered)
                    {
                        ShowStatus(ErrorCondition.NoPower);
                    }
                }
            }
        }

        private void KinectStop()
        {
            if (Kinect != null)
            {
                Kinect = null;
            }
        }

        private bool IsKinectStarted
        {
            get { return Kinect != null; }
        }

        private void KinectDiscovery()
        {
            //listen to any status change for Kinects
            Runtime.Kinects.StatusChanged += new EventHandler<StatusChangedEventArgs>(Kinects_StatusChanged);

            //loop through all the Kinects attached to this PC, and start the first that is connected without an error.
            foreach (Runtime kinect in Runtime.Kinects)
            {
                if (kinect.Status == KinectStatus.Connected)
                {
                    if (Kinect == null)
                    {
                        Kinect = kinect;
                        return;
                    }
                }
            }
        }

        private void Kinects_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case KinectStatus.Connected:
                    if (Kinect == null)
                    {
                        Kinect = e.KinectRuntime; //if Runtime.Init() fails due to an AppDeviceConflict, this property will be null after return.
                        ShowStatus(ErrorCondition.None);
                    }
                    break;
                case KinectStatus.Disconnected:
                    if (Kinect == e.KinectRuntime)
                    {
                        Kinect = null;
                    }
                    break;
                case KinectStatus.NotReady:
                    if (Kinect == null)
                    {
                        ShowStatus(ErrorCondition.NotReady);
                    }
                    break;
                case KinectStatus.NotPowered:
                    if (Kinect == e.KinectRuntime)
                    {
                        Kinect = null;
                        ShowStatus(ErrorCondition.NoPower);
                    }
                    break;
                default:
                    throw new Exception("Unhandled Status: " + e.Status);
            }
            if (Kinect == null)
            {
                ShowStatus(ErrorCondition.NoKinect);
            }
        }

        public Runtime Kinect
        {
            get
            {
                return _Kinect;
            }
            set
            {
                if (_Kinect != null)
                {
                    UninitializeKinectServices(_Kinect);
                }
                _Kinect = value;
                if (_Kinect != null)
                {
                    InitializeKinectServices(_Kinect);
                }
            }
        }

        private Runtime _Kinect;
        private ErrorCondition currentErrorCondition;

        internal enum ErrorCondition
        {
            None,
            NoPower,
            NoKinect,
            NoSpeech,
            NotReady,
            KinectAppConflict,
        }
        #endregion Most apps won't modify this code

        #endregion Kinect discovery + setup

        
        static double Angle(double a, double b, double c)
        {
            double aSquare = Math.Pow(a, 2);
            double bSquare = Math.Pow(b, 2);
            double cSquare = Math.Pow(c, 2);
            double angle = Math.Acos((cSquare - aSquare - bSquare) / (-2 * a * b));
            return angle;
        }

        #region Kinect Skeleton processing
        void SkeletonsReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonFrame skeletonFrame = e.SkeletonFrame;

            //KinectSDK TODO: This nullcheck shouldn't be required. 
            //Unfortunately, this version of the Kinect Runtime will continue to fire some skeletonFrameReady events after the Kinect USB is unplugged.
            if (skeletonFrame == null)
            {
                return;
            }

            int iSkeletonSlot = 0;

            foreach (SkeletonData data in skeletonFrame.Skeletons)
            {
                if (SkeletonTrackingState.Tracked == data.TrackingState)
                {
                    Player player;
                    if (players.ContainsKey(iSkeletonSlot))
                    {
                        player = players[iSkeletonSlot];
                    }
                    else
                    {
                        player = new Player(iSkeletonSlot);
                        player.setBounds(playerBounds);
                        players.Add(iSkeletonSlot, player);
                    }

                    player.lastUpdated = DateTime.Now;

                    // Update player's bone and joint positions
                    if (data.Joints.Count > 0)
                    {
                        player.isAlive = true;


                        
                        Microsoft.Research.Kinect.Nui.Vector a = data.Joints[JointID.Spine].Position;
                        Microsoft.Research.Kinect.Nui.Vector b =data.Joints[JointID.HipCenter].Position;
                        Microsoft.Research.Kinect.Nui.Vector c = data.Joints[JointID.HipRight].Position;

                        double sideC = Math.Sqrt(Math.Pow((a.X - b.X), 2) + Math.Pow((a.Y - b.Y), 2));
                        double sideA = Math.Sqrt(Math.Pow((c.X - b.X), 2) + Math.Pow((c.Y - b.Y), 2));
                        double sideB = Math.Sqrt(Math.Pow((c.X - a.X), 2) + Math.Pow((c.Y - a.Y), 2));

                        double radAngle = Angle(sideC, sideA, sideB);
                        double degAngle = 57.2957795 * radAngle;
                        
                        Console.WriteLine("Sides");
                        Console.WriteLine(sideA);
                        Console.WriteLine(sideB);
                        Console.WriteLine(sideC);
                        Console.WriteLine("Hip Angle?");
                        Console.WriteLine(degAngle);

                        // Head, hands, feet (hit testing happens in order here)
                        player.UpdateJointPosition(data.Joints, JointID.Head);
                        player.UpdateJointPosition(data.Joints, JointID.HandLeft);
                        player.UpdateJointPosition(data.Joints, JointID.HandRight);
                        player.UpdateJointPosition(data.Joints, JointID.FootLeft);
                        player.UpdateJointPosition(data.Joints, JointID.FootRight);

                        // Hands and arms
                        player.UpdateBonePosition(data.Joints, JointID.HandRight, JointID.WristRight);
                        player.UpdateBonePosition(data.Joints, JointID.WristRight, JointID.ElbowRight);
                        player.UpdateBonePosition(data.Joints, JointID.ElbowRight, JointID.ShoulderRight);

                        player.UpdateBonePosition(data.Joints, JointID.HandLeft, JointID.WristLeft);
                        player.UpdateBonePosition(data.Joints, JointID.WristLeft, JointID.ElbowLeft);
                        player.UpdateBonePosition(data.Joints, JointID.ElbowLeft, JointID.ShoulderLeft);

                        // Head and Shoulders
                        player.UpdateBonePosition(data.Joints, JointID.ShoulderCenter, JointID.Head);
                        player.UpdateBonePosition(data.Joints, JointID.ShoulderLeft, JointID.ShoulderCenter);
                        player.UpdateBonePosition(data.Joints, JointID.ShoulderCenter, JointID.ShoulderRight);

                        // Legs
                        player.UpdateBonePosition(data.Joints, JointID.HipLeft, JointID.KneeLeft);
                        player.UpdateBonePosition(data.Joints, JointID.KneeLeft, JointID.AnkleLeft);
                        player.UpdateBonePosition(data.Joints, JointID.AnkleLeft, JointID.FootLeft);

                        player.UpdateBonePosition(data.Joints, JointID.HipRight, JointID.KneeRight);
                        player.UpdateBonePosition(data.Joints, JointID.KneeRight, JointID.AnkleRight);
                        player.UpdateBonePosition(data.Joints, JointID.AnkleRight, JointID.FootRight);

                        player.UpdateBonePosition(data.Joints, JointID.HipLeft, JointID.HipCenter);
                        player.UpdateBonePosition(data.Joints, JointID.HipCenter, JointID.HipRight);

                        // Spine
                        player.UpdateBonePosition(data.Joints, JointID.HipCenter, JointID.ShoulderCenter);
                    }
                }
                iSkeletonSlot++;
            }
        }

        void CheckPlayers()
        {
            foreach (var player in players)
            {
                if (!player.Value.isAlive)
                {
                    // Player left scene since we aren't tracking it anymore, so remove from dictionary
                    players.Remove(player.Value.getId());
                    break;
                }
            }

            // Count alive players
            int alive = 0;
            foreach (var player in players)
            {
                if (player.Value.isAlive)
                    alive++;
            }
            if (alive != playersAlive)
            {
                if (alive == 2)
                    fallingThings.SetGameMode(FallingThings.GameMode.TwoPlayer);
                else if (alive == 1)
                    fallingThings.SetGameMode(FallingThings.GameMode.Solo);
                else if (alive == 0)
                    fallingThings.SetGameMode(FallingThings.GameMode.Off);

                if ((playersAlive == 0) && (speechRecognizer != null))
                    BannerText.NewBanner(Properties.Resources.Vocabulary, screenRect, true, Color.FromArgb(200, 255, 255, 255));

                playersAlive = alive;
            }
        }

        private void Playfield_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdatePlayfieldSize();
        }

        private void UpdatePlayfieldSize()
        {
            // Size of player wrt size of playfield, putting ourselves low on the screen.
            screenRect.X = 0;
            screenRect.Y = 0;
            screenRect.Width = playfield.ActualWidth;
            screenRect.Height = playfield.ActualHeight;

            BannerText.UpdateBounds(screenRect);

            playerBounds.X = 0;
            playerBounds.Width = playfield.ActualWidth;
            playerBounds.Y = playfield.ActualHeight * 0.2;
            playerBounds.Height = playfield.ActualHeight * 0.75;

            foreach (var player in players)
                player.Value.setBounds(playerBounds);

            Rect rFallingBounds = playerBounds;
            rFallingBounds.Y = 0;
            rFallingBounds.Height = playfield.ActualHeight;
            if (fallingThings != null)
            {
                fallingThings.SetBoundaries(rFallingBounds);
            }
        }
        #endregion Kinect Skeleton processing

        #region GameTimer/Thread
        private void GameThread()
        {
            runningGameThread = true;
            predNextFrame = DateTime.Now;
            actualFrameTime = 1000.0 / targetFramerate;

            // Try to dispatch at as constant of a framerate as possible by sleeping just enough since
            // the last time we dispatched.
            while (runningGameThread)
            {
                // Calculate average framerate.  
                DateTime now = DateTime.Now;
                if (lastFrameDrawn == DateTime.MinValue)
                    lastFrameDrawn = now;
                double ms = now.Subtract(lastFrameDrawn).TotalMilliseconds;
                actualFrameTime = actualFrameTime * 0.95 + 0.05 * ms;
                lastFrameDrawn = now;

                // Adjust target framerate down if we're not achieving that rate
                frameCount++;
                if (((frameCount % 100) == 0) && (1000.0 / actualFrameTime < targetFramerate * 0.92))
                    targetFramerate = Math.Max(MinFramerate, (targetFramerate + 1000.0 / actualFrameTime) / 2);

                if (now > predNextFrame)
                    predNextFrame = now;
                else
                {
                    double msSleep = predNextFrame.Subtract(now).TotalMilliseconds;
                    if (msSleep >= TimerResolution)
                        Thread.Sleep((int)(msSleep + 0.5));
                }
                predNextFrame += TimeSpan.FromMilliseconds(1000.0 / targetFramerate);

                Dispatcher.Invoke(DispatcherPriority.Send,
                    new Action<int>(HandleGameTimer), 0);
            }
        }

        private void HandleGameTimer(int param)
        {
            // Every so often, notify what our actual framerate is
            if ((frameCount % 100) == 0)
                fallingThings.SetFramerate(1000.0 / actualFrameTime);

            // Advance animations, and do hit testing.
            for (int i = 0; i < NumIntraFrames; ++i)
            {
                foreach (var pair in players)
                {
                    HitType hit = fallingThings.LookForHits(pair.Value.segments, pair.Value.getId());
                    if ((hit & HitType.Squeezed) != 0)
                        squeezeSound.Play();
                    else if ((hit & HitType.Popped) != 0)
                        popSound.Play();
                    else if ((hit & HitType.Hand) != 0)
                        hitSound.Play();
                }
                fallingThings.AdvanceFrame();
            }

            // Draw new Wpf scene by adding all objects to canvas
            playfield.Children.Clear();
            fallingThings.DrawFrame(playfield.Children);
            foreach (var player in players)
                player.Value.Draw(playfield.Children);
            BannerText.Draw(playfield.Children);
            FlyingText.Draw(playfield.Children);

            CheckPlayers();
        }
        #endregion GameTimer/Thread

        #region Kinect Speech processing
        void recognizer_SaidSomething(object sender, SpeechRecognizer.SaidSomethingEventArgs e)
        {
            FlyingText.NewFlyingText(screenRect.Width / 30, new Point(screenRect.Width / 2, screenRect.Height / 2), e.Matched);
            switch (e.Verb)
            {
                case SpeechRecognizer.Verbs.Pause:
                    fallingThings.SetDropRate(0);
                    fallingThings.SetGravity(0);
                    break;
                case SpeechRecognizer.Verbs.Resume:
                    fallingThings.SetDropRate(dropRate);
                    fallingThings.SetGravity(dropGravity);
                    break;
                case SpeechRecognizer.Verbs.Reset:
                    dropRate = DefaultDropRate;
                    dropSize = DefaultDropSize;
                    dropGravity = DefaultDropGravity;
                    fallingThings.SetPolies(PolyType.All);
                    fallingThings.SetDropRate(dropRate);
                    fallingThings.SetGravity(dropGravity);
                    fallingThings.SetSize(dropSize);
                    fallingThings.SetShapesColor(Color.FromRgb(0, 0, 0), true);
                    fallingThings.Reset();
                    break;
                case SpeechRecognizer.Verbs.DoShapes:
                    fallingThings.SetPolies(e.Shape);
                    break;
                case SpeechRecognizer.Verbs.RandomColors:
                    fallingThings.SetShapesColor(Color.FromRgb(0, 0, 0), true);
                    break;
                case SpeechRecognizer.Verbs.Colorize:
                    fallingThings.SetShapesColor(e.RGBColor, false);
                    break;
                case SpeechRecognizer.Verbs.ShapesAndColors:
                    fallingThings.SetPolies(e.Shape);
                    fallingThings.SetShapesColor(e.RGBColor, false);
                    break;
                case SpeechRecognizer.Verbs.More:
                    dropRate *= 1.5;
                    fallingThings.SetDropRate(dropRate);
                    break;
                case SpeechRecognizer.Verbs.Fewer:
                    dropRate /= 1.5;
                    fallingThings.SetDropRate(dropRate);
                    break;
                case SpeechRecognizer.Verbs.Bigger:
                    dropSize *= 1.5;
                    if (dropSize > MaxShapeSize)
                        dropSize = MaxShapeSize;
                    fallingThings.SetSize(dropSize);
                    break;
                case SpeechRecognizer.Verbs.Biggest:
                    dropSize = MaxShapeSize;
                    fallingThings.SetSize(dropSize);
                    break;
                case SpeechRecognizer.Verbs.Smaller:
                    dropSize /= 1.5;
                    if (dropSize < MinShapeSize)
                        dropSize = MinShapeSize;
                    fallingThings.SetSize(dropSize);
                    break;
                case SpeechRecognizer.Verbs.Smallest:
                    dropSize = MinShapeSize;
                    fallingThings.SetSize(dropSize);
                    break;
                case SpeechRecognizer.Verbs.Faster:
                    dropGravity *= 1.25;
                    if (dropGravity > 4.0)
                        dropGravity = 4.0;
                    fallingThings.SetGravity(dropGravity);
                    break;
                case SpeechRecognizer.Verbs.Slower:
                    dropGravity /= 1.25;
                    if (dropGravity < 0.25)
                        dropGravity = 0.25;
                    fallingThings.SetGravity(dropGravity);
                    break;
            }
        }
        #endregion Kinect Speech processing
    }
}

// Since the timer resolution defaults to about 10ms precisely, we need to
// increase the resolution to get framerates above between 50fps with any
// consistency.
public class Win32Timer
{
    [DllImport("Winmm.dll")]
    public static extern int timeBeginPeriod(UInt32 uPeriod);
}