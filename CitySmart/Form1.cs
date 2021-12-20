using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using DBUtility;
using System.Data.OleDb;
using OpenTK;
using System.Drawing.Imaging;

namespace CitySmart
{
    public partial class Form1 : Form
    {
        //selection绘图选择,精度
        public string accuracy_x;
        public string accuracy_y;
        public string accuracy_t;

        public string w_length;
        public string w_width;
        public string e_length;
        public string e_width;
        public string s_length;
        public string s_width;
        public string n_length;
        public string n_width;



        //basicinfo分析范围
        public string start_t;
        public string end_t;
        public string start_x;
        public string end_x;
        public string start_y;
        public string end_y;

        //basicstats简单分析结果
        public string stats_car;
        public string stats_v;
        public string stats_delay;
        public string stats_queue;


        bool glControlLoaded = false;

        private EasyAccess _easyAccess = new EasyAccess();

        private double MouseScale = 100.0f;

        private bool BackGround_Init = false;
        private Bitmap BackGroundImg = null;
        private BitmapData bmp_data = null;
        private int textureID;

        private double xRotAng = 0;                     //   角度 
        private double yRotAng = 0;                     //   角度 
        private double xRotAngInit = 45;             //   角度 
        private double yRotAngInit = 45;             //   角度 

        private float xTransInit = 0; //3D 平移初始值
        private float yTransInit = 0;
        private float xTrans = 0; //3D 平移值
        private float yTrans = 0;

        private float c = (float)Math.PI / 180.0f; //弧度和角度转换参数
        private int du = 90, oldmy = -1, oldmx = -1; //du是视点绕y轴的角度,opengl里默认y轴是上方向
        private float r = 1.0f, h = 0.0f; //r是视点绕y轴的半径,h是视点高度即在y轴上的坐标



        private float camera_alpha = -90;
        private float camera_fy = 1;

        Point[] cudeCoordinate = new Point[8];//半透明立方体的坐标
        private bool IsSemitransparencyCube = false; // 判断是否显示半透明立方体

        private bool isClipPlaneDisplay = false;//判断是否显示切面
        private  Point[] cutPoint = new Point[3];
        private double[] ClipPlanePara = new double[4];
        private double[][] CudePlanePara = new double[8][];//半透明立方体平面参数

        private double[] BackGround_Coordinate = { -100.0, -100, 100, 100 };

        private List<string> _routDec; //已废弃，由_vehicleRoutesMap代替
        private List<string> _vehcTypes;
        private List<string> _links;
        private List<string> _lanes; //已废弃，由_vehicleRoutesMap代替
        private List<VehicleRoute> _vehicleRoutes;
        private Dictionary<string, VehicleRoute> _vehicleRoutesMap;
        private bool _strongFlag = false; //是否强化开关

        private double zZoom = 0.01;                 //  3D 放大系数
      //  private double zZoom_2D = 0.01;             //   2D 放大系数

        public double _thickness = 0.001; //保存二维切面时的厚度


        private CitySmart _citySmart = new CitySmart();
        public Form1()
        {
            InitializeComponent();
            InitProgram();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog myDialog = new FolderBrowserDialog();

            myDialog.ShowNewFolderButton = false;

            myDialog.Description = "选择你要设置的文件夹或目录 ";

            if (myDialog.ShowDialog() == DialogResult.OK)
                MessageBox.Show(myDialog.SelectedPath.ToString());

            myDialog.Dispose();
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.tabControl1.Size = this.Size;

        }

        private void textBox1_TabStopChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(this.textBox1.Text);
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {

        }

