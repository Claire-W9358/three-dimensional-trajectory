using System;
using System.Collections.Generic;
using System.Text;

namespace CitySmart
{
    public class Constant
    {
        public const string KEY_COLOR_SET = "colorSet";

        public const int VEH_NR = 1;
        public const int TYPE = 0;
        public const int VEH_TYPE_NAME = 2;
        public const int WORLD_X = 3;
        public const int WORLD_Y = 5;
        public const int T = 4;
        public const int V_MS = 7;
        public const int LINK = 6;
        public const int LANE = 8;
        public const int TQ_DELAY = 10;
        public const int ROUTE = 9;
        public const int ROUTE_DEC = 11;
        public const int Vx = 12;
        public const int Vy = 13;
        public const int Index = 14;

        public const int VEH_WIDTH = 2;
        public const int VEH_LENGTH = 3;
        public const int VEH_RIGHT_SIDE = 5;
        public const int VEH_LEFT_SIDE = 4;
        public const int VEH_FRONT = 6;
        //交通参与者类别
        public const int LarCar = 0;
        public const int SmaCar = 1;
        public const int NMVeh = 2;
        public const int Pas = 3;

        public Color[] COLOR_SET = new Color[12];

        public Constant() 
        {
            for (int i = 0; i < 12; i++)
            {
                COLOR_SET[i] = new Color();
                COLOR_SET[i].red = 1;
                COLOR_SET[i].green = 0;
                COLOR_SET[i].blue = 0;
            }

            COLOR_SET[0].red = 0.0627;
            COLOR_SET[0].green = 0.1451;
            COLOR_SET[0].blue = 0.2471;

            COLOR_SET[1].red = 0.2157;
            COLOR_SET[1].green = 0.3686;
            COLOR_SET[1].blue = 0.5725;

            COLOR_SET[2].red = 0.3333;
            COLOR_SET[2].green = 0.5569;
            COLOR_SET[2].blue = 0.8353;

            COLOR_SET[3].red = 0.5961;
            COLOR_SET[3].green = 0.2824;
            COLOR_SET[3].blue = 0.0275;

            COLOR_SET[4].red = 0.8941;
            COLOR_SET[4].green = 0.4235;
            COLOR_SET[4].blue = 0.0392;

            COLOR_SET[5].red = 0.9804;
            COLOR_SET[5].green = 0.7529;
            COLOR_SET[5].blue = 0.5647;

            COLOR_SET[6].red = 0.3098;
            COLOR_SET[6].green = 0.3843;
            COLOR_SET[6].blue = 0.1569;

            COLOR_SET[7].red = 0.4667;
            COLOR_SET[7].green = 0.5765;
            COLOR_SET[7].blue = 0.2353;

            COLOR_SET[8].red = 0.7647;
            COLOR_SET[8].green = 0.8392;
            COLOR_SET[8].blue = 0.6078;

            COLOR_SET[9].red = 0.2510;
            COLOR_SET[9].green = 0.1922;
            COLOR_SET[9].blue = 0.3216;

            COLOR_SET[10].red = 0.3765;
            COLOR_SET[10].green = 0.2902;
            COLOR_SET[10].blue = 0.4824;

            COLOR_SET[11].red = 0.7020;
            COLOR_SET[11].green = 0.6353;
            COLOR_SET[11].blue = 0.7804;
        }
    }
}
