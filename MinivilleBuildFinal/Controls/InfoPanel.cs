using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MinivilleBuildFinal.Controls
{
    // This is the panel that shows all the information about card that are clicked upon
    class InfoPanel
    {
        public Sprite backdrop;
        public Sprite card;
        public Sprite desc;
        public Sprite button;
        public NumberForm price;

        public bool IsBuy;
        public string Text;
        public Card CardType;

        public Form1 form;

        public InfoPanel(Form1 foorm)
        {
            form = foorm;
            backdrop = new Sprite(Image.FromFile("Sprites/InfoPanel.png"), new Point(0, 0), 0);
            price = new NumberForm(0, 3);
        }

        // This is used to update the panel's image and description to fit the clicked card
        public void UpdateImage()
        {
            card = new Sprite(Image.FromFile(String.Format("Sprites/Cards/{0}.png", CardType._name)), new Point(backdrop.pos.X + 24, backdrop.pos.Y + 24), 0);
            desc = new Sprite(Image.FromFile(String.Format("Sprites/Cards/Desc/{0}.png", CardType._name)), backdrop.pos, 0);

            button = new Sprite(Image.FromFile("Sprites/buy.png"), new Point(backdrop.pos.X + 24, backdrop.pos.Y + 286), 0);
            price.ChangeNumber(CardType._cost, 3);
            price.SpriteHandler.pos = new Point(backdrop.pos.X + 94, backdrop.pos.Y + 294);
        }
    }
}
