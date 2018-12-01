using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Rino.Forthic;

namespace Rino.ForthicTests
{
    [TestClass]
    public class TokenizerTest
    {
        [TestMethod]
        public void TestConstruction()
        {
            string input = "";
            Tokenizer tokenizer = new Tokenizer(input);
            Assert.IsNotNull(tokenizer);
        }

        [TestMethod]
        public void TestWhitespace()
        {
            string input = "       ()     \t\r\n       ";
            Tokenizer tokenizer = new Tokenizer(input);
            Token tok = tokenizer.NextToken();
            Assert.AreEqual(TokenType.EOS, tok.Type);
            Assert.AreEqual(TokenType.EOS, tok.Type);
        }

        [TestMethod]
        public void TestComment()
        {
            string input = "  #This is a comment";
            Tokenizer tokenizer = new Tokenizer(input);

            Token tok = tokenizer.NextToken();
            Assert.AreEqual(TokenType.COMMENT, tok.Type);

            tok = tokenizer.NextToken();
            Assert.AreEqual(TokenType.EOS, tok.Type);
        }

        [TestMethod]
        public void TestStartEndDefinition()
        {
            string input = ": DEF1 ;";
            Tokenizer tokenizer = new Tokenizer(input);

            StartDefinitionToken tok = (StartDefinitionToken)tokenizer.NextToken();
            Assert.AreEqual(TokenType.START_DEFINITION, tok.Type);
            Assert.AreEqual("DEF1", tok.Name);

            Token tok2 = tokenizer.NextToken();
            Assert.AreEqual(TokenType.END_DEFINITION, tok2.Type);
        }

        [TestMethod]
        public void TestStartEndArray()
        {
            string input = "[ ]";
            Tokenizer tokenizer = new Tokenizer(input);

            Token tok = tokenizer.NextToken();
            Assert.AreEqual(TokenType.START_ARRAY, tok.Type);

            tok = tokenizer.NextToken();
            Assert.AreEqual(TokenType.END_ARRAY, tok.Type);
        }

        [TestMethod]
        public void TestStartEndNamedModule()
        {
            string input = "{html }";
            Tokenizer tokenizer = new Tokenizer(input);

            StartModuleToken tok = (StartModuleToken)tokenizer.NextToken();
            Assert.AreEqual(TokenType.START_MODULE, tok.Type);
            Assert.AreEqual("html", tok.Name);

            Token tok2 = tokenizer.NextToken();
            Assert.AreEqual(TokenType.END_MODULE, tok2.Type);
        }

        [TestMethod]
        public void TestAnonymousModule()
        {
            string input = "{ }";
            Tokenizer tokenizer = new Tokenizer(input);

            StartModuleToken tok = (StartModuleToken)tokenizer.NextToken();
            Assert.AreEqual(TokenType.START_MODULE, tok.Type);
            Assert.AreEqual("", tok.Name);

            Token tok2 = tokenizer.NextToken();
            Assert.AreEqual(TokenType.END_MODULE, tok2.Type);
        }

        [TestMethod]
        public void TestIsTripleQuote()
        {
            Tokenizer t1 = new Tokenizer(@"'''Now'''");
            Assert.IsTrue(t1.IsTripleQuote(0, '\''));
            Assert.IsTrue(t1.IsTripleQuote(6, '\''));

            Tokenizer t2 = new Tokenizer(@"""""""Now""""""");
            Assert.IsTrue(t2.IsTripleQuote(0, '"'));
            Assert.IsTrue(t2.IsTripleQuote(6, '"'));
        }

        [TestMethod]
        public void TestTripleQuoteString()
        {
            string input = @"'''This is a ""triple-quoted"" string!'''";
            Tokenizer tokenizer = new Tokenizer(input);

            StringToken tok = (StringToken)tokenizer.NextToken();
            Assert.AreEqual(TokenType.STRING, tok.Type);
            Assert.AreEqual(@"This is a ""triple-quoted"" string!", tok.Text);
        }

        [TestMethod]
        public void TestString()
        {
            string input = @"'Single quote' ""Double quote""";
            Tokenizer tokenizer = new Tokenizer(input);

            StringToken tok = (StringToken)tokenizer.NextToken();
            Assert.AreEqual(TokenType.STRING, tok.Type);
            Assert.AreEqual(@"Single quote", tok.Text);

            tok = (StringToken)tokenizer.NextToken();
            Assert.AreEqual(@"Double quote", tok.Text);
        }

        [TestMethod]
        public void TestWord()
        {
            string input = "WORD1 WORD2";
            Tokenizer tokenizer = new Tokenizer(input);

            WordToken tok = (WordToken)tokenizer.NextToken();
            Assert.AreEqual(TokenType.WORD, tok.Type);
            Assert.AreEqual("WORD1", tok.Text);

            tok = (WordToken)tokenizer.NextToken();
            Assert.AreEqual(TokenType.WORD, tok.Type);
            Assert.AreEqual("WORD2", tok.Text);
        }
    }
}
