using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using MackPortfolio.DAL.MediaModels;

namespace MackPortfolio.Extensions
{
    public class ImageFile
    {
        private const string rootFolder = "~/Media/";

        private static Dictionary<string, Size> dSize = new Dictionary<string, Size>()
        {
            {"Original",new Size(2048, 1080)},
            {"Standard",new Size(800,600)},
            {"Thumbnail",new Size(160,160)}
        };
        private List<string> list
        {
            get
            {
                return dSize.Keys.ToList();
            }
        }
        private Media model = new Media();
        private Size size = dSize["Original"];
        private Point point = new Point(0, 0);
        private string contentImageId = DateTime.Now.Ticks.ToString();
        private string ImageName = SequentialGuid.NewGuid().ToString();
        private bool keepWidth = false;
        private bool isCentered = true;
        private int x = 0;
        private int y = 0;

        public ImageFile()
        {
        }

        public ImageFile(Image image)
        {
            Image = image;
        }

        public ImageFile(HttpPostedFileBase file)
        {
            postedFile = file;
            Image = Image.FromStream(postedFile.InputStream, true, true);
        }

        public ImageFile(string url)
        {
            var _file = new FileInfo(mapServerPath(url));
            _file.Open(FileMode.Open, FileAccess.ReadWrite);

            var file = new FileStream(_file.FullName, FileMode.Open, FileAccess.Read);
            var filesize = (Int32)_file.Length;
            BinaryReader binaryFile = new BinaryReader(file);
            var buffer = binaryFile.ReadBytes(filesize);

            model = new Media
            {
                Type = "Image"
            };

            Image = Image.FromStream(postedFile.InputStream, true, true);
        }

        private void mapModel()
        {
            model.Type = "Image";
            model.ContentType = postedFile.ContentType;
            model.Size = postedFile.ContentLength;
            model.Extension = Path.GetExtension(postedFile.FileName);
            model.Title = Path.GetFileNameWithoutExtension(postedFile.FileName);
            model.Description = Path.GetFileName(postedFile.FileName) + ": " + postedFile.ContentType;
            model.Name = Path.GetFileNameWithoutExtension(postedFile.FileName).ToUrlString("-") + Path.GetExtension(postedFile.FileName);
        }
        public Media MapSaveImage(string folder)
        {
            mapModel();
            VirtualPath = Path.Combine(rootFolder, folder);
            ServerPath = mapServerPath(VirtualPath);
            ImageName += Path.GetExtension(model.Name);

            return model;
        }

        public string CreateThumbnail()
        {

            return "";
        }

        public void ReplaceImage(string oldImagePath)
        {

        }

        public string VirtualPath { get; private set; }
        public string ServerPath { get; private set; }
        public Image newImage { get; private set; }

        private string targetFolder { get; set; }
        private int Width { get; set; }
        private int Height { get; set; }
        private Image Image { get; set; }
        private HttpPostedFileBase postedFile { get; set; }


        //TODO: Crop Image
        private void crop(Size cropSize)
        {
            if (isCentered)
            {
                y = (Image.Height / 2) - (cropSize.Height / 2);
                x = (Image.Width / 2) - (cropSize.Width / 2);
            }
            Height = Image.Height > cropSize.Height ? cropSize.Height : Image.Height;
            Width = Image.Width > cropSize.Width ? cropSize.Width : Image.Width;

            point = new Point((x < 0 ? 0 : x), (y < 0 ? 0 : y));
            size = new Size(Width, Height);
            var rect = new Rectangle(point, size);

            Bitmap bmpImage = new Bitmap(Image);
            Bitmap bmpCrop = bmpImage.Clone(rect, bmpImage.PixelFormat);
            newImage = (Image)(bmpCrop);
        }

        //TODO: Resize Image
        private void resize()
        {
            float percentWidth = 0;
            float percentHeight = 0;
            float percent = 0;

            // Prevent using images internal thumbnail
            Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
            Image.RotateFlip(RotateFlipType.Rotate180FlipNone);

            int originalWidth = Image.Width;
            int originalHeight = Image.Height;
            percentWidth = (float)size.Width / (float)originalWidth;
            percentHeight = (float)size.Height / (float)originalHeight;

            if (keepWidth)
            {
                percent = percentWidth;
            }
            else
            {
                percent = percentHeight < percentWidth ? percentHeight : percentWidth;
            }
            Width = (int)(originalWidth * percent);
            Height = (int)(originalHeight * percent);

            newImage = new Bitmap(Width, Height);
        }

        //TODO: Draw Image
        private void draw()
        {
            try
            {
                using (var g = Graphics.FromImage(newImage))
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(Image, new Rectangle(0, 0, newImage.Width, newImage.Height));
                }
                //return newImage;
            }
            catch
            {
                if (newImage != null) newImage.Dispose();
                throw new Exception("Resize Image Error");
            }
        }

        //TODO: Save
        private void save(string serverPath)
        {
            var qualityParam = new EncoderParameter(Encoder.Quality, 90L);

            // Jpeg image codec
            ImageCodecInfo jpegCodec = getEncoderInfo("image/jpeg");
            using (var encParams = new EncoderParameters(1))
            {
                encParams.Param[0] = qualityParam;
                newImage.Save(serverPath, jpegCodec, encParams);
            }
            //return ServerPath;
        }
        private ImageCodecInfo getEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }

        //TODO: map or delete directory / image
        private string mapServerPath(string virtualPath)
        {
            return HttpContext.Current.Server.MapPath(virtualPath);
        }
        private string mapDirectory(string directory, string file)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            return Path.Combine(directory, file);
        }
        private void removeFile(string oldImagePath)
        {
            if (File.Exists(oldImagePath))
            {
                File.Delete(oldImagePath);
            }
        }

    }
}
