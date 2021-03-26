using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessing;
using System.Drawing;
using System.Drawing.Imaging;
using System;

namespace ImageProcessingTests
{
    [TestClass]
    public class ImageProcessingTest
    {
        ImagePr img = new ImagePr();
        Bitmap bmp_test;
        Bitmap bmp_ref;

        public void CreateTestBitmap()
        {
            bmp_test = new Bitmap(3, 3, PixelFormat.Format24bppRgb);
            bmp_ref = new Bitmap(3, 3, PixelFormat.Format24bppRgb);
            //Create Test Bitmap with dominate blue color 
            for (int i = 0; i < bmp_test.Width; i++)
            {
                for (int j = 0; j < bmp_test.Height; j++)
                {
                    Color c = new Color();
                    c = Color.FromArgb(163, 25, 220);
                    bmp_test.SetPixel(i, j, c);
                }
            }
            //LoadImage
            img.LoadImage(bmp_test);


            //Create Reference Bitmap all pixel are blue #0000ff
            for (int i = 0; i < bmp_ref.Width; i++)
            {
                for (int j = 0; j < bmp_ref.Height; j++)
                {
                    Color c = new Color();
                    c = Color.FromArgb(0, 0, 255);
                    bmp_ref.SetPixel(i, j, c);
                }
            }
        }

        [TestMethod]
        public void LoadImageTest()
        {
            CreateTestBitmap();
            img.LoadImage(bmp_test);

            //Assert
            Assert.AreNotEqual(img.bmp, null);
        }


        [TestMethod]
        public void SaveImageWrongParametersTest()
        {
            CreateTestBitmap();
            img.LoadImage(bmp_test);
            img.SaveImage("test", "HJK");


            //Assert
            Assert.AreEqual(false, img.isSave);
        }


        [TestMethod]
        public void SaveImageGoodParametersTest()
        {
            CreateTestBitmap();
            img.LoadImage(bmp_test);
            img.SaveImage("test", "JPG");


            //Assert
            Assert.AreEqual(true, img.isSave);
        }



        [TestMethod]
        public void ToMainColorsTestOnePixelSynchronous()
        {
            int synch = 0;

            CreateTestBitmap();

            //Change reference bitmap into byte array
            Rectangle rec_ref = new Rectangle(0, 0, bmp_ref.Width, bmp_ref.Height);
            BitmapData bmpData_ref = bmp_ref.LockBits(rec_ref, ImageLockMode.ReadWrite, bmp_ref.PixelFormat);
            int bytes_ref = Math.Abs(bmpData_ref.Stride) * bmp_ref.Height;
            byte[] ref_ = new byte[bytes_ref];

            Bitmap bmp_new =img.ToMainColors(synch);

            //Change reference bitmap into byte array
            Rectangle rec_new = new Rectangle(0, 0, bmp_new.Width, bmp_new.Height);
            BitmapData bmpData_new = bmp_new.LockBits(rec_new, ImageLockMode.ReadWrite, bmp_new.PixelFormat);
            int bytes_new = Math.Abs(bmpData_new.Stride) * bmp_new.Height;
            byte[] new_ = new byte[bytes_new];

            // Assert
            CollectionAssert.AreEqual(new_,ref_);

        }

        [TestMethod]
        public void ToMainColorsTestOnePixelAsynchronous()
        {
            int synch = 1;

            CreateTestBitmap();

            //Change reference bitmap into byte array
            Rectangle rec_ref = new Rectangle(0, 0, bmp_ref.Width, bmp_ref.Height);
            BitmapData bmpData_ref = bmp_ref.LockBits(rec_ref, ImageLockMode.ReadWrite, bmp_ref.PixelFormat);
            int bytes_ref = Math.Abs(bmpData_ref.Stride) * bmp_ref.Height;
            byte[] ref_ = new byte[bytes_ref];

            Bitmap bmp_new = img.ToMainColors(synch);

            //Change reference bitmap into byte array
            Rectangle rec_new = new Rectangle(0, 0, bmp_new.Width, bmp_new.Height);
            BitmapData bmpData_new = bmp_new.LockBits(rec_new, ImageLockMode.ReadWrite, bmp_new.PixelFormat);
            int bytes_new = Math.Abs(bmpData_new.Stride) * bmp_new.Height;
            byte[] new_ = new byte[bytes_new];

            // Assert
            CollectionAssert.AreEqual(new_, ref_);

        }



    }
}
