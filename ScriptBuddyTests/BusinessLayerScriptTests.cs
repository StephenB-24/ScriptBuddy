using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBuddy.BL;
using ScriptBuddy.Models;
using System;
using System.Linq;

namespace ScriptBuddyTests
{
    [TestClass]
    public class BusinessLayerScriptTests
    {
        [TestMethod]
        public void AScriptInsert()
        {
            IBusinessLayer businessLayer = new BusinessLayer();

            User user = businessLayer.GetAllUsers().FirstOrDefault();

            Script script = new Script()
            {
                Name = "My Test Script",
                Description = "Test Script Insertion",
                UserId = user.Id,
                CommunityTagId = 0,
                TimeLastSaved = DateTime.Now,
                Accessibility = true
            };

            Assert.IsTrue(businessLayer.InsertScript(script));
        }

        [TestMethod]
        public void BScriptInsertInvalidUserId()
        {
            IBusinessLayer businessLayer = new BusinessLayer();

            Script script = new Script()
            {
                Name = "My Test Script",
                Description = "Test Script Insertion",
                UserId = -1,
                CommunityTagId = 0,
                TimeLastSaved = DateTime.Now,
                Accessibility = true
            };

            bool insertSuccessful = false;
            try
            {
                insertSuccessful = businessLayer.InsertScript(script);
            }
            catch (Exception e)
            {
                insertSuccessful = false;
            }
            Assert.IsFalse(insertSuccessful);
        }

        [TestMethod]
        public void CScriptInsertInvalidNameLength()
        {
            IBusinessLayer businessLayer = new BusinessLayer();

            User user = businessLayer.GetAllUsers().FirstOrDefault();

            Script script = new Script()
            {
                Name = "My test name that is too long! My test name that is too long! My test name that is too long!" +
                " My test name that is too long! My test name that is too long! My test name that is too long!" +
                " My test name that is too long! My test name that is too long! My test name that is too long!" +
                " My test name that is too long! My test name that is too long! My test name that is too long!" +
                " My test name that is too long! My test name that is too long! My test name that is too long!" +
                " My test name that is too long! My test name that is too long! My test name that is too long!" +
                " My test name that is too long! My test name that is too long! My test name that is too long!" +
                " My test name that is too long! My test name that is too long! My test name that is too long!",
                Description = "Test Script Insertion",
                UserId = user.Id,
                CommunityTagId = 0,
                TimeLastSaved = DateTime.Now,
                Accessibility = true
            };

            bool insertSuccessful = false;
            try
            {
                insertSuccessful = businessLayer.InsertScript(script);
            }
            catch (Exception e)
            {
                insertSuccessful = false;
            }
            Assert.IsFalse(insertSuccessful);
        }

        [TestMethod]
        public void DScriptInsertInvalidDescriptionLength()
        {
            IBusinessLayer businessLayer = new BusinessLayer();

            User user = businessLayer.GetAllUsers().FirstOrDefault();

            Script script = new Script()
            {
                Name = "Test Insertion Name",
                Description = "Too long of a description, this should get rejected ! " +
                "Too long of a description, this should get rejected ! " +
                "Too long of a description, this should get rejected ! " +
                "Too long of a description, this should get rejected ! " +
                "Too long of a description, this should get rejected ! " +
                "Too long of a description, this should get rejected ! " +
                "Too long of a description, this should get rejected ! " +
                "Too long of a description, this should get rejected ! " +
                "Too long of a description, this should get rejected ! " +
                "Too long of a description, this should get rejected ! " +
                "Too long of a description, this should get rejected ! " +
                "Too long of a description, this should get rejected ! ",
                UserId = user.Id,
                CommunityTagId = 0,
                TimeLastSaved = DateTime.Now,
                Accessibility = true
            };

            bool insertSuccessful = false;
            try
            {
                insertSuccessful = businessLayer.InsertScript(script);
            }
            catch (Exception e)
            {
                insertSuccessful = false;
            }
            Assert.IsFalse(insertSuccessful);
        }

        [TestMethod]
        public void EGetScript()
        {
            IBusinessLayer businessLayer = new BusinessLayer();

            User user = businessLayer.GetAllUsers().FirstOrDefault();

            Script script = new Script()
            {
                Name = "My Test Script",
                UserId = user.Id,
                CommunityTagId = 0,
                TimeLastSaved = DateTime.Now,
                Accessibility = true,
                Description = "Test Script Insertion"
            };

            Assert.IsTrue(businessLayer.GetScript(script.UserId, script.Name) != null);
        }

        [TestMethod]
        public void FGetScriptInvalidUser()
        {
            IBusinessLayer businessLayer = new BusinessLayer();

            User user = businessLayer.GetAllUsers().FirstOrDefault();

            Script script = new Script()
            {
                Name = "My Test Script",
                UserId = -1,
            };

            Script retrievedScript = businessLayer.GetScript(script.UserId, script.Name);
            Assert.IsTrue(retrievedScript == null);
        }

        [TestMethod]
        public void GGetScriptInvalidScriptName()
        {
            IBusinessLayer businessLayer = new BusinessLayer();

            User user = businessLayer.GetAllUsers().FirstOrDefault();

            Script script = new Script()
            {
                Name = "",
                UserId = user.Id,
            };

            Script retrievedScript = businessLayer.GetScript(user.Id, script.Name);
            Assert.IsTrue(retrievedScript == null);
        }

        [TestMethod]
        public void HGetScriptInvalidId()
        {
            IBusinessLayer businessLayer = new BusinessLayer();

            Script retrievedScript = businessLayer.GetScript(-1);
            Assert.IsTrue(retrievedScript == null);
        }

        [TestMethod]
        public void IDeleteScript()
        {
            IBusinessLayer businessLayer = new BusinessLayer();
            User user = businessLayer.GetAllUsers().FirstOrDefault();

            Script script = new Script()
            {
                Name = "My Test Script",
                Description = "Test Script Insertion",
                UserId = user.Id,
                CommunityTagId = 0,
                TimeLastSaved = DateTime.Now,
                Accessibility = true
            };

            Script retrievedScript = businessLayer.GetScript(user.Id, script.Name);

            Assert.IsTrue(businessLayer.DeleteScript(retrievedScript.Id));
        }

        [TestMethod]
        public void JDeleteNonExistingScript()
        {
            IBusinessLayer businessLayer = new BusinessLayer();

            Assert.IsTrue(businessLayer.DeleteScript(-1));
        }
    }
}