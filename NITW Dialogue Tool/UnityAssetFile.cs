using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace NITW_Dialogue_Tool
{
    class UnityAssetFile
    {
        public static Dictionary<string,yarnFile> read(string assetsFilePath)
        {
            string assetFileName = Path.GetFileName(assetsFilePath);
            Form1.Log("Reading " + assetFileName + "...");

            Dictionary<string, yarnFile> yarnFiles = new Dictionary<string, yarnFile>();
            
            if (File.Exists(assetsFilePath))
            {
                if (IsFileLocked(assetsFilePath))
                {
                    MessageBox.Show("The file " + assetFileName + " is locked." + Environment.NewLine + Environment.NewLine + 
                        "Try closing any programs that could use it and if that fails try restarting your computer.",
                        "File Locked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Form1.Log("Failed to read " + assetFileName + " (locked)", true);
                    return yarnFiles;
                }
                
                using (BinaryReader reader = new BinaryReader(File.Open(assetsFilePath, FileMode.Open)))
                {
                    //read object data offset
                    reader.BaseStream.Seek(12, SeekOrigin.Begin);
                    byte[] objectDataOffsetBytes = reader.ReadBytes(4);
                    Array.Reverse(objectDataOffsetBytes);
                    uint objectDataOffset = BitConverter.ToUInt32(objectDataOffsetBytes, 0);

                    //skip header
                    reader.BaseStream.Seek(33, SeekOrigin.Begin);

                    //find beginning of object info
                    long position = 0;
                    while (position == 0)
                    {
                        if(reader.BaseStream.Position == reader.BaseStream.Length)
                        {
                            //asset file does not contain any objects, skip it
                            Form1.Log("Reading " + assetFileName + " skipped", true);
                            return yarnFiles;
                        }

                        if (reader.ReadByte() == 0x1)
                        {
                            for (int i = 0; i < 11; i++)
                            {
                                if (reader.BaseStream.Position == reader.BaseStream.Length)
                                {
                                    //asset file does not contain any objects, skip it
                                    Form1.Log("Reading " + assetFileName + " skipped", true);
                                    return yarnFiles;
                                }
                                if (reader.ReadByte() != 0x0)
                                {
                                    reader.BaseStream.Position -= 1;
                                    break;
                                }
                                else if (i == 10)
                                {
                                    position = reader.BaseStream.Position - 12 - 7;
                                }
                            }
                        }
                    }
                    reader.BaseStream.Seek(position, SeekOrigin.Begin);

                    //read object info
                    int objectInfoCount = reader.ReadInt32();
                    reader.BaseStream.Seek(3, SeekOrigin.Current);
                    long objectInfoPosition = reader.BaseStream.Position;
                    for (int i = 0; i < objectInfoCount; i++)
                    {
                        if (reader.BaseStream.Position > reader.BaseStream.Length - 16) //hack
                        {
                            //too close to end of file
                            break;
                        }
                        long index = reader.ReadInt64();
                        uint offset = reader.ReadUInt32();
                        uint length = reader.ReadUInt32();

                        //check if object is yarn
                        position = reader.BaseStream.Position;

                        if(objectDataOffset + offset < reader.BaseStream.Length)
                        {
                            reader.BaseStream.Seek(objectDataOffset + offset, SeekOrigin.Begin);
                            {
                                uint nameLength = reader.ReadUInt32();
                                if (nameLength < 100) //hack
                                {
                                    string yarnFileName = Encoding.UTF8.GetString(reader.ReadBytes((int)nameLength));
                                    if (yarnFileName.Contains(".yarn"))
                                    {
                                        Form1.Log("  Found " + yarnFileName);

                                        int namePadding = 0;
                                        if (yarnFileName.Length % 4 > 0)
                                        {
                                            namePadding = 4 - yarnFileName.Length % 4;
                                        }

                                        reader.BaseStream.Seek(namePadding, SeekOrigin.Current);
                                        int yarnLength = reader.ReadInt32();

                                        if (yarnLength < 9999999) //hack
                                        {
                                            
                                            string yarnContent = Encoding.UTF8.GetString(reader.ReadBytes(yarnLength));

                                            if(yarnLength % 4 > 0)
                                            {
                                                reader.BaseStream.Seek(4 - (yarnLength % 4), SeekOrigin.Current);
                                            }

                                            uint yarnPathLength = reader.ReadUInt32();
                                            string yarnPath2 = "";
                                            if (yarnPathLength > 0)
                                            {
                                                yarnPath2 = Encoding.UTF8.GetString(reader.ReadBytes(yarnLength));
                                            }

                                            //save index, offset, length and write yarn to file
                                            string yarnDir = AppDomain.CurrentDomain.BaseDirectory + @"\yarn files";
                                            if (!Directory.Exists(yarnDir))
                                            {
                                                Directory.CreateDirectory(yarnDir);
                                            }

                                            string yarnPath = Path.Combine(yarnDir, yarnFileName) + ".txt";
                                            File.Create(yarnPath).Dispose();
                                            File.WriteAllText(yarnPath, yarnContent);
                                            DateTime lastModified = File.GetLastWriteTime(yarnPath);

                                            yarnFiles.Add(yarnFileName, new yarnFile(assetsFilePath, index, offset, length, yarnFileName, yarnContent, yarnPath2, lastModified,
                                                objectDataOffset, objectInfoCount, objectInfoPosition));
                                        }
                                    }
                                }
                            }
                            //skip rest of object info
                            reader.BaseStream.Seek(position + 12, SeekOrigin.Begin);
                        }
                    }
                }
            }
            else
            {
                Form1.Log("Failed to read " + assetFileName + " (not found)", true);
                return yarnFiles;
            }

            Form1.Log("Reading " + assetFileName + " done", true);

            return yarnFiles;
        }


        public static void write(string fileName, ref yarnDictionary rootz)
        {
            yarnFile f;
            if (!rootz.yarnFiles.TryGetValue(fileName, out f))
            {
                //yarn file not found
                Form1.Log("ERROR yarn file not found: " + fileName, true);
                return;
            }

            string assetFileName = Path.GetFileName(f.assetsFilePath);
            Form1.Log("Writing " + f.yarnFileName + " to " + assetFileName + "...");

            if (File.Exists(f.assetsFilePath))
            {
                if (IsFileLocked(f.assetsFilePath))
                {
                    MessageBox.Show("The file " + assetFileName + " is locked." + Environment.NewLine + Environment.NewLine + 
                        "Try closing any programs that could use it and if that fails try restarting your computer.",
                        "File Locked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Form1.Log("Failed to write " + assetFileName + " (locked)", true);
                    return;
                }
                                
                //calculate new yarn object length
                string yarnFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\yarn files\" + f.yarnFileName + ".txt";
                
                if (IsFileLocked(yarnFilePath))
                {
                    MessageBox.Show("The file " + f.yarnFileName + " is locked." + Environment.NewLine + Environment.NewLine +
                        "Sublime is known to cause this problem, try editing with another text editor. Also close any programs that could use " + f.yarnFileName +
                        " and if that fails try restarting your computer.",
                        "File Locked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Form1.Log("Failed to write " + assetFileName + " (" + f.yarnFileName + " locked)", true);
                    return;
                }
                string newContent = File.ReadAllText(yarnFilePath);

                byte[] newContentBytes = Encoding.UTF8.GetBytes(newContent);
                byte[] pathBytes = Encoding.UTF8.GetBytes(f.yarnPath);

                uint newLength = 0;
                uint newLength_name = 0;
                uint newLength_namePadding = 0;
                uint newLength_content = 0;
                uint newLength_contentPadding = 0;
                uint newLength_path = 0;
                uint newLength_pathPadding = 0;

                newLength_name = 4 + (uint)f.yarnFileName.Length;
                if (newLength_name % 4 > 0)
                {
                    newLength_namePadding = 4 - (newLength_name % 4);
                }

                newLength_content = 4 + (uint)newContentBytes.Length;
                if (newLength_content % 4 > 0)
                {
                    newLength_contentPadding = 4 - (newLength_content % 4);
                }

                newLength_path = 4 + (uint)f.yarnPath.Length;
                if (newLength_path % 4 > 0)
                {
                    newLength_pathPadding = 4 - (newLength_path % 4);
                }

                newLength = newLength_name + newLength_namePadding;
                newLength += newLength_content + newLength_contentPadding;
                newLength += newLength_path + newLength_pathPadding;

                //calculate offset delta
                int offsetDelta = (int)newLength - (int)f.length;

                //change file length
                long nextObjectOriginalPosition = f.objectDataOffset + f.offset + f.length;
                long nextObjectNewPosition = nextObjectOriginalPosition + offsetDelta;
                long assetfileSize = new FileInfo(f.assetsFilePath).Length;
                long followingObjectsSize = assetfileSize - nextObjectOriginalPosition;
                long newAssetFileSize = assetfileSize + offsetDelta;
                                
                if (offsetDelta < 0)
                {
                    //yarn object is smaller now -> transpose yarn object following bytes and truncate file
                    FileOps.FileHelper.Transpose(f.assetsFilePath, nextObjectOriginalPosition, nextObjectNewPosition, followingObjectsSize);
                    FileOps.FileHelper.SetFileLen(f.assetsFilePath, newAssetFileSize);
                }                
                else if (offsetDelta > 0)
                {
                    //yarn object is bigger now -> transpose yarn object following bytes
                    FileOps.FileHelper.Transpose(f.assetsFilePath, nextObjectOriginalPosition, nextObjectNewPosition, followingObjectsSize);
                }

                //update object table with new length and new offsets for following objects
                List<uint> offsets = new List<uint>();

                //put all object offsets in a list
                using (BinaryReader reader = new BinaryReader(File.Open(f.assetsFilePath, FileMode.Open)))
                {
                    reader.BaseStream.Seek(f.objectInfoPosition, SeekOrigin.Begin);
                    for (int i = 0; i < f.objectInfoCount; i++)
                    {
                        //skip uint64 index
                        reader.BaseStream.Seek(8, SeekOrigin.Current);

                        //save offsets for later
                        uint offset = reader.ReadUInt32();
                        offsets.Add(offset);

                        //skip rest of entry
                        reader.BaseStream.Seek(16, SeekOrigin.Current);
                    }
                }



                //write new length and new offsets and new file size
                using (BinaryWriter writer = new BinaryWriter(File.Open(f.assetsFilePath, FileMode.Open)))
                {
                    //update filesize
                    writer.BaseStream.Seek(4, SeekOrigin.Begin);
                    byte[] newAssetFileSizeBytes = BitConverter.GetBytes((uint)newAssetFileSize);
                    Array.Reverse(newAssetFileSizeBytes);
                    writer.Write(newAssetFileSizeBytes);

                    long indexPosition = (f.index - 1) * 28;
                    writer.BaseStream.Seek(f.objectInfoPosition + indexPosition, SeekOrigin.Begin);

                    //update yarn length
                    writer.BaseStream.Seek(12, SeekOrigin.Current);
                    writer.Write(newLength);
                    writer.BaseStream.Seek(12, SeekOrigin.Current);

                    //update all offsets behind f.index
                    for (int i = (int)(f.index); i < f.objectInfoCount; i++)
                    {
                        uint newOffset = (uint)(offsets[i] + offsetDelta);

                        writer.BaseStream.Seek(8, SeekOrigin.Current);
                        writer.Write(newOffset);
                        writer.BaseStream.Seek(16, SeekOrigin.Current);
                    }

                    //update yarn content
                    writer.BaseStream.Seek(f.objectDataOffset + f.offset + 4 + f.yarnFileName.Length + newLength_namePadding, SeekOrigin.Begin);
                    writer.Write((uint)(newContentBytes.Length));


                    writer.Write(newContentBytes);
                    if (newContentBytes.Length > 0)
                    {
                        int paddingTemp = 0;
                        if (newContentBytes.Length % 4 > 0)
                        {
                            paddingTemp = 4 - (newContentBytes.Length % 4);
                        }

                        for (int i = 0; i < paddingTemp; i++)
                        {
                            writer.Write((byte)0);
                        }
                    }

                    writer.BaseStream.Position = writer.BaseStream.Position;

                    writer.Write((uint)f.yarnPath.Length);
                    writer.Write(pathBytes);
                    if (pathBytes.Length > 0)
                    {
                        int paddingTemp = 0;
                        if (pathBytes.Length % 4 > 0)
                        {
                            paddingTemp = 4 - (pathBytes.Length % 4);
                        }

                        for (int i = 0; i < paddingTemp; i++)
                        {
                            writer.Write((byte)0);
                        }
                    }
                }

                Form1.Log("Writing " + f.yarnFileName + " to " + assetFileName + " done", true);

                //update yarn dictionary
                f.lastModified = File.GetLastWriteTime(yarnFilePath);
                f.length = newLength;
                f.edited = true;
                rootz.yarnFiles[f.yarnFileName] = f;
                JsonUtil.saveYarnDictionary(rootz);
            }
            else
            {
                Form1.Log("Failed to write " + assetFileName + " (not found)", true);
                return;
            }
        }

        private static bool IsFileLocked(string filePath)
        {
            FileStream stream = null;

            FileInfo file = new FileInfo(filePath);

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
    }
}
