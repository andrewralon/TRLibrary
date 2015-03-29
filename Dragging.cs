using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TRLibrary
{
    public class Dragging
    {
        public static void DragAndEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        public static string[] GetDroppedFiles(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
			
			//foreach (string file in files)
			//{
			//	Console.WriteLine(file);
			//}

			// RESOLVE LINKS HERE!

            if (files[0] != null && 
                (File.Exists(files[0]) || Directory.Exists(files[0])))
            {
                return files;
            }
            else
            {
                return null;
            }
        }

		public static bool IsLink(string filename)
		{


			return false;
		}

        #region Handlers

        //private void HandleDragAndEnter(object sender, DragEventArgs e)
        //{
        //    DragAndEnter(sender, e);
        //}

        //private void HandleDragAndDrop(object sender, DragEventArgs e)
        //{
        //    //DoStuff(sender, e);
        //}

        //private void MainForm_DragEnter(object sender, DragEventArgs e)
        //{
        //    HandleDragAndEnter(sender, e);
        //}

        //private void MainForm_DragDrop(object sender, DragEventArgs e)
        //{
        //    HandleDragAndDrop(sender, e);
        //}

        #endregion Handlers
    }
}
