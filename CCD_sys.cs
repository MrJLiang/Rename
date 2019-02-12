using HalconDotNet;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace HAAGONtest
{
    internal class CCD_sys
    {
        Enum_sys.CCDMode CCDRunMode;
        string ModelName, Size, X, Y;
        string ModelPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/ATL-BATTERY/Model/";
        string LogPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/ATL-BATTERY/日志/";

        /// <summary>
        /// CCD处理结束发送数控事件
        /// </summary>
        /// <param name="sender"></param>
        public delegate void CCDProcessEvent(object sender, SendData e);

        public event CCDProcessEvent CCDResultEvent;

        void SendCCDResult(SendData e)
        {
            //定义一个局部变量，将事件对象赋值给该变量，防止多线程情况下取消事件
            CCDProcessEvent mEvent = CCDResultEvent;
            if (mEvent != null)
            {
                mEvent(this, e);
            }
        }

        //baumer virables
        BGAPI2.SystemList systemList = null;

        BGAPI2.System mSystem = null;
        BGAPI2.InterfaceList interfaceList = null;
        BGAPI2.Interface mInterface = null;
        BGAPI2.DeviceList deviceList = null;
        BGAPI2.Device mDevice = null;
        BGAPI2.DataStreamList datastreamList = null;
        BGAPI2.DataStream mDataStream = null;
        BGAPI2.BufferList bufferList = null;
        BGAPI2.Buffer mBuffer = null;
        BGAPI2.ImageProcessor imgProcessor = null;
        BGAPI2.Image mImage = null;
        BGAPI2.Image mTransformImage = null;

        public CCD_sys()
        {
            CreatHomMat2D();
        }
        HTuple TranHomMat2D, hv_dx, hv_dy;
        private void CreatHomMat2D()
        {
            HTuple hv_RowRe = new HTuple();
            hv_RowRe[0] = 394.778;
            hv_RowRe[1] = 561.193;
            hv_RowRe[2] = 763.07;
            hv_RowRe[3] = 785.359;
            hv_RowRe[4] = 1001.09;
            hv_RowRe[5] = 1107.62;
            HTuple hv_ColumnRe = new HTuple();
            hv_ColumnRe[0] = 877.163;
            hv_ColumnRe[1] = 1080.3;
            hv_ColumnRe[2] = 1272.22;
            hv_ColumnRe[3] = 842.656;
            hv_ColumnRe[4] = 1070.13;
            hv_ColumnRe[5] = 828.77;



            HTuple hv_x = new HTuple();
            hv_x[0] = -459.452;
            hv_x[1] = -450.075;
            hv_x[2] = -438.673;
            hv_x[3] = -436.990;
            hv_x[4] = -424.788;
            hv_x[5] = -418.380;
            HTuple hv_y = new HTuple();
            hv_y[0] = 2.293;
            hv_y[1] = 14.318;
            hv_y[2] = 25.591;
            hv_y[3] = 0.931;
            hv_y[4] = 14.226;
            hv_y[5] = 0.546;

            HOperatorSet.VectorToHomMat2d(hv_RowRe, hv_ColumnRe, hv_x, hv_y, out TranHomMat2D);

            hv_dx = new HTuple();
            hv_dy = new HTuple();
            hv_dx = -0.471743;
            hv_dy = -0.470538;
        }

        /// <summary>
        /// CCD初始化
        /// </summary>          
        public void CCDInit()
        {
            BaumerInit();
        }

        private void BaumerInit()
        {
            systemList = BGAPI2.SystemList.Instance;
            systemList.Refresh();
            mSystem = systemList.Values.First();         //gige: Values.First() or Values.ElementAt(0);
                                                         //mSystem = systemList.Values.ElementAt(1);  //usb3: Values.Last()  or Values.ElementAt(1);
            mSystem.Open();

            interfaceList = mSystem.Interfaces;
            interfaceList.Refresh(100);
            // mInterface = interfaceList.Values.First();      //first interface:  Values.First() or Values.ElementAt(0);
            mInterface = interfaceList.Values.ElementAt(0); //second interface: Values.ElementAt(1);
                                                            //mInterface = interfaceList.Values.ElementAt(2); //third interface:  Values.ElementAt(2);
            mInterface.Open();

            deviceList = mInterface.Devices;
            deviceList.Refresh(100);

            if (deviceList.Count > 0)
            {
                mDevice = deviceList.Values.First();
                mDevice.Open();

                //1. Software TriggerMode
                mDevice.RemoteNodeList["TriggerSource"].Value = "Software";

                mDevice.RemoteNodeList["TriggerMode"].Value = "On";


                // 2. SET EXPOSURE TIME
                mDevice.RemoteNodeList["ExposureTime"].Value = 10000.0;


                //String sExposureNodeName = "";
                //if (mDevice.GetRemoteNodeList().GetNodePresent("ExposureTime"))
                //{
                //    sExposureNodeName = "ExposureTime";
                //}
                //else if (mDevice.GetRemoteNodeList().GetNodePresent("ExposureTimeAbs"))
                //{
                //    sExposureNodeName = "ExposureTimeAbs";
                //}
                //mDevice.RemoteNodeList[sExposureNodeName].Value = 100.0;

                datastreamList = mDevice.DataStreams;
                datastreamList.Refresh();
                mDataStream = datastreamList.Values.ElementAt(0);
                mDataStream.Open();

                bufferList = mDataStream.BufferList;
                for (int i = 0; i < 4; i++)
                {
                    mBuffer = new BGAPI2.Buffer();
                    bufferList.Add(mBuffer);
                    mBuffer.QueueBuffer();
                }
                //软触事件
                mDataStream.NewBufferEvent += new BGAPI2.Events.DataStreamEventControl.NewBufferEventHandler(mDataStream_NewBufferEvent);
                mDataStream.RegisterNewBufferEvent(BGAPI2.Events.EventMode.EVENT_HANDLER);

                mDataStream.StartAcquisition();
                mDevice.RemoteNodeList["AcquisitionStart"].Execute();
            }
            else
            {
                throw new NotImplementedException();
            }

            //很重要
            imgProcessor = BGAPI2.ImageProcessor.Instance;
        }


        public HTuple ModelWindowHandle;

        /// <summary>
        /// CCD窗口控件初始化
        /// </summary>
        /// <param name="Window"></param>

        public void InitCCDModelWindow(System.Windows.Forms.PictureBox Window)
        {
            HTuple Fatherwindow = Window.Handle;
            //设置窗口颜色
            HOperatorSet.SetWindowAttr("background_color", "black");

            //PictureBox控件转换为halcon图像格式的控件
            HOperatorSet.OpenWindow(0, 0, Window.Width, Window.Height, Fatherwindow, "visible", "", out ModelWindowHandle);
        }

        /// <summary>
        /// CCD启动
        /// </summary>
        public void CCDExcuteStart(string _ModelName)
        {
            CCDRunMode = Enum_sys.CCDMode.图像处理;
            ModelName = _ModelName;

            mDevice.RemoteNodeList["TriggerSoftware"].Execute();
        }

        /// <summary>
        /// 创建CCD模板
        /// </summary>
        /// <param name="ModelName"></param>
        public void CCDCreateStart(string _ModelName)
        {
            CCDRunMode = Enum_sys.CCDMode.创建模板;
            ModelName = _ModelName;

            mDevice.RemoteNodeList["TriggerSoftware"].Execute();
        }

        /// <summary>
        /// 量块标定
        /// </summary>
        /// <param name="_ModelName"></param>
        /// <param name="_Size"></param>
        public void CCDcalibrationStart(string _Size)
        {
            CCDRunMode = Enum_sys.CCDMode.量块标定;

            Size = _Size;

            mDevice.RemoteNodeList["TriggerSoftware"].Execute();
        }

        /// <summary>
        /// 定位参照
        /// </summary>
        /// <param name="_ModelName"></param>
        /// <param name="_X"></param>
        /// <param name="_Y"></param>
        public void CCDOffsetStart(string _ModelName, string _X, string _Y)
        {
            CCDRunMode = Enum_sys.CCDMode.定位参照;
            ModelName = _ModelName;
            X = _X;
            Y = _Y;

            mDevice.RemoteNodeList["TriggerSoftware"].Execute();
        }

        public void CCDSetFocus()
        {
            CCDRunMode = Enum_sys.CCDMode.调整焦距;
            mDevice.RemoteNodeList["TriggerSoftware"].Execute();
        }

        public void CCDFitCoord(string _ModelName)
        {
            ModelName = _ModelName;
            CCDRunMode = Enum_sys.CCDMode.定位补偿;
            mDevice.RemoteNodeList["TriggerSoftware"].Execute();
        }



        /// <summary>
        /// CCD关闭
        /// </summary>
        public void CCDClose()
        {
            try
            {
                mDevice.RemoteNodeList["AcquisitionAbort"].Execute();
                mDevice.RemoteNodeList["AcquisitionStop"].Execute();
                mDataStream.StopAcquisition();

                bufferList.DiscardAllBuffers();
                while (bufferList.Count > 0)
                {
                    mBuffer = (BGAPI2.Buffer)bufferList.Values.First();
                    bufferList.RevokeBuffer(mBuffer);
                }
                mDataStream.Close();
                mDevice.Close();
                mInterface.Close();
                mSystem.Close();
            }
            catch
            {
                return;

            }

        }

        /// <summary>
        /// CCD采集事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mDSEvent"></param>
        private void mDataStream_NewBufferEvent(object sender, BGAPI2.Events.NewBufferEventArgs mDSEvent)
        {
            try
            {
                BGAPI2.Buffer mBufferFilled = null;
                mBufferFilled = mDSEvent.BufferObj;
                if (mBufferFilled == null)
                {
                    System.Console.Write("Error: Buffer Timeout after 1000 msec\r\n");
                }
                else if (mBufferFilled.IsIncomplete == true)
                {
                    System.Console.Write("Error: Image is incomplete\r\n");
                    // queue buffer again
                    mBufferFilled.QueueBuffer();
                }
                else
                {
                    ImageTranHalcon(mBufferFilled);

                    //   System.Console.Write(" Image {0, 5:d} received in memory address {1:X}\r\n", mBufferFilled.FrameID, (ulong)mBufferFilled.MemPtr);
                    // queue buffer again
                    mBufferFilled.QueueBuffer();
                }
            }
            catch
            {
            }
            return;
        }

        /// <summary>
        /// CCD转换Halcon
        /// </summary>
        /// <param name="objIFrameData"></param>
        private void ImageTranHalcon(BGAPI2.Buffer objIFrameData)
        {
            //system中的变量句柄
            IntPtr pBufferMono = IntPtr.Zero;
            //halconSDK变量
            HObject halcon_image = null;
            //按照大恒相机帧信息的宽高生成数组指针

            byte[] m_pImageData = new byte[(int)objIFrameData.Width * (int)objIFrameData.Height];

            try
            {
                mImage = imgProcessor.CreateImage((uint)objIFrameData.Width, (uint)objIFrameData.Height, (string)objIFrameData.PixelFormat, objIFrameData.MemPtr, (ulong)objIFrameData.MemSize);

                mTransformImage = mImage.TransformImage("Mono8");
                //大恒SDK变量转换为system的变量
                pBufferMono = mTransformImage.Buffer;

                //复制大恒相机帧数据到m_pImageData指针
                Marshal.Copy(pBufferMono, m_pImageData, 0, (int)objIFrameData.Width * (int)objIFrameData.Height);

                unsafe//锁帧存操作
                {
                    fixed (byte* p = m_pImageData)
                    {
                        //halcon的SDK自带指针转换成图形变量的函数
                        HOperatorSet.GenImage1(out halcon_image, "byte", (int)objIFrameData.Width, (int)objIFrameData.Height, new IntPtr(p));
                    }
                }
                //图像翻转90度
                HOperatorSet.RotateImage(halcon_image, out halcon_image, 90, "constant");
                HOperatorSet.MedianImage(halcon_image, out halcon_image, "circle", 3, "mirrored");

                //图像显示
                CCDProcessMode(halcon_image);
                halcon_image.Dispose();
            }
            catch
            {
                halcon_image.Dispose();
            }
        }

        /// <summary>
        /// CCD处理模式
        /// </summary>
        /// <param name="Image"></param>
        private void CCDProcessMode(HObject Image)
        {
            switch (CCDRunMode)
            {
                case Enum_sys.CCDMode.创建模板:
                    CreatModel(Image);
                    break;

                case Enum_sys.CCDMode.量块标定:
                    CreatCilabration(Image);
                    break;

                case Enum_sys.CCDMode.定位参照:
                    SetOffset(Image);
                    break;

                case Enum_sys.CCDMode.图像处理:
                    if (Mod_sys.Instance.gfrmAutoRun.radioIS.Checked)
                    {
                        Execute有极耳(Image);
                    }
                    else
                    {
                        Execute没极耳(Image);
                    }

                    break;

                case Enum_sys.CCDMode.调整焦距:
                    Setfocus(Image);
                    break;

                case Enum_sys.CCDMode.定位补偿:
                    FitCoordination(Image);
                    break;

                default:
                    break;
            }
            Image.Dispose();
        }

        //***********************************图像处理方法*********************************//

        /// <summary>
        /// 创建CCD模板
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="ModelName"></param>
        private void CreatModel(HObject Image)
        {
            HTuple Width = null, Height = null;
            HTuple RecRow1 = null, RecColumn1 = null, RecRow2 = null, RecColumn2 = null;
            HTuple NCCModelId = null;
            HTuple RetangleRow = null, RetangleCol = null, RetangleArea = null;
            HTuple SideRowBegin = null, SideColumnBegin = null, SideRowEnd = null, SideColumnEnd = null;
            HTuple SideLine = null, MetrologyLine = null;
            HTuple MetrologyHandle = null;


            HObject ImageReduced = null;
            HObject Rectangle = null;
            HObject ThresholdRegion = null;

            HObject RectangleCross = null;

            HOperatorSet.GenEmptyObj(out ImageReduced);
            HOperatorSet.GenEmptyObj(out Rectangle);
            HOperatorSet.GenEmptyObj(out ThresholdRegion);

            HOperatorSet.GenEmptyObj(out RectangleCross);

            try
            {
                HOperatorSet.SetDraw(ModelWindowHandle, "margin");
                HOperatorSet.SetColor(ModelWindowHandle, "red");

                //Image Acquisition 01: Do something
                HOperatorSet.GetImageSize(Image, out Width, out Height);
                HOperatorSet.SetPart(ModelWindowHandle, 0, 0, Height - 1, Width - 1);
                HOperatorSet.DispObj(Image, ModelWindowHandle);

                //*********************创建极耳模板******************//

                MessageBox.Show("在图像上框选极耳模板，按鼠标右键确认!", "创建极耳模板");

                HOperatorSet.DrawRectangle1(ModelWindowHandle, out RecRow1, out RecColumn1,
                    out RecRow2, out RecColumn2);

                HOperatorSet.GenRectangle1(out Rectangle, RecRow1, RecColumn1, RecRow2, RecColumn2);


                HOperatorSet.WriteRegion(Rectangle, ModelPath + ModelName + "RectangleEar.hobj");
                //*********************创建边角定位模板******************//
                MessageBox.Show("在电芯右下角框选边角定位模板，按鼠标右键确认!", "创建边角定位模板");

                HOperatorSet.DrawRectangle1(ModelWindowHandle, out RecRow1, out RecColumn1,
                    out RecRow2, out RecColumn2);

                HOperatorSet.GenRectangle1(out RectangleCross, RecRow1, RecColumn1, RecRow2, RecColumn2);

                HOperatorSet.ReduceDomain(Image, RectangleCross, out ImageReduced);

                HOperatorSet.CreateNccModel(ImageReduced, "auto", (new HTuple(-40)).TupleRad()
                , (new HTuple(80)).TupleRad(), "auto", "use_polarity", out NCCModelId);


                HOperatorSet.WriteNccModel(NCCModelId, ModelPath + ModelName + "NCC.ncm");
                HOperatorSet.WriteRegion(RectangleCross, ModelPath + ModelName + "RectangleCross.hobj");
                //*********************创建电芯两侧ROI******************//
                MessageBox.Show("在图像上框选电芯两侧ROI，按鼠标右键确认!", "创建电芯两侧ROI");

                HOperatorSet.DrawRectangle1(ModelWindowHandle, out RecRow1, out RecColumn1,
                    out RecRow2, out RecColumn2);

                HOperatorSet.GenRectangle1(out Rectangle, RecRow1, RecColumn1, RecRow2, RecColumn2);


                HOperatorSet.WriteRegion(Rectangle, ModelPath + ModelName + "RectangleSide.hobj");

                //*********************创建电芯顶部ROI******************//
                MessageBox.Show("在图像上框选电芯顶部ROI，按鼠标右键确认!", "创建电芯顶部ROI");

                HOperatorSet.DrawRectangle1(ModelWindowHandle, out RecRow1, out RecColumn1,
                    out RecRow2, out RecColumn2);

                HOperatorSet.GenRectangle1(out Rectangle, RecRow1, RecColumn1, RecRow2, RecColumn2);


                HOperatorSet.WriteRegion(Rectangle, ModelPath + ModelName + "RectangleUp.hobj");

                //*********************创建电芯底部ROI******************//
                MessageBox.Show("在图像上框选电芯底部ROI，按鼠标右键确认!", "创建电芯底部ROI");

                HOperatorSet.DrawRectangle1(ModelWindowHandle, out RecRow1, out RecColumn1,
                    out RecRow2, out RecColumn2);

                HOperatorSet.GenRectangle1(out Rectangle, RecRow1, RecColumn1, RecRow2, RecColumn2);


                HOperatorSet.WriteRegion(Rectangle, ModelPath + ModelName + "RectangleDown.hobj");

                //*******************创建基准线********************//

                MessageBox.Show("从上往下绘制电芯基准边缘，按鼠标右键确认!", "创建基准边缘");

                //侧边测量直线
                HOperatorSet.DrawLine(ModelWindowHandle, out SideRowBegin, out SideColumnBegin,
                    out SideRowEnd, out SideColumnEnd);
                SideLine = new HTuple();
                SideLine = SideLine.TupleConcat(SideRowBegin);
                SideLine = SideLine.TupleConcat(SideColumnBegin);
                SideLine = SideLine.TupleConcat(SideRowEnd);
                SideLine = SideLine.TupleConcat(SideColumnEnd);

                HOperatorSet.AreaCenter(RectangleCross, out RetangleArea, out RetangleRow, out RetangleCol);


                HOperatorSet.GetImageSize(Image, out Width, out Height);
                //创建测量模型
                HOperatorSet.CreateMetrologyModel(out MetrologyHandle);
                //设置测量对象的图像大小
                HOperatorSet.SetMetrologyModelImageSize(MetrologyHandle, Width, Height);

                HOperatorSet.AddMetrologyObjectGeneric(MetrologyHandle, "line", SideLine, 200, 5, 1, 30, new HTuple(), new HTuple(), out MetrologyLine);

                HOperatorSet.SetMetrologyModelParam(MetrologyHandle, "reference_system",
                    ((RetangleRow.TupleConcat(RetangleCol))).TupleConcat(0));

                HOperatorSet.WriteMetrologyModel(MetrologyHandle, ModelPath + ModelName + ".mtr");



                HTuple RegionArea = null, RegionRow = null, RegionCol = null, UsedThreshold = null;
                HOperatorSet.BinaryThreshold(Image, out ThresholdRegion, "max_separability", "dark", out UsedThreshold);
                HOperatorSet.Connection(ThresholdRegion, out ThresholdRegion);
                HOperatorSet.SelectShapeStd(ThresholdRegion, out ThresholdRegion, "max_area", 70);
                HOperatorSet.AreaCenter(ThresholdRegion, out RegionArea, out RegionRow, out RegionCol);
                HOperatorSet.WriteTuple(RegionArea, ModelPath + ModelName + "RegionInfo.tup");

                MessageBox.Show("电芯模板创建成功!", "创建成功");
                Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text = ModelName;
            }
            catch
            {

                MessageBox.Show("电芯模板创建失败!", "创建失败");
            }

            Image.Dispose();
            Rectangle.Dispose();
            ImageReduced.Dispose();
            ThresholdRegion.Dispose();
            RectangleCross.Dispose();
            HOperatorSet.ClearNccModel(NCCModelId);
            HOperatorSet.ClearMetrologyModel(MetrologyHandle);


        }

        /// <summary>
        /// 量块标定
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="_ModelName"></param>
        /// <param name="Size"></param>
        private void CreatCilabration(HObject Image)
        {
            HTuple Width = null, Height = null;
            HTuple RecRow1 = null, RecColumn1 = null, RecRow2 = null, RecColumn2 = null;
            HTuple RowBegin = null, ColBegin, RowEnd = null, ColEnd = null, Nr = null, Nc = null, Dist = null;
            HTuple Row2 = null, Col2 = null, Distance = null, ratio = null;

            HObject Rectangle = null;
            HObject ImageReduced = null;
            HObject Edges = null, ContoursXld = null, SelectedXLD1 = null, SelectedXLD2 = null;

            HOperatorSet.GenEmptyObj(out Rectangle);
            HOperatorSet.GenEmptyObj(out ImageReduced);
            HOperatorSet.GenEmptyObj(out Edges);
            HOperatorSet.GenEmptyObj(out ContoursXld);
            HOperatorSet.GenEmptyObj(out SelectedXLD1);
            HOperatorSet.GenEmptyObj(out SelectedXLD2);

            try
            {
                //Image Acquisition 01: Do something
                HOperatorSet.GetImageSize(Image, out Width, out Height);
                HOperatorSet.SetPart(ModelWindowHandle, 0, 0, Height - 1, Width - 1);
                HOperatorSet.DispObj(Image, ModelWindowHandle);

                HOperatorSet.SetColor(ModelWindowHandle, "red");
                HOperatorSet.SetDraw(ModelWindowHandle, "margin");

                MessageBox.Show("在图像上框选量块的宽度尺寸，按鼠标右键确认!", "创建尺寸");

                HOperatorSet.DrawRectangle1(ModelWindowHandle, out RecRow1, out RecColumn1,
                 out RecRow2, out RecColumn2);

                HOperatorSet.GenRectangle1(out Rectangle, RecRow1, RecColumn1, RecRow2, RecColumn2);

                HOperatorSet.ReduceDomain(Image, Rectangle, out ImageReduced);

                HOperatorSet.EdgesSubPix(ImageReduced, out Edges, "canny", 1, 20,
                    40);

                HOperatorSet.SelectShapeXld(Edges, out ContoursXld, "contlength",
                    "and", 100, 500);

                HOperatorSet.SelectObj(ContoursXld, out SelectedXLD1, 1);
                HOperatorSet.FitLineContourXld(SelectedXLD1, "tukey", -1, 0, 5, 2,
                    out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr,
                    out Nc, out Dist);

                HOperatorSet.SelectObj(ContoursXld, out SelectedXLD2, 2);
                HOperatorSet.GetContourXld(SelectedXLD2, out Row2, out Col2);

                HOperatorSet.DistancePl(Row2, Col2, RowBegin, ColBegin, RowEnd, ColEnd, out Distance);

                Distance = Distance.TupleMean();

                double Num = Convert.ToDouble(Size);
                ratio = Distance / Num;

                HOperatorSet.WriteTuple(ratio, ModelPath + "ratio.tup");

                MessageBox.Show("量块标定成功，请返回操作界面!", "标定成功");

                Rectangle.Dispose();
                ImageReduced.Dispose();
                Edges.Dispose();
                ContoursXld.Dispose();
                SelectedXLD1.Dispose();
                SelectedXLD2.Dispose();
            }
            catch
            {
                Rectangle.Dispose();
                ImageReduced.Dispose();
                Edges.Dispose();
                ContoursXld.Dispose();
                SelectedXLD1.Dispose();
                SelectedXLD2.Dispose();
            }
        }

        /// <summary>
        /// 吸取坐标偏移
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="ModelName"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        private void SetOffset(HObject Image)
        {
            HTuple Width = null, Height = null;
            HTuple MatchingRow = null, MatchingCol = null, MatchingAngle = null;

            HTuple RetangleArea = null, RetangleRow = null, RetangleColumn = null;
            HTuple MatchingScore = null, NCCModelId = null, RegionHomMat2D = null;
            HTuple ratio = null, DegMean = null, Rad = null, dh = null, dv = null, dx = null, dy = null;
            double Xoffset = Convert.ToDouble(X);
            double Yoffset = Convert.ToDouble(Y);
            HTuple IsOverlapping = null, CrossRow1 = null, CrossColumn1 = null, CrossRow2 = null, CrossColumn2 = null;
            HTuple Deg1 = null, Deg2 = null;
            HTuple row = null, Col = null, phi = null, L1 = null, L2 = null;

            HTuple RowBegin1 = null, ColBegin1 = null, RowEnd1 = null, ColEnd1 = null, Nr1 = null, Nc1 = null, Dist1 = null;
            HTuple RowBegin2 = null, ColBegin2 = null, RowEnd2 = null, ColEnd2 = null, Nr2 = null, Nc2 = null, Dist2 = null;
            HTuple RowBegin3 = null, ColBegin3 = null, RowEnd3 = null, ColEnd3 = null, Nr3 = null, Nc3 = null, Dist3 = null;
            HTuple RowBegin4 = null, ColBegin4 = null, RowEnd4 = null, ColEnd4 = null, Nr4 = null, Nc4 = null, Dist4 = null;



            HObject ResultCross = null;
            HObject RectangleCross = null;

            HObject RectangleSide = null, RectangleUp = null, RectangleDown = null, ImageReduced = null, Edges = null, SelectedXLD1 = null, SelectedXLD2 = null;


            HOperatorSet.GenEmptyObj(out RectangleSide);
            HOperatorSet.GenEmptyObj(out RectangleUp);
            HOperatorSet.GenEmptyObj(out RectangleDown);
            HOperatorSet.GenEmptyObj(out ImageReduced);
            HOperatorSet.GenEmptyObj(out Edges);
            HOperatorSet.GenEmptyObj(out SelectedXLD1);
            HOperatorSet.GenEmptyObj(out SelectedXLD2);
            HOperatorSet.GenEmptyObj(out ResultCross);
            HOperatorSet.GenEmptyObj(out RectangleCross);

            HOperatorSet.SetDraw(ModelWindowHandle, "margin");

            HOperatorSet.ReadNccModel(ModelPath + ModelName + "NCC.ncm", out NCCModelId);
            HOperatorSet.ReadTuple(ModelPath + "ratio.tup", out ratio);
            HOperatorSet.ReadRegion(out RectangleCross, ModelPath + ModelName + "RectangleCross.hobj");

            HOperatorSet.ReadRegion(out RectangleSide, ModelPath + ModelName + "RectangleSide.hobj");
            HOperatorSet.ReadRegion(out RectangleUp, ModelPath + ModelName + "RectangleUp.hobj");
            HOperatorSet.ReadRegion(out RectangleDown, ModelPath + ModelName + "RectangleDown.hobj");


            //Image Acquisition 01: Do something
            HOperatorSet.GetImageSize(Image, out Width, out Height);
            HOperatorSet.SetPart(ModelWindowHandle, 0, 0, Height - 1, Width - 1);
            HOperatorSet.DispObj(Image, ModelWindowHandle);

            HOperatorSet.FindNccModel(Image, NCCModelId, (new HTuple(-40)).TupleRad()
           , (new HTuple(80)).TupleRad(), 0.9, 1, 0.5, "true", 0, out MatchingRow, out MatchingCol,
           out MatchingAngle, out MatchingScore);

            if ((int)(MatchingScore.TupleLength()) == 1)
            {
                try
                {
                    HOperatorSet.AreaCenter(RectangleCross, out RetangleArea, out RetangleRow, out RetangleColumn);

                    HOperatorSet.VectorAngleToRigid(RetangleRow, RetangleColumn, 0, MatchingRow, MatchingCol, MatchingAngle, out RegionHomMat2D);

                    HOperatorSet.AffineTransRegion(RectangleSide, out RectangleSide, RegionHomMat2D, "nearest_neighbor");
                    HOperatorSet.AffineTransRegion(RectangleUp, out RectangleUp, RegionHomMat2D, "nearest_neighbor");
                    HOperatorSet.AffineTransRegion(RectangleDown, out RectangleDown, RegionHomMat2D, "nearest_neighbor");

                    //*******************获取侧边边缘线********************//
                    HOperatorSet.ReduceDomain(Image, RectangleSide, out ImageReduced);
                    HOperatorSet.GrayClosingRect(ImageReduced, out ImageReduced, 11, 3);
                    HOperatorSet.Emphasize(ImageReduced, out ImageReduced, 7, 7, 1);

                    HOperatorSet.EdgesSubPix(ImageReduced, out Edges, "canny", 4, 20,
                        40);

                    HOperatorSet.SmallestRectangle2(RectangleSide, out row, out Col, out phi, out L1, out L2);

                    HOperatorSet.SelectShapeXld(Edges, out Edges, "contlength",
                     "and", L2 * 2 * 0.1, L2 * 3);
                    HOperatorSet.UnionCollinearContoursXld(Edges, out Edges, L2 * 2 * 0.4, L2 * 2 * 0.2, 10, 0.1, "attr_keep");
                    HOperatorSet.SelectShapeXld(Edges, out Edges, "contlength",
                     "and", L2 * 2 * 0.5, L2 * 3);

                    HOperatorSet.SetColor(ModelWindowHandle, "red");
                    HOperatorSet.DispObj(Edges, ModelWindowHandle);

                    HOperatorSet.SortContoursXld(Edges, out Edges, "upper_left", "true", "column");

                    HOperatorSet.SelectObj(Edges, out SelectedXLD1, 1);
                    HOperatorSet.FitLineContourXld(SelectedXLD1, "tukey", -1, 0, 5, 2,
                        out RowBegin1, out ColBegin1, out RowEnd1, out ColEnd1, out Nr1,
                        out Nc1, out Dist1);

                    HOperatorSet.SelectObj(Edges, out SelectedXLD2, 2);
                    HOperatorSet.FitLineContourXld(SelectedXLD2, "tukey", -1, 0, 5, 2,
                     out RowBegin2, out ColBegin2, out RowEnd2, out ColEnd2, out Nr2,
                     out Nc2, out Dist2);

                    HOperatorSet.AngleLx(RowBegin1, ColBegin1, RowEnd1, ColEnd1, out Deg1);
                    HOperatorSet.TupleDeg(Deg1, out Deg1);
                    if ((int)(new HTuple(Deg1.TupleLess(0))) != 0)
                    {
                        Deg1 = Deg1 + 180;
                    }

                    HOperatorSet.AngleLx(RowBegin2, ColBegin2, RowEnd2, ColEnd2, out Deg2);
                    HOperatorSet.TupleDeg(Deg2, out Deg2);
                    if ((int)(new HTuple(Deg2.TupleLess(0))) != 0)
                    {
                        Deg2 = Deg2 + 180;
                    }



                    //*******************获取顶部边缘线********************//

                    HOperatorSet.ReduceDomain(Image, RectangleUp, out ImageReduced);
                    HOperatorSet.GrayClosingRect(ImageReduced, out ImageReduced, 3, 11);
                    HOperatorSet.Emphasize(ImageReduced, out ImageReduced, 7, 7, 1);
                    HOperatorSet.EdgesSubPix(ImageReduced, out Edges, "canny", 2, 20,
                        40);
                    HOperatorSet.SegmentContoursXld(Edges, out Edges, "lines", 5, 4, 2);

                    HOperatorSet.SmallestRectangle2(RectangleUp, out row, out Col, out phi, out L1, out L2);

                    HOperatorSet.SelectShapeXld(Edges, out Edges, "contlength",
                     "and", L1 * 2 * 0.1, L1 * 3);

                    HOperatorSet.UnionCollinearContoursXld(Edges, out Edges, L1 * 2 * 0.4, L1 * 2 * 0.2, 10, 0.1, "attr_keep");
                    HOperatorSet.SelectShapeXld(Edges, out Edges, "contlength",
                   "and", L1 * 2 * 0.3, L1 * 3);

                    HOperatorSet.SetColor(ModelWindowHandle, "red");
                    HOperatorSet.DispObj(Edges, ModelWindowHandle);

                    HOperatorSet.FitLineContourXld(Edges, "tukey", -1, 0, 5, 2,
                      out RowBegin3, out ColBegin3, out RowEnd3, out ColEnd3, out Nr3,
                      out Nc3, out Dist3);



                    //*******************获取底部边缘线********************//

                    HOperatorSet.ReduceDomain(Image, RectangleDown, out ImageReduced);

                    HOperatorSet.GrayClosingRect(ImageReduced, out ImageReduced, 3, 11);
                    HOperatorSet.Emphasize(ImageReduced, out ImageReduced, 7, 7, 1);

                    HOperatorSet.EdgesSubPix(ImageReduced, out Edges, "canny", 2, 20,
                        40);
                    HOperatorSet.SmallestRectangle2(RectangleDown, out row, out Col, out phi, out L1, out L2);

                    HOperatorSet.SelectShapeXld(Edges, out Edges, "contlength",
                     "and", L1 * 2 * 0.1, L1 * 3);

                    HOperatorSet.UnionCollinearContoursXld(Edges, out Edges, L1 * 2 * 0.4, L1 * 2 * 0.2, 10, 0.1, "attr_keep");
                    HOperatorSet.SelectShapeXld(Edges, out Edges, "contlength",
                   "and", L1 * 2 * 0.3, L1 * 3);

                    HOperatorSet.FitLineContourXld(Edges, "tukey", -1, 0, 5, 2,
                      out RowBegin4, out ColBegin4, out RowEnd4, out ColEnd4, out Nr4,
                      out Nc4, out Dist4);

                    HOperatorSet.SetColor(ModelWindowHandle, "red");
                    HOperatorSet.DispObj(Edges, ModelWindowHandle);
                    //*******************获取边角交点********************//
                    HOperatorSet.SetColor(ModelWindowHandle, "green");
                    HOperatorSet.IntersectionLines(RowBegin1, ColBegin1, RowEnd1, ColEnd1,
                    RowBegin3, ColBegin3, RowEnd3, ColEnd3, out CrossRow1, out CrossColumn1, out IsOverlapping);
                    HOperatorSet.GenCrossContourXld(out ResultCross, CrossRow1, CrossColumn1, 100, (new HTuple(45)).TupleRad());
                    HOperatorSet.DispObj(ResultCross, ModelWindowHandle);

                    HOperatorSet.IntersectionLines(RowBegin2, ColBegin2, RowEnd2, ColEnd2,
                    RowBegin4, ColBegin4, RowEnd4, ColEnd4, out CrossRow2, out CrossColumn2, out IsOverlapping);
                    HOperatorSet.GenCrossContourXld(out ResultCross, CrossRow2, CrossColumn2, 100, (new HTuple(45)).TupleRad());
                    HOperatorSet.DispObj(ResultCross, ModelWindowHandle);





                    HOperatorSet.GenCrossContourXld(out ResultCross, CrossRow1 + ((CrossRow2 - CrossRow1) / 2.0), CrossColumn1 + ((CrossColumn2 - CrossColumn1) / 2.0), 100, (new HTuple(45)).TupleRad());
                    HOperatorSet.DispObj(ResultCross, ModelWindowHandle);

                    DegMean = (Deg1 + Deg2) / 2.0;
                    Rad = DegMean.TupleRad();


                    dh = Xoffset * ratio;
                    dv = Yoffset * ratio;

                    dx = (dh * (Rad.TupleSin())) + (dv * (Rad.TupleCos()));
                    dy = (dh * (Rad.TupleCos())) - (dv * (Rad.TupleSin()));

                    HOperatorSet.GenCrossContourXld(out ResultCross, dy + CrossRow1 + ((CrossRow2 - CrossRow1) / 2.0),
                        dx + CrossColumn1 + ((CrossColumn2 - CrossColumn1) / 2.0), 100, (new HTuple(45)).TupleRad());
                    HOperatorSet.DispObj(ResultCross, ModelWindowHandle);

                    IniAPI.INIWriteValue(ModelPath + ModelName + "FitCoordConfig.ini", "拍照", "DX", Xoffset.ToString());
                    IniAPI.INIWriteValue(ModelPath + ModelName + "FitCoordConfig.ini", "拍照", "DY", Yoffset.ToString());

                }
                catch
                {

                }
            }

            RectangleSide.Dispose();
            RectangleUp.Dispose();
            RectangleDown.Dispose();
            ImageReduced.Dispose();
            Edges.Dispose();
            SelectedXLD1.Dispose();
            SelectedXLD2.Dispose();


            RectangleCross.Dispose();
            Image.Dispose();
            ResultCross.Dispose();
            HOperatorSet.ClearNccModel(NCCModelId);

        }
        /// <summary>
        /// 对焦
        /// </summary>
        /// <param name="Image"></param>
        private void Setfocus(HObject Image)
        {
            HObject EdgeAmplitude = null;

            HTuple Mean = new HTuple();
            HTuple Deviation = new HTuple();
            HTuple Width = null, Height = null;

            HOperatorSet.GetImageSize(Image, out Width, out Height);
            HOperatorSet.SetPart(ModelWindowHandle, 0, 0, Height - 1, Width - 1);
            HOperatorSet.SobelAmp(Image, out EdgeAmplitude, "sum_abs", 3);
            HOperatorSet.Intensity(Image, EdgeAmplitude, out Mean, out Deviation);
            double _Deviation = Deviation;
            Mod_sys.Instance.gfrmProdChange.txt_MaxValue.Text = _Deviation.ToString();
            HOperatorSet.DispObj(EdgeAmplitude, ModelWindowHandle);
            Image.Dispose();
            EdgeAmplitude.Dispose();
        }

        private void HobjectToHimage(HObject hobject, ref HImage image)
        {
            HTuple pointer, type, width, height;
            HOperatorSet.GetImagePointer1(hobject, out pointer, out type, out width, out height);
            image.GenImage1(type, width, height, pointer);
        }

        public Enum_sys.ProdState ShowCellState;
        public void PaintImage()
        {

            if (ShowImage.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ShowImage);
            }
            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("green");

            if (RectangleCross.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(RectangleCross);
            }
            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("yellow");

            if (RectangleSide.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(RectangleSide);
            }
            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("blue");

            if (ShowLine1.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ShowLine1);
            }
            if (ShowLine2.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ShowLine2);
            }
            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("red");

            if (EdgesSide.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(EdgesSide);
            }
            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("yellow");

            if (RectangleUp.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(RectangleUp);
            }
            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("red");

            if (EdgesUp.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(EdgesUp);
            }
            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("blue");

            if (ShowLine3.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ShowLine3);
            }
            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("yellow");

            if (RectangleDown.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(RectangleDown);
            }
            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("red");

            if (EdgesDown.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(EdgesDown);
            }
            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("blue");

            if (ShowLine4.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ShowLine4);
            }
            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("green");

            if (ResultCross1.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ResultCross1);
            }
            if (ResultCross2.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ResultCross2);
            }
            if (ResultCross3.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ResultCross3);
            }
            if (ResultCross4.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ResultCross4);
            }
            if (ResultCrossW.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ResultCrossW);
            }
            if (RectangleEar.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(RectangleEar);
            }
            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("red");

            if (ShowMetContour.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ShowMetContour);
            }
            if (XldSelected1.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(XldSelected1);
            }
            if (XldSelected2.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(XldSelected2);
            }
            if (XldSelected3.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(XldSelected3);
            }
            if (XldSelected4.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(XldSelected4);
            }
            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("blue");

            if (Arrow1.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(Arrow1);
            }
            if (Arrow2.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(Arrow2);
            }
            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("pink");

            if (Arrow3.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(Arrow3);
            }
            if (Arrow4.IsInitialized())
            {
                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(Arrow4);
            }
            ShowCCDStatic(ShowCellState);

        }

        public HImage m_Image = new HImage();
        public HObject ShowImage = new HObject();
        HObject ThresholdRegion = new HObject();
        HObject ResultCross1 = new HObject(), ResultCross2 = new HObject(), ResultCross3 = new HObject(), ResultCross4 = new HObject(), ResultCrossW = new HObject();
        HObject RectangleEar = new HObject();
        HObject MetrRefContour = new HObject(), ImageReduced = new HObject();
        HObject EdgesEar = new HObject(), EdgesSide = new HObject(), EdgesUp = new HObject(), EdgesDown = new HObject();
        HObject XldSelected1 = new HObject(), XldSelected2 = new HObject(), XldSelected3 = new HObject(), XldSelected4 = new HObject();
        HObject Arrow1 = new HObject(), Arrow2 = new HObject(), Arrow3 = new HObject(), Arrow4 = new HObject();
        HObject ShowMetContour = new HObject(), RectangleCross = new HObject();
        HObject RectangleSide = new HObject(), RectangleUp = new HObject(), RectangleDown = new HObject(), SelectedXLD1 = new HObject(), SelectedXLD2 = new HObject();
        HObject ShowLine1 = new HObject(), ShowLine2 = new HObject(), ShowLine3 = new HObject(), ShowLine4 = new HObject();

        /// <summary>
        /// 图像处理方法
        /// </summary>
        /// <param name="ShowImage"></param>
        private void Execute有极耳(HObject Image)
        {
            SendData ResultData = new SendData();

            HTuple Width = null, Height = null;
            HTuple ResultRow = null, ResultColumn = null;
            HTuple UsedThreshold = null, RegionHomMat2D = null;
            HTuple ratio = null, DegMean = null, Rad = null, dh = null, dv = null, dx = null, dy = null;
            double Xoffset = Convert.ToDouble(X);
            double Yoffset = Convert.ToDouble(Y);
            HTuple CrossArea = null, CrossRow = null, CrossColumn = null;
            HTuple RetangleArea = null, RetangleRow = null, RetangleColumn = null;
            HTuple Deg1 = null, Deg2 = null, Deg3 = null, Deg4 = null, ObjCount = null;


            HTuple NCCModelId = null;

            HTuple MatchingRow = null, MatchingCol = null, MatchingScore = null;
            HTuple RowBegin1 = null, ColBegin1 = null, RowEnd1 = null, ColEnd1 = null, Nr1 = null, Nc1 = null, Dist1 = null;
            HTuple RowL1 = null, ColL1 = null, RowL2 = null, ColL2 = null, RowR1 = null, ColR1 = null, RowR2 = null, ColR2 = null;
            HTuple RowBegin3 = null, ColBegin3 = null, RowEnd3 = null, ColEnd3 = null, Nr3 = null, Nc3 = null, Dist3 = null;
            HTuple MainDisMedLeft1 = null, MainDisMedLeft2 = null, MainDisMedRight1 = null, MainDisMedRight2 = null;
            HTuple MetRowBegin = null, MetColBegin = null, MetRowEnd = null, MetColEnd = null, MetNr = null, MetNc = null, Metist = null;
            HTuple[] RowProj = new HTuple[2], ColProj = new HTuple[2];
            HTuple MetrologyHandle = null;
            HTuple MatchingAngle = null;
            HTuple MainDistanceL1 = null, MainDistanceL2 = null, MainDistanceR1 = null, MainDistanceR2 = null;
            HTuple IsOverlapping = null, CrossRow1 = null, CrossColumn1 = null, CrossRow2 = null, CrossColumn2 = null;
            HTuple RowBegin2 = null, ColBegin2 = null, RowEnd2 = null, ColEnd2 = null, Nr2 = null, Nc2 = null, Dist2 = null;
            HTuple RowBegin4 = null, ColBegin4 = null, RowEnd4 = null, ColEnd4 = null, Nr4 = null, Nc4 = null, Dist4 = null;
            HTuple row = null, Col = null, phi = null, L1 = null, L2 = null;
            HTuple Row1 = null, Col1 = null, CellWidth = null;
            HTuple MaxL1 = null, MaxL2 = null, MaxR1 = null, MaxR2 = null;
            HTuple IndexL1 = null, IndexL2 = null, IndexR1 = null, IndexR2 = null;

            ResultCross1.Dispose();
            ResultCross2.Dispose();
            ResultCross3.Dispose();
            ResultCross4.Dispose();
            ResultCrossW.Dispose();
            ShowLine1.Dispose();
            ShowLine2.Dispose();
            ShowLine3.Dispose();
            ShowLine4.Dispose();
            ShowImage.Dispose();
            m_Image.Dispose();
            RectangleSide.Dispose();
            RectangleUp.Dispose();
            RectangleDown.Dispose();
            SelectedXLD1.Dispose();
            SelectedXLD2.Dispose();
            ThresholdRegion.Dispose();
            RectangleCross.Dispose();
            RectangleEar.Dispose();
            MetrRefContour.Dispose();
            ImageReduced.Dispose();

            EdgesEar.Dispose();
            EdgesSide.Dispose();
            EdgesUp.Dispose();
            EdgesDown.Dispose();

            XldSelected1.Dispose();
            XldSelected2.Dispose();
            XldSelected3.Dispose();
            XldSelected4.Dispose();
            Arrow1.Dispose();
            Arrow2.Dispose();
            Arrow3.Dispose();
            Arrow4.Dispose();
            ShowMetContour.Dispose();

            ShowImage = Image.Clone();
            Image.Dispose();


            HOperatorSet.GetImageSize(ShowImage, out Width, out Height);
            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetDraw("margin");

            HOperatorSet.ReadNccModel(ModelPath + ModelName + "NCC.ncm", out NCCModelId);
            HOperatorSet.ReadMetrologyModel(ModelPath + ModelName + ".mtr", out MetrologyHandle);
            HOperatorSet.ReadTuple(ModelPath + "ratio.tup", out ratio);
            HOperatorSet.ReadRegion(out RectangleEar, ModelPath + ModelName + "RectangleEar.hobj");
            HOperatorSet.ReadRegion(out RectangleCross, ModelPath + ModelName + "RectangleCross.hobj");
            HOperatorSet.ReadRegion(out RectangleSide, ModelPath + ModelName + "RectangleSide.hobj");
            HOperatorSet.ReadRegion(out RectangleUp, ModelPath + ModelName + "RectangleUp.hobj");
            HOperatorSet.ReadRegion(out RectangleDown, ModelPath + ModelName + "RectangleDown.hobj");


            double.TryParse(IniAPI.INIGetStringValue(ModelPath + ModelName + "FitCoordConfig.ini", "拍照", "DX", "0"), out Xoffset);
            double.TryParse(IniAPI.INIGetStringValue(ModelPath + ModelName + "FitCoordConfig.ini", "拍照", "DY", "0"), out Yoffset);

            //if (ShowImage.IsInitialized())
            //{
            //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ShowImage);
            //}

            HOperatorSet.FindNccModel(ShowImage, NCCModelId, (new HTuple(-40)).TupleRad()
            , (new HTuple(80)).TupleRad(), 0.9, 1, 0.5, "true", 0, out MatchingRow, out MatchingCol,
            out MatchingAngle, out MatchingScore);

            HOperatorSet.BinaryThreshold(ShowImage, out ThresholdRegion, "max_separability", "dark", out UsedThreshold);

            HOperatorSet.Connection(ThresholdRegion, out ThresholdRegion);

            HOperatorSet.SelectShapeStd(ThresholdRegion, out ThresholdRegion, "max_area", 70);

            //****************************安全大小*************************//
            HTuple RegionInfo = null, RegionArea = null, RegionRow = null, RegionCol = null;
            HOperatorSet.AreaCenter(ThresholdRegion, out RegionArea, out RegionRow, out RegionCol);
            HOperatorSet.ReadTuple(ModelPath + ModelName + "RegionInfo.tup", out RegionInfo);

            try
            {
                if (((int)(new HTuple(MatchingScore.TupleLess(0.99))) == 0) && ((int)(new HTuple(RegionArea.TupleLess(RegionInfo * (1 + 0.2)))) != 0))
                {
                    HOperatorSet.AreaCenter(RectangleCross, out RetangleArea, out RetangleRow, out RetangleColumn);

                    HOperatorSet.VectorAngleToRigid(RetangleRow, RetangleColumn, 0, MatchingRow, MatchingCol, MatchingAngle, out RegionHomMat2D);

                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("green");
                    HOperatorSet.AffineTransRegion(RectangleCross, out RectangleCross, RegionHomMat2D, "nearest_neighbor");
                    //if (RectangleCross.IsInitialized())
                    //{
                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(RectangleCross);
                    //}
                    Mod_sys.Instance.gfrmAutoRun.txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " -->匹配分数:" + MatchingScore.ToString() + "\r\n";

                    //************************定位**********************//

                    try
                    {
                        HOperatorSet.AffineTransRegion(RectangleSide, out RectangleSide, RegionHomMat2D, "nearest_neighbor");
                        HOperatorSet.AffineTransRegion(RectangleUp, out RectangleUp, RegionHomMat2D, "nearest_neighbor");
                        HOperatorSet.AffineTransRegion(RectangleDown, out RectangleDown, RegionHomMat2D, "nearest_neighbor");

                        //*******************获取侧边边缘线********************//
                        HOperatorSet.ReduceDomain(ShowImage, RectangleSide, out ImageReduced);
                        HOperatorSet.GrayClosingRect(ImageReduced, out ImageReduced, 11, 3);
                        HOperatorSet.Emphasize(ImageReduced, out ImageReduced, 7, 7, 1);
                        Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("yellow");

                        //if (RectangleSide.IsInitialized())
                        //{
                        //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(RectangleSide);
                        //}
                        HOperatorSet.EdgesSubPix(ImageReduced, out EdgesSide, "canny", 4, 20,
                            40);

                        HOperatorSet.SmallestRectangle2(RectangleSide, out row, out Col, out phi, out L1, out L2);

                        HOperatorSet.SelectShapeXld(EdgesSide, out EdgesSide, "contlength",
                      "and", L2 * 2 * 0.1, L2 * 3);
                        HOperatorSet.UnionCollinearContoursXld(EdgesSide, out EdgesSide, L2 * 2 * 0.4, L2 * 2 * 0.2, 10, 0.1, "attr_keep");
                        HOperatorSet.SelectShapeXld(EdgesSide, out EdgesSide, "contlength",
                         "and", L2 * 2 * 0.5, L2 * 3);

                        HOperatorSet.SortContoursXld(EdgesSide, out EdgesSide, "upper_left", "true", "column");
                        //if (EdgesSide.IsInitialized())
                        //{
                        //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(EdgesSide);
                        //}

                        HOperatorSet.SelectObj(EdgesSide, out SelectedXLD1, 1);
                        HOperatorSet.FitLineContourXld(SelectedXLD1, "tukey", -1, 0, 5, 2,
                            out RowBegin1, out ColBegin1, out RowEnd1, out ColEnd1, out Nr1,
                            out Nc1, out Dist1);

                        HOperatorSet.SelectObj(EdgesSide, out SelectedXLD2, 2);
                        HOperatorSet.FitLineContourXld(SelectedXLD2, "tukey", -1, 0, 5, 2,
                         out RowBegin2, out ColBegin2, out RowEnd2, out ColEnd2, out Nr2,
                         out Nc2, out Dist2);
                        if ((int)(RowBegin1.TupleLength()) == 1 && (int)(RowBegin2.TupleLength()) == 1)
                        {
                            Mod_sys.Instance.gfrmAutoRun.txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " -->侧边边缘提取成功" + "\r\n";

                        }
                        else
                        {
                            Mod_sys.Instance.gfrmAutoRun.txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " -->侧边边缘提取失败" + "\r\n";
                            throw new Exception();

                        }
                        HOperatorSet.AngleLx(RowBegin1, ColBegin1, RowEnd1, ColEnd1, out Deg1);
                        //HOperatorSet.GenRectangle2(out ShowLine1, RowBegin1 + (RowEnd1 - RowBegin1) / 2.0, ColBegin1 + (ColEnd1 - ColBegin1) / 2.0, Deg1, 1000, 0.5);
                        HOperatorSet.GenContourPolygonXld(out ShowLine1,
                    (new HTuple(RowBegin1 + (RowEnd1 - RowBegin1) / 2.0 - 1000 * (new HTuple(-Deg1)).TupleSin())).TupleConcat(RowBegin1 + (RowEnd1 - RowBegin1) / 2.0 + 1000 * (new HTuple(-Deg1)).TupleSin()),
                    (new HTuple(ColBegin1 + (ColEnd1 - ColBegin1) / 2.0 - 1000 * (new HTuple(-Deg1)).TupleCos())).TupleConcat(ColBegin1 + (ColEnd1 - ColBegin1) / 2.0 + 1000 * (new HTuple(-Deg1)).TupleCos()));

                        Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("blue");

                        //if (ShowLine1.IsInitialized())
                        //{
                        //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ShowLine1);
                        //}
                        HOperatorSet.TupleDeg(Deg1, out Deg1);
                        if ((int)(new HTuple(Deg1.TupleLess(0))) != 0)
                        {
                            Deg1 = Deg1 + 180;
                        }

                        HOperatorSet.AngleLx(RowBegin2, ColBegin2, RowEnd2, ColEnd2, out Deg2);
                        //HOperatorSet.GenRectangle2(out ShowLine2, RowBegin2 + (RowEnd2 - RowBegin2) / 2.0, ColBegin2 + (ColEnd2 - ColBegin2) / 2.0, Deg2, 1000, 0.5);
                        HOperatorSet.GenContourPolygonXld(out ShowLine2,
         (new HTuple(RowBegin2 + (RowEnd2 - RowBegin2) / 2.0 - 1000 * (new HTuple(-Deg2)).TupleSin())).TupleConcat(RowBegin2 + (RowEnd2 - RowBegin2) / 2.0 + 1000 * (new HTuple(-Deg2)).TupleSin()),
         (new HTuple(ColBegin2 + (ColEnd2 - ColBegin2) / 2.0 - 1000 * (new HTuple(-Deg2)).TupleCos())).TupleConcat(ColBegin2 + (ColEnd2 - ColBegin2) / 2.0 + 1000 * (new HTuple(-Deg2)).TupleCos()));


                        //if (ShowLine2.IsInitialized())
                        //{
                        //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ShowLine2);
                        //}
                        HOperatorSet.TupleDeg(Deg2, out Deg2);
                        if ((int)(new HTuple(Deg2.TupleLess(0))) != 0)
                        {
                            Deg2 = Deg2 + 180;
                        }

                        Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("red");



                        HOperatorSet.GetContourXld(SelectedXLD1, out Row1, out Col1);

                        HOperatorSet.DistancePl(Row1, Col1, RowBegin2, ColBegin2, RowEnd2, ColEnd2, out CellWidth);

                        CellWidth = CellWidth.TupleMean();
                        ResultData.Width = CellWidth / ratio;
                        //*******************获取顶部边缘线********************//

                        HOperatorSet.ReduceDomain(ShowImage, RectangleUp, out ImageReduced);
                        HOperatorSet.GrayClosingRect(ImageReduced, out ImageReduced, 3, 11);
                        HOperatorSet.Emphasize(ImageReduced, out ImageReduced, 7, 7, 1);
                        Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("yellow");

                        //if (RectangleUp.IsInitialized())
                        //{
                        //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(RectangleUp);
                        //}
                        HOperatorSet.EdgesSubPix(ImageReduced, out EdgesUp, "canny", 3, 30, 50);
                        HOperatorSet.SegmentContoursXld(EdgesUp, out EdgesUp, "lines", 5, 4, 2);

                        HOperatorSet.SmallestRectangle2(RectangleUp, out row, out Col, out phi, out L1, out L2);

                        HOperatorSet.SelectShapeXld(EdgesUp, out EdgesUp, "contlength",
                         "and", L1 * 2 * 0.1, L1 * 3);

                        HOperatorSet.UnionCollinearContoursXld(EdgesUp, out EdgesUp, L1 * 2 * 0.4, L1 * 2 * 0.2, 15, 0.3, "attr_keep");
                        HOperatorSet.SelectShapeXld(EdgesUp, out EdgesUp, "contlength", "and", L1 * 2 * 0.5, L1 * 3);
                        Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("red");

                        //if (EdgesUp.IsInitialized())
                        //{
                        //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(EdgesUp);
                        //}
                        HOperatorSet.FitLineContourXld(EdgesUp, "tukey", -1, 0, 5, 2,
                          out RowBegin3, out ColBegin3, out RowEnd3, out ColEnd3, out Nr3,
                          out Nc3, out Dist3);
                        if ((int)(RowBegin3.TupleLength()) == 1)
                        {
                            Mod_sys.Instance.gfrmAutoRun.txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " -->顶部边缘提取成功" + "\r\n";

                        }
                        else
                        {
                            Mod_sys.Instance.gfrmAutoRun.txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " -->顶部边缘提取失败" + "\r\n";
                            throw new Exception();

                        }
                        HOperatorSet.AngleLx(RowBegin3, ColBegin3, RowEnd3, ColEnd3, out Deg3);
                        HOperatorSet.GenRectangle2(out ShowLine3, RowBegin3 + (RowEnd3 - RowBegin3) / 2.0, ColBegin3 + (ColEnd3 - ColBegin3) / 2.0, Deg3, 1000, 1);
                        HOperatorSet.GenContourPolygonXld(out ShowLine3,
          (new HTuple(RowBegin3 + (RowEnd3 - RowBegin3) / 2.0 - 1000 * (new HTuple(-Deg3)).TupleSin())).TupleConcat(RowBegin3 + (RowEnd3 - RowBegin3) / 2.0 + 1000 * (new HTuple(-Deg3)).TupleSin()),
          (new HTuple(ColBegin3 + (ColEnd3 - ColBegin3) / 2.0 - 1000 * (new HTuple(-Deg3)).TupleCos())).TupleConcat(ColBegin3 + (ColEnd3 - ColBegin3) / 2.0 + 1000 * (new HTuple(-Deg3)).TupleCos()));

                        Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("blue");

                        //if (ShowLine3.IsInitialized())
                        //{
                        //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ShowLine3);
                        //}


                        //*******************获取底部边缘线********************//

                        HOperatorSet.ReduceDomain(ShowImage, RectangleDown, out ImageReduced);

                        HOperatorSet.GrayClosingRect(ImageReduced, out ImageReduced, 3, 11);
                        HOperatorSet.Emphasize(ImageReduced, out ImageReduced, 7, 7, 1);

                        Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("yellow");

                        //if (RectangleDown.IsInitialized())
                        //{
                        //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(RectangleDown);
                        //}
                        HOperatorSet.EdgesSubPix(ImageReduced, out EdgesDown, "canny", 3, 30, 50);

                        HOperatorSet.SmallestRectangle2(RectangleDown, out row, out Col, out phi, out L1, out L2);

                        HOperatorSet.SelectShapeXld(EdgesDown, out EdgesDown, "contlength",
                         "and", L1 * 2 * 0.1, L1 * 3);

                        HOperatorSet.UnionCollinearContoursXld(EdgesDown, out EdgesDown, L1 * 2 * 0.4, L1 * 2 * 0.2, 10, 0.1, "attr_keep");

                        HOperatorSet.UnionAdjacentContoursXld(EdgesDown, out EdgesDown, L1 * 2 * 0.1, 5, "attr_keep");


                        HOperatorSet.SelectShapeXld(EdgesDown, out EdgesDown, "contlength", "and", L1 * 2 * 0.6, L1 * 3);

                        HOperatorSet.FitLineContourXld(EdgesDown, "tukey", -1, 0, 5, 2,
                          out RowBegin4, out ColBegin4, out RowEnd4, out ColEnd4, out Nr4,
                          out Nc4, out Dist4);
                        Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("red");

                        //if (EdgesDown.IsInitialized())
                        //{
                        //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(EdgesDown);
                        //}
                        if ((int)(RowBegin4.TupleLength()) == 1)
                        {
                            Mod_sys.Instance.gfrmAutoRun.txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " -->底部边缘提取成功" + "\r\n";

                        }
                        else
                        {
                            Mod_sys.Instance.gfrmAutoRun.txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " -->底部边缘提取失败" + "\r\n";
                            throw new Exception();
                        }


                        HOperatorSet.AngleLx(RowBegin4, ColBegin4, RowEnd4, ColEnd4, out Deg4);
                        // HOperatorSet.GenRectangle2(out ShowLine4, RowBegin4 + (RowEnd4 - RowBegin4) / 2.0, ColBegin4 + (ColEnd4 - ColBegin4) / 2.0, Deg4, 1000, 1);
                        HOperatorSet.GenContourPolygonXld(out ShowLine4,
               (new HTuple(RowBegin4 + (RowEnd4 - RowBegin4) / 2.0 - 1000 * (new HTuple(-Deg4)).TupleSin())).TupleConcat(RowBegin4 + (RowEnd4 - RowBegin4) / 2.0 + 1000 * (new HTuple(-Deg4)).TupleSin()),
               (new HTuple(ColBegin4 + (ColEnd4 - ColBegin4) / 2.0 - 1000 * (new HTuple(-Deg4)).TupleCos())).TupleConcat(ColBegin4 + (ColEnd4 - ColBegin4) / 2.0 + 1000 * (new HTuple(-Deg4)).TupleCos()));


                        Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("blue");

                        //if (ShowLine4.IsInitialized())
                        //{
                        //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ShowLine4);
                        //}


                        //*******************获取边角交点********************//
                        Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("green");

                        HOperatorSet.IntersectionLines(RowBegin1, ColBegin1, RowEnd1, ColEnd1,
                        RowBegin3, ColBegin3, RowEnd3, ColEnd3, out CrossRow1, out CrossColumn1, out IsOverlapping);
                        HOperatorSet.GenCrossContourXld(out ResultCross1, CrossRow1, CrossColumn1, 100, (new HTuple(45)).TupleRad());
                        //if (ResultCross1.IsInitialized())
                        //{
                        //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ResultCross1);
                        //}
                        HOperatorSet.IntersectionLines(RowBegin2, ColBegin2, RowEnd2, ColEnd2,
                        RowBegin4, ColBegin4, RowEnd4, ColEnd4, out CrossRow2, out CrossColumn2, out IsOverlapping);
                        HOperatorSet.GenCrossContourXld(out ResultCross2, CrossRow2, CrossColumn2, 100, (new HTuple(45)).TupleRad());
                        //if (ResultCross2.IsInitialized())
                        //{
                        //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ResultCross2);
                        //}
                        HOperatorSet.GenCrossContourXld(out ResultCross3, CrossRow1 + ((CrossRow2 - CrossRow1) / 2.0), CrossColumn1 + ((CrossColumn2 - CrossColumn1) / 2.0), 100, (new HTuple(45)).TupleRad());
                        //if (ResultCross3.IsInitialized())
                        //{
                        //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ResultCross3);
                        //}
                        DegMean = (Deg1 + Deg2) / 2.0;
                        Rad = DegMean.TupleRad();


                        dh = Xoffset * ratio;
                        dv = Yoffset * ratio;

                        dx = (dh * (Rad.TupleSin())) + (dv * (Rad.TupleCos()));
                        dy = (dh * (Rad.TupleCos())) - (dv * (Rad.TupleSin()));

                        HOperatorSet.GenCrossContourXld(out ResultCross4, dy + CrossRow1 + ((CrossRow2 - CrossRow1) / 2.0),
                            dx + CrossColumn1 + ((CrossColumn2 - CrossColumn1) / 2.0), 100, (new HTuple(45)).TupleRad());
                        //if (ResultCross4.IsInitialized())
                        //{
                        //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ResultCross4);
                        //}

                        HOperatorSet.AffineTransPixel(TranHomMat2D, dy + CrossRow1 + ((CrossRow2 - CrossRow1) / 2.0), dx + CrossColumn1 + ((CrossColumn2 - CrossColumn1) / 2.0),
                             out ResultRow, out ResultColumn);
                        ResultData.X = ResultRow;
                        ResultData.Y = ResultColumn;
                        ResultData.U = DegMean - 90;

                        ResultData.located = true;
                        Mod_sys.Instance.gfrmAutoRun.txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " -->定位坐标计算成功" + "\r\n";

                    }
                    catch
                    {

                        if (Mod_sys.Instance.gfrmAutoRun.UNSAFT_CCD_MODE)
                        {
                            HOperatorSet.Threshold(ShowImage, out ThresholdRegion, 0, 50); HOperatorSet.Connection(ThresholdRegion, out ThresholdRegion);
                            HOperatorSet.SelectShapeStd(ThresholdRegion, out ThresholdRegion, "max_area", 70);
                            HOperatorSet.AreaCenter(ThresholdRegion, out CrossArea, out CrossRow, out CrossColumn);
                            if (((int)(new HTuple(CrossArea.TupleLess(RegionInfo * 0.5))) == 0))
                            {

                                dv = Yoffset * ratio;
                                HOperatorSet.AffineTransPixel(TranHomMat2D, CrossRow - dv, CrossColumn, out ResultRow, out ResultColumn);
                                HOperatorSet.GenCrossContourXld(out ResultCrossW, CrossRow - dv, CrossColumn, 75, (new HTuple(45)).TupleRad());
                                Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("green");
                                //if (ResultCrossW.IsInitialized())
                                //{
                                //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ResultCrossW);
                                //}
                                ResultData.X = ResultRow;
                                ResultData.Y = ResultColumn;
                                ResultData.U = 0;
                                ResultData.Width = 0;
                                ResultData.located = true;
                            }
                            else
                            {
                                ResultData.X = 0;
                                ResultData.Y = 0;
                                ResultData.U = 0;
                                ResultData.Width = 0;
                                ResultData.located = false;

                            }

                        }
                        else
                        {
                            ResultData.X = 0;
                            ResultData.Y = 0;
                            ResultData.U = 0;
                            ResultData.Width = 0;
                            ResultData.located = false;
                        }
                    }


                    //**************************测量******************************//
                    try
                    {
                        HOperatorSet.AlignMetrologyModel(MetrologyHandle, MatchingRow, MatchingCol, MatchingAngle);

                        //测量并对测量区域拟合几何形状
                        HOperatorSet.ApplyMetrologyModel(ShowImage, MetrologyHandle);

                        HOperatorSet.GetMetrologyObjectResultContour(out MetrRefContour, MetrologyHandle, 0, "all", 1.5);


                        HOperatorSet.AffineTransRegion(RectangleEar, out RectangleEar, RegionHomMat2D, "nearest_neighbor");

                        HOperatorSet.ReduceDomain(ShowImage, RectangleEar, out ImageReduced);

                        HOperatorSet.EdgesSubPix(ImageReduced, out EdgesEar, "canny", 1.5, 20, 40);
                        HOperatorSet.SegmentContoursXld(EdgesEar, out EdgesEar, "lines", 5, 4, 2);

                        HOperatorSet.UnionCollinearContoursXld(EdgesEar, out EdgesEar, 10, 1, 2, 0.1, "attr_keep");

                        HOperatorSet.SelectShapeXld(EdgesEar, out EdgesEar, "contlength", "and", 100, 500);

                        HOperatorSet.SelectShapeXld(EdgesEar, out EdgesEar, (new HTuple("rect2_phi")).TupleConcat(
                            "rect2_phi"), "or", (new HTuple(-1.8)).TupleConcat(1.2), (new HTuple(-1.2)).TupleConcat(
                            1.8));
                        HOperatorSet.SortContoursXld(EdgesEar, out EdgesEar, "upper_left", "true", "column");

                        HOperatorSet.CountObj(EdgesEar, out ObjCount);
                        //if (RectangleEar.IsInitialized())
                        //{
                        //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(RectangleEar);
                        //}
                        if ((int)ObjCount == 4)
                        {
                            //**********************左侧极耳xld*****************//

                            //第1条极耳左侧xld
                            HOperatorSet.SelectObj(EdgesEar, out XldSelected1, 1);
                            //HOperatorSet.FitLineContourXld(XldSelected1, "tukey", -1, 0, 5, 2,
                            //    out RowBegin1, out ColBegin1, out RowEnd1, out ColEnd1,
                            //    out Nr1, out Nc1, out Dist1);
                            HOperatorSet.GetContourXld(XldSelected1, out RowL1, out ColL1);

                            //第1条极耳右侧xld
                            HOperatorSet.SelectObj(EdgesEar, out XldSelected2, 2);
                            HOperatorSet.GetContourXld(XldSelected2, out RowL2, out ColL2);


                            //**********************右侧极耳xld*****************//
                            //第2条极耳左侧xld
                            HOperatorSet.SelectObj(EdgesEar, out XldSelected3, 3);

                            HOperatorSet.GetContourXld(XldSelected3, out RowR1, out ColR1);


                            //第2条极耳右侧xld
                            HOperatorSet.SelectObj(EdgesEar, out XldSelected4, 4);
                            HOperatorSet.GetContourXld(XldSelected4, out RowR2, out ColR2);

                            //*********************************计算两极耳到边缘距离************************//
                            HOperatorSet.FitLineContourXld(MetrRefContour, "tukey", -1, 0, 5, 2, out MetRowBegin, out MetColBegin, out MetRowEnd, out MetColEnd, out MetNr, out MetNc, out Metist);

                            //参考基准线，使用极耳xld求距离，避免扩展定义域
                            HOperatorSet.TupleMax(RowL1, out MaxL1);
                            HOperatorSet.TupleMax(RowL2, out MaxL2);
                            HOperatorSet.TupleMax(RowR1, out MaxR1);
                            HOperatorSet.TupleMax(RowR2, out MaxR2);

                            HOperatorSet.TupleFind(RowL1, MaxL1, out IndexL1);
                            HOperatorSet.TupleFind(RowL2, MaxL2, out IndexL2);
                            HOperatorSet.TupleFind(RowR1, MaxR1, out IndexR1);
                            HOperatorSet.TupleFind(RowR2, MaxR2, out IndexR2);

                            HOperatorSet.DistancePl(MaxL1, ColL1[IndexL1], MetRowBegin, MetColBegin, MetRowEnd, MetColEnd, out MainDistanceL1);

                            MainDisMedLeft1 = MainDistanceL1.TupleMedian();

                            HOperatorSet.DistancePl(MaxL2, ColL2[IndexL2], MetRowBegin, MetColBegin, MetRowEnd, MetColEnd, out MainDistanceL2);

                            MainDisMedLeft2 = MainDistanceL2.TupleMedian();

                            HOperatorSet.DistancePl(MaxR1, ColR1[IndexR1], MetRowBegin, MetColBegin, MetRowEnd, MetColEnd, out MainDistanceR1);

                            MainDisMedRight1 = MainDistanceR1.TupleMedian();

                            HOperatorSet.DistancePl(MaxR2, ColR2[IndexR2], MetRowBegin, MetColBegin, MetRowEnd, MetColEnd, out MainDistanceR2);

                            MainDisMedRight2 = MainDistanceR2.TupleMedian();


                            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("red");

                            HOperatorSet.ProjectionPl(MaxL2, ColL2[IndexL2], MetRowBegin, MetColBegin, MetRowEnd, MetColEnd, out RowProj[0],
                                                                      out ColProj[0]);
                            HOperatorSet.ProjectionPl(MaxR2, ColR2[IndexR2], MetRowBegin, MetColBegin, MetRowEnd, MetColEnd, out RowProj[1],
                                                                             out ColProj[1]);

                            if (MaxR2 < MaxL2)
                            {
                                HOperatorSet.GenContourPolygonXld(out ShowMetContour, MetRowEnd.TupleConcat(
                                                         RowProj[1]), MetColEnd.TupleConcat(ColProj[1]));
                                //if (ShowMetContour.IsInitialized())
                                //{
                                //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ShowMetContour);
                                //}
                            }
                            else
                            {
                                HOperatorSet.GenContourPolygonXld(out ShowMetContour, MetRowEnd.TupleConcat(
                                             RowProj[0]), MetColEnd.TupleConcat(ColProj[0]));
                                //if (ShowMetContour.IsInitialized())
                                //{
                                //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ShowMetContour);
                                //}
                            }



                            if (MainDisMedLeft1 > MainDisMedRight1)
                            {
                                if (MainDisMedLeft1 < MainDisMedLeft2)
                                {
                                    ResultData.MedLM = MainDisMedLeft1 / ratio;
                                    //if (XldSelected1.IsInitialized())
                                    //{
                                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(XldSelected1);
                                    //}
                                    gen_arrow_contour_xld(out Arrow1, MaxL1, ColL1[IndexL1], RowProj[0], ColProj[0], 50, 50);
                                    gen_arrow_contour_xld(out Arrow2, RowProj[0], ColProj[0], MaxL1, ColL1[IndexL1], 50, 50);
                                }
                                else
                                {
                                    ResultData.MedLM = MainDisMedLeft2 / ratio;
                                    //if (XldSelected2.IsInitialized())
                                    //{
                                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(XldSelected2);
                                    //}
                                    gen_arrow_contour_xld(out Arrow1, MaxL2, ColL2[IndexL2], RowProj[0], ColProj[0], 50, 50);
                                    gen_arrow_contour_xld(out Arrow2, RowProj[0], ColProj[0], MaxL2, ColL2[IndexL2], 50, 50);
                                }

                                if (MainDisMedRight1 < MainDisMedRight2)
                                {
                                    ResultData.MedM = MainDisMedRight1 / ratio;
                                    //if (XldSelected3.IsInitialized())
                                    //{
                                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(XldSelected3);
                                    //}
                                    gen_arrow_contour_xld(out Arrow3, MaxR1, ColR1[IndexR1], RowProj[1], ColProj[1], 50, 50);
                                    gen_arrow_contour_xld(out Arrow4, RowProj[1], ColProj[1], MaxR1, ColR1[IndexR1], 50, 50);
                                }
                                else
                                {
                                    ResultData.MedM = MainDisMedRight2 / ratio;
                                    //if (XldSelected4.IsInitialized())
                                    //{
                                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(XldSelected4);
                                    //}
                                    gen_arrow_contour_xld(out Arrow3, MaxR2, ColR2[IndexR2], RowProj[1], ColProj[1], 50, 50);
                                    gen_arrow_contour_xld(out Arrow4, RowProj[1], ColProj[1], MaxR2, ColR2[IndexR2], 50, 50);
                                }
                            }
                            else
                            {
                                if (MainDisMedLeft1 < MainDisMedLeft2)
                                {
                                    ResultData.MedM = MainDisMedLeft1 / ratio;
                                    //if (XldSelected1.IsInitialized())
                                    //{
                                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(XldSelected1);
                                    //}
                                    gen_arrow_contour_xld(out Arrow1, MaxL1, ColL1[IndexL1], RowProj[0], ColProj[0], 50, 50);
                                    gen_arrow_contour_xld(out Arrow2, RowProj[0], ColProj[0], MaxL1, ColL1[IndexL1], 50, 50);
                                }
                                else
                                {
                                    ResultData.MedM = MainDisMedLeft2 / ratio;
                                    //if (XldSelected2.IsInitialized())
                                    //{
                                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(XldSelected2);
                                    //}
                                    gen_arrow_contour_xld(out Arrow1, MaxL2, ColL2[IndexL2], RowProj[0], ColProj[0], 50, 50);
                                    gen_arrow_contour_xld(out Arrow2, RowProj[0], ColProj[0], MaxL2, ColL2[IndexL2], 50, 50);
                                }

                                if (MainDisMedRight1 < MainDisMedRight2)
                                {
                                    ResultData.MedLM = MainDisMedRight1 / ratio;
                                    //if (XldSelected3.IsInitialized())
                                    //{
                                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(XldSelected3);
                                    //}
                                    gen_arrow_contour_xld(out Arrow3, MaxR1, ColR1[IndexR1], RowProj[1], ColProj[1], 50, 50);
                                    gen_arrow_contour_xld(out Arrow4, RowProj[1], ColProj[1], MaxR1, ColR1[IndexR1], 50, 50);
                                }
                                else
                                {
                                    ResultData.MedLM = MainDisMedRight2 / ratio;
                                    //if (XldSelected4.IsInitialized())
                                    //{
                                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(XldSelected4);
                                    //}
                                    gen_arrow_contour_xld(out Arrow3, MaxR2, ColR2[IndexR2], RowProj[1], ColProj[1], 50, 50);
                                    gen_arrow_contour_xld(out Arrow4, RowProj[1], ColProj[1], MaxR2, ColR2[IndexR2], 50, 50);
                                }
                            }

                            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("blue");

                            //if (Arrow1.IsInitialized())
                            //{
                            //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(Arrow1);
                            //}
                            //if (Arrow2.IsInitialized())
                            //{
                            //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(Arrow2);
                            //}
                            //Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("pink");

                            //if (Arrow3.IsInitialized())
                            //{
                            //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(Arrow3);
                            //}
                            //if (Arrow4.IsInitialized())
                            //{
                            //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(Arrow4);
                            //}
                            ResultData.measured = true;
                            Mod_sys.Instance.gfrmAutoRun.txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " -->极耳边缘提取成功" + "\r\n";

                        }
                        else
                        {
                            Mod_sys.Instance.gfrmAutoRun.txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " -->极耳边缘提取失败" + "\r\n";
                            throw new Exception();
                        }
                    }
                    catch
                    {
                        ResultData.MedM = 0;
                        ResultData.MedLM = 0;
                        ResultData.measured = false;
                        Mod_sys.Instance.gfrmAutoRun.txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " -->极耳边缘提取报错失败" + "\r\n";
                    }
                }
                else
                {

                    Mod_sys.Instance.gfrmAutoRun.txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " -->模板匹配失败" + "\r\n";
                    throw new Exception();
                }
            }
            catch
            {
                if (Mod_sys.Instance.gfrmAutoRun.UNSAFT_CCD_MODE)
                {
                    HOperatorSet.Threshold(ShowImage, out ThresholdRegion, 0, 50);
                    HOperatorSet.Connection(ThresholdRegion, out ThresholdRegion);
                    HOperatorSet.SelectShapeStd(ThresholdRegion, out ThresholdRegion, "max_area", 70);
                    HOperatorSet.AreaCenter(ThresholdRegion, out CrossArea, out CrossRow, out CrossColumn);
                    if (((int)(new HTuple(CrossArea.TupleLess(RegionInfo * 0.5))) == 0))
                    {


                        dv = Yoffset * ratio;
                        HOperatorSet.AffineTransPixel(TranHomMat2D, CrossRow - dv, CrossColumn, out ResultRow, out ResultColumn);
                        HOperatorSet.GenCrossContourXld(out ResultCrossW, CrossRow - dv, CrossColumn, 75, (new HTuple(45)).TupleRad());
                        Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("green");
                        //if (ResultCrossW.IsInitialized())
                        //{
                        //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ResultCrossW);
                        //}
                        ResultData.MedM = 0;
                        ResultData.MedLM = 0;
                        ResultData.X = ResultRow;
                        ResultData.Y = ResultColumn;
                        ResultData.U = 0;
                        ResultData.Width = 0;
                        ResultData.located = true;
                    }
                    else
                    {
                        ResultData.MedM = 0;
                        ResultData.MedLM = 0;
                        ResultData.X = 0;
                        ResultData.Y = 0;
                        ResultData.U = 0;
                        ResultData.Width = 0;
                        ResultData.located = false;
                    }

                }
                else
                {
                    ResultData.MedM = 0;
                    ResultData.MedLM = 0;
                    ResultData.X = 0;
                    ResultData.Y = 0;
                    ResultData.U = 0;
                    ResultData.Width = 0;
                    ResultData.located = false;
                }

            }





            HobjectToHimage(ShowImage, ref m_Image);
            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.Image = m_Image;
            SendCCDResult(ResultData);
            Mod_sys.Instance.gfrmAutoRun.BeginInvoke(new MethodInvoker(delegate
            {
                Mod_sys.Instance.gfrmAutoRun.SaveLog();
            }));

            HOperatorSet.ClearNccModel(NCCModelId);
            HOperatorSet.ClearMetrologyModel(MetrologyHandle);

            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.DispImageFit();




        }

        private void Execute没极耳(HObject Image)
        {
            SendData ResultData = new SendData();
            HTuple Width = null, Height = null;
            HTuple ResultRow = null, ResultColumn = null;
            HTuple UsedThreshold = null, RegionHomMat2D = null;
            HTuple ratio = null, DegMean = null, Rad = null, dh = null, dv = null, dx = null, dy = null;
            double Xoffset = Convert.ToDouble(X);
            double Yoffset = Convert.ToDouble(Y);
            HTuple CrossArea = null, CrossRow = null, CrossColumn = null;
            HTuple Deg1 = null, Deg2 = null, Deg3 = null, Deg4 = null;
            HTuple NCCModelId = null;
            HTuple RetangleArea = null, RetangleRow = null, RetangleColumn = null;

            HTuple MatchingRow = null, MatchingCol = null, MatchingScore = null, MatchingAngle = null;
            HTuple IsOverlapping = null, CrossRow1 = null, CrossColumn1 = null, CrossRow2 = null, CrossColumn2 = null;
            HTuple Row1 = null, Col1 = null, CellWidth = null;

            HTuple RowBegin1 = null, ColBegin1 = null, RowEnd1 = null, ColEnd1 = null, Nr1 = null, Nc1 = null, Dist1 = null;
            HTuple RowBegin2 = null, ColBegin2 = null, RowEnd2 = null, ColEnd2 = null, Nr2 = null, Nc2 = null, Dist2 = null;
            HTuple RowBegin3 = null, ColBegin3 = null, RowEnd3 = null, ColEnd3 = null, Nr3 = null, Nc3 = null, Dist3 = null;
            HTuple RowBegin4 = null, ColBegin4 = null, RowEnd4 = null, ColEnd4 = null, Nr4 = null, Nc4 = null, Dist4 = null;
            HTuple row = null, Col = null, phi = null, L1 = null, L2 = null;

            ResultCross1.Dispose();
            ResultCross2.Dispose();
            ResultCross3.Dispose();
            ResultCross4.Dispose();
            ResultCrossW.Dispose();
            ShowLine1.Dispose();
            ShowLine2.Dispose();
            ShowLine3.Dispose();
            ShowLine4.Dispose();

            m_Image.Dispose();
            RectangleSide.Dispose();
            RectangleUp.Dispose();
            RectangleDown.Dispose();
            SelectedXLD1.Dispose();
            SelectedXLD2.Dispose();
            ThresholdRegion.Dispose();
            RectangleCross.Dispose();
            RectangleEar.Dispose();
            MetrRefContour.Dispose();
            ImageReduced.Dispose();
            ShowImage.Dispose();
            EdgesEar.Dispose();
            EdgesSide.Dispose();
            EdgesUp.Dispose();
            EdgesDown.Dispose();

            XldSelected1.Dispose();
            XldSelected2.Dispose();
            XldSelected3.Dispose();
            XldSelected4.Dispose();
            Arrow1.Dispose();
            Arrow2.Dispose();
            Arrow3.Dispose();
            Arrow4.Dispose();
            ShowMetContour.Dispose();


            ShowImage = Image.Clone();
            Image.Dispose();

            HOperatorSet.GetImageSize(ShowImage, out Width, out Height);


            HOperatorSet.ReadNccModel(ModelPath + ModelName + "NCC.ncm", out NCCModelId);
            HOperatorSet.ReadTuple(ModelPath + "ratio.tup", out ratio);
            HOperatorSet.ReadRegion(out RectangleCross, ModelPath + ModelName + "RectangleCross.hobj");
            HOperatorSet.ReadRegion(out RectangleSide, ModelPath + ModelName + "RectangleSide.hobj");
            HOperatorSet.ReadRegion(out RectangleUp, ModelPath + ModelName + "RectangleUp.hobj");
            HOperatorSet.ReadRegion(out RectangleDown, ModelPath + ModelName + "RectangleDown.hobj");


            double.TryParse(IniAPI.INIGetStringValue(ModelPath + ModelName + "FitCoordConfig.ini", "拍照", "DX", "0"), out Xoffset);
            double.TryParse(IniAPI.INIGetStringValue(ModelPath + ModelName + "FitCoordConfig.ini", "拍照", "DY", "0"), out Yoffset);



            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetDraw("margin");
            //if (ShowImage.IsInitialized())
            //{
            //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ShowImage);
            //}
            HOperatorSet.FindNccModel(ShowImage, NCCModelId, (new HTuple(-40)).TupleRad()
            , (new HTuple(80)).TupleRad(), 0.9, 1, 0.5, "true", 0, out MatchingRow, out MatchingCol,
            out MatchingAngle, out MatchingScore);
            HOperatorSet.BinaryThreshold(ShowImage, out ThresholdRegion, "max_separability", "dark", out UsedThreshold);

            HOperatorSet.Connection(ThresholdRegion, out ThresholdRegion);

            HOperatorSet.SelectShapeStd(ThresholdRegion, out ThresholdRegion, "max_area", 70);


            //****************************安全大小*************************//
            HTuple RegionInfo = null, RegionArea = null, RegionRow = null, RegionCol = null;
            HOperatorSet.AreaCenter(ThresholdRegion, out RegionArea, out RegionRow, out RegionCol);
            HOperatorSet.ReadTuple(ModelPath + ModelName + "RegionInfo.tup", out RegionInfo);

            try
            {
                if ((int)(MatchingScore.TupleLength()) == 1 && ((int)(new HTuple(RegionArea.TupleLess(RegionInfo * (1 + 0.2)))) != 0))
                {
                    HOperatorSet.AreaCenter(RectangleCross, out RetangleArea, out RetangleRow, out RetangleColumn);

                    HOperatorSet.VectorAngleToRigid(RetangleRow, RetangleColumn, 0, MatchingRow, MatchingCol, MatchingAngle, out RegionHomMat2D);

                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("green");
                    HOperatorSet.AffineTransRegion(RectangleCross, out RectangleCross, RegionHomMat2D, "nearest_neighbor");
                    //if (RectangleCross.IsInitialized())
                    //{
                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(RectangleCross);
                    //}
                    Mod_sys.Instance.gfrmAutoRun.txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " -->匹配分数:" + MatchingScore.ToString() + "\r\n";

                    //************************定位**********************//


                    HOperatorSet.AffineTransRegion(RectangleSide, out RectangleSide, RegionHomMat2D, "nearest_neighbor");
                    HOperatorSet.AffineTransRegion(RectangleUp, out RectangleUp, RegionHomMat2D, "nearest_neighbor");
                    HOperatorSet.AffineTransRegion(RectangleDown, out RectangleDown, RegionHomMat2D, "nearest_neighbor");

                    //*******************获取侧边边缘线********************//
                    HOperatorSet.ReduceDomain(ShowImage, RectangleSide, out ImageReduced);
                    HOperatorSet.GrayClosingRect(ImageReduced, out ImageReduced, 11, 3);
                    HOperatorSet.Emphasize(ImageReduced, out ImageReduced, 7, 7, 1);
                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("yellow");

                    //if (RectangleSide.IsInitialized())
                    //{
                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(RectangleSide);
                    //}
                    HOperatorSet.EdgesSubPix(ImageReduced, out EdgesSide, "canny", 4, 20,
                        40);

                    HOperatorSet.SmallestRectangle2(RectangleSide, out row, out Col, out phi, out L1, out L2);

                    HOperatorSet.SelectShapeXld(EdgesSide, out EdgesSide, "contlength",
                  "and", L2 * 2 * 0.1, L2 * 3);
                    HOperatorSet.UnionCollinearContoursXld(EdgesSide, out EdgesSide, L2 * 2 * 0.4, L2 * 2 * 0.2, 10, 0.1, "attr_keep");
                    HOperatorSet.SelectShapeXld(EdgesSide, out EdgesSide, "contlength",
                     "and", L2 * 2 * 0.5, L2 * 3);

                    HOperatorSet.SortContoursXld(EdgesSide, out EdgesSide, "upper_left", "true", "column");

                    HOperatorSet.SelectObj(EdgesSide, out SelectedXLD1, 1);
                    HOperatorSet.FitLineContourXld(SelectedXLD1, "tukey", -1, 0, 5, 2,
                        out RowBegin1, out ColBegin1, out RowEnd1, out ColEnd1, out Nr1,
                        out Nc1, out Dist1);

                    HOperatorSet.SelectObj(EdgesSide, out SelectedXLD2, 2);
                    HOperatorSet.FitLineContourXld(SelectedXLD2, "tukey", -1, 0, 5, 2,
                     out RowBegin2, out ColBegin2, out RowEnd2, out ColEnd2, out Nr2,
                     out Nc2, out Dist2);
                    if ((int)(RowBegin1.TupleLength()) == 1 && (int)(RowBegin2.TupleLength()) == 1)
                    {
                        Mod_sys.Instance.gfrmAutoRun.txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " -->侧边边缘提取成功" + "\r\n";

                    }
                    else
                    {
                        Mod_sys.Instance.gfrmAutoRun.txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " -->侧边边缘提取失败" + "\r\n";
                        throw new Exception();

                    }
                    HOperatorSet.AngleLx(RowBegin1, ColBegin1, RowEnd1, ColEnd1, out Deg1);
                    //HOperatorSet.GenRectangle2(out ShowLine1, RowBegin1 + (RowEnd1 - RowBegin1) / 2.0, ColBegin1 + (ColEnd1 - ColBegin1) / 2.0, Deg1, 1000, 1);
                    HOperatorSet.GenContourPolygonXld(out ShowLine1,
                        (new HTuple(RowBegin1 + (RowEnd1 - RowBegin1) / 2.0 - 1000 * (new HTuple(-Deg1)).TupleSin())).TupleConcat(RowBegin1 + (RowEnd1 - RowBegin1) / 2.0 + 1000 * (new HTuple(-Deg1)).TupleSin()),
                        (new HTuple(ColBegin1 + (ColEnd1 - ColBegin1) / 2.0 - 1000 * (new HTuple(-Deg1)).TupleCos())).TupleConcat(ColBegin1 + (ColEnd1 - ColBegin1) / 2.0 + 1000 * (new HTuple(-Deg1)).TupleCos()));

                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("blue");

                    //if (ShowLine1.IsInitialized())
                    //{
                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ShowLine1);
                    //}
                    HOperatorSet.TupleDeg(Deg1, out Deg1);
                    if ((int)(new HTuple(Deg1.TupleLess(0))) != 0)
                    {
                        Deg1 = Deg1 + 180;
                    }

                    HOperatorSet.AngleLx(RowBegin2, ColBegin2, RowEnd2, ColEnd2, out Deg2);
                    //HOperatorSet.GenRectangle2(out ShowLine2, RowBegin2 + (RowEnd2 - RowBegin2) / 2.0, ColBegin2 + (ColEnd2 - ColBegin2) / 2.0, Deg2, 1000, 1);
                    HOperatorSet.GenContourPolygonXld(out ShowLine2,
                 (new HTuple(RowBegin2 + (RowEnd2 - RowBegin2) / 2.0 - 1000 * (new HTuple(-Deg2)).TupleSin())).TupleConcat(RowBegin2 + (RowEnd2 - RowBegin2) / 2.0 + 1000 * (new HTuple(-Deg2)).TupleSin()),
                 (new HTuple(ColBegin2 + (ColEnd2 - ColBegin2) / 2.0 - 1000 * (new HTuple(-Deg2)).TupleCos())).TupleConcat(ColBegin2 + (ColEnd2 - ColBegin2) / 2.0 + 1000 * (new HTuple(-Deg2)).TupleCos()));


                    //if (ShowLine2.IsInitialized())
                    //{
                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ShowLine2);
                    //}
                    HOperatorSet.TupleDeg(Deg2, out Deg2);
                    if ((int)(new HTuple(Deg2.TupleLess(0))) != 0)
                    {
                        Deg2 = Deg2 + 180;
                    }

                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("red");

                    //if (EdgesSide.IsInitialized())
                    //{
                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(EdgesSide);
                    //}


                    HOperatorSet.GetContourXld(SelectedXLD1, out Row1, out Col1);

                    HOperatorSet.DistancePl(Row1, Col1, RowBegin2, ColBegin2, RowEnd2, ColEnd2, out CellWidth);

                    CellWidth = CellWidth.TupleMean();
                    ResultData.Width = CellWidth / ratio;
                    //*******************获取顶部边缘线********************//

                    HOperatorSet.ReduceDomain(ShowImage, RectangleUp, out ImageReduced);

                    HOperatorSet.GrayClosingRect(ImageReduced, out ImageReduced, 3, 11);
                    HOperatorSet.Emphasize(ImageReduced, out ImageReduced, 7, 7, 1);

                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("yellow");

                    //if (RectangleUp.IsInitialized())
                    //{
                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(RectangleUp);
                    //}
                    HOperatorSet.EdgesSubPix(ImageReduced, out EdgesUp, "canny", 3, 30, 50);


                    HOperatorSet.SegmentContoursXld(EdgesUp, out EdgesUp, "lines", 5, 4, 2);

                    HOperatorSet.SmallestRectangle2(RectangleUp, out row, out Col, out phi, out L1, out L2);

                    HOperatorSet.SelectShapeXld(EdgesUp, out EdgesUp, "contlength",
                     "and", L1 * 2 * 0.1, L1 * 3);
                    HOperatorSet.UnionCollinearContoursXld(EdgesUp, out EdgesUp, L1 * 2 * 0.4, L1 * 2 * 0.2, 15, 0.3, "attr_keep");

                    HOperatorSet.UnionAdjacentContoursXld(EdgesUp, out EdgesUp, L1 * 2 * 0.1, 5, "attr_keep");

                    HOperatorSet.SelectShapeXld(EdgesUp, out EdgesUp, "contlength", "and", L1 * 2 * 0.5, L1 * 3);
                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("red");

                    //if (EdgesUp.IsInitialized())
                    //{
                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(EdgesUp);
                    //}
                    HOperatorSet.FitLineContourXld(EdgesUp, "tukey", -1, 0, 5, 2,
                      out RowBegin3, out ColBegin3, out RowEnd3, out ColEnd3, out Nr3,
                      out Nc3, out Dist3);
                    if ((int)(RowBegin3.TupleLength()) == 1)
                    {
                        Mod_sys.Instance.gfrmAutoRun.txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " -->顶部边缘提取成功" + "\r\n";

                    }
                    else
                    {
                        Mod_sys.Instance.gfrmAutoRun.txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " -->顶部边缘提取失败" + "\r\n";
                        throw new Exception();

                    }
                    HOperatorSet.AngleLx(RowBegin3, ColBegin3, RowEnd3, ColEnd3, out Deg3);
                    //HOperatorSet.GenRectangle2(out ShowLine3, RowBegin3 + (RowEnd3 - RowBegin3) / 2.0, ColBegin3 + (ColEnd3 - ColBegin3) / 2.0, Deg3, 1000, 1);
                    HOperatorSet.GenContourPolygonXld(out ShowLine3,
                 (new HTuple(RowBegin3 + (RowEnd3 - RowBegin3) / 2.0 - 1000 * (new HTuple(-Deg3)).TupleSin())).TupleConcat(RowBegin3 + (RowEnd3 - RowBegin3) / 2.0 + 1000 * (new HTuple(-Deg3)).TupleSin()),
                 (new HTuple(ColBegin3 + (ColEnd3 - ColBegin3) / 2.0 - 1000 * (new HTuple(-Deg3)).TupleCos())).TupleConcat(ColBegin3 + (ColEnd3 - ColBegin3) / 2.0 + 1000 * (new HTuple(-Deg3)).TupleCos()));

                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("blue");

                    //if (ShowLine3.IsInitialized())
                    //{
                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ShowLine3);
                    //}


                    //*******************获取底部边缘线********************//

                    HOperatorSet.ReduceDomain(ShowImage, RectangleDown, out ImageReduced);

                    HOperatorSet.GrayClosingRect(ImageReduced, out ImageReduced, 3, 11);
                    HOperatorSet.Emphasize(ImageReduced, out ImageReduced, 7, 7, 1);

                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("yellow");

                    //if (RectangleDown.IsInitialized())
                    //{
                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(RectangleDown);
                    //}
                    HOperatorSet.EdgesSubPix(ImageReduced, out EdgesDown, "canny", 3, 30, 50);

                    HOperatorSet.SmallestRectangle2(RectangleDown, out row, out Col, out phi, out L1, out L2);

                    HOperatorSet.SelectShapeXld(EdgesDown, out EdgesDown, "contlength",
                     "and", L1 * 2 * 0.1, L1 * 3);

                    HOperatorSet.UnionCollinearContoursXld(EdgesDown, out EdgesDown, L1 * 2 * 0.4, L1 * 2 * 0.2, 10, 0.1, "attr_keep");

                    HOperatorSet.UnionAdjacentContoursXld(EdgesDown, out EdgesDown, L1 * 2 * 0.1, 5, "attr_keep");


                    HOperatorSet.SelectShapeXld(EdgesDown, out EdgesDown, "contlength", "and", L1 * 2 * 0.6, L1 * 3);

                    HOperatorSet.FitLineContourXld(EdgesDown, "tukey", -1, 0, 5, 2,
                      out RowBegin4, out ColBegin4, out RowEnd4, out ColEnd4, out Nr4,
                      out Nc4, out Dist4);
                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("red");

                    //if (EdgesDown.IsInitialized())
                    //{
                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(EdgesDown);
                    //}
                    if ((int)(RowBegin4.TupleLength()) == 1)
                    {
                        Mod_sys.Instance.gfrmAutoRun.txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " -->底部边缘提取成功" + "\r\n";

                    }
                    else
                    {
                        Mod_sys.Instance.gfrmAutoRun.txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " -->底部边缘提取失败" + "\r\n";
                        throw new Exception();
                    }


                    HOperatorSet.AngleLx(RowBegin4, ColBegin4, RowEnd4, ColEnd4, out Deg4);
                    // HOperatorSet.GenRectangle2(out ShowLine4, RowBegin4 + (RowEnd4 - RowBegin4) / 2.0, ColBegin4 + (ColEnd4 - ColBegin4) / 2.0, Deg4, 1000, 1);
                    HOperatorSet.GenContourPolygonXld(out ShowLine4,
                     (new HTuple(RowBegin4 + (RowEnd4 - RowBegin4) / 2.0 - 1000 * (new HTuple(-Deg4)).TupleSin())).TupleConcat(RowBegin4 + (RowEnd4 - RowBegin4) / 2.0 + 1000 * (new HTuple(-Deg4)).TupleSin()),
                     (new HTuple(ColBegin4 + (ColEnd4 - ColBegin4) / 2.0 - 1000 * (new HTuple(-Deg4)).TupleCos())).TupleConcat(ColBegin4 + (ColEnd4 - ColBegin4) / 2.0 + 1000 * (new HTuple(-Deg4)).TupleCos()));


                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("blue");

                    //if (ShowLine4.IsInitialized())
                    //{
                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ShowLine4);
                    //}


                    //*******************获取边角交点********************//
                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("green");

                    HOperatorSet.IntersectionLines(RowBegin1, ColBegin1, RowEnd1, ColEnd1,
                    RowBegin3, ColBegin3, RowEnd3, ColEnd3, out CrossRow1, out CrossColumn1, out IsOverlapping);
                    HOperatorSet.GenCrossContourXld(out ResultCross1, CrossRow1, CrossColumn1, 100, (new HTuple(45)).TupleRad());
                    //if (ResultCross1.IsInitialized())
                    //{
                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ResultCross1);
                    //}
                    HOperatorSet.IntersectionLines(RowBegin2, ColBegin2, RowEnd2, ColEnd2,
                    RowBegin4, ColBegin4, RowEnd4, ColEnd4, out CrossRow2, out CrossColumn2, out IsOverlapping);
                    HOperatorSet.GenCrossContourXld(out ResultCross2, CrossRow2, CrossColumn2, 100, (new HTuple(45)).TupleRad());
                    //if (ResultCross2.IsInitialized())
                    //{
                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ResultCross2);
                    //}
                    //HOperatorSet.GenCrossContourXld(out ResultCross3, CrossRow1 + ((CrossRow2 - CrossRow1) / 2.0), CrossColumn1 + ((CrossColumn2 - CrossColumn1) / 2.0), 100, (new HTuple(45)).TupleRad());
                    //if (ResultCross3.IsInitialized())
                    //{
                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ResultCross3);
                    //}
                    DegMean = (Deg1 + Deg2) / 2.0;
                    Rad = DegMean.TupleRad();


                    dh = Xoffset * ratio;
                    dv = Yoffset * ratio;

                    dx = (dh * (Rad.TupleSin())) + (dv * (Rad.TupleCos()));
                    dy = (dh * (Rad.TupleCos())) - (dv * (Rad.TupleSin()));

                    HOperatorSet.GenCrossContourXld(out ResultCross4, dy + CrossRow1 + ((CrossRow2 - CrossRow1) / 2.0),
                        dx + CrossColumn1 + ((CrossColumn2 - CrossColumn1) / 2.0), 100, (new HTuple(45)).TupleRad());
                    //if (ResultCross4.IsInitialized())
                    //{
                    //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ResultCross4);
                    //}

                    ResultData.MedM = 1;
                    ResultData.MedLM = 1;
                    HOperatorSet.AffineTransPixel(TranHomMat2D, dy + CrossRow1 + ((CrossRow2 - CrossRow1) / 2.0), dx + CrossColumn1 + ((CrossColumn2 - CrossColumn1) / 2.0),
                        out ResultRow, out ResultColumn);
                    ResultData.X = ResultRow;
                    ResultData.Y = ResultColumn;
                    ResultData.U = DegMean - 90;
                    ResultData.located = true;
                }
                else
                {
                    Mod_sys.Instance.gfrmAutoRun.txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " -->模板匹配失败" + "\r\n";
                    throw new Exception();
                }
            }
            catch
            {

                if (Mod_sys.Instance.gfrmAutoRun.UNSAFT_CCD_MODE)
                {
                    HOperatorSet.Threshold(ShowImage, out ThresholdRegion, 0, 50); HOperatorSet.Connection(ThresholdRegion, out ThresholdRegion);
                    HOperatorSet.SelectShapeStd(ThresholdRegion, out ThresholdRegion, "max_area", 70);
                    HOperatorSet.AreaCenter(ThresholdRegion, out CrossArea, out CrossRow, out CrossColumn);
                    if (((int)(new HTuple(CrossArea.TupleLess(RegionInfo * 0.5))) == 0))
                    {

                        dv = Yoffset * ratio;
                        HOperatorSet.AffineTransPixel(TranHomMat2D, CrossRow - dv, CrossColumn, out ResultRow, out ResultColumn);
                        HOperatorSet.GenCrossContourXld(out ResultCrossW, CrossRow - dv, CrossColumn, 75, (new HTuple(45)).TupleRad());
                        Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("green");
                        //if (ResultCrossW.IsInitialized())
                        //{
                        //    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.DispObj(ResultCrossW);
                        //}
                        ResultData.MedM = 0;
                        ResultData.MedLM = 0;
                        ResultData.X = ResultRow;
                        ResultData.Y = ResultColumn;
                        ResultData.U = 0;
                        ResultData.Width = 0;
                        ResultData.located = true;
                    }
                    else
                    {
                        ResultData.MedM = 0;
                        ResultData.MedLM = 0;
                        ResultData.X = 0;
                        ResultData.Y = 0;
                        ResultData.U = 0;
                        ResultData.Width = 0;
                        ResultData.located = false;
                    }

                }
                else
                {
                    ResultData.MedM = 0;
                    ResultData.MedLM = 0;
                    ResultData.X = 0;
                    ResultData.Y = 0;
                    ResultData.U = 0;
                    ResultData.Width = 0;
                    ResultData.located = false;

                }

            }



            HobjectToHimage(ShowImage, ref m_Image);
            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.Image = m_Image;
            SendCCDResult(ResultData);

            Mod_sys.Instance.gfrmAutoRun.BeginInvoke(new MethodInvoker(delegate
            {
                Mod_sys.Instance.gfrmAutoRun.SaveLog();
            }));
            HOperatorSet.ClearNccModel(NCCModelId);
            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.DispImageFit();


        }
        /// <summary>
        /// 坐标拟合
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="ModelName"></param>
        private void FitCoordination(HObject Image)
        {
            HTuple RetangleArea = null, RetangleRow = null, RetangleColumn = null;
            HTuple ResultRow = null, ResultColumn = null;
            HTuple NCCModelId = null;
            HTuple RegionHomMat2D = null;
            HTuple ratio = null, Deg2 = null, Rad = null, dh = null, dv = null, dx = null, dy = null;
            double Xoffset = Convert.ToDouble(X);
            double Yoffset = Convert.ToDouble(Y);

            HTuple Deg1 = null, DegMean = null;
            HTuple MatchingRow = null, MatchingCol = null, MatchingAngle = null, MatchingScore = null;
            HTuple IsOverlapping = null, CrossRow1 = null, CrossColumn1 = null, CrossRow2 = null, CrossColumn2 = null;
            HTuple row = null, Col = null, phi = null, L1 = null, L2 = null;

            HTuple RowBegin1 = null, ColBegin1 = null, RowEnd1 = null, ColEnd1 = null, Nr1 = null, Nc1 = null, Dist1 = null;
            HTuple RowBegin2 = null, ColBegin2 = null, RowEnd2 = null, ColEnd2 = null, Nr2 = null, Nc2 = null, Dist2 = null;
            HTuple RowBegin3 = null, ColBegin3 = null, RowEnd3 = null, ColEnd3 = null, Nr3 = null, Nc3 = null, Dist3 = null;
            HTuple RowBegin4 = null, ColBegin4 = null, RowEnd4 = null, ColEnd4 = null, Nr4 = null, Nc4 = null, Dist4 = null;

            HObject ResultCross = null;
            HObject RectangleCross = null;
            HObject RectangleSide = null, RectangleUp = null, RectangleDown = null, ImageReduced = null, Edges = null, SelectedXLD1 = null, SelectedXLD2 = null;


            HOperatorSet.GenEmptyObj(out RectangleSide);
            HOperatorSet.GenEmptyObj(out RectangleUp);
            HOperatorSet.GenEmptyObj(out RectangleDown);
            HOperatorSet.GenEmptyObj(out ImageReduced);
            HOperatorSet.GenEmptyObj(out Edges);
            HOperatorSet.GenEmptyObj(out SelectedXLD1);
            HOperatorSet.GenEmptyObj(out SelectedXLD2);
            HOperatorSet.GenEmptyObj(out ResultCross);
            HOperatorSet.GenEmptyObj(out RectangleCross);


            HOperatorSet.ReadTuple(ModelPath + "ratio.tup", out ratio);
            HOperatorSet.ReadNccModel(ModelPath + ModelName + "NCC.ncm", out NCCModelId);
            HOperatorSet.ReadRegion(out RectangleCross, ModelPath + ModelName + "RectangleCross.hobj");
            HOperatorSet.ReadRegion(out RectangleSide, ModelPath + ModelName + "RectangleSide.hobj");
            HOperatorSet.ReadRegion(out RectangleUp, ModelPath + ModelName + "RectangleUp.hobj");
            HOperatorSet.ReadRegion(out RectangleDown, ModelPath + ModelName + "RectangleDown.hobj");


            double.TryParse(IniAPI.INIGetStringValue(ModelPath + ModelName + "FitCoordConfig.ini", "拍照", "DX", "0"), out Xoffset);
            double.TryParse(IniAPI.INIGetStringValue(ModelPath + ModelName + "FitCoordConfig.ini", "拍照", "DY", "0"), out Yoffset);


            HOperatorSet.FindNccModel(Image, NCCModelId, (new HTuple(-40)).TupleRad()
            , (new HTuple(80)).TupleRad(), 0.9, 1, 0.5, "true", 0, out MatchingRow, out MatchingCol,
            out MatchingAngle, out MatchingScore);

            if ((int)(MatchingScore.TupleLength()) == 1)
            {

                try
                {
                    HOperatorSet.AreaCenter(RectangleCross, out RetangleArea, out RetangleRow, out RetangleColumn);

                    HOperatorSet.VectorAngleToRigid(RetangleRow, RetangleColumn, 0, MatchingRow, MatchingCol, MatchingAngle, out RegionHomMat2D);

                    HOperatorSet.AffineTransRegion(RectangleSide, out RectangleSide, RegionHomMat2D, "nearest_neighbor");
                    HOperatorSet.AffineTransRegion(RectangleUp, out RectangleUp, RegionHomMat2D, "nearest_neighbor");
                    HOperatorSet.AffineTransRegion(RectangleDown, out RectangleDown, RegionHomMat2D, "nearest_neighbor");

                    //*******************获取侧边边缘线********************//
                    HOperatorSet.ReduceDomain(Image, RectangleSide, out ImageReduced);
                    HOperatorSet.GrayClosingRect(ImageReduced, out ImageReduced, 11, 3);
                    HOperatorSet.Emphasize(ImageReduced, out ImageReduced, 7, 7, 1);
                    HOperatorSet.EdgesSubPix(ImageReduced, out Edges, "canny", 4, 20,
                        40);

                    HOperatorSet.SmallestRectangle2(RectangleSide, out row, out Col, out phi, out L1, out L2);

                    HOperatorSet.SelectShapeXld(Edges, out Edges, "contlength",
                       "and", L2 * 2 * 0.1, L2 * 3);
                    HOperatorSet.UnionCollinearContoursXld(Edges, out Edges, L2 * 2 * 0.4, L2 * 2 * 0.2, 10, 0.1, "attr_keep");
                    HOperatorSet.SelectShapeXld(Edges, out Edges, "contlength",
                     "and", L2 * 2 * 0.5, L2 * 3);

                    HOperatorSet.SortContoursXld(Edges, out Edges, "upper_left", "true", "column");

                    HOperatorSet.SelectObj(Edges, out SelectedXLD1, 1);
                    HOperatorSet.FitLineContourXld(SelectedXLD1, "tukey", -1, 0, 5, 2,
                        out RowBegin1, out ColBegin1, out RowEnd1, out ColEnd1, out Nr1,
                        out Nc1, out Dist1);

                    HOperatorSet.SelectObj(Edges, out SelectedXLD2, 2);
                    HOperatorSet.FitLineContourXld(SelectedXLD2, "tukey", -1, 0, 5, 2,
                     out RowBegin2, out ColBegin2, out RowEnd2, out ColEnd2, out Nr2,
                     out Nc2, out Dist2);

                    HOperatorSet.AngleLx(RowBegin1, ColBegin1, RowEnd1, ColEnd1, out Deg1);
                    HOperatorSet.TupleDeg(Deg1, out Deg1);
                    if ((int)(new HTuple(Deg1.TupleLess(0))) != 0)
                    {
                        Deg1 = Deg1 + 180;
                    }

                    HOperatorSet.AngleLx(RowBegin2, ColBegin2, RowEnd2, ColEnd2, out Deg2);
                    HOperatorSet.TupleDeg(Deg2, out Deg2);
                    if ((int)(new HTuple(Deg2.TupleLess(0))) != 0)
                    {
                        Deg2 = Deg2 + 180;
                    }

                    HOperatorSet.SetColor(ModelWindowHandle, "red");
                    HOperatorSet.DispObj(Edges, ModelWindowHandle);

                    //*******************获取顶部边缘线********************//

                    HOperatorSet.ReduceDomain(Image, RectangleUp, out ImageReduced);

                    HOperatorSet.GrayClosingRect(ImageReduced, out ImageReduced, 3, 11);
                    HOperatorSet.Emphasize(ImageReduced, out ImageReduced, 7, 7, 1);

                    HOperatorSet.EdgesSubPix(ImageReduced, out Edges, "canny", 2, 20,
                        40);
                    HOperatorSet.SegmentContoursXld(Edges, out Edges, "lines", 5, 4, 2);

                    HOperatorSet.SmallestRectangle2(RectangleUp, out row, out Col, out phi, out L1, out L2);

                    HOperatorSet.SelectShapeXld(Edges, out Edges, "contlength",
                     "and", L1 * 2 * 0.1, L1 * 3);

                    HOperatorSet.UnionCollinearContoursXld(Edges, out Edges, L1 * 2 * 0.4, L1 * 2 * 0.2, 10, 0.1, "attr_keep");
                    HOperatorSet.SelectShapeXld(Edges, out Edges, "contlength",
                   "and", L1 * 2 * 0.3, L1 * 3);
                    HOperatorSet.SetColor(ModelWindowHandle, "red");
                    HOperatorSet.DispObj(Edges, ModelWindowHandle);

                    HOperatorSet.FitLineContourXld(Edges, "tukey", -1, 0, 5, 2,
                      out RowBegin3, out ColBegin3, out RowEnd3, out ColEnd3, out Nr3,
                      out Nc3, out Dist3);



                    //*******************获取底部边缘线********************//

                    HOperatorSet.ReduceDomain(Image, RectangleDown, out ImageReduced);

                    HOperatorSet.GrayClosingRect(ImageReduced, out ImageReduced, 3, 11);
                    HOperatorSet.Emphasize(ImageReduced, out ImageReduced, 7, 7, 1);

                    HOperatorSet.EdgesSubPix(ImageReduced, out Edges, "canny", 2, 20,
                        40);
                    HOperatorSet.SmallestRectangle2(RectangleDown, out row, out Col, out phi, out L1, out L2);

                    HOperatorSet.SelectShapeXld(Edges, out Edges, "contlength",
                     "and", L1 * 2 * 0.1, L1 * 3);

                    HOperatorSet.UnionCollinearContoursXld(Edges, out Edges, L1 * 2 * 0.4, L1 * 2 * 0.2, 10, 0.1, "attr_keep");
                    HOperatorSet.SelectShapeXld(Edges, out Edges, "contlength",
                   "and", L1 * 2 * 0.3, L1 * 3);

                    HOperatorSet.FitLineContourXld(Edges, "tukey", -1, 0, 5, 2,
                      out RowBegin4, out ColBegin4, out RowEnd4, out ColEnd4, out Nr4,
                      out Nc4, out Dist4);

                    HOperatorSet.SetColor(ModelWindowHandle, "red");
                    HOperatorSet.DispObj(Edges, ModelWindowHandle);
                    //*******************获取边角交点********************//
                    HOperatorSet.SetColor(ModelWindowHandle, "green");
                    HOperatorSet.IntersectionLines(RowBegin1, ColBegin1, RowEnd1, ColEnd1,
                    RowBegin3, ColBegin3, RowEnd3, ColEnd3, out CrossRow1, out CrossColumn1, out IsOverlapping);
                    HOperatorSet.GenCrossContourXld(out ResultCross, CrossRow1, CrossColumn1, 100, (new HTuple(45)).TupleRad());
                    HOperatorSet.DispObj(ResultCross, ModelWindowHandle);

                    HOperatorSet.IntersectionLines(RowBegin2, ColBegin2, RowEnd2, ColEnd2,
                    RowBegin4, ColBegin4, RowEnd4, ColEnd4, out CrossRow2, out CrossColumn2, out IsOverlapping);
                    HOperatorSet.GenCrossContourXld(out ResultCross, CrossRow2, CrossColumn2, 100, (new HTuple(45)).TupleRad());
                    HOperatorSet.DispObj(ResultCross, ModelWindowHandle);





                    HOperatorSet.GenCrossContourXld(out ResultCross, CrossRow1 + ((CrossRow2 - CrossRow1) / 2.0), CrossColumn1 + ((CrossColumn2 - CrossColumn1) / 2.0), 100, (new HTuple(45)).TupleRad());
                    HOperatorSet.DispObj(ResultCross, ModelWindowHandle);

                    DegMean = (Deg1 + Deg2) / 2.0;
                    Rad = DegMean.TupleRad();


                    dh = Xoffset * ratio;
                    dv = Yoffset * ratio;

                    dx = (dh * (Rad.TupleSin())) + (dv * (Rad.TupleCos()));
                    dy = (dh * (Rad.TupleCos())) - (dv * (Rad.TupleSin()));

                    HOperatorSet.GenCrossContourXld(out ResultCross, dy + CrossRow1 + ((CrossRow2 - CrossRow1) / 2.0),
                        dx + CrossColumn1 + ((CrossColumn2 - CrossColumn1) / 2.0), 100, (new HTuple(45)).TupleRad());
                    HOperatorSet.DispObj(ResultCross, ModelWindowHandle);



                    HOperatorSet.AffineTransPixel(TranHomMat2D, dy + CrossRow1 + ((CrossRow2 - CrossRow1) / 2.0), dx + CrossColumn1 + ((CrossColumn2 - CrossColumn1) / 2.0),
                        out ResultRow, out ResultColumn);


                    double DX, DY, CCDX, CCDY, DU;
                    DX = ResultRow;
                    DY = ResultColumn;
                    DU = DegMean - 90;

                    CCDX = Convert.ToDouble(IniAPI.INIGetStringValue(ModelPath + ModelName + "RunCoordConfig.ini", "A-拍照", "X", "-1"));
                    CCDY = Convert.ToDouble(IniAPI.INIGetStringValue(ModelPath + ModelName + "RunCoordConfig.ini", "A-拍照", "Y", "-1"));

                    Mod_sys.Instance.gfrmProdChange.txt_fitx.Text = (DX - CCDX).ToString("#0.000");
                    Mod_sys.Instance.gfrmProdChange.txt_fity.Text = (DY - CCDY).ToString("#0.000");
                    Mod_sys.Instance.gfrmProdChange.txt_fitu.Text = (DU - 0).ToString("#0.000");

                    IniAPI.INIWriteValue(ModelPath + ModelName + "FitCoordConfig.ini", "落料", "DX", (DX - CCDX).ToString("#0.000"));
                    IniAPI.INIWriteValue(ModelPath + ModelName + "FitCoordConfig.ini", "落料", "DY", (DY - CCDY).ToString("#0.000"));
                    IniAPI.INIWriteValue(ModelPath + ModelName + "FitCoordConfig.ini", "拍照", "DU", (DU - 0).ToString("#0.000"));

                }
                catch
                {

                }


            }


            RectangleSide.Dispose();
            RectangleUp.Dispose();
            RectangleDown.Dispose();
            ImageReduced.Dispose();
            Edges.Dispose();
            SelectedXLD1.Dispose();
            SelectedXLD2.Dispose();
            Image.Dispose();
            ResultCross.Dispose();
            RectangleCross.Dispose();
            HOperatorSet.ClearNccModel(NCCModelId);

        }

        //生成箭头显示
        public void gen_arrow_contour_xld(out HObject ho_Arrow, HTuple hv_Row1, HTuple hv_Column1,
        HTuple hv_Row2, HTuple hv_Column2, HTuple hv_HeadLength, HTuple hv_HeadWidth)
        {
            // Stack for temporary objects
            HObject[] OTemp = new HObject[20];

            // Local iconic variables

            HObject ho_TempArrow = null;

            // Local control variables

            HTuple hv_Length = null, hv_ZeroLengthIndices = null;
            HTuple hv_DR = null, hv_DC = null, hv_HalfHeadWidth = null;
            HTuple hv_RowP1 = null, hv_ColP1 = null, hv_RowP2 = null;
            HTuple hv_ColP2 = null, hv_Index = null;
            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_TempArrow);
            try
            {
                //This procedure generates arrow shaped XLD contours,
                //pointing from (Row1, Column1) to (Row2, Column2).
                //If starting and end point are identical, a contour consisting
                //of a single point is returned.
                //
                //input parameteres:
                //Row1, Column1: Coordinates of the arrows' starting points
                //Row2, Column2: Coordinates of the arrows' end points
                //HeadLength, HeadWidth: Size of the arrow heads in pixels
                //
                //output parameter:
                //Arrow: The resulting XLD contour
                //
                //The input tuples Row1, Column1, Row2, and Column2 have to be of
                //the same length.
                //HeadLength and HeadWidth either have to be of the same length as
                //Row1, Column1, Row2, and Column2 or have to be a single element.
                //If one of the above restrictions is violated, an error will occur.
                //
                //
                //Init
                ho_Arrow.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Arrow);
                //
                //Calculate the arrow length
                HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Length);
                //
                //Mark arrows with identical start and end point
                //(set Length to -1 to avoid division-by-zero exception)
                hv_ZeroLengthIndices = hv_Length.TupleFind(0);
                if ((int)(new HTuple(hv_ZeroLengthIndices.TupleNotEqual(-1))) != 0)
                {
                    if (hv_Length == null)
                        hv_Length = new HTuple();
                    hv_Length[hv_ZeroLengthIndices] = -1;
                }
                //
                //Calculate auxiliary variables.
                hv_DR = (1.0 * (hv_Row2 - hv_Row1)) / hv_Length;
                hv_DC = (1.0 * (hv_Column2 - hv_Column1)) / hv_Length;
                hv_HalfHeadWidth = hv_HeadWidth / 2.0;
                //
                //Calculate end points of the arrow head.
                hv_RowP1 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) + (hv_HalfHeadWidth * hv_DC);
                hv_ColP1 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) - (hv_HalfHeadWidth * hv_DR);
                hv_RowP2 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) - (hv_HalfHeadWidth * hv_DC);
                hv_ColP2 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) + (hv_HalfHeadWidth * hv_DR);
                //
                //Finally create output XLD contour for each input point pair
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Length.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
                {
                    if ((int)(new HTuple(((hv_Length.TupleSelect(hv_Index))).TupleEqual(-1))) != 0)
                    {
                        //Create_ single points for arrows with identical start and end point
                        ho_TempArrow.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_TempArrow, hv_Row1.TupleSelect(
                            hv_Index), hv_Column1.TupleSelect(hv_Index));
                    }
                    else
                    {
                        //Create arrow contour
                        ho_TempArrow.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_TempArrow, ((((((((((hv_Row1.TupleSelect(
                            hv_Index))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                            hv_RowP1.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                            hv_RowP2.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)),
                            ((((((((((hv_Column1.TupleSelect(hv_Index))).TupleConcat(hv_Column2.TupleSelect(
                            hv_Index)))).TupleConcat(hv_ColP1.TupleSelect(hv_Index)))).TupleConcat(
                            hv_Column2.TupleSelect(hv_Index)))).TupleConcat(hv_ColP2.TupleSelect(
                            hv_Index)))).TupleConcat(hv_Column2.TupleSelect(hv_Index)));
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_Arrow, ho_TempArrow, out ExpTmpOutVar_0);
                        ho_Arrow.Dispose();
                        ho_Arrow = ExpTmpOutVar_0;
                    }
                }
                ho_TempArrow.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_TempArrow.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void set_display_font(HTuple hv_WindowHandle, HTuple hv_Size, HTuple hv_Font,
        HTuple hv_Bold, HTuple hv_Slant)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_OS = null, hv_BufferWindowHandle = new HTuple();
            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_Scale = new HTuple(), hv_Exception = new HTuple();
            HTuple hv_SubFamily = new HTuple(), hv_Fonts = new HTuple();
            HTuple hv_SystemFonts = new HTuple(), hv_Guess = new HTuple();
            HTuple hv_I = new HTuple(), hv_Index = new HTuple(), hv_AllowedFontSizes = new HTuple();
            HTuple hv_Distances = new HTuple(), hv_Indices = new HTuple();
            HTuple hv_FontSelRegexp = new HTuple(), hv_FontsCourier = new HTuple();
            HTuple hv_Bold_COPY_INP_TMP = hv_Bold.Clone();
            HTuple hv_Font_COPY_INP_TMP = hv_Font.Clone();
            HTuple hv_Size_COPY_INP_TMP = hv_Size.Clone();
            HTuple hv_Slant_COPY_INP_TMP = hv_Slant.Clone();

            // Initialize local and output iconic variables 
            //This procedure sets the text font of the current window with
            //the specified attributes.
            //It is assumed that following fonts are installed on the system:
            //Windows: Courier New, Arial Times New Roman
            //Mac OS X: CourierNewPS, Arial, TimesNewRomanPS
            //Linux: courier, helvetica, times
            //Because fonts are displayed smaller on Linux than on Windows,
            //a scaling factor of 1.25 is used the get comparable results.
            //For Linux, only a limited number of font sizes is supported,
            //to get comparable results, it is recommended to use one of the
            //following sizes: 9, 11, 14, 16, 20, 27
            //(which will be mapped internally on Linux systems to 11, 14, 17, 20, 25, 34)
            //
            //Input parameters:
            //WindowHandle: The graphics window for which the font will be set
            //Size: The font size. If Size=-1, the default of 16 is used.
            //Bold: If set to 'true', a bold font is used
            //Slant: If set to 'true', a slanted font is used
            //
            HOperatorSet.GetSystem("operating_system", out hv_OS);
            // dev_get_preferences(...); only in hdevelop
            // dev_set_preferences(...); only in hdevelop
            if ((int)((new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(-1)))) != 0)
            {
                hv_Size_COPY_INP_TMP = 16;
            }
            if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
            {
                //Set font on Windows systems
                try
                {
                    //Check, if font scaling is switched on
                    //open_window(...);
                    HOperatorSet.SetFont(hv_WindowHandle, "-Consolas-16-*-0-*-*-1-");
                    HOperatorSet.GetStringExtents(hv_WindowHandle, "test_string", out hv_Ascent,
                        out hv_Descent, out hv_Width, out hv_Height);
                    //Expected width is 110
                    hv_Scale = 110.0 / hv_Width;
                    hv_Size_COPY_INP_TMP = ((hv_Size_COPY_INP_TMP * hv_Scale)).TupleInt();
                    //close_window(...);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //throw (Exception)
                }
                if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))).TupleOr(
                    new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Courier New";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Consolas";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Arial";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Times New Roman";
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = 1;
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = 0;
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = 1;
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = 0;
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, ((((((("-" + hv_Font_COPY_INP_TMP) + "-") + hv_Size_COPY_INP_TMP) + "-*-") + hv_Slant_COPY_INP_TMP) + "-*-*-") + hv_Bold_COPY_INP_TMP) + "-");
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //throw (Exception)
                }
            }
            else if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Dar"))) != 0)
            {
                //Set font on Mac OS X systems. Since OS X does not have a strict naming
                //scheme for font attributes, we use tables to determine the correct font
                //name.
                hv_SubFamily = 0;
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_SubFamily = hv_SubFamily.TupleBor(1);
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleNotEqual("false"))) != 0)
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_SubFamily = hv_SubFamily.TupleBor(2);
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleNotEqual("false"))) != 0)
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "Menlo-Regular";
                    hv_Fonts[1] = "Menlo-Italic";
                    hv_Fonts[2] = "Menlo-Bold";
                    hv_Fonts[3] = "Menlo-BoldItalic";
                }
                else if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))).TupleOr(
                    new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "CourierNewPSMT";
                    hv_Fonts[1] = "CourierNewPS-ItalicMT";
                    hv_Fonts[2] = "CourierNewPS-BoldMT";
                    hv_Fonts[3] = "CourierNewPS-BoldItalicMT";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "ArialMT";
                    hv_Fonts[1] = "Arial-ItalicMT";
                    hv_Fonts[2] = "Arial-BoldMT";
                    hv_Fonts[3] = "Arial-BoldItalicMT";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "TimesNewRomanPSMT";
                    hv_Fonts[1] = "TimesNewRomanPS-ItalicMT";
                    hv_Fonts[2] = "TimesNewRomanPS-BoldMT";
                    hv_Fonts[3] = "TimesNewRomanPS-BoldItalicMT";
                }
                else
                {
                    //Attempt to figure out which of the fonts installed on the system
                    //the user could have meant.
                    HOperatorSet.QueryFont(hv_WindowHandle, out hv_SystemFonts);
                    hv_Fonts = new HTuple();
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Regular");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "MT");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[0] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                    //Guess name of slanted font
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Italic");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-ItalicMT");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Oblique");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[1] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                    //Guess name of bold font
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Bold");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldMT");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[2] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                    //Guess name of bold slanted font
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldItalic");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldItalicMT");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldOblique");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[3] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                }
                hv_Font_COPY_INP_TMP = hv_Fonts.TupleSelect(hv_SubFamily);
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, (hv_Font_COPY_INP_TMP + "-") + hv_Size_COPY_INP_TMP);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //throw (Exception)
                }
            }
            else
            {
                //Set font for UNIX systems
                hv_Size_COPY_INP_TMP = hv_Size_COPY_INP_TMP * 1.25;
                hv_AllowedFontSizes = new HTuple();
                hv_AllowedFontSizes[0] = 11;
                hv_AllowedFontSizes[1] = 14;
                hv_AllowedFontSizes[2] = 17;
                hv_AllowedFontSizes[3] = 20;
                hv_AllowedFontSizes[4] = 25;
                hv_AllowedFontSizes[5] = 34;
                if ((int)(new HTuple(((hv_AllowedFontSizes.TupleFind(hv_Size_COPY_INP_TMP))).TupleEqual(
                    -1))) != 0)
                {
                    hv_Distances = ((hv_AllowedFontSizes - hv_Size_COPY_INP_TMP)).TupleAbs();
                    HOperatorSet.TupleSortIndex(hv_Distances, out hv_Indices);
                    hv_Size_COPY_INP_TMP = hv_AllowedFontSizes.TupleSelect(hv_Indices.TupleSelect(
                        0));
                }
                if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))).TupleOr(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual(
                    "Courier")))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "courier";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "helvetica";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "times";
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = "bold";
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = "medium";
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("times"))) != 0)
                    {
                        hv_Slant_COPY_INP_TMP = "i";
                    }
                    else
                    {
                        hv_Slant_COPY_INP_TMP = "o";
                    }
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = "r";
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, ((((((("-adobe-" + hv_Font_COPY_INP_TMP) + "-") + hv_Bold_COPY_INP_TMP) + "-") + hv_Slant_COPY_INP_TMP) + "-normal-*-") + hv_Size_COPY_INP_TMP) + "-*-*-*-*-*-*-*");
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    if ((int)((new HTuple(((hv_OS.TupleSubstr(0, 4))).TupleEqual("Linux"))).TupleAnd(
                        new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
                    {
                        HOperatorSet.QueryFont(hv_WindowHandle, out hv_Fonts);
                        hv_FontSelRegexp = (("^-[^-]*-[^-]*[Cc]ourier[^-]*-" + hv_Bold_COPY_INP_TMP) + "-") + hv_Slant_COPY_INP_TMP;
                        hv_FontsCourier = ((hv_Fonts.TupleRegexpSelect(hv_FontSelRegexp))).TupleRegexpMatch(
                            hv_FontSelRegexp);
                        if ((int)(new HTuple((new HTuple(hv_FontsCourier.TupleLength())).TupleEqual(
                            0))) != 0)
                        {
                            hv_Exception = "Wrong font name";
                            //throw (Exception)
                        }
                        else
                        {
                            try
                            {
                                HOperatorSet.SetFont(hv_WindowHandle, (((hv_FontsCourier.TupleSelect(
                                    0)) + "-normal-*-") + hv_Size_COPY_INP_TMP) + "-*-*-*-*-*-*-*");
                            }
                            // catch (Exception) 
                            catch (HalconException HDevExpDefaultException2)
                            {
                                HDevExpDefaultException2.ToHTuple(out hv_Exception);
                                //throw (Exception)
                            }
                        }
                    }
                    //throw (Exception)
                }
            }
            // dev_set_preferences(...); only in hdevelop

            return;
        }

        // Chapter: Graphics / Text
        // Short Description: This procedure writes a text message. 
        public void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem,
            HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
            HTuple hv_Row1Part = null, hv_Column1Part = null, hv_Row2Part = null;
            HTuple hv_Column2Part = null, hv_RowWin = null, hv_ColumnWin = null;
            HTuple hv_WidthWin = new HTuple(), hv_HeightWin = null;
            HTuple hv_MaxAscent = null, hv_MaxDescent = null, hv_MaxWidth = null;
            HTuple hv_MaxHeight = null, hv_R1 = new HTuple(), hv_C1 = new HTuple();
            HTuple hv_FactorRow = new HTuple(), hv_FactorColumn = new HTuple();
            HTuple hv_UseShadow = null, hv_ShadowColor = null, hv_Exception = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
            HTuple hv_W = new HTuple(), hv_H = new HTuple(), hv_FrameHeight = new HTuple();
            HTuple hv_FrameWidth = new HTuple(), hv_R2 = new HTuple();
            HTuple hv_C2 = new HTuple(), hv_DrawMode = new HTuple();
            HTuple hv_CurrentColor = new HTuple();
            HTuple hv_Box_COPY_INP_TMP = hv_Box.Clone();
            HTuple hv_Color_COPY_INP_TMP = hv_Color.Clone();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();
            HTuple hv_String_COPY_INP_TMP = hv_String.Clone();

            // Initialize local and output iconic variables 
            //This procedure displays text in a graphics window.
            //
            //Input parameters:
            //WindowHandle: The WindowHandle of the graphics window, where
            //   the message should be displayed
            //String: A tuple of strings containing the text message to be displayed
            //CoordSystem: If set to 'window', the text position is given
            //   with respect to the window coordinate system.
            //   If set to 'image', image coordinates are used.
            //   (This may be useful in zoomed images.)
            //Row: The row coordinate of the desired text position
            //   If set to -1, a default value of 12 is used.
            //Column: The column coordinate of the desired text position
            //   If set to -1, a default value of 12 is used.
            //Color: defines the color of the text as string.
            //   If set to [], '' or 'auto' the currently set color is used.
            //   If a tuple of strings is passed, the colors are used cyclically
            //   for each new textline.
            //Box: If Box[0] is set to 'true', the text is written within an orange box.
            //     If set to' false', no box is displayed.
            //     If set to a color string (e.g. 'white', '#FF00CC', etc.),
            //       the text is written in a box of that color.
            //     An optional second value for Box (Box[1]) controls if a shadow is displayed:
            //       'true' -> display a shadow in a default color
            //       'false' -> display no shadow (same as if no second value is given)
            //       otherwise -> use given string as color string for the shadow color
            //
            //Prepare window
            HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);
            HOperatorSet.GetPart(hv_WindowHandle, out hv_Row1Part, out hv_Column1Part,
                out hv_Row2Part, out hv_Column2Part);
            HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowWin, out hv_ColumnWin,
                out hv_WidthWin, out hv_HeightWin);
            HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_HeightWin - 1, hv_WidthWin - 1);
            //
            //default settings
            if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Row_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Column_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(new HTuple()))) != 0)
            {
                hv_Color_COPY_INP_TMP = "";
            }
            //
            hv_String_COPY_INP_TMP = ((("" + hv_String_COPY_INP_TMP) + "")).TupleSplit("\n");
            //
            //Estimate extentions of text depending on font size.
            HOperatorSet.GetFontExtents(hv_WindowHandle, out hv_MaxAscent, out hv_MaxDescent,
                out hv_MaxWidth, out hv_MaxHeight);
            if ((int)(new HTuple(hv_CoordSystem.TupleEqual("window"))) != 0)
            {
                hv_R1 = hv_Row_COPY_INP_TMP.Clone();
                hv_C1 = hv_Column_COPY_INP_TMP.Clone();
            }
            else
            {
                //Transform image to window coordinates
                hv_FactorRow = (1.0 * hv_HeightWin) / ((hv_Row2Part - hv_Row1Part) + 1);
                hv_FactorColumn = (1.0 * hv_WidthWin) / ((hv_Column2Part - hv_Column1Part) + 1);
                hv_R1 = ((hv_Row_COPY_INP_TMP - hv_Row1Part) + 0.5) * hv_FactorRow;
                hv_C1 = ((hv_Column_COPY_INP_TMP - hv_Column1Part) + 0.5) * hv_FactorColumn;
            }
            //
            //Display text box depending on text size
            hv_UseShadow = 1;
            hv_ShadowColor = "gray";
            if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(0))).TupleEqual("true"))) != 0)
            {
                if (hv_Box_COPY_INP_TMP == null)
                    hv_Box_COPY_INP_TMP = new HTuple();
                hv_Box_COPY_INP_TMP[0] = "#fce9d4";
                hv_ShadowColor = "#f28d26";
            }
            if ((int)(new HTuple((new HTuple(hv_Box_COPY_INP_TMP.TupleLength())).TupleGreater(
                1))) != 0)
            {
                if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(1))).TupleEqual("true"))) != 0)
                {
                    //Use default ShadowColor set above
                }
                else if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(1))).TupleEqual(
                    "false"))) != 0)
                {
                    hv_UseShadow = 0;
                }
                else
                {
                    hv_ShadowColor = hv_Box_COPY_INP_TMP[1];
                    //Valid color?
                    try
                    {
                        HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(
                            1));
                    }
                    // catch (Exception) 
                    catch (HalconException HDevExpDefaultException1)
                    {
                        HDevExpDefaultException1.ToHTuple(out hv_Exception);
                        hv_Exception = "Wrong value of control parameter Box[1] (must be a 'true', 'false', or a valid color string)";
                        throw new HalconException(hv_Exception);
                    }
                }
            }
            if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(0))).TupleNotEqual("false"))) != 0)
            {
                //Valid color?
                try
                {
                    HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(
                        0));
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Exception = "Wrong value of control parameter Box[0] (must be a 'true', 'false', or a valid color string)";
                    throw new HalconException(hv_Exception);
                }
                //Calculate box extents
                hv_String_COPY_INP_TMP = (" " + hv_String_COPY_INP_TMP) + " ";
                hv_Width = new HTuple();
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    HOperatorSet.GetStringExtents(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                        hv_Index), out hv_Ascent, out hv_Descent, out hv_W, out hv_H);
                    hv_Width = hv_Width.TupleConcat(hv_W);
                }
                hv_FrameHeight = hv_MaxHeight * (new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    ));
                hv_FrameWidth = (((new HTuple(0)).TupleConcat(hv_Width))).TupleMax();
                hv_R2 = hv_R1 + hv_FrameHeight;
                hv_C2 = hv_C1 + hv_FrameWidth;
                //Display rectangles
                HOperatorSet.GetDraw(hv_WindowHandle, out hv_DrawMode);
                HOperatorSet.SetDraw(hv_WindowHandle, "fill");
                //Set shadow color
                HOperatorSet.SetColor(hv_WindowHandle, hv_ShadowColor);
                if ((int)(hv_UseShadow) != 0)
                {
                    HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1 + 1, hv_C1 + 1, hv_R2 + 1,
                        hv_C2 + 1);
                }
                //Set box color
                HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(
                    0));
                HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1, hv_C1, hv_R2, hv_C2);
                HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode);
            }
            //Write text.
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                )) - 1); hv_Index = (int)hv_Index + 1)
            {
                hv_CurrentColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_Index % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
                    )));
                if ((int)((new HTuple(hv_CurrentColor.TupleNotEqual(""))).TupleAnd(new HTuple(hv_CurrentColor.TupleNotEqual(
                    "auto")))) != 0)
                {
                    HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor);
                }
                else
                {
                    HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                }
                hv_Row_COPY_INP_TMP = hv_R1 + (hv_MaxHeight * hv_Index);
                HOperatorSet.SetTposition(hv_WindowHandle, hv_Row_COPY_INP_TMP, hv_C1);
                HOperatorSet.WriteString(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                    hv_Index));
            }
            //Reset changed window settings
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
            HOperatorSet.SetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part,
                hv_Column2Part);

            return;
        }

        public void ShowCCDStatic(Enum_sys.ProdState e)
        {
            Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetFont("-Consolas-40-*-0-*-*-1-");

            switch (e)
            {

                case Enum_sys.ProdState.OK:
                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("green");
                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetTposition(12, 12);
                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.WriteString("OK");
                    //set_display_font(MainWindowHandle, 40, "mono", "true", "false");
                    //disp_message(MainWindowHandle, "OK", "window", 12, 12, "green", "false");
                    break;
                case Enum_sys.ProdState.NG:
                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("red");
                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetTposition(12, 12);

                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.WriteString("NG");

                    //set_display_font(MainWindowHandle, 40, "mono", "true", "false");
                    //disp_message(MainWindowHandle, "NG", "window", 12, 12, "red", "false");
                    break;
                case Enum_sys.ProdState.异常:
                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetColor("red");
                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.SetTposition(12, 12);

                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.HWindowID.WriteString("清理或放上电芯");

                    //set_display_font(MainWindowHandle, 40, "mono", "true", "false");
                    //disp_message(MainWindowHandle, "清理或放上电芯", "window", 12, 12, "red", "false");
                    break;
                default:
                    break;
            }

        }

    }

    public class SendData
    {
        public double MedM;
        public double MedLM;

        public double X;
        public double Y;
        public double U;
        public double Width;

        public bool located;
        public bool measured;
    }
}