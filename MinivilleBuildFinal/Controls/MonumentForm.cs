using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MinivilleBuildFinal.Controls
{
    // This class displays the player's monument on his HUD. It simply has two states : on or off, and is permanently displayed on the player's HUD
    class MonumentForm
    {
        public Image notBought;
        public Image bought;

        public int Type;
        public bool isBought;

        public Sprite sprite;

        public MonumentForm()
        {
            bought = Image.FromFile("Sprites/monumentBought.png");
            notBought = Image.FromFile("Sprites/monumentNotBought.png");
            sprite = new Sprite(notBought, new Point(0, 0), 0);
        }
    }
}
