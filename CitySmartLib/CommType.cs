using System;
using System.Collections.Generic;
using System.Text;

namespace CitySmart
{
    //车辆的时空信息
    public class VehcileInfo
    {
        public double vehNr;
        public double type;
        public string vehTypeName;
        public double worldX;
        public double worldY;
        public double t;
        public double vMS;
        public double link;
        public double lane;
        public double tQDelay;
        public double route;
        public double routDec;

        public VehcileInfo()
        {
            vehNr = 0;
            type = 0;
            vehTypeName = "car";
            worldX = 0;
            worldY = 0;
            t = 0;
            vMS = 0;
            link = 0;
            lane = 0;
            tQDelay = 0;
            route = 0;
            routDec = 0;
        }
    }  


    public class Point
    {
        public double x;
        public double y;
        public double z;
        public Point(double x_, double y_, double z_)
        {
            x = x_;
            y = y_;
            z = z_;
        }
        public Point()
        {
            x = 0.0;
            y = 0.0;
            z = 0.0;
        }
    }

    public class Surface
    {
        public Point p1;
        public Point p2;
        public Point p3;
        public Point p4;
        public Surface()
        {
            p1 = new Point(0, 0, 0);
            p2 = new Point(0, 0, 0);
            p3 = new Point(0, 0, 0);
            p4 = new Point(0, 0, 0);
        }
    }

    public class VehcShape
    {
        public double width;
        public double length;
        public double rightSide;
        public double leftSide;
        public double front;

        public VehcShape()
        {
            width = 0;
            length = 0;
            rightSide = 0;
            leftSide = 0;
            front = 0;
        }
    }

    public class Color
    {
        public double red;
        public double green;
        public double blue;

        public Color()
        {
            red = 0;
            green = 0.5;
            blue = 1;
        }
    }

    public class VehicleRoute
    {
        public VehicleRoute()
        {
            routes = new List<string>();
        }
        public VehicleRoute(string routDec_, List<string> routes_)
        {
            routDec = routDec_;
            routes = routes_;
            filterSwitch = true;
        }
        public VehicleRoute(string routDec_)
        {
            routDec = routDec_;
            filterSwitch = false;
        }
        public string routDec;
        public List<string> routes;
        public bool filterSwitch = false;
    }

    public class CRstSet
    {
        public double utilization;
        public int carNum;
        public double avaMS;
        public double avaTQDelay;
        public double density;
        
        public CRstSet(int carNum_, double utilization_)
        {
            utilization = utilization_;
            carNum = carNum_;
        }
        public CRstSet()
        { }
    }
}
