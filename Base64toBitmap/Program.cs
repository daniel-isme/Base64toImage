using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
                                    //.OrderBy(f => f.Length)
                                    .ToList();

            int widthPx = 0;
            foreach (FileInfo file in Files)
            {
                try
                {
                    string path = txtFolder + file.Name;
                    string ImageText = File.ReadAllText(path);
                    Byte[] bitmapData = Convert.FromBase64String(FixBase64ForImage(ImageText));
                    int position = 0;

                    string decodedString = Encoding.UTF8.GetString(bitmapData);
                    Regex regex = new Regex(@"my position b['].+[']");
                    Match match = regex.Match(decodedString);
                    if (match.Success)
                    {
                        Match matchInBrackets = new Regex(@"['].+[']").Match(match.Value);

                        string posEncoded = new Regex(@"[a-zA-Z0-9]+").Match(matchInBrackets.Value).Value;
                        string posDecoded = Encoding.UTF8.GetString(Convert.FromBase64String(posEncoded));
                        position = int.Parse(new Regex(@"[0-9]+").Match(posDecoded).Value);
                    }

                    MemoryStream streamBitmap = new MemoryStream(bitmapData);
                    Bitmap bitImage = new Bitmap((Bitmap)Image.FromStream(streamBitmap));

                    Graphics gr = Graphics.FromImage(finalBitmap);

                    gr.DrawImage(bitImage, position, 0);
                }
                catch
                {
                    //Console.WriteLine(file.Name);
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
