using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MinivilleBuildFinal.Controls
{
    // This class handles the visuals used for the gains animation. It contains numbers and a few symboles that change depending on the nature of the gain
    class GainForm
    {
        Image plusimg;
        Image minusimg;
        Image moneyimg;
        Image moneyntimg;

        Sprite signSprite;
        NumberForm number1;
        NumberForm number2;
        Sprite money;

        public int value;
        public bool isAnim = false;

        public GainForm()
        {
            plusimg = Image.FromFile("Sprites/Plus.png");
            minusimg = Image.FromFile("Sprites/Minus.png");
            moneyimg = Image.FromFile("Sprites/Money.png");
            moneyntimg = Image.FromFile("Sprites/Moneynt.png");

            signSprite = new Sprite(plusimg, new Point(0, 0), 0);
            number1 = new NumberForm(0, 1);
            number2 = new NumberForm(0, 1);
            money = new Sprite(moneyimg, new Point(0, 0), 0);
        }
        // This method initiates the gains for the animation
        public void InitGain(int val, Point pos)
        {
            value = val;
            if (value < 0)
            {
                signSprite.sprite = minusimg;
                number1.ChangeNumber((Math.Abs(value) - (Math.Abs(value) % 10)) / 10, 0);
                number2.ChangeNumber(Math.Abs(value) % 10, 0);
                money.sprite = moneyntimg;
            }
            else
            {
                signSprite.sprite = plusimg;
                number1.ChangeNumber((Math.Abs(value) - (Math.Abs(value) % 10)) / 10, 1);
                number2.ChangeNumber(Math.Abs(value) % 10, 1);
                money.sprite = moneyimg;
            }

            signSprite.pos = pos;
            number1.SpriteHandler.pos = new Point(pos.X + 48, pos.Y);
            number2.SpriteHandler.pos = new Point(pos.X + 96, pos.Y);
            money.pos = new Point(pos.X + 144, pos.Y);

            isAnim = true;
        }
        // And this one animates them
        public List<Sprite> AnimateGain()
        {
            if (isAnim)
            {
                signSprite.pos = new Point(signSprite.pos.X, signSprite.pos.Y - 1);
                number1.SpriteHandler.pos = new Point(number1.SpriteHandler.pos.X, number1.SpriteHandler.pos.Y - 1);
                number2.SpriteHandler.pos = new Point(number2.SpriteHandler.pos.X, number2.SpriteHandler.pos.Y - 1);
                money.pos = new Point(money.pos.X, money.pos.Y - 1);

                List<Sprite> sprt = new List<Sprite>();
                sprt.Add(signSprite);
                sprt.Add(number1.SpriteHandler);
                sprt.Add(number2.SpriteHandler);
                sprt.Add(money);
                return sprt;
            }
            else
            {
                return null;
            }
        }
        public void EndAnim()
        {
            isAnim = false;
        }
    }
}
