using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkjaTextar.Helpers;
using SkjaTextar.Models;
using SkjaTextar.Tests.Mocks;
using SkjaTextar.Controllers;
using System.Web.Mvc;
using System.Collections.Generic;
using SkjaTextar.Exceptions;
using System.Web;
using System.IO;

namespace SkjaTextar.Tests.Helpers
{
    [TestClass]
    public class SubtitleParserTest
    {
        [TestMethod]
        public void SubtitleParserTests()
        {
            var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            
            // This file contains a segment with invalid timestamp format
            try
            {
                string path = string.Format("{0}\\{1}", directory, "Content\\timestamperror.srt");
                var translation = SubtitleParser.Parse(path, "srt");
            }
            catch(Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(SubtitleParseException));
            }

            // This file contains 3 segments, the last one has id 9999 and should therefore be cut off
            string path2 = string.Format("{0}\\{1}", directory, "Content\\cutsOff.srt");
            var translation2 = SubtitleParser.Parse(path2, "srt");
            Assert.IsTrue(translation2.TranslationSegments.Count == 2);


            // This file contains a segment with invalid id
            try
            {
                string path = string.Format("{0}\\{1}", directory, "Content\\idError.srt");
                var translation = SubtitleParser.Parse(path, "srt");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(SubtitleParseException));
            }
        }
    }
}
