using System;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using System.Data.OleDb;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace CitySmart
{   

    public class CitySmart
    {
        public bool _cutFlag;
        public Point _centralPoint;
        public List<double> _routDecs;//已废弃,由_vehicleRoute取代
        public List<double> _vehcTypes;
        public List<double> _links;
        public List<double> _lanes;//已废弃,由_vehicleRoute取代
        public List<VehicleRoute> _vehicleRoutes;
        public double _startTime;
        public double _endTime;        
        public List<double> _vicNrs;
        public List<Surface> _surfaceSet; //所有经过筛选后的记录
        public List<int> _colorSet; //所有经过筛选后的记录中各种颜色所包含的记录数目
        public List<int> _unitSet; //所有经过筛选后的记录中各车所包含的记录数目
        public double _maxheight;
        public Point[] _cutPoint;
        public List<Point> _cutPoints = new List<Point>(); //保存所有切割信息
        public List<VehcShape> _vecShapes; //用于保存所有显示车辆的形状信息



        private EasyAccess _easyAccess = new EasyAccess();
        private Point _vector;
        private Dictionary<double, VehcShape> _vehcShape;
        private bool _fistFlag = true;
        private Surface _lastSurface = new Surface();

        //计算资源
        private List<double> _cVicNrs;
        

        public CitySmart()
        {
            _vehcShape = new Dictionary<double, VehcShape>();
            _centralPoint = new Point(0, 0, 0);
            _routDecs = new List<double>();   //已废弃,由_vehicleRoute取代
            _vehcTypes = new List<double>();
            _links = new List<double>();
            _lanes = new List<double>();  //已废弃,由_vehicleRoute取代
            _vehicleRoutes = new List<VehicleRoute>();
            _cutPoint = new Point[4];
            
            _vicNrs = new List<double>();
            _surfaceSet = new List<Surface>();
            _colorSet = new List<int>();
            _unitSet = new List<int>();
            _cutFlag = false;

            _vecShapes = new List<VehcShape>();

            _cVicNrs = new List<double>();
            
            _maxheight = 100;
            _vector = new Point();
        }

        public void ReflashInfo()
        {
            _surfaceSet = new List<Surface>();
            _colorSet = new List<int>();
            _unitSet = new List<int>();
            _vector = new Point();
            _vecShapes = new List<VehcShape>();

            List<Surface> tmpContainer1 = new List<Surface>();
            List<Surface> tmpContainer2 = new List<Surface>();
            List<Surface> tmpContainer3 = new List<Surface>();
            List<Surface> tmpContainer4 = new List<Surface>();
            List<Surface> tmpContainer5 = new List<Surface>();
            List<Surface> tmpContainer6 = new List<Surface>();
            List<Surface> tmpContainer7 = new List<Surface>();
            List<Surface> tmpContainer8 = new List<Surface>();
            List<Surface> tmpContainer9 = new List<Surface>();
            List<Surface> tmpContainer10 = new List<Surface>();
            List<Surface> tmpContainer11 = new List<Surface>();
            List<Surface> tmpContainer12 = new List<Surface>();            

            List<int> tmpUnitC1 = new List<int>();
            List<int> tmpUnitC2 = new List<int>();
            List<int> tmpUnitC3 = new List<int>();
            List<int> tmpUnitC4 = new List<int>();
            List<int> tmpUnitC5 = new List<int>();
            List<int> tmpUnitC6 = new List<int>();
            List<int> tmpUnitC7 = new List<int>();
            List<int> tmpUnitC8 = new List<int>();
            List<int> tmpUnitC9 = new List<int>();
            List<int> tmpUnitC10 = new List<int>();
            List<int> tmpUnitC11 = new List<int>();
            List<int> tmpUnitC12 = new List<int>();

            foreach (double vehNr in GetVicNrs())
            {
                int i = 0;                
                int color = 0;
                bool flag = true;
                Point preLocation = new Point();
                foreach (VehcileInfo info in GetVehcInfo(vehNr))
                {
                    i++;
                    if (flag)
                    {
                        color = Convert.ToInt32((info.routDec - 1) * 3 + info.route); 
                        preLocation = GenerateFirstLocation(info);
                        _vecShapes.Add(GetVehcShape(info.type));
                        flag = false;
                    }
                    

                    switch (color)
                    {
                        case 1:
                            {                                 
                                tmpContainer1.Add(GetSurface(info,ref preLocation));
                                break;
                            }
                        case 2:
                            {
                                tmpContainer2.Add(GetSurface(info, ref preLocation));
                                break;
                            }
                        case 3:
                            {
                                tmpContainer3.Add(GetSurface(info, ref preLocation));
                                break;
                            }
                        case 4:
                            {
                                tmpContainer4.Add(GetSurface(info, ref preLocation));
                                break;
                            }
                        case 5:
                            {
                                tmpContainer5.Add(GetSurface(info, ref preLocation));
                                break;
                            }
                        case 6:
                            {
                                tmpContainer6.Add(GetSurface(info, ref preLocation));
                                break;
                            }
                        case 7:
                            {
                                tmpContainer7.Add(GetSurface(info, ref preLocation));
                                break;
                            }
                        case 8:
                            {
                                tmpContainer8.Add(GetSurface(info, ref preLocation));
                                break;
                            }
                        case 9:
                            {
                                tmpContainer9.Add(GetSurface(info, ref preLocation));
                                break;
                            }
                        case 10:
                            {
                                tmpContainer10.Add(GetSurface(info, ref preLocation));
                                break;
                            }
                        case 11:
                            {
                                tmpContainer11.Add(GetSurface(info, ref preLocation));
                                break;
                            }
                        case 12:
                            {
                                tmpContainer12.Add(GetSurface(info, ref preLocation));
                                break;
                            }
                    }
                }
                switch (color)
                {
                    case 1:
                        {
                            tmpUnitC1.Add(i);
                            break;
                        }
                    case 2:
                        {
                            tmpUnitC2.Add(i);
                            break;
                        }
                    case 3:
                        {
                            tmpUnitC3.Add(i);
                            break;
                        }
                    case 4:
                        {
                            tmpUnitC4.Add(i);
                            break;
                        }
                    case 5:
                        {
                            tmpUnitC5.Add(i);
                            break;
                        }
                    case 6:
                        {
                            tmpUnitC6.Add(i);
                            break;
                        }
                    case 7:
                        {
                            tmpUnitC7.Add(i);
                            break;
                        }
                    case 8:
                        {
                            tmpUnitC8.Add(i);
                            break;
                        }
                    case 9:
                        {
                            tmpUnitC9.Add(i);
                            break;
                        }
                    case 10:
                        {
                            tmpUnitC10.Add(i);
                            break;
                        }
                    case 11:
                        {
                            tmpUnitC11.Add(i);
                            break;
                        }
                    case 12:
                        {
                            tmpUnitC12.Add(i);
                            break;
                        }
                }
            }

            _colorSet.Add(tmpContainer1.Count);
            _colorSet.Add(tmpContainer2.Count);
            _colorSet.Add(tmpContainer3.Count);
            _colorSet.Add(tmpContainer4.Count);
            _colorSet.Add(tmpContainer5.Count);
            _colorSet.Add(tmpContainer6.Count);
            _colorSet.Add(tmpContainer7.Count);
            _colorSet.Add(tmpContainer8.Count);
            _colorSet.Add(tmpContainer9.Count);
            _colorSet.Add(tmpContainer10.Count);
            _colorSet.Add(tmpContainer11.Count);
            _colorSet.Add(tmpContainer12.Count);

            _surfaceSet.AddRange(tmpContainer1);
            _surfaceSet.AddRange(tmpContainer2);
            _surfaceSet.AddRange(tmpContainer3);
            _surfaceSet.AddRange(tmpContainer4);
            _surfaceSet.AddRange(tmpContainer5);
            _surfaceSet.AddRange(tmpContainer6);
            _surfaceSet.AddRange(tmpContainer7);
            _surfaceSet.AddRange(tmpContainer8);
            _surfaceSet.AddRange(tmpContainer9);
            _surfaceSet.AddRange(tmpContainer10);
            _surfaceSet.AddRange(tmpContainer11);
            _surfaceSet.AddRange(tmpContainer12);

            _unitSet.AddRange(tmpUnitC1);
            _unitSet.AddRange(tmpUnitC2);
            _unitSet.AddRange(tmpUnitC3);
            _unitSet.AddRange(tmpUnitC4);
            _unitSet.AddRange(tmpUnitC5);
            _unitSet.AddRange(tmpUnitC6);
            _unitSet.AddRange(tmpUnitC7);
            _unitSet.AddRange(tmpUnitC8);
            _unitSet.AddRange(tmpUnitC9);
            _unitSet.AddRange(tmpUnitC10);
            _unitSet.AddRange(tmpUnitC11);
            _unitSet.AddRange(tmpUnitC12);

        }

        public List<VehcileInfo> GetVehcInfo(double vehNr_)
        {
            List<VehcileInfo> lVehcInfo = new List<VehcileInfo>();

            

            string SQL = "select * from data530_VEH_RECORD where 1=1 ";
            SQL += GenerateQuaSQL_VEHICLE_DEC().Length != 0 ? " and " + GenerateQuaSQL_VEHICLE_DEC() + " " : " ";
            SQL += GenerateQuaSQL(1).Length != 0 ? " and " + GenerateQuaSQL(1) + " " : " ";
            SQL += GenerateQuaSQL(2).Length != 0 ? " and " + GenerateQuaSQL(2) + " " : " ";
            SQL += GenerateQuaSQL(3).Length != 0 ? " and " + GenerateQuaSQL(3) + " " : " ";

            SQL += " and t < ";
            SQL += _endTime.ToString();
            SQL += " and t >= ";
            SQL += _startTime.ToString();
            SQL += " and VehNr = ";
            SQL += vehNr_.ToString();

            OleDbDataReader dataReader = _easyAccess.ExecuteDataReader(SQL);
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    VehcileInfo info = new VehcileInfo();
                    info.vehNr = Convert.ToDouble(dataReader.GetValue(Constant.VEH_NR).ToString());
                    info.type = Convert.ToDouble(dataReader.GetValue(Constant.TYPE).ToString());
                    info.vehTypeName = dataReader.GetValue(Constant.VEH_TYPE_NAME).ToString();
                    info.worldX = Convert.ToDouble(dataReader.GetValue(Constant.WORLD_X).ToString());
                    info.worldY = Convert.ToDouble(dataReader.GetValue(Constant.WORLD_Y).ToString());
                    info.t = Convert.ToDouble(dataReader.GetValue(Constant.T).ToString());
                    info.vMS = Convert.ToDouble(dataReader.GetValue(Constant.V_MS).ToString());
                    info.link = Convert.ToDouble(dataReader.GetValue(Constant.LINK).ToString());
                    info.lane = Convert.ToDouble(dataReader.GetValue(Constant.LANE).ToString());
                    info.tQDelay = Convert.ToDouble(dataReader.GetValue(Constant.TQ_DELAY).ToString());
                    info.route = Convert.ToDouble(dataReader.GetValue(Constant.ROUTE).ToString());
                    info.routDec = Convert.ToDouble(dataReader.GetValue(Constant.ROUTE_DEC).ToString());
                    if (IsRemain(info))
                    {
                        lVehcInfo.Add(info);
                    }
                }
            }
            return lVehcInfo;
        }       

        public void ReflashVicNrs()
        {
            //EasyAccess easyAccess = new EasyAccess();
            _vicNrs.Clear();

            string tmp = "17";

            string SQL = "select distinct(VehNr) from data530_VEH_RECORD where 1=1 ";
            SQL += GenerateQuaSQL_VEHICLE_DEC().Length != 0 ? " and " + GenerateQuaSQL_VEHICLE_DEC() + " " : " ";            
            SQL += GenerateQuaSQL(1).Length != 0 ? " and " + GenerateQuaSQL(1) + " " : " ";
            SQL += GenerateQuaSQL(2).Length != 0 ? " and " + GenerateQuaSQL(2) + " " : " ";
            SQL += GenerateQuaSQL(3).Length != 0 ? " and " + GenerateQuaSQL(3) + " " : " ";
            SQL += " and t < ";
            SQL += _endTime.ToString();
            SQL += " and t >= ";
            SQL += _startTime.ToString();
            //SQL += " and VehNr = " + tmp;
            //SQL += " group by VehNr";

            OleDbDataReader dataReader = _easyAccess.ExecuteDataReader(SQL);
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    _vicNrs.Add(Convert.ToDouble(dataReader.GetValue(0).ToString()));
                }
            }
        }

        public List<double> GetVicNrs()
        {
            return _vicNrs;
        }


        public void DrawCube()
        {
            GL.PushMatrix();
            {
                GL.Rotate(10, 1, 0, 0);
                GL.Rotate(10, 0, 1, 0);

                GL.Color3(0.0, 0.5, 1.0);


                // *** SET THIS TO FALSE TO CHANGE TO GLUTSOLIDCUBE ***
                bool drawQuads = true;

                if (drawQuads)
                {
                    GL.Begin(BeginMode.Quads);
                    {
                        // front face
                        GL.Normal3(0, 0, 1);
                        GL.Vertex3(-0.5, -0.5, 0.5);
                        GL.Vertex3(0.5, -0.5, 0.5);
                        GL.Vertex3(0.5, 0.5, 0.5);
                        GL.Vertex3(-0.5, 0.5, 0.5);

                        GL.Vertex3(-5, -5, 5);
                        GL.Vertex3(5, -5, 5);
                        GL.Vertex3(5, 5, 5);
                        GL.Vertex3(-5, 5, 5);

                        // back face
                        GL.Normal3(0, 0, -1);
                        GL.Vertex3(-0.5, -0.5, -0.5);
                        GL.Vertex3(-0.5, 0.5, -0.5);
                        GL.Vertex3(0.5, 0.5, -0.5);
                        GL.Vertex3(0.5, -0.5, -0.5);

                        // top face
                        GL.Normal3(0, 1, 0);
                        GL.Vertex3(-0.5, 0.5, -0.5);
                        GL.Vertex3(-0.5, 0.5, 0.5);
                        GL.Vertex3(0.5, 0.5, 0.5);
                        GL.Vertex3(0.5, 0.5, -0.5);

                        // bottom face
                        GL.Normal3(0, -1, 0);
                        GL.Vertex3(-0.5, -0.5, -0.5);
                        GL.Vertex3(0.5, -0.5, -0.5);
                        GL.Vertex3(0.5, -0.5, 0.5);
                        GL.Vertex3(-0.5, -0.5, 0.5);

                        // right face
                        GL.Normal3(1, 0, 0);
                        GL.Vertex3(0.5, -0.5, -0.5);
                        GL.Vertex3(0.5, 0.5, -0.5);
                        GL.Vertex3(0.5, 0.5, 0.5);
                        GL.Vertex3(0.5, -0.5, 0.5);

                        // left face
                        GL.Normal3(-1, 0, 0);
                        GL.Vertex3(-0.5, -0.5, -0.5);
                        GL.Vertex3(-0.5, -0.5, 0.5);
                        GL.Vertex3(-0.5, 0.5, 0.5);
                        GL.Vertex3(-0.5, 0.5, -0.5);
                    }
                    GL.End();
                }
            }
            GL.PopMatrix(); //弹出变换矩阵
            GL.Flush();

        }


        public CRstSet CSCompute(double startTime_, double endTime_, double buttonX_, double topX_, double buttonY_, double topY_, double realSize_)
        {
            double totalVolume = realSize_ * (endTime_ - startTime_);
            double vehicleVolume = 0;

            //刷新所有时空内的车辆
            RefreshCVicNrs(startTime_, endTime_,  buttonX_,  topX_,  buttonY_,  topY_);

            //计算单条链的车辆时空体积
            foreach (double vicNur in _cVicNrs)
            {
                vehicleVolume += ComputeVolume(vicNur, startTime_, endTime_, buttonX_, topX_, buttonY_, topY_);
            }

            if (totalVolume < 0.01)
            {
                return new CRstSet(_cVicNrs.Count, 1);
            }
            else
            {
                return new CRstSet(_cVicNrs.Count, vehicleVolume/totalVolume);
            }
        }

        public CRstSet CSCompute_Efficiency(double startTime_, double endTime_, double buttonX_, double topX_, double buttonY_, double topY_, double tstep, double _totalvolume)
        {
            double totalVolume = _totalvolume;
            double step = tstep;
            double st1=0 ;
            double vx1 ;
            double vy1 ;
            double vms1 ;

            //刷新所有时空内的车辆
            RefreshCVicNrs(startTime_, endTime_, buttonX_, topX_, buttonY_, topY_);

            //计算单条链的车辆时空体积
            //以下新建
            string SQL = "select * from data530_VEH_RECORD where 1=1 ";
            SQL += GenerateQuaSQL_VEHICLE_DEC().Length != 0 ? " and " + GenerateQuaSQL_VEHICLE_DEC() + " " : " ";
            SQL += GenerateQuaSQL(1).Length != 0 ? " and " + GenerateQuaSQL(1) + " " : " ";
            SQL += GenerateQuaSQL(2).Length != 0 ? " and " + GenerateQuaSQL(2) + " " : " ";
            SQL += GenerateQuaSQL(3).Length != 0 ? " and " + GenerateQuaSQL(3) + " " : " ";
            SQL += " and t < ";
            SQL += endTime_.ToString();
            SQL += " and t >= ";
            SQL += startTime_.ToString();
            SQL += " and WorldX <= " + topX_.ToString();
            SQL += " and WorldX > " + buttonX_.ToString();
            SQL += " and WorldY <= " + topY_.ToString();
            SQL += " and WorldY > " + buttonY_.ToString();

            OleDbDataReader dataReader = _easyAccess.ExecuteDataReader(SQL);

            int i = 0;
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    vx1 = Math.Abs(Convert.ToDouble(dataReader.GetValue(Constant.Vx).ToString()));
                    vy1 = Math.Abs(Convert.ToDouble(dataReader.GetValue(Constant.Vy).ToString()));
                    vms1 = Convert.ToDouble(dataReader.GetValue(Constant.V_MS).ToString());
                    //时空利用率计算

                    if (vms1 >= 13.89)
                    {
                        st1 += ((3 + vx1 * step) * (4 + vy1 * step) - 3 * 4 + 3.5 * 15.28) * step;
                    }
                    else if (11.11 < vms1 && vms1 < 13.89)
                    {
                        st1 += ((3 + vx1 * step) * (4 + vy1 * step) - 3 * 4 + 3.5 * 12.5) * step;
                    }
                    else if (8.33 < vms1 && vms1 < 11.11)
                    {
                        st1 += ((3 + vx1 * step) * (4 + vy1 * step) - 3 * 4 + 3.5 * 9.72) * step;
                    }
                    else if (5.56 < vms1 && vms1 < 8.33)
                    {
                        st1 += ((3 + vx1 * step) * (4 + vy1 * step) - 3 * 4 + 3.5 * 6.95) * step;
                    }
                    else if (2.78 < vms1 && vms1 < 5.56)
                    {
                        st1 += ((3 + vx1 * step) * (4 + vy1 * step) - 3 * 4 + 3.5 * 4.17) * step;
                    }
                    else if (0 < vms1 && vms1 < 2.78)
                    {
                        st1 += ((3 + vx1 * step) * (4 + vy1 * step) - 3 * 4 + 3.5 * 1.39) * step;
                    }
                    i++;
                }
            }
            double STU = st1 + _cVicNrs.Count * 3 * 4 * step;
            double STE = STU / totalVolume;
            return new CRstSet(_cVicNrs.Count, STE);
            // CRstSet rst = new CRstSet(_cVicNrs.Count, 1);
            // rst.utilization = streal;
            // return rst;
        }


        public CRstSet CSCompute_1(double startTime_, double endTime_, double buttonX_, double topX_, double buttonY_, double topY_, double length_)
        {

            double ms = 0;
            double TQDelay = 0;

            //刷新所有时空内的车辆
            RefreshCVicNrs(startTime_, endTime_, buttonX_, topX_, buttonY_, topY_);        

            string SQL = "select * from data530_VEH_RECORD where 1=1 ";
            SQL += GenerateQuaSQL_VEHICLE_DEC().Length != 0 ? " and " + GenerateQuaSQL_VEHICLE_DEC() + " " : " ";
            SQL += GenerateQuaSQL(1).Length != 0 ? " and " + GenerateQuaSQL(1) + " " : " ";
            SQL += GenerateQuaSQL(2).Length != 0 ? " and " + GenerateQuaSQL(2) + " " : " ";
            SQL += GenerateQuaSQL(3).Length != 0 ? " and " + GenerateQuaSQL(3) + " " : " ";
            SQL += " and t < ";
            SQL += endTime_.ToString();
            SQL += " and t >= ";
            SQL += startTime_.ToString();
            SQL += " and WorldX <= " + topX_.ToString();
            SQL += " and WorldX > " + buttonX_.ToString();
            SQL += " and WorldY <= " + topY_.ToString();
            SQL += " and WorldY > " + buttonY_.ToString();

            OleDbDataReader dataReader = _easyAccess.ExecuteDataReader(SQL);

            int i = 0;
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    ms += Convert.ToDouble(dataReader.GetValue(Constant.V_MS).ToString());
                    TQDelay += Convert.ToDouble(dataReader.GetValue(Constant.TQ_DELAY).ToString());
                    i++;
                }
            }

           CRstSet rst = new CRstSet(_cVicNrs.Count,0);
           rst.avaMS = ms / i;
           rst.avaTQDelay = TQDelay / i;
           rst.density = _cVicNrs.Count / length_;
           return rst;
        }


        public void CleanDB()
        {
            List<string> vicNrs = new List<string>();
            string SQL = "select distinct(VehNr) from data530_VEH_RECORD where 1=1 ";                       

            OleDbDataReader dataReader = _easyAccess.ExecuteDataReader(SQL);
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    vicNrs.Add(dataReader.GetValue(0).ToString());
                }
            }
            foreach (string vicNr in vicNrs)
            {
                CleanDB(vicNr);
            }

        }



 /*================================ private functions ====================================*/
        #region private functions

        private void CleanDB(string vicNr_)
        {
            string SQL = "select * from data530_VEH_RECORD where Route <> 0 and RoutDec <> 0 and VehNr = " + vicNr_;

            OleDbDataReader dataReader = _easyAccess.ExecuteDataReader(SQL);
            bool flag = false;
            string route = "";
            string routDec = "";
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    route = dataReader.GetValue(Constant.ROUTE).ToString();
                    routDec = dataReader.GetValue(Constant.ROUTE_DEC).ToString();
                    flag = true;
                }
            }

            if (flag)
            {
                SQL = "update data530_VEH_RECORD  set Route=" + route + ",RoutDec=" + routDec + " where VehNr =" + vicNr_;
            }
            else
            {
                SQL = "delete from data530_VEH_RECORD  where VehNr =" + vicNr_;
            }
            _easyAccess.ExecuteNoneQuery(SQL);
        }

        private double ComputeVolume(double vicNur_, double startTime_, double endTime_, double buttonX_, double topX_, double buttonY_, double topY_)
        {
            Point innerButton = new Point();
            Point innerTop = new Point();            
            Point outerButton = new Point();
            Point outerTop = new Point();

            bool haveOuterTop = false;
            bool haveOuterButton = false;

            double vecType = 0;

            //获得内边界点
            string SQL = "select * from data530_VEH_RECORD where 1=1 ";
            SQL += GenerateQuaSQL_VEHICLE_DEC().Length != 0 ? " and " + GenerateQuaSQL_VEHICLE_DEC() + " " : " ";
            SQL += GenerateQuaSQL(1).Length != 0 ? " and " + GenerateQuaSQL(1) + " " : " ";
            SQL += GenerateQuaSQL(2).Length != 0 ? " and " + GenerateQuaSQL(2) + " " : " ";
            SQL += " and t < ";
            SQL += endTime_.ToString();
            SQL += " and t >= ";
            SQL += startTime_.ToString();
            SQL += " and WorldX <= " + topX_.ToString();
            SQL += " and WorldX > " + buttonX_.ToString();
            SQL += " and WorldY <= " + topY_.ToString();
            SQL += " and WorldY > " + buttonY_.ToString();
            SQL += " and vehNr = " + vicNur_.ToString() +  " order by t";

            OleDbDataReader dataReader = _easyAccess.ExecuteDataReader(SQL);
            
            if (dataReader.HasRows)
            {
                bool flag = true;
                while (dataReader.Read())
                {
                    if (flag)
                    {
                        innerButton.x = Convert.ToDouble(dataReader.GetValue(Constant.WORLD_X).ToString());
                        innerButton.y = Convert.ToDouble(dataReader.GetValue(Constant.WORLD_Y).ToString());
                        innerButton.z = Convert.ToDouble(dataReader.GetValue(Constant.T).ToString());                        

                        innerTop.x = innerButton.x;
                        innerTop.y = innerButton.y;
                        innerTop.z = innerButton.z;

                        vecType = Convert.ToDouble(dataReader.GetValue(Constant.TYPE).ToString());
                        flag = false;
                    }
                    else
                    {
                        innerTop.x = Convert.ToDouble(dataReader.GetValue(Constant.WORLD_X).ToString());
                        innerTop.y = Convert.ToDouble(dataReader.GetValue(Constant.WORLD_Y).ToString());
                        innerTop.z = Convert.ToDouble(dataReader.GetValue(Constant.T).ToString());                        
                    }                    
                }
            }    
        
            //获得上外边界点
            SQL = "select * from data530_VEH_RECORD where 1=1 ";
            SQL += GenerateQuaSQL_VEHICLE_DEC().Length != 0 ? " and " + GenerateQuaSQL_VEHICLE_DEC() + " " : " ";
            SQL += GenerateQuaSQL(1).Length != 0 ? " and " + GenerateQuaSQL(1) + " " : " ";
            SQL += GenerateQuaSQL(2).Length != 0 ? " and " + GenerateQuaSQL(2) + " " : " ";
            SQL += GenerateQuaSQL(3).Length != 0 ? " and " + GenerateQuaSQL(3) + " " : " ";
            SQL += " and t > ";
            SQL += innerTop.z.ToString();
            SQL += " and vehNr = " + vicNur_.ToString() + " order by t";

            dataReader = _easyAccess.ExecuteDataReader(SQL);

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    outerTop.x = Convert.ToDouble(dataReader.GetValue(Constant.WORLD_X).ToString());
                    outerTop.y = Convert.ToDouble(dataReader.GetValue(Constant.WORLD_Y).ToString());
                    outerTop.z = Convert.ToDouble(dataReader.GetValue(Constant.T).ToString());
                    haveOuterTop = true;
                    break;
                }
            }   
 
            //获得下外边界点
            SQL = "select * from data530_VEH_RECORD where 1=1 ";
            SQL += GenerateQuaSQL_VEHICLE_DEC().Length != 0 ? " and " + GenerateQuaSQL_VEHICLE_DEC() + " " : " ";
            SQL += GenerateQuaSQL(1).Length != 0 ? " and " + GenerateQuaSQL(1) + " " : " ";
            SQL += GenerateQuaSQL(2).Length != 0 ? " and " + GenerateQuaSQL(2) + " " : " ";
            SQL += GenerateQuaSQL(3).Length != 0 ? " and " + GenerateQuaSQL(3) + " " : " ";
            SQL += " and t < ";
            SQL += innerButton.z.ToString();
            SQL += " and vehNr = " + vicNur_.ToString() + " order by t desc";

            dataReader = _easyAccess.ExecuteDataReader(SQL);

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    outerButton.x = Convert.ToDouble(dataReader.GetValue(Constant.WORLD_X).ToString());
                    outerButton.y = Convert.ToDouble(dataReader.GetValue(Constant.WORLD_Y).ToString());
                    outerButton.z = Convert.ToDouble(dataReader.GetValue(Constant.T).ToString());
                    haveOuterButton= true;
                    break;
                }
            }

            //计算顶部离体时间
            double Li = 0;
            double Lio = 0;
            double T = 0;
            double Tio = 0;

            double Tty = -1;
            double Tby = -1;
            double Ttx = -1;
            double Tbx = -1;

            if ((outerTop.x < topX_ && outerTop.x > buttonX_ && outerTop.y < topY_ && outerTop.y > buttonY_) || !haveOuterTop)
            {
                T = endTime_ - innerTop.z;
            }
            else
            {
                Li = topY_ - innerTop.y;
                Lio = outerTop.y - innerTop.y;
                Tio = outerTop.z - innerTop.z;
                if (Lio > 0.01)
                {
                    Tty = Li * Tio / Lio;
                }

                Li = innerTop.y - buttonY_;
                Lio = innerTop.y - outerTop.y;
                Tio = outerTop.z - innerTop.z;
                if (Lio > 0.01)
                {
                    Tby = Li * Tio / Lio;
                }

                Li = topX_ - innerTop.x;
                Lio = outerTop.x - innerTop.x;
                Tio = outerTop.z - innerTop.z;
                if (Lio > 0.01)
                {
                    Ttx = Li * Tio / Lio;
                }

                Li = innerTop.x - buttonX_;
                Lio = innerTop.x - outerTop.x;
                Tio = outerTop.z - innerTop.z;
                if (Lio > 0.01)
                {
                    Tbx = Li * Tio / Lio;
                }

                if (Ttx > 0)
                {
                    T = Ttx;
                }
                else if (Tbx > 0)
                {
                    T = Tbx;                
                }
                else if (Tty > 0)
                {
                    T = Tty;
                }
                else if (Tby > 0)
                {
                    T = Tby;
                }

                if (Tbx < T && Tbx > 0)
                {
                    T = Tbx;
                }
                if (Tby < T && Tby > 0)
                {
                    T = Tby;
                }
                if (Ttx < T && Ttx > 0)
                {
                    T = Ttx;
                }
                if (Tty < T && Tty > 0)
                {
                    T = Tty;
                }
            }
            double Tt = innerTop.z + T;
            if (Tt > endTime_)
            {
                Tt = endTime_;
            }

            //获取底部离体时间
            if ((outerButton.x < topX_ && outerButton.x > buttonX_ && outerButton.y < topY_ && outerButton.y > buttonY_) || !haveOuterButton)
            {
                T = innerButton.z - startTime_;
            }
            else
            {
                Li = topY_ - innerButton.y;
                Lio = outerButton.y - innerButton.y;
                Tio = innerButton.z - outerButton.z;
                if (Lio > 0.01)
                {
                    Tty = Li * Tio / Lio;
                }

                Li = innerButton.y - buttonY_;
                Lio = innerButton.y - outerButton.y;
                
                if (Lio > 0.01)
                {
                    Tby = Li * Tio / Lio;
                }

                Li = topX_ - innerButton.x;
                Lio = outerButton.x - innerButton.x;

                if (Lio > 0.01)
                {
                    Ttx = Li * Tio / Lio;
                }

                Li = innerButton.x - buttonX_;
                Lio = innerButton.x - outerButton.x;

                if (Lio > 0.01)
                {
                    Tbx = Li * Tio / Lio;
                }

                if (Ttx > 0)
                {
                    T = Ttx;
                }
                else if (Tbx > 0)
                {
                    T = Tbx;
                }
                else if (Tty > 0)
                {
                    T = Tty;
                }
                else if (Tby > 0)
                {
                    T = Tby;
                }

                if (Tbx < T && Tbx > 0)
                {
                    T = Tbx;
                }
                if (Tby < T && Tby > 0)
                {
                    T = Tby;
                }
                if (Ttx < T && Ttx > 0)
                {
                    T = Ttx;
                }
                if (Tty < T && Tty > 0)
                {
                    T = Tty;
                }
            }
            double Tb = innerButton.z - T;
            if (Tb < startTime_)
            {
                Tb = startTime_;
            }

            VehcShape shape = GetVehcShape(vecType);

            return (Tt - Tb) * shape.length * shape.width;
        }



        private void RefreshCVicNrs(double startTime_, double endTime_, double buttonX_, double topX_, double buttonY_, double topY_)
        {
            _cVicNrs.Clear();

            string SQL = "select distinct(VehNr) from data530_VEH_RECORD where 1=1 ";
            SQL += GenerateQuaSQL_VEHICLE_DEC().Length != 0 ? " and " + GenerateQuaSQL_VEHICLE_DEC() + " " : " ";
            SQL += GenerateQuaSQL(1).Length != 0 ? " and " + GenerateQuaSQL(1) + " " : " ";
            SQL += GenerateQuaSQL(2).Length != 0 ? " and " + GenerateQuaSQL(2) + " " : " ";
            SQL += GenerateQuaSQL(3).Length != 0 ? " and " + GenerateQuaSQL(3) + " " : " ";
            SQL += " and t < ";
            SQL += endTime_.ToString();
            SQL += " and t >= ";
            SQL += startTime_.ToString();
            SQL += " and WorldX <= " + topX_.ToString();
            SQL += " and WorldX > " + buttonX_.ToString();
            SQL += " and WorldY <= " + topY_.ToString();
            SQL += " and WorldY > " + buttonY_.ToString();

            OleDbDataReader dataReader = _easyAccess.ExecuteDataReader(SQL);
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    _cVicNrs.Add(Convert.ToDouble(dataReader.GetValue(0).ToString()));
                }
            }
        }

        private string GenerateQuaSQL(int param_)
        {
            List<double> v = new List<double>();
            string name = "";
            switch (param_)
            {
                case 0:
                    {
                        v = _routDecs;
                        name = "RoutDec";
                        break;
                    }
                case 1:
                    {
                        v = _vehcTypes;
                        name = "Type";
                        break;
                    }
                case 2:
                    {
                        v = _links;
                        name = "Link";
                        break;
                    }
                case 3:
                    {
                        v = _lanes;
                        name = "Lane";
                        break;
                    }
            }

            string tmp = "";
            foreach (double pV in v)
            {
                tmp += name;
                tmp += " = ";
                tmp += pV.ToString();
                tmp += " or ";
            }
            if (tmp.Length != 0)
            {
                tmp = tmp.Substring(0, tmp.Length - 3);
                tmp = " (" + tmp + ") ";
            }
            return tmp;          
        }

        private Surface GetSurface(VehcileInfo info_,ref Point preLocation_)
        {            
            Surface s = new Surface();
            double xOffset = 0;
            double yOffset = 0;
            VehcShape shape = GetVehcShape(info_.type);
            Point PL = preLocation_;            
            Point NL = new Point(info_.worldX - _centralPoint.x, info_.worldY - _centralPoint.y, info_.t - _startTime);
            if (PL.x != NL.x || PL.y != NL.y)
            {
                preLocation_ = NL;
            }
            else
            {
                _lastSurface.p1.z = NL.z;
                _lastSurface.p2.z = NL.z;
                _lastSurface.p3.z = NL.z;              
                _lastSurface.p4.z = NL.z;

                Surface rst = new Surface();
                rst.p1.x = _lastSurface.p1.x;
                rst.p1.y = _lastSurface.p1.y;
                rst.p1.z = _lastSurface.p1.z;


                rst.p2.x = _lastSurface.p2.x;
                rst.p2.y = _lastSurface.p2.y;
                rst.p2.z = _lastSurface.p2.z;


                rst.p3.x = _lastSurface.p3.x;
                rst.p3.y = _lastSurface.p3.y;
                rst.p3.z = _lastSurface.p3.z;


                rst.p4.x = _lastSurface.p4.x;
                rst.p4.y = _lastSurface.p4.y;
                rst.p4.z = _lastSurface.p4.z;


                return rst;
            }


            double L1 = shape.width / 2;
            double L2 = Math.Abs(Math.Sqrt((NL.x - PL.x) * (NL.x - PL.x) + (NL.y - PL.y) * (NL.y - PL.y)));
            double e = Math.Abs(NL.x - PL.x);
            yOffset = L1 * e / L2;

            if (L1 > yOffset) //部分情况，由于数据的原因可能导致yOffset微大于L1，此时应当为0；
            {
                xOffset = Math.Abs(Math.Sqrt(L1 * L1 - yOffset * yOffset));
            }
            else
            {
                xOffset = 0;
            }

            /*
            if(PL.x != NL.x)
            {
                double slope = (NL.y - PL.y)/(NL.x - PL.x);
                yOffset = Math.Abs(Math.Sqrt(shape.width/(2*(1+slope*slope))));
                xOffset = Math.Abs(slope*yOffset);
            }
            else
            {
                xOffset = shape.width/2;
            }
             * */
            

            //计算柱体的高度
            double height = _maxheight;
            if (info_.vMS != 0)
            {
                height = (shape.length / info_.vMS);    
            }

            //计算车长与两点距离比
            double coefficient = 0;
            double length = Math.Sqrt((NL.x - PL.x)*(NL.x - PL.x) + (NL.y - PL.y)*(NL.y - PL.y));
            coefficient = shape.length / length;

            
            
            //柱体的底面到Z=0平面距离为时间t
            s.p1.x = NL.y > PL.y ? NL.x - xOffset : NL.x + xOffset;
            s.p1.y = NL.x > PL.x ? NL.y + yOffset : NL.y - yOffset;
            s.p1.z = NL.z;



            s.p2.x = NL.y > PL.y ? NL.x + xOffset : NL.x - xOffset;
            s.p2.y = NL.x > PL.x ? NL.y - yOffset : NL.y + yOffset;
            s.p2.z = NL.z;
            
            s.p3.x = s.p2.x - (NL.x - PL.x) * coefficient;
            s.p3.y = s.p2.y - (NL.y - PL.y) * coefficient;
            s.p3.z = NL.z;

            s.p4.x = s.p1.x - (NL.x - PL.x) * coefficient;
            s.p4.y = s.p1.y - (NL.y - PL.y) * coefficient;
            s.p4.z = NL.z;

            if (_fistFlag)
            {
                s.p3.x = s.p2.x;
                s.p3.y = s.p2.y;
                s.p3.z = s.p2.z;

                s.p4.x = s.p1.x;
                s.p4.y = s.p1.y;
                s.p4.z = s.p1.z;
                _fistFlag = false;
            }

            double tmp1 = (s.p1.x - s.p2.x) * (s.p1.x - s.p2.x) + (s.p1.y - s.p2.y) * (s.p1.y - s.p2.y);
            double tmp2 = (s.p1.x - s.p4.x) * (s.p1.x - s.p4.x) + (s.p1.y - s.p4.y) * (s.p1.y - s.p4.y);

            _lastSurface.p1.x = s.p1.x;
            _lastSurface.p1.y = s.p1.y;
            _lastSurface.p1.z = s.p1.z;

            _lastSurface.p2.x = s.p2.x;
            _lastSurface.p2.y = s.p2.y;
            _lastSurface.p2.z = s.p2.z;

            _lastSurface.p3.x = s.p3.x;
            _lastSurface.p3.y = s.p3.y;
            _lastSurface.p3.z = s.p3.z;

            _lastSurface.p4.x = s.p4.x;
            _lastSurface.p4.y = s.p4.y;
            _lastSurface.p4.z = s.p4.z;


            return s;
                                    
        }

        private Point GenerateFirstLocation(VehcileInfo info_)
        {
            _fistFlag = true;
            Point p = new Point(info_.worldX - _centralPoint.x, info_.worldY - _centralPoint.y, info_.t - _startTime);
            Point rst = new Point();
            double offset = 10;
            if (Math.Abs(p.x) >= Math.Abs(p.y))
            {
                if (p.x > 0)
                {
                    rst.x = p.x + offset;
                }
                else
                {
                    rst.x = p.x - offset;
                }
            }
            else
            {
                if (p.y > 0)
                {
                    rst.y = p.y + offset;
                }
                else
                {
                    rst.y = p.y - offset;
                }
            }
            return rst;
        }

        private int CutPoints(ref List<VehcileInfo> infos_)
        {
            List<VehcileInfo> tmp = new List<VehcileInfo>();
            foreach (VehcileInfo info in infos_)
            { 
                
            }
            return 1;
        }

        private VehcShape GetVehcShape(double vehrType_)
        {

            if (_vehcShape.ContainsKey(vehrType_))
            {
                return _vehcShape[vehrType_];
            }

            string SQL = "select * from tb_vehicleShape where 1=1 ";
            SQL += " and FvehicType = ";
            SQL += vehrType_.ToString();
            OleDbDataReader dataReader = _easyAccess.ExecuteDataReader(SQL);

            VehcShape shape = new VehcShape();
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    shape.width = Convert.ToDouble(dataReader.GetValue(Constant.VEH_WIDTH).ToString());
                    shape.length = Convert.ToDouble(dataReader.GetValue(Constant.VEH_LENGTH).ToString());
                    shape.rightSide = Convert.ToDouble(dataReader.GetValue(Constant.VEH_RIGHT_SIDE).ToString());
                    shape.leftSide = Convert.ToDouble(dataReader.GetValue(Constant.VEH_LEFT_SIDE).ToString());
                    shape.front = Convert.ToDouble(dataReader.GetValue(Constant.VEH_FRONT).ToString());
                    _vehcShape.Add(vehrType_, shape);
                    return shape;
                }                
            }
            shape.width = 3;
            shape.length = 4;
            shape.rightSide = 0.5;
            shape.leftSide = 0.5;
            shape.front = 3;
            return shape;
        }

        private bool IsRemain(VehcileInfo info_)
        {
            if (!_cutFlag)
            {
                return true;
            }
            bool rst = true;
            Point c0 = new Point();
            Point c1 = new Point();
            Point c2 = new Point();
            Point c3 = new Point();
            int i = 0;

            if (info_.worldX > 100)
            {
                Console.WriteLine("");
            }
            else if (info_.worldX > 50)
            {
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("");
            }
            foreach (Point p in _cutPoints)
            {

                if (i % 4 == 3)
                {
                    c3 = p;
                    rst = (rst && IsRemain(new Point(info_.worldX, info_.worldY, info_.t), c0, c1, c2, c3));
                }
                else
                {
                    switch (i % 4)
                    {
                        case 0:
                            c0 = p;
                            break;
                        case 1:
                            c1 = p;
                            break;
                        case 2:
                            c2 = p;
                            break;
                    }
                }
                i++;
            }
            return rst;
        }

        private bool IsRemain(Point p_, Point cutP1_, Point cutP2_, Point cutP3_, Point flagP3_)
        {
            double a = 0;
            double b = 0;
            double c = 0;
            if (p_.x < 0)
            {
                a = 1;
            }

            //求出法向量
            double x1 = _cutPoint[0].x;
            double y1 = _cutPoint[0].y;
            double z1 = _cutPoint[0].z;

            double x2 = _cutPoint[1].x;
            double y2 = _cutPoint[1].y;
            double z2 = _cutPoint[1].z;

            double x3 = _cutPoint[2].x;
            double y3 = _cutPoint[2].y;
            double z3 = _cutPoint[2].z;

            double a1 = x1 - x2;
            double b1 = y1 - y2;
            double c1 = z1 - z2;

            double a2 = x3 - x2;
            double b2 = y3 - y2;
            double c2 = z3 - z2;


            a = b1 * c2 - b2 * c1;
            b = c1 * a2 - a1 * c2;
            c = a1 * b2 - a2 * b1;


            double d = a * cutP1_.x + b * cutP1_.y + c * cutP1_.z;
            return (a * p_.x + b * p_.y + c * p_.z - d) * (a * flagP3_.x + b * flagP3_.y + c * flagP3_.z - d) >= 0;           
        }

        private List<double> gaoss(double[,] a)//高斯消元求未知数X，详细算法看文件里的PPT
        {
            int _rows = 3;
            int _cols = 3;
            List<double> rst = new List<double>();
            int L = _rows - 1;
            int i, j, l, n, m, k = 0;
            double[] temp1 = new double[_rows];

            /*第一个do-while是将增广矩阵消成上三角形式*/
            do
            {
                n = 0;
                for (l = k; l < L; l++)

                    temp1[n++] = a[l + 1, k] / a[k, k];
                for (m = 0, i = k + 1; i < _rows; i++, m++)
                {
                    for (j = k; j < _cols; j++)
                        a[i, j] -= temp1[m] * a[k, j];
                }

                k++;


            } while (k < _rows);

            ///*第二个do-while是将矩阵消成对角形式，并且重新给k赋值,最后只剩下对角线和最后一列的数，其它都为0*/
            k = L - 1;

            do
            {
                n = 0;
                for (l = k; l >= 0; l--)
                    temp1[n++] = a[k - l, k + 1] / a[k + 1, k + 1];
                for (m = 0, i = k; i >= 0; i--, m++)
                {
                    for (j = k; j < _cols; j++)
                        a[k - i, j] -= temp1[m] * a[k + 1, j];
                }
                k--;

            } while (k >= 0);
            /*下一个for是解方程组*/
            for (i = 0; i < _rows; i++)
            {
                double value = a[i, _rows] / a[i, i];
                rst.Add(value);                
            }            
            return rst;
        }

        private double TrimDouble(double v_)
        {
            string s = v_.ToString("0.0000000");
            return Convert.ToDouble(s);
        }

        private string GenerateQuaSQL_VEHICLE_DEC()
        {
            string tmp = "";
            List<string> tmpList = new List<string>();
            foreach (VehicleRoute vehicleRoute in _vehicleRoutes)
            {
                if (vehicleRoute.filterSwitch)
                {
                    foreach(string route in vehicleRoute.routes)
                    {
                        tmpList.Add("(RoutDec = " + vehicleRoute.routDec + " and Route = " + route + ")");
                    }
                }
                else
                {
                    tmpList.Add("(RoutDec = " + vehicleRoute.routDec + ")");
                }
            }
            if (tmpList.Count > 0)
            {
                tmp = "(" + string.Join(" or ", tmpList.ToArray()) + ")";
            }
            return tmp;
        }

        #endregion

    }

    
}
