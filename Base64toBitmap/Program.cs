using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Base64toBitmap
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap finalBitmap = new Bitmap(800, 1066);
            string txtFolder = @"C:\Users\danii\source\repos\Base64toBitmap\Base64toBitmap\txt\";

            DirectoryInfo d = new DirectoryInfo(txtFolder);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.txt"); //Getting Text files

            List<string> fileNames = new List<string>();
            foreach (FileInfo file in Files)
            {
                fileNames.Add(file.Name);
            }

            fileNames.Sort();

            int i = 0;
            foreach (string fileName in fileNames)
            {
                try
                {
                    string path = txtFolder + fileName;
                    string ImageText = File.ReadAllText(path);
                    Byte[] bitmapData = Convert.FromBase64String(FixBase64ForImage(ImageText));
                    MemoryStream streamBitmap = new MemoryStream(bitmapData);
                    Bitmap bitImage = new Bitmap((Bitmap)Image.FromStream(streamBitmap));

                    Graphics gr = Graphics.FromImage(finalBitmap);
                    gr.DrawImage(bitImage, i++, 0);
                }
                catch
                {
                    continue;
                }
            }

            //string path1 = @"C:\Users\danii\source\repos\Base64toBitmap\Base64toBitmap\txt\00059d52cee54eaea00a8c3c3167e231.txt";
            //string ImageText1 = File.ReadAllText(path1);
            //Byte[] bitmapData1 = Convert.FromBase64String(FixBase64ForImage(ImageText1));
            //MemoryStream streamBitmap = new MemoryStream(bitmapData1);
            //Bitmap bitImage1 = new Bitmap((Bitmap)Image.FromStream(streamBitmap));

            //Graphics gr = Graphics.FromImage(finalBitmap);
            //gr.DrawImage(bitImage1, 0, 0);

            finalBitmap.Save(@"C:\Users\danii\source\repos\Base64toBitmap\Base64toBitmap\images\image.jpeg", ImageFormat.Jpeg);
        }

        static string FixBase64ForImage(string Image)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", String.Empty); sbText.Replace(" ", String.Empty);
            return sbText.ToString();
        }
    }
}
