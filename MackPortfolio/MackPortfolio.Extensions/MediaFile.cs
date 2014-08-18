using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Helpers;
using MackPortfolio.MediaModels.DAL;

namespace MackPortfolio.Extensions
{
    public static class MediaFile
    {
        private const string folderRoot = "~/Media/";

        public static string CreateThumbnail(this WebImage image, string oldImagePath, string serverPath, string folder)
        {
            if (image.Width > 240)
            {
                image.Resize(240, 240, preserveAspectRatio: true, preventEnlarge: true);
            }
            var imgName = Path.GetFileNameWithoutExtension(image.FileName).ToUrlString() + Path.GetExtension(image.FileName);

            //Add GUID
            var guidID = SequentialGuid.NewGuid();
            var srvrImg = Path.Combine(serverPath, guidID + "_" + imgName);
            image.Save(srvrImg);

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            imgName = Path.Combine(folderRoot, folder, guidID + "_" + imgName);

            return imgName;
        }
        public static Media CreateMedia(this HttpPostedFileBase file)
        {

            if (file == null)
            {
                return new Media();
            }
            //get file information
            string type;
            var folder = folderRoot;
            int len = file.ContentLength;
            var guidId = SequentialGuid.NewGuid();
            var filetype = file.ContentType;
            var name = Path.GetFileNameWithoutExtension(file.FileName).ToUrlString("-");
            var filename = name + Path.GetExtension(file.FileName);
            if (filetype.ToLower().Contains("image"))
            {
                type = "Image";
                folder += "Images";
            }
            else if (filetype.ToLower().Contains("video") || filetype.ToLower().Contains("movie"))
            {
                type = "Video";
                folder += "Videos";
            }
            else
            {
                type = "Document";
                folder += "Documents";
            }

            //save file info
            var m = new Media()
            {
                UrlParameter = name,
                IsActive = true,
                Type = type,
                Name = filename,
                ContentType = filetype,
                Size = len,
                Title = Path.GetFileNameWithoutExtension(file.FileName),
                Description = Path.GetFileName(file.FileName) + ": " + file.ContentType,
                Extension = Path.GetExtension(file.FileName),
                Directory = Path.Combine(folder, name)
            };

            if (m.Type == "Image")
            {
                using (var image = Image.FromStream(file.InputStream, true, true))
                {
                    var maindirectory = Path.Combine(folder, name);
                    var newname = guidId + Path.GetExtension(file.FileName);

                    //Get New Image Resized and Save
                    var org = GetNewImage<Original>(image, new Size(), maindirectory, "", newname);
                    var std = GetNewImage<Standard>(image, new Size(800, 600), maindirectory, "Standard", newname);
                    var thb = GetNewImage<Thumbnail>(image, new Size(160, 160), maindirectory, "Thumbnail", newname);

                    image.Dispose();

                    //Get General Image Ratio
                    m.Thumbnail = thb;
                    m.Standard = std;
                    m.Original = org;
                }
            }
            else
            {
                //Add GUID & save file
                m.Directory = Path.Combine(folder, filename);
                file.SaveAs(Path.Combine(HttpContext.Current.Server.MapPath(folder), filename));
            }
            return m;
        }

        public static string FromFileToBase64String(this HttpPostedFileBase file)
        {
            var imgString = String.Empty;
            if (file == null || file.ContentLength == 0) { return imgString; }

            var fileBytes = new byte[file.ContentLength];
            int byteCount = file.InputStream.Read(fileBytes, 0, (int)file.ContentLength);
            if (byteCount == 0) { return imgString; }

            var image = Image.FromStream(file.InputStream, true, true);
            var imgf = image.RawFormat;

            using (var ms = new MemoryStream())
            {
                image.Save(ms, imgf);
                ms.Position = 0;
                imgString = Convert.ToBase64String(ms.ToArray(), Base64FormattingOptions.None);
            }

            return "data:image/" + imgf.ToString() + ";base64," + imgString;
        }