        private void textBox1_CursorChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                _citySmart._centralPoint = new Point(Convert.ToDouble(this.textBox1.Text), Convert.ToDouble(this.textBox2.Text), 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            _citySmart._routDecs.Clear();
            _citySmart._vehcTypes.Clear();
            _citySmart._links.Clear();
            _citySmart._lanes.Clear();
            _citySmart._vehicleRoutes.Clear();

            foreach (string v in _routDec)
            {
                if (IsDouble(v))
                {
                    _citySmart._routDecs.Add(Convert.ToDouble(v));
                }

                if (IsDouble(v))
                {
                    if (_vehicleRoutesMap.ContainsKey(v))
                    {
                        _citySmart._vehicleRoutes.Add(_vehicleRoutesMap[v]);
                    }
                }
            }

            foreach (string v in _vehcTypes)
            {
                if (IsDouble(v))
                {
                    _citySmart._vehcTypes.Add(Convert.ToDouble(v));
                }
            }

            foreach (string v in _links)
            {
                if (IsDouble(v))
                {
                    _citySmart._links.Add(Convert.ToDouble(v));
                }
            }

            foreach (string v in _lanes)
            {
                if (IsDouble(v))
                {
                    _citySmart._lanes.Add(Convert.ToDouble(v));
                }



            }

            try
            {
                _citySmart._startTime = Convert.ToDouble(this.startTime.Text);
                _citySmart._endTime = Convert.ToDouble(this.endTime.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("请输入正确的时间");
            }
            _citySmart.ReflashVicNrs();
            _citySmart.ReflashInfo();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            
        }
        private void ComputeClipPlaneParmemter(Point cutPoint_0, Point cutPoint_1, Point cutPoint_2, double[] PlanePara)
        {
            Point vctor1 = new Point();
            vctor1.x = cutPoint_0.x - cutPoint_1.x;
            vctor1.y = cutPoint_0.y - cutPoint_1.y;
            vctor1.z = cutPoint_0.z - cutPoint_1.z;

            Point vctor2 = new Point();
            vctor2.x = cutPoint_2.x - cutPoint_1.x;
            vctor2.y = cutPoint_2.y - cutPoint_1.y;
            vctor2.z = cutPoint_2.z - cutPoint_1.z;

            if (vctor1.x * vctor2.y == vctor2.x * vctor1.y && vctor1.x * vctor2.z == vctor2.x * vctor1.z && vctor1.y * vctor2.z == vctor2.y * vctor1.z)
            {
                MessageBox.Show("您输入的点在同一条直线，请重新输入。");
                return;
            }

            PlanePara[0] = vctor1.y * vctor2.z - vctor1.z * vctor2.y;
            PlanePara[1] = -(vctor1.x * vctor2.z - vctor1.z * vctor2.x);
            PlanePara[2] = vctor1.x * vctor2.y - vctor1.y * vctor2.x;
            PlanePara[3] = -(PlanePara[0] * cutPoint_0.x + PlanePara[1] * cutPoint_0.y + PlanePara[2] * cutPoint_0.z);

            double Norm = System.Math.Sqrt(PlanePara[0] * PlanePara[0] + PlanePara[1] * PlanePara[1] + PlanePara[2] * PlanePara[2] + PlanePara[3] * PlanePara[3]);
            PlanePara[0] /= Norm;
            PlanePara[1] /= Norm;
            PlanePara[2] /= Norm;
            PlanePara[3] /= Norm;
            //PlanePara[3] += 0.001;
            PlanePara[3] += _thickness;
        }

        private bool IsDouble(string v_)
        {
            try
            {
                double a = Convert.ToDouble(v_);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private void SetupViewport(int w, int h)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, 0, h, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
        }

        private void reshape(int w, int h)
        {
            GL.Viewport(0, 0, w, h);
            GL.MatrixMode(MatrixMode.Projection);

            GL.LoadIdentity();
            if (w <= h)
            {
                GL.Ortho(-1.5, 1.5, -1.5 * (double)h / (double)w, 1.5 * (double)h / (double)w, -100.0, 100.0);
            }
            else
            {
                GL.Ortho(-1.5 * (double)w / (double)h, 1.5 * (double)w / (double)h, -1.5, 1.5, -100.0, 100.0);
            }
            //GL.Ortho(0, w, 0, h, -10, 10);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

        }



        private void glControl_MouseWheel(object sender, MouseEventArgs e)
        {
            double a = e.Delta / MouseScale;
            if (e.Delta > 0)
            {
                zZoom *= System.Math.Abs(a);

            }
            if (e.Delta < 0)
            {
                zZoom /= System.Math.Abs(a);

            }
            glControl.Invalidate();
        }
        private void Draw_glControlRightDown()
        {
            //int total = 0;
            // List<double> VicNrs = _citySmart.GetVicNrs();
            //需要获得一共有多少条记录
            //  int a =VicNrs.Count();


        }

        private void Draw6FaceObj_NoBT(Surface s1_, Surface s2_)
        {
            Surface s1 = s1_;
            Surface s2 = s2_;

            GL.Begin(BeginMode.Quads);
            {
                GL.Vertex3(s1.p1.x, s1.p1.y, s1.p1.z);
                GL.Vertex3(s1.p2.x, s1.p2.y, s1.p2.z);
                GL.Vertex3(s2.p2.x, s2.p2.y, s2.p2.z);
                GL.Vertex3(s2.p1.x, s2.p1.y, s2.p1.z);
            }
            GL.End();

            GL.Begin(BeginMode.Quads);
            {
                GL.Vertex3(s1.p2.x, s1.p2.y, s1.p2.z);
                GL.Vertex3(s1.p3.x, s1.p3.y, s1.p3.z);
                GL.Vertex3(s2.p3.x, s2.p3.y, s2.p3.z);
                GL.Vertex3(s2.p2.x, s2.p2.y, s2.p2.z);
            }
            GL.End();

            GL.Begin(BeginMode.Quads);
            {
                GL.Vertex3(s1.p3.x, s1.p3.y, s1.p3.z);
                GL.Vertex3(s1.p4.x, s1.p4.y, s1.p4.z);
                GL.Vertex3(s2.p4.x, s2.p4.y, s2.p4.z);
                GL.Vertex3(s2.p3.x, s2.p3.y, s2.p3.z);
            }
            GL.End();

            GL.Begin(BeginMode.Quads);
            {
                GL.Vertex3(s1.p4.x, s1.p4.y, s1.p4.z);
                GL.Vertex3(s1.p1.x, s1.p1.y, s1.p1.z);
                GL.Vertex3(s2.p1.x, s2.p1.y, s2.p1.z);
                GL.Vertex3(s2.p4.x, s2.p4.y, s2.p4.z);
            }
            GL.End();

        }

        private void Draw6FaceObj(Surface s1_, Surface s2_)
        {
            Surface s1 = s1_;
            Surface s2 = s2_;

            GL.Begin(BeginMode.Quads);
            {
                GL.Vertex3(s1.p1.x, s1.p1.y, s1.p1.z);
                GL.Vertex3(s1.p2.x, s1.p2.y, s1.p2.z);
                GL.Vertex3(s1.p3.x, s1.p3.y, s1.p3.z);
                GL.Vertex3(s1.p4.x, s1.p4.y, s1.p4.z);
            }
            GL.End();
            GL.Begin(BeginMode.Quads);
            {
                GL.Vertex3(s2.p1.x, s2.p1.y, s2.p1.z);
                GL.Vertex3(s2.p2.x, s2.p2.y, s2.p2.z);
                GL.Vertex3(s2.p3.x, s2.p3.y, s2.p3.z);
                GL.Vertex3(s2.p4.x, s2.p4.y, s2.p4.z);
            }
            GL.End();

            GL.Begin(BeginMode.Quads);
            {
                GL.Vertex3(s1.p1.x, s1.p1.y, s1.p1.z);
                GL.Vertex3(s1.p2.x, s1.p2.y, s1.p2.z);
                GL.Vertex3(s2.p2.x, s2.p2.y, s2.p2.z);
                GL.Vertex3(s2.p1.x, s2.p1.y, s2.p1.z);
            }
            GL.End();

            GL.Begin(BeginMode.Quads);
            {
                GL.Vertex3(s1.p2.x, s1.p2.y, s1.p2.z);
                GL.Vertex3(s1.p3.x, s1.p3.y, s1.p3.z);
                GL.Vertex3(s2.p3.x, s2.p3.y, s2.p3.z);
                GL.Vertex3(s2.p2.x, s2.p2.y, s2.p2.z);
            }
            GL.End();

            GL.Begin(BeginMode.Quads);
            {
                GL.Vertex3(s1.p3.x, s1.p3.y, s1.p3.z);
                GL.Vertex3(s1.p4.x, s1.p4.y, s1.p4.z);
                GL.Vertex3(s2.p4.x, s2.p4.y, s2.p4.z);
                GL.Vertex3(s2.p3.x, s2.p3.y, s2.p3.z);
            }
            GL.End();

            GL.Begin(BeginMode.Quads);
            {
                GL.Vertex3(s1.p4.x, s1.p4.y, s1.p4.z);
                GL.Vertex3(s1.p1.x, s1.p1.y, s1.p1.z);
                GL.Vertex3(s2.p1.x, s2.p1.y, s2.p1.z);
                GL.Vertex3(s2.p4.x, s2.p4.y, s2.p4.z);
            }
            GL.End();
        }



        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Draw_glControlRightDown();
        }

        /*=========================private functions*/
        #region private function

        private void DrawSemitransparencyCube()
        {
            if (!IsSemitransparencyCube)
                return;
            //double [,] cudeCoordinate=new double[8,3];  
            // GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Blend);
            //glBlendFunc(GL_SRC_ALPHA, GL_DST_COLOR);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.DstAlpha);
            GL.Color4(0.5, 0.5, 0.5, 0.1);
            GL.Begin(BeginMode.Quads);
            {

                // front face
                GL.Normal3(0, 0, 1);
                GL.Vertex3(cudeCoordinate[0].x, cudeCoordinate[0].y, cudeCoordinate[0].z); //立方体的右上
                GL.Vertex3(cudeCoordinate[1].x, cudeCoordinate[1].y, cudeCoordinate[1].z); //立方体的左上
                GL.Vertex3(cudeCoordinate[2].x, cudeCoordinate[2].y, cudeCoordinate[2].z);//立方体的左下
                GL.Vertex3(cudeCoordinate[3].x, cudeCoordinate[3].y, cudeCoordinate[3].z);//立方体的右下

                // back face
                GL.Normal3(0, 0, -1);
                GL.Vertex3(cudeCoordinate[4].x, cudeCoordinate[4].y, cudeCoordinate[4].z);// 立方体的右上
                GL.Vertex3(cudeCoordinate[5].x, cudeCoordinate[5].y, cudeCoordinate[5].z);// 立方体的左上
                GL.Vertex3(cudeCoordinate[6].x, cudeCoordinate[6].y, cudeCoordinate[6].z); // 立方体的左下
                GL.Vertex3(cudeCoordinate[7].x, cudeCoordinate[7].y, cudeCoordinate[7].z);// 立方体的右下

                // top face
                GL.Normal3(0, 1, 0);
                GL.Vertex3(cudeCoordinate[4].x, cudeCoordinate[4].y, cudeCoordinate[4].z);// 立方体的右上
                GL.Vertex3(cudeCoordinate[5].x, cudeCoordinate[5].y, cudeCoordinate[5].z);// 立方体的左上
                GL.Vertex3(cudeCoordinate[1].x, cudeCoordinate[1].y, cudeCoordinate[1].z); // 立方体的左下
                GL.Vertex3(cudeCoordinate[0].x, cudeCoordinate[0].y, cudeCoordinate[0].z);// 立方体的右下 

                // bottom face
                GL.Normal3(0, -1, 0);
                GL.Vertex3(cudeCoordinate[7].x, cudeCoordinate[7].y, cudeCoordinate[7].z);// 立方体的右上
                GL.Vertex3(cudeCoordinate[6].x, cudeCoordinate[6].y, cudeCoordinate[6].z);// 立方体的左上
                GL.Vertex3(cudeCoordinate[2].x, cudeCoordinate[2].y, cudeCoordinate[2].z); // 立方体的左下
                GL.Vertex3(cudeCoordinate[3].x, cudeCoordinate[3].y, cudeCoordinate[3].z);// 立方体的右下 

                // right face
                GL.Normal3(1, 0, 0);
                GL.Vertex3(cudeCoordinate[4].x, cudeCoordinate[4].y, cudeCoordinate[4].z);// 立方体的右上
                GL.Vertex3(cudeCoordinate[0].x, cudeCoordinate[0].y, cudeCoordinate[0].z);// 立方体的左上
                GL.Vertex3(cudeCoordinate[3].x, cudeCoordinate[3].y, cudeCoordinate[3].z); // 立方体的左下
                GL.Vertex3(cudeCoordinate[7].x, cudeCoordinate[7].y, cudeCoordinate[7].z);// 立方体的右下 

                // left face
                GL.Normal3(-1, 0, 0);
                GL.Vertex3(cudeCoordinate[1].x, cudeCoordinate[1].y, cudeCoordinate[1].z);// 立方体的右上
                GL.Vertex3(cudeCoordinate[2].x, cudeCoordinate[2].y, cudeCoordinate[2].z);// 立方体的左上
                GL.Vertex3(cudeCoordinate[6].x, cudeCoordinate[6].y, cudeCoordinate[6].z); // 立方体的左下
                GL.Vertex3(cudeCoordinate[5].x, cudeCoordinate[5].y, cudeCoordinate[5].z);// 立方体的右下 
            }
            GL.End();

            GL.Begin(BeginMode.LineLoop);
            {
                // front face
                //GL.Normal3(0, 0, 1);
                GL.Vertex3(cudeCoordinate[0].x, cudeCoordinate[0].y, cudeCoordinate[0].z); //立方体的右上
                GL.Vertex3(cudeCoordinate[1].x, cudeCoordinate[1].y, cudeCoordinate[1].z); //立方体的左上
                GL.Vertex3(cudeCoordinate[2].x, cudeCoordinate[2].y, cudeCoordinate[2].z);//立方体的左下
                GL.Vertex3(cudeCoordinate[3].x, cudeCoordinate[3].y, cudeCoordinate[3].z);//立方体的右下

            }
            GL.End();

            GL.Begin(BeginMode.LineLoop);
            {
                // back face
                //  GL.Normal3(0, 0, -1);
                GL.Vertex3(cudeCoordinate[4].x, cudeCoordinate[4].y, cudeCoordinate[4].z);// 立方体的右上
                GL.Vertex3(cudeCoordinate[5].x, cudeCoordinate[5].y, cudeCoordinate[5].z);// 立方体的左上
                GL.Vertex3(cudeCoordinate[6].x, cudeCoordinate[6].y, cudeCoordinate[6].z); // 立方体的左下
                GL.Vertex3(cudeCoordinate[7].x, cudeCoordinate[7].y, cudeCoordinate[7].z);// 立方体的右下
            }
            GL.End();

            GL.Begin(BeginMode.LineLoop);
            {
                // top face
                //GL.Normal3(0, 1, 0);
                GL.Vertex3(cudeCoordinate[4].x, cudeCoordinate[4].y, cudeCoordinate[4].z);// 立方体的右上
                GL.Vertex3(cudeCoordinate[5].x, cudeCoordinate[5].y, cudeCoordinate[5].z);// 立方体的左上
                GL.Vertex3(cudeCoordinate[1].x, cudeCoordinate[1].y, cudeCoordinate[1].z); // 立方体的左下
                GL.Vertex3(cudeCoordinate[0].x, cudeCoordinate[0].y, cudeCoordinate[0].z);// 立方体的右下 
            }
            GL.End();

            GL.Begin(BeginMode.LineLoop);
            {
                // bottom face
                //GL.Normal3(0, -1, 0);
                GL.Vertex3(cudeCoordinate[7].x, cudeCoordinate[7].y, cudeCoordinate[7].z);// 立方体的右上
                GL.Vertex3(cudeCoordinate[6].x, cudeCoordinate[6].y, cudeCoordinate[6].z);// 立方体的左上
                GL.Vertex3(cudeCoordinate[2].x, cudeCoordinate[2].y, cudeCoordinate[2].z); // 立方体的左下
                GL.Vertex3(cudeCoordinate[3].x, cudeCoordinate[3].y, cudeCoordinate[3].z);// 立方体的右下 
            }
            GL.End();

            GL.Begin(BeginMode.LineLoop);
            {
                // right face
                //GL.Normal3(1, 0, 0);
                GL.Vertex3(cudeCoordinate[4].x, cudeCoordinate[4].y, cudeCoordinate[4].z);// 立方体的右上
                GL.Vertex3(cudeCoordinate[0].x, cudeCoordinate[0].y, cudeCoordinate[0].z);// 立方体的左上
                GL.Vertex3(cudeCoordinate[3].x, cudeCoordinate[3].y, cudeCoordinate[3].z); // 立方体的左下
                GL.Vertex3(cudeCoordinate[7].x, cudeCoordinate[7].y, cudeCoordinate[7].z);// 立方体的右下 
            }
            GL.End();

            GL.Begin(BeginMode.LineLoop);
            {

                // left face
                //GL.Normal3(-1, 0, 0);
                GL.Vertex3(cudeCoordinate[1].x, cudeCoordinate[1].y, cudeCoordinate[1].z);// 立方体的右上
                GL.Vertex3(cudeCoordinate[2].x, cudeCoordinate[2].y, cudeCoordinate[2].z);// 立方体的左上
                GL.Vertex3(cudeCoordinate[6].x, cudeCoordinate[6].y, cudeCoordinate[6].z); // 立方体的左下
                GL.Vertex3(cudeCoordinate[5].x, cudeCoordinate[5].y, cudeCoordinate[5].z);// 立方体的右下 
            }
            GL.End();

            GL.Disable(EnableCap.Blend);
            GL.Color3(1.0f, 1.0f, 1.0f);
        }

        private void DrawOuter()
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.DstAlpha);
            double alpha = 0.1;
            VehcShape shape = new VehcShape();
            Constant constant = new Constant();
            //  GL.PushMatrix();
            {
                //绘制车流
                if (_citySmart._surfaceSet.Count == 0)
                {
                    return;
                }
                int color = 0;
                int i = 0;
                int vehic = 0;
                int colorSum = _citySmart._colorSet.ElementAt(color);
                int vehicSum = _citySmart._unitSet.ElementAt(vehic);

                GL.Color4(constant.COLOR_SET[color].red, constant.COLOR_SET[color].green, constant.COLOR_SET[color].blue, alpha); //初始化颜色
                Surface preSurface = _citySmart._surfaceSet.ElementAt(0);
                shape = _citySmart._vecShapes.ElementAt(0);

                double ratio = 0.01;

                Point tP1 = new Point();
                Point tP2 = new Point();
                Point tP3 = new Point();
                Point tP4 = new Point();

                tP1.x = preSurface.p1.x;
                tP1.y = preSurface.p1.y;
                tP1.z = preSurface.p1.z;

                tP2.x = preSurface.p2.x;
                tP2.y = preSurface.p2.y;
                tP2.z = preSurface.p2.z;

                tP3.x = preSurface.p3.x;
                tP3.y = preSurface.p3.y;
                tP3.z = preSurface.p3.z;

                tP4.x = preSurface.p4.x;
                tP4.y = preSurface.p4.y;
                tP4.z = preSurface.p4.z;

                Point cP = new Point((tP1.x + tP2.x) / 2, (tP1.y + tP2.y) / 2, tP1.z);
                tP1.x = cP.x - (tP1.x - cP.x) * ratio;
                tP1.y = cP.y - (tP1.y - cP.y) * ratio;

                tP2.x = cP.x - (tP2.x - cP.x) * ratio;
                tP2.y = cP.y - (tP2.y - cP.y) * ratio;

                tP3.x = cP.x - (tP3.x - cP.x) * ratio;
                tP3.y = cP.y - (tP3.y - cP.y) * ratio;

                tP4.x = cP.x - (tP4.x - cP.x) * ratio;
                tP4.y = cP.y - (tP4.y - cP.y) * ratio;

                preSurface.p1 = tP1;
                preSurface.p2 = tP2;
                preSurface.p3 = tP3;
                preSurface.p4 = tP4;


                Surface tmpPreSurface = new Surface();
                ratio = 2; //用于设置车辆外部着色的放大比例
                //计算车辆外部着色初始位置
                /*
                Point center = new Point();
                center.x = (preSurface.p1.x + preSurface.p3.x)/2;
                center.y = (preSurface.p1.y + preSurface.p3.y)/2;
                center.z = preSurface.p1.z;

                tmpPreSurface.p1.z = center.z;
                tmpPreSurface.p2.z = center.z;
                tmpPreSurface.p3.z = center.z;
                tmpPreSurface.p4.z = center.z;

                tmpPreSurface.p1.x = center.x + ratio * (preSurface.p1.x - center.x);
                tmpPreSurface.p1.y = center.y + ratio * (preSurface.p1.y - center.y);

                tmpPreSurface.p2.x = center.x + ratio * (preSurface.p2.x - center.x);
                tmpPreSurface.p2.y = center.y + ratio * (preSurface.p2.y - center.y);

                tmpPreSurface.p3.x = center.x + ratio * (preSurface.p3.x - center.x);
                tmpPreSurface.p3.y = center.y + ratio * (preSurface.p3.y - center.y);

                tmpPreSurface.p4.x = center.x + ratio * (preSurface.p4.x - center.x);
                tmpPreSurface.p4.y = center.y + ratio * (preSurface.p4.y - center.y);
                */

                Point lc = new Point();
                lc.x = (preSurface.p3.x + preSurface.p4.x) / 2;
                lc.y = (preSurface.p3.y + preSurface.p4.y) / 2;

                tmpPreSurface.p4.x = lc.x - ((shape.leftSide + shape.width / 2) * (lc.x - preSurface.p4.x) / (shape.width / 2));
                tmpPreSurface.p4.y = lc.y + ((shape.leftSide + shape.width / 2) * (preSurface.p4.y - lc.y) / (shape.width / 2));

                tmpPreSurface.p3.x = tmpPreSurface.p4.x + ((shape.leftSide + shape.rightSide + shape.width) * (lc.x - tmpPreSurface.p4.x) / (shape.leftSide + shape.width / 2));
                tmpPreSurface.p3.y = tmpPreSurface.p4.y - ((shape.leftSide + shape.rightSide + shape.width) * (tmpPreSurface.p4.y - lc.y) / (shape.leftSide + shape.width / 2));

                tmpPreSurface.p1.x = preSurface.p1.x - (preSurface.p4.x - tmpPreSurface.p4.x);
                tmpPreSurface.p1.y = preSurface.p1.y + (tmpPreSurface.p4.y - preSurface.p4.y);

                tmpPreSurface.p2.x = preSurface.p2.x - (preSurface.p3.x - tmpPreSurface.p3.x);
                tmpPreSurface.p2.y = preSurface.p2.y + (tmpPreSurface.p3.y - preSurface.p3.y);

                tmpPreSurface.p1.x = tmpPreSurface.p4.x + ((tmpPreSurface.p1.x - tmpPreSurface.p4.x) * (shape.length + shape.front) / shape.length);
                tmpPreSurface.p1.y = tmpPreSurface.p4.y + ((tmpPreSurface.p1.y - tmpPreSurface.p4.y) * (shape.length + shape.front) / shape.length);


                tmpPreSurface.p2.x = tmpPreSurface.p3.x + ((tmpPreSurface.p2.x - tmpPreSurface.p3.x) * (shape.length + shape.front) / shape.length);
                tmpPreSurface.p2.y = tmpPreSurface.p3.y + ((tmpPreSurface.p2.y - tmpPreSurface.p3.y) * (shape.length + shape.front) / shape.length);

                foreach (Surface surface in _citySmart._surfaceSet)
                {
                    while (i == colorSum && color < _citySmart._colorSet.Count)
                    {
                        color++;
                        GL.Color4(constant.COLOR_SET[ConfigInfo.colorSet[color]].red, constant.COLOR_SET[ConfigInfo.colorSet[color]].green, constant.COLOR_SET[ConfigInfo.colorSet[color]].blue, alpha); //修改颜色                        
                        colorSum += _citySmart._colorSet.ElementAt(color);
                    }


                    //计算车辆外部着色位置
                    /*
                    center = new Point();
                    center.x = (surface.p1.x + surface.p3.x) / 2;
                    center.y = (surface.p1.y + surface.p3.y) / 2;
                    center.z = surface.p1.z;
                     
                    tmpSurface.p1.z = surface.p1.z;
                    tmpSurface.p2.z = surface.p2.z;
                    tmpSurface.p3.z = surface.p3.z;
                    tmpSurface.p4.z = surface.p4.z;

                    tmpSurface.p1.x = center.x + ratio * (surface.p1.x - center.x);
                    tmpSurface.p1.y = center.y + ratio * (surface.p1.y - center.y);

                    tmpSurface.p2.x = center.x + ratio * (surface.p2.x - center.x);
                    tmpSurface.p2.y = center.y + ratio * (surface.p2.y - center.y);

                    tmpSurface.p3.x = center.x + ratio * (surface.p3.x - center.x);
                    tmpSurface.p3.y = center.y + ratio * (surface.p3.y - center.y);

                    tmpSurface.p4.x = center.x + ratio * (surface.p4.x - center.x);
                    tmpSurface.p4.y = center.y + ratio * (surface.p4.y - center.y);
                    */
                    Surface tmpSurface = new Surface();
                    tmpSurface.p1.z = surface.p1.z;
                    tmpSurface.p2.z = surface.p2.z;
                    tmpSurface.p3.z = surface.p3.z;
                    tmpSurface.p4.z = surface.p4.z;
                    lc = new Point();
                    lc.x = (surface.p3.x + surface.p4.x) / 2;
                    lc.y = (surface.p3.y + surface.p4.y) / 2;

                    tmpSurface.p4.x = lc.x - ((shape.leftSide + shape.width / 2) * (lc.x - surface.p4.x) / (shape.width / 2));
                    tmpSurface.p4.y = lc.y + ((shape.leftSide + shape.width / 2) * (surface.p4.y - lc.y) / (shape.width / 2));

                    tmpSurface.p3.x = tmpSurface.p4.x + ((shape.leftSide + shape.rightSide + shape.width) * (lc.x - tmpSurface.p4.x) / (shape.leftSide + shape.width / 2));
                    tmpSurface.p3.y = tmpSurface.p4.y - ((shape.leftSide + shape.rightSide + shape.width) * (tmpSurface.p4.y - lc.y) / (shape.leftSide + shape.width / 2));

                    tmpSurface.p1.x = surface.p1.x - (surface.p4.x - tmpSurface.p4.x);
                    tmpSurface.p1.y = surface.p1.y + (tmpSurface.p4.y - surface.p4.y);

                    tmpSurface.p2.x = surface.p2.x - (surface.p3.x - tmpSurface.p3.x);
                    tmpSurface.p2.y = surface.p2.y + (tmpSurface.p3.y - surface.p3.y);

                    tmpSurface.p1.x = tmpSurface.p4.x + ((tmpSurface.p1.x - tmpSurface.p4.x) * (shape.length + shape.front) / shape.length);
                    tmpSurface.p1.y = tmpSurface.p4.y + ((tmpSurface.p1.y - tmpSurface.p4.y) * (shape.length + shape.front) / shape.length);


                    tmpSurface.p2.x = tmpSurface.p3.x + ((tmpSurface.p2.x - tmpSurface.p3.x) * (shape.length + shape.front) / shape.length);
                    tmpSurface.p2.y = tmpSurface.p3.y + ((tmpSurface.p2.y - tmpSurface.p3.y) * (shape.length + shape.front) / shape.length);




                    while (i == vehicSum && vehic < _citySmart._unitSet.Count - 1)
                    {
                        vehic++;
                        tmpPreSurface = tmpSurface;
                        shape = _citySmart._vecShapes.ElementAt(vehic);
                        vehicSum += _citySmart._unitSet.ElementAt(vehic);
                    }



                    //GL.Color3(0, 0.5, 1);
                    //绘制六面体
                    Draw6FaceObj_NoBT(tmpPreSurface, tmpSurface);


                    tmpPreSurface = tmpSurface;
                    if (i == 52)
                    {
                        //Console.Write("");
                    }
                    i++;
                }

                GL.Disable(EnableCap.Blend);
                GL.Color3(1.0f, 1.0f, 1.0f);
            }
        }
        private void DrawCoordination(double Coor_Length, double Coor_BigInterval_Len, double Coor_Interval_len, double Coor_BigInterval_Height, double Coor_Interval_Height, double[] Coor_Origin, int Directionflag)
        {
            int Coor_BigInterval = (int)(Coor_Length / Coor_BigInterval_Len);
            int Coor_Interval = (int)(Coor_Length / Coor_Interval_len);

            //x轴
            if (Directionflag == 2)
            {
                for (int k = 0; k < Coor_BigInterval; k++)
                {
                    GL.Begin(BeginMode.Lines);
                    GL.Vertex3(Coor_Origin[0] + k * 25, Coor_Origin[1], 0);
                    GL.Vertex3(Coor_Origin[0] + k * 25, Coor_Origin[1], 0 + Coor_BigInterval_Height);
                    GL.End();
                    for (int k1 = 1; k1 < 5; k1++)
                    {
                        GL.Begin(BeginMode.Lines);
                        GL.Vertex3(Coor_Origin[0] + k * 25 + k1 * 5, Coor_Origin[1], 0);
                        GL.Vertex3(Coor_Origin[0] + k * 25 + k1 * 5, Coor_Origin[1], 0+ Coor_Interval_Height);
                        GL.End();
                    }
                }
            }
            else
            {
                for (int k = 0; k < Coor_BigInterval; k++)
                {
                    GL.Begin(BeginMode.Lines);
                    GL.Vertex3(Coor_Origin[0] + k * 25,0, Coor_Origin[2]);
                    GL.Vertex3(Coor_Origin[0] + k * 25, 0 + Coor_BigInterval_Height, Coor_Origin[2]);
                    GL.End();
                    for (int k1 = 1; k1 < 5; k1++)
                    {
                        GL.Begin(BeginMode.Lines);
                        GL.Vertex3(Coor_Origin[0] + k * 25 + k1 * 5, 0, Coor_Origin[2]);
                        GL.Vertex3(Coor_Origin[0] + k * 25 + k1 * 5, 0 + Coor_Interval_Height, Coor_Origin[2]);
                        GL.End();
                    }
                }
            }

            //y轴
            if (Directionflag == 1 )
            {
                for (int k = 0; k < Coor_BigInterval; k++)
                {
                    GL.Begin(BeginMode.Lines);
                    GL.Vertex3(Coor_Origin[0], Coor_Origin[1] + k * 25, 0);
                    GL.Vertex3(Coor_Origin[0], Coor_Origin[1] + k * 25, 0 + Coor_BigInterval_Height);
                    GL.End();

                    for (int k1 = 1; k1 < 5; k1++)
                    {
                        GL.Begin(BeginMode.Lines);
                        GL.Vertex3(Coor_Origin[0], Coor_Origin[1] + k * 25 + k1 * 5, 0);
                        GL.Vertex3(Coor_Origin[0], Coor_Origin[1] + k * 25 + k1 * 5, 0 + Coor_Interval_Height);
                        GL.End();
                    }
                }
            }
            else
            {
                for (int k = 0; k < Coor_BigInterval; k++)
                {
                    GL.Begin(BeginMode.Lines);
                    GL.Vertex3(0, Coor_Origin[1] + k * 25, Coor_Origin[2]);
                    GL.Vertex3(0 + Coor_BigInterval_Height, Coor_Origin[1] + k * 25, Coor_Origin[2]);
                    GL.End();

                    for (int k1 = 1; k1 < 5; k1++)
                    {
                        GL.Begin(BeginMode.Lines);
                        GL.Vertex3(0, Coor_Origin[1] + k * 25 + k1 * 5, Coor_Origin[2]);
                        GL.Vertex3(0 + Coor_Interval_Height, Coor_Origin[1] + k * 25 + k1 * 5, Coor_Origin[2]);
                        GL.End();
                    }
                }
            }

            //z轴
            if (Directionflag == 2)
            {
                for (int k = 0; k < Coor_BigInterval; k++)
                {
                    GL.Begin(BeginMode.Lines);
                    GL.Vertex3(0, Coor_Origin[1], Coor_Origin[2] + k * 25);
                    GL.Vertex3(0+ Coor_BigInterval_Height, Coor_Origin[1] , Coor_Origin[2] + k * 25);
                    GL.End();
                    for (int k1 = 1; k1 < 5; k1++)
                    {
                        GL.Begin(BeginMode.Lines);
                        GL.Vertex3(0, Coor_Origin[1], Coor_Origin[2] + k * 25 + k1 * 5);
                        GL.Vertex3(0 + Coor_Interval_Height, Coor_Origin[1], Coor_Origin[2] + k * 25 + k1 * 5);
                        GL.End();
                    }
                }
            }
            else
            {
                for (int k = 0; k < Coor_BigInterval; k++)
                {
                    GL.Begin(BeginMode.Lines);
                    GL.Vertex3(Coor_Origin[0],0, Coor_Origin[2] + k * 25);
                    GL.Vertex3(Coor_Origin[0],0 + Coor_BigInterval_Height, Coor_Origin[2] + k * 25);
                    GL.End();
                    for (int k1 = 1; k1 < 5; k1++)
                    {
                        GL.Begin(BeginMode.Lines);
                        GL.Vertex3(Coor_Origin[0], 0, Coor_Origin[2] + k * 25 + k1 * 5);
                        GL.Vertex3(Coor_Origin[0], 0 + Coor_Interval_Height, Coor_Origin[2] + k * 25 + k1 * 5);
                        GL.End();
                    }
                }
            }

            if (Directionflag == 1)
            {
                GL.Begin(BeginMode.Lines);
                GL.Vertex3(Coor_Origin[0], Coor_Origin[1], 0);
                GL.Vertex3(Coor_Origin[0], Coor_Origin[1] + Coor_Length, 0);
                GL.End();

                GL.Begin(BeginMode.Lines);
                GL.Vertex3(Coor_Origin[0], 0, Coor_Origin[2]);
                GL.Vertex3(Coor_Origin[0], 0, Coor_Origin[2] + Coor_Length);
                GL.End();
            }
            if (Directionflag == 2)
            {
                GL.Begin(BeginMode.Lines);
                GL.Vertex3(Coor_Origin[0], Coor_Origin[1], 0);
                GL.Vertex3(Coor_Origin[0] + Coor_Length, Coor_Origin[1],0);
                GL.End();

                GL.Begin(BeginMode.Lines);
                GL.Vertex3(0, Coor_Origin[1], Coor_Origin[2]);
                GL.Vertex3(0, Coor_Origin[1], Coor_Origin[2] + Coor_Length);
                GL.End();
            }
            if (Directionflag == 3)
            {
                GL.Begin(BeginMode.Lines);
                GL.Vertex3(Coor_Origin[0], 0, Coor_Origin[2]);
                GL.Vertex3(Coor_Origin[0] + Coor_Length, 0, Coor_Origin[2]);
                GL.End();

                GL.Begin(BeginMode.Lines);
                GL.Vertex3(0, Coor_Origin[1], Coor_Origin[2]);
                GL.Vertex3(0, Coor_Origin[1] + Coor_Length, Coor_Origin[2]);
                GL.End();
            }
           


        }
        private void DrawCube()
        {
            Constant constant = new Constant();
            //  GL.PushMatrix();

            //绘制车流
            if (_citySmart._surfaceSet.Count == 0)
            {
                return;
            }
            int color = 0;
            GL.Normal3(0, 0, 1);
            int i = 0;
            int vehic = 0;
            int colorSum = _citySmart._colorSet.ElementAt(color);
            int vehicSum = _citySmart._unitSet.ElementAt(vehic);
            GL.Color3(constant.COLOR_SET[color].red, constant.COLOR_SET[color].green, constant.COLOR_SET[color].blue); //初始化颜色
            Surface preSurface = _citySmart._surfaceSet.ElementAt(0);

            double ratio = 0.01;

            Point tP1 = new Point();
            Point tP2 = new Point();
            Point tP3 = new Point();
            Point tP4 = new Point();

            tP1.x = preSurface.p1.x;
            tP1.y = preSurface.p1.y;
            tP1.z = preSurface.p1.z;

            tP2.x = preSurface.p2.x;
            tP2.y = preSurface.p2.y;
            tP2.z = preSurface.p2.z;

            tP3.x = preSurface.p3.x;
            tP3.y = preSurface.p3.y;
            tP3.z = preSurface.p3.z;

            tP4.x = preSurface.p4.x;
            tP4.y = preSurface.p4.y;
            tP4.z = preSurface.p4.z;

            Point cP = new Point((tP1.x + tP2.x) / 2, (tP1.y + tP2.y) / 2, tP1.z);
            tP1.x = cP.x - (tP1.x - cP.x) * ratio;
            tP1.y = cP.y - (tP1.y - cP.y) * ratio;

            tP2.x = cP.x - (tP2.x - cP.x) * ratio;
            tP2.y = cP.y - (tP2.y - cP.y) * ratio;

            tP3.x = cP.x - (tP3.x - cP.x) * ratio;
            tP3.y = cP.y - (tP3.y - cP.y) * ratio;

            tP4.x = cP.x - (tP4.x - cP.x) * ratio;
            tP4.y = cP.y - (tP4.y - cP.y) * ratio;

            preSurface.p1 = tP1;
            preSurface.p2 = tP2;
            preSurface.p3 = tP3;
            preSurface.p4 = tP4;


            foreach (Surface surface in _citySmart._surfaceSet)
            {
                while (i == colorSum && color < _citySmart._colorSet.Count)
                {
                    color++;
                    GL.Color3(constant.COLOR_SET[ConfigInfo.colorSet[color]].red, constant.COLOR_SET[ConfigInfo.colorSet[color]].green, constant.COLOR_SET[ConfigInfo.colorSet[color]].blue); //修改颜色                        
                    colorSum += _citySmart._colorSet.ElementAt(color);
                }

                while (i == vehicSum && vehic < _citySmart._unitSet.Count - 1)
                {
                    vehic++;
                    preSurface = surface;
                    vehicSum += _citySmart._unitSet.ElementAt(vehic);
                }

                //GL.Color3(0, 0.5, 1);
                //绘制六面体
                Draw6FaceObj(preSurface, surface);


                preSurface = surface;
                if (i == 52)
                {
                    //Console.Write("");
                }
                i++;
            }
            GL.Color3(1.0f, 1.0f, 1.0f);

        }


