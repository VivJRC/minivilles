using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MinivilleBuildFinal.Controls
{
    // This class is used to update and display the end animation which shows which player won
    class EndAnimClass
    {
        public List<Sprite> Letters;
        public NumberForm numberform;

        public List<int> intednedY;
        public int NumberIntendedPos;

        int count = 0;

        // Here we instantiate the class and all it's sprites
        public EndAnimClass()
        {
            Letters = new List<Sprite>();
            Letters.Add(new Sprite(Image.FromFile("Sprites/Letters/P.png"), new Point(192, -288), 0));
            Letters.Add(new Sprite(Image.FromFile("Sprites/Letters/L.png"), new Point(288, -288), 0));
            Letters.Add(new Sprite(Image.FromFile("Sprites/Letters/A.png"), new Point(384, -288), 0));
            Letters.Add(new Sprite(Image.FromFile("Sprites/Letters/Y.png"), new Point(480, -288), 0));
            Letters.Add(new Sprite(Image.FromFile("Sprites/Letters/E.png"), new Point(576, -288), 0));
            Letters.Add(new Sprite(Image.FromFile("Sprites/Letters/R.png"), new Point(672, -288), 0));
            Letters.Add(new Sprite(Image.FromFile("Sprites/Letters/W.png"), new Point(384, -96), 0));
            Letters.Add(new Sprite(Image.FromFile("Sprites/Letters/I.png"), new Point(480, -96), 0));
            Letters.Add(new Sprite(Image.FromFile("Sprites/Letters/N.png"), new Point(576, -96), 0));
            Letters.Add(new Sprite(Image.FromFile("Sprites/Letters/S.png"), new Point(672, -96), 0));

            intednedY = new List<int>();
            intednedY.Add(-288);
            intednedY.Add(-288);
            intednedY.Add(-288);
            intednedY.Add(-288);
            intednedY.Add(-288);
            intednedY.Add(-288);
            intednedY.Add(-96);
            intednedY.Add(-96);
            intednedY.Add(-96);
            intednedY.Add(-96);

            numberform = new NumberForm(0, 3);
            numberform.SpriteHandler.pos = new Point(192, 864);
        }
        // And here we animate them by lowering them one after the other
        public List<Sprite> Anim()
        {
            List<Sprite> sprt = new List<Sprite>();

            int i = 0;
            foreach(Sprite s in Letters)
            {
                if(i <= count)
                {
                    s.pos = new Point(s.pos.X, s.pos.Y + ((intednedY[i] - s.pos.Y) / 4));
                }
                sprt.Add(s);
                i++;
            }
            count++;

            numberform.SpriteHandler.pos = new Point(numberform.SpriteHandler.pos.X, numberform.SpriteHandler.pos.Y + ((NumberIntendedPos - numberform.SpriteHandler.pos.Y) / 4));
            sprt.Add(numberform.SpriteHandler);

            return sprt;
        }
    }
}
