using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreieckeZählen
{
    class cPunkte
    {
        private static int localId = 0;
        //SP = Schnittpunkt
        float SpX;
        float SpY;
        int StreckenID1;
        int StreckenID2;

        public int MyId { get; set; }

        public cPunkte(float _SpX, float _SpY, int Id1, int Id2)
        {
            SpX = _SpX;
            SpY = _SpY;
            StreckenID1 = Id1;
            StreckenID2 = Id2;
            MyId = localId;
            localId++;
        }

        public float SpX1
        {
            get
            {
                return SpX;
            }

        }

        public float SpY1
        {
            get
            {
                return SpY;
            }
        }

        public int StreckenID11
        {
            get
            {
                return StreckenID1;
            }
        }

        public int StreckenID21
        {
            get
            {
                return StreckenID2;
            }
        }
    }
}
