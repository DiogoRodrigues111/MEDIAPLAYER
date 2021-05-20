using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDIA_PLAYER
{
	public struct PlaylistStruct
	{
		public volatile bool _CanNext;

		public class Playlist
		{
			public readonly PlaylistPage _pagePlaylistForm;

			public bool DoOpenPlaylist()
			{
				if (_pagePlaylistForm.Visibility == System.Windows.Visibility.Hidden)
				{
					_pagePlaylistForm.Visibility = System.Windows.Visibility.Visible;
					return true;
				}

				return false;
			}

			public bool DoClosePlaylist()
			{
				if (_pagePlaylistForm.Visibility == System.Windows.Visibility.Visible)
				{
					_pagePlaylistForm.Visibility = System.Windows.Visibility.Hidden;
					return true;
				}

				return true;
			}
		}
	}
}
