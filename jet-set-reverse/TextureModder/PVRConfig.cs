using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSReverse.TextureModder
{
    public class PVRConfigData
    {
        public uint GlobalIndex;
        public VrSharp.PvrTexture.PvrDataFormat DataFormat;
        public VrSharp.PvrTexture.PvrCompressionFormat CompressionFormat;
        public VrSharp.PvrTexture.PvrPixelFormat PixelFormat;

        public PVRConfigData(String line)
        {
            var data = line.Split(",".ToCharArray());
            GlobalIndex = uint.Parse(data[0]);
            DataFormat = (VrSharp.PvrTexture.PvrDataFormat)Enum.Parse(typeof(VrSharp.PvrTexture.PvrDataFormat), data[1]);
            CompressionFormat = (VrSharp.PvrTexture.PvrCompressionFormat)Enum.Parse(typeof(VrSharp.PvrTexture.PvrCompressionFormat), data[2]);
            PixelFormat = (VrSharp.PvrTexture.PvrPixelFormat)Enum.Parse(typeof(VrSharp.PvrTexture.PvrPixelFormat), data[3]);
        }

        public String ToPVRConvFlags()
        {
            var sb = new StringBuilder();
            sb.Append(" -gi ").Append(GlobalIndex);
            
            //Data Format
            if (DataFormat == VrSharp.PvrTexture.PvrDataFormat.Rectangle)
                sb.Append(" -r");
            else if (DataFormat == VrSharp.PvrTexture.PvrDataFormat.RectangleTwiddled)
                sb.Append(" -r -t");
            else if (DataFormat == VrSharp.PvrTexture.PvrDataFormat.Vq)
                sb.Append(" -v3 -nvm");
            else if (DataFormat == VrSharp.PvrTexture.PvrDataFormat.VqMipmaps)
                sb.Append(" -v3");
            else if (DataFormat == VrSharp.PvrTexture.PvrDataFormat.SquareTwiddled)
                sb.Append(" -t -nm");
            else if (DataFormat == VrSharp.PvrTexture.PvrDataFormat.SmallVq)
                sb.Append(" -sv3 -nvm");
            else if (DataFormat == VrSharp.PvrTexture.PvrDataFormat.SmallVqMipmaps)
                sb.Append(" -sv3");
            else
                throw new Exception("Unsupported DataFormat");

            //Compression Format
            if (CompressionFormat == VrSharp.PvrTexture.PvrCompressionFormat.Rle)
                throw new Exception("Unsupported CompressionFormat");

            //Pixel Format
            if (PixelFormat == VrSharp.PvrTexture.PvrPixelFormat.Rgb565)
                sb.Append(" -565");
            else if (PixelFormat == VrSharp.PvrTexture.PvrPixelFormat.Argb4444)
                sb.Append(" -4444");
            else if (PixelFormat == VrSharp.PvrTexture.PvrPixelFormat.Argb1555)
                sb.Append(" -1555");

            return sb.ToString();
        }
    }

    public class PVRConfig
    {
        private List<String> data = new List<String>();

        public void Add(VrSharp.PvrTexture.PvrTexture texture)
        {
            String line = texture.GlobalIndex + "," + texture.DataFormat + "," + texture.CompressionFormat + "," + texture.PixelFormat;
            data.Add(line);
        }

        public PVRConfigData Get(int index)
        {
            return new PVRConfigData(data[index]);
        }

        public void Save(String filePath)
        {
            System.IO.File.WriteAllLines(filePath, data);
        }
        
        public void Load(String filePath)
        {
            data = System.IO.File.ReadAllLines(filePath).ToList();
        }
    }
}
