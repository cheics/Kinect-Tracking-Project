/////////////////////////////////////////////////////////////////////////
//
// This module provides sample code used to demonstrate the use
// of the KinectAudioSource for speech recognition in a game setting.
//
// Copyright © Microsoft Corporation.  All rights reserved.  
// This code is licensed under the terms of the 
// Microsoft Kinect for Windows SDK (Beta) 
// License Agreement: http://kinectforwindows.org/KinectSDK-ToU
//
/////////////////////////////////////////////////////////////////////////
/*
 * IMPORTANT: This sample requires the following components to be installed:
 * 
 * Speech Platform Runtime (v10.2) x86. Even on x64 platforms the x86 needs to be used because the Kinect SDK runtime is x86
 * http://www.microsoft.com/downloads/en/details.aspx?FamilyID=bb0f72cb-b86b-46d1-bf06-665895a313c7
 * 
 * Kinect English Language Pack: MSKinectLangPack_enUS.msi 
 * http://go.microsoft.com/fwlink/?LinkId=220942
 * 
 * Speech Platform SDK (v10.2) 
 * http://www.microsoft.com/downloads/en/details.aspx?FamilyID=1b1604d3-4f66-4241-9a21-90a294a5c9a4&displaylang=en
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Microsoft.Research.Kinect.Audio;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using ShapeGame.Utils;

namespace ShapeGame.Speech
{
    public class SpeechRecognizer
    {
        #region Speech Grammar
        #region Grammar Data
        public enum Verbs
        {
            None = 0,
            Bigger,
            Biggest,
            Smaller,
            Smallest,
            More,
            Fewer,
            Faster,
            Slower,
            Colorize,
            RandomColors,
            DoShapes,
            ShapesAndColors,
            Reset,
            Pause,
            Resume
        };

        struct WhatSaid
        {
            public Verbs verb;
            public PolyType shape;
            public Color color;
        }

        Dictionary<string, WhatSaid> GameplayPhrases = new Dictionary<string, WhatSaid>()
        {
            {"Faster", new WhatSaid()       {verb=Verbs.Faster}},
            {"Slower", new WhatSaid()       {verb=Verbs.Slower}},
            {"Bigger Shapes", new WhatSaid() {verb=Verbs.Bigger}},
            {"Bigger", new WhatSaid()       {verb=Verbs.Bigger}},
            {"Larger", new WhatSaid()       {verb=Verbs.Bigger}},
            {"Huge", new WhatSaid()         {verb=Verbs.Biggest}},
            {"Giant", new WhatSaid()        {verb=Verbs.Biggest}},
            {"Biggest", new WhatSaid()      {verb=Verbs.Biggest}},
            {"Super Big", new WhatSaid()    {verb=Verbs.Biggest}},
            {"Smaller", new WhatSaid()      {verb=Verbs.Smaller}},
            {"Tiny", new WhatSaid()         {verb=Verbs.Smallest}},
            {"Super Small", new WhatSaid()  {verb=Verbs.Smallest}},
            {"Smallest", new WhatSaid()     {verb=Verbs.Smallest}},
            {"More Shapes", new WhatSaid()  {verb=Verbs.More}},
            {"More", new WhatSaid()         {verb=Verbs.More}},
            {"Less", new WhatSaid()         {verb=Verbs.Fewer}},
            {"Fewer", new WhatSaid()        {verb=Verbs.Fewer}},
        };

        Dictionary<string, WhatSaid> ShapePhrases = new Dictionary<string, WhatSaid>()
        {
            {"7 Pointed Stars", new WhatSaid()  {verb=Verbs.DoShapes, shape=PolyType.Star7}},
            {"Triangles", new WhatSaid()        {verb=Verbs.DoShapes, shape=PolyType.Triangle}},
            {"Squares", new WhatSaid()          {verb=Verbs.DoShapes, shape=PolyType.Square}},
            {"Boxes", new WhatSaid()            {verb=Verbs.DoShapes, shape=PolyType.Square}},
            {"Hexagons", new WhatSaid()         {verb=Verbs.DoShapes, shape=PolyType.Hex}},
            {"Pentagons", new WhatSaid()        {verb=Verbs.DoShapes, shape=PolyType.Pentagon}},
            {"Stars", new WhatSaid()            {verb=Verbs.DoShapes, shape=PolyType.Star}},
            {"Circles", new WhatSaid()          {verb=Verbs.DoShapes, shape=PolyType.Circle}},
            {"Balls", new WhatSaid()            {verb=Verbs.DoShapes, shape=PolyType.Circle}},
            {"Bubbles", new WhatSaid()          {verb=Verbs.DoShapes, shape=PolyType.Bubble}},
            {"All Shapes", new WhatSaid()       {verb=Verbs.DoShapes, shape=PolyType.All}},
            {"Everything", new WhatSaid()       {verb=Verbs.DoShapes, shape=PolyType.All}},
            {"Shapes", new WhatSaid()           {verb=Verbs.DoShapes, shape=PolyType.All}},
        };

        Dictionary<string, WhatSaid> ColorPhrases = new Dictionary<string, WhatSaid>()
        {
            {"Every Color", new WhatSaid()      {verb=Verbs.RandomColors}},
            {"All Colors", new WhatSaid()       {verb=Verbs.RandomColors}},
            {"Random Colors", new WhatSaid()    {verb=Verbs.RandomColors}},
            {"Red", new WhatSaid()              {verb=Verbs.Colorize, color = Color.FromRgb(240,60,60)}},
            {"Green", new WhatSaid()            {verb=Verbs.Colorize, color = Color.FromRgb(60,240,60)}},
            {"Blue", new WhatSaid()             {verb=Verbs.Colorize, color = Color.FromRgb(60,60,240)}},
            {"Yellow", new WhatSaid()           {verb=Verbs.Colorize, color = Color.FromRgb(240,240,60)}},
            {"Orange", new WhatSaid()           {verb=Verbs.Colorize, color = Color.FromRgb(255,110,20)}},
            {"Purple", new WhatSaid()           {verb=Verbs.Colorize, color = Color.FromRgb(70,30,255)}},
            {"Violet", new WhatSaid()           {verb=Verbs.Colorize, color = Color.FromRgb(160,30,245)}},
            {"Pink", new WhatSaid()             {verb=Verbs.Colorize, color = Color.FromRgb(255,128,225)}},
            {"Gray", new WhatSaid()             {verb=Verbs.Colorize, color = Color.FromRgb(192,192,192)}},
            {"Brown", new WhatSaid()            {verb=Verbs.Colorize, color = Color.FromRgb(130,80,50)}},
            {"Dark", new WhatSaid()             {verb=Verbs.Colorize, color = Color.FromRgb(40,40,40)}},
            {"Black", new WhatSaid()            {verb=Verbs.Colorize, color = Color.FromRgb(5,5,5)}},
            {"Bright", new WhatSaid()           {verb=Verbs.Colorize, color = Color.FromRgb(240,240,240)}},
            {"White", new WhatSaid()            {verb=Verbs.Colorize, color = Color.FromRgb(255,255,255)}},
        };

        Dictionary<string, WhatSaid> SinglePhrases = new Dictionary<string, WhatSaid>()
        {
            {"Speed Up", new WhatSaid()         {verb=Verbs.Faster}},
            {"Slow Down", new WhatSaid()        {verb=Verbs.Slower}},
            {"Reset", new WhatSaid()            {verb=Verbs.Reset}},
            {"Clear", new WhatSaid()            {verb=Verbs.Reset}},
            {"Stop", new WhatSaid()             {verb=Verbs.Pause}},
            {"Pause Game", new WhatSaid()       {verb=Verbs.Pause}},
            {"Freeze", new WhatSaid()           {verb=Verbs.Pause}},
            {"Unfreeze", new WhatSaid()         {verb=Verbs.Resume}},
            {"Resume", new WhatSaid()           {verb=Verbs.Resume}},
            {"Continue", new WhatSaid()         {verb=Verbs.Resume}},
            {"Play", new WhatSaid()             {verb=Verbs.Resume}},
            {"Start", new WhatSaid()            {verb=Verbs.Resume}},
            {"Go", new WhatSaid()               {verb=Verbs.Resume}},
        };
        #endregion Grammar Data

        private void LoadGrammar(SpeechRecognitionEngine speechRecognitionEngine)
        {
            // Build a simple grammar of shapes, colors, and some simple program control
            var single = new Choices();
            foreach (var phrase in SinglePhrases)
                single.Add(phrase.Key);

            var gameplay = new Choices();
            foreach (var phrase in GameplayPhrases)
                gameplay.Add(phrase.Key);

            var shapes = new Choices();
            foreach (var phrase in ShapePhrases)
                shapes.Add(phrase.Key);

            var colors = new Choices();
            foreach (var phrase in ColorPhrases)
                colors.Add(phrase.Key);

            var coloredShapeGrammar = new GrammarBuilder();
            coloredShapeGrammar.Append(colors);
            coloredShapeGrammar.Append(shapes);

            var objectChoices = new Choices();
            objectChoices.Add(gameplay);
            objectChoices.Add(shapes);
            objectChoices.Add(colors);
            objectChoices.Add(coloredShapeGrammar);

            var actionGrammar = new GrammarBuilder();
            actionGrammar.AppendWildcard();
            actionGrammar.Append(objectChoices);

            var allChoices = new Choices();
            allChoices.Add(actionGrammar);
            allChoices.Add(single);

            var gb = new GrammarBuilder();
            gb.Culture = speechRecognitionEngine.RecognizerInfo.Culture; //This is needed to ensure that it will work on machines with any culture, not just en-us.
            gb.Append(allChoices);

            var g = new Grammar(gb);
            speechRecognitionEngine.LoadGrammar(g);
            speechRecognitionEngine.SpeechRecognized += sre_SpeechRecognized;
            speechRecognitionEngine.SpeechHypothesized += sre_SpeechHypothesized;
            speechRecognitionEngine.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(sre_SpeechRecognitionRejected);
        }
        #endregion Speech Grammar

        #region SaidSomethingEventArgs
        public class SaidSomethingEventArgs : EventArgs
        {
            public Verbs Verb { get; set; }
            public PolyType Shape { get; set; }
            public Color RGBColor { get; set; }
            public string Phrase { get; set; }
            public string Matched { get; set; }
        }

        public event EventHandler<SaidSomethingEventArgs> SaidSomething;
        #endregion SaidSomethingEventArgs

        #region Private state
        private KinectAudioSource kinectAudioSource;
        private SpeechRecognitionEngine sre;
        private bool paused = false;
        #endregion Private state

        #region ctor + Initialization
        //This method exists so that it can be easily called and return safely if the speech prereqs aren't installed.
        //We isolate the try/catch inside this class, and don't impose the need on the caller.
        public static SpeechRecognizer Create()
        {
            SpeechRecognizer recognizer = null;

            try
            {
                recognizer = new SpeechRecognizer();
            }
            catch (Exception)
            {
                //speech prereq isn't installed. a null recognizer will be handled properly by the app.
            }
            return recognizer;
        }

        private SpeechRecognizer()
        {
            RecognizerInfo ri = GetKinectRecognizer();
            sre = new SpeechRecognitionEngine(ri);
            LoadGrammar(sre);
        }

        private static RecognizerInfo GetKinectRecognizer()
        {
            Func<RecognizerInfo, bool> matchingFunc = r =>
            {
                string value;
                r.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) && "en-US".Equals(r.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
            };
            return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();
        }
        #endregion ctor + Initialization

        #region SpeechRecognition Start/Stop
        public void Start(KinectAudioSource kinectSource)
        {
            kinectAudioSource = kinectSource;
            kinectAudioSource.SystemMode = SystemMode.OptibeamArrayOnly;
            kinectAudioSource.FeatureMode = true;
            kinectAudioSource.AutomaticGainControl = false;
            kinectAudioSource.MicArrayMode = MicArrayMode.MicArrayAdaptiveBeam;
            var kinectStream = kinectAudioSource.Start();
            sre.SetInputToAudioStream(kinectStream, new SpeechAudioFormatInfo(
                                                  EncodingFormat.Pcm, 16000, 16, 1,
                                                  32000, 2, null));
            sre.RecognizeAsync(RecognizeMode.Multiple);
        }

        public void Stop()
        {
            if (sre != null)
            {
                kinectAudioSource.Stop();
                sre.RecognizeAsyncCancel();
                sre.RecognizeAsyncStop();
                kinectAudioSource.Dispose();
            }
        }
        #endregion Start/Stop

        #region Speech engine events
        void sre_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            var said = new SaidSomethingEventArgs();
            said.Verb = Verbs.None;
            said.Matched = "?";
            //Kinect SDK TODO: should make sure after Stop, that we don't get Speech event calls.
            if (SaidSomething != null)
            {
                SaidSomething(new object(), said);
            }
            Console.WriteLine("\nSpeech Rejected");
        }

        void sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            Console.Write("\rSpeech Hypothesized: \t{0}", e.Result.Text);
        }

        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.Write("\rSpeech Recognized: \t{0}", e.Result.Text);

            if (SaidSomething == null)
                return;

            var said = new SaidSomethingEventArgs();
            said.RGBColor = Color.FromRgb(0, 0, 0);
            said.Shape = 0;
            said.Verb = 0;
            said.Phrase = e.Result.Text;

            // First check for color, in case both color _and_ shape were both spoken
            bool foundColor = false;
            foreach (var phrase in ColorPhrases)
                if (e.Result.Text.Contains(phrase.Key) && (phrase.Value.verb == Verbs.Colorize))
                {
                    said.RGBColor = phrase.Value.color;
                    said.Matched = phrase.Key;
                    foundColor = true;
                    break;
                }

            // Look for a match in the order of the lists below, first match wins.
            List<Dictionary<string, WhatSaid>> allDicts = new List<Dictionary<string, WhatSaid>>() { GameplayPhrases, ShapePhrases, ColorPhrases, SinglePhrases };

            bool found = false;
            for (int i = 0; i < allDicts.Count && !found; ++i)
            {
                foreach (var phrase in allDicts[i])
                {
                    if (e.Result.Text.Contains(phrase.Key))
                    {
                        said.Verb = phrase.Value.verb;
                        said.Shape = phrase.Value.shape;
                        if ((said.Verb == Verbs.DoShapes) && (foundColor))
                        {
                            said.Verb = Verbs.ShapesAndColors;
                            said.Matched += " " + phrase.Key;
                        }
                        else
                        {
                            said.Matched = phrase.Key;
                            said.RGBColor = phrase.Value.color;
                        }
                        found = true;
                        break;
                    }
                }
            }

            if (!found)
                return;

            if (paused) // Only accept restart or reset
            {
                if ((said.Verb != Verbs.Resume) && (said.Verb != Verbs.Reset))
                    return;
                paused = false;
            }
            else
            {
                if (said.Verb == Verbs.Resume)
                    return;
            }

            if (said.Verb == Verbs.Pause)
                paused = true;

            if (SaidSomething != null)
            {
                SaidSomething(new object(), said);
            }
        }
        #endregion Speech engine events
    }
}