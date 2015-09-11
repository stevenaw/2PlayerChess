using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChessGameLib.Test
{
    /// <summary>
    /// Summary description for BitBoardTest
    /// </summary>
    [TestClass]
    public class BitBoardTest
    {
        public BitBoardTest()
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
        public void TestCtorEmpty()
        {
            BitBoard expected = new BitBoard(0);
            BitBoard actual = new BitBoard();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestCtorPopulated()
        {
            BitBoard expected = new BitBoard(1);
            BitBoard actual = new BitBoard();

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestAddPositionCoord()
        {
            int row = 3;
            int column = 6;
            long expectedPos = 0x0000000040000000;

            BitBoard expected = new BitBoard(expectedPos);
            BitBoard actual = new BitBoard();

            actual.AddPosition(column, row);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestRemovePositionCoord()
        {
            int row = 3;
            int column = 6;
            long startPos = 0x0000000040000000;
            long expectedPos = 0;

            BitBoard expected = new BitBoard(expectedPos);
            BitBoard actual = new BitBoard(startPos);

            actual.RemovePosition(column, row);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestContainsPositionCoord()
        {
            int row = 3;
            int column = 6;
            long expectedPosLong = 0x0000000040000000;
            BitBoard actual = new BitBoard();

            actual.AddPosition(column, row);

            Assert.IsTrue(actual.Contains(column, row));
            Assert.IsTrue(actual.Contains(expectedPosLong));
            Assert.IsTrue(actual.Contains(new BoardSquare(column, row)));
        }
    }
}
