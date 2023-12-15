using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinivilleBuildFinal
{
    internal class Die
    {
        public int NbFaces;
        private Random random = new Random();
        private int face;
        public Die(int nbfaces = 6)
        {
            NbFaces = nbfaces;
            Face = random.Next(0, NbFaces + 1);
        }
        public override string ToString()
        {
            string toString = String.Format("\n+---+\n| {0} |\n+---+", face);
            return toString;
        }

        public int Face
        {
            get
            {
                return face;
            }
            private set
            {
                face = value;
            }
        }

        public virtual void Throw()
        {
            Face = this.random.Next(1, NbFaces + 1);
        }
    }
}
