using System;
using System.Collections.Generic;
using System.Text;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.Special
{
    public class MatrixElem
    {
        public MatrixElem() { }
        public MatrixElem(int line, int pos, float value)
        {
            this.Line = line;
            this.Pos = pos;
            this.Value = value;
        }
        public int Line;
        public int Pos;
        public float Value;
    }
}
