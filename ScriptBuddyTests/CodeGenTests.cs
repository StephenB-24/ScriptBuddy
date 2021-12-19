//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using ScriptBuddy.BL.CodeGen.Models;
//using System.Collections.Generic;

//namespace ScriptBuddyTests
//{
//    /// <summary>
//    /// A collection of tests which focus on testing the Code Generator's ability to generate
//    /// valid AHK code.
//    /// </summary>
//    [TestClass]
//    public class CodeGenTests
//    {
//        /* Tests for the Click class */
//        [TestMethod]
//        public void TestClickA()
//        {
//            Click click = new Click(ClickType.Left);
//            Assert.AreEqual<string>("Click Left", click.GenerateCode());
//        }
//        [TestMethod]
//        public void TestClickB()
//        {
//            Click click = new Click(ClickType.Right);
//            Assert.AreEqual<string>("Click Right", click.GenerateCode());
//        }
//        [TestMethod]
//        public void TestClickC()
//        {
//            Click click = new Click(ClickType.Middle);
//            Assert.AreEqual<string>("Click Middle", click.GenerateCode());
//        }
//        [TestMethod]
//        public void TestClickD()
//        {
//            Click click = new Click(ClickType.M4);
//            Assert.AreEqual<string>("Click X1", click.GenerateCode());
//        }
//        [TestMethod]
//        public void TestClickE()
//        {
//            Click click = new Click(ClickType.M5);
//            Assert.AreEqual<string>("Click X2", click.GenerateCode());
//        }

//        /* A series of tests for the hotstring */
//        [TestMethod]
//        public void TestHotStringA()
//        {
//            HotString hs = new HotString("btw", "by the way");
//            Assert.AreEqual<string>("::btw::by the way", hs.GenerateCode());
//        }
//        [TestMethod]
//        public void TestHotStringB()
//        {
//            HotString hs = new HotString("", "");
//            Assert.AreEqual<string>("::::", hs.GenerateCode());
//        }
//        [TestMethod]
//        public void TestHotStringC()
//        {
//            HotString hs = new HotString("ASDASD", "ASDA22D2D2");
//            Assert.AreEqual<string>("::ASDASD::ASDA22D2D2", hs.GenerateCode());
//        }

//        /* TypeSequence Tests */
//        [TestMethod]
//        public void TestTypeSequenceA()
//        {
//            TypeSequence ts = new TypeSequence("");
//            Assert.AreEqual<string>("Send, ", ts.GenerateCode());
//        }
//        [TestMethod]
//        public void TestTypeSequenceB()
//        {
//            TypeSequence ts = new TypeSequence("AAAHHH");
//            Assert.AreEqual<string>("Send, AAAHHH", ts.GenerateCode());
//        }

//        /* Tests for the Script class overall */
//        [TestMethod]
//        public void TestCodeGenEmpty()
//        {
//            Script script = new Script(new List<IAction> { }, new List<ICodeExecutor> { });
//            Assert.AreEqual<string>("", script.GenerateCode());
//        }

//        [TestMethod]
//        public void TestCodeGenClicks()
//        {
//            List<IAction> startupSequence = new List<IAction>{
//                new Click(ClickType.Left),
//                new Click(ClickType.Right),
//                new Click(ClickType.Middle),
//                new Click(ClickType.M4),
//                new Click(ClickType.M5)
//            };


//            Script script = new Script(startupSequence, new List<ICodeExecutor> { });
//            string expected = 
//@"Click Left
//Click Right
//Click Middle
//Click X1
//Click X2
//".Replace("\r\n", "\n").Replace("    ", "\t");
//            Assert.AreEqual<string>(expected, script.GenerateCode());
//        }

//        [TestMethod]
//        public void TestCodeGenClicks2()
//        {
//            List<IAction> startupSequence = new List<IAction>{

//            };
//            List<ICodeExecutor> hotkeys = new List<ICodeExecutor> {
//                new SingleButtonHotkey(
//                    'a',
//                    new List<HotkeyPrefix>{ HotkeyPrefix.Alt, HotkeyPrefix.Ctrl, HotkeyPrefix.Shift},
//                    new List<IAction>{ 
//                        new TypeSequence("aaabbbccc"),
//                        new MoveMouse(50, 50),
//                        new Click(ClickType.Left)
//                        }
//                    )
//            };


//            Script script = new Script(startupSequence, hotkeys);
//            string expected =
//@"!^+a::
//    Send, aaabbbccc
//    MouseMove, 50, 50
//	Click Left
//    return
//".Replace("\r\n", "\n").Replace("    ", "\t");
//            string result = script.GenerateCode();
//            Assert.AreEqual<string>(expected, result);
//        }

//        [TestMethod]
//        public void TestCodeGenHotStrings()
//        {
//            List<IAction> startupSequence = new List<IAction> {};
//            List<ICodeExecutor> hotkeys = new List<ICodeExecutor> {
//                new HotString("btw", "by the way")
//            };


//            Script script = new Script(startupSequence, hotkeys);
//            string expected =
//@"::btw::by the way
//".Replace("\r\n", "\n").Replace("    ", "\t");
//            string result = script.GenerateCode();
//            Assert.AreEqual<string>(expected, result);
//        }
//    }
//}
