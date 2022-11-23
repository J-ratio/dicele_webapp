var JsPlugin = {


    storeShare: function(msg){
        window.StoreShareMsg(Pointer_stringify(msg));
    },
    shareTrigger: function(msg)
    {   
        window.ShareMsg(Pointer_stringify(msg));
    },
    ArchiveShareTrigger: function(msg,num,count,time)
    {   
        window.ArchiveShareMsg(Pointer_stringify(msg),num,count,time);
    },
    Gform_Redirect: function()
    {
        window.Redirect();
    },
    OnFinish: function(isSolved,finalArr,swaps,time)
    {
        window.onFinish(isSolved,Pointer_stringify(finalArr),swaps,time);
    },
    OnArchiveFinish: function(isSolved,swaps,archiveNum)
    {
        window.onArchiveFinish(isSolved,swaps,archiveNum);
    },
};
mergeInto(LibraryManager.library, JsPlugin);