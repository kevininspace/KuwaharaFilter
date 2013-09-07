//using System;
//using System.Windows.Forms;

//namespace KuwaharaFilter
//{
//    public class Pnd
//    {
//        // put this wherever it's convenient for you
//        // http://msdn.microsoft.com/en-us/library/aa363362(VS.85).aspx
//        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode, SetLastError = false)]
//        public static extern void OutputDebugStringW(
//            [System.Runtime.InteropServices.In] [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lpOutputString);


//        #region UICode
//        int Size = 20;
//        #endregion

//        //http://www.gutgames.com/post/Kuwahara-Filter-in-C.aspx
//        void Render(Surface dst, Surface src, Rectangle rect)
//        {
//            // Delete any of these lines you don't need
//            Rectangle selection = EnvironmentParameters.GetSelection(src.Bounds).GetBoundsInt();
//            int CenterX = ((selection.Right - selection.Left) / 2) + selection.Left;
//            int CenterY = ((selection.Bottom - selection.Top) / 2) + selection.Top;
//            ColorBgra PrimaryColor = (ColorBgra)EnvironmentParameters.PrimaryColor;
//            ColorBgra SecondaryColor = (ColorBgra)EnvironmentParameters.SecondaryColor;
//            int BrushWidth = (int)EnvironmentParameters.BrushWidth;
//            Random TempRandom = new Random();
//            int[] ApetureMinX = { -(Size / 2), 0, -(Size / 2), 0 };
//            int[] ApetureMaxX = { 0, (Size / 2), 0, (Size / 2) };
//            int[] ApetureMinY = { -(Size / 2), -(Size / 2), 0, 0 };
//            int[] ApetureMaxY = { 0, 0, (Size / 2), (Size / 2) };

//            //OutputDebugStringW("ApetureMinX[]: " + ApetureMinX[0] + "," +ApetureMinX[1] + "," +ApetureMinX[2] + "," +ApetureMinX[3]);
//            //OutputDebugStringW("ApetureMaxX[]: " + ApetureMaxX[0] + "," + ApetureMaxX[1] + "," + ApetureMaxX[2] + "," + ApetureMaxX[3]);
//            //OutputDebugStringW("ApetureMinY[]: " + ApetureMinY[0] + "," + ApetureMinY[1] + "," + ApetureMinY[2] + "," + ApetureMinY[3]);
//            //OutputDebugStringW("ApetureMinX[]: " + ApetureMaxY[0] + "," + ApetureMaxY[1] + "," + ApetureMaxY[2] + "," + ApetureMaxY[3]);

//            ColorBgra CurrentPixel;
//            for (int y = selection.Top; y < selection.Bottom; y++)
//            {
//                //OutputDebugStringW("Y position: " + y);
//                for (int x = selection.Left; x < selection.Right; x++)
//                {
//                    CurrentPixel = src[x, y];
//                    int[] RValues = { 0, 0, 0, 0 };
//                    int[] GValues = { 0, 0, 0, 0 };
//                    int[] BValues = { 0, 0, 0, 0 };
//                    int[] NumPixels = { 0, 0, 0, 0 };
//                    int[] MaxRValue = { 0, 0, 0, 0 };
//                    int[] MaxGValue = { 0, 0, 0, 0 };
//                    int[] MaxBValue = { 0, 0, 0, 0 };
//                    int[] MinRValue = { 255, 255, 255, 255 };
//                    int[] MinGValue = { 255, 255, 255, 255 };
//                    int[] MinBValue = { 255, 255, 255, 255 };

//                    for (int i = 0; i < 4; ++i)
//                    {

//                        for (int x2 = ApetureMinX[i]; x2 < ApetureMaxX[i]; ++x2)
//                        {

//                            //if (y == selection.Top && x == selection.Left)
//                            //{
//                            //    OutputDebugStringW("x2: " + x2 + ", x: " + x + ", i: " + i + ", ApetureMinX[i]: " + ApetureMinX[i] + ", ApetureMaxX[i]: " + ApetureMaxX[i]);
//                            //}
//                            int TempX = x + x2;

//                            //                MessageBox.Show(
//                            //"y = " + y.ToString() + "; " +
//                            //"x = " + x.ToString() + "; " +
//                            //"i = " + i.ToString() + "; " +
//                            //"x2 = " + x2.ToString() + "; " +
//                            //"ApetureMinX[i] = " + ApetureMinX[i].ToString() + "; " +
//                            //"ApetureMaxX[i] = " + ApetureMaxX[i].ToString() + "; " +
//                            //                      "TempX = " + TempX.ToString() + "; " +
//                            //                      "selection.Width = " + selection.Width.ToString() + "; "
//                            //                );
//                            if (TempX >= 0 && TempX < selection.Left + selection.Width)
//                            {

//                                for (int y2 = ApetureMinY[i]; y2 < ApetureMaxY[i]; ++y2)
//                                {
//                                    int TempY = y + y2;

//                                    //OutputDebugStringW("TempY = " + TempY.ToString() + "; " +
//                                    //    "selection.Height = " + selection.Height.ToString() + "; "
//                                    //    );

