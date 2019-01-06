using System;
using System.Collections.Generic;
using System.Text;
using PaintDotNet;
using PaintDotNet.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace PDNRSonicGFXLoader
{
    public class GraphicsImage : FileType
    {
        public GraphicsImage()
        : base("Retro Engine Grapics Image (v2/2007 ver)",         //Description
        FileTypeFlags.SupportsLoading | FileTypeFlags.SupportsSaving,   //Flags
        new String[] { ".gfx" })
        { }                                    //FileType

        RSDKvRS.gfx gfx = new RSDKvRS.gfx(); //use this to hold gfx image data, and etc

        protected override Document OnLoad(Stream input)
        {
            try
            {
                gfx = new RSDKvRS.gfx(input); //Load our gfx file (see that file for details)

                return Document.FromImage(gfx.gfxImage); //Set the main canvas data to our image data
            }
            catch
            {
                //Oh no!
                MessageBox.Show("Problem Importing File");

                //set a safe default
                Bitmap b = new Bitmap(256, 256);

                using (Graphics g = Graphics.FromImage(b))
                {
                    g.Clear(Color.Black); //set it out so that everything is colour 000000 (since that's RSonic's transparent colour)
                }

                return Document.FromImage(b); //put our image data in the canvas
            }
        }

        protected override void OnSave(Document input, Stream output, SaveConfigToken token,
            Surface scratchSurface, ProgressEventHandler callback)
        {
            RenderArgs ra = new RenderArgs(new Surface(input.Size)); //get image data

            gfx.importFromBitmap(ra.Bitmap); //make a .gfx from the image data (see that file for more info)
        }
    }

    public class GraphicsImageFactory : IFileTypeFactory
    {
        public FileType[] GetFileTypeInstances()
        {
            return new FileType[] { new GraphicsImage() }; //load file info
        }
    }
}
