using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;


namespace CitySmart
{
    public class ConfigInfo
    {
        public static int[] colorSet = new int[12];

        static ConfigInfo()
        {
            string colorSets = ConfigurationManager.AppSettings[Constant.KEY_COLOR_SET];
            string [] tmp = colorSets.Split(';');
            int i = 0;
            while (i < 12)
            {
                colorSet[i] = Convert.ToInt16(tmp[i]);
                i++;
            }
        }
        public static void ReloadColorSet()
        {
            string colorSets = ConfigurationManager.AppSettings[Constant.KEY_COLOR_SET];
            string[] tmp = colorSets.Split(';');
            int i = 0;
            while (i < 12)
            {
                colorSet[i] = Convert.ToInt16(tmp[i]);
                i++;
            }
        }
    }
}