//                                    if (TempY >= 0 && TempY < selection.Bottom + selection.Height)
//                                    {
//                                        //Color TempColor = src.GetPixel(TempX, TempY);
//                                        //MessageBox.Show(
//                                        //                      "R = " + CurrentPixel.R.ToString() + "; " +
//                                        //                      "G = " + CurrentPixel.G.ToString() + "; " +
//                                        //                      "B = " + CurrentPixel.B.ToString() + "; " +
//                                        //"MaxRValue[i] = " + MaxRValue[i].ToString() + "; " +
//                                        //"MinRValue[i] = " + MinRValue[i].ToString() + "; " +
//                                        //"MaxGValue[i] = " + MaxGValue[i].ToString() + "; " +
//                                        //"MinGValue[i] = " + MinGValue[i].ToString() + "; " +
//                                        //"MaxBValue[i] = " + MaxBValue[i].ToString() + "; " +
//                                        //"MinBValue[i] = " + MinBValue[i].ToString() + "; "
//                                        //                );

//                                        RValues[i] += CurrentPixel.R; //TempColor.R;
//                                        GValues[i] += CurrentPixel.G; //TempColor.G;
//                                        BValues[i] += CurrentPixel.B; //TempColor.B;

//                                        if (CurrentPixel.R > MaxRValue[i])
//                                        {
//                                            MaxRValue[i] = CurrentPixel.R;
//                                        }

//                                        else if (CurrentPixel.R < MinRValue[i])
//                                        {
//                                            MinRValue[i] = CurrentPixel.R;
//                                        }

//                                        if (CurrentPixel.G > MaxGValue[i])
//                                        {
//                                            MaxGValue[i] = CurrentPixel.G;
//                                        }

//                                        else if (CurrentPixel.G < MinGValue[i])
//                                        {
//                                            MinGValue[i] = CurrentPixel.G;
//                                        }

//                                        if (CurrentPixel.B > MaxBValue[i])
//                                        {
//                                            MaxBValue[i] = CurrentPixel.B;
//                                        }

//                                        else if (CurrentPixel.B < MinBValue[i])
//                                        {
//                                            MinBValue[i] = CurrentPixel.B;
//                                        }

//                                        ++NumPixels[i];
//                                    }
//                                }
//                            }
//                        }
//                    }

//                    int j = 0;
//                    int MinDifference = 10000;

//                    for (int i = 0; i < 4; ++i)
//                    {
//                        int CurrentDifference = (MaxRValue[i] - MinRValue[i]) + (MaxGValue[i] - MinGValue[i]) + (MaxBValue[i] - MinBValue[i]);
//                        if (CurrentDifference < MinDifference && NumPixels[i] > 0)
//                        {
//                            j = i;
//                            MinDifference = CurrentDifference;
//                        }
//                    }

//                    //MessageBox.Show(
//                    //                      "NumPixels[j] = " + NumPixels[j].ToString() + "; " +
//                    //                      "RValues[j] = " + RValues[j].ToString() + "; " +
//                    //                      "GValues[j] = " + GValues[j].ToString() + "; " +
//                    //"BValues[j] = " + BValues[j].ToString() + "; " +
//                    //"(byte)255 = " + (byte)255 + "; "
//                    //                );

//                    ColorBgra MeanPixel = new ColorBgra();
//                    MeanPixel.R = NumPixels[j] == 0 ? (byte)RValues[j] : (byte)(RValues[j] / NumPixels[j]);
//                    MeanPixel.G = NumPixels[j] == 0 ? (byte)GValues[j] : (byte)(GValues[j] / NumPixels[j]);
//                    MeanPixel.B = NumPixels[j] == 0 ? (byte)BValues[j] : (byte)(BValues[j] / NumPixels[j]);
//                    MeanPixel.A = (byte)255;
//                    //NewBitmap.SetPixel(x, y, MeanPixel);

//                    // TODO: Add pixel processing code here
//                    // Access RGBA values this way, for example:
//                    // CurrentPixel.R = (byte)PrimaryColor.R;
//                    // CurrentPixel.G = (byte)PrimaryColor.G;
//                    // CurrentPixel.B = (byte)PrimaryColor.B;
//                    // CurrentPixel.A = (byte)PrimaryColor.A;
//                    dst[x, y] = MeanPixel;
//                }
//            }
//        }
//    }

//    //internal class ColorBgra
//    //{
//    //    public int R { get; set; }
//    //    public int G { get; set; }
//    //    public int B { get; set; }
//    //    public int A { get; set; }
//    //}

//    //internal static class EnvironmentParameters
//    //{
//    //    public static Selection GetSelection(Rectangle bounds)
//    //    {
//    //        return new Selection();
//    //    }

//    //    public static ColorBgra PrimaryColor { get; set; }

//    //    public static ColorBgra SecondaryColor { get; set; }

//    //    public static int BrushWidth { get; set; }
//    //}

//    //internal class Selection
//    //{
//    //    public Rectangle GetBoundsInt()
//    //    {
//    //        return new Rectangle();
//    //    }
//    //}

//    //internal class Rectangle
//    //{
//    //    public int Right { get; set; }
//    //    public int Left { get; set; }
//    //    public int Top { get; set; }
//    //    public int Bottom { get; set; }

//    //    public int Width { get; set; }

//    //    public double Height { get; set; }
//    //}

//    //internal class Surface
//    //{
//    //    public Rectangle Bounds { get; set; }

//    //    public ColorBgra this[int i, int i1]
//    //    {
//    //        get { return new ColorBgra(); }
//    //        set
//    //        {
//    //            ColorBgra colorBgra = new ColorBgra();
//    //            colorBgra = value;
//    //        }
//    //    }
//    //}
//}