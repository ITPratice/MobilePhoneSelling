using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Common;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class FileUploadController : Controller
    {
        public ActionResult FileUpload()
        {
            return View();
        }

        public ActionResult FileUploadPartial()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file_Uploader)
        {
            if (file_Uploader != null)
            {
                string fileName = string.Empty;
                string destinationPath = string.Empty;

                List<FileUploadModel> uploadFileModel = new List<FileUploadModel>();

                fileName = Path.GetFileName(file_Uploader.FileName);
                destinationPath = Path.Combine(Server.MapPath(Constants.PATH_IMAGE), fileName);
                file_Uploader.SaveAs(destinationPath);
                destinationPath = Constants.PATH_IMAGE + "/" + fileName;
                if (Session["FileUploader"] != null)
                {
                    var isFileNameRepeat = ((List<FileUploadModel>)Session["FileUploader"]).Find(x => x.FileName == fileName);
                    if (isFileNameRepeat == null)
                    {
                        uploadFileModel.Add(new FileUploadModel { FileName = fileName, FilePath = destinationPath });
                        ((List<FileUploadModel>)Session["FileUploader"]).Add(new FileUploadModel { FileName = fileName, FilePath = destinationPath });
                    }
                }
                else
                {
                    uploadFileModel.Add(new FileUploadModel { FileName = fileName, FilePath = destinationPath });
                    Session["FileUploader"] = uploadFileModel;
                }
            }
            return View();
        }

        [HttpParamAction]
        public ActionResult Upload(HttpPostedFileBase fileBase)
        {
            if (fileBase != null)
            {
                string fileName = string.Empty;
                string destinationPath = string.Empty;

                List<FileUploadModel> uploadFileModel = new List<FileUploadModel>();

                fileName = Path.GetFileName(fileBase.FileName);
                destinationPath = Path.Combine(Server.MapPath(Constants.PATH_IMAGE), fileName);
                fileBase.SaveAs(destinationPath);
                destinationPath = Constants.PATH_IMAGE + "/" + fileName;
                if (Session["FileUploader"] != null)
                {
                    var isFileNameRepeat = ((List<FileUploadModel>)Session["FileUploader"]).
                        Find(x => x.FileName == fileName);
                    if (isFileNameRepeat == null)
                    {
                        uploadFileModel.Add(new FileUploadModel { FileName = fileName, FilePath = destinationPath });
                        ((List<FileUploadModel>)Session["FileUploader"]).Add(
                            new FileUploadModel { FileName = fileName, FilePath = destinationPath });
                    }
                }
                else
                {
                    uploadFileModel.Add(new FileUploadModel { FileName = fileName, FilePath = destinationPath });
                    Session["FileUploader"] = uploadFileModel;
                }
            }
            return View();
        }

        public ActionResult RemoveUploadFile(string fileName)
        {
            try
            {
                if (Session["FileUploader"] != null)
                {
                    ((List<FileUploadModel>)Session["FileUploader"]).RemoveAll(x => x.FileName == fileName);
                    if (((List<FileUploadModel>)Session["FileUploader"]).Count == 0) Session["FileUploader"] = null;
                    if (!String.IsNullOrEmpty(fileName))
                    {
                        string fullPath = Request.MapPath(Constants.PATH_IMAGE + "/" + fileName);
                        if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("FileUpload");
        }
    }
}