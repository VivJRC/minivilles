using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MinivilleBuildFinal.Controls
{
    // This class is used when the users have yet to chose the amount of players. They are th button that allow them to do their choice
    class PlayerChoiceButton
    {
        public Sprite sprite;
        public Image on;
        public Image off;

        int ID;

        public PlayerChoiceButton(int id)
        {
            on = Image.FromFile("Sprites/PlayerYes.png");
            off = Image.FromFile("Sprites/PlayerNot.png");

            ID = id;
            if (ID == 0 || ID == 1)
            {
                sprite = new Sprite(on, new Point(0, 0), 0);
            }
            else
            {
                sprite = new Sprite(off, new Point(0, 0), 0);
            }
        }
        override public string ToString()
        {
            return ID.ToString();
        }
    }
}
