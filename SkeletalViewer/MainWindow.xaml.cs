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

namespace SkeletalViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region ctor & Window events
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            if (minKinectCount > 0)
            {
                kinectRequiredOrEnabled.Text = "Requires Kinect";
            }
            else
            {
                kinectRequiredOrEnabled.Text = "Kinect Enabled";
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
                    insertKinectSensor.Visibility = System.Windows.Visibility.Visible;
                    insertAnotherKinectSensor.Visibility = System.Windows.Visibility.Collapsed;
                    switchToAnotherKinectSensor.Visibility = System.Windows.Visibility.Collapsed;
                    button1.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                default:
                    insertKinectSensor.Visibility = System.Windows.Visibility.Collapsed;
                    button1.Visibility = System.Windows.Visibility.Visible;
                    if (viewerHolder.Items.Count < maxKinectCount)
                    {
                        insertAnotherKinectSensor.Visibility = System.Windows.Visibility.Visible;
                        switchToAnotherKinectSensor.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        insertAnotherKinectSensor.Visibility = System.Windows.Visibility.Collapsed;
                        switchToAnotherKinectSensor.Visibility = (Runtime.Kinects.Count > maxKinectCount) ?
                            System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
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
            kinectViewer.kinectDepthViewer.MouseLeftButtonDown += new MouseButtonEventHandler(kinectDepthViewer_MouseLeftButtonDown);
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

            double[] ccArray = { criticalC1D, criticalC2D, criticalC3D, criticalC1D, criticalC2D, criticalC3D, criticalC1D, criticalC2D, criticalC3D,
                                   criticalC1D, criticalC2D, criticalC3D, criticalC1D, criticalC2D, criticalC3D};

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

        private void startTraining()
        {
            // wait to capture data
            capture = false;

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
                // begin capturing
                capture = true;
                exercise = dlg.file;
                criticalC1 = dlg.criticalC1;
                criticalC2 = dlg.criticalC2;
                criticalC3 = dlg.criticalC3;
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (button1.Content.ToString() == "Start")
            {
                if (!path)
                {
                    var dlg = new FolderPickerDialog();
                    if (dlg.ShowDialog() == true)
                    {
                        folder = dlg.SelectedPath;
                        path = true;
                    }
                }
                if (path & training)
                {
                    startTraining();
                }
                else if (path & !training)
                {
                    startDemo();
                }
                if (path & capture)
                {
                    button1.Background = Brushes.Red;
                    button1.Content = "End";
                    index1 = KinectDiagnosticViewer.dt1.Rows.Count;
                    timeStamp = DateTime.Now.ToString();
                }
            }
            else
            {
                index2 = KinectDiagnosticViewer.dt1.Rows.Count;
                DataTable dt1 = KinectDiagnosticViewer.dt1.Copy();
                
                for (int i = (index1 + 1); i < index2-1; i++)
                {
                    for (int j = 0; j < 60; j++)
                    {
                        skelData.Add(dt1.Rows[i][j]);
                    }
                    kinectTilt.Add(dt1.Rows[i][60]);
                    gpVector.Add(dt1.Rows[i][61]);
                    gpVector.Add(dt1.Rows[i][62]);
                    gpVector.Add(dt1.Rows[i][63]);
                    gpVector.Add(dt1.Rows[i][64]);
                }
                if (!training)
                {
                    endDemo();
                }

                createMatFile();

                button1.Background = Brushes.Green;
                button1.Content = "Start";

                skelData.Clear();
                gpVector.Clear();
                kinectTilt.Clear();
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
        private bool training = true;
        private bool capture = true;
        private bool path = false;
        private string folder = "";
        private string exercise = "";
        private string index = "";
        private string criticalC1 = "0";
        private string criticalC2 = "0";
        private string criticalC3 = "0";
        private SoundPlayer startAudio = new SoundPlayer(@"C:\Users\Abdi\Documents\Visual Studio 2010\Projects\Kinect-Tracking-Project\DataFiles\Demo\hi.wav");
        private SoundPlayer endAudio = new SoundPlayer(@"C:\Users\Abdi\Documents\Visual Studio 2010\Projects\Kinect-Tracking-Project\DataFiles\Demo\hi.wav");
        ArrayList skelData = new ArrayList();
        ArrayList gpVector = new ArrayList();
        ArrayList kinectTilt = new ArrayList();
    }
}
