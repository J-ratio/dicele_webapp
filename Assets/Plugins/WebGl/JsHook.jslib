var JsPlugin = {


    shareTrigger: function(msg)
    {   
        window.ShareMsg(Pointer_stringify(msg));
    },
    ArchiveShareTrigger: function(msg,num,count)
    {   
        window.ArchiveShareMsg(Pointer_stringify(msg),num,count);
    },
    Gform_Redirect: function()
    {
        window.Redirect();
    },
    OnFinish: function(isSolved,finalArr,swaps)
    {
        window.onFinish(isSolved,Pointer_stringify(finalArr),swaps);
    },
    OnArchiveFinish: function(isSolved,swaps,archiveNum)
    {
        window.onArchiveFinish(isSolved,swaps,archiveNum);
    },
};
mergeInto(LibraryManager.library, JsPlugin);