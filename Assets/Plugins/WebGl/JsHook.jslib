var JsPlugin = {


    shareTrigger: function()
    {   
        window.ShareMsg();
    },
    Gform_Redirect: function()
    {
        window.Redirect();
    },
};
mergeInto(LibraryManager.library, JsPlugin);