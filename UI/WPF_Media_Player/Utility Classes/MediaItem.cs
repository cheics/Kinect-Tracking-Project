using System;
using System.IO;
using System.Collections;

namespace WPF_Media_Player
{
    /// <summary>
    /// represents a single media item
    /// </summary>
    public class MediaItem
    {
        #region instance fields
        private string itemUri;
        private int itemRating;
        public static string[] allowableMediaTypes = 
                                { 
                                    ".mpg", ".mpeg", ".m1v", ".mp2", ".mpa", 
                                    ".mpe", ".avi",".wmv"
                                };
        #endregion
        #region Ctor
        public MediaItem(string itemUri, int itemRating)
        {
            this.itemUri = itemUri;
        }
        #endregion
        #region Public methods/properties
        public string ItemUri
        {
            get { return itemUri; }
        }
 
        public int ItemRating
        {
            get { return itemRating; }
            set { itemRating=value; }
        }

        public string ItemRatingFull
        {
            get { return itemRating + " out of 5"; }
            
        }

        public override string ToString()
        {
            return itemUri + " " + itemRating;
        }
        #endregion
    }
}
