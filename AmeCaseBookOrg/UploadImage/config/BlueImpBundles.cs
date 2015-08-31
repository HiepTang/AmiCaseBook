using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Optimization;

namespace Backload.Bundles
{

    /// <summary>
    /// Registers bundles for client side scripts and styles. This is an optional feature.
    /// </summary>
    public class BlueImpBundles
    {

        /// <summary>
        /// Registers bundles for the client side scripts and styles
        /// </summary>
        /// <param name="bundles">A BundleCollection instance</param>
        /// <remarks>This is optional. The Backload component does not need bundeling internally.</remarks>
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Default path to the client side files
            string vendor = "~/UploadImage/";
            string plugin = "fileupload";
            string jsplugin = string.Empty;
            string cssplugin = string.Empty;
            

            
            #region Bundles for the jQuery File Upload Plugin
           
            jsplugin = string.Format("{0}{1}/js/", vendor, plugin);
            cssplugin = string.Format("{0}{1}/css/", vendor, plugin);

 
            
            #region jQuery File Upload Plugin: Basic theme (Bootstrap)

            string[] scripts = new string[] {
                jsplugin + "vendor/jquery.ui.widget.js",
                jsplugin + "jquery.iframe-transport.js",
                jsplugin + "jquery.fileupload.js",
                jsplugin + "themes/jquery.fileupload-themes.js" };
            
            string[] styles = new string[] {
                cssplugin + "jquery.fileupload.css" };
            

            bundles.Add(new ScriptBundle("~/backload/blueimp/bootstrap/Basic").Include(scripts));
            bundles.Add(new StyleBundle("~/backload/blueimp/bootstrap/Basic/css").Include(styles));

            // The following virtual path is for backward compatibility only and can be removed
            bundles.Add(new ScriptBundle("~/bundles/fileUpload/bootstrap/Basic/js").Include(scripts));
            bundles.Add(new StyleBundle("~/bundles/fileUpload/bootstrap/Basic/css").Include(styles));
            
            #endregion



            #region jQuery File Upload Plugin: Basic Plus (Bootstrap)

            scripts = new string[] {
                vendor + "loadimage/js/load-image.all.min.js",
                vendor + "blob/js/canvas-to-blob.min.js",
                jsplugin + "vendor/jquery.ui.widget.js",
                jsplugin + "jquery.iframe-transport.js",
                jsplugin + "jquery.fileupload.js",
                jsplugin + "jquery.fileupload-process.js",
                jsplugin + "jquery.fileupload-image.js",
                jsplugin + "jquery.fileupload-audio.js",
                jsplugin + "jquery.fileupload-video.js",
                jsplugin + "jquery.fileupload-validate.js",
                jsplugin + "themes/jquery.fileupload-themes.js" };

            styles = new string[] {
                cssplugin + "jquery.fileupload.css" };


            bundles.Add(new ScriptBundle("~/backload/blueimp/bootstrap/BasicPlus").Include(scripts));
            bundles.Add(new StyleBundle("~/backload/blueimp/bootstrap/BasicPlus/css").Include(styles));

            // The following virtual path is for backward compatibility only and can be removed
            bundles.Add(new ScriptBundle("~/bundles/fileUpload/bootstrap/BasicPlus/js").Include(scripts));
            bundles.Add(new StyleBundle("~/bundles/fileUpload/bootstrap/BasicPlus/css").Include(styles));
            
            #endregion



            #region jQuery File Upload Plugin: Basic Plus UI (Bootstrap)

            scripts = new string[] {
                vendor + "templates/js/tmpl.min.js",
                vendor + "loadimage/js/load-image.all.min.js",
                vendor + "blob/js/canvas-to-blob.min.js",
                vendor + "gallery/js/jquery.blueimp-gallery.min.js",
                jsplugin + "vendor/jquery.ui.widget.js",
                jsplugin + "jquery.iframe-transport.js",
                jsplugin + "jquery.fileupload.js",
                jsplugin + "jquery.fileupload-process.js",
                jsplugin + "jquery.fileupload-image.js",
                jsplugin + "jquery.fileupload-audio.js",
                jsplugin + "jquery.fileupload-video.js",
                jsplugin + "jquery.fileupload-validate.js",
                jsplugin + "jquery.fileupload-ui.js",
                jsplugin + "themes/jquery.fileupload-themes.js" };

