﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Adv;
using AdvLib.Tests.Generators;
using AdvLib.Tests.Util;
using NUnit.Framework;

namespace AdvLib.Tests.Adv_V2
{
    [TestFixture]
    public class TestV2FileGeneration
    {
        [Test]
        public void SimpleTest()
        {
            var fileGen = new AdvGenerator();
            var cfg = new AdvGenerationConfig()
            {
                CameraDepth = 16,
                DynaBits = 16,
                SourceFormat = AdvSourceDataFormat.Format16BitUShort,
                NumberOfFrames = 10,
                UsesCompression = false,
                NormalPixelValue = null
            };

            string fileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            if (File.Exists(fileName)) File.Delete(fileName);
            AdvFile2 file = null;
            try
            {
                // Generate
                fileGen.GenerateaAdv_V2(cfg, fileName);

                // Verify
                file = new AdvFile2(fileName);
                
            }
            finally
            {
                try
                {
                    if (file != null) file.Close();
                    if (File.Exists(fileName))
                        File.Delete(fileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Trace.WriteLine(ex);
                }
            }
        }

        [Test]
        [TestCase(@"F:\WORK\ADVVer2\ADVLib\AdvLib.Tests\TestFiles\UNCOMPRESSED\TestFile.Win32.GNU.adv")]
        [TestCase(@"F:\WORK\ADVVer2\ADVLib\AdvLib.Tests\TestFiles\UNCOMPRESSED\TestFile.Win64.GNU.adv")]
        [TestCase(@"F:\WORK\ADVVer2\ADVLib\AdvLib.Tests\TestFiles\UNCOMPRESSED\TestFile.Win32.MSVC.adv")]
        [TestCase(@"F:\WORK\ADVVer2\ADVLib\AdvLib.Tests\TestFiles\UNCOMPRESSED\TestFile.Win64.MSVC.adv")]
        public void ReadLinuxFile1(string fileName)
        {
            var hasher = new Hasher();
            string h1 = hasher.CalcMd5(fileName);
            Console.WriteLine(h1);

            var file = new AdvFile2(fileName);
            Console.WriteLine("MainSteamInfo.FrameCount: " + file.MainSteamInfo.FrameCount);
            Console.WriteLine("CalibrationSteamInfo.FrameCount: " + file.CalibrationSteamInfo.FrameCount);
        }

        [Test]
        public void CompareFiles()
        {
            var hasher = new Hasher();
            string h1 = hasher.CalcMd5(@"F:\WORK\ADVVer2\ADVLib\AdvLib.Tests\TestFiles\UNCOMPRESSED\TestFile.Win32.GNU.adv");
            string h2 = hasher.CalcMd5(@"F:\WORK\ADVVer2\ADVLib\AdvLib.Tests\TestFiles\UNCOMPRESSED\TestFile.Win32.MSVC.adv");
            string h3 = hasher.CalcMd5(@"F:\WORK\ADVVer2\ADVLib\AdvLib.Tests\TestFiles\UNCOMPRESSED\TestFile.Win64.GNU.adv");
            string h4 = hasher.CalcMd5(@"F:\WORK\ADVVer2\ADVLib\AdvLib.Tests\TestFiles\UNCOMPRESSED\TestFile.Win64.MSVC.adv");
            Console.WriteLine(h1);
            Console.WriteLine(h2);
            Console.WriteLine(h3);
            Console.WriteLine(h4);
            Assert.AreEqual(h1, h2);
            Assert.AreEqual(h1, h3);
            Assert.AreEqual(h1, h4);
        }

    }
}
