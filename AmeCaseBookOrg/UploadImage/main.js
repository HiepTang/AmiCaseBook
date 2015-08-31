function initUploadFile( inputTag, url) {
    'use strict';

    // We use the upload handler integrated into Backload:
    // In this example we set an objectContect (id) in the url query (or as form parameter).
    // You can use a user id as objectContext give users only access to their own uploads.
   // var url = '/Backload/FileHandler?objectContext=C5F260DD3787';
    //var url = '/Home/UploadFiles';
    // Initialize the jQuery File Upload widget:
    inputTag.fileupload({
        autoUpload : true,
        maxChunkSize: 10000000,                                          // Optional: file chunking with 10MB chunks
        acceptFileTypes: /(jpg)|(jpeg)|(png)|(gif)|(txt)|(pdf)$/i,              // Allowed file types
        filesContainer: "#tbMainContainer",
        done: function (e, data) {
            if (e.isDefaultPrevented()) {
                return false;
            }
            var that = $(this).data('blueimp-fileupload') ||
                    $(this).data('fileupload'),
                getFilesFromResponse = data.getFilesFromResponse ||
                    that.options.getFilesFromResponse,
                files = getFilesFromResponse(data),
                template,
                deferred;
            if (data.context) {
                data.context.each(function (index) {
                    var file = files[index] ||
                            { error: 'Empty file upload result' };
                    $('<input name="upoadedfile[]" value="'+ file.id +'"/>').appendTo(document.body);
                    deferred = that._addFinishedDeferreds();
                    that._transition($(this)).done(
                        function () {
                            var node = $(this);
                            template = that._renderDownload([file])
                                .replaceAll(node);
                            that._forceReflow(template);
                            that._transition(template).done(
                                function () {
                                    data.context = $(this);
                                    that._trigger('completed', e, data);
                                    that._trigger('finished', e, data);
                                    deferred.resolve();
                                }
                            );
                        }
                    );
                });
            } else {
                template = that._renderDownload(files)[
                    that.options.prependFiles ? 'prependTo' : 'appendTo'
                ](that.options.filesContainer);
                that._forceReflow(template);
                deferred = that._addFinishedDeferreds();
                that._transition(template).done(
                    function () {
                        data.context = $(this);
                        that._trigger('completed', e, data);
                        that._trigger('finished', e, data);
                        deferred.resolve();
                    }
                );
            }
        }
    });
    if (url != null) {
        inputTag.options.url = url;
    }
    
    inputTag.bind('fileuploadsubmit', function (e, data) {
        // Optional: We add a random uuid form parameter. On chunk uploads the uuid is used to store the chunks.
        data.formData = { uuid: Math.random().toString(36).substr(2, 8) };
    });



    //// Load existing files:
    //$('#fileupload').addClass('fileupload-processing');
    //$.ajax({
    //    // Uncomment the following to send cross-domain cookies:
    //    // xhrFields: {withCredentials: true},
    //    url: url,
    //    dataType: 'json',
    //    context: $('#fileupload')[0]
    //}).always(function () {
    //    $(this).removeClass('fileupload-processing');
    //}).done(function (result) {
    //    $(this).fileupload('option', 'done')
    //        .call(this, $.Event('done'), { result: result });
    //});
};
