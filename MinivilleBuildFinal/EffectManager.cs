using MinivilleBuildFinal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using static MinivilleBuildFinal.GameManager;
using MinivilleBuildFinal.Enum;

namespace MinivilleBuildFinal
{
    class EffectManager
    {
        private enum ResearchType
        {
            Payments = 0,
            Incomes = 1,
            Exchanges = 2,
        }
        private List<(Player, Card)> SearchCard(List<Player> players, int currentPlayerNumber, ResearchType typeResearch, int roll)
        {

            //Console.WriteLine("\n--------------\n{0} :\n", typeResearch);
            int index = currentPlayerNumber;
            List<(Player, Card)> cardsToActivate = new List<(Player, Card)>();
            for (int i = 0; i < players.Count; i++)
            {
                foreach (Card key in players[index]._terrain.Keys)
                {
                    int value = players[index]._terrain[key];

                    //Check Payments With Red Cards
                    if (typeResearch == ResearchType.Payments && currentPlayerNumber != index && key._color == ColorCard.red && key._rollToActivate.Contains(roll))
                    {
                        for (int iteration = value; iteration > 0; iteration--)
                        { cardsToActivate.Add((players[index], key)); }
                    }

                    //Check Payments With Blue Cards
                    else if (typeResearch == ResearchType.Incomes && key._color == ColorCard.blue && key._rollToActivate.Contains(roll))
                    {
                        for (int iteration = value; iteration > 0; iteration--)
                        { cardsToActivate.Add((players[index], key)); }
                    }

                    //Check Payments With Green and Purple Cards
                    else if (typeResearch == ResearchType.Incomes && currentPlayerNumber == index && (key._color == ColorCard.green || (key._color == ColorCard.purple && !key._properties.Contains(Propertie.Exchangeable))) && key._rollToActivate.Contains(roll))
                    {
                        for (int iteration = value; iteration > 0; iteration--)
                        { cardsToActivate.Add((players[index], key)); }
                    }


                    //Check Exchanges With Purple Cards
                    else if (typeResearch == ResearchType.Exchanges && currentPlayerNumber == index && key._color == ColorCard.purple && key._properties.Contains(Propertie.Exchangeable) && key._rollToActivate.Contains(roll))
                    {
                        for (int iteration = value; iteration > 0; iteration--)
                        { cardsToActivate.Add((players[index], key)); }
                    }

                }
                if (index > 0) index -= 1;
                else index = players.Count - 1;
            }
            //foreach (var kvp in cardsToActivate) Console.WriteLine("{0} by {1}", kvp.Item2._name, kvp.Item1._name);
            return cardsToActivate;
        }

        public List<List<int>> Effect(int roll, Player currentPlayer, List<Player> players)
        {
            //Initiate EffectOrder
            List<(Player, Card)> effectOrder = new List<(Player, Card)>();

            //Reset Effect to Activate
            List<(Player, Card)> income;
            List<(Player, Card)> payment;
            List<(Player, Card)> exchange;

            //Find Current Player's number
            bool numPlayerFound = false;
            int currPlayerNumber = 0;
            while (!numPlayerFound)
            {
                if (players[currPlayerNumber] == currentPlayer) numPlayerFound = true;
                else currPlayerNumber++;
            }
            //Console.WriteLine(currPlayerNumber);


            //Find Payments
            income = this.SearchCard(players, currPlayerNumber, ResearchType.Incomes, roll);

            //Find Revenues
            payment = this.SearchCard(players, currPlayerNumber, ResearchType.Payments, roll);

            //Find Exchanges
            exchange = this.SearchCard(players, currPlayerNumber, ResearchType.Exchanges, roll);

            //Make the effectOrder
            foreach (var element in income) effectOrder.Add(element);
            foreach (var element in payment) effectOrder.Add(element);
            foreach (var element in exchange) effectOrder.Add(element);

            List<List<int>> toReturn = new List<List<int>>();
            //Tell the cards to activate their effect
            for (int i = 0; i < effectOrder.Count; i++)
            {
                toReturn.Add(effectOrder[i].Item2.Effect(currentPlayer, effectOrder[i].Item1, players));
            }
            return toReturn;
        }
    }
}