namespace MyServer.ViewComponents.ImageGallery._Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Microsoft.Extensions.Localization;

    using MyServer.Common;
    using MyServer.Common.ImageGallery;
    using MyServer.Data.Models;
    using MyServer.ViewComponents.ImageGallery.Resources;

    public static class MappingFunctions
    {
        public static IStringLocalizer<ViewComponentResources> SharedLocalizer { get; set; }

        public static void LoadResource(IStringLocalizer<ViewComponentResources> localizerParam)
        {
            SharedLocalizer = localizerParam;
        }

        public static string MapCoverImage(Album source)
        {
            return Constants.MainContentFolder + "/" + source.CoverAlbumId + "/" + Constants.ImageFolderLow + "/"
                   + source.CoverAlbumFilename;
        }

        public static string MapDate(Album source)
        {
            if (source.ImagesCount == 0)
            {
                return string.Empty;
            }

            if (source.FirstTmageTaken != null && source.LastTmageTaken != null)
            {
                var firstDate = source.FirstTmageTaken.Value;
                var lastDate = source.LastTmageTaken.Value;

                if (firstDate.Date == lastDate.Date)
                {
                    return firstDate.ToString("dd MMMM yyyy");
                }
                else if (firstDate.Year == lastDate.Year && firstDate.Month == lastDate.Month)
                {
                    return firstDate.Day + "-" + lastDate.Day + " " + firstDate.ToString("MMMM yyyy");
                }
                else if (firstDate.Year == lastDate.Year)
                {
                    return firstDate.ToString("dd MMMM") + "-" + lastDate.ToString("dd MMMM") + " "
                           + lastDate.ToString("yyyy");
                }
                else
                {
                    return firstDate.ToString("dd MMMM yyyy") + "-" + lastDate.ToString("dd MMMM yyyy");
                }
            }

            return string.Empty;
        }

        public static string MapDescription(Album source)
        {
            var culture = CultureInfo.CurrentCulture.ToString();

            if (culture == "bg-BG")
            {
                return source.DescriptionBg;
            }
            else if (culture == "en-US")
            {
                return source.DescriptionEn;
            }

            return null;
        }

        public static string MapFbImage(Album source)
        {
            return Constants.MainContentFolder + "/" + source.CoverAlbumId + "/" + Constants.ImageFolderMiddle + "/"
                   + source.CoverAlbumFilename;
        }

        public static List<double> MapGpsCoordinates(Image source)
        {
            return (source.GpsLatitude != null && source.GpsLongitude != null)
                       ? new List<double>() { source.GpsLatitude.Value, source.GpsLongitude.Value }
                       : null;
        }

        public static string MapGpsName(Image source)
        {
            return source.GpsLocationName;
        }

        public static int MapHeight(Album source)
        {
            return source.CoverAlbumId == null
                       ? Convert.ToInt32(Convert.ToDouble(Constants.ImageLowMaxSize) / 1.5)
                       : source.CoverLowHeight;
        }

        //public static IEnumerable<ImageGpsData> MapImageCoordinates(Album source)
        //{
        //    return null;
        //    //;source.Images?.Where(x => x.ImageGpsData != null).Select(x => x.ImageGpsData).Distinct();
        //}

        public static string MapImagesCountCover(Album source)
        {
            switch (source.ImagesCount)
            {
                case 0: return SharedLocalizer["NoItems"];
                case 1: return "1 " + SharedLocalizer["Item"];
                default: return source.ImagesCount + " " + SharedLocalizer["Items"];
            }
        }

        public static string MapInfo(Image source)
        {
            var result = new StringBuilder();

            if (!string.IsNullOrEmpty(source.Title))
            {
                result.Append(source.Title + "<br/>");
            }

            result.Append("<small>");

            if (!string.IsNullOrEmpty(source.CameraMaker))
            {
                result.Append(source.CameraMaker + " ");
            }

            if (!string.IsNullOrEmpty(source.CameraModel))
            {
                result.Append(source.CameraModel + " ");
            }

            if (!string.IsNullOrEmpty(source.Aperture))
            {
                result.Append(source.Aperture + " ");
            }

            if (!string.IsNullOrEmpty(source.ShutterSpeed))
            {
                result.Append(source.ShutterSpeed + " sec ");
            }

            if (source.FocusLen != null)
            {
                result.Append(source.FocusLen + " mm ");
            }

            if (source.Iso != null)
            {
                result.Append(source.Iso + " ISO");
            }

            if (source.DateTaken != null)
            {
                result.Append("<br/>" + source.DateTaken.Value.ToString("dd-MMMM-yy"));
            }

            result.Append("</small>");

            return result.ToString();
        }

        public static MyServerAccessType MapAccess(Album source)
        {
            return (MyServerAccessType)source.Access;
        }

        public static string MapTitle(Album source)
        {
            var culture = CultureInfo.CurrentCulture.ToString();

            if (culture == "bg-BG")
            {
                return source.TitleBg;
            }
            else if (culture == "en-US")
            {
                return source.TitleEn;
            }

            return null;
        }

        public static int MapWidth(Album source)
        {
            return source.CoverAlbumId == null ? Constants.ImageLowMaxSize : source.CoverLowWidth;
        }
    }
}