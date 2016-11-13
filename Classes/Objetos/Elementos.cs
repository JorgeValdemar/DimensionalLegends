using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classes.Objetos
{
    public class Elementos
    {
        private int _elementoId;
        private string _elemento;

        // 8x8 fogo, gelo, terra, raio, luz, trevas, agua, vento
        public static int[][] TabelaElemental = new int[][]
                        {           
                            new int[] { 0,  1,  0,  0,  0,  0, -1,  0},
                            new int[] {-1,  0,  0, -1,  0,  0,  1,  0},
                            new int[] { 0,  0,  0,  1,  0,  0, -1,  1},
                            new int[] { 0,  1, -1,  0,  0,  0,  1, -1},
                            new int[] { 0,  0,  0,  0,  0,  2,  0,  0},
                            new int[] { 0,  0,  0,  0,  2,  0,  0,  0},
                            new int[] { 1,  0,  1, -1,  0,  0,  0,  0},
                            new int[] { 0, -1, -1,  1,  0,  0,  0,  0}
                        };
                
        public int ElementoId 
        {
            get { return _elementoId == 0 ? 9 : _elementoId; }
            set { _elementoId = value; }
        }
        public string Elemento 
        {
            get { return string.IsNullOrEmpty(_elemento) ? "" : _elemento; }
            set { _elemento = value; } 
        }

    }
}
