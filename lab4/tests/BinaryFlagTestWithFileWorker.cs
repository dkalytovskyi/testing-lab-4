using System.IO;
using NUnit.Framework;
using IIG.BinaryFlag;
using IIG.FileWorker;
namespace lab4.tests
{
    [TestFixture]
    public class BinaryFlagTestWithFileWorker
    {
        private static string writeFile = @"C:\Users\dkali\Desktop\KPI\4 курс\testing\testing-lab-4\lab4\txt_src\write.txt";
        private static string readLinesFile = @"C:\Users\dkali\Desktop\KPI\4 курс\testing\testing-lab-4\lab4\txt_src\read_lines.txt";
        private static string readAllFile = @"C:\Users\dkali\Desktop\KPI\4 курс\testing\testing-lab-4\lab4\txt_src\read_all.txt";
        private static string tryWriteFile = @"C:\Users\dkali\Desktop\KPI\4 курс\testing\testing-lab-4\lab4\txt_src\try_write.txt";

        [TearDown]
        public void Clear()
        {
            File.WriteAllText(writeFile, string.Empty);
            File.WriteAllText(tryWriteFile, string.Empty);

        }

        [Test]
        public void WriteGetFlag()
        {
            string expectedOutput = "True";

            MultipleBinaryFlag flag = new MultipleBinaryFlag(44);

            Assert.IsTrue(BaseFileWorker.Write(flag.GetFlag().ToString(), writeFile));
            Assert.AreEqual(expectedOutput, File.ReadAllText(writeFile));
        }

        [Test]
        public void TryWriteGetFlag()
        {

            string expectedOutput = "False";

            MultipleBinaryFlag flag = new MultipleBinaryFlag(4, false);
            Assert.IsTrue(BaseFileWorker.TryWrite(flag.GetFlag().ToString(), tryWriteFile, 3));
            Assert.AreEqual(expectedOutput, File.ReadAllText(tryWriteFile));
        }

        [Test]
        public void ReadLinesFlag()
        {
            MultipleBinaryFlag[] flags = { new MultipleBinaryFlag(10, true),
                new MultipleBinaryFlag(15, true),
                new MultipleBinaryFlag(100500) };

            flags[2].ResetFlag(1);

            //the output should be true, true, false

            string[] results = BaseFileWorker.ReadLines(readLinesFile);

            Assert.AreEqual(flags.Length, results.Length);

            for (int i = 0; i < flags.Length; i++)
            {
                Assert.AreEqual(flags[i].GetFlag().ToString(), results[i]);
            }
        }


        [Test]
        public void ReadLinesFlagNotEqual()
        {
            MultipleBinaryFlag[] flags = { new MultipleBinaryFlag(10, false),
               new MultipleBinaryFlag(15, false),
               new MultipleBinaryFlag(2)  };

            //the output should be false, false, true

            string[] results = BaseFileWorker.ReadLines(readLinesFile);

            Assert.AreEqual(flags.Length, results.Length);
            for (int i = 0; i < flags.Length; i++)
            {
                Assert.AreNotEqual(flags[i].GetFlag().ToString(), results[i]);
            }
        }

        [Test]
        public void ReadAllEqualGetFlag()
        {
            MultipleBinaryFlag flag = new MultipleBinaryFlag(2);
            Assert.AreEqual(flag.GetFlag().ToString(), BaseFileWorker.ReadAll(readAllFile));

        }

        [Test]
        public void ReadlAllNotEqualGetFlag()
        {
            MultipleBinaryFlag flag = new MultipleBinaryFlag(2);
            flag.ResetFlag(1);
            Assert.AreNotEqual(flag.GetFlag().ToString(), BaseFileWorker.ReadAll(readAllFile));
        }
    }
}
