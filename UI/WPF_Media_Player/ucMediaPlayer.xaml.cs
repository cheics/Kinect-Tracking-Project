using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;


namespace WPF_Media_Player
{
    /// <summary>
    /// prvoides a media player with a track list and volume controls and rating system
    /// </summary>
    public partial class ucMediaPlayer : System.Windows.Controls.UserControl
    {
        #region Instance Fields
        private double mediaItemsWidth = -1;
        bool singleMode = false;
        private Dictionary<string, MediaItem> mediaItems = new Dictionary<string, MediaItem>();
        ToggleButton[] btnStars = new ToggleButton[5];
        #endregion
        #region Ctor
        public ucMediaPlayer()
        {
            InitializeComponent();

            sliderTime.IsEnabled = false;
            sliderVolume.IsEnabled = false;
            txtRating.Visibility = Visibility.Hidden;
        }
        #endregion
        #region Private methods
        /// <summary>
        /// save the media items
        /// </summary>
        //private void lblSave_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (mediaItems.Count > 0)
        //    {
        //        System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
        //        sfd.Filter = "xml (*.xml)|*.xml";
        //        if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
        //        {
        //            string filename = sfd.FileName;
        //            FileInfo f = new FileInfo(filename);
        //            if (f.Extension.EndsWith(".xml"))
        //            {
        //                List<MediaItem> mediaList = new List<MediaItem>();
        //                foreach (MediaItem mi in mediaItems.Values)
        //                    mediaList.Add(mi);
        //                if (XmlHandler.WriteXmlFile(filename, mediaList))
        //                {
        //                    MessageBox.Show("Sucessfully saved your items");
        //                }
        //            }
        //            else
        //            {
        //                MessageBox.Show("Can only save to xml files", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("There are no items to save");
        //    }
        //}

        /// <summary>
        /// add extra media items from a saved file
        /// </summary>
        //private void lblLoad_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
        //    ofd.Filter = "xml (*.xml)|*.xml";
        //    if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
        //    {
        //        string filename = ofd.FileName;
        //        FileInfo f = new FileInfo(filename);
        //        if (f.Extension.EndsWith(".xml"))
        //        {
        //            List<MediaItem> mediaList = XmlHandler.ReadXmlFile(filename);
        //            if (mediaList.Count > 0)
        //            {
        //                foreach (MediaItem mi in mediaList)
        //                {
        //                    if (!mediaItems.ContainsKey(mi.ItemUri))
        //                    {
        //                        mediaItems.Add(mi.ItemUri.Substring(mi.ItemUri.LastIndexOf(@"\") + 1), mi);
        //                        lstMediaItems.Items.Add(mi);
        //                    }
        //                }
        //                MessageBox.Show("Sucessfully added your items");
        //            }
        //            else
        //            {
        //                MessageBox.Show("There were no items found to add");
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Can only open xml files", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //}

        /// <summary>
        /// Clear all media items
        /// </summary>
        //private void lblClear_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    lstMediaItems.Items.Clear();
        //}

        /// <summary>
        /// Change to 3D media buttons
        /// </summary>
        //private void lbl3D_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    // Get the selected Style from the Window's Resources.
        //    Style buttonStyle = this.FindResource("btn3DStyle") as Style;

        //    // Add the selected Style to the Window's Resources with the
        //    // resource key used by the Button.Style's DynamicResource reference.
        //    this.Resources["ButtonStyle"] = buttonStyle;
        //}

        //private void lblNormal_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    // Get the selected Style from the Window's Resources.
        //    Style buttonStyle = this.FindResource("VideoButton") as Style;

        //    // Add the selected Style to the Window's Resources with the
        //    // resource key used by the Button.Style's DynamicResource reference.
        //    this.Resources["ButtonStyle"] = buttonStyle;
        //}

        /// <summary>
        /// set the rating to the current stars for the current media items
        /// </summary>
        //private void btnStars_Click(object sender, RoutedEventArgs e)
        //{
        //    int rating = 0;
        //    for (int i = 0; i < 5; i++)
        //    {
        //        rating += btnStars[i].IsChecked.Value ? 1 : 0;
        //    }
        //    string key = mediaPlayerMain.Source.AbsolutePath.Substring(mediaPlayerMain.Source.AbsolutePath.LastIndexOf(@"/")+1);
        //    mediaItems[key].ItemRating = rating;
        //}

        /// <summary>
        /// Stop media when ended
        /// </summary>
        private void mediaPlayerMain_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaPlayerMain.Stop();
        }

        /// <summary>
        /// Initialise UI elements based on current media item
        /// </summary>
        private void mediaPlayerMain_MediaOpened(object sender, RoutedEventArgs e)
        {
            sliderTime.Maximum = mediaPlayerMain.NaturalDuration.TimeSpan.TotalMilliseconds;
            sliderTime.IsEnabled = mediaPlayerMain.IsLoaded;
            sliderVolume.IsEnabled = mediaPlayerMain.IsLoaded;
        }

