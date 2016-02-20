using System.IO;
using System.Windows.Forms;

namespace TRLibrary
{
	public class Dragging
	{
		public static void DragAndEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) // Normal files or folders
			{
				e.Effect = DragDropEffects.Copy;
				return;
			}

			if (e.Data.GetDataPresent(DataFormats.Text)) // URL drag and drop from browser "lock" icon
			{
				e.Effect = DragDropEffects.Copy;
				return;
			}
		}

		public static string[] GetDroppedFiles(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

			if (files == null) // Check for URL drag and drop from browser "lock" icon
			{
				files = new string[]
				{
					(string)e.Data.GetData(DataFormats.Text, false)
				};
			}

			if (files[0] != null &&
				(File.Exists(files[0]) ||
				Directory.Exists(files[0]) || 
				Admin.IsValidUrl(files[0])))
            {
                return files;
            }
            else
            {
                return null;
            }
        }

        #region Handlers

        #endregion Handlers
    }
}
