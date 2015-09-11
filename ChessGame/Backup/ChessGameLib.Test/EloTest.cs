using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//TODO: Elo >= 2200
namespace ChessGameLib.Test
{
    /// <summary>
    /// Summary description for EloTest
    /// </summary>
    [TestClass]
    public class EloTest
    {
        public EloTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestEloWinEven()
        {
            int startRate = 1000;
            int otherRating = 1000;
            int expectedRating = 1016;
            double decision = 1.0;
            Player p = new Player("", startRate, 0, 0, 0, 1);

            p.CalcNewELO(otherRating, decision);

            Assert.AreEqual(expectedRating, p.Rating);
        }

        [TestMethod]
        public void TestEloLoseEven()
        {
            int startRate = 1000;
            int otherRating = 1000;
            int expectedRating = 984;
            double decision = 0;
            Player p = new Player("", startRate, 0, 0, 0, 1);

            p.CalcNewELO(otherRating, decision);

            Assert.AreEqual(expectedRating, p.Rating);
        }

        [TestMethod]
        public void TestEloDrawEven()
        {
            int startRate = 1000;
            int otherRating = 1000;
            int expectedRating = 1000;
            double decision = 0.5;
            Player p = new Player("", startRate, 0, 0, 0, 1);

            p.CalcNewELO(otherRating, decision);

            Assert.AreEqual(expectedRating, p.Rating);
        }

        [TestMethod]
        public void TestEloDrawHigher()
        {
            int startRate = 1000;
            int otherRating = 600;
            int expectedRating = 987;
            double decision = 0.5;
            Player p = new Player("", startRate, 0, 0, 0, 1);

            p.CalcNewELO(otherRating, decision);

            Assert.AreEqual(expectedRating, p.Rating);
        }

        [TestMethod]
        public void TestEloWinHigher()
        {
            int startRate = 1000;
            int otherRating = 600;
            int expectedRating = 1003;
            double decision = 1.0;
            Player p = new Player("", startRate, 0, 0, 0, 1);

            p.CalcNewELO(otherRating, decision);

            Assert.AreEqual(expectedRating, p.Rating);
        }

        [TestMethod]
        public void TestEloLoseHigher()
        {
            int startRate = 1000;
            int otherRating = 600;
            int expectedRating = 971;
            double decision = 0;
            Player p = new Player("", startRate, 0, 0, 0, 1);

            p.CalcNewELO(otherRating, decision);

            Assert.AreEqual(expectedRating, p.Rating);
        }

        [TestMethod]
        public void TestEloDrawLower()
        {
            int startRate = 600;
            int otherRating = 1000;
            int expectedRating = 613;
            double decision = 0.5;
            Player p = new Player("", startRate, 0, 0, 0, 1);

            p.CalcNewELO(otherRating, decision);

            Assert.AreEqual(expectedRating, p.Rating);
        }

        [TestMethod]
        public void TestEloWinLower()
        {
            int startRate = 600;
            int otherRating = 1000;
            int expectedRating = 629;
            double decision = 1.0;
            Player p = new Player("", startRate, 0, 0, 0, 1);

            p.CalcNewELO(otherRating, decision);

            Assert.AreEqual(expectedRating, p.Rating);
        }

        [TestMethod]
        public void TestEloLoseLower()
        {
            int startRate = 600;
            int otherRating = 1000;
            int expectedRating = 597;
            double decision = 0;
            Player p = new Player("", startRate, 0, 0, 0, 1);

            p.CalcNewELO(otherRating, decision);

            Assert.AreEqual(expectedRating, p.Rating);
        }

        [TestMethod]
        public void TestEloModDraw()
        {
            Player p = new Player("", 1000, 0, 0, 0, 1);
            double decision = 0.5;

            p.CalcNewELO(1000, decision);

            TestWLD(p, 0, 0, 1);
        }

        [TestMethod]
        public void TestEloModLoss()
        {
            Player p = new Player("", 1000, 0, 0, 0, 1);
            double decision = 0.0;

            p.CalcNewELO(1000, decision);

            TestWLD(p, 0, 1, 0);
        }

        [TestMethod]
        public void TestEloModWin()
        {
            Player p = new Player("", 1000, 0, 0, 0, 1);
            double decision = 1.0;

            p.CalcNewELO(1000, decision);

            TestWLD(p, 1, 0, 0);
        }

        private void TestWLD(Player p, int w, int l, int d)
        {
            Assert.AreEqual(l, p.Losses);
            Assert.AreEqual(w, p.Wins);
            Assert.AreEqual(d, p.Draws);
        }
    }
}