            styles = new string[] {
                vendor + "gallery/css/blueimp-gallery.min.css",
                cssplugin + "jquery.fileupload.css",
                cssplugin + "jquery.fileupload-ui.css" };


            bundles.Add(new ScriptBundle("~/backload/blueimp/bootstrap/BasicPlusUI").Include(scripts));
            bundles.Add(new StyleBundle("~/backload/blueimp/bootstrap/BasicPlusUI/css").Include(styles));

            // The following virtual path is for backward compatibility only and can be removed
            bundles.Add(new ScriptBundle("~/bundles/fileUpload/bootstrap/BasicPlusUI/js").Include(scripts));
            bundles.Add(new StyleBundle("~/bundles/fileUpload/bootstrap/BasicPlusUI/css").Include(styles));
            
            #endregion



            #region jQuery File Upload Plugin: AngularJS theme

            scripts = new string[] {
                vendor + "loadimage/js/load-image.all.min.js",
                vendor + "blob/js/canvas-to-blob.min.js",
                vendor + "gallery/js/jquery.blueimp-gallery.min.js",
                jsplugin + "vendor/jquery.ui.widget.js",
                jsplugin + "jquery.iframe-transport.js",
                jsplugin + "jquery.fileupload.js",
                jsplugin + "jquery.fileupload-process.js",
                jsplugin + "jquery.fileupload-image.js",
                jsplugin + "jquery.fileupload-audio.js",
                jsplugin + "jquery.fileupload-video.js",
                jsplugin + "jquery.fileupload-validate.js",
                jsplugin + "jquery.fileupload-angular.js" };

            styles = new string[] {
                vendor + "gallery/css/blueimp-gallery.min.css",
                cssplugin + "jquery.fileupload.css",
                cssplugin + "jquery.fileupload-ui.css" };


            bundles.Add(new ScriptBundle("~/backload/blueimp/angularjs").Include(scripts));
            bundles.Add(new StyleBundle("~/backload/blueimp/angularjs/css").Include(styles));

            // The following virtual path is for backward compatibility only and can be removed
            bundles.Add(new ScriptBundle("~/bundles/fileUpload/angularjs/js").Include(scripts));
            bundles.Add(new StyleBundle("~/bundles/fileUpload/angularjs/css").Include(styles));
            
            #endregion



            #region jQuery File Upload Plugin: jQueryUI theme

            scripts = new string[] {
                vendor + "templates/js/tmpl.min.js",
                vendor + "loadimage/js/load-image.all.min.js",
                vendor + "blob/js/canvas-to-blob.min.js",
                vendor + "gallery/js/jquery.blueimp-gallery.min.js",
                jsplugin + "jquery.iframe-transport.js",
                jsplugin + "jquery.fileupload.js",
                jsplugin + "jquery.fileupload-process.js",
                jsplugin + "jquery.fileupload-image.js",
                jsplugin + "jquery.fileupload-audio.js",
                jsplugin + "jquery.fileupload-video.js",
                jsplugin + "jquery.fileupload-validate.js",
                jsplugin + "jquery.fileupload-ui.js",
                jsplugin + "jquery.fileupload-jquery-ui.js" };

            styles = new string[] {
                vendor + "gallery/css/blueimp-gallery.min.css",
                cssplugin + "jquery.fileupload.css",
                cssplugin + "jquery.fileupload-ui.css" };


            bundles.Add(new ScriptBundle("~/backload/blueimp/jqueryui").Include(scripts));
            bundles.Add(new StyleBundle("~/backload/blueimp/jqueryui/css").Include(styles));

            // The following virtual path is for backward compatibility only and can be removed
            bundles.Add(new ScriptBundle("~/bundles/fileupload/jqueryui/BasicPlusUI/js").Include(scripts));
            bundles.Add(new StyleBundle("~/bundles/fileupload/jqueryui/BasicPlusUI/css").Include(styles));
            
            #endregion            
            
            #endregion
 
     
        }
        
    }
}
