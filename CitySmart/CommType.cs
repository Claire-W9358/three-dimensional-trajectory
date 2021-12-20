using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CitySmart
{
    class CommType
    {
    }

    public enum SelectOption
    { 
        LANE,
        LINK,
        VEHC_TYPE,
        ROUT_DEC,
        VEHICLE_ROUTE
    }

    public class SelectParams : EventArgs
    {
        public List<string> param;
        public SelectOption option;
        public string routDec;
        public SelectParams(SelectOption option_, List<string> param_)
        {
            param = param_;
            option = option_;
        }
        public SelectParams(string routDec_)
        {
            routDec = routDec_;
            option = SelectOption.VEHICLE_ROUTE;
        }
        public SelectParams(string routDec_, List<string> param_)
        {
            param = param_;
            routDec = routDec_;
            option = SelectOption.VEHICLE_ROUTE;
        }


    }
}
