using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBuddy.Models;
using ScriptBuddy.BL;

namespace ScriptBuddyTests
{
    /// <summary>
    /// This test class tests all the methods related to the Action class.
    /// Alphabetical lettering is used so tests run sequentially.
    /// </summary>
    [TestClass]
    public class BusinessLayerActionTests
    {
        /// <summary>
        /// First ensure that inserting an action works.
        /// </summary>
        [TestMethod]
        public void AInsertAction()
        {
            Action action = new Action() { ActionPosition = 1, ScriptId = 1, ActionTypeId = 1 };

            IBusinessLayer businessLayer = new BusinessLayer();

            Assert.IsTrue(businessLayer.InsertAction(action));
        }

        /// <summary>
        /// Next, attempt to insert an action with the same position in the script sequence (expected error).
        /// </summary>
        [TestMethod]
        public void BInsertActionDuplicatePosition()
        {
            Action action = new Action() { ActionPosition = 1, ScriptId = 1, ActionTypeId = 1 };

            IBusinessLayer businessLayer = new BusinessLayer();

            bool successfullyInserted = false;
            try
            {
                successfullyInserted = businessLayer.InsertAction(action);
            }
            catch (System.Exception)
            {
                successfullyInserted = false;
            }

            Assert.IsFalse(successfullyInserted);
        }

        /// <summary>
        /// Next, attempt to insert an action a script id that does not exist (expected error).
        /// </summary>
        [TestMethod]
        public void CInsertActionInvalidScriptId()
        {
            Action action = new Action() { ActionPosition = 1, ScriptId = -1, ActionTypeId = 1 };

            IBusinessLayer businessLayer = new BusinessLayer();

            bool successfullyInserted = false;
            try
            {
                successfullyInserted = businessLayer.InsertAction(action);
            }
            catch (System.Exception)
            {
                successfullyInserted = false;
            }
            Assert.IsFalse(successfullyInserted);
        }

        /// <summary>
        /// Trying to break insertion by inserting a action that has nothing filled in (expected error).
        /// </summary>
        [TestMethod]
        public void DInsertActionBlank()
        {
            Action action = new Action() { };

            IBusinessLayer businessLayer = new BusinessLayer();

            bool successfullyInserted = false;
            try
            {
                successfullyInserted = businessLayer.InsertAction(action);
            }
            catch (System.Exception)
            {
                successfullyInserted = false;
            }
            Assert.IsFalse(successfullyInserted);
        }

        /// <summary>
        /// Gets the action by scriptId and actionPosition.
        /// </summary>
        [TestMethod]
        public void EGetExistingAction()
        {
            Action action = new Action() { ActionPosition = 1, ScriptId = 1, ActionTypeId = 1 };

            IBusinessLayer businessLayer = new BusinessLayer();

            Action retrievedAction = businessLayer.GetAction(action.ScriptId, action.ActionPosition);

            Assert.IsTrue(retrievedAction != null);
        }

        /// <summary>
        /// Gets an action that does not exist. This should return null.
        /// </summary>
        [TestMethod]
        public void FGetNonExistingAction()
        {
            Action action = new Action() { ActionPosition = 1, ScriptId = -1, ActionTypeId = 1 };

            IBusinessLayer businessLayer = new BusinessLayer();

            Action retrievedAction = businessLayer.GetAction(action.ScriptId, action.ActionPosition);

            Assert.IsTrue(retrievedAction == null);
        }

        [TestMethod]
        public void GDeleteExistingAction()
        {
            Action action = new Action() { ActionPosition = 1, ScriptId = 1, ActionTypeId = 1 };

            IBusinessLayer businessLayer = new BusinessLayer();

            Action retrievedAction = businessLayer.GetAction(action.ScriptId, action.ActionPosition);

            Assert.IsTrue(businessLayer.DeleteAction(retrievedAction.Id));
        }

        /// <summary>
        /// Deletes an action that does not exist. This should return true, because the action is not found
        /// and stops operation early.
        /// </summary>
        [TestMethod]
        public void HDeleteNonExistingAction()
        {
            IBusinessLayer businessLayer = new BusinessLayer();

            Assert.IsTrue(businessLayer.DeleteAction(-1));
        }
    }
}
