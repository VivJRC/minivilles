using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MinivilleBuildFinal.Controls
{
    // This sprite class is the backbone of the program's visuals. It is the only thing rendered on the board, and every element that needs to be rendered
    // is  or has a sprite class. 
    class Sprite
    {
        public Image sprite;
        public Point pos;
        public int rota = 0;


        public Sprite(Image img, Point p, int rot)
        {
            sprite = img;
            pos = p;
        }

        // This method simply renders itself. 
        public void Render(Graphics g)
        {
            g.DrawImage(sprite, pos);
        }
    }
    // Having a simple all-icompassing class for rendering made the program much easier to create. I simply needed to assemble a list of every sprite that needed
    // to be rendered and call their Render function. This simplified the program a lot and meant I had full control over the rendering of the screen and it's elements
}
