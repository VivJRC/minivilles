using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinivilleBuildFinal
{
    internal class Player
    {
        public bool isActive = false;
        public List<Card> cards = new List<Card>();
        public bool[] monuments = new bool[4]; //{ true, true, true, true }; // Change number if you add monuments


        public string _name;
        public Dictionary<Card, int> _terrain;
        public int _coins = 3;
        public List<Die> _dice;
        public List<Monument> _monuments;
        public List<int> _currentThrow;
        public Player(string name, int coins, List<Monument> monuments, Dictionary<Card, int> terrain)
        {

            this._name = name;
            this._coins = coins;
            this._terrain = terrain;

            foreach(var v in _terrain)
            {
                if(v.Value != 0)
                {
                    cards.Add(v.Key);
                }
            }

            Die initialDie = new Die();
            _dice = new List<Die>();
            this._dice.Add(initialDie);
            _currentThrow = new List<int>();
            this._monuments = monuments;

        }
        public int ThrowDie()
        {
            int numberThrew = 0;
            int roll = 0;
            _dice[0].Throw();
            roll += _dice[0].Face;
            return _dice[0].Face;
            /*
            bool thatsSoundsGoodToMe = false;
            bool continueThrowingDie = true;
            if (this._dice.Count == 1)
            {
                while (true)
                {
                    roll = 0;
                    _dice[0].Throw();
                    roll += _dice[0].Face;
                    continueThrowingDie = false;
                    foreach (Monument monument in _monuments)
                    {
                        _currentThrow.Clear();
                        _currentThrow.Add(_dice[0].Face);
                        if (!continueThrowingDie) { continueThrowingDie = monument.CanResetDiceThrow(_currentThrow, roll); }
                    }
                    if (!continueThrowingDie) { break; }
                }
            }
            else
            {
                while (true)
                {
                    roll = 0;
                    while (true)
                    {
                        Console.WriteLine("How many die do you want to throw ? (1 to {0})", this._dice.Count);
                        numberThrew = int.Parse(Console.ReadLine()); // TEMPORARY
                        if (numberThrew >= 1 && numberThrew <= this._dice.Count) { thatsSoundsGoodToMe = true; }
                        if (thatsSoundsGoodToMe) { break; }
                    }
                    for (int i = 0; i < numberThrew; i++)
                    {
                        _dice[i].Throw();
                        roll += _dice[i].Face;
                    }
                    continueThrowingDie = false;
                    foreach (Monument monument in _monuments)
                    {
                        if (!continueThrowingDie)
                        {
                            _currentThrow.Clear();
                            for (int i = 0; i < numberThrew; i++) { _currentThrow.Add(_dice[i].Face); }
                            continueThrowingDie = monument.CanResetDiceThrow(_currentThrow, roll);
                        }
                    }
                    if (!continueThrowingDie) { break; }
                }
            }

            foreach (Monument monument in _monuments)
            {
                if (this._dice.Count == 1) { monument.CheckIfTurnWillRepeat(new List<int>() { _dice[0].Face }); }
                else
                {
                    List<int> listToInput = new List<int>();
                    for (int i = 0; i < numberThrew; i++) { listToInput.Add(_dice[i].Face); }
                    monument.CheckIfTurnWillRepeat(listToInput);
                }

            }
            */
            Random rnd = new Random();
            int a = rnd.Next(1, 7);
            return a;
        }
        public bool Win()
        {
            int builtMonumentsCount = 0;

            foreach (bool b in monuments)
            {
                if (b)
                {
                    builtMonumentsCount++;
                }
            }
            return builtMonumentsCount == monuments.Length;
        }
    }
}
