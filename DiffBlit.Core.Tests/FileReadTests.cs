﻿using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DiffBlit.Core.Extensions;
using DiffBlit.Core.IO;
using DiffBlit.Core.Utilities;

namespace DiffBlit.Core.Tests
{
    [TestClass]
    public class FileReadTests
    {
        [TestMethod]
        public void LocalFileReadTest()
        {
            ReadOnlyFile localFile = new ReadOnlyFile(@"C:\Windows\notepad.exe");
            using (ReadOnlyStream s = localFile.Open())
            {
                byte[] data = s.ReadAllBytes();
                Assert.IsTrue(s.Length > 0);
                Assert.AreEqual(data.Length, s.Length);
                Assert.IsFalse(s.CanWrite);

                var temp = Utility.GetTempFilePath();
                localFile.Copy(temp);
                Assert.IsTrue(data.IsEqual(File.ReadAllBytes(temp)));
                File.Delete(temp);
            }
        }

        [TestMethod]
        public void WebFileReadTest()
        {
            ReadOnlyFile webFile = new ReadOnlyFile("http://speedtest.tele2.net/1KB.zip");
            using (ReadOnlyStream s = webFile.Open())
            using (StreamReader sr = new StreamReader(s))
            {
                byte[] data = s.ReadAllBytes();
                Assert.AreEqual(data.Length, 0x400);
                Assert.IsFalse(s.CanWrite);

                var temp = Utility.GetTempFilePath();
                webFile.Copy(temp);
                Assert.IsTrue(data.IsEqual(File.ReadAllBytes(temp)));
                File.Delete(temp);
            }
        }

        [TestMethod]
        public void FtpFileReadTest()
        {
            ReadOnlyFile ftpFile = new ReadOnlyFile("ftp://speedtest.tele2.net/1KB.zip");
            using (ReadOnlyStream s = ftpFile.Open())
            {
                byte[] data = s.ReadAllBytes();
                Assert.AreEqual(data.Length, 0x400);
                Assert.IsFalse(s.CanWrite);

                var temp = Utility.GetTempFilePath();
                ftpFile.Copy(temp);
                Assert.IsTrue(data.IsEqual(File.ReadAllBytes(temp)));
                File.Delete(temp);
            }
        }
    }
}