        public static string CreateGalleryThumbnail(FileInfo file, Size size)
        {
            var imgString = "";
            var strDir = Path.Combine(folderRoot, "Gallery");
            var targetPath = Path.Combine(strDir, file.Name);
            try
            {
                using (var img = ResizeImage(Image.FromFile(file.FullName, true), size, true))
                {
                    var imgCropped = CenteredCropImage(img, 400, 1320);
                    using (var httpClient = new HttpClient())
                    {
                        imgString = SaveImage(imgCropped, targetPath);
                    }
                    img.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return imgString;
        }

        public static Image CenteredCropImage(Image img, int cropHeight, int cropWidth)
        {
            var cropY = (img.Height / 2) - (cropHeight / 2);
            var cropX = (img.Width / 2) - (cropWidth / 2);
            var cHeight = img.Height > cropHeight ? cropHeight : img.Height;
            var cWidth = img.Width > cropWidth ? cropWidth : img.Width;

            var point = new Point((cropX < 0 ? 0 : cropX), (cropY < 0 ? 0 : cropY));
            var newSize = new Size(cWidth, cHeight);
            var rect = new Rectangle(point, newSize);
            return CropImage(img, rect);
        }

        private static T GetNewImage<T>(Image image, Size size, string mainDirectory, string subDirectory, string newFileName)
            where T : Identity, new()
        {
            if (size.IsEmpty)
            {
                if (image.Width > 2048 || image.Height > 1080)
                {
                    size = new Size(2048, 1080);
                }
                else
                {
                    size = new Size(image.Width, image.Height);
                }
                using (var redub = ResizeImage(image, size))
                {
                    var targetFile = Path.Combine(mainDirectory, newFileName);
                    var imagepath = SaveImage(redub, targetFile);
                    var model = new T()
                    {
                        url = VirtualPathUtility.ToAppRelative(targetFile),
                        width = redub.Width,
                        height = redub.Height,
                        size = Convert.ToInt32((new FileInfo(imagepath)).Length)
                    };
                    redub.Dispose();
                    return model;
                }
            }

            using (var redub = ResizeImage(image, size))
            {
                var filepath = Path.Combine(mainDirectory, subDirectory, newFileName);
                var imagepath = SaveImage(redub, filepath);
                var model = new T()
                {
                    url = VirtualPathUtility.ToAppRelative(filepath),
                    width = redub.Width,
                    height = redub.Height,
                    size = Convert.ToInt32((new FileInfo(imagepath)).Length)
                };
                redub.Dispose();
                return model;
            }
        }

        private static string SaveImage(Image originalImage, string filepath)
        {
            string directory = Path.GetDirectoryName(filepath);
            directory = HttpContext.Current.Server.MapPath(directory);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }            // Encoder parameter for image quality

            filepath = Path.Combine(directory, Path.GetFileName(filepath));
            //quality should be in the range [0..100]
            var qualityParam = new EncoderParameter(Encoder.Quality, 90L);

            // Jpeg image codec
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
            using (var encParams = new EncoderParameters(1))
            {
                encParams.Param[0] = qualityParam;
                originalImage.Save(filepath, jpegCodec, encParams);
            }
            return filepath;
        }

        private static Image ResizeImage(Image image, Size size, bool keepEqualWidth = false)
        {
            float percentWidth = 0;
            float percentHeight = 0;
            float percent = 0;
            int newHeight = 0;
            int newWidth = 0;

            // Prevent using images internal thumbnail
            image.RotateFlip(RotateFlipType.Rotate180FlipNone);
            image.RotateFlip(RotateFlipType.Rotate180FlipNone);

            int originalWidth = image.Width;
            int originalHeight = image.Height;
            percentWidth = (float)size.Width / (float)originalWidth;
            percentHeight = (float)size.Height / (float)originalHeight;

            if (keepEqualWidth)
            {
                percent = percentWidth;
            }
            else
            {
                percent = percentHeight < percentWidth ? percentHeight : percentWidth;
            }
            newWidth = (int)(originalWidth * percent);
            newHeight = (int)(originalHeight * percent);

            Image newImage = new Bitmap(newWidth, newHeight);
            try
            {
                using (var g = Graphics.FromImage(newImage))
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(image, new Rectangle(0, 0, newImage.Width, newImage.Height));
                }
                return newImage;
            }
            catch
            {
                if (newImage != null) newImage.Dispose();
                throw new Exception("Resize Image Error");
            }
        }

        private static Image CropImage(Image image, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(image);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return (Image)(bmpCrop);
        }
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }

        public static void Empty(string folderName, bool deleteDirectory = true)
        {
            folderName = HttpContext.Current.Server.MapPath(folderName);
            if (Directory.Exists(folderName))
            {
                var directory = new DirectoryInfo(folderName);
                directory.GetFiles().ToList().ForEach(f => f.Delete());
                directory.GetDirectories().ToList().ForEach(f => f.Delete(true));
                if (deleteDirectory)
                {
                    directory.Delete();
                }
            }            // Encoder parameter for image quality
        }

        private static void ClearFolder(string folderName)
        {
            var dir = new DirectoryInfo(folderName);
            foreach (var fi in dir.GetFiles())
            {
                fi.IsReadOnly = false;
                fi.Delete();
            }

            foreach (var di in dir.GetDirectories())
            {
                ClearFolder(di.FullName);
                di.Delete();
            }
        }
    }
}
