using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MinivilleBuildFinal.Controls
{
    // This class is used to diplay a card. It has all the information required to be displayed correctly as the card it's supposed to be

    class CardForm
    {
        public Card CardType;
        public bool isSmall;
        public bool isNumber;

        public bool isShop;

        public Sprite sprite;

        public CardForm(Card type, bool small, bool number, bool issShop = false)
        {
            CardType = type;
            isSmall = small;
            isNumber = number;
            isShop = issShop;

            if (isSmall)
            {
                sprite = new Sprite(Image.FromFile(String.Format("Sprites/Cards/{0}.png", CardType._color)), new Point(0, 0), 0);
            }
            else
            {
                sprite = new Sprite(Image.FromFile(String.Format("Sprites/Cards/{0}.png", CardType._name)), new Point(0, 0), 0);
            }
        }
        // These two methods are used to switch the card's state from smaller to higher, or from higher to lower
        public void setToSmall()
        {
            isSmall = true;
            sprite = new Sprite(Image.FromFile(String.Format("Sprites/Cards/{0}.png", CardType._color)), sprite.pos, 0);
        }
        public void setToBig()
        {
            isSmall = false;
            sprite = new Sprite(Image.FromFile(String.Format("Sprites/Cards/{0}.png", CardType._name)), sprite.pos, 0);
        }
    }
}
