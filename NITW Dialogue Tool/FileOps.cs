using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Collections.Generic;
using Microsoft.VisualBasic.Devices;

namespace FileOps
{
    /// <summary>
    /// Why should the VB coders get all the easy one-line shortcuts?
    /// </summary>
    public static class My
    {
        public static Microsoft.VisualBasic.Devices.Audio Audio = new Audio();
        public static Microsoft.VisualBasic.Devices.Clock Clock = new Clock();
        public static Microsoft.VisualBasic.Devices.Computer Computer = new Computer();
        public static Microsoft.VisualBasic.Devices.ComputerInfo ComputerInfo = new ComputerInfo();
        public static Microsoft.VisualBasic.Devices.Keyboard Keyboard = new Keyboard();
        public static Microsoft.VisualBasic.Devices.Mouse Mouse = new Mouse();
        public static Microsoft.VisualBasic.Devices.Network Network = new Network();
        public static Microsoft.VisualBasic.Devices.Ports Ports = new Ports();
        public static Microsoft.VisualBasic.Devices.ServerComputer ServerComputer = new ServerComputer();
    }

    [Flags]
    internal enum InsertStrategies
    {
        UseTargetAsTempFile = 1,
        UseTransposeReverse = 2,
        UseTransposeForward = 4
    }

    class TestCases
    {
        #region referenceMethods
        private const int KB = 1024;
        private const int MB = 1024 * 1024;
        private const int GB = 1024 * 1024 * 1024;

        public static long milliseconds
        {
            get
            {
                return (long)(DateTime.Now.Ticks * 0.0001);
            }
        }
        public static string GetFriendlyFilesizeString(long i) // i == filesize
        {
            double di = (double)i;
            double dKB = (double)KB;
            double dMB = (double)MB;
            double dGB = (double)GB;

            string numString = "";
            numString = (i >= GB) ?
                string.Format("{0:n}{1}", Math.Round((di / dGB), 2), " GB") :
                (i < GB && i >= MB) ?
                string.Format("{0:n}{1}", Math.Round((di / dMB), 2), " MB") :
                (i < MB && i >= KB) ?
                string.Format("{0:n}{1}", Math.Round((di / dKB), 2), " KB") :
                string.Format("{0:n}{1}", i, " B");
            return numString.PadLeft(9);
        }
        public static long AvailableMemory
        {
            get { return (int)My.ComputerInfo.AvailablePhysicalMemory; }
        }

        #endregion

        public static void RunDemo()
        {
            // FIRST ARE THE SHORT DEMOS TO SHOW THE BASIC TECHNIQUES:
            Console.WriteLine("Started at " + DateTime.Now.ToShortTimeString());
            Console.WriteLine();
            Console.WriteLine("Press Enter to start simple demos...");
            Console.ReadLine();
            
            RunSimpleDemo();

            // If desired, compare the performance of several different read/write buffer sizes:
            Console.Write("\nRun long performance tests? This can take many minutes... (Y/N): ");
            if (Console.ReadLine().Trim().ToUpper() == "Y")
            {
                RunPerformanceTests();
            }
        }

