using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace MinivilleBuildFinal.Controls
{
    // This class is used whenever variable numbers need to be displayed at the same spot, like in the card of the player's money on it's HUD, the
    // Value of gains and the price of the cards on the info panels
    class NumberForm
    {
        public Image[] BigSprite;
        public Image[] SmallSprite;
        public Image[] AltSmallSprite;
        public Image[] CostSprite;

        public int Value;
        public int State; // 0 - Normal / 1 - Alt / 2 - Big / 3 - Cost

        public Sprite SpriteHandler;
        public NumberForm(int value, int state)
        {
            BigSprite = new Image[]
            {
                Image.FromFile("Sprites/Number0Big.png"),
                Image.FromFile("Sprites/Number1Big.png"),
                Image.FromFile("Sprites/Number2Big.png"),
                Image.FromFile("Sprites/Number3Big.png"),
                Image.FromFile("Sprites/Number4Big.png"),
                Image.FromFile("Sprites/Number5Big.png"),
                Image.FromFile("Sprites/Number6Big.png"),
                Image.FromFile("Sprites/Number7Big.png"),
                Image.FromFile("Sprites/Number8Big.png"),
                Image.FromFile("Sprites/Number9Big.png"),
            };
            SmallSprite = new Image[]
            {
                Image.FromFile("Sprites/Number0Small.png"),
                Image.FromFile("Sprites/Number1Small.png"),
                Image.FromFile("Sprites/Number2Small.png"),
                Image.FromFile("Sprites/Number3Small.png"),
                Image.FromFile("Sprites/Number4Small.png"),
                Image.FromFile("Sprites/Number5Small.png"),
                Image.FromFile("Sprites/Number6Small.png"),
                Image.FromFile("Sprites/Number7Small.png"),
                Image.FromFile("Sprites/Number8Small.png"),
                Image.FromFile("Sprites/Number9Small.png"),
            };
            AltSmallSprite = new Image[]
            {
                Image.FromFile("Sprites/Number0AltSmall.png"),
                Image.FromFile("Sprites/Number1AltSmall.png"),
                Image.FromFile("Sprites/Number2AltSmall.png"),
                Image.FromFile("Sprites/Number3AltSmall.png"),
                Image.FromFile("Sprites/Number4AltSmall.png"),
                Image.FromFile("Sprites/Number5AltSmall.png"),
                Image.FromFile("Sprites/Number6AltSmall.png"),
                Image.FromFile("Sprites/Number7AltSmall.png"),
                Image.FromFile("Sprites/Number8AltSmall.png"),
                Image.FromFile("Sprites/Number9AltSmall.png"),
            };
            CostSprite = new Image[]
            {
                Image.FromFile("Sprites/Number0Cost.png"),
                Image.FromFile("Sprites/Number1Cost.png"),
                Image.FromFile("Sprites/Number2Cost.png"),
                Image.FromFile("Sprites/Number3Cost.png"),
                Image.FromFile("Sprites/Number4Cost.png"),
                Image.FromFile("Sprites/Number5Cost.png"),
                Image.FromFile("Sprites/Number6Cost.png"),
                Image.FromFile("Sprites/Number7Cost.png"),
                Image.FromFile("Sprites/Number8Cost.png"),
                Image.FromFile("Sprites/Number9Cost.png"),
            };

            int Value = value;
            int State = state;

            SpriteHandler = new Sprite(BigSprite[Value], new Point(0, 0), 0);
        }

        // This method simply changes th number's value and look, like in the case of the player's number which gets bigger and changes color when they're the active player
        public void ChangeNumber(int value, int state)
        {
            Value = value;
            State = state;
            switch (State)
            {
                case (0):
                    SpriteHandler.sprite = SmallSprite[Value];
                    break;
                case (1):
                    SpriteHandler.sprite = AltSmallSprite[Value];
                    break;
                case (2):
                    SpriteHandler.sprite = BigSprite[Value];
                    break;
                case (3):
                    SpriteHandler.sprite = CostSprite[Value];
                    break;
                default:
                    SpriteHandler.sprite = SmallSprite[Value];
                    break;
            }
        }
    }
}
