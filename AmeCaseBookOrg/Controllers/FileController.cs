using AmeCaseBookOrg.Models;
using AmeCaseBookOrg.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmeCaseBookOrg.Controllers
{
    public class FileController : Controller
    {
        private string StorageRoot
        {
            get { return Path.Combine(Server.MapPath("~/Files")); }
        }
        private IFileService fileService;
        public FileController (IFileService service)
        {
            fileService = service;
        }
        // GET: File
        public ActionResult Index(int id)
        {
            var fileToRetrieve = fileService.getFile(id);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }
        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        [HttpPost]
        public JsonResult Delete(int id)
        {
            Models.File file = fileService.getFile(id);
            if (file != null)
            {
                fileService.deleteFile(file);
            }
            return Json(new { result = true });
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        [HttpGet]
        public ActionResult Download(int id)
        {
            var context = HttpContext;
            var file = fileService.getFile(id);
            if(file == null)
            {
                context.Response.StatusCode = 404;
                return HttpNotFound();
            }    
            context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + file.FileName + "\"");
            context.Response.ContentType = "application/octet-stream";
            context.Response.ClearContent();
            context.Response.Write(file.Content);
            return Json(new { result = true }, JsonRequestBehavior.AllowGet); 
        }
        [HttpPost]
        public ActionResult UploadFiles()
        {
            var r = new List<ViewDataUploadFilesResult>();

            foreach (string file in Request.Files)
            {
                var statuses = new List<ViewDataUploadFilesResult>();
                var headers = Request.Headers;

                if (string.IsNullOrEmpty(headers["X-File-Name"]))
                {
                    UploadWholeFile(Request, statuses);
                }
                else
                {
                    UploadPartialFile(headers["X-File-Name"], Request, statuses);
                }
                var jsonData = new
                {
                    files = (from s in statuses
                             select new
                             {
                                 id = s.id,
                                 name = s.name,
                                 url = s.url,
                                 thumbnailUrl = s.thumbnail_url,
                                 deleteUrl = s.delete_url,
                                 deleteType = s.delete_type,
                                 size = s.size
                             })
                };
                
                JsonResult result = Json(jsonData);
                result.ContentType = "application/json";
                return result;
            }

            return Json(r);
        }
        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        //Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        private void UploadPartialFile(string fileName, HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        {
            if (request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var file = request.Files[0];
            var inputStream = file.InputStream;

            var fullName = Path.Combine(StorageRoot, Path.GetFileName(fileName));

            using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
            {
                var buffer = new byte[1024];

                var l = inputStream.Read(buffer, 0, 1024);
                while (l > 0)
                {
                    fs.Write(buffer, 0, l);
                    l = inputStream.Read(buffer, 0, 1024);
                }
                fs.Flush();
                fs.Close();
            }
            var avatar = new Models.File
            {
                FileName = fileName,
                FileType = FileType.Avatar,
                ContentType = file.ContentType,
                Content = System.IO.File.ReadAllBytes(fullName)
            };
            Models.File fileModel = fileService.addFile(avatar);
            statuses.Add(new ViewDataUploadFilesResult()
            {
                id = fileModel.FileId,
                name = fileName,
                size = file.ContentLength,
                type = file.ContentType,
                url = "/File/Download?id=" + fileModel.FileId,
                delete_url = "/File/Delete?id=" + fileModel.FileId,
                thumbnail_url = @"data:image/png;base64," + EncodeFile(fullName),
                delete_type = "GET",
            });
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        //Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        private void UploadWholeFile(HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        {
            for (int i = 0; i < request.Files.Count; i++)
            {
                var upload = request.Files[i];
                if (upload != null && upload.ContentLength > 0)
                {
                    String fileName = System.IO.Path.GetFileName(upload.FileName);
                    String contentType = upload.ContentType;
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        var avatar = new Models.File
                        {
                            FileName = fileName,
                            FileType = FileType.Avatar,
                            ContentType = contentType,
                            Content = reader.ReadBytes(upload.ContentLength)
                        };
                        Models.File file = fileService.addFile(avatar);
                        statuses.Add(new ViewDataUploadFilesResult()
                        {
                            id = file.FileId,
                            name = upload.FileName,
                            size = upload.ContentLength,
                            type = upload.ContentType,
                            url = "/File/Download?id=" + file.FileId,
                            delete_url = "/File/Delete?id=" + file.FileId,
                            thumbnail_url = @"data:image/png;base64," + EncodeFile(file.Content),
                            delete_type = "POST",
                        });
                    }
                }
                
            }
        }
        private string EncodeFile(string fileName)
        {
            return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName));
        }
        private string EncodeFile(byte[] data)
        {
            return Convert.ToBase64String(data);
        }
    }

    public class ViewDataUploadFilesResult
    {
        public int id { get; set; }
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string delete_url { get; set; }
        public string thumbnail_url { get; set; }
        public string delete_type { get; set; }
    }
}