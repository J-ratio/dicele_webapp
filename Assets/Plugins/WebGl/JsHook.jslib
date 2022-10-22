var JsPlugin = {


    shareTrigger: function(msg)
    {   
        window.ShareMsg(Pointer_stringify(msg));
    },
    Gform_Redirect: function()
    {
        window.Redirect();
    },
    OnFinish: function(isSolved,finalArr,swaps)
    {
        window.onFinish(isSolved,Pointer_stringify(finalArr),swaps);
    },
};
mergeInto(LibraryManager.library, JsPlugin);