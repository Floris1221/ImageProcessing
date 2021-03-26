using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public class ImagePr: EventArgs
    {
        public Bitmap bmp;
        public int w, h;
        int red, blue, green;
        public string elapsedTime;
        public bool isSave = false;
        Stopwatch stopwatch;

        public ImagePr() {stopwatch = new Stopwatch(); }
        



        //Load Image
        public void LoadImage(Image image)
        {
            bmp = new Bitmap(image);
            w = bmp.Width;
            h = bmp.Height;
        }

        //Save Image
        public void SaveImage(string name,string format)
        {
            if (format.Equals("JPG"))
            {
                bmp.Save(name + ".jpg", ImageFormat.Jpeg);
                isSave = true;
            }
            else if (format.Equals("PNG"))
            {
                bmp.Save(name + ".png", ImageFormat.Png);
                isSave = true;
            }
            else if (format.Equals("BMP"))
            {
                bmp.Save(name + ".bmp", ImageFormat.Bmp);
                isSave = true;
            }
            else
            {
                isSave = false;
            }
        }

        //Choose dominate and set color
        private Color ColorSelect(int red,int blue,int green)
        {
            int next;
            Color color;
            if(red >= blue)
            {
                next = red;
                color = Color.FromArgb(255, 0, 0);
            }
            else
            {
                next = blue;
                color = Color.FromArgb(0, 0, 255);
            }
            if(next >= green)
            {
                return color;
            }
            else
            {
                return Color.FromArgb(0, 255, 0);

            }

        }

        public Bitmap ToMainColors(int syn)
        {
            //if synchronous
            if (syn == 0)
            {
                stopwatch.Start();
                //set pixel by pixel
                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < h; j++)
                    {
                        Color color = bmp.GetPixel(i, j);
                        red = color.R;
                        blue = color.B;
                        green = color.G;
                        bmp.SetPixel(i, j, ColorSelect(red,blue,green));
                    }
                }
                stopwatch.Stop();
                ProcesingTime();    //reset stowatch remember time
                return bmp;
            }
            // if asynchronous
            else
            {
                unsafe
                {
                    stopwatch.Start();
                    Rectangle rec = new Rectangle(0, 0, w, h);
                    //lock bitmap
                    BitmapData bmpData = bmp.LockBits(rec, ImageLockMode.ReadWrite, bmp.PixelFormat);
                    //bytes in one pixel 
                    int bytesPerPixel = Bitmap.GetPixelFormatSize(bmp.PixelFormat) / 8;
                    int heightInPixels = bmpData.Height;
                    int widthInBytes = bmpData.Width * bytesPerPixel;
                    byte* PtrFirstPixel = (byte*)bmpData.Scan0;

                    Parallel.For(0, heightInPixels, y =>
                      {
                          //set image pixel line by line 
                          byte* currentLine = PtrFirstPixel + (y * bmpData.Stride);
                          for(int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                          {
                              red = currentLine[x + 2];
                              blue = currentLine[x];
                              green = currentLine[x + 1];

                              //call method to check wchich color is dominate
                              Color co = ColorSelect(red, blue, green);
                              //if red is dominate
                              if (co.R != 0)
                              {
                                  currentLine[x + 2] = 255;
                                  currentLine[x] = 0;
                                  currentLine[x + 1] = 0;
                              }
                              //if blue is dominate
                              else if (co.B != 0)
                              {
                                  currentLine[x + 2] = 0;
                                  currentLine[x] = 255;
                                  currentLine[x + 1] = 0;
                              }
                              //if green is dominate
                              else if (co.G != 0)
                              {
                                  currentLine[x + 2] = 0;
                                  currentLine[x] = 0;
                                  currentLine[x + 1] = 255;
                              }
                          }
                      });
                    //unlock bitmap
                    bmp.UnlockBits(bmpData);
                }
                stopwatch.Stop();
                ProcesingTime();   //reset stowatch remember time
                return bmp;
            }
        }


        //reset stopwacth and remeber processing time
        private void ProcesingTime()
        {
            TimeSpan ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}",ts.Minutes,
            ts.Seconds,ts.Milliseconds / 10);
            stopwatch.Reset();
        } 

    }


}
