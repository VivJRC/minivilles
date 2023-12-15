using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MinivilleBuildFinal.Controls
{
    // This game object is the player's deck, their card on the board. It contains a list of all the player's cards and the orientation of the player on the board
    class PlayerDeckForm
    {
        public List<CardForm> PlayerCards;
        public int IntendedRota;

        public int drawnCard = -1;
        public int displacementfordrawncard = 0;

        public List<Point> cardIntendedPos;

        public PlayerDeckForm()
        {

        }
        // This method is used to update the player's deck depending on their cards and orientation. it then returns a list of all the sprites to display
        public List<Sprite> UpdatePlayerDeckForm()
        {
            List<Sprite> PLAYERDECK = new List<Sprite>();

            cardIntendedPos = new List<Point>();

            int inter = 0;
            int i = 0;
            // For each card...
            foreach (CardForm c in PlayerCards)
            {
                cardIntendedPos.Add(new Point(0, 0));

                Sprite sprt;

                // It calculates it's place on the board. Since there is a maximum allowed space for all the players cards, the spacing between them
                // needs to be dynamic, and it changes depending on the player's orientation
                int aha = 0;
                if (drawnCard == i)
                {
                    aha = displacementfordrawncard;
                }
                c.sprite.rota = IntendedRota;
                switch (IntendedRota)
                {
                    case (0):
                        if (c.isSmall)
                        {
                            cardIntendedPos[i] = new Point(336 + inter, 576);
                            sprt = new Sprite(c.sprite.sprite, new Point(336 + inter, 576), 0);
                        }
                        else
                        {
                            cardIntendedPos[i] = new Point(192 + inter, 480 - aha);
                            sprt = new Sprite(c.sprite.sprite, new Point(192 + inter, 480 - aha), 0);
                            inter += 192 / (PlayerCards.Count - 1);
                        }
                        break;
                    case (1):
                        cardIntendedPos[i] = new Point(816 - aha, 480 - inter);
                        sprt = new Sprite(c.sprite.sprite, new Point(816 - aha, 480 - inter), 3);
                        break;
                    case (2):
                        cardIntendedPos[i] = new Point(576 - inter, 96 + aha);
                        sprt = new Sprite(c.sprite.sprite, new Point(576 - inter, 96 + aha), 2);
                        break;
                    case (3):
                        cardIntendedPos[i] = new Point(72 + aha, 240 + inter);
                        sprt = new Sprite(c.sprite.sprite, new Point(72 + aha, 240 + inter), 1);
                        break;
                    default:
                        cardIntendedPos[i] = new Point(336, 576);
                        sprt = new Sprite(c.sprite.sprite, new Point(336, 576), 0);
                        break;
                }
                PLAYERDECK.Add(sprt);

                i++;
                inter += 240 / (PlayerCards.Count - 1);
            }

            return PLAYERDECK;
        }

        // This method is used to move the card one quarter of the way from their current position to their final destination. We only move them a fraction
        // of the way to get a smooth animation of them sliding on the board and not just teleporting to their intended postion. This helps the players understand
        // where their cards went
        public List<Sprite> moveCards()
        {
            List<Sprite> PLAYERDECK = new List<Sprite>();

            int i = 0;
            foreach (CardForm c in PlayerCards)
            {
                if (!(c.sprite.pos.X + 1 < cardIntendedPos[i].X & c.sprite.pos.X - 1 > cardIntendedPos[i].X & c.sprite.pos.Y + 1 < cardIntendedPos[i].Y & c.sprite.pos.Y - 1 > cardIntendedPos[i].Y))
                {
                    Point nextpos = new Point(0, 0);
                    nextpos.X = c.sprite.pos.X + ((cardIntendedPos[i].X - c.sprite.pos.X) / 4);
                    nextpos.Y = c.sprite.pos.Y + ((cardIntendedPos[i].Y - c.sprite.pos.Y) / 4);
                    c.sprite.pos = nextpos;
                }
                PLAYERDECK.Add(new Sprite(c.sprite.sprite, c.sprite.pos, IntendedRota));
                i++;
            }
            return PLAYERDECK;
        }

        // And this is used to check the card's position and see if it has arrived to it's target destination
        public bool testCardPos()
        {
            bool test = true;
            int i = 0;
            foreach (CardForm c in PlayerCards)
            {
                if (!(c.sprite.pos.X + 5 > cardIntendedPos[i].X & c.sprite.pos.X - 5 < cardIntendedPos[i].X & c.sprite.pos.Y + 5 > cardIntendedPos[i].Y & c.sprite.pos.Y - 5 < cardIntendedPos[i].Y))
                {
                    test = false;
                }
                i++;
            }

            return test;
        }
    }
}
