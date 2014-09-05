using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SingleRoll_One_ScoreHundred()
        {
            var diceRoll = new DiceRoll().Roll(new Dice(1),
                 new Dice(2), new Dice(3),
                 new Dice(4), new Dice(4));
            var game = new Game();
            Assert.AreEqual(100, game.Score(diceRoll));
        }
        [TestMethod]
        public void SingleRoll_Five_ScoreFifty()
        {
            var dices = new DiceRoll().Roll(new Dice(5),
             new Dice(2), new Dice(3),
             new Dice(4), new Dice(4));

            var game = new Game();
            Assert.AreEqual(50, game.Score(dices));
        }
        [TestMethod]
        public void SingleRoll_DoubleOne_Score200()
        {
            var dices = new DiceRoll().Roll(new Dice(1),
           new Dice(1), new Dice(3),
           new Dice(4), new Dice(4));

            var game = new Game();
            Assert.AreEqual(200, game.Score(dices));
        }
        [TestMethod]
        public void SingleRoll_TripleOne_Score1000()
        {
            var dices = new DiceRoll().Roll(new Dice(1),
           new Dice(1), new Dice(1),
           new Dice(4), new Dice(4));

            var game = new Game();
            Assert.AreEqual(1000, game.Score(dices));
        }
        [TestMethod]
        public void SingleRoll_Triple5_Score1000()
        {
            var dices = new DiceRoll().Roll(new Dice(5),
             new Dice(5), new Dice(5),
             new Dice(4), new Dice(4));

            var game = new Game();
            Assert.AreEqual(500, game.Score(dices));
        }

        [TestMethod]
        public void SingleRoll_12513_Score250()
        {
            var dices = new DiceRoll().Roll(new Dice(1),
             new Dice(2), new Dice(5),
             new Dice(1), new Dice(3));

            var game = new Game();
            Assert.AreEqual(250, game.Score(dices));

        }
        [TestMethod]
        public void SingleRoll_Triple6_Score600()
        {
            var dices = new DiceRoll().Roll(new Dice(6),
             new Dice(6), new Dice(6),
             new Dice(1), new Dice(3));

            var game = new Game();
            Assert.AreEqual(600, game.Score(dices));

        }
        [TestMethod]
        public void SingleRoll_FourSix_Score1200()
        {
            var dices = new DiceRoll().Roll(new Dice(6),
             new Dice(6), new Dice(6),
             new Dice(6), new Dice(3));

            var game = new Game();
            Assert.AreEqual(1200, game.Score(dices));

        }
        [TestMethod]
        public void SingleRoll_FiveSix_Score2400()
        {
            var dices = new DiceRoll().Roll(new Dice(6),
             new Dice(6), new Dice(6),
             new Dice(6), new Dice(6));

            var game = new Game();
            Assert.AreEqual(2400, game.Score(dices));

        }
        [TestMethod]
        public void SingleRoll_SixSix_Score2400()
        {
            var dices = new DiceRoll().Roll(
                         new Dice(6),
                         new Dice(6),
                         new Dice(6),
                         new Dice(6),
                         new Dice(6),
                         new Dice(6));

            var game = new Game();
            Assert.AreEqual(4800, game.Score(dices));

        }
        [TestMethod]
        public void SingleRoll_ThreePairsOfTwo_Score800()
        {
            var dices = new DiceRoll().Roll(new Dice(6),
             new Dice(6), new Dice(3),
             new Dice(3), new Dice(5), new Dice(5));

            var game = new Game();
            Assert.AreEqual(800, game.Score(dices));

        }
        [TestMethod]
        public void SingleRoll_StraightSequence_Score1200()
        {
            var dices = new DiceRoll().Roll(new Dice(2),
             new Dice(3), new Dice(1),
             new Dice(4), new Dice(5), new Dice(6));

            var game = new Game();
            Assert.AreEqual(1200, game.Score(dices));

        }
    }

    public class DiceRoll
    {

        public List<Dice> Roll(Dice _dice1, Dice _dice2, Dice _dice3, Dice _dice4, Dice _dice5, Dice _dice6 = null)
        {
            var Dice = new List<Dice>();
            Dice.Add(_dice1);
            Dice.Add(_dice2);
            Dice.Add(_dice3);
            Dice.Add(_dice4);
            Dice.Add(_dice5);
            if (_dice6 != null)
            {
                Dice.Add(_dice6);
            }
            return Dice;
        }

    }
    public class Game
    {
        private Dictionary<Dice, Value> ScoreLookup;
        public Game()
        {
            ScoreLookup = new Dictionary<Dice, Value>(new Dice.EqualityComparer());
            ScoreLookup.Add(new Dice(1), new Value(100, 1000));
            ScoreLookup.Add(new Dice(2), new Value(0, 200));
            ScoreLookup.Add(new Dice(3), new Value(0, 300));
            ScoreLookup.Add(new Dice(4), new Value(0, 400));
            ScoreLookup.Add(new Dice(5), new Value(50, 500));
            ScoreLookup.Add(new Dice(6), new Value(0, 600));
        }


        internal int Score(List<Dice> dice)
        {
            if (CheckForThreePairs(dice))
            {
                return 800;
            }
            if (CheckForSequence(dice))
            {
                return 1200;
            }
            foreach (var outcome in ScoreLookup)
            {
                int score = dice.Where(i => i.Equals(outcome.Key)).Count();
                switch (score)
                {
                    case 3:
                        return ScoreThreeOfAKind(outcome);
                    case 4:
                        return ScoreFourOfAKind(outcome);
                    case 5:
                        return ScoreFiveOfAKind(outcome);
                    case 6:
                        return ScoreSixOfAKind(outcome);

                }

            }
            return CalculateOtherScores(dice);


        }

        private bool CheckForSequence(List<Dice> dice)
        {
            var x = new Dice(1);
            foreach (var item in dice.OrderBy(i => i))
            {
                if (!item.Equals( x))
                {
                    return false;
                }
                x=x.Add(new Dice(1));
            }
            return true ;
        }

        private int ScoreSixOfAKind(KeyValuePair<Dice, Value> outcome)
        {
            return ScoreThreeOfAKind(outcome) * 8;
        }

        private int ScoreFiveOfAKind(KeyValuePair<Dice, Value> outcome)
        {
            return ScoreThreeOfAKind(outcome) * 4;
        }

        private int ScoreFourOfAKind(KeyValuePair<Dice, Value> outcome)
        {
            return ScoreThreeOfAKind(outcome) * 2;
        }

        private bool CheckForThreePairs(List<Dice> dice)
        {
            return dice.GroupBy(i => i).Where(x => x.Count() == 2).Count() == 3;
        }

        private int ScoreThreeOfAKind(KeyValuePair<Dice, Value> outcome)
        {
            return outcome.Value.TripleScore;
        }



        private int CalculateOtherScores(List<Dice> dice)
        {
            int total = 0;
            foreach (var item in dice)
            {

                total += ScoreLookup[item].Score;

            }
            return total;
        }
    }
    public class Dice : IComparable
    {
        public Dice(int roll)
        {
            _roll = roll;
        }
        private int _roll;

        public override bool Equals(object obj)
        {
            var diceObjectToCompare = obj as Dice;

            if (diceObjectToCompare._roll == this._roll)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return 0;
        }
        public class EqualityComparer : IEqualityComparer<Dice>
        {
            public bool Equals(Dice x, Dice y)
            {
                if (x._roll == y._roll)
                    return true;
                else
                    return false;
            }

            public int GetHashCode(Dice obj)
            {
                return 0;
            }
        }

        public int CompareTo(object obj)
        {
            var diceToCompare = obj as Dice;
            if (this._roll >= diceToCompare._roll)
            {
                return 1;
            }
            else if (this._roll.Equals(diceToCompare._roll))
            {
                return 0;
            }
            else
                return -1;
        }

        internal Dice Add(Dice dice)
        {
            return new Dice(this._roll + dice._roll);
        }
    }

    public class Value
    {
        public Value(int score, int tripleScore)
        {
            Score = score;
            TripleScore = tripleScore;
        }
        public int Score { get; private set; }
        public int TripleScore { get; private set; }
    }
}