        static void RunSimpleDemo()
        {
            // Create a 128kb test file & hide a banana near the end:
            string randomFile = @"c:\randomtestfile-" + DateTime.Today.DayOfYear.ToString() + ".txt";
            FileOps.TestCases.MakeRandomTestFile(
                randomFile,
                1024 * 128,
                "banana",
                1024 * 100,
                1024 * 64);
            Console.WriteLine("Test file created, starting Find operation...");

            // See how int it takes to find the banana in the file:
            long start;
            string msg;
            long end;
            using (FileStream fs
                = new FileStream
                    (randomFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                start = milliseconds;
                msg = string.Format("banana found at position: {0:n}", FileOps.FileHelper.Find("banana", fs).ToString());
                end = milliseconds;
                fs.Close();
                Console.WriteLine(msg + "\n"
                    + "Took " + (end - start).ToString() + " milliseconds to find banana");
            }

            // Insert a coconut towards the beginning of the file:
            byte[] bytesToInsert =
                System.Text.Encoding.ASCII.GetBytes("coconut");
            start = milliseconds;
            FileOps.FileHelper.InsertBytes(bytesToInsert, randomFile, 1024 * 4);
            end = milliseconds;
            Console.WriteLine(
                    "Took " + (end - start).ToString() + " milliseconds to insert coconut");

            // Insert a cherry towards the end of the file:
            bytesToInsert =
                System.Text.Encoding.ASCII.GetBytes("cherry");
            start = milliseconds;
            FileOps.FileHelper.InsertBytes(bytesToInsert, randomFile, 1024 * 112);
            end = milliseconds;
            Console.WriteLine(
                    "Took " + (end - start).ToString() + " milliseconds to insert cherry");
        }

        /// <summary>
        /// This runs a series of long tests to compare the impact of different 
        /// read/write buffer sizes on performance when moving large segments of 
        /// a large text file.
        /// </summary>
        public static void RunPerformanceTests()
        {
            long start;
            long end;
            string insert = "";
            string inserted = "";
            string stopwatch = "";

            // Make a test file of (2 * AvailableMemory) bytes:
            long mem2 = Math.Min(2 * AvailableMemory, int.MaxValue);
            Console.Clear();
            Console.WriteLine("\n\tAVAILABLE MEMORY:\t" + GetFriendlyFilesizeString(AvailableMemory));
            Console.WriteLine("\n\n The default test file that will be created is the lesser of:\n\t(a) the biggest chunk of free disk space availalbe, or \n\t(b) double your available memory.");
            string filename = MakeBigassTestFile(2 * AvailableMemory);
            long actualsize = new FileInfo(filename).Length;
            // Measure insert time of transposeReverse using read/write
            // buffers from 256 bytes to Max (== min(whole file,availableMemory)):
            long[] bigSizes = new long[] { 256, 512, 1 * KB, 4 * KB, 16 * KB, 32 * KB, 64 * KB, 128 * KB, 256 * KB, 512 * KB, 1 * MB, 2 * MB, 4 * MB, 16 * MB, 64 * MB, 256 * MB, AvailableMemory / 2, AvailableMemory, (new FileInfo(filename).Length) };
            List<int> lSizes = new List<int>();
            foreach (long i in bigSizes)
            {
                if (i <= AvailableMemory && i <= actualsize && i < int.MaxValue)
                    lSizes.Add((int)i);
            }
            int[] sizes = new int[lSizes.Count];
            lSizes.CopyTo(sizes);

            byte[] bytes;
            int totalInsertedBytes = 0;

            string answer;
            Console.WriteLine("\n\tPLEASE CHOOSE AN INSERT STRATEGY TO TEST:\n");
            InsertStrategies strategy;
            string[] strategies = System.Enum.GetNames(typeof(InsertStrategies));
            int[] vals = (int[])System.Enum.GetValues(typeof(InsertStrategies));
            for (int i = 0; i < vals.Length; i++)
            {
                Console.WriteLine("\t\t" + strategies[i] + ", enter \t[" + vals[i].ToString() + "]");
            }
            Console.Write("\n\tENTER YOUR CHOICE NOW:");
            answer = Console.ReadLine().Trim();
            try
            {
                strategy = (InsertStrategies)System.Enum.Parse(typeof(InsertStrategies), answer, true);
            }
            catch { strategy = InsertStrategies.UseTransposeReverse; }
            // Buffer Size is irrelevant to the EofTemp strategy, so cut down array down to 1:
            if (strategy == InsertStrategies.UseTargetAsTempFile)
                sizes = new int[] { 256 };
            Console.WriteLine("\n\t==============================================\n");
            Console.WriteLine("\t\tSIZE OF TEST FILE IS:\t" + GetFriendlyFilesizeString(actualsize).Trim());
            Console.WriteLine("\t" + sizes.Length + " BUFFER SIZES BEING TESTED:\t" + GetFriendlyFilesizeString(sizes[0]).Trim() + " to " + GetFriendlyFilesizeString(sizes[sizes.Length - 1]).Trim());
            Console.WriteLine("\tESTIMATED TIME FOR ALL TESTS:\t~" + Math.Round((double)(new FileInfo(filename).Length) / (25.6 * (double)MB), 1).ToString() + " minutes.\n");
            Console.WriteLine("\tINSERT STRATEGY IS:\t" + strategy.ToString());
            Console.WriteLine("\n\nStarting long performance tests at " + DateTime.Now.ToShortTimeString());
            Console.WriteLine("(it is normal not to see any output for a few moments...)\n");
            
            // HERE IS WHERE EACH BUFFER SIZE GETS TIMED:
            foreach (int i in sizes)
            {
                insert = "BUFFER_SIZE = {0}:\t";
                insert = string.Format(insert, GetFriendlyFilesizeString(i));
                inserted = insert + "                                \n";
                bytes = Encoding.ASCII.GetBytes(inserted);
                totalInsertedBytes += bytes.Length;
                start = milliseconds;
                FileHelper.InsertBytes(bytes,filename,0,i,strategy);
                end = milliseconds;
                if (end < start)
                    System.Diagnostics.Debug.Write("Woah Nellie!");
                stopwatch = string.Format("{0:n}", Math.Round(((double)end - (double)start) / 1000, 2) + " seconds");
                bytes = Encoding.ASCII.GetBytes(stopwatch);
                FileHelper.WriteBytes(bytes, filename, insert.Length);
                Console.WriteLine(insert + stopwatch);
            }
            Console.WriteLine("\n\t**************************************************************");
            Console.Write("\n\nView head of test file after " + sizes.Length.ToString() + " inserts ? (Y/N): ");
            answer = Console.ReadLine();
            if (answer.Trim().ToUpper() == "Y")
            {
                Console.WriteLine("\t********** Beginning of file contents after " + sizes.Length.ToString() + " lines written **********\n");
                Console.WriteLine(FileHelper.Head(filename, (int)(totalInsertedBytes * 1.25)));
            }
            Console.Write("\n Delete temp file to reclaim disk space? (Y/N): ");
            answer = Console.ReadLine();
            if (answer.Trim().ToUpper() == "Y")
            {
                try
                { System.IO.File.Delete(filename); }
                catch 
                {
                    Console.WriteLine("\nFile could not presently be deleted; please remember to delete");
                    Console.WriteLine("(" + filename + ") when all locks are released."); 
                }
            }
            Console.WriteLine();
            Console.WriteLine("\nTests completed at " + DateTime.Now.ToShortTimeString());
            Console.Write("\n\t[Y] TO RUN AGAIN, [ENTER] TO QUIT: ");
            answer = Console.ReadLine();
            if (answer.Trim().ToUpper() == "Y")
            {
                RunPerformanceTests();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Create a test file full of random keyboard characters;
        /// </summary>
        /// <param name="filename">Target file</param>
        /// <param name="sizeInBytes">Size to make it</param>
        /// <param name="searchword">Word to hide amongst the random bytes</param>
        /// <param name="searchwordPosition">Where to put searchword within the file (must be less than sizeInBytes)</param>
        /// <param name="chunksize">Bytes per packet for write buffering</param>
        public static void MakeRandomTestFile(string filename, Int32 sizeInBytes, string searchword, Int32 searchwordPosition, int chunksize)
        {
            // Creates a file ~sizeInBytes composed of chunksize'd repeated random text chunks,
            // with a searchword embedded near [searchwordPosition] for testing find/replace
            // operations later. This is more portable than zipping up test files with the project...
            Random rnd = new Random();
            byte[] chunk = new byte[chunksize];
            using (FileStream fs = new FileStream
                (filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                string seedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
                    + "`1234567890-~!@#$%^&*()_+[]\\{}|;':\",./<>?\r\n";
                byte[] seeds = System.Text.Encoding.ASCII.GetBytes(seedChars);
                BinaryWriter bw = new BinaryWriter(fs);
                for (int i = 0; i < sizeInBytes; i += chunksize)
                {
                    if (i >= searchwordPosition - chunksize)
                        bw.Write(searchword);
                    else
                        for (int j = 0; j < chunksize; j++)
                        {
                            chunk[j] = seeds[rnd.Next(seeds.Length - 1)];
                        }
                    bw.Write(chunk);
                }
                bw.Close(); fs.Close();
            }
        }

        public static string MakeBigassTestFile(long sizeInBytes)
        {
            // Creates a file ~sizeInBytes composed of random bytes:
            // and returns the filename:
            long mostFreeSpace = 0;
            string driveLetter;
            Console.WriteLine("\n Estimated time for these tests is ~10 minutes for a 256MB test file...\n");
            Console.WriteLine(" I can create the test file for you if you like; choose an option:\n\n" +
                "* [Ctrl + C]\t\t to exit without running tests,\n" +
                "* [VALID PATH/FILENAME]\t to use an existing file,\n" +
                "* [NUMERIC VALUE in MB]\t to change test file size,\n" +
                "* [ENTER]\t\t to let me create the " + GetFriendlyFilesizeString(sizeInBytes).Trim() + " file...\n");
            Console.Write("\tPLEASE CHOOSE NOW: ");
            string filename;
            filename = Console.ReadLine().Trim();
            Console.WriteLine();
            double outSize = 0;
            if (File.Exists(filename))
            {
                Console.WriteLine("\n Okay, I'm going to use your supplied " + GetFriendlyFilesizeString(new FileInfo(filename).Length).Trim() + " test file.");
                return filename;
            }
            else if (double.TryParse(filename, out outSize))
            {
                sizeInBytes = (long)(outSize * MB);
            }
            Console.WriteLine("\tIS THERE A FASTER DRIVE THAN YOUR SYSTEM DRIVE? IF SO,");
            Console.Write("\tENTER DRIVE LETTER, or hit [ENTER] TO USE SYSTEM DRIVE: ");
            DriveInfo tempDrive;
            try
            {
                driveLetter = Console.ReadLine().Substring(0,1).ToUpper();
                tempDrive = new DriveInfo(driveLetter);
                // using drive root:
                filename = Path.Combine(driveLetter + ":\\", new FileInfo(Path.GetTempFileName()).Name);// .Replace("C:","M:"); 
            }
            catch 
            {
                filename = Path.GetTempFileName();
                driveLetter = filename.Substring(0, 1).ToUpper();
                tempDrive = new DriveInfo(driveLetter);
            }
            // See if the temp drive is big enough to hold
            // our test file:
            if (tempDrive.AvailableFreeSpace > sizeInBytes)
            {
                mostFreeSpace = tempDrive.AvailableFreeSpace;
            }
            // Otherwise, can we find a drive that is?
            else
            {
                foreach (DriveInfo di in DriveInfo.GetDrives())
                {
                    if (di.DriveType == DriveType.Fixed)
                    {
                        if (di.AvailableFreeSpace > mostFreeSpace)
                        {
                            mostFreeSpace = di.AvailableFreeSpace;
                            driveLetter = di.Name;
                            filename = Path.Combine(driveLetter + ":\\", System.IO.Path.GetFileName(filename));
                        }
                    }
                }
            }
            if (mostFreeSpace < sizeInBytes)
            {
                Console.WriteLine("\nI couldn't find enough free space to create a " +
                    GetFriendlyFilesizeString(sizeInBytes).Trim() + " test file...\n");
                Console.WriteLine("Reducing size to " + GetFriendlyFilesizeString(mostFreeSpace).Trim() + "\n");
            }
            sizeInBytes = (long)Math.Min(sizeInBytes, (mostFreeSpace * .75));
            if (!(sizeInBytes > 1 * KB))
                throw new ArgumentOutOfRangeException("sizeInBytes", "No drives were found with enough free space to run this test.");
            long start = milliseconds;
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                Console.WriteLine("\tCreating " + GetFriendlyFilesizeString(sizeInBytes).Trim() + " test file; hold on a minute...");
                string seedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-----------\n";
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 1000; i++)
                {
                    sb.Append(seedChars);
                }
                    //+ "`1234567890-~!@#$%^&*()_+[]\\{}|;':\",./<>?\r\n";
                byte[] seeds = System.Text.Encoding.ASCII.GetBytes(sb.ToString());
                sb = null;
                fs.SetLength(sizeInBytes);
                BinaryWriter bw = new BinaryWriter(fs);
                try
                {
                    for (uint i = 0; i < (uint)sizeInBytes; i += (uint)seeds.Length)
                    {
                        bw.Write(seeds);
                    }
                }
                catch 
                    {   // just in case someone fills up the target disk
                        // after we measured it; do nothing.
                    }
                bw.Close(); fs.Close();
            }
            long end = milliseconds;
            string stopwatch = string.Format("{0:n}", Math.Round(((double)end - (double)start) / 1000, 2) + " seconds");
            Console.Clear();
            Console.WriteLine("\n\t\tCREATED TEST FILE IN:\t" + stopwatch);
            return filename;
        }
    }

    class FileHelper
    {
        private const int KB = 1024;
        private const int MB = 1024 * 1024;
        private const int GB = 1024 * 1024 * 1024;

        /// <summary>
        /// Writes bytes[] into file referred to by <paramref name="filename"/> at <paramref name="position"/>, 
        /// increasing length of file by length of inserted bytes.
        /// </summary>
        /// <param name="bytes">Array of bytes to insert into file</param>
        /// <param name="filename">Target file to receive <paramref name="bytes[]"/></param>
        /// <param name="position">Position in file at which to insert <paramref name="bytes[]"/></param>
        public static void InsertBytes(byte[] bytes, string filename, long position)
        {
            InsertBytes(bytes, filename, position, 32 * KB);

        }

        /// <summary>
        /// Identical to the other InsertBytes() overload, except it adds the bfrSz argument
        /// to force a specific read/write buffer size when Transpose or transposeReverse is 
        /// called. Also, removes the redundant length comparison logic in the other overload, 
        /// since Transpose() now supports that internally:
        /// </summary>
        public static void InsertBytes(byte[] bytes, string filename, long position, int bfrSz)
        {
            // Length of file before insert:
            long fileLen = new FileInfo(filename).Length;
            // Extend the target file to accomodate bytes[]:
            SetFileLen(filename, fileLen + bytes.Length);
            // Move the bytes after our insert position to make room for 
            // the bytes we're inserting, in one fell swoop:
            Transpose(filename, position, position + bytes.Length, fileLen - position, bfrSz);
            // Then insert the desired bytes and we're done:
            WriteBytes(bytes, filename, position);
        }

        /// <summary>
        /// *** NOT FOR PRODUCTION USE, LACKS ERROR CHECKING ***
        /// Allows performance testing of all insert stragegies
        /// *** NOT FOR PRODUCTION USE, LACKS ERROR CHECKING ***
        /// </summary>
        public static void InsertBytes(byte[] bytes, string filename, long position, int bfrSz, InsertStrategies strategy)
        {
            // Length of file before insert:
            long fileLen = new FileInfo(filename).Length;            
            
            // Move the bytes after our insert position to make room for 
            // the bytes we're inserting, in one fell swoop:
            switch(strategy)
            {
                case InsertStrategies.UseTransposeForward:
                    // Extend the target file to accomodate bytes[]:
                    SetFileLen(filename, fileLen + bytes.Length);
                    transposeForward(filename, position, position + bytes.Length, fileLen - position, bfrSz);
                    break;
                case InsertStrategies.UseTargetAsTempFile:
                    InsertBytesUsingEOFTemp(bytes, filename, position);
                    break;
                case InsertStrategies.UseTransposeReverse:
                    // Extend the target file to accomodate bytes[]:
                    SetFileLen(filename, fileLen + bytes.Length);
                    // Transpose will automatically use TransposeReverse:
                    transposeReverse(filename, position, position + bytes.Length, fileLen - position, bfrSz);
                    break;
            }
            // Then write in the desired bytes and we're done:
            WriteBytes(bytes, filename, position);
        }

        /// <summary>
        /// Inserts bytes into a file while avoiding any external memory or disk buffers;
        /// when needed, the target file provides its own temp space:
        /// </summary>
        /// <param name="bytes">Bytes to be inserted</param>
        /// <param name="filename">Target file</param>
        /// <param name="position">Insertion position</param>
        public static void InsertBytesUsingEOFTemp(byte[] bytes, string filename, long position)
        {
            long fileLen = new FileInfo(filename).Length;
            long suffixLen = fileLen - position;
            long suffixTempPosition;
            long tempLen;
            // Is the Inserted text inter or shorter than the right segment?

            long compare = suffixLen.CompareTo(bytes.Length);
            // If we're shifting the RH segment right by its own length or more, 
            // then we have it easy; shift it exactly enough to accomodate the
            // inserted bytes...
            if (compare < 0)
            {
                suffixTempPosition = position + bytes.Length;
                tempLen = (suffixTempPosition + suffixLen);
                SetFileLen(filename, tempLen);
                Transpose(filename, position, suffixTempPosition, suffixLen);
                WriteBytes(bytes, filename, position);
            }
            // Otherwise, if we're shifting the RH segment right by less than 
            // its own length, we'll encounter a write/read collision, so
            // we would need to preserve the RH segment by buffering [1]:
            else
            {
                suffixTempPosition = fileLen;
                tempLen = (fileLen + suffixLen);
                SetFileLen(filename, tempLen);
                Transpose(filename, position, suffixTempPosition, suffixLen);
                WriteBytes(bytes, filename, position);
                Transpose(filename, suffixTempPosition, position + bytes.Length, suffixLen);
                SetFileLen(filename, fileLen + bytes.Length);
            }
            // [1] See InsertBytes() and transposeReverse() for a more efficient approach;
        }

        /// <summary>
        /// Within <paramref name="filename"/>, moves a range of <paramref name="Len"/> bytes 
        /// starting at <paramref name="SourcePos"/> to <paramref name="DestPos"/>.
        /// </summary>
        /// <param name="filename">The target file</param>
        /// <param name="SourcePos">The starting position of the byte range to move</param>
        /// <param name="DestPos">The destination position of the byte range</param>
        /// <param name="Len">The number of bytes to move</param>
        public static void Transpose(string filename, long SourcePos, long DestPos, long Len)
        {
            // 32KB is consistently among the most efficient buffer sizes:
            Transpose(filename, SourcePos, DestPos, Len, 32 * KB);
        }

        /// <summary>
        /// Identical to Transpose(), but allows the caller to specify a read/write buffer
        /// size if transposeReverse is called:
        /// </summary>
        public static void Transpose(string filename, long SourcePos, long DestPos, long Len, int bfrSz)
        {
            if (DestPos > SourcePos && Len > (DestPos - SourcePos))
            {
                // Delegate work to transposeReverse, telling it to use a
                // specified read/write buffer size:
               transposeForward(filename, SourcePos, DestPos, Len, bfrSz);
            }
            else
            {
                using (FileStream fsw = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
                {
                    using (FileStream fsr = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (BinaryReader br = new BinaryReader(fsr)) //StreamReader reads bytes wrong somehow
                        {
                            using (BinaryWriter bw = new BinaryWriter(fsw))
                            {
                                br.BaseStream.Position = SourcePos;
                                bw.BaseStream.Seek(DestPos, SeekOrigin.Begin);
                                //bw.Seek(int.Parse(DestPos.ToString()), SeekOrigin.Begin);
                                for (long i = 0; i < Len; i++)
                                {
                                    bw.Write(br.ReadByte());
                                }
                                bw.Close();
                                br.Close();
                            }
                        }
                    }
                }
            }
        }

        private static void transposeForward(string filename, long SourcePos, long DestPos, long Length, int bfrSz)
        {
            long distance = DestPos - SourcePos;
            if (distance < 1)
            {
                throw new ArgumentOutOfRangeException
                    ("DestPos", "DestPos is less than SourcePos, and this method can only copy byte ranges to the right.\r\n" +
                    "Use the public Transpose() method to copy a byte range to the left of itself.");
            }
            long readPos = SourcePos;// +Length;
            long writePos = DestPos;// +Length;
            bfrSz = bfrSz < 1 ? 32 * KB :
                (int)Math.Min(bfrSz, Length);
            // more than 40% of available memory poses a high risk of
            // OutOfMemoryExceptions when allocating 2x buffer, and
            // saps performance anyway:
            bfrSz=(int)Math.Min(bfrSz, (My.ComputerInfo.AvailablePhysicalMemory * .4));

            long numReads = Length / bfrSz;
            byte[] buff = new byte[bfrSz];
            byte[] buff2 = new byte[bfrSz];
            int remainingBytes = (int)Length % bfrSz;
            using (FileStream fsw = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
            {
                using (FileStream fsr = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fsr))
                    {
                        using (BinaryWriter bw = new BinaryWriter(fsw))
                        {
                            sr.BaseStream.Seek(readPos, SeekOrigin.Begin);
                            bw.BaseStream.Seek(writePos, SeekOrigin.Begin);
                            // prime Buffer B:
                            sr.BaseStream.Read(buff2, 0, bfrSz);
                            for (long i = 1; i < numReads; i++)
                            {
                                buff2.CopyTo(buff,0);
                                sr.BaseStream.Read(buff2, 0, bfrSz);
                                bw.Write(buff, 0, bfrSz);                                
                                                                
                            }
                            buff2.CopyTo(buff,0);
                            if (remainingBytes > 0)
                            {
                                buff2 = new byte[remainingBytes];
                                sr.BaseStream.Read(buff2, 0, remainingBytes);
                                bw.Write(buff, 0, bfrSz);
                                bfrSz = remainingBytes;
                                buff = new byte[bfrSz];
                                buff2.CopyTo(buff,0);
                            }
                            bw.Write(buff, 0, bfrSz);
                            bw.Close();
                            sr.Close();
                            buff = null;
                            buff2 = null;
                        }
                    }
                }
            }
            GC.Collect();
        }

        private static void transposeReverse(string filename, long SourcePos, long DestPos, long Length, int bfrSz)
        {
            long distance = DestPos - SourcePos;
            if (distance < 1)
            {
                throw new ArgumentOutOfRangeException
                    ("DestPos", "DestPos is less than SourcePos, and this method can only copy byte ranges to the right.\r\n" +
                    "Use the public Transpose() method to copy a byte range to the left of itself.");
            }
            long readPos = SourcePos + Length;
            long writePos = DestPos + Length;
            bfrSz = bfrSz < 1 ? (int)Math.Min(My.ComputerInfo.AvailablePhysicalMemory * .9, Length) : (int)Math.Min(bfrSz, Length);

            long numReads = Length / bfrSz;
            byte[] buff = new byte[bfrSz];
            int remainingBytes = (int)Length % bfrSz;
            using (FileStream fsw = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
            {
                using (FileStream fsr = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fsr))
                    {
                        using (BinaryWriter bw = new BinaryWriter(fsw))
                        {
                            sr.BaseStream.Seek(readPos, SeekOrigin.Begin);
                            bw.BaseStream.Seek(writePos, SeekOrigin.Begin);
                            for (long i = 0; i < numReads; i++)
                            {
                                readPos -= bfrSz;
                                writePos -= bfrSz;
                                sr.DiscardBufferedData();
                                sr.BaseStream.Seek(readPos, SeekOrigin.Begin);
                                sr.BaseStream.Read(buff, 0, bfrSz);
                                bw.BaseStream.Seek(writePos, SeekOrigin.Begin);
                                bw.Write(buff, 0, bfrSz);
                            }
                            if (remainingBytes > 0)
                            {
                                bfrSz = remainingBytes;
                                readPos -= bfrSz;
                                writePos -= bfrSz;
                                sr.DiscardBufferedData();
                                sr.BaseStream.Seek(readPos, SeekOrigin.Begin);
                                sr.BaseStream.Read(buff, 0, bfrSz);
                                bw.BaseStream.Seek(writePos, SeekOrigin.Begin);
                                bw.Write(buff, 0, bfrSz);
                            }
                            bw.Close();
                            sr.Close();
                            buff = null;
                        }
                    }
                }
            }
            GC.Collect();
        }

        /// <summary>
        /// Writes bytes[] into <paramref name="file"/> at [position], overwriting existing contents
        /// </summary>
        /// <param name="bytes">Array of bytes to write into <paramref name="file"/></param>
        /// <param name="filename">Target file to be written to
        /// <param name="position">Position at which to begin writing
        public static void WriteBytes(byte[] bytes, string filename, long position)
        {
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                BinaryWriter bw = new BinaryWriter(fs);
                bw.BaseStream.Seek(position, SeekOrigin.Begin);
                bw.Write(bytes);
                bw.Close(); fs.Close();
            }
        }

