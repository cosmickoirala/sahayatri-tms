using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using TravelManagementSystem.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace TravelManagementSystem.Utilities.Helpers
{
    //Image uploader
    public static class ApplicationUtilities
    {
   


        public static Models.Image UploadImage(IWebHostEnvironment webHost, IFormFile ImageFile, string imageType = "Place",string imagePath = "uploads")
        {
             
            Models.Image image = new Models.Image();
            string uniqueFileName = null;
            if (ImageFile != null)
            {
                string uploadsFolder = Path.Combine(webHost.WebRootPath,imagePath,imageType,"Images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                
                uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(fileStream);
                }
 
                image.ImagePath = Path.GetRelativePath(webHost.WebRootPath,filePath);
                image.Name = uniqueFileName;
            }
            
            return image;
        }
        public static List<SelectListItem> LoadDropdowns(string Name, ApplicationDbContext db)
        {
           List<SelectListItem> list = new List<SelectListItem>();
            Name = Name.Trim().ToLower();
            switch (Name)
            {
                case "countries":

                    list = db.Countries.Select(a =>
                                new SelectListItem
                                {
                                    Value = a.Id.ToString(),
                                    Text = a.CountryName
                                }).ToList();
                    list.Insert(0, new SelectListItem { Text = "-- select Country--", Value = "" });
                    return list;
                case "states":

                    list = db.States.Select(a =>
                                new SelectListItem
                                {
                                    Value = a.Id.ToString(),
                                    Text = a.StateName
                                }).ToList();
                    list.Insert(0, new SelectListItem { Text = "-- select State--", Value = "" });
                    return list;
                case "cities":

                    list = db.Cities.Select(a =>
                                new SelectListItem
                                {
                                    Value = a.Id.ToString(),
                                    Text = a.CityName
                                }).ToList();
                    list.Insert(0, new SelectListItem { Text = "-- select City--", Value = "" });
                    return list;
            case "street":

                    list = db.Streets.Select(a =>
                                new SelectListItem
                                {
                                    Value = a.Id.ToString(),
                                    Text = a.StreetName
                                }).ToList();
                    list.Insert(0, new SelectListItem { Text = "-- select street--", Value = "" });
                    return list;
            

            case "placetype":

                    list = new List<SelectListItem>{
                       new SelectListItem{ Text="--Select Type--", Value=""},
                       new SelectListItem{ Text="Adventurous", Value="adventurous"},
                       new SelectListItem{ Text="Religious", Value="religious"},
                       new SelectListItem{ Text="Lakesides", Value="lakesides"},
                       new SelectListItem{ Text="Hills", Value="hills"},
                       new SelectListItem{ Text="Mountains", Value="mountains"},
                       new SelectListItem{ Text="Others", Value="Unspecified"}
                   };
            return list;
        }

            return list;

        }

        public static string GetHtmlRating(double ratingValue)
        {
            string Rating = "";
            double halfstar =ratingValue%1;
            ratingValue = Math.Floor(ratingValue);
            for (int i = 1; i <= ratingValue; i++)
            {
                 Rating += @"<li><i class=""fa fa-star""></i></li>";
            }
            if(halfstar>0)
            {
                Rating += @"<li><i class=""fa fa-star-half""></i></li>";

            }
            return Rating;

        }




    }


    //public class MaxFileSizeAttribute : ValidationAttribute
    //{
    //    private readonly int _maxFileSize;
     
    //    public MaxFileSizeAttribute(int maxFileSize)
    //    {
    //        _maxFileSize = maxFileSize;
    //    }

    //    protected override ValidationResult IsValid(
    //    object value, ValidationContext validationContext)
    //    {
    //        var file = value as IFormFile;
    //        if (file != null)
    //        {
    //            if (file.Length > _maxFileSize)
    //            {
    //                return new ValidationResult(GetErrorMessage());
    //            }
    //        }

    //        return ValidationResult.Success;
    //    }

    //    public string GetErrorMessage()
    //    {
    //        return $"Maximum allowed file size is { _maxFileSize} bytes.";
    //    }
    //}

    //public class CoverImageDimenssionAttribute : ValidationAttribute
    //{
    //    private readonly int _minWidth;
    //    private readonly int _minHeight;

    //    public CoverImageDimenssionAttribute(int minWidth , int minHeight)
    //    {
    //        _minWidth = minWidth;
    //        _minHeight=minHeight;
    //    }

    //    protected override ValidationResult IsValid(
    //    object value, ValidationContext validationContext)
    //    {

    //        var file = value as IFormFile;
    //        if (file != null)
    //        {
    //            if (file.Length > _maxFileSize)
    //            {
    //                return new ValidationResult(GetErrorMessage());
    //            }
    //        }

    //        return ValidationResult.Success;
    //    }

    //    public string GetErrorMessage()
    //    {
    //        return $"Maximum allowed file size is { _maxFileSize} bytes.";
    //    }
    //}


    //public class AllowedExtensionsAttribute : ValidationAttribute
    //{
    //    private readonly string[] _extensions = { "png", "jpg", "jpeg", "gif" };
    //    public AllowedExtensionsAttribute(string[] extensions)
    //    {
    //        _extensions = extensions;
    //    }

    //    protected override ValidationResult IsValid(
    //    object value, ValidationContext validationContext)
    //    {
    //        var file = value as IFormFile;
    //        if (file != null)
    //        {
    //            var extension = Path.GetExtension(file.FileName);
    //            extension = extension.ToLower();
    //            bool validExtension = Array.Exists(_extensions, extension);
    //            if (!validExtension)
    //            {
    //                return new ValidationResult(GetErrorMessage());
    //            }
    //        }

    //        return ValidationResult.Success;
    //    }

    //    public string GetErrorMessage()
    //    {
    //        return $"This photo extension is not allowed!";
    //    }
    //}
}
