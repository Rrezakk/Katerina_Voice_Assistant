using System;
using System.Collections.Generic;
using System.Text;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.Special
{
    public class MatrixElem
    {
        public MatrixElem() { }
        public MatrixElem(int row, int col, float value)
        {
            this.Row = row;
            this.Col = col;
            this.Value = value;
        }
        public int Row;
        public int Col;
        public float Value;
    }
}
