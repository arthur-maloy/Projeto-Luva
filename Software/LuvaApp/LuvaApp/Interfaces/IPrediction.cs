using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuvaApp.Interfaces
{
    internal interface IPrediction
    {
        public string Predict(object[] sensorValues);
    }
}
