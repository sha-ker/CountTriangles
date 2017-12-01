using System;
using System.Collections.Generic;
using System.Drawing;
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

        public bool istGleich(cDreiecke tempDreieck)
        {
            PointF tempDreieckP1 = new PointF(tempDreieck.AX, tempDreieck.AY);
            PointF tempDreieckP2 = new PointF(tempDreieck.BX, tempDreieck.BY);
            PointF tempDreieckP3 = new PointF(tempDreieck.CX, tempDreieck.CY);
            PointF dreieckP1 = new PointF(AX, AY);
            PointF dreieckP2 = new PointF(BX, BY);
            PointF dreieckP3 = new PointF(CX, CY);

            if (dreieckP1 == tempDreieckP1 || dreieckP1 == tempDreieckP2 || dreieckP1 == tempDreieckP3)
            {
                if (dreieckP2 == tempDreieckP2 || dreieckP2 == tempDreieckP1 || dreieckP2 == tempDreieckP3)
                {
                    if (dreieckP3 == tempDreieckP3 || dreieckP3 == tempDreieckP2 || dreieckP3 == tempDreieckP1)
                    {
                        return true;
                    }
                }
            }
            return false;
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
