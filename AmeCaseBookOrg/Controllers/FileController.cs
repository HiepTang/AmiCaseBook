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

                if (string.IsNullOrEmpty(headers["Content-Range"]))
                {
                    UploadWholeFile(Request, statuses);
                }
                else
                {
                    UploadPartialFile(headers["Content-Range"], Request, statuses);
                }
                var jsonData = new
                {
                    files = (from s in statuses
                             select new
                             {
                                 id = s.id,
                                 name = s.name,
                                 url = s.url,
                                 thumbnailUrl = "",
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
        private void UploadPartialFile( string contentRange, HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        {
            if (request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");

            string[] ranges = contentRange.Split(new char[] { ' ', '-', '/' });

            var upload = request.Files[0];
            if (upload != null && upload.ContentLength > 0)
            {
                String fileName = System.IO.Path.GetFileName(upload.FileName);
                String contentType = upload.ContentType;
                Models.File currFile = null;
                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {
                    byte[] content = reader.ReadBytes(upload.ContentLength);
                    if (ranges.Length>1 && ranges[1] == "0")
                    {
                        //this is first part => create new file;
                        var avatar = new Models.File
                        {
                            FileName = fileName,
                            FileType = FileType.Avatar,
                            ContentType = contentType,
                            Content = content
                        };
                        currFile = fileService.addFile(avatar);
                    }
                    else
                    {
                        currFile = fileService.getFile(fileName);
                        //append the file content
                        byte[] newContent = new byte[currFile.Content.Length + content.Length];
                        Array.Copy(currFile.Content, newContent, currFile.Content.Length);
                        Array.Copy(content, 0, newContent, currFile.Content.Length, content.Length);
                        currFile.Content = newContent;
                        fileService.saveFile();
                    }
                    string imageUrl = "/File/Download?id=" + currFile.FileId;
                    if (upload.ContentType.Contains("image"))
                    {
                        imageUrl = "/File?id=" + currFile.FileId;
                    }
                    statuses.Add(new ViewDataUploadFilesResult()
                    {
                        id = currFile.FileId,
                        name = upload.FileName,
                        size = currFile.Content.Length,
                        type = upload.ContentType,
                        url = imageUrl,
                        delete_url = "/File/Delete?id=" + currFile.FileId,
                        thumbnail_url = @"data:image/png;base64," + EncodeFile(currFile.Content),
                        delete_type = "POST",
                    });
                }
            }  
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
                       
                        string imageUrl = "/File/Download?id=" + file.FileId;
                        if (upload.ContentType.Contains("image"))
                        {
                            imageUrl = "/File?id=" + file.FileId;
                        }
                        statuses.Add(new ViewDataUploadFilesResult()
                        {
                            id = file.FileId,
                            name = upload.FileName,
                            size = upload.ContentLength,
                            type = upload.ContentType,
                            url = imageUrl,
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