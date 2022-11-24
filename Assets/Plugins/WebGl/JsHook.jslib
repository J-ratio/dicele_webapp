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
    GameLoaded: function()
    {
        window.gameLoadEvent();
    },
    GameOpen: function(levelNum)
    {
        window.gameOpenEvent(levelNum);
    },
    GameStart: function(time)
    {
        window.gameStartEvent(time);
    },
    GameEnd: function(result,gameTime,movesCount,matchCount,movesTimes)
    {
        window.gameEndEvent(result,gameTime,movesCount,matchCount)
    },
    GameShare: function(Sharetype){
        window.gameShareEvent(Sharetype);
    },
    StatOpen: function()
    {
        window.statOpenEvent();
    },
    HelpOpen: function()
    {
        window.helpOpenEvent();
    },
    ArchiveOpen: function()
    {
        window.archiveOpenEvent();
    },
};
mergeInto(LibraryManager.library, JsPlugin);