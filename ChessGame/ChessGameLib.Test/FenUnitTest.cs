using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChessGameLib.Test
{
    /// <summary>
    /// Summary description for FenUnitTest
    /// </summary>
    [TestClass]
    public class FenUnitTest
    {
        public FenUnitTest()
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
        public void TestFromFenValid()
        {
            const string FEN_STRING = "pQ4N1/3k3R/1r4n1/KbBbBppP/8/8/q7/7n";
            FENSerializer ser = new FENSerializer();
            Board expected = new Board(null);
            Board actual;
            
            // Setup
            expected['8', 'A'] = new Pawn('b');
            expected['8', 'B'] = new Queen('w');
            expected['8', 'G'] = new Knight('w');

            expected['7', 'D'] = new King('b');
            expected['7', 'H'] = new Rook('w');

            expected['6', 'B'] = new Rook('b');
            expected['6', 'G'] = new Knight('b');

            expected['5', 'A'] = new King('w');
            expected['5', 'B'] = new Bishop('b');
            expected['5', 'C'] = new Bishop('w');
            expected['5', 'D'] = new Bishop('b');
            expected['5', 'E'] = new Bishop('w');
            expected['5', 'F'] = new Pawn('b');
            expected['5', 'G'] = new Pawn('b');
            expected['5', 'H'] = new Pawn('w');

            expected['2', 'A'] = new Queen('b');
            expected['1', 'H'] = new Knight('b');

            // Act
            actual = ser.Deserialize(FEN_STRING);

            // Test
            for (int file = 0; file < Board.NUM_FILES; file++)
            {
                for (int row = 0; row < Board.NUM_ROWS; row++)
                {
                    Piece expectedPiece = expected[row, file];
                    Piece actualPiece = actual[row, file];
                    bool areEqual = Piece.AreEqual(expectedPiece, actualPiece);

                    Assert.IsTrue(areEqual);
                }
            }
        }

        [TestMethod]
        public void TestInvalidTooFewRows()
        {
            const string FEN_STRING = "8/8/8/8/8/8/q7";
            const string EXPECTED_MSG = "Not enough rows specified!";
            const int EXPECTED_INDEX = -1;

            TestInvalidFen(FEN_STRING, EXPECTED_MSG, EXPECTED_INDEX);
        }

        [TestMethod]
        public void TestInvalidTooManyRows()
        {
            const string FEN_STRING = "8/8/8/8/8/8/q7/8/8";
            const string EXPECTED_MSG = "Too many rows!";

            TestInvalidFen(FEN_STRING, EXPECTED_MSG, FEN_STRING.Length-2);
        }

        [TestMethod]
        public void TestInvalidEndInDelimTooMany()
        {
            const string FEN_STRING = "8/8/8/8/8/8/q7/8/";
            const string EXPECTED_MSG = "Too many rows!";

            TestInvalidFen(FEN_STRING, EXPECTED_MSG, FEN_STRING.Length-1);
        }

        [TestMethod]
        public void TestInvalidEndInDelimTooFew()
        {
            const string FEN_STRING = "8/8/8/8/8/8/q7/";
            const string EXPECTED_MSG = "Not enough columns specified!";
            const int EXPECTED_INDEX = -1;

            TestInvalidFen(FEN_STRING, EXPECTED_MSG, EXPECTED_INDEX);
        }

        [TestMethod]
        public void TestInvalidTooFewColPieces()
        {
            const string FEN_STRING = "8/8/8/8/8/8/bbbbbbb/8";
            const string EXPECTED_MSG = "Unanticipated new row, columns to fill!";

            TestInvalidFen(FEN_STRING, EXPECTED_MSG, FEN_STRING.Length-2);
        }

        [TestMethod]
        public void TestInvalidTooFewColEmpty()
        {
            const string FEN_STRING = "8/8/8/8/8/8/7/8";
            const string EXPECTED_MSG = "Unanticipated new row, columns to fill!";

            TestInvalidFen(FEN_STRING, EXPECTED_MSG, FEN_STRING.Length-2);
        }

        [TestMethod]
        public void TestInvalidTooManyColEmpty()
        {
            const string FEN_STRING = "8/8/8/8/8/8/9/8";
            const string EXPECTED_MSG = "Blank spot exceeds column count!";
            const int EXPECTED_INDEX = 12;

            TestInvalidFen(FEN_STRING, EXPECTED_MSG, EXPECTED_INDEX);
        }

        [TestMethod]
        public void TestInvalidTooManyColPieces()
        {
            const string FEN_STRING = "8/8/8/8/8/8/bbbbbbbbb/8";
            const string EXPECTED_MSG = "Too many pieces specified in row!";
            const int EXPECTED_INDEX = 20;

            TestInvalidFen(FEN_STRING, EXPECTED_MSG, EXPECTED_INDEX);
        }

        [TestMethod]
        public void TestInvalidEmpty()
        {
            string FEN_STRING = String.Empty;
            const string EXPECTED_MSG = "String can not be empty!";
            const int EXPECTED_INDEX = -1;

            TestInvalidFen(FEN_STRING, EXPECTED_MSG, EXPECTED_INDEX);
        }

        [TestMethod]
        public void TestInvalidZeroGap()
        {
            const string FEN_STRING = "8/8/8/8/8/8/bb0bbbbb/8";
            const string EXPECTED_MSG = "Can not have a 0 in a FEN diagram!";
            const int EXPECTED_INDEX = 14;

            TestInvalidFen(FEN_STRING, EXPECTED_MSG, EXPECTED_INDEX);
        }

        [TestMethod]
        public void TestInvalidConsecutiveGaps()
        {
            const string FEN_STRING = "8/8/8/8/8/8/bb11bbbb/8";
            const string EXPECTED_MSG = "Can not specify two consecutive blank spots!";
            const int EXPECTED_INDEX = 15;

            TestInvalidFen(FEN_STRING, EXPECTED_MSG, EXPECTED_INDEX);
        }

        [TestMethod]
        public void TestInvalidUnknownNotation()
        {
            const string FEN_STRING = "8/8/8/8/8/8/bb1cbbbb/8";
            const string EXPECTED_MSG = "Unsupported piece notation: c";
            const int EXPECTED_INDEX = 15;

            TestInvalidFen(FEN_STRING, EXPECTED_MSG, EXPECTED_INDEX);
        }

        private void TestInvalidFen(string fen, string expectedErrMsg, int expectedIndex)
        {
            //Setup
            FENSerializer ser = new FENSerializer();
            bool exceptionThrown = false;
            string actualErrMsg = String.Empty;
            int actualIndex = Int32.MinValue;

            try
            {
                // Act
                ser.Deserialize(fen);
            }
            catch (FENException e)
            {
                exceptionThrown = true;
                actualErrMsg = e.Message;
                actualIndex = e.ErrorIndex;
            }

            // Test
            Assert.IsTrue(exceptionThrown);
            Assert.AreEqual(expectedErrMsg, actualErrMsg);
            Assert.AreEqual(expectedIndex, actualIndex);
            Assert.AreEqual(exceptionThrown, !ser.IsValidSerialization(fen));
        }

    }
}