        /// <summary>
        /// stop the media playing
        /// </summary>
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            // The Stop method stops and resets the media to be played from
            // the beginning.
            mediaPlayerMain.Stop();
        }

        /// <summary>
        /// pause the media playing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            // The Pause method pauses the media if it is currently running.
            // The Play method can be used to resume.
            mediaPlayerMain.Pause();
        }

        /// <summary>
        /// play the media
        /// </summary>
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            // The Play method will begin the media if it is not currently active or 
            // resume media if it is paused. This has no effect if the media is
            // already running.
            mediaPlayerMain.Play();
            mediaPlayerMain.Volume = (double)sliderVolume.Value;
        }

        /// <summary>
        /// collapse/expand media items to take up entire grid row (so no media item list)
        /// </summary>
        //private void btnMediaPlayerMain_Click(object sender, RoutedEventArgs e)
        //{
        //    if (mediaPlayerBorder != null)
        //    {
        //        int col = Grid.GetColumn(mediaPlayerBorder);
        //        //work out which columns need to be resized in grid
        //        for (int indexCol = 0; indexCol < grdMain.ColumnDefinitions.Count; indexCol++)
        //        {
        //            if (indexCol != col)
        //            {
        //                //grid column for media length animations
        //                GridLengthAnimation gla = new GridLengthAnimation();
        //                gla.From = new GridLength(singleMode ? 10 : mediaItemsWidth, GridUnitType.Pixel);
        //                gla.To = new GridLength(singleMode ? mediaItemsWidth : 10, GridUnitType.Pixel);
        //                gla.Duration = new TimeSpan(0, 0, 2);
        //                grdMain.ColumnDefinitions[indexCol].BeginAnimation(ColumnDefinition.WidthProperty, gla);
        //                //opacity (fade media items list)
        //                DoubleAnimation hideShowOpacAnim = new DoubleAnimation();
        //                hideShowOpacAnim.From = singleMode ? 0.0 : 1.0;
        //                hideShowOpacAnim.To = singleMode ? 1.0 : 0.0;
        //                hideShowOpacAnim.Duration = new Duration(TimeSpan.Parse("0:0:1"));
        //                svMediaItems.BeginAnimation(ScrollViewer.OpacityProperty, hideShowOpacAnim);
        //            }
        //        }
        //    }
        //    singleMode = !singleMode;            
        //}

        /// <summary>
        /// reset selected index if one selected. Item style uses buttons, so user uses buttons
        /// rather than selected index
        /// </summary>
        //private void lstMediaItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //dont want to use the listbox selected item/index. As the Item template is
        //    //a button for each listitem lets use the buttons click event
        //    lstMediaItems.SelectedIndex = -1;
        //}

        /// <summary>
        /// initialise UI state
        /// </summary>
        private void ucMediaPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            //mediaItemsWidth = svMediaItems.Width;
            mediaPlayerBorder.Visibility = Visibility.Hidden;

            // Get the selected Style from the Window's Resources.
            Style buttonStyle = this.FindResource("btn3DStyle") as Style;

            // Add the selected Style to the Window's Resources with the
            // resource key used by the Button.Style's DynamicResource reference.
            this.Resources["ButtonStyle"] = buttonStyle;

            mediaPlayerMain.Source = null;
            Uri uri = new Uri(this.Content.ToString());
            mediaPlayerMain.Source = uri;
            mediaPlayerBorder.Visibility = Visibility.Visible;
            mediaPlayerMain.Play();
        }

        /// <summary>
        /// seek media to position (x)
        /// </summary>
        private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            int SliderValue = (int)sliderTime.Value;
            // Overloaded constructor takes the arguments days, hours, minutes, seconds, miniseconds.
            // Create a TimeSpan with miliseconds equal to the slider value.
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            mediaPlayerMain.Position = ts;
        }

        /// <summary>
        /// change media volume to position (x)
        /// </summary>
        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            mediaPlayerMain.Volume = (double)sliderVolume.Value;
        }

        /// <summary>
        /// create star buttons
        /// </summary>
        //private void createStarButtons()
        //{
        //    string key = mediaPlayerMain.Source.AbsolutePath.Substring(mediaPlayerMain.Source.AbsolutePath.LastIndexOf(@"/") + 1);
        //    int itemRating = mediaItems[key].ItemRating;

        //    starStack.Children.Clear();

        //    for (int i = 0; i < 5; i++)
        //    {
        //        btnStars[i] = new ToggleButton();
        //        btnStars[i].Width = 14;
        //        btnStars[i].Height = 20;
        //        btnStars[i].IsChecked = itemRating >= i+1;
        //        btnStars[i].Click += new RoutedEventHandler(btnStars_Click);
        //        btnStars[i].Checked += new RoutedEventHandler(btnStars_Checked);
        //        btnStars[i].Unchecked += new RoutedEventHandler(btnStars_Unchecked);
        //        btnStars[i].IsEnabled = false;
        //        addStarStyle(btnStars[i]);
        //    }
        //    btnStars[0].IsEnabled = true;

        //    //show the text only if the buttons were added successfully
        //    txtRating.Visibility = starStack.Children.Count > 0 ? Visibility.Visible : Visibility.Hidden;
        //}

        /// <summary>
        /// work out which other stars can be checked
        /// </summary>
        //private void btnStars_Checked(object sender, RoutedEventArgs e)
        //{

        //    ToggleButton starButton = (ToggleButton)sender;
        //    int pos = -1;

        //    for (int i = 0; i < 5; i++)
        //    {
        //        if (btnStars[i].Equals(starButton))
        //        {
        //            pos = i;
        //        }
        //    }

        //    //special case need to reset position 0 star, as it will still be enabled
        //    //from when the stars were created
        //    if (pos == 1)
        //        btnStars[0].IsEnabled = false;

        //    //only allow next consecutive starButton to be toggled
        //    //basically must be checked in order
        //    if (pos > -1 && pos < 4)
        //    {
        //        btnStars[pos].IsEnabled = false;
        //        btnStars[pos + 1].IsEnabled = true;
        //    }
        //}

        /// <summary>
        /// work out which other stars can be Unchecked
        /// </summary>
        //private void btnStars_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    ToggleButton starButton = (ToggleButton)sender;
        //    int pos = -1;

        //    for (int i = 0; i < 5; i++)
        //    {
        //        if (btnStars[i].Equals(starButton))
        //        {
        //            pos = i;
        //        }
        //    }
        //    //only allow next consecutive starButton to be toggled
        //    //basically must be checked in order
        //    if (pos > 0 && pos < 5)
        //    {
        //        if( pos < 4)
        //            btnStars[pos+1].IsEnabled = false;
        //        btnStars[pos].IsEnabled = true;
        //        btnStars[pos - 1].IsEnabled = true;
        //    }
        //}

        /// <summary>
        /// add Style to star buttons
        /// </summary>
        //private void addStarStyle(ToggleButton btnStar)
        //{
        //    Style starStyle = this.Resources["StarToggleButton"] as Style; ;
        //    if (starStyle != null)
        //    {
        //        btnStar.Style = starStyle;
        //        starStack.Children.Add(btnStar);
        //    }
        //}

        /// <summary>
        /// open new media item based on item clicked
        /// </summary>
        //private void btnMediaItems_Clicked(object sender, RoutedEventArgs e)
        //{
        //    Button b = sender as Button;
        //    MediaElement mel = b.Content as MediaElement;
        //    mediaPlayerMain.Source = null;
        //    mediaPlayerMain.Source = mel.Source;
        //    mediaPlayerBorder.Visibility = Visibility.Visible;
        //    mediaPlayerMain.Play();
        //    mediaPlayerMain.Volume = (double)sliderVolume.Value;
        //    createStarButtons();
        //}

        /// <summary>
        /// clear media items list
        /// </summary>
        //private void btnClearMediaList_Click(object sender, RoutedEventArgs e)
        //{
        //    lstMediaItems.Items.Clear();
        //}

        /// <summary>
        /// Handles Drop Event for Media Items.
        /// </summary>
        //private void Media_Drop(object sender, DragEventArgs e)
        //{
        //    string[] fileNames = e.Data.GetData(DataFormats.FileDrop, true) as string[];
        //    //keep a dictionary of added files
        //    foreach (string f in fileNames)
        //    {
        //        if (IsValidMediaItem(f))
        //            mediaItems.Add(f.Substring(f.LastIndexOf(@"\")+1),new MediaItem(@f,0));
        //    }

        //    //now add to the list
        //    foreach (MediaItem mi in mediaItems.Values)
        //        lstMediaItems.Items.Add(mi);

        //    // Mark the event as handled, so the control's native Drop handler is not called.
        //    e.Handled = true;
        //}

        /// <summary>
        /// check to see if dragged items are valid
        /// </summary>
        /// <returns>true if filename is valid</returns>
        //private bool IsValidMediaItem(string filename)
        //{
        //    bool isValid = false;
        //    string fileExtesion = filename.Substring(filename.LastIndexOf("."));
        //    foreach (string s in MediaItem.allowableMediaTypes)
        //    {
        //        if (s.Equals(fileExtesion, StringComparison.CurrentCultureIgnoreCase))
        //            isValid = true;
        //    }
        //    return isValid;
        //}
        #endregion 

        #region DependencyProperty Content

        /// <summary>
        /// Registers a dependency property as backing store for the Content property
        /// </summary>
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(ucMediaPlayer),
            new FrameworkPropertyMetadata(null,
                  FrameworkPropertyMetadataOptions.AffectsRender |
                  FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        /// <summary>
        /// Gets or sets the Content.
        /// </summary>
        /// <value>The Content.</value>
        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        #endregion

    }
}