        /// <summary>
        /// Wrapper for FileStream.SetLength().
        /// </summary>
        /// <remarks>
        /// When lengthening a file, this method appends null characters to it which 
        /// does NOT leave it in an XML-parseable state. After all your transpositions, 
        /// ALWAYS come back and truncate the file unless you've overwritten the 
        /// appended space with valid characters.
        /// </remarks>
        /// <param name="filename">Name of file to resize</param>
        /// <param name="len">New size of file</param>
        public static void SetFileLen(string filename, long len)
        {
            using (FileStream fsw = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
            {
                fsw.SetLength(len);
                fsw.Close();
            }
        }

        /// <summary>
        /// Overwrites <paramref name="length"/> bytes in <paramref name="filename"/> 
        /// with spaces, beginning at <paramref name="start"/>.
        /// </summary>
        /// <param name="filename">The target file</param>
        /// <param name="start">The position at which to begin writing spaces</param>
        /// <param name="length">How many spaces to write</param>
        public static void WriteSpaces(string filename, int start, int length)
        {
            using (FileStream fs = new FileStream
                (filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Seek(start, SeekOrigin.Begin);
                for (int i = 0; i < length; i++)
                {
                    bw.Write(" ");
                }
                bw.Flush(); bw.Close(); fs.Close();
            }
        }

        /// <summary>
        /// Grab the desired number of bytes from the beginning of a file;
        /// useful, e.g. for files too large to open in Notepad.
        /// </summary>
        /// <param name="filename">Target file</param>
        /// <param name="lines">Number of lines to grab</param>
        /// <returns>First n lines from the file</returns>
        public static string Head(string filename, int bytes)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                char[] buffer = (char[])Array.CreateInstance(typeof(char), bytes); 
                StringBuilder sb = new StringBuilder();
                using (StreamReader sr = new StreamReader(fs))
                {
                    sr.BaseStream.Seek(0, SeekOrigin.Begin);
                    sr.ReadBlock(buffer, 0, buffer.Length);
                    return new string(buffer);
                }
            }
        }

        /// <summary>
        /// Grab the desired number of kilobytes from the end of a file;
        /// useful, e.g. for files too large to open in Notepad.
        /// </summary>
        /// <param name="filename">Target file</param>
        /// <param name="kb">Number of kilobytes to grab</param>
        /// <returns>Last kb bytes from the file</returns>
        public static string Tail(string filename, int bytes)
        {
            using (FileStream fs = new FileStream(filename,FileMode.Open,FileAccess.Read))
            {
                char[] buffer = (char[])Array.CreateInstance(typeof(char), bytes);
                string txt;
                using (StreamReader sr = new StreamReader(fs))
                {
                    sr.BaseStream.Seek((-1024 * bytes), SeekOrigin.End);
                    sr.ReadBlock(buffer, 0, buffer.Length);
                    txt = new string(buffer);
                    sr.Close(); fs.Close(); 
                }
                return txt;
            }
        }

        /// <summary>
        /// Returns the position of the first occurrence of <paramref name="FindWhat"/> 
        /// within <paramref name="InStream"/>, or -1 if <paramref name="FindWhat"/> is
        /// not found.
        /// </summary>
        /// <param name="FindWhat">The string being sought</param>
        /// <param name="InStream">The stream in which to search (must be readable & seekable)</param>
        /// <returns>The position of the first occurrence of <paramref name="FindWhat"/> 
        /// within <paramref name="InStream"/>, or -1 if <paramref name="FindWhat"/> is
        /// not found
        /// </returns>
        public static int Find(string FindWhat, Stream InStream)
        {
            // TODO: Investigate performance optimizations using a smart string-search
            // algorithm, like Boyer-Moore, Knuth-Morris-Pratt, etc. Automate choice of
            // brute force vs. smart algorithm; Optionally, run performance tests & save 
            // results to a configuration file indicating, e.g., where the tradeoff between
            // algorithms would be for a given length of FindWhat & file size.
            int streamPos = 0;
            int findPos;
            bool found = true;
            char findChar;
            StreamReader sr = new StreamReader(InStream);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            // Outer loop for entire file stream reader...
            while (sr.Peek() >= 0)
            {
                findPos = 0;
                findChar = Convert.ToChar(FindWhat.Substring(findPos, 1));
                found = findChar == (char)sr.Read();
                // Per MSDN:
                // "StreamReader might buffer input such that the position of the
                //  underlying stream will not match the StreamReader position."
                //  Since sr.BaseStream.Position is not an accurate indicator
                //  for determining streamPos, we'll track it ourselves...
                streamPos += 1;
                findPos += 1;
                // Inner loop for comparing findwhat to candidate 
                //  when we hit a potential match...
                while (found)
                {
                    while (findPos <= FindWhat.Length)
                    {
                        findChar = Convert.ToChar(FindWhat.Substring(findPos, 1));
                        found = findChar == (char)sr.Read();
                        if (!found)
                            break;
                        streamPos += 1;
                        findPos += 1;
                        if (findPos == FindWhat.Length) return streamPos - findPos;
                    }
                }
            }
            // No luck finding it?
            return -1;
        }

        /// <summary>
        /// Experimental; researching various approaches to quickly validating
        /// very large XML files, avoiding XmlDocument and XmlSchema instances
        /// </summary>
        /// <param name="filename">The XML file to be validated</param>
        /// <returns>True if valid, False if not. Capiche?</returns>
        public static bool IsValidXmlFile(string filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Open))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationEventHandler += new ValidationEventHandler(_validationHandler);
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationFlags = XmlSchemaValidationFlags.ProcessInlineSchema;
                XmlReader xr = XmlReader.Create(stream, settings);
                while (xr.Read())
                { }//do nothing, just read; if there's a validation error, it'll hit the callback.
                xr.Close(); stream.Close();
                return _validationErrorsCount == 0;
            }
        }

        private static int _validationErrorsCount;// = 0;
        private static void _validationHandler(object sender, ValidationEventArgs args)
        {
            if (args.Severity != XmlSeverityType.Warning &&
                args.Exception.Message.IndexOf
                    ("An element or attribute information item has already been validated from the '' namespace")
                    < 0)
            {
                Console.WriteLine(args.Exception.ToString());
                _validationErrorsCount++;
            }
        }
    }
}
