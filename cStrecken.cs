using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreieckeZählen
{
    class cStrecken
    {
        private static int localId = 0;
        //ep = Endpunkt
        float ep1X;
        float ep1Y;
        float ep2X;
        float ep2Y;

        public int MyId { get; set; }

        public cStrecken(float _ep1X, float _ep1Y, float _ep2X, float _ep2Y)
        {
            ep1X = _ep1X;
            ep1Y = _ep1Y;
            ep2X = _ep2X;
            ep2Y = _ep2Y;
            MyId = localId;
            localId++;
        }

        public float Ep1X
        {
            get
            {
                return ep1X;
            }
        }

        public float Ep1Y
        {
            get
            {
                return ep1Y;
            }
        }

        public float Ep2X
        {
            get
            {
                return ep2X;
            }
        }

        public float Ep2Y
        {
            get
            {
                return ep2Y;
            }
        }
    }
}
