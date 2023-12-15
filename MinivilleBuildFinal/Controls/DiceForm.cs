using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MinivilleBuildFinal.Controls
{
    // This class handles the dices
    class DiceForm
    {
        public Sprite dice;

        public int value;
        public int animSpeed;

        private bool isDone = false;

        public DiceForm(int _value)
        {
            value = _value;
            animSpeed = 0;
            dice = new Sprite(Image.FromFile(String.Format("sprites/Dice{0}.png",value)), new Point(0, 0), 0);
        }

        // This code initializes the dice for it's animation
        public void InitAnim(Point pos, int speed)
        {
            isDone = false;
            dice.pos = pos;
            animSpeed = speed;
        }

        // And this class animates the dice
        public bool Anim()
        {
            if (animSpeed < 1)
            {
                isDone = true;
            }
            else
            {
                dice.pos = new Point(dice.pos.X + animSpeed, dice.pos.Y);
                animSpeed -= 1;
            }
            return isDone; // returns true when the animation is over 
        }
    }
}
