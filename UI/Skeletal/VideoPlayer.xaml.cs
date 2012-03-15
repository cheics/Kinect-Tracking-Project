using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_Media_Player;

namespace SkeletalViewer
{
    /// <summary>
    /// Interaction logic for VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : Window
    {
        public VideoPlayer()
        {
            InitializeComponent();
        }
        internal void passVideo(String path)
        {
            ucMediaPlayer1.Content = path;
        }
    }
}