        private void HandleSelectVehileRoute(object sender, EventArgs e)
        {
            string routDec = ((SelectParams)e).routDec;

            //获取该路径的所有路径选择
            List<string> param = new List<string>();
            string SQL = "select distinct(Route) from data530_VEH_RECORD where RoutDec=" + routDec;
            OleDbDataReader dataReader = _easyAccess.ExecuteDataReader(SQL);
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    param.Add(dataReader.GetValue(0).ToString());
                }
            }

            //获取该路径现已有的所有选择
            VehicleRoute vehicleRoute = new VehicleRoute();
            if (_vehicleRoutesMap.ContainsKey(routDec))
            {
                vehicleRoute = _vehicleRoutesMap[routDec];
            }

            Choice choice = new Choice(routDec, param, vehicleRoute.routes);
            choice.submitSelectOpt += new EventHandler(HandleSubmitSelectOpt);
            choice.ShowDialog();

        }

        private void HandleSubmitSelectOpt(object sender, EventArgs e)
        {
            SelectParams arg = (SelectParams)e;
            switch (arg.option)
            {
                case SelectOption.LANE:
                    _lanes = arg.param;
                    break;
                case SelectOption.LINK:
                    _links = arg.param;
                    break;
                case SelectOption.ROUT_DEC:
                    _routDec = arg.param;
                    foreach (string routDec in arg.param)
                    {
                        if (!_vehicleRoutesMap.ContainsKey(routDec)) //判断是否对其进行了帅选，未筛选则填入空值
                        {
                            _vehicleRoutesMap.Add(routDec, new VehicleRoute(routDec));
                        }
                    }
                    break;
                case SelectOption.VEHC_TYPE:
                    _vehcTypes = arg.param;
                    break;
                case SelectOption.VEHICLE_ROUTE:
                    {
                        if (_vehicleRoutesMap.ContainsKey(arg.routDec))
                        {
                            _vehicleRoutesMap[arg.routDec] = new VehicleRoute(arg.routDec, arg.param);
                        }
                        else
                        {
                            _vehicleRoutesMap.Add(arg.routDec, new VehicleRoute(arg.routDec, arg.param));
                        }
                    }
                    break;
            }
        }
        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            List<string> param = new List<string>();
            string SQL = "select distinct(RoutDec) from data530_VEH_RECORD ";
            OleDbDataReader dataReader = _easyAccess.ExecuteDataReader(SQL);
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    param.Add(dataReader.GetValue(0).ToString());
                }
            }


            Choice choice = new Choice(SelectOption.ROUT_DEC, param, _routDec);
            choice.submitSelectOpt += new EventHandler(HandleSubmitSelectOpt);
            choice.selectVehileRoute += new EventHandler(HandleSelectVehileRoute);

            choice.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<string> param = new List<string>();
            string SQL = "select distinct(Type) from data530_VEH_RECORD where 1=1  ";
            //SQL += GenerateQuaSQL(SelectOption.ROUT_DEC).Length != 0 ? " and " + GenerateQuaSQL(SelectOption.ROUT_DEC) + " " : " ";


            OleDbDataReader dataReader = _easyAccess.ExecuteDataReader(SQL);
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    param.Add(dataReader.GetValue(0).ToString());
                }
            }
            Choice choice = new Choice(SelectOption.VEHC_TYPE, param, _vehcTypes);
            choice.submitSelectOpt += new EventHandler(HandleSubmitSelectOpt);
            choice.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            List<string> param = new List<string>();
            string SQL = "select distinct(Link) from data530_VEH_RECORD where 1=1  ";
            //SQL += GenerateQuaSQL(SelectOption.ROUT_DEC).Length != 0 ? " and " + GenerateQuaSQL(SelectOption.ROUT_DEC) + " " : " ";
            SQL += GenerateQuaSQL(SelectOption.VEHC_TYPE).Length != 0 ? " and " + GenerateQuaSQL(SelectOption.VEHC_TYPE) + " " : " ";


            OleDbDataReader dataReader = _easyAccess.ExecuteDataReader(SQL);
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    param.Add(dataReader.GetValue(0).ToString());
                }
            }
            Choice choice = new Choice(SelectOption.LINK, param, _links);
            choice.submitSelectOpt += new EventHandler(HandleSubmitSelectOpt);
            choice.ShowDialog();
        }


        private void button7_Click(object sender, EventArgs e)
        {
            List<string> param = new List<string>();
            string SQL = "select distinct(Lane) from data530_VEH_RECORD where 1=1  ";
            //SQL += GenerateQuaSQL(SelectOption.ROUT_DEC).Length != 0 ? " and " + GenerateQuaSQL(SelectOption.ROUT_DEC) + " " : " ";
            SQL += GenerateQuaSQL(SelectOption.VEHC_TYPE).Length != 0 ? " and " + GenerateQuaSQL(SelectOption.VEHC_TYPE) + " " : " ";
            SQL += GenerateQuaSQL(SelectOption.LINK).Length != 0 ? " and " + GenerateQuaSQL(SelectOption.LINK) + " " : " ";


            OleDbDataReader dataReader = _easyAccess.ExecuteDataReader(SQL);
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    param.Add(dataReader.GetValue(0).ToString());
                }
            }
            Choice choice = new Choice(SelectOption.LANE, param, _lanes);
            choice.submitSelectOpt += new EventHandler(HandleSubmitSelectOpt);
            //choice.selectVehileRoute += new EventHandler(HandleSelectVehileRoute);
            choice.ShowDialog();
        }

        private string GenerateQuaSQL(SelectOption param_)
        {
            List<string> v = new List<string>();
            string name = "";
            switch (param_)
            {
                case SelectOption.ROUT_DEC:
                    {
                        v = _routDec;
                        name = "RoutDec";
                        break;
                    }
                case SelectOption.VEHC_TYPE:
                    {
                        v = _vehcTypes;
                        name = "Type";
                        break;
                    }
                case SelectOption.LINK:
                    {
                        v = _links;
                        name = "Link";
                        break;
                    }
                case SelectOption.LANE:
                    {
                        v = _lanes;
                        name = "Lane";
                        break;
                    }
            }

            string tmp = "";
            foreach (string pV in v)
            {
                tmp += name;
                tmp += " = ";
                tmp += pV;
                tmp += " or ";
            }
            if (tmp.Length != 0)
            {
                tmp = tmp.Substring(0, tmp.Length - 3);
                tmp = " (" + tmp + ") ";
            }
            return tmp;
        }

        private void InitProgram()
        {
            _routDec = new List<string>();
            _vehcTypes = new List<string>();
            _links = new List<string>();
            _lanes = new List<string>();
            _vehicleRoutes = new List<VehicleRoute>();
            _vehicleRoutesMap = new Dictionary<string, VehicleRoute>();
        }


        /*

        private void button9_Click(object sender, EventArgs e)
        {
            double startTime = 0;
            double endTime = 0;
            double buttonX = 0;
            double topX = 0;
            double buttonY = 0;
            double topY = 0;
            double realSize = 0;


            try
            {
                startTime = Convert.ToDouble(this.textBox3.Text);
                endTime = Convert.ToDouble(this.textBox4.Text);
                buttonX = Convert.ToDouble(this.textBox6.Text);
                topX = Convert.ToDouble(this.textBox5.Text);
                buttonY = Convert.ToDouble(this.textBox8.Text);
                topY = Convert.ToDouble(this.textBox7.Text);
                realSize = Convert.ToDouble(this.textBox9.Text);
                //length = Convert.ToDouble(this.textBox15.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("请检查您的输入");
                return;
            }

            if (startTime >= endTime)
            {
                MessageBox.Show("起始时间必须小于终止时间");
                return;
            }
            if (buttonX >= topX)
            {
                MessageBox.Show("X轴起始必须小于X轴终止");
                return;
            }
            if (buttonY >= topY)
            {
                MessageBox.Show("Y轴起始必须小于Y轴终止");
                return;
            }
            if (realSize < 0.001)
            {
                MessageBox.Show("您输入的实际面积过小，请重新输入");
            }

            /*
            if (length < 0.001)
            {
                MessageBox.Show("请您输入正确的车道长度");
            }
           
              
            CRstSet rst = _citySmart.CSCompute(startTime, endTime, buttonX, topX, buttonY, topY, realSize);
            //MessageBox.Show("本空间共有" + rst.carNum.ToString() + "辆汽车\r\n" + "时空利用率为" + (rst.utilization * 100).ToString() + "%");
            this.textBox14.Text = PrivateRound((rst.utilization * 100), 4).ToString();
        }
        */
        private void 整理数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //_citySmart.CleanDB();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            string path = Application.StartupPath;
            openFileDialog.InitialDirectory = path;
            openFileDialog.Filter = "所有文件|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //0223,未操作读取的文件
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            isClipPlaneDisplay = false;
        }

        private void LoadBackGround_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string path = Application.StartupPath;
            openFileDialog.InitialDirectory = path + "\\pic";
            openFileDialog.Filter = "位图文件|*.bmp*|jpg文件|*.jpg|所有文件|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fName = openFileDialog.FileName;
                BackGroundImg = new Bitmap(fName);
                //  BackGroundImg.RotateFlip(RotateFlipType.RotateNoneFlipY);
                bmp_data = BackGroundImg.LockBits(new Rectangle(0, 0, BackGroundImg.Width, BackGroundImg.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                //bmp_data = BackGroundImg.LockBits(new Rectangle(0, 0, BackGroundImg.Width, BackGroundImg.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                BackGround_Init = true;
                if (glControlLoaded)
                    textureID = LoadTexture();

            }
        }
        private void RenderBackGround()
        {
            if (BackGround_Init)
            {
                GL.PushMatrix();
                // GL.RasterPos2(128.779236, - 14.708110);
                GL.DrawPixels(bmp_data.Width, bmp_data.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte,
                        bmp_data.Scan0);
                GL.PopMatrix();

            }
        }
        private int LoadTexture()
        {
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);


            //  bmp_data = BackGroundImg.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            return id;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BackGround_Init)
                BackGroundImg.UnlockBits(bmp_data);
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            this.tabPage2.Parent = null;
            this.tabPage1.Parent = null;

            glControlLoaded = true;

            GL.ClearColor(System.Drawing.Color.Gray);

            if (BackGround_Init)
                textureID = LoadTexture();

            reshape(tabPage4.Width, tabPage4.Height);
            // GL.PushMatrix();
            // GL.Rotate(10, 0.5, 0.5, 0.5);  
            //GL.PopMatrix();


        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {

            this.tabPage2.Parent = null;
            this.tabPage1.Parent = null;

            if (!glControlLoaded)
                return;
            //  GL.Rotate(10, 0.5, 0.5, 0.5);
            glControl.MakeCurrent();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);


            
           // Matrix4 t_mv = Matrix4.LookAt(r * (float)Math.Cos(c * du), h, r * (float)Math.Sin(c * du), 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);

            Matrix4 t_mv = Matrix4.LookAt(r * (float)Math.Sin(c*camera_fy)*(float)Math.Cos(c*camera_alpha),
                                          r * (float)Math.Sin(c*camera_fy) * (float)Math.Sin(c*camera_alpha), 
                r * (float)Math.Cos(c*camera_fy), 
                0.0f, 0.0f, 0.0f, 
                0.0f, 0.0f, 1.0f);
            

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.LoadMatrix(ref t_mv);

            GL.PushMatrix();
            GL.Scale(zZoom, zZoom, zZoom);
            GL.Translate(xTrans, -yTrans, 0);
            //  System.Dynamic.(xTrans);
            //  Console.WriteLine(zZoom);
            //Console.WriteLine(-yTrans);
            //GL.Rotate(xRotAng, 0.0d, 1.0, 0.0d);	         //   旋转x轴 
            //GL.Rotate(yRotAng, 1.0, 0.0d, 0.0d);	 //   旋转y轴 

            //剪切
            if (isClipPlaneDisplay)
            {

                GL.ClipPlane((OpenTK.Graphics.OpenGL.ClipPlaneName.ClipPlane0 + 0), ClipPlanePara);
                GL.Enable((EnableCap.ClipPlane0 + 0));
                //Console.WriteLine(ClipPlanePara[3]);

                double[] eqn1 = { -ClipPlanePara[0], -ClipPlanePara[1], -ClipPlanePara[2], -ClipPlanePara[3] + _thickness };


                //在剪切面显示坐标轴
                //垂直于z轴
                if (ClipPlanePara[0] == 0 && ClipPlanePara[1] == 0)
                {
                    eqn1[3] = -ClipPlanePara[3] + _thickness;
                    GL.ClipPlane((OpenTK.Graphics.OpenGL.ClipPlaneName.ClipPlane1 + 0), eqn1);
                    GL.Enable((EnableCap.ClipPlane1 + 0));

                    double t = ClipPlanePara[3] / ClipPlanePara[2];
                    double[] origin = new double [3];
                    if (System.Math.Abs(t) > System.Math.Abs(cutPoint[0].z))
                    {
                        origin[0] = -150;
                        origin[1] = -150;
                        origin[2] =System.Math.Abs(t) - 0.01 ;
                    }
                    else
                    {
                        origin[0] = -150;
                        origin[1] = -150;
                        origin[2] = System.Math.Abs(t) + 0.01;
                    }
                    DrawCoordination(300, 25, 5, 5, 2.5, origin,3);
                }
                //垂直于y轴
                if (ClipPlanePara[0] == 0 && ClipPlanePara[2] == 0)
                {
                    GL.ClipPlane((OpenTK.Graphics.OpenGL.ClipPlaneName.ClipPlane1 + 0), eqn1);
                    GL.Enable((EnableCap.ClipPlane1 + 0));
                    double y = ClipPlanePara[3] / ClipPlanePara[1];
                    double[] origin = new double[3];
                    if (System.Math.Abs(y) > System.Math.Abs(cutPoint[0].y))
                    {
                        origin[0] = -150;
                        origin[1] = System.Math.Abs(y) - 0.01;
                        origin[2] = -150;
                    }
                    else
                    {
                        origin[0] = -150;
                        origin[1] = System.Math.Abs(y) + 0.01;
                        origin[2] = -150;
                    }
                    DrawCoordination(300, 25, 5, 5, 2.5, origin,2);
                }
                //垂直于x轴
                if (ClipPlanePara[1] == 0 && ClipPlanePara[2] == 0)
                {
                    GL.ClipPlane((OpenTK.Graphics.OpenGL.ClipPlaneName.ClipPlane1 + 0), eqn1);
                    GL.Enable((EnableCap.ClipPlane1 + 0));

                    double x = ClipPlanePara[3] / ClipPlanePara[0];
                    double[] origin = new double[3];
                    if (System.Math.Abs(x) > System.Math.Abs(cutPoint[0].x))
                    {
                        origin[0] = System.Math.Abs(x) - 0.01;
                        origin[1] = -150;
                        origin[2] = -150;
                    }
                    else
                    {
                        origin[0] = System.Math.Abs(x) + 0.01;
                        origin[1] = -150;
                        origin[2] = -150 ;
                    }
                    DrawCoordination(300, 25, 5, 5, 2.5, origin,1);
                }


            }
            else 
            {
                //绘制坐标轴
                double[] origin = { 0, 0, 0 };
                DrawCoordination(300, 25, 5, 5, 2.5, origin,0);
                double x_change = Convert.ToDouble(accuracy_x);//0727
                double y_change = Convert.ToDouble(accuracy_y);
                double t_change = Convert.ToDouble(accuracy_t);
           
                double[] Mesh_origin = new double[3];
                Mesh_origin[0] = Convert.ToDouble(start_x);
                Mesh_origin[1] = Convert.ToDouble(start_y);
                Mesh_origin[2] = Convert.ToDouble(start_t);
                double[] Mesh_destination = new double[3];
                Mesh_destination[0] = Convert.ToDouble(end_x);
                Mesh_destination[1] = Convert.ToDouble(end_y);
                Mesh_destination[2] = Convert.ToDouble(end_t);
                //DrawMesh(x_change, y_change, t_change, Mesh_origin, Mesh_destination);
                if (Convert.ToDouble(accuracy_t) != 0)
                {
                    DrawMesh1(x_change, y_change, t_change, Mesh_origin, Mesh_destination);
                   
                    GL.Color3(1.0f, 1.0f, 1.0f);
                }


            }



            //半透明立方体切割
            if (IsSemitransparencyCube)
            {
                GL.ClipPlane(OpenTK.Graphics.OpenGL.ClipPlaneName.ClipPlane0, CudePlanePara[0]);
                GL.Enable(EnableCap.ClipPlane0);

                GL.ClipPlane(OpenTK.Graphics.OpenGL.ClipPlaneName.ClipPlane1, CudePlanePara[1]);
                GL.Enable(EnableCap.ClipPlane1);

                GL.ClipPlane(OpenTK.Graphics.OpenGL.ClipPlaneName.ClipPlane2, CudePlanePara[2]);
                GL.Enable(EnableCap.ClipPlane2);

                GL.ClipPlane(OpenTK.Graphics.OpenGL.ClipPlaneName.ClipPlane3, CudePlanePara[3]);
                GL.Enable(EnableCap.ClipPlane3);

                GL.ClipPlane(OpenTK.Graphics.OpenGL.ClipPlaneName.ClipPlane4, CudePlanePara[4]);
                GL.Enable(EnableCap.ClipPlane4);

                GL.ClipPlane(OpenTK.Graphics.OpenGL.ClipPlaneName.ClipPlane5, CudePlanePara[5]);
                GL.Enable(EnableCap.ClipPlane5);

                //绘制车流
                DrawCube();
                if (_strongFlag)
                {
                    DrawOuter();
                }
                DrawSemitransparencyCube();

            }
            else
            {
                //绘制车流
                DrawCube();
                if (_strongFlag)
                {
                    DrawOuter();
                }
                DrawSemitransparencyCube();
                //GL.Disable(EnableCap.ClipPlane0);
                //GL.Disable(EnableCap.ClipPlane1);
                //GL.Disable(EnableCap.ClipPlane2);
                //GL.Disable(EnableCap.ClipPlane3);
                //GL.Disable(EnableCap.ClipPlane4);
                //GL.Disable(EnableCap.ClipPlane5);

            }

         


            //    GL.PopMatrix();

            //绘制背景图
            if (BackGround_Init)
            {
                //禁用剪切平面
                GL.Disable((EnableCap.ClipPlane0));
                GL.Disable((EnableCap.ClipPlane1));
                GL.Disable(EnableCap.ClipPlane2);
                GL.Disable(EnableCap.ClipPlane3);
                GL.Disable(EnableCap.ClipPlane4);
                GL.Disable(EnableCap.ClipPlane5);

                GL.Normal3(0.0f, 0.0f, 1.0f);
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, textureID);
                GL.Begin(BeginMode.Quads);

                GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(BackGround_Coordinate[0], BackGround_Coordinate[1]);//左上
                GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(BackGround_Coordinate[2], BackGround_Coordinate[1]);//右上
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(BackGround_Coordinate[2], BackGround_Coordinate[3]);//右下
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(BackGround_Coordinate[0], BackGround_Coordinate[3]);//左下

                GL.End();

                GL.Disable(EnableCap.Texture2D);

            }

            GL.PopMatrix();
            glControl.SwapBuffers();
        }

       

        private void glControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                xRotAngInit = e.X;
                yRotAngInit = e.Y;
                oldmx = e.X;
                oldmy = e.Y;

            }
            if (e.Button == MouseButtons.Left)
            {
                xTransInit = e.X;
                yTransInit = e.Y;
            }
        }
        private void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                xRotAng += (e.X - xRotAngInit) / (float)(zZoom * 300);
                yRotAng += (e.Y - yRotAngInit) / (float)(zZoom * 300);
                xRotAngInit = e.X;
                yRotAngInit = e.Y;

                /*
                du += (int)((float)(e.X - oldmx) / (float)(zZoom * 300)); //鼠标在窗口x轴方向上的增量加到视点绕y轴的角度上，这样就左右转了
                h += 0.03f * (e.Y - oldmy) / (float)(zZoom * 300); //鼠标在窗口y轴方向上的改变加到视点的y坐标上，就上下转了
                if (h > 100.0f) h = 100.0f; //视点y坐标作一些限制，不会使视点太奇怪
                else if (h < -100.0f) h = -100.0f;

                */



                camera_alpha += (float)(oldmx -  e.X ) / (float)(zZoom * 300);                  /*根据鼠标移动的方向设置新的球坐标*/
                camera_fy += (float)(oldmy - e.Y) / (float)(zZoom * 300);

                if (camera_fy > 90) camera_fy = 90;
                if (camera_fy < 0) camera_fy = 0.01f;

                if (camera_alpha > 180) camera_alpha = 180;
                if (camera_alpha < -180) camera_alpha = -180f;
 
                  /*将当前坐标置为旧坐标*/

                oldmx = e.X; oldmy = e.Y;

                glControl.Invalidate();                                     //调用glDraw()重绘window 
            }
            if (e.Button == MouseButtons.Left)
            {
                xTrans += (e.X - xTransInit) / (float)(zZoom * 300);
                yTrans += (e.Y - yTransInit) / (float)(zZoom * 300);
                xTransInit = e.X;
                yTransInit = e.Y;
                glControl.Invalidate();
            }

        }
        private void tabPage4_Resize(object sender, EventArgs e)
        {
            glControl.Width = this.Width;
            glControl.Height = this.Height;
            reshape(tabPage4.Width, tabPage4.Height);
            //SetupViewport(this.Width, this.Height);

        }
        private void reverseCudePlanePara(double[] PlanePara)
        {
            for (int i = 0; i < 4; i++)
            {
                PlanePara[i] = -PlanePara[i];
            }
        }
      

        private void button_ClearCudeCute_Click(object sender, EventArgs e)
        {
            //消除切割
            IsSemitransparencyCube = false;
        }
        private void DrawMesh1(double Mesh_delt_len, double Mesh_delt_width, double Mesh_delt_height, double[] Mesh_Origin, double[] Mesh_destination)
        //画坐标轴，Coor_Length坐标长度，Coor_BigInterval_Len大间隔，Coor_Interval_len小间隔
        //Coor_BigInterval_Height高度大间隔，Coor_Interval_Height高度小间隔，Coor_Origin坐标起点,Directionflag代表x,y,z
        {
            double Length = Mesh_destination[0] - Mesh_Origin[0];
            double Width = Mesh_destination[1] - Mesh_Origin[1];
            double Height = Mesh_destination[2] - Mesh_Origin[2];

            int numx = (int)(Length / Mesh_delt_len);
            int numy = (int)(Width / Mesh_delt_width);
            int numz = (int)(Height / Mesh_delt_height);

            double _xlen = Mesh_delt_len;
            double _ylen = Mesh_delt_len;
            double _zlen = Mesh_delt_height;
            /*
             //绘制立方体小格子
             string SQL = "select * from data530_VEH_RECORD where 1=1 ";
             OleDbDataReader dataReader = _easyAccess.ExecuteDataReader(SQL);
             if (dataReader.HasRows)
             {
                 while (dataReader.Read())
                 {
                     Cube_Info info1 = new Cube_Info();
                     info1.t = Convert.ToDouble(dataReader.GetValue(4).ToString());
                     info1.x = Convert.ToDouble(dataReader.GetValue(3).ToString());
                     info1.y = Convert.ToDouble(dataReader.GetValue(5).ToString());
                     // int c=Convert.ToInt32(dataReader.GetValue(5).ToString());
                     //if ( c==1)
                     //{
                     DrawNewCube(info1, _xlen, _ylen, _zlen);//WXH
                     //}
                 }
             }
             */
            //绘制平行于x轴的直线
            //线条样式
            GL.LineWidth(0.5f);
            GL.LineStipple(1, 0x0303);//线条样式
            GL.Enable(EnableCap.Blend);
            //glBlendFunc(GL_SRC_ALPHA, GL_DST_COLOR);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.DstAlpha);
            //GL.Color4(0.1, 0.1, 0.1, 0.1);

            int xi = Convert.ToInt32(Mesh_Origin[0]);
            int yi = Convert.ToInt32(Mesh_Origin[1]);
            int zi = Convert.ToInt32(Mesh_Origin[2]);
            //绘制平行于X的直线            
            GL.Begin(BeginMode.Lines);
            //GL.Enable(GL_LINE_SMOOTH)
            GL.Color4(0.8, 1, 0.6, 0.1);
            //0227原来整体画法

            //0227线画网格，有分割

            double width_in1 = 1.5;
            double width_out1 = 11.1;
            double width_in2 = -1.5;
            double width_out2 = -11.1;

            double width_in3 = 0.5;
            double width_out3 = 11.3;
            double width_in4 = -0.8;
            double width_out4 = -11.6;

            double length_in1 = 15 - 1.2;
            double length_out1 = 100.2;
            double length_in2 = -15 + 1.2;
            double length_out2 = -100.2;

            double z0 = 0;
            double z1 = Convert.ToDouble(end_t) - Convert.ToDouble(start_t);


            _zlen = Convert.ToDouble(accuracy_t);
            _ylen = Convert.ToDouble(accuracy_y);
            _xlen = Convert.ToDouble(accuracy_x);
            double z;
            if ((_zlen == 0) || (z1 == 0))
            {
                numz = 0;

            }
            else
            {
                numz = Convert.ToInt16((z1 - z0) / _zlen);
            }


            //东进口道
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (yi = 0; yi <= (width_out1 - width_in1) / 1.2; yi++)
                {
                    double y = _ylen * yi + width_in1;

                    GL.Vertex3(length_in1, y, z);
                    GL.Vertex3(length_out1, y, z);

                }

            }
            //绘制平行于Y的直线D:\大四\三维轨迹\SouceProgram1\CitySmart 1.07\CitySmart\App.config
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (xi = 0; xi <= (length_out1 - length_in1) / 1.2; xi++)
                {
                    double x = _xlen * xi + length_in1;
                    GL.Vertex3(x, width_in1, z);
                    GL.Vertex3(x, width_out1, z);
                }
            }

            //绘制平行于Z的直线
            for (yi = 0; yi <= (width_out1 - width_in1) / 1.2; yi++)
            {
                double y = _ylen * yi + width_in1;
                for (xi = 0; xi <= (length_out1 - length_in1) / 1.2; xi++)
                {
                    double x = _xlen * xi + length_in1;
                    GL.Vertex3(x, y, z0);
                    GL.Vertex3(x, y, z1);

                }

            }

            //东出口道
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (yi = 0; yi <= -(width_out2 - width_in2) / 1.2; yi++)
                {
                    double y = _ylen * yi + width_out2;

                    GL.Vertex3(length_in1, y, z);
                    GL.Vertex3(length_out1, y, z);

                }

            }
            //绘制平行于Y的直线D:\大四\三维轨迹\SouceProgram1\CitySmart 1.07\CitySmart\App.config
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (xi = 0; xi <= (length_out1 - length_in1) / 1.2; xi++)
                {
                    double x = _xlen * xi + length_in1;
                    GL.Vertex3(x, width_out2, z);
                    GL.Vertex3(x, width_in2, z);
                }
            }

            //绘制平行于Z的直线
            for (yi = 0; yi <= -(width_out2 - width_in2) / 1.2; yi++)
            {
                double y = _ylen * yi + width_out2;
                for (xi = 0; xi <= (length_out1 - length_in1) / 1.2; xi++)
                {
                    double x = _xlen * xi + length_in1;
                    GL.Vertex3(x, y, z0);
                    GL.Vertex3(x, y, z1);

                }

            }


            //西出口道
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (yi = 0; yi <= (width_out1 - width_in1) / 1.2; yi++)
                {
                    double y = _ylen * yi + width_in1;

                    GL.Vertex3(length_in2, y, z);
                    GL.Vertex3(length_out2, y, z);

                }
            }


            //绘制平行于Y的直线D:\大四\三维轨迹\SouceProgram1\CitySmart 1.07\CitySmart\App.config
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (xi = 0; xi <= -(length_out2 - length_in2) / 1.2; xi++)
                {
                    double x = _xlen * xi + length_out2;
                    GL.Vertex3(x, width_in1, z);
                    GL.Vertex3(x, width_out1, z);
                }
            }

            //绘制平行于Z的直线
            for (yi = 0; yi <= (width_out1 - width_in1) / 1.2; yi++)
            {
                double y = _ylen * yi + width_in1;
                for (xi = 0; xi <= -(length_out2 - length_in2) / 1.2; xi++)
                {
                    double x = _xlen * xi + length_out2;
                    GL.Vertex3(x, y, z0);
                    GL.Vertex3(x, y, z1);

                }

            }

            //西进口道
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (yi = 0; yi <= -(width_out2 - width_in2) / 1.2; yi++)
                {
                    double y = _ylen * yi + width_out2;

                    GL.Vertex3(length_in2, y, z);
                    GL.Vertex3(length_out2, y, z);

                }
            }

            //绘制平行于Y的直线D:\大四\三维轨迹\SouceProgram1\CitySmart 1.07\CitySmart\App.config
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (xi = 0; xi <= -(length_out2 - length_in2) / 1.2; xi++)
                {
                    double x = _xlen * xi + length_out2;
                    GL.Vertex3(x, width_in2, z);
                    GL.Vertex3(x, width_out2, z);
                }
            }

            //绘制平行于Z的直线
            for (yi = 0; yi <= -(width_out2 - width_in2) / 1.2; yi++)
            {
                double y = _ylen * yi + width_out2;
                for (xi = 0; xi <= -(length_out2 - length_in2) / 1.2; xi++)
                {
                    double x = _xlen * xi + length_out2;
                    GL.Vertex3(x, y, z0);
                    GL.Vertex3(x, y, z1);

                }

            }


            //北出口道
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (yi = 0; yi <= (length_out1 - length_in1) / 1.2; yi++)
                {
                    double y = _ylen * yi + length_in1;

                    GL.Vertex3(width_in3, y, z);
                    GL.Vertex3(width_out3, y, z);

                }
            }

            //绘制平行于Y的直线D:\大四\三维轨迹\SouceProgram1\CitySmart 1.07\CitySmart\App.config
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (xi = 0; xi <= (width_out3 - width_in3) / 1.2; xi++)
                {
                    double x = _xlen * xi + width_in3;
                    GL.Vertex3(x, length_in1, z);
                    GL.Vertex3(x, length_out1, z);
                }
            }

            //绘制平行于Z的直线
            for (yi = 0; yi <= (length_out1 - length_in1) / 1.2; yi++)
            {
                double y = _ylen * yi + length_in1;
                for (xi = 0; xi <= (width_out3 - width_in3) / 1.2; xi++)
                {
                    double x = _xlen * xi + width_in3;
                    GL.Vertex3(x, y, z0);
                    GL.Vertex3(x, y, z1);

                }

            }

            //北进口道
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (yi = 0; yi <= (length_out1 - length_in1) / 1.2; yi++)
                {
                    double y = _ylen * yi + length_in1;

                    GL.Vertex3(width_in4, y, z);
                    GL.Vertex3(width_out4, y, z);

                }
            }


            //绘制平行于Y的直线D:\大四\三维轨迹\SouceProgram1\CitySmart 1.07\CitySmart\App.config
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (xi = 0; xi <= -(width_out4 - width_in4) / 1.2; xi++)
                {
                    double x = _xlen * xi + width_out4;
                    GL.Vertex3(x, length_in1, z);
                    GL.Vertex3(x, length_out1, z);
                }
            }

            //绘制平行于Z的直线
            for (yi = 0; yi <= (length_out1 - length_in1) / 1.2; yi++)
            {
                double y = _ylen * yi + length_in1;
                for (xi = 0; xi <= -(width_out4 - width_in4) / 1.2; xi++)
                {
                    double x = _xlen * xi + width_out4;
                    GL.Vertex3(x, y, z0);
                    GL.Vertex3(x, y, z1);

                }

            }


            //南出口道
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (yi = 0; yi <= -(length_out2 - length_in2) / 1.2; yi++)
                {
                    double y = _ylen * yi + length_out2;

                    GL.Vertex3(width_in4, y, z);
                    GL.Vertex3(width_out4, y, z);

                }
            }

            //绘制平行于Y的直线D:\大四\三维轨迹\SouceProgram1\CitySmart 1.07\CitySmart\App.config
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (xi = 0; xi <= -(width_out4 - width_in4) / 1.2; xi++)
                {
                    double x = _xlen * xi + width_out4;
                    GL.Vertex3(x, length_in2, z);
                    GL.Vertex3(x, length_out2, z);
                }
            }

            //绘制平行于Z的直线
            for (yi = 0; yi <= -(length_out2 - length_in2) / 1.2; yi++)
            {
                double y = _ylen * yi + length_out2;
                for (xi = 0; xi <= -(width_out4 - width_in4) / 1.2; xi++)
                {
                    double x = _xlen * xi + width_out4;
                    GL.Vertex3(x, y, z0);
                    GL.Vertex3(x, y, z1);

                }

            }


            //南进口道
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (yi = 0; yi <= -(length_out2 - length_in2) / 1.2; yi++)
                {
                    double y = _ylen * yi + length_out2;

                    GL.Vertex3(width_in3, y, z);
                    GL.Vertex3(width_out3, y, z);

                }
            }

            //绘制平行于Y的直线D:\大四\三维轨迹\SouceProgram1\CitySmart 1.07\CitySmart\App.config
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (xi = 0; xi <= (width_out3 - width_in3) / 1.2; xi++)
                {
                    double x = _xlen * xi + width_in3;
                    GL.Vertex3(x, length_in2, z);
                    GL.Vertex3(x, length_out2, z);
                }
            }

            //绘制平行于Z的直线
            for (yi = 0; yi <= -(length_out2 - length_in2) / 1.2; yi++)
            {
                double y = _ylen * yi + length_out2;
                for (xi = 0; xi <= (width_out3 - width_in3) / 1.2; xi++)
                {
                    double x = _xlen * xi + width_in3;
                    GL.Vertex3(x, y, z0);
                    GL.Vertex3(x, y, z1);

                }

            }


            //交叉口内部,中心部分+-2.4
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (yi = 0; yi <= (30 - 4.8) / 1.2; yi++)
                {
                    double y = _ylen * yi - 15 + 2.4;

                    GL.Vertex3(-15 + 2.4, y, z);
                    GL.Vertex3(15 - 2.4, y, z);

                }
            }

            //绘制平行于Y的直线D:\大四\三维轨迹\SouceProgram1\CitySmart 1.07\CitySmart\App.config
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (xi = 0; xi <= (30 - 4.8) / 1.2; xi++)
                {
                    double x = _xlen * xi - 15 + 2.4;
                    GL.Vertex3(x, -15 + 2.4, z);
                    GL.Vertex3(x, 15 - 2.4, z);
                }
            }

            //绘制平行于Z的直线
            for (yi = 0; yi <= (30 - 4.8) / 1.2; yi++)
            {
                double y = _ylen * yi - 15 + 2.4;
                for (xi = 0; xi <= (30 - 4.8) / 1.2; xi++)
                {
                    double x = _xlen * xi - 15 + 2.4;
                    GL.Vertex3(x, y, z0);
                    GL.Vertex3(x, y, z1);

                }

            }


            //交叉口中心的四个侧面
            double cn1 = -15 + 1.2;
            double cn2 = -15 + 2.4;
            double cn3 = 15 - 1.2;
            double cn4 = 15 - 2.4;
            //西
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (yi = 0; yi <= (30 - 4.8) / 1.2; yi++)
                {
                    double y = _ylen * yi + cn2;

                    GL.Vertex3(cn1, y, z);
                    GL.Vertex3(cn2, y, z);

                }

            }
            //绘制平行于Y的直线D:\大四\三维轨迹\SouceProgram1\CitySmart 1.07\CitySmart\App.config
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (xi = 0; xi <= 1.2 / 1.2; xi++)
                {
                    double x = _xlen * xi + cn1;
                    GL.Vertex3(x, cn2, z);
                    GL.Vertex3(x, cn4, z);
                }
            }

            //绘制平行于Z的直线
            for (yi = 0; yi <= (30 - 4.8) / 1.2; yi++)
            {
                double y = _ylen * yi + cn2;
                for (xi = 0; xi <= 1.2 / 1.2; xi++)
                {
                    double x = _xlen * xi + cn1;
                    GL.Vertex3(x, y, z0);
                    GL.Vertex3(x, y, z1);

                }

            }

            //东
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (yi = 0; yi <= (30 - 4.8) / 1.2; yi++)
                {
                    double y = _ylen * yi + cn2;

                    GL.Vertex3(cn4, y, z);
                    GL.Vertex3(cn3, y, z);

                }

            }
            //绘制平行于Y的直线D:\大四\三维轨迹\SouceProgram1\CitySmart 1.07\CitySmart\App.config
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (xi = 0; xi <= 1.2 / 1.2; xi++)
                {
                    double x = _xlen * xi + cn4;
                    GL.Vertex3(x, cn2, z);
                    GL.Vertex3(x, cn4, z);
                }
            }

            //绘制平行于Z的直线
            for (yi = 0; yi <= (30 - 4.8) / 1.2; yi++)
            {
                double y = _ylen * yi + cn2;
                for (xi = 0; xi <= 1.2 / 1.2; xi++)
                {
                    double x = _xlen * xi + cn4;
                    GL.Vertex3(x, y, z0);
                    GL.Vertex3(x, y, z1);

                }

            }

            //南
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (yi = 0; yi <= 1.2 / 1.2; yi++)
                {
                    double y = _ylen * yi + cn1;

                    GL.Vertex3(cn2, y, z);
                    GL.Vertex3(cn4, y, z);

                }

            }
            //绘制平行于Y的直线D:\大四\三维轨迹\SouceProgram1\CitySmart 1.07\CitySmart\App.config
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (xi = 0; xi <= (30 - 4.8) / 1.2; xi++)
                {
                    double x = _xlen * xi + cn2;
                    GL.Vertex3(x, cn1, z);
                    GL.Vertex3(x, cn2, z);
                }
            }

            //绘制平行于Z的直线
            for (yi = 0; yi <= 1.2 / 1.2; yi++)
            {
                double y = _ylen * yi + cn1;
                for (xi = 0; xi <= (30 - 4.8) / 1.2; xi++)
                {
                    double x = _xlen * xi + cn2;
                    GL.Vertex3(x, y, z0);
                    GL.Vertex3(x, y, z1);

                }

            }

            //北
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (yi = 0; yi <= 1.2 / 1.2; yi++)
                {
                    double y = _ylen * yi + cn4;

                    GL.Vertex3(cn2, y, z);
                    GL.Vertex3(cn4, y, z);

                }
            }

            //绘制平行于Y的直线D:\大四\三维轨迹\SouceProgram1\CitySmart 1.07\CitySmart\App.config
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (xi = 0; xi <= (30 - 4.8) / 1.2; xi++)
                {
                    double x = _xlen * xi + cn2;
                    GL.Vertex3(x, cn4, z);
                    GL.Vertex3(x, cn3, z);
                }
            }

            //绘制平行于Z的直线
            for (yi = 0; yi <= 1.2 / 1.2; yi++)
            {
                double y = _ylen * yi + cn4;
                for (xi = 0; xi <= (30 - 4.8) / 1.2; xi++)
                {
                    double x = _xlen * xi + cn2;
                    GL.Vertex3(x, y, z0);
                    GL.Vertex3(x, y, z1);

                }

            }
            GL.End();

            GL.Enable(EnableCap.Blend);
            //glBlendFunc(GL_SRC_ALPHA, GL_DST_COLOR);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.DstAlpha);
            GL.Color4(0, 0,0, 1);
            GL.Disable(EnableCap.Blend);
           

            // 禁用关闭混合

            //立方体轮廓


            /*
            //原整体画法
            for (zi = 0; zi <= numz; zi++)
            {
                double z = _zlen * zi + Mesh_Origin[2];
                for (yi = 0; yi <= numy; yi++)
                {
                    double y = _ylen * yi + Mesh_Origin[1];

                    GL.Vertex3(Mesh_Origin[0], y, z);
                    GL.Vertex3(Mesh_destination[0], y, z);

                }

            }
            //绘制平行于Y的直线D:\大四\三维轨迹\SouceProgram1\CitySmart 1.07\CitySmart\App.config
            for (zi = 0; zi <= numz; zi++)
            {
                double z = _zlen * zi + Mesh_Origin[2];
                for (xi = 0; xi <= numx; xi++)
                {
                    double x = _xlen * xi + Mesh_Origin[0];
                    GL.Vertex3(x, Mesh_Origin[1], z);
                    GL.Vertex3(x, Mesh_destination[1], z);
                }
            }

            //绘制平行于Z的直线
            for (yi = 0; yi <= numy; yi++)
            {
                double y = _ylen * yi + Mesh_Origin[1];
                for (xi = 0; xi <= numx; xi++)
                {
                    double x = _xlen * xi + Mesh_Origin[0];
                    GL.Vertex3(x, y, Mesh_Origin[2]);
                    GL.Vertex3(x, y, Mesh_destination[2]);

                }

            }
            */
        }
        //0310符合basicinfo的画法，网格
        private void DrawMesh(double Mesh_delt_len, double Mesh_delt_width, double Mesh_delt_height, double[] Mesh_Origin, double[] Mesh_destination)
        {
            //五个板块画，不区分进口道和出口道
            //东西南北路宽的位置,in是小值，out是大值
            double width_in_w = -(Convert.ToDouble(w_width) / 2);
            double width_out_w = (Convert.ToDouble(w_width) / 2);
            double width_in_e = -(Convert.ToDouble(e_width) / 2);
            double width_out_e = (Convert.ToDouble(w_width) / 2);
            double width_in_s = -(Convert.ToDouble(s_width) / 2);
            double width_out_s = (Convert.ToDouble(s_width) / 2);
            double width_in_n = -(Convert.ToDouble(n_width) / 2);
            double width_out_n = (Convert.ToDouble(n_width) / 2);

            //东西南北路长的位置
            double length_in_w = -(Convert.ToDouble(n_width) / 2) - Convert.ToDouble(w_length);
            double length_out_w = -(Convert.ToDouble(n_width) / 2);
            double length_in_e = (Convert.ToDouble(n_width) / 2);
            double length_out_e = (Convert.ToDouble(n_width) / 2) + Convert.ToDouble(e_length);
            double length_in_s = -(Convert.ToDouble(e_width) / 2) - Convert.ToDouble(s_length);
            double length_out_s = -(Convert.ToDouble(e_width) / 2);
            double length_in_n = (Convert.ToDouble(e_width) / 2);
            double length_out_n = (Convert.ToDouble(e_width) / 2) + Convert.ToDouble(n_length);

            double z0 = Convert.ToDouble(start_t);
            double z1 = Convert.ToDouble(end_t);
            double _zlen = Convert.ToDouble(accuracy_t);
            double _ylen = Convert.ToDouble(accuracy_y);
            double _xlen = Convert.ToDouble(accuracy_x);
            double numz = (z1 - z0) / _zlen;

            double z;
            double y;
            double x;
            double zi;
            double yi;
            double xi;
            Cube_Info info = new Cube_Info();

            //东进口道
            for (zi = 0; zi <= numz; zi++)
            {
                for (yi = 0; yi <= (width_out_e - width_in_e) / _ylen; yi++)
                {
                    for (xi = 0; xi <= (length_out_e - length_in_e) / _xlen; xi++)
                    {
                        y = _ylen * yi + width_in_e;
                        z = _zlen * zi + z0;
                        x = _xlen * xi + length_in_e;
                        info.x = x;
                        info.y = y;
                        info.t = z;
                        GL.Enable(EnableCap.Blend);
                        GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.DstAlpha);
                        GL.Color4(0.1, 0.1, 0.1, 0.01);
                        DrawNewCube(info, _xlen, _ylen, _zlen);
                        GL.Disable(EnableCap.DepthTest);
                        DrawNewLoop(info, _xlen, _ylen, _zlen);
                        GL.Enable(EnableCap.DepthTest);
                    }
                }
            }

            //西出口道
            for (zi = 0; zi <= numz; zi++)
            {
                for (yi = 0; yi <= (width_out_w - width_in_w) / _ylen; yi++)
                {
                    for (xi = 0; xi <= (length_out_w - length_in_w) / _xlen; xi++)
                    {
                        y = _ylen * yi + width_in_w;
                        z = _zlen * zi + z0;
                        x = _xlen * xi + length_in_w;
                        info.x = x;
                        info.y = y;
                        info.t = z;
                        GL.Enable(EnableCap.Blend);
                        GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.DstAlpha);
                        GL.Color4(0.5, 0.3, 0.5, 0.01);
                        DrawNewCube(info, _xlen, _ylen, _zlen);
                        GL.Disable(EnableCap.DepthTest);
                        DrawNewLoop(info, _xlen, _ylen, _zlen);
                        GL.Enable(EnableCap.DepthTest);
                    }
                }
            }

            //北出口道
            for (zi = 0; zi <= numz; zi++)
            {

                for (yi = 0; yi <= (length_out_n - length_in_n) / _ylen; yi++)
                {


                    for (xi = 0; xi <= (width_out_n - width_in_n) / _xlen; xi++)
                    {
                        y = _ylen * yi + length_in_n;
                        z = _zlen * zi + z0;
                        x = _xlen * xi + width_in_n;
                        info.x = x;
                        info.y = y;
                        info.t = z;
                        GL.Enable(EnableCap.Blend);
                        GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.DstAlpha);
                        GL.Color4(0.5, 0.3, 0.5, 0.01);
                        DrawNewCube(info, _xlen, _ylen, _zlen);
                        GL.Disable(EnableCap.DepthTest);
                        DrawNewLoop(info, _xlen, _ylen, _zlen);
                        GL.Enable(EnableCap.DepthTest);
                    }
                }
            }

            //南出口道
            for (zi = 0; zi <= numz; zi++)
            {
                for (yi = 0; yi <= (length_out_s - length_in_s) / _ylen; yi++)
                {
                    for (xi = 0; xi <= (width_out_s - width_in_s) / _xlen; xi++)
                    {
                        y = _ylen * yi + length_in_s;
                        z = _zlen * zi + z0;
                        x = _xlen * xi + width_in_s;
                        info.x = x;
                        info.y = y;
                        info.t = z;
                        GL.Enable(EnableCap.Blend);
                        GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.DstAlpha);
                        GL.Color4(0.5, 0.3, 0.5, 0.01);
                        DrawNewCube(info, _xlen, _ylen, _zlen);
                        GL.Disable(EnableCap.DepthTest);
                        DrawNewLoop(info, _xlen, _ylen, _zlen);
                        GL.Enable(EnableCap.DepthTest);
                    }
                }
            }

            //交叉口内部,中心部分+-2.4
            for (zi = 0; zi <= numz; zi++)
            {
                z = _zlen * zi + z0;
                for (yi = 0; yi <= (width_out_e - width_in_e) / _ylen; yi++)
                {
                    for (xi = 0; xi <= (width_out_n - width_in_n) / _xlen; xi++)
                    {
                        z = _zlen * zi + z0;
                        x = _xlen * xi - (width_out_n - width_in_n) / 2;
                        y = _ylen * yi - (width_out_e - width_in_e) / 2;
                        info.x = x;
                        info.y = y;
                        info.t = z;
                        GL.Enable(EnableCap.Blend);
                        GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.DstAlpha);
                        GL.Color4(0.5, 0.3, 0.5, 0.01);
                        DrawNewCube(info, _xlen, _ylen, _zlen);
                        GL.Disable(EnableCap.DepthTest);
                        DrawNewLoop(info, _xlen, _ylen, _zlen);
                        GL.Enable(EnableCap.DepthTest);
                    }

                }
            }
        }

        private void button_BG_Coordination_Click(object sender, EventArgs e)
        {
            BackGround_Coordinate[0] = Convert.ToDouble(this.textBox_BG_LeftBottom_X.Text);
            BackGround_Coordinate[1] = Convert.ToDouble(this.textBox_BG_LeftBottom_Y.Text);
            BackGround_Coordinate[2] = Convert.ToDouble(this.textBox_BG_RightUp_X.Text);
            BackGround_Coordinate[3] = Convert.ToDouble(this.textBox_BG_RightUp_Y.Text);
            
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (_strongFlag)
            {
                this.button13.Text = "强化效果";
            }
            else
            {
                this.button13.Text = "关闭强化";
            }
            _strongFlag = !_strongFlag;
        }
        private void DrawNewCube(Cube_Info info, double len, double width, double height)//0718
        {
            Constant constant = new Constant();
            //  GL.PushMatrix();
            double _len = len / 2;
            double _width = width / 2;
            double _height = height / 2;
            //深度测试开启，实现遮挡关系
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            // GL.Enable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.DstAlpha);
            GL.Color4(0.0f, 0.0f, 0.5f, 0.5f);
            GL.Begin(BeginMode.Quads);
            //底面 
            GL.Vertex3(info.x - _len, info.y - _width, info.t - _height);//D
            GL.Vertex3(info.x + _len, info.y - _width, info.t - _height);//A
            GL.Vertex3(info.x + _len, info.y + _width, info.t - _height);//B
            GL.Vertex3(info.x - _len, info.y + _width, info.t - _height);//C
                                                                         //侧面A
            GL.Vertex3(info.x - _len, info.y - _width, info.t - _height);//D
            GL.Vertex3(info.x + _len, info.y - _width, info.t - _height);//A
            GL.Vertex3(info.x + _len, info.y - _width, info.t + _height);//E
            GL.Vertex3(info.x - _len, info.y - _width, info.t + _height);//H
                                                                         //侧面B
            GL.Vertex3(info.x + _len, info.y - _width, info.t - _height);//A
            GL.Vertex3(info.x + _len, info.y + _width, info.t - _height);//B
            GL.Vertex3(info.x + _len, info.y + _width, info.t + _height);//F
            GL.Vertex3(info.x + _len, info.y - _width, info.t + _height);//E
                                                                         //侧面C 
            GL.Vertex3(info.x + _len, info.y + _width, info.t - _height);//B
            GL.Vertex3(info.x - _len, info.y + _width, info.t - _height);//C
            GL.Vertex3(info.x - _len, info.y + _width, info.t + _height);//G
            GL.Vertex3(info.x + _len, info.y + _width, info.t + _height);//F
                                                                         //侧面D
            GL.Vertex3(info.x - _len, info.y + _width, info.t - _height);//C
            GL.Vertex3(info.x - _len, info.y - _width, info.t - _height);//D
            GL.Vertex3(info.x - _len, info.y - _width, info.t + _height);//H
            GL.Vertex3(info.x - _len, info.y + _width, info.t + _height);//G
                                                                         //顶面
            GL.Vertex3(info.x - _len, info.y - _width, info.t + _height);//H
            GL.Vertex3(info.x + _len, info.y - _width, info.t + _height);//E
            GL.Vertex3(info.x + _len, info.y + _width, info.t + _height);//F
            GL.Vertex3(info.x - _len, info.y + _width, info.t + _height);//G
            GL.End();

            GL.Disable(EnableCap.Blend);
        }


        //0301画包围圈
        private void DrawNewLoop(Cube_Info info, double len, double width, double height)//0718
        {
            Constant constant = new Constant();
            //  GL.PushMatrix();      
            //GL.Disable(EnableCap.LineStipple);禁用线条样式

            double _len = len / 2;
            double _width = width / 2;
            double _height = height / 2;
            // GL.Enable(EnableCap.Blend);//开启渲染
            GL.Enable(EnableCap.LineSmooth);//启用反走样
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.DstAlpha);
            GL.Color4(0.3f, 0f, 0.8f, 1.0f);
            // GL.Enable(EnableCap.LineStipple);
            GL.LineStipple(1, 0x24FF);//线条样式
            GL.LineWidth(3.0f);
            GL.Begin(BeginMode.LineLoop);
            {
                // front face
                //GL.Normal3(0, 0, 1);

                //底面 
                GL.Vertex3(info.x - _len, info.y - _width, info.t - _height);//D
                GL.Vertex3(info.x + _len, info.y - _width, info.t - _height);//A
                GL.Vertex3(info.x + _len, info.y + _width, info.t - _height);//B
                GL.Vertex3(info.x - _len, info.y + _width, info.t - _height);//C

            }
            GL.End();
            GL.Begin(BeginMode.LineLoop);
            {

                //侧面A
                GL.Vertex3(info.x - _len, info.y - _width, info.t - _height);//D
                GL.Vertex3(info.x + _len, info.y - _width, info.t - _height);//A
                GL.Vertex3(info.x + _len, info.y - _width, info.t + _height);//E
                GL.Vertex3(info.x - _len, info.y - _width, info.t + _height);//H
            }
            GL.End();
            GL.Begin(BeginMode.LineLoop);
            {
                //侧面B
                GL.Vertex3(info.x + _len, info.y - _width, info.t - _height);//A
                GL.Vertex3(info.x + _len, info.y + _width, info.t - _height);//B
                GL.Vertex3(info.x + _len, info.y + _width, info.t + _height);//F
                GL.Vertex3(info.x + _len, info.y - _width, info.t + _height);//E
            }
            GL.End();
            GL.Begin(BeginMode.LineLoop);
            {

                //侧面C 
                GL.Vertex3(info.x + _len, info.y + _width, info.t - _height);//B
                GL.Vertex3(info.x - _len, info.y + _width, info.t - _height);//C
                GL.Vertex3(info.x - _len, info.y + _width, info.t + _height);//G
                GL.Vertex3(info.x + _len, info.y + _width, info.t + _height);//F
            }
            GL.End();
            GL.Begin(BeginMode.LineLoop);
            {

                //侧面D
                GL.Vertex3(info.x - _len, info.y + _width, info.t - _height);//C
                GL.Vertex3(info.x - _len, info.y - _width, info.t - _height);//D
                GL.Vertex3(info.x - _len, info.y - _width, info.t + _height);//H
                GL.Vertex3(info.x - _len, info.y + _width, info.t + _height);//G

            }
            GL.End();
            GL.Begin(BeginMode.LineLoop);
            {

                //顶面
                GL.Vertex3(info.x - _len, info.y - _width, info.t + _height);//H
                GL.Vertex3(info.x + _len, info.y - _width, info.t + _height);//E
                GL.Vertex3(info.x + _len, info.y + _width, info.t + _height);//F
                GL.Vertex3(info.x - _len, info.y + _width, info.t + _height);//G

            }
            GL.End();
            GL.LineWidth(1.0f);
            GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Disable(EnableCap.Blend);
        }

        private void button14_Click(object sender, EventArgs e)
        {

        }
        public class Cube_Info
        {
            public double t;
            public double x;
            public double y;
            public Cube_Info(double t_, double x_, double y_)
            {
                t = t_;
                y = y_;
                x = x_;
            }
            public Cube_Info()
            {
                x = 0.0;
                y = 0.0;
                t = 0.0;
            }
        }



        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string path = Application.StartupPath;
            openFileDialog.InitialDirectory = path;
            openFileDialog.Filter = "access文件|*.mdb|txt文件|*.txt*|xlsx文件|*.xlsx|所有文件|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //0223,未操作读取的文件
            }
        }

        private void 背景图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string path = Application.StartupPath;
            openFileDialog.InitialDirectory = path + "\\pic";
            openFileDialog.Filter = "jpg文件|*.jpg|位图文件|*.bmp*|所有文件|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fName = openFileDialog.FileName;
                BackGroundImg = new Bitmap(fName);
                //  BackGroundImg.RotateFlip(RotateFlipType.RotateNoneFlipY);
                bmp_data = BackGroundImg.LockBits(new Rectangle(0, 0, BackGroundImg.Width, BackGroundImg.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                //bmp_data = BackGroundImg.LockBits(new Rectangle(0, 0, BackGroundImg.Width, BackGroundImg.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                BackGround_Init = true;
                if (glControlLoaded)
                    textureID = LoadTexture();
            }
        }

        private void 分析范围ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicInfo basicInfo = new BasicInfo();
            basicInfo.ShowDialog();
            // basicInfo.Show(this);
            // double start_t = Convert.ToDouble(basicInfo.textBox3);
            // double end_t = Convert.ToDouble(basicInfo.textBox4);
            start_t = basicInfo.textBox3.Text;
            end_t = basicInfo.textBox4.Text;
            //x和y轴起终点
            start_x = basicInfo.textBox6.Text;
            end_x = basicInfo.textBox5.Text;
            start_y = basicInfo.textBox8.Text;
            end_y = basicInfo.textBox7.Text;

            //交叉口标注
            w_length = basicInfo.textBox11.Text;
            w_width = basicInfo.textBox12.Text;
            s_length = basicInfo.textBox2.Text;
            s_width = basicInfo.textBox15.Text;
            n_length = basicInfo.textBox9.Text;
            n_width = basicInfo.textBox1.Text;
            e_length = basicInfo.textBox10.Text;
            e_width = basicInfo.textBox14.Text;
        }

        private void 设置颜色ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorSet colorSet = new ColorSet();
            colorSet.ShowDialog();
        }

        private void 设置颜色ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 强化效果ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

                if (_strongFlag)
                {

                    this.强化效果ToolStripMenuItem.Text = "强化效果";
                }
                else
                {
                    this.强化效果ToolStripMenuItem.Text = "关闭强化";

                }
                _strongFlag = !_strongFlag;
        }

        private void 俯视ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
                camera_alpha = -90;
                camera_fy = 0.1f;
                glControl.Invalidate();
          
        }

        private void 正视ToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
                // du = 90;
                //  h = -11.0f;
                camera_alpha = -90;
                camera_fy = 90;
                glControl.Invalidate();
           
        }

        private void 筛选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
       
        }

        private void 筛选ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.tabPage2.Parent = tabControl1;
            this.tabControl1.SelectedTab = tabPage2;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void 基本统计结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            double startTime = 0;
            double endTime = 0;
            double buttonX = 0;
            double topX = 0;
            double buttonY = 0;
            double topY = 0;
            double length = 0;
            try
            {
                startTime = Convert.ToDouble(start_t);
                endTime = Convert.ToDouble(end_t);
                buttonX = Convert.ToDouble(start_x);
                topX = Convert.ToDouble(end_x);
                buttonY = Convert.ToDouble(start_y);
                topY = Convert.ToDouble(end_y);
                length = (Convert.ToDouble(e_length) + Convert.ToDouble(s_length) + Convert.ToDouble(n_length) + Convert.ToDouble(w_length)) * 2;
            }
            catch (Exception ex)
            {
                MessageBox.Show("请检查您的输入");
                return;
            }

            if (startTime >= endTime)
            {
                MessageBox.Show("起始时间必须小于终止时间");
                return;
            }
            if (buttonX >= topX)
            {
                MessageBox.Show("X轴起始必须小于X轴终止");
                return;
            }
            if (buttonY >= topY)
            {
                MessageBox.Show("Y轴起始必须小于Y轴终止");
                return;
            }


            if (length < 0.001)
            {
                MessageBox.Show("请您输入正确的车道长度");
            }

            CRstSet rst = _citySmart.CSCompute_1(startTime, endTime, buttonX, topX, buttonY, topY, length);
            stats_car = PrivateRound(rst.carNum, 4).ToString();
            stats_v = PrivateRound(rst.avaMS, 4).ToString();
            stats_delay = PrivateRound(rst.avaTQDelay, 4).ToString();
            stats_queue = PrivateRound(rst.density * 1000, 4).ToString();

            BasicStats basicStats = new BasicStats();

            basicStats.textBox10.Text = stats_car;
            basicStats.textBox11.Text = stats_v;
            basicStats.textBox12.Text = stats_delay;
            basicStats.textBox13.Text = stats_queue;

            basicStats.ShowDialog();


        }

        private void button14_Click_1(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void 安全分析ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Safetychoice safetychoice = new Safetychoice();
            safetychoice.ShowDialog();
        }

        private void 效率分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Efficiency efficiency = new Efficiency();
            /*
            double startTime = 0;
            double endTime = 0;
            double buttonX = 0;
            double topX = 0;
            double buttonY = 0;
            double topY = 0;
            double length = 0;
            startTime = Convert.ToDouble(start_t);
            endTime = Convert.ToDouble(end_t);
            buttonX = Convert.ToDouble(start_x);
            topX = Convert.ToDouble(end_x);
            buttonY = Convert.ToDouble(start_y);
            topY = Convert.ToDouble(end_y);

           
            double elength = Convert.ToDouble(e_length);
            double ewidth = Convert.ToDouble(e_width);
            double slength = Convert.ToDouble(s_length);
            double swidth = Convert.ToDouble(s_width);
            double nlength = Convert.ToDouble(n_length);
            double nwidth = Convert.ToDouble(n_width);
            double wlength = Convert.ToDouble(w_length);
            double wwidth = Convert.ToDouble(w_width);
            length = (elength + slength + nlength + wlength) / 4;

            double tstep = Convert.ToDouble(efficiency.textBox8.Text);

            double space = elength * ewidth + wlength * wwidth + nlength * nwidth + slength * swidth + wwidth * ewidth * swidth * nwidth;
            double _totalvolume = (endTime - startTime) * space;

            CRstSet rst = _citySmart.CSCompute_Efficiency(startTime, endTime, buttonX, topX, buttonY, topY, tstep, _totalvolume);
            stats_car = PrivateRound(rst.carNum, 4).ToString();
            stats_v = PrivateRound(rst.avaMS, 4).ToString();
            stats_delay = PrivateRound(rst.avaTQDelay, 4).ToString();
            stats_queue = PrivateRound(rst.density * 1000, 4).ToString();

            efficiency.textBox10.Text = Convert.ToString(rst.utilization*100);

            double carnumber = Convert.ToDouble(stats_car);
            efficiency.textBox9.Text = Convert.ToString(rst.utilization / carnumber * 100);
*/
            efficiency.ShowDialog();

        }

        private void 设置坐标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.tabPage1.Parent = tabControl1;
           this.tabControl1.SelectedTab = tabPage1;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (_strongFlag)
            {
                accuracy_x = "0";
                accuracy_y = "0";
                accuracy_t = "0";

                start_t = "0";
                end_t = "0";

                this.button3.Text = "开启网格";


            }
            else
            {
                accuracy_x = this.textBox20.Text;
                accuracy_y = this.textBox17.Text;
                accuracy_t = this.textBox16.Text;

                start_t = this.textBox18.Text;
                end_t = this.textBox19.Text;
                this.button3.Text = "关闭网格";

            }
            _strongFlag = !_strongFlag;
        }

        private void 安全分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Safetychoice safetychoice = new Safetychoice();
            safetychoice.ShowDialog();

        }

        private void 侧视ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        
            //du = 0;
            //h = -10.0f;
            camera_alpha = -180;
            camera_fy = 90;
             glControl.Invalidate(); 
        }


        /*
        private void button14_Click_1(object sender, EventArgs e)
        {
            double startTime = 0;
            double endTime = 0;
            double buttonX = 0;
            double topX = 0;
            double buttonY = 0;
            double topY = 0;
            double length = 0;


            try
            {
                startTime = Convert.ToDouble(this.textBox3.Text);
                endTime = Convert.ToDouble(this.textBox4.Text);
                buttonX = Convert.ToDouble(this.textBox6.Text);
                topX = Convert.ToDouble(this.textBox5.Text);
                buttonY = Convert.ToDouble(this.textBox8.Text);
                topY = Convert.ToDouble(this.textBox7.Text);
                length = Convert.ToDouble(this.textBox15.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("请检查您的输入");
                return;
            }

            if (startTime >= endTime)
            {
                MessageBox.Show("起始时间必须小于终止时间");
                return;
            }
            if (buttonX >= topX)
            {
                MessageBox.Show("X轴起始必须小于X轴终止");
                return;
            }
            if (buttonY >= topY)
            {
                MessageBox.Show("Y轴起始必须小于Y轴终止");
                return;
            }


            if (length < 0.001)
            {
                MessageBox.Show("请您输入正确的车道长度");
            }
 

            CRstSet rst = _citySmart.CSCompute_1(startTime, endTime, buttonX, topX, buttonY, topY, length);
            

            this.textBox10.Text = PrivateRound(rst.carNum, 4).ToString();
            this.textBox11.Text = PrivateRound(rst.avaMS, 4).ToString(); 
            this.textBox12.Text = PrivateRound(rst.avaTQDelay, 4).ToString(); 
            this.textBox13.Text = PrivateRound(rst.density*1000, 4).ToString();

        }

    */
        private double PrivateRound(double v_, int length_)
        {
            if (v_ % 1 < 0.0001)
            {
                v_ = Convert.ToInt32( v_);
            }

            string tmp = v_.ToString();
            string[] array = tmp.Split('.');
            if (array.Length == 1)
            {
                return v_;
            }
            else
            {
                if (array[1].Length > length_)
                {
                    char[] a =  array[1].Substring(0, length_).ToArray();
                    foreach (char character in a)
                    {
                        if (character != '0')
                        {
                            return Convert.ToDouble((array[0] + "." + array[1].Substring(0, length_)));
                        }
                    }
                    return Convert.ToDouble((array[0]));
                }
                else
                {
                    return v_;
                }
            }
        }








    }
}
