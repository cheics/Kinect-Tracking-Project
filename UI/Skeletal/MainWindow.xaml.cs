/////////////////////////////////////////////////////////////////////////
//
// This module contains code to do Kinect NUI initialization and
// processing and also to display NUI streams on screen.
//
// Copyright © Microsoft Corporation.  All rights reserved.  
// This code is licensed under the terms of the 
// Microsoft Kinect for Windows SDK (Beta) 
// License Agreement: http://kinectforwindows.org/KinectSDK-ToU
//
/////////////////////////////////////////////////////////////////////////
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Research.Kinect.Nui;
using Microsoft.Samples.Kinect.WpfViewers;
using System.Windows.Media;
using System.Data;
using System.IO;
using System.Media;
using csmatio.io;
using csmatio.types;
using FolderPickerLib;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Visiblox.Charts;
using System.Windows.Controls;
//using LumenWorks.Framework.IO.Csv;
//using MathWorks.MATLAB.NET.Utility;
//using MathWorks.MATLAB.NET.Arrays;
//using dotnet;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace SkeletalViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region ctor & Window events
        string cc1legend;
        string cc2legend;
        string cc3legend;
        string criticalComponent1Feedback;
        string criticalComponent2Feedback;
        string criticalComponent3Feedback;
        string cc1Audio;
        string cc2Audio;
        string cc3Audio;
        MongoDatabase exerHist;
        public MainWindow()
        {
            InitializeComponent();
            string[] exercises = new string[4] {"Squat", "Leg Raise", "Arm Raise", "Hip Abduction"};
            exercisesCB.ItemsSource = exercises;
            int[] reps = new int[1] { 5 };
            repsCB.ItemsSource = reps;
            repsCB.SelectedIndex = 0;

            //// graph
            ////We need one data series for each chart series
            //DataSeries<int, double> cc1 = new DataSeries<int, double>("cc1");
            //DataSeries<int, double> cc2 = new DataSeries<int, double>("cc2");
            //DataSeries<int, double> cc3 = new DataSeries<int, double>("cc3");

            //String MyDocs = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //String ProjectLocation = "Visual Studio 2010\\Projects\\Kinect-Tracking-Project\\MatlabPrototypes\\FeatureDetection";

            //StreamReader FileStreamReader;

            //FileStreamReader = File.OpenText(System.IO.Path.Combine(MyDocs, ProjectLocation) + "\\results.csv");

            //int i = 1;

            //while (FileStreamReader.Peek() != -1)
            //{
            //    string[] words;
            //    words = FileStreamReader.ReadLine().Split(',');

            //    double test = double.Parse(words[0]);

            //    if (test == -1)
            //    {
            //        continue;
            //    }

            //    double cc1v = test;
            //    double cc2v = double.Parse(words[1]);
            //    double cc3v = double.Parse(words[2]);

            //    if (cc1v == 0) 
            //    {
            //        cc1v = 0.05;
            //    }
            //    if (cc2v == 0) 
            //    {
            //        cc2v = 0.05;
            //    }
            //    if (cc3v == 0) 
            //    {
            //        cc3v = 0.05;
            //    }

            //    cc1.Add(new DataPoint<int, double>() {X = i, Y = cc1v});
            //    cc2.Add(new DataPoint<int, double>() { X = i, Y = cc2v});
            //    cc3.Add(new DataPoint<int, double>() { X = i, Y = cc3v});

            //    i = i + 1;
            //}
            //FileStreamReader.Close();

            ////Finally, associate the data series with the chart series
            //userchart.Series[0].DataSeries = cc1;
            //userchart.Series[1].DataSeries = cc2;
            //userchart.Series[2].DataSeries = cc3;

            
            MongoServer server = MongoServer.Create();
            exerHist = server.GetDatabase("exerHist");

            //MLApp.MLAppClass matlab = new MLApp.MLAppClass();

            //String MyDocs = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //String ProjectLocation = "Visual Studio 2010\\Projects\\Kinect-Tracking-Project\\MatlabPrototypes\\FeatureDetection";
            //String matFileCD_command = String.Format("cd '{0}';", System.IO.Path.Combine(MyDocs, ProjectLocation));

            //matlab.Execute(matFileCD_command);
            //matlab.Execute("initWorkspace();");
        }



        private void squatCriticalComponents()
        {
            cc1legend = "Depth";
            cc2legend = "Straight back";
            cc3legend = "Balance";
            criticalComponent1Feedback = cc1legend + ": Work on your full range of motion by squatting till parallel to the ground";
            criticalComponent2Feedback = cc2legend + ": Keep your back straight throughout the exercise by driving your hip";
            criticalComponent3Feedback = cc3legend + ": Keep your heels on the floor by pushing on them";
            cc1Audio = "squat_cc1.wav";
            cc2Audio = "squat_cc2.wav";
            cc3Audio = "squat_cc3.wav";
        }

        private void legExtensionCriticalComponents()
        {
            cc1legend = "Hip level";
            cc2legend = "Hip angle";
            cc3legend = "Spine stability";
            criticalComponent1Feedback = cc1legend + ": Keep your hip level throughout the exercise";
            criticalComponent2Feedback = cc2legend + ": Work on your full range of motion by contracting the glute muscle";
            criticalComponent3Feedback = cc3legend + ": Keep your back up right throughout the exercise";
            cc1Audio = "legExt_cc1.wav";
            cc2Audio = "legExt_cc2.wav";
            cc3Audio = "legExt_cc3.wav";
        }

        private void armRaiseCriticalComponents()
        {
            cc1legend = "Straight arm";
            cc2legend = "Hands in front";
            cc3legend = "Front shoulder angle";
            criticalComponent1Feedback = cc1legend + ": Keep your elbows locked through out the exercise";
            criticalComponent2Feedback = cc2legend + ": Keep your arms/hands parallel and facing each other through out the exercise";
            criticalComponent3Feedback = cc3legend + ": Increase your range of motion by raising your hands above your head";
            cc1Audio = "armRaise_cc1.wav";
            cc2Audio = "armRaise_cc2.wav";
            cc3Audio = "armRaise_cc3.wav";
        }

        private void legLiftCriticalComponents()
        {
            cc1legend = "Knee angle";
            cc2legend = "Spine stability";
            cc3legend = "Knee out";
            criticalComponent1Feedback = cc1legend + ": Lift your leg until your thigh is parallel to the floor";
            criticalComponent2Feedback = cc2legend + ": Keep your back straight throughout the exercise";
            criticalComponent3Feedback = cc3legend + ": Keep your knees in front of your body";
            cc1Audio = "kneeRaise_cc1.wav";
            cc2Audio = "kneeRaise_cc2.wav";
            cc3Audio = "kneeRaise_cc3.wav";
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            if (minKinectCount > 0)
            {
                //kinectRequiredOrEnabled.Text = "Requires Kinect";
            }
            else
            {
                //kinectRequiredOrEnabled.Text = "Kinect Enabled";
            }

            //Watch for Kinects connecting, disconnecting - and gracefully handle them.
            Runtime.Kinects.StatusChanged += new EventHandler<StatusChangedEventArgs>(Kinects_StatusChanged);

            //create a KinectViewer for each Kinect that is found.
            CreateAllKinectViewers();

            /*DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            string[] array = {"HipCenterX", "HipCenterY", "HipCenterZ", "SpineX", "SpineY", "SpineZ", "ShoulderCenterX", 
                                 "ShoulderCenterY", "ShoulderCenterZ", "HeadX", "HeadY", "HeadZ", "ShoulderLeftX", 
                             "ShoulderLeftY", "ShoulderLeftZ", "ElbowLeftX", "ElbowLeftY", "ElbowLeftZ", "WristLeftX", 
                             "WristLeftY", "WristLeftZ", "HandLeftX", "HandLeftY", "HandLeftZ", "ShoulderRightX", 
                             "ShoulderRightY", "ShoulderRightZ", "ElbowRightX", "ElbowRightY", "ElbowRightZ", 
                             "WristRightX", "WristRightY", "WristRightZ", "HandRightX", "HandRightY", "HandRightZ", 
                             "HipLeftX", "HipLeftY", "HipLeftZ", "KneeLeftX", "KneeLeftY", "KneeLeftZ", "AnkleLeftX", 
                             "AnkleLeftY", "AnkleLeftZ", "FootLeftX", "FootLeftY", "FootLeftZ", "HipRightX", "HipRightY", 
                             "HipRightZ", "KneeRightX", "KneeRightY", "KneeRightZ", "AnkleRightX", "AnkleRightY", 
                             "AnkleRightZ", "FootRightX", "FootRightY", "FootRightZ"};

            for (int i = 0; i < 60; i++)
            {
                DataColumn column1 = new DataColumn(array[i], System.Type.GetType("System.Double"));
                DataColumn column2 = new DataColumn(array[i], System.Type.GetType("System.Double"));
                dt1.Columns.Add(column1);
                dt2.Columns.Add(column2);
            }*/
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CleanUpAllKinectViewers();
        }
        #endregion ctor & Window events

        #region Kinect discovery + setup
        private void Kinects_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case KinectStatus.Connected:
                    var foundViewer = FindViewer(e.KinectRuntime);
                    if (foundViewer != null)
                    {
                        foundViewer.Kinect = e.KinectRuntime; //will cause a uninit, init
                    }
                    else if (viewerHolder.Items.Count < maxKinectCount)
                    {
                        AddKinectViewer(e.KinectRuntime);
                    }
                    break;
                case KinectStatus.Disconnected:
                    if (Runtime.Kinects.Count >= maxKinectCount)
                    {
                        UpdateRuntimeOfKinectViewerToNextKinect(e.KinectRuntime);
                    }
                    else
                    {
                        RemoveKinectViewer(e.KinectRuntime);
                    }
                    break;
                default:
                    if (e.Status.HasFlag(KinectStatus.Error))
                    {
                        DisableOrAddKinectViewer(e.KinectRuntime);
                    }
                    break;
            }
            UpdateUIBasedOnKinectCount();
        }

        public bool SkeletalEngineAvailable
        {
            get
            {
                return null == SkeletalDiagnosticViewer;
            }
        }
        #endregion Kinect discovery + setup

        private void UpdateUIBasedOnKinectCount()
        {
            //Update the visibility of the status messages based on min/maxKinectCount and the number of Kinects
            //that are connected to the system.
            switch (Runtime.Kinects.Count)
            {
                case 0:
                    //insertKinectSensor.Visibility = System.Windows.Visibility.Visible;
                    //insertAnotherKinectSensor.Visibility = System.Windows.Visibility.Collapsed;
                    //switchToAnotherKinectSensor.Visibility = System.Windows.Visibility.Collapsed;
                    button1.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                default:
                    //insertKinectSensor.Visibility = System.Windows.Visibility.Collapsed;
                    button1.Visibility = System.Windows.Visibility.Visible;
                    if (viewerHolder.Items.Count < maxKinectCount)
                    {
                        //insertAnotherKinectSensor.Visibility = System.Windows.Visibility.Visible;
                        //switchToAnotherKinectSensor.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        //insertAnotherKinectSensor.Visibility = System.Windows.Visibility.Collapsed;
                        //switchToAnotherKinectSensor.Visibility = (Runtime.Kinects.Count > maxKinectCount) ?
                            //System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                    }
                    break;
            }
            foreach (UIElement element in viewerHolder.Items)
            {
                var kinectViewer = element as KinectDiagnosticViewer;
                if (kinectViewer != null)
                {
                    kinectViewer.UpdateUi();
                }
            }
        }

        #region KinectViewer Utilities
        private void AddKinectViewer(Runtime runtime)
        {
            var kinectViewer = new KinectDiagnosticViewer();
            //kinectViewer.kinectDepthViewer.MouseLeftButtonDown += new MouseButtonEventHandler(kinectDepthViewer_MouseLeftButtonDown);
            kinectViewer.Kinect= runtime;
            viewerHolder.Items.Add(kinectViewer);
        }

        KinectDiagnosticViewer GetViewer(object sender)
        {
            while (sender != null && sender is FrameworkElement)
            {
                sender = ((FrameworkElement) sender).Parent;
                if (sender is KinectDiagnosticViewer)
                {
                    return sender as KinectDiagnosticViewer;
                }
            }
            return null;
        }

        void kinectDepthViewer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var currentSkeletalViewer = SkeletalDiagnosticViewer;
            var thisViewer = GetViewer(sender);
            if (currentSkeletalViewer != null && currentSkeletalViewer != thisViewer)
            {
                // Take the skeletal engine from the other viewer!
                var otherKinect = currentSkeletalViewer.Kinect;
                var thisKinect = thisViewer.Kinect;
                currentSkeletalViewer.Kinect = null;        // Force the other runtime to give up the skeletal engine.
                thisViewer.Kinect = null;                   // Uninit as we are going to re-init with the skeletal engine.
                thisViewer.Kinect = thisKinect;             // This runtime should get me the skeletal engine.
                currentSkeletalViewer.Kinect = otherKinect; // Other runtime will not get the skeletal engine.
            }
            else if (SkeletalEngineAvailable)
            {
                // No one (including thisKinect) has the Skeletal Engine.  Take it!
                thisViewer.ReInitRuntime();    // Will Uninit/Reinit
            }
        }

        internal KinectDiagnosticViewer SkeletalDiagnosticViewer
        {
            get
            {
                //find the DiagViewer which has a Runtime who is doing skeletal tracking.
                return (from v in viewerHolder.Items.OfType<KinectDiagnosticViewer>()
                        where v.Kinect != null && v.Kinect.SkeletonEngine != null
                        select v).FirstOrDefault();
            }
        }

        private void RemoveKinectViewer(Runtime runtime)
        {
            var foundViewer = FindViewer(runtime);

            if (foundViewer != null)
            {
                foundViewer.Kinect = null;
                viewerHolder.Items.Remove(foundViewer);
            }
        }

        private void DisableOrAddKinectViewer(Runtime runtime)
        {
            var foundViewer = FindViewer(runtime);

            if (foundViewer != null)
            {
                runtime.Uninitialize();
            }
            else
            {
                AddKinectViewer(runtime);
            }
        }

        private void CreateAllKinectViewers()
        {
            foreach (Runtime runtime in Runtime.Kinects)
            {
                if (viewerHolder.Items.Count == maxKinectCount)
                {
                    break; //end this loop, as we want to limit the count to maxKinectCount
                }
                AddKinectViewer(runtime);
            }
            UpdateUIBasedOnKinectCount();
        }

        private void CleanUpAllKinectViewers()
        {
            foreach (object item in viewerHolder.Items)
            {
                KinectDiagnosticViewer kinectViewer = item as KinectDiagnosticViewer;
                kinectViewer.Kinect = null;
            }
            viewerHolder.Items.Clear();
        }

        private KinectDiagnosticViewer FindViewer(Runtime runtime)
        {
            // Return the Viewer associated with the runtime.
            return (from v in viewerHolder.Items.OfType<KinectDiagnosticViewer>() where Object.ReferenceEquals(v.Kinect, runtime) select v).FirstOrDefault();
        }

        private void UpdateRuntimeOfKinectViewerToNextKinect(Runtime previousRuntime)
        {
            KinectDiagnosticViewer kinectViewer = viewerHolder.Items[0] as KinectDiagnosticViewer;
            bool foundRuntime = false;
            foreach (Runtime runtime in Runtime.Kinects)
            {
                if (foundRuntime)
                {
                    kinectViewer.Kinect = runtime;
                    return;
                }
                if (runtime == kinectViewer.Kinect)
                {
                    foundRuntime = true;
                }
            }
            //must have been the last Runtime in the collection, or wasn't found, so we should switch to the first
            if (Runtime.Kinects.Count > 0)
            {
                kinectViewer.Kinect = Runtime.Kinects[0];
            }
        }
        #endregion KinectViewer Utilities

        #region event handlers
        private void switchSensors(object sender, RoutedEventArgs e)
        {
            KinectDiagnosticViewer kinectViewer = viewerHolder.Items[0] as KinectDiagnosticViewer;

            UpdateRuntimeOfKinectViewerToNextKinect(kinectViewer.Kinect);
        }

        private void showMoreInfo(object sender, RoutedEventArgs e)
        {
            Hyperlink hyperlink = e.OriginalSource as Hyperlink;
            //Careful - ensure that this NavigateUri comes from a trusted source, as in this sample, before launching a process using it.
            Process.Start(new ProcessStartInfo(hyperlink.NavigateUri.ToString()));
            e.Handled = true;
        }

        //if you mousedown with a control key pressed down, it will try to begin execution again...as if it was a fresh startup.
        //likely not useful to include in the tool, once we fix bugs.
        private void mouseDown(object sender, MouseEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                CleanUpAllKinectViewers();
                CreateAllKinectViewers();
            }
        }
        #endregion event handlers

        #region Private state
        private int minKinectCount = 1;       //0 - app is "Kinect Enabled". 1 - app "Requires Kinect".
        const int maxKinectCount = 2;

        //public void CreateCSVFile(DataTable dt, string strFilePath)
        //{


        //    #region Export Grid to CSV





        //    // Create the CSV file to which grid data will be exported.


        //    StreamWriter sw = new StreamWriter(strFilePath, false);


        //    // First we will write the headers.


        //    //DataTable dt = m_dsProducts.Tables[0];


        //    int iColCount = dt.Columns.Count;


        //    for (int i = 0; i < iColCount; i++)
        //    {


        //        sw.Write(dt.Columns[i]);


        //        if (i < iColCount - 1)
        //        {


        //            sw.Write(",");


        //        }


        //    }


        //    sw.Write(sw.NewLine);


        //    // Now write all the rows.


        //    foreach (DataRow dr in dt.Rows)
        //    {


        //        for (int i = 0; i < iColCount; i++)
        //        {


        //            if (!Convert.IsDBNull(dr[i]))
        //            {


        //                sw.Write(dr[i].ToString());


        //            }


        //            if (i < iColCount - 1)
        //            {


        //                sw.Write(",");


        //            }


        //        }


        //        sw.Write(sw.NewLine);


        //    }


        //    sw.Close();





        //    #endregion


        //}

        private MLArray createStruct()
        {
            MLStructure outStruct = new MLStructure("kinectData", new int[] { 1, 1 });
            //string[] headers = {"HipCenterX", "HipCenterY", "HipCenterZ", "SpineX", "SpineY", "SpineZ", "ShoulderCenterX", 
            //                     "ShoulderCenterY", "ShoulderCenterZ", "HeadX", "HeadY", "HeadZ", "ShoulderLeftX", 
            //                 "ShoulderLeftY", "ShoulderLeftZ", "ElbowLeftX", "ElbowLeftY", "ElbowLeftZ", "WristLeftX", 
            //                 "WristLeftY", "WristLeftZ", "HandLeftX", "HandLeftY", "HandLeftZ", "ShoulderRightX", 
            //                 "ShoulderRightY", "ShoulderRightZ", "ElbowRightX", "ElbowRightY", "ElbowRightZ", 
            //                 "WristRightX", "WristRightY", "WristRightZ", "HandRightX", "HandRightY", "HandRightZ", 
            //                 "HipLeftX", "HipLeftY", "HipLeftZ", "KneeLeftX", "KneeLeftY", "KneeLeftZ", "AnkleLeftX", 
            //                 "AnkleLeftY", "AnkleLeftZ", "FootLeftX", "FootLeftY", "FootLeftZ", "HipRightX", "HipRightY", 
            //                 "HipRightZ", "KneeRightX", "KneeRightY", "KneeRightZ", "AnkleRightX", "AnkleRightY", 
            //                 "AnkleRightZ", "FootRightX", "FootRightY", "FootRightZ"};

            double criticalC1D = Convert.ToDouble(criticalC1);
            double criticalC2D = Convert.ToDouble(criticalC2);
            double criticalC3D = Convert.ToDouble(criticalC3);
            double criticalC11D = Convert.ToDouble(criticalC11);
            double criticalC21D = Convert.ToDouble(criticalC21);
            double criticalC31D = Convert.ToDouble(criticalC31);
            double criticalC12D = Convert.ToDouble(criticalC12);
            double criticalC22D = Convert.ToDouble(criticalC22);
            double criticalC32D = Convert.ToDouble(criticalC32);
            double criticalC13D = Convert.ToDouble(criticalC13);
            double criticalC23D = Convert.ToDouble(criticalC23);
            double criticalC33D = Convert.ToDouble(criticalC33);
            double criticalC14D = Convert.ToDouble(criticalC14);
            double criticalC24D = Convert.ToDouble(criticalC24);
            double criticalC34D = Convert.ToDouble(criticalC34);

            double[] ccArray = { criticalC1D, criticalC2D, criticalC3D, criticalC11D, criticalC21D, criticalC31D, criticalC12D, criticalC22D, criticalC32D,
                                   criticalC13D, criticalC23D, criticalC33D, criticalC14D, criticalC24D, criticalC34D};

            Double[] skelDataA = (Double[])skelData.ToArray(typeof(double));
            Double[] gpVectorA = (Double[])gpVector.ToArray(typeof(double));
            Double[] kinectTiltA = (Double[])kinectTilt.ToArray(typeof(double));

            outStruct["skelData", 0] = new MLDouble("", skelDataA, 60);
            outStruct["dateHeader", 0] = new MLChar("", timeStamp);
            outStruct["criticalComps", 0] = new MLDouble("", ccArray, 3);
            outStruct["exercise", 0] = new MLChar("", exercise);

            MLStructure groundStruct = new MLStructure("", new int[] { 1, 1 });
            groundStruct["height", 0] = new MLDouble("", new double[] { 0.68 }, 1); // metres?
            groundStruct["gpVector", 0] = new MLDouble("", gpVectorA, 4); //metres?
            groundStruct["kinectTilt", 0] = new MLDouble("", kinectTiltA, 1); //degrees
            outStruct["groundPlaneData", 0] = groundStruct;

            return outStruct;
        }

        private void createMatFile()
        {
            List<MLArray> mlList = new List<MLArray>();
            mlList.Add(createStruct());

            try
            {
                MatFileWriter mfw = new MatFileWriter(folder + @"\" + exercise +criticalC1 + criticalC2 + criticalC3 + "-" + index + ".mat", mlList, true);
                //StringBuilder sb = new StringBuilder();
                //sb.Appe
                StreamWriter w = File.AppendText(folder + @"\" + exercise + ".txt");
                w.WriteLine(criticalC1 + " " + criticalC2 + " " + criticalC3);
                w.WriteLine(criticalC11 + " " + criticalC21 + " " + criticalC31);
                w.WriteLine(criticalC12 + " " + criticalC22 + " " + criticalC32);
                w.WriteLine(criticalC13 + " " + criticalC23 + " " + criticalC33);
                w.WriteLine(criticalC14 + " " + criticalC24 + " " + criticalC34);
                w.Flush();
            }
            catch (Exception err)
            {
                Console.WriteLine("shit...");
            }
        }

        private void startDemo()
        {
            // please begin exercise
            startAudio.PlaySync();
            return;
        }

        //private void startTraining()
        //{
        //    // wait to capture data
        //    capture = false;

        //    // Instantiate the dialog box
        //    ExerciseNameDialogBox dlg = new ExerciseNameDialogBox();

        //    // Configure the dialog box
        //    dlg.Owner = this;

        //    // Pass the folder location
        //    dlg.passFolder(folder, exercise, criticalC1, criticalC2, criticalC3);

        //    // Open the dialog box modally 
        //    dlg.ShowDialog();

        //    // Process data entered by user if dialog box is accepted
        //    if (dlg.DialogResult == true)
        //    {
        //        // begin capturing
        //        capture = true;
        //        exercise = dlg.file;
        //        criticalC1 = dlg.criticalC1;
        //        criticalC2 = dlg.criticalC2;
        //        criticalC3 = dlg.criticalC3;
        //        criticalC11 = dlg.criticalC11;
        //        criticalC21 = dlg.criticalC21;
        //        criticalC31 = dlg.criticalC31;
        //        criticalC12 = dlg.criticalC12;
        //        criticalC22 = dlg.criticalC22;
        //        criticalC32 = dlg.criticalC32;
        //        criticalC13 = dlg.criticalC13;
        //        criticalC23 = dlg.criticalC23;
        //        criticalC33 = dlg.criticalC33;
        //        criticalC14 = dlg.criticalC14;
        //        criticalC24 = dlg.criticalC24;
        //        criticalC34 = dlg.criticalC34;
        //        index = dlg.index;
        //    }
        //    return;
        //}

        private void endTraining()
        {
            // Instantiate the dialog box
            ExerciseNameDialogBox dlg = new ExerciseNameDialogBox();

            // Configure the dialog box
            dlg.Owner = this;

            // Pass the folder location
            dlg.passFolder(folder, exercise, criticalC1, criticalC2, criticalC3);

            // Open the dialog box modally 
            dlg.ShowDialog();

            // Process data entered by user if dialog box is accepted
            if (dlg.DialogResult == true)
            {
                exercise = dlg.file;
                criticalC1 = dlg.criticalC1;
                criticalC2 = dlg.criticalC2;
                criticalC3 = dlg.criticalC3;
                criticalC11 = dlg.criticalC11;
                criticalC21 = dlg.criticalC21;
                criticalC31 = dlg.criticalC31;
                criticalC12 = dlg.criticalC12;
                criticalC22 = dlg.criticalC22;
                criticalC32 = dlg.criticalC32;
                criticalC13 = dlg.criticalC13;
                criticalC23 = dlg.criticalC23;
                criticalC33 = dlg.criticalC33;
                criticalC14 = dlg.criticalC14;
                criticalC24 = dlg.criticalC24;
                criticalC34 = dlg.criticalC34;
                index = dlg.index;
            }
            return;
        }

        private void endDemo()
        {
            // thank you
            endAudio.PlaySync();
            return;
        }

        static private string[] scores(DataTable kinectTable, double[] groundPlane, string exercise)
        {
            double[,] kinectData = new double[kinectTable.Rows.Count, kinectTable.Columns.Count];
            double[,] kinectZeros = new double[kinectTable.Rows.Count, kinectTable.Columns.Count];
            double[] groundPlaneZeros = new double[4];

            //System.Array cr = new double[3];
            //System.Array ci = new double[3];

            for (int r = 0; r < kinectTable.Rows.Count; r++)
            {
                for (int c = 0; c < kinectTable.Columns.Count; c++)
                {
                    kinectData[r, c] = (double)kinectTable.Rows[r][c];
                }
            }

            MLApp.MLAppClass matlab = new MLApp.MLAppClass();
            matlab.PutFullMatrix("CS_kinectData", "base", kinectData, kinectZeros);
            matlab.PutFullMatrix("CS_groundPlane", "base", groundPlane, groundPlaneZeros);



            String MyDocs = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            String ProjectLocation = "Visual Studio 2010\\Projects\\Kinect-Tracking-Project\\MatlabPrototypes\\FeatureDetection";
            String matFileCD_command = String.Format("cd '{0}';", System.IO.Path.Combine(MyDocs, ProjectLocation));

            matlab.Execute(matFileCD_command);
            matlab.Execute("initWorkspace();");
            if (exercise == "Squat")
            {
                matlab.Execute("c = cs_matlab_classifier('squats', CS_kinectData, CS_groundPlane);");
            }
            else if (exercise == "Arm Raise")
            {
                matlab.Execute("c = cs_matlab_classifier('armRaise', CS_kinectData, CS_groundPlane);");
            }
            else if (exercise == "Leg Raise")
            {
                matlab.Execute("c = cs_matlab_classifier('legRaise', CS_kinectData, CS_groundPlane);");
            }
            else if (exercise == "Hip Abduction")
            {
                matlab.Execute("c = cs_matlab_classifier('legExt', CS_kinectData, CS_groundPlane);");
            }

            string[] cr_d = new string[30];

            StreamReader FileStreamReader;

            FileStreamReader = File.OpenText(System.IO.Path.Combine(MyDocs, ProjectLocation) + "\\results.csv");

            int i = 0;

            while (FileStreamReader.Peek() != -1)
            {
                string[] words;
                words = FileStreamReader.ReadLine().Split(',');

                cr_d[i] = words[0];
                cr_d[i + 1] = words[1];
                cr_d[i + 2] = words[2];

                i = i + 3;
            }
            FileStreamReader.Close();

            //matlab.GetFullMatrix("c", "base", cr, ci);

            //double[] cr_d = new double[3];
            //cr_d = (double[])cr;
            return cr_d;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (button1.Content.ToString() == "Start")
            {
                sample.IsEnabled = false;
                tb.IsEnabled = false;
                exercisesCB.IsEnabled = false;
                //if (!path)
                //{
                //    var dlg = new FolderPickerDialog();
                //    if (dlg.ShowDialog() == true)
                //    {
                //        folder = dlg.SelectedPath;
                //        path = true;
                //    }
                //}
                //if (path & training)
                //{
                //    //startTraining();
                //}
                //else if (path & !training)
                //{
                //startDemo();
                //}
                //if (path & capture)
                //{
                    button1.Background = Brushes.Red;
                    button1.Content = "End";
                    index1 = KinectDiagnosticViewer.dt1.Rows.Count;
                    timeStamp = DateTime.Now.ToString();
                //}
            }
            else
            {
                index2 = KinectDiagnosticViewer.dt1.Rows.Count;
                DataTable dt1 = KinectDiagnosticViewer.dt1.Copy();
                DataTable dt2 = KinectDiagnosticViewer.dt1.Clone();
                DataTable dt1gp = KinectDiagnosticViewer.dt1gp.Copy();

                double[] ground = new double[4];

                ground[0] = (double)dt1gp.Rows[index1 + 1][0];
                ground[1] = (double)dt1gp.Rows[index1 + 1][1];
                ground[2] = (double)dt1gp.Rows[index1 + 1][2];
                ground[3] = (double)dt1gp.Rows[index1 + 1][3];

                string test = dt1gp.Rows[0][0].ToString();

                for (int i = (index1 + 1); i < index2-1; i++)
                {
                    //for (int j = 0; j < 60; j++)
                    //{
                    //    skelData.Add(dt1.Rows[i][j]);
                    //}
                    dt2.ImportRow(dt1.Rows[i]);
                    //kinectTilt.Add(dt1.Rows[i][60]);
                    //gpVector.Add(dt1.Rows[i][61]);
                    //gpVector.Add(dt1.Rows[i][62]);
                    //gpVector.Add(dt1.Rows[i][63]);
                    //gpVector.Add(dt1.Rows[i][64]);
                }
                

                //if (!training)
                //{
                    //endDemo();
                //}
                //else
                //{
                //    endTraining();
                //}

                //createMatFile();

                button1.Background = Brushes.Green;
                button1.Content = "Start";

                skelData.Clear();
                gpVector.Clear();
                kinectTilt.Clear();

                string[] output = scores(dt2, ground, exercisesCB.SelectedValue.ToString());

                DateTime now = DateTime.UtcNow.ToLocalTime();

                chartCurrent(output, now);



                //feedback.Content = "Feedback: 1. {" + output[0].ToString() + ", " + output[1].ToString() + ", " + output[2].ToString() + "}, 2. {" + output[3].ToString() + ", " + output[4].ToString() + ", " + output[5].ToString() + "}, 3. {" + output[6].ToString() + ", " + output[7].ToString() + ", " + output[8].ToString() + "}, 4. {" + output[9].ToString() + ", " + output[10].ToString() + ", " + output[11].ToString() + "}, 5. {" + output[12].ToString() + ", " + output[13].ToString() + ", " + output[14].ToString() + "}";

                sample.IsEnabled = true;
                tb.IsEnabled = true;
                exercisesCB.IsEnabled = true;
            }
            return;
        } //Change to 1 if you only want to view one at a time. Switching will be enabled.
                                      //Each Kinect needs to be in its own USB hub, otherwise it won't have enough USB bandwidth.
                                      //Currently only 1 Kinect per process can have SkeletalTracking working, but color and depth work for all.
                                      //KinectSDK TODO: enable a larger maxKinectCount (assuming your PC can dedicate a USB hub for each Kinect)
        #endregion Private state

        private int index1;
        private int index2;
        private string timeStamp;
        //private bool training = true;
        //private bool capture = true;
        //private bool path = false;
        private string folder = "";
        private string exercise = "";
        private string index = "";
        private string criticalC1 = "0";
        private string criticalC2 = "0";
        private string criticalC3 = "0";
        private string criticalC11 = "0";
        private string criticalC21 = "0";
        private string criticalC31 = "0";
        private string criticalC12 = "0";
        private string criticalC22 = "0";
        private string criticalC32 = "0";
        private string criticalC13 = "0";
        private string criticalC23 = "0";
        private string criticalC33 = "0";
        private string criticalC14 = "0";
        private string criticalC24 = "0";
        private string criticalC34 = "0";
        private SoundPlayer startAudio = new SoundPlayer(@"C:\Users\Abdi\Documents\Visual Studio 2010\Projects\Kinect-Tracking-Project\DataFiles\Demo\hi.wav");
        private SoundPlayer endAudio = new SoundPlayer(@"C:\Users\Abdi\Documents\Visual Studio 2010\Projects\Kinect-Tracking-Project\DataFiles\Demo\hi.wav");
        ArrayList skelData = new ArrayList();
        ArrayList gpVector = new ArrayList();
        ArrayList kinectTilt = new ArrayList();
        private string path = "";

        private void sample_Click(object sender, RoutedEventArgs e)
        {
            VideoPlayer vd = new VideoPlayer();
            vd.passVideo(path);
            vd.Show();
            //vd.ShowDialog();
        }

        private void exercisesCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sample.IsEnabled = true;
            button1.IsEnabled = true;
            String MyDocs = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            String ProjectLocation = "Visual Studio 2010\\Projects\\Kinect-Tracking-Project\\Videos";
            path = System.IO.Path.Combine(MyDocs, ProjectLocation) + "\\"+ exercisesCB.SelectedValue.ToString().ToLower().Trim() + ".mp4";
            switch (exercisesCB.SelectedValue.ToString())
            {
                case "Squat":
                    squatCriticalComponents();
                    break;
                case "Arm Raise":
                    armRaiseCriticalComponents();
                    break;
                case "Hip Abduction":
                    legExtensionCriticalComponents();
                    break;
                case "Leg Raise":
                    legLiftCriticalComponents();
                    break;
            }

            // clear feedback
            clearFeedback();

            chartMostRecent();
        }

        private void clearFeedback()
        {
            criticalComponent1.Text = "";
            criticalComponent2.Text = "";
            criticalComponent3.Text = "";
            buttoncc1.IsEnabled = false;
            buttoncc2.IsEnabled = false;
            buttoncc3.IsEnabled = false;
        }

        private void chartCurrent(string[] scores, DateTime dateTime)
        {
            MongoCollection<BsonDocument> history = exerHist.GetCollection<BsonDocument>("history");

            // graph
            //We need one data series for each chart series
            DataSeries<int, double> cc1 = new DataSeries<int, double>(cc1legend);
            DataSeries<int, double> cc2 = new DataSeries<int, double>(cc2legend);
            DataSeries<int, double> cc3 = new DataSeries<int, double>(cc3legend);

            bool perfect1 = true;
            bool perfect2 = true;
            bool perfect3 = true;

            int j = 0;

            for(int i = 1; i <=10; i++)
            {
                double cc1v = double.Parse(scores[(i*3)-3]);
                double cc2v = double.Parse(scores[(i*3)-2]);
                double cc3v = double.Parse(scores[(i*3)-1]);

                if (cc1v == -1 | cc1v == -2)
                {
                    j = i;
                    break;
                }

                if (cc1v == 0)
                {
                    cc1v = 0.05;
                }
                if (cc2v == 0)
                {
                    cc2v = 0.05;
                }
                if (cc3v == 0)
                {
                    cc3v = 0.05;
                }

                BsonDocument rep = new BsonDocument {
                { "name", user.Content.ToString() },
                { "exercise", exercisesCB.SelectedValue.ToString() },
                { "cc1", cc1v },
                { "cc2", cc2v },
                { "cc3", cc3v },
                { "time", dateTime.AddSeconds(i)},
                {"timeid", dateTime}
                };
                var options = new MongoInsertOptions(history) { SafeMode = SafeMode.True };
                history.Save(rep, options);

                if (perfect1 & (cc1v == 0.05 | cc1v == 1))
                {
                    perfect1 = false;
                }
                if (perfect2 & (cc2v == 0.05 | cc2v == 1))
                {
                    perfect2 = false;
                }
                if (perfect3 & (cc3v == 0.05 | cc3v == 1))
                {
                    perfect3 = false;
                }

                cc1.Add(new DataPoint<int, double>() { X = i, Y = cc1v });
                cc2.Add(new DataPoint<int, double>() { X = i, Y = cc2v });
                cc3.Add(new DataPoint<int, double>() { X = i, Y = cc3v });
            }

            if (j == 1)
            {
                // plot nothing
                for (int k = 1; k <= 5; k++)
                {
                    cc1.Add(new DataPoint<int, double>() { X = k, Y = 0 });
                    cc2.Add(new DataPoint<int, double>() { X = k, Y = 0 });
                    cc3.Add(new DataPoint<int, double>() { X = k, Y = 0 });
                }
                criticalComponent1.Text = "";
                criticalComponent1.Text = "";
                criticalComponent1.Text = "";
                date.Content = "N/A";
            }
            else
            {
                if (perfect1)
                {
                    criticalComponent1.Text = cc1legend + ": Excellent!";
                    buttoncc1.IsEnabled = false;
                }
                else
                {
                    criticalComponent1.Text = criticalComponent1Feedback;
                    buttoncc1.IsEnabled = true;
                }
                if (perfect2)
                {
                    criticalComponent2.Text = cc2legend + ": Excellent!";
                    buttoncc2.IsEnabled = false;
                }
                else
                {
                    criticalComponent2.Text = criticalComponent2Feedback;
                    buttoncc2.IsEnabled = true;
                }
                if (perfect3)
                {
                    criticalComponent3.Text = cc3legend + ": Excellent!";
                    buttoncc3.IsEnabled = false;
                }
                else
                {
                    criticalComponent3.Text = criticalComponent3Feedback;
                    buttoncc3.IsEnabled = true;
                }
                date.Content = dateTime.ToLongDateString() + " " + dateTime.ToShortTimeString(); ;
            }

            //Finally, associate the data series with the chart series
            userchart.Series[0].DataSeries = cc1;
            userchart.Series[1].DataSeries = cc2;
            userchart.Series[2].DataSeries = cc3;
        }

        private void chartMostRecent()
        {
            // graph
            //We need one data series for each chart series
            DataSeries<int, double> cc1 = new DataSeries<int, double>(cc1legend);
            DataSeries<int, double> cc2 = new DataSeries<int, double>(cc2legend);
            DataSeries<int, double> cc3 = new DataSeries<int, double>(cc3legend);

            IMongoSortBy sort = SortBy.Descending("time");

            MongoCollection<BsonDocument> history = exerHist.GetCollection<BsonDocument>("history");
            var query = Query.And(Query.EQ("exercise", exercisesCB.SelectedValue.ToString()), Query.EQ("name", user.Content.ToString()));

            int i = 1;
            bool perfect1 = true;
            bool perfect2 = true;
            bool perfect3 = true;

            bool pass = false;

            DateTime dateSearch = new DateTime();

            foreach (BsonDocument rep in history.Find(query).SetSortOrder(sort))
            {
                dateSearch = (DateTime)rep["timeid"];
                pass = true;
                break;
            }

            if (pass)
            {
                var query1 = Query.And(Query.EQ("exercise", exercisesCB.SelectedValue.ToString()), Query.EQ("name", user.Content.ToString()), Query.EQ("timeid", dateSearch));

                foreach (BsonDocument rep in history.Find(query1).SetSortOrder(sort))
                {

                    double cc1v = (double)rep["cc1"];
                    double cc2v = (double)rep["cc2"];
                    double cc3v = (double)rep["cc3"];

                    if (cc1v == 0)
                    {
                        cc1v = 0.05;
                    }
                    if (cc2v == 0)
                    {
                        cc2v = 0.05;
                    }
                    if (cc3v == 0)
                    {
                        cc3v = 0.05;
                    }

                    if (perfect1 & (cc1v == 0.05 | cc1v == 1))
                    {
                        perfect1 = false;
                    }
                    if (perfect2 & (cc2v == 0.05 | cc2v == 1))
                    {
                        perfect2 = false;
                    }
                    if (perfect3 & (cc3v == 0.05 | cc3v == 1))
                    {
                        perfect3 = false;
                    }

                    cc1.Add(new DataPoint<int, double>() { X = i, Y = cc1v });
                    cc2.Add(new DataPoint<int, double>() { X = i, Y = cc2v });
                    cc3.Add(new DataPoint<int, double>() { X = i, Y = cc3v });

                    i++;
                }
            }

            if (i == 1)
            {
                //// plot nothing
                //for (int j = 1; j <= 5; j++)
                //{
                //    cc1.Add(new DataPoint<int, double>() { X = j, Y = 0 });
                //    cc2.Add(new DataPoint<int, double>() { X = j, Y = 0 });
                //    cc3.Add(new DataPoint<int, double>() { X = j, Y = 0 });
                //}
                criticalComponent1.Text = "";
                criticalComponent1.Text = "";
                criticalComponent1.Text = "";
                date.Content = "N/A";
            }
            else
            {
                if (perfect1)
                {
                    criticalComponent1.Text = cc1legend + ": Excellent!";
                    buttoncc1.IsEnabled = false;
                }
                else
                {
                    criticalComponent1.Text = criticalComponent1Feedback;
                    buttoncc1.IsEnabled = true;
                }
                if (perfect2)
                {
                    criticalComponent2.Text = cc2legend + ": Excellent!";
                    buttoncc2.IsEnabled = false;
                }
                else
                {
                    criticalComponent2.Text = criticalComponent2Feedback;
                    buttoncc2.IsEnabled = true;
                }
                if (perfect3)
                {
                    criticalComponent3.Text = cc3legend + ": Excellent!";
                    buttoncc3.IsEnabled = false;
                }
                else
                {
                    criticalComponent3.Text = criticalComponent3Feedback;
                    buttoncc3.IsEnabled = true;
                }
                date.Content = dateSearch.AddHours(-4).ToLongDateString() + " " + dateSearch.AddHours(-4).ToShortTimeString();
            }

            //Finally, associate the data series with the chart series
            userchart.Series[0].DataSeries = cc1;
            userchart.Series[1].DataSeries = cc2;
            userchart.Series[2].DataSeries = cc3;

        }

        private void clearChart()
        {
            // graph
            //We need one data series for each chart series
            DataSeries<int, double> cc1 = new DataSeries<int, double>(cc1legend);
            DataSeries<int, double> cc2 = new DataSeries<int, double>(cc2legend);
            DataSeries<int, double> cc3 = new DataSeries<int, double>(cc3legend);

            for (int i = 1; i <= 5; i++)
            {
                cc1.Add(new DataPoint<int, double>() { X = i, Y = 0 });
                cc2.Add(new DataPoint<int, double>() { X = i, Y = 0 });
                cc3.Add(new DataPoint<int, double>() { X = i, Y = 0 });
            }

            //Finally, associate the data series with the chart series
            userchart.Series[0].DataSeries = cc1;
            userchart.Series[1].DataSeries = cc2;
            userchart.Series[2].DataSeries = cc3;

            criticalComponent1.Text = "";
            criticalComponent1.Text = "";
            criticalComponent1.Text = "";
        }

        private void reconstructGraph()
        {
            // graph
            //We need one data series for each chart series
            DataSeries<int, double> cc1 = new DataSeries<int, double>(cc1legend);
            DataSeries<int, double> cc2 = new DataSeries<int, double>(cc2legend);
            DataSeries<int, double> cc3 = new DataSeries<int, double>(cc3legend);

            String MyDocs = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            String ProjectLocation = "Visual Studio 2010\\Projects\\Kinect-Tracking-Project\\MatlabPrototypes\\FeatureDetection";

            StreamReader FileStreamReader;

            FileStreamReader = File.OpenText(System.IO.Path.Combine(MyDocs, ProjectLocation) + "\\results.csv");

            int i = 1;

            while (FileStreamReader.Peek() != -1)
            {
                string[] words;
                words = FileStreamReader.ReadLine().Split(',');

                double test = double.Parse(words[0]);

                if (test == -1)
                {
                    continue;
                }

                double cc1v = test;
                double cc2v = double.Parse(words[1]);
                double cc3v = double.Parse(words[2]);

                if (cc1v == 0)
                {
                    cc1v = 0.05;
                }
                if (cc2v == 0)
                {
                    cc2v = 0.05;
                }
                if (cc3v == 0)
                {
                    cc3v = 0.05;
                }

                cc1.Add(new DataPoint<int, double>() { X = i, Y = cc1v });
                cc2.Add(new DataPoint<int, double>() { X = i, Y = cc2v });
                cc3.Add(new DataPoint<int, double>() { X = i, Y = cc3v });

                i = i + 1;
            }
            FileStreamReader.Close();

            //Finally, associate the data series with the chart series
            userchart.Series[0].DataSeries = cc1;
            userchart.Series[1].DataSeries = cc2;
            userchart.Series[2].DataSeries = cc3;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Logon logon = new Logon();
            App.Current.MainWindow = logon;
            logon.Show();
            this.Close();
        }

        internal void passName(String name)
        {
            user.Content = name;
        }

        private void buttoncc1_Click(object sender, RoutedEventArgs e)
        {
            String MyDocs = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            String ProjectLocation = "Visual Studio 2010\\Projects\\Kinect-Tracking-Project\\Audio";
            string audioPath = System.IO.Path.Combine(MyDocs, ProjectLocation) + "\\" + cc1Audio;
            SoundPlayer audio = new SoundPlayer(audioPath);
            audio.PlaySync();
        }

        private void buttoncc2_Click(object sender, RoutedEventArgs e)
        {
            String MyDocs = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            String ProjectLocation = "Visual Studio 2010\\Projects\\Kinect-Tracking-Project\\Audio";
            string audioPath = System.IO.Path.Combine(MyDocs, ProjectLocation) + "\\" + cc2Audio;
            SoundPlayer audio = new SoundPlayer(audioPath);
            audio.PlaySync();
        }

        private void buttoncc3_Click(object sender, RoutedEventArgs e)
        {
            String MyDocs = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            String ProjectLocation = "Visual Studio 2010\\Projects\\Kinect-Tracking-Project\\Audio";
            string audioPath = System.IO.Path.Combine(MyDocs, ProjectLocation) + "\\" + cc3Audio;
            SoundPlayer audio = new SoundPlayer(audioPath);
            audio.PlaySync();
        }
    }
}
