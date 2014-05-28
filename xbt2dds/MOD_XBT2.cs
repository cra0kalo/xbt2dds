using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

using System.IO;




namespace xbt2dds
{
public class MOD_XBT2
{


    private string In_FilePath;
    private string Out_FolderPath;
    private string Out_FileName;

    //FAT
    private BinaryReader br;
    private FileStream fs;


    //DAT
    private BinaryReader d_br;
    private FileStream d_fs;

    //Writer
    private BinaryWriter bw;
    private FileStream fw;

    private bool ParsedHeader;

    //Main Variables here
    private string dwID;
    private byte dwVersion;
    private UInt32 dwUnknown;
    private UInt32 dwHeaderSize;





    public MOD_XBT2(string InFilePath, string OutFolderPath)
    {
        this.In_FilePath = InFilePath;
        this.Out_FolderPath = OutFolderPath;
        this.Out_FileName = Path.GetFileNameWithoutExtension(InFilePath) + "." + "dds";
    }



    public void ParseExport()
    {


        _tmain.VText("xbt2dds working");
        fs = new FileStream(In_FilePath, FileMode.Open, FileAccess.Read);
        br = new BinaryReader(fs);


        //Read magic

        dwID = ReadStringABSLen(br, 3);
        if (dwID == "TBX")
        {
            _tmain.VText("Reading texture " + Path.GetFileName(In_FilePath));
            //read rest of header
            dwVersion = br.ReadByte();
            dwUnknown = br.ReadUInt32();
            dwHeaderSize = br.ReadUInt32();
            //calc how much to skip
            long skipamount = (dwHeaderSize - br.BaseStream.Position);
            long readamount = (fs.Length - dwHeaderSize);
            br.BaseStream.Position += skipamount;
            ParsedHeader = true;
            byte[] DDSTextureFile = br.ReadBytes(Convert.ToInt32(readamount));
            using (FileStream xporter = new FileStream(Path.Combine(Out_FolderPath, Out_FileName), FileMode.Create, FileAccess.Write))
            {
                BinaryWriter bw = new BinaryWriter(xporter);
                bw.Write(DDSTextureFile);
            }
            _tmain.VText("Wrote " + Path.GetFileNameWithoutExtension(In_FilePath) + ".dds");
        }
        else
        {
            ParsedHeader = false;
            _tmain.VText("ERROR: Looking for TBX header found something else.");
            _tmain.VText("Wrong MAGIC");
            br.Close();
        }
    }



    /// <summary>
    /// Takes a binary stream reads set many bytes returns string without any trails
    /// </summary>
    /// <param name="ii_Reader"></param>
    /// <param name="bytes2read"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public string ReadStringABSLen(BinaryReader ii_Reader, UInt32 bytes2read)
    {


        //Reads String from current Binary stream
        string str = string.Empty;
        byte[] StringBuffer = ii_Reader.ReadBytes(Convert.ToInt32(bytes2read));
        str = System.Text.Encoding.ASCII.GetString(StringBuffer).TrimEnd('\0');
        return str;
    }


}

}