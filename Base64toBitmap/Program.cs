using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Base64toBitmap
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap finalBitmap = new Bitmap(800, 1066);
            string workingDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
            string txtFolder = workingDirectory + @"txt\";
            string imagesFolder = workingDirectory + @"images\";

            DirectoryInfo d = new DirectoryInfo(txtFolder);//Assuming Test is your Folder
            List<FileInfo> Files = d.GetFiles("*.txt")
                                    .OrderBy(f => f.Length)
                                    .ToList(); //Getting Text files

            //fileNames = fileNames.OrderBy(a => Guid.NewGuid()).ToList();
            //fileNames.Sort();

            int widthPx = 0;
            int sum = 400;
            int koef = -1;
            foreach (FileInfo file in Files)
            {
                try
                {
                    string path = txtFolder + file.Name;
                    string ImageText = File.ReadAllText(path);
                    Byte[] bitmapData = Convert.FromBase64String(FixBase64ForImage(ImageText));
                    MemoryStream streamBitmap = new MemoryStream(bitmapData);
                    Bitmap bitImage = new Bitmap((Bitmap)Image.FromStream(streamBitmap));

                    Graphics gr = Graphics.FromImage(finalBitmap);

                    var pos = widthPx;
                    //pos = 400 + koef * sum + -koef * widthPx;

                    gr.DrawImage(bitImage, pos, 0);
                    koef *= -1;
                    widthPx++;
                }
                catch
                {
                    Console.WriteLine(file.Name);
                    continue;
                }
            }

            finalBitmap.Save($"{imagesFolder}image.png", ImageFormat.Png);
        }

        static string FixBase64ForImage(string Image)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", String.Empty); sbText.Replace(" ", String.Empty);
            return sbText.ToString();
        }
    }
}
