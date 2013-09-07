using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace KuwaharaFilter
{
    public class EffectPlugin : PropertyBasedEffect
    {
        private struct Kolor
        {
            private double _r;
            private double _g;
            private double _b;

            public Kolor(double r, double g, double b)
            {
                _r = r;
                _g = g;
                _b = b;
            }
        }
        private class ValueSurface
        {
            public static int Width;
            public static int Height;
            private static Kolor _kolor;
            public static double operator+(ValueSurface left, double right)
            {
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        ValueSurface temp = new ValueSurface();
                        _kolor = new Kolor();
                        this[i,j].kolor = 
                    }
                    
                }
            }

            public ValueSurface this[int w, int h]
            {
                get
                {
                    Width = w;
                    Height = h;
                    return this;
                }
            }

            public Kolor kolor
            {
                get {
                    return _kolor;
                }
                set {
                    _kolor = value;
                }
            }
        }

        #region UICode
        //public int size = 20;
        #endregion

        public static string StaticName
        {
            get
            {
                return "Kuwahara Filter";
            }
        }

        public enum PropertyNames
        {
            Type,
            Aperture
        }

        public enum ChoiceList
        {
            Keep = 0,
            Primal = 1,
            BandW = 2
        }

        // public static Bitmap StaticIcon
        // {
        //     get
        //     {
        //         return new Bitmap(typeof(EffectPlugin), "EffectPluginIcon.png");
        //     }
        // }
        // 
        // Entire StaticIcon method (above) can be replaced by the line below referencing an icon (png) held in Resources
        //
        public static Bitmap StaticImage { get { return Properties.Resources.Icon; } }

        public static string StaticSubMenuName
        {
            get
            {
                //return null; // Use for no submenu
                return SubmenuNames.Artistic;  // Use for existing submenu
                // return "My SubMenu";     // Use for custom submenu
            }
        }

        public EffectPlugin()
            : base(EffectPlugin.StaticName, EffectPlugin.StaticImage, EffectPlugin.StaticSubMenuName, EffectFlags.Configurable)
        {

        }

        protected override PropertyCollection OnCreatePropertyCollection()
        {

            System.Collections.Generic.List<Property> props = new List<Property>(); //= new List<Property>();
            props.Add(StaticListChoiceProperty.CreateForEnum<ChoiceList>(PropertyNames.Type, ChoiceList.Primal, false));
            props.Add(new Int32Property(PropertyNames.Aperture, 25, 0, 50));
            //props.Add(StaticListChoiceProperty.CreateForEnum<Amount2Options>(PropertyNames.Amount2, 0, false));
            //props.Add(new BooleanProperty(PropertyNames.Amount3, false));
            return new PropertyCollection(props);
        }

        protected override ControlInfo OnCreateConfigUI(PropertyCollection props)
        {
            /*
             * Use this to have preview
            Surface sourceSurface = this.EnvironmentParameters.SourceSurface;
            Bitmap bitmap = sourceSurface.CreateAliasedBitmap();
            ImageResource imageResource = ImageResource.FromImage(bitmap);
            configUI.SetPropertyControlValue(PropertyNames.Center, ControlInfoPropertyNames.StaticImageUnderlay, imageResource);
            return configUI;
             */


            ControlInfo configUI = CreateDefaultConfigUI(props);

            //configUI.SetPropertyControlValue(PropertyNames.Type, ControlInfoPropertyNames.DisplayName, "Choose the color mode");

            //PropertyControlInfo pci = configUI.FindControlForPropertyName(PropertyNames.Type);
            //pci.SetValueDisplayName(ChoiceList.Keep, "Keep original colors");
            //pci.SetValueDisplayName(ChoiceList.Primal, "Reduce to primal colors");
            //pci.SetValueDisplayName(ChoiceList.BandW, "Reduce to black and white");
            //configUI.SetPropertyControlType(PropertyNames.Type, PropertyControlType.RadioButton);

            configUI.SetPropertyControlValue(PropertyNames.Aperture, ControlInfoPropertyNames.DisplayName, "Aperture size");
            configUI.SetPropertyControlValue(PropertyNames.Aperture, ControlInfoPropertyNames.SliderLargeChange, 5);
            configUI.SetPropertyControlValue(PropertyNames.Aperture, ControlInfoPropertyNames.SliderSmallChange, 1);
            configUI.SetPropertyControlValue(PropertyNames.Aperture, ControlInfoPropertyNames.UpDownIncrement, 1);

            //configUI.SetPropertyControlValue(PropertyNames.Tolerance, ControlInfoPropertyNames.DisplayName, "Color tolerance level");
            //configUI.SetPropertyControlValue(PropertyNames.Tolerance, ControlInfoPropertyNames.SliderLargeChange, 5);
            //configUI.SetPropertyControlValue(PropertyNames.Tolerance, ControlInfoPropertyNames.SliderSmallChange, 1);
            //configUI.SetPropertyControlValue(PropertyNames.Tolerance, ControlInfoPropertyNames.UpDownIncrement, 1);

            //configUI.SetPropertyControlValue(PropertyNames.Alpha, ControlInfoPropertyNames.DisplayName, "Include transparent pixels");
            //configUI.SetPropertyControlValue(PropertyNames.Alpha, ControlInfoPropertyNames.Description, "OFF:no ON:yes");

            return configUI;
        }

        private int Value1;
        private int Value2;
        private int Value3;
        private bool Value4;

        protected override void OnSetRenderInfo(PropertyBasedEffectConfigToken newToken, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            this.Value1 = (int)newToken.GetProperty<StaticListChoiceProperty>(PropertyNames.Type).Value;
            this.Value2 = newToken.GetProperty<Int32Property>(PropertyNames.Aperture).Value;
            //this.Value3 = newToken.GetProperty<Int32Property>(PropertyNames.Tolerance).Value;
            //this.Value4 = newToken.GetProperty<BooleanProperty>(PropertyNames.Alpha).Value;
            base.OnSetRenderInfo(newToken, dstArgs, srcArgs);
        }


        protected override void OnRender(Rectangle[] rois, int startIndex, int length)
        {
            for (int i = startIndex; i < startIndex + length; ++i)
            {
                Rectangle rect = rois[i];
                RenderRI(DstArgs, SrcArgs, rect);
            }
        }


        //public override EffectConfigDialog CreateConfigDialog()
        //{
        //    return new EffectPluginConfigDialog();
        //}

        public void RenderRI(RenderArgs dstArgs, RenderArgs srcArgs, Rectangle rois)
        {
            //EffectPluginConfigToken effectPluginConfigToken = (EffectPluginConfigToken)parameters;
            //int size = effectPluginConfigToken.ApertureSize;
            int size = Value2;
            PdnRegion selectionRegion = EnvironmentParameters.GetSelection(srcArgs.Bounds);

            // Delete any of these lines you don't need
            Rectangle selection = EnvironmentParameters.GetSelection(srcArgs.Bounds).GetBoundsInt();
            int CenterX = ((selection.Right - selection.Left) / 2) + selection.Left;
            int CenterY = ((selection.Bottom - selection.Top) / 2) + selection.Top;
            ColorBgra PrimaryColor = (ColorBgra)EnvironmentParameters.PrimaryColor;
            ColorBgra SecondaryColor = (ColorBgra)EnvironmentParameters.SecondaryColor;
            int BrushWidth = (int)EnvironmentParameters.BrushWidth;
            Random TempRandom = new Random();
            int[] ApetureMinX = { -(size / 2), 0, -(size / 2), 0 };
            int[] ApetureMaxX = { 0, (size / 2), 0, (size / 2) };
            int[] ApetureMinY = { -(size / 2), -(size / 2), 0, 0 };
            int[] ApetureMaxY = { 0, 0, (size / 2), (size / 2) };


            //ColorBgra CurrentPixel;
            //for (int w = startIndex; w < startIndex + length; ++w)
            //{
            /* Java source from original*/

            Rectangle roi = rois; //[w];
            int width = roi.Width;
            int height = roi.Height;
            int size2 = (size + 1) / 2;
            int offset = (size - 1) / 2;
            int width2 = srcArgs.Width + offset;
            int height2 = srcArgs.Height + offset;
            ValueSurface means = new ValueSurface { Width = width2, Height = height2 };

            //double[,] meanr = new double[width2, height2];
            double[,] variancer = new double[width2, height2];
            //double[,] meang = new double[width2, height2];
            double[,] varianceg = new double[width2, height2];
            //double[,] meanb = new double[width2, height2];
            double[,] varianceb = new double[width2, height2];
            int x1Start = roi.X;
            int y1Start = roi.Y;
            for (int y1 = y1Start - offset; y1 < y1Start + height; y1++)
            {
                //if ((y1%20)==0) IJ.showProgress(0.7*(y1-y1start)/height);
                for (int x1 = x1Start - offset; x1 < x1Start + width; x1++)
                {
                    double sumr = 0;
                    double sumr2 = 0;
                    double sumg = 0;
                    double sumg2 = 0;
                    double sumb = 0;
                    double sumb2 = 0;
                    int n = 0;
                    for (int x2 = x1; x2 < x1 + size2; x2++)
                    {
                        if (x2 < 0 || x2 >= srcArgs.Bounds.Right)
                        {
                            continue;
                        }

                        for (int y2 = y1; y2 < y1 + size2; y2++)
                        {
                            if (y2 < 0 || y2 >= srcArgs.Bounds.Bottom)
                            {
                                continue;
                            }
                            int vr = srcArgs.Surface[x2, y2].R;
                            sumr += vr;
                            sumr2 += vr * vr;
                            int vg = srcArgs.Surface[x2, y2].G;
                            sumg += vg;
                            sumg2 += vg * vg;
                            int vb = srcArgs.Surface[x2, y2].B;
                            sumb += vb;
                            sumb2 += vb * vb;
                            n++;
                        }
                    }
                    means[x1 + offset, y1 + offset].kolor = new Kolor(sumr / n, sumg / n, sumb / n);
                    //meanr[x1 + offset, y1 + offset] = sumr / n;
                    variancer[x1 + offset, y1 + offset] = sumr2 - sumr * sumr / n;
                    //meang[x1 + offset, y1 + offset] = sumg / n;
                    varianceg[x1 + offset, y1 + offset] = sumg2 - sumg * sumg / n;
                    //meanb[x1 + offset, y1 + offset] = sumb / n;
                    varianceb[x1 + offset, y1 + offset] = sumb2 - sumb * sumb / n;
                }
            }
            //new ImagePlus("Variance", new FloatProcessor(variance)).show(); // ImageJ 1.35b or later
            int xbase2 = 0;
            int ybase2 = 0;
            for (int y1 = y1Start; y1 < y1Start + height; y1++)
            {
                //if ((y1%20)==0) IJ.showProgress(0.7+0.3*(y1-y1start)/height);
                for (int x1 = x1Start; x1 < x1Start + width; x1++)
                {
                    double minr = double.MaxValue;
                    double ming = double.MaxValue;
                    double minb = double.MaxValue;
                    int xbase = x1; //double xbase;
                    int ybase = y1; //double ybase;
                    double varr = variancer[xbase, ybase];
                    double varg = varianceg[xbase, ybase];
                    double varb = varianceb[xbase, ybase];
                    if (varr < minr) { minr = varr; xbase2 = xbase; ybase2 = ybase; }
                    if (varg < ming) { ming = varg; xbase2 = xbase; ybase2 = ybase; }
                    if (varb < minb) { minb = varb; xbase2 = xbase; ybase2 = ybase; }
                    xbase = x1 + offset;
                    varr = variancer[xbase, ybase];
                    varg = varianceg[xbase, ybase];
                    varb = varianceb[xbase, ybase];
                    if (varr < minr) { minr = varr; xbase2 = xbase; ybase2 = ybase; }
                    if (varg < ming) { ming = varg; xbase2 = xbase; ybase2 = ybase; }
                    if (varb < minb) { minb = varb; xbase2 = xbase; ybase2 = ybase; }
                    ybase = y1 + offset;
                    varr = variancer[xbase, ybase];
                    varg = varianceg[xbase, ybase];
                    varb = varianceb[xbase, ybase];
                    if (varr < minr) { minr = varr; xbase2 = xbase; ybase2 = ybase; }
                    if (varg < ming) { ming = varg; xbase2 = xbase; ybase2 = ybase; }
                    if (varb < minb) { minb = varb; xbase2 = xbase; ybase2 = ybase; }
                    xbase = x1;
                    varr = variancer[xbase, ybase];
                    varg = varianceg[xbase, ybase];
                    varb = varianceb[xbase, ybase];
                    if (varr < minr) { minr = varr; xbase2 = xbase; ybase2 = ybase; }
                    if (varg < ming) { ming = varg; xbase2 = xbase; ybase2 = ybase; }
                    if (varb < minb) { minb = varb; xbase2 = xbase; ybase2 = ybase; }
                    ColorBgra tempColorBgra = new ColorBgra();
                    tempColorBgra = (byte) (means[xbase2, ybase2] + 0.5);
                    //tempColorBgra.R = (byte)(meanr[xbase2, ybase2] + 0.5);
                    tempColorBgra.G = (byte)(meang[xbase2, ybase2] + 0.5);
                    tempColorBgra.B = (byte)(meanb[xbase2, ybase2] + 0.5);
                    tempColorBgra.A = (byte)255;
                    dstArgs.Surface[x1, y1] = tempColorBgra;
                }
            }
            //IJ.showProgress(1.0);
            /* End of Java source */
            //    Rectangle rect = rois[w];

            //    for (int y = rect.Top; y < rect.Bottom; ++y)
            //    {
            //        for (int x = rect.Left; x < rect.Right; ++x)
            //        {


            //            //OutputDebugStringW("ApetureMinX[]: " + ApetureMinX[0] + "," +ApetureMinX[1] + "," +ApetureMinX[2] + "," +ApetureMinX[3]);
            //            //OutputDebugStringW("ApetureMaxX[]: " + ApetureMaxX[0] + "," + ApetureMaxX[1] + "," + ApetureMaxX[2] + "," + ApetureMaxX[3]);
            //            //OutputDebugStringW("ApetureMinY[]: " + ApetureMinY[0] + "," + ApetureMinY[1] + "," + ApetureMinY[2] + "," + ApetureMinY[3]);
            //            //OutputDebugStringW("ApetureMinX[]: " + ApetureMaxY[0] + "," + ApetureMaxY[1] + "," + ApetureMaxY[2] + "," + ApetureMaxY[3]);


            //            //for (int y = selection.Top; y < selection.Bottom; y++)
            //            //{
            //            //    //OutputDebugStringW("Y position: " + y);
            //            //    for (int x = selection.Left; x < selection.Right; x++)
            //            //    {
            //            CurrentPixel = srcArgs.Surface[x, y];
            //            ColorBgra NewPixel = new ColorBgra(); //dstArgs.Surface[x, y];
            //            int[] RValues = { 0, 0, 0, 0 };
            //            int[] GValues = { 0, 0, 0, 0 };
            //            int[] BValues = { 0, 0, 0, 0 };
            //            int[] NumPixels = { 0, 0, 0, 0 };
            //            int[] MaxRValue = { 0, 0, 0, 0 };
            //            int[] MaxGValue = { 0, 0, 0, 0 };
            //            int[] MaxBValue = { 0, 0, 0, 0 };
            //            int[] MinRValue = { 255, 255, 255, 255 };
            //            int[] MinGValue = { 255, 255, 255, 255 };
            //            int[] MinBValue = { 255, 255, 255, 255 };

            //            for (int i = 0; i < 4; ++i)
            //            {

            //                for (int x2 = ApetureMinX[i]; x2 < ApetureMaxX[i]; ++x2)
            //                {

            //                    //if (y == selection.Top && x == selection.Left)
            //                    //{
            //                    //    OutputDebugStringW("x2: " + x2 + ", x: " + x + ", i: " + i + ", ApetureMinX[i]: " + ApetureMinX[i] + ", ApetureMaxX[i]: " + ApetureMaxX[i]);
            //                    //}
            //                    int TempX = x + x2;

            //                    //                MessageBox.Show(
            //                    //"y = " + y.ToString() + "; " +
            //                    //"x = " + x.ToString() + "; " +
            //                    //"i = " + i.ToString() + "; " +
            //                    //"x2 = " + x2.ToString() + "; " +
            //                    //"ApetureMinX[i] = " + ApetureMinX[i].ToString() + "; " +
            //                    //"ApetureMaxX[i] = " + ApetureMaxX[i].ToString() + "; " +
            //                    //                      "TempX = " + TempX.ToString() + "; " +
            //                    //                      "selection.Width = " + selection.Width.ToString() + "; "
            //                    //                );
            //                    if (TempX >= 0 && TempX < selection.Left + selection.Width)
            //                    {

            //                        for (int y2 = ApetureMinY[i]; y2 < ApetureMaxY[i]; ++y2)
            //                        {
            //                            int TempY = y + y2;

            //                            //OutputDebugStringW("TempY = " + TempY.ToString() + "; " +
            //                            //    "selection.Height = " + selection.Height.ToString() + "; "
            //                            //    );

            //                            if (TempY >= 0 && TempY < selection.Bottom + selection.Height)
            //                            {
            //                                //Color TempColor = src.GetPixel(TempX, TempY);
            //                                //MessageBox.Show(
            //                                //                      "R = " + CurrentPixel.R.ToString() + "; " +
            //                                //                      "G = " + CurrentPixel.G.ToString() + "; " +
            //                                //                      "B = " + CurrentPixel.B.ToString() + "; " +
            //                                //"MaxRValue[i] = " + MaxRValue[i].ToString() + "; " +
            //                                //"MinRValue[i] = " + MinRValue[i].ToString() + "; " +
            //                                //"MaxGValue[i] = " + MaxGValue[i].ToString() + "; " +
            //                                //"MinGValue[i] = " + MinGValue[i].ToString() + "; " +
            //                                //"MaxBValue[i] = " + MaxBValue[i].ToString() + "; " +
            //                                //"MinBValue[i] = " + MinBValue[i].ToString() + "; "
            //                                //                );

            //                                //Color TempColor = TempBitmap.GetPixel(TempX, TempY);
            //                                NewPixel = srcArgs.Surface[x, y];
            //                                RValues[i] += NewPixel.R; //TempColor.R;
            //                                GValues[i] += NewPixel.G; //TempColor.G;
            //                                BValues[i] += NewPixel.B; //TempColor.B;

            //                                if (NewPixel.R > MaxRValue[i])
            //                                {
            //                                    MaxRValue[i] = NewPixel.R;
            //                                }

            //                                else if (NewPixel.R < MinRValue[i])
            //                                {
            //                                    MinRValue[i] = NewPixel.R;
            //                                }

            //                                if (NewPixel.G > MaxGValue[i])
            //                                {
            //                                    MaxGValue[i] = NewPixel.G;
            //                                }

            //                                else if (NewPixel.G < MinGValue[i])
            //                                {
            //                                    MinGValue[i] = NewPixel.G;
            //                                }

            //                                if (NewPixel.B > MaxBValue[i])
            //                                {
            //                                    MaxBValue[i] = NewPixel.B;
            //                                }

            //                                else if (NewPixel.B < MinBValue[i])
            //                                {
            //                                    MinBValue[i] = NewPixel.B;
            //                                }

            //                                ++NumPixels[i];
            //                            }
            //                        }
            //                    }
            //                }
            //            }

            //            int j = 0;
            //            int MinDifference = 10000;

            //            for (int i = 0; i < 4; ++i)
            //            {
            //                int CurrentDifference = (MaxRValue[i] - MinRValue[i]) + (MaxGValue[i] - MinGValue[i]) + (MaxBValue[i] - MinBValue[i]);
            //                if (CurrentDifference < MinDifference && NumPixels[i] > 0)
            //                {
            //                    j = i;
            //                    MinDifference = CurrentDifference;
            //                }
            //            }

            //            //MessageBox.Show(
            //            //                      "NumPixels[j] = " + NumPixels[j].ToString() + "; " +
            //            //                      "RValues[j] = " + RValues[j].ToString() + "; " +
            //            //                      "GValues[j] = " + GValues[j].ToString() + "; " +
            //            //"BValues[j] = " + BValues[j].ToString() + "; " +
            //            //"(byte)255 = " + (byte)255 + "; "
            //            //                );

            //            ColorBgra MeanPixel = new ColorBgra();
            //            MeanPixel.R = NumPixels[j] == 0 ? (byte)RValues[j] : (byte)(RValues[j] / NumPixels[j]);
            //            MeanPixel.G = NumPixels[j] == 0 ? (byte)GValues[j] : (byte)(GValues[j] / NumPixels[j]);
            //            MeanPixel.B = NumPixels[j] == 0 ? (byte)BValues[j] : (byte)(BValues[j] / NumPixels[j]);
            //            MeanPixel.A = (byte)255;
            //            //NewBitmap.SetPixel(x, y, MeanPixel);

            //            // TODO: Add pixel processing code here
            //            // Access RGBA values this way, for example:
            //            // CurrentPixel.R = (byte)PrimaryColor.R;
            //            // CurrentPixel.G = (byte)PrimaryColor.G;
            //            // CurrentPixel.B = (byte)PrimaryColor.B;
            //            // CurrentPixel.A = (byte)PrimaryColor.A;
            //            dstArgs.Surface[x, y] = MeanPixel;
            //            //    }
            //            //}

            //        }
            //    }
            //}
        }

    }
}