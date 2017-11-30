using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreieckeZählen
{
    class cDreiecke
    {
        float aX, aY, bX, bY, cX, cY;
        public cDreiecke(float _aX, float _aY, float _bX, float _bY, float _cX, float _cY)
        {
            aX = _aX;
            aY = _aY;
            bX = _bX;
            bY = _bY;
            cX = _cX;
            cY = _cY;
        }

        public float AX
        {
            get
            {
                return aX;
            }
        }

        public float AY
        {
            get
            {
                return aY;
            }
        }

        public float BX
        {
            get
            {
                return bX;
            }
        }

        public float BY
        {
            get
            {
                return bY;
            }
        }

        public float CX
        {
            get
            {
                return cX;
            }
        }

        public float CY
        {
            get
            {
                return cY;
            }
        }
    }
}
