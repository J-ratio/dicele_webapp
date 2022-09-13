var JsPlugin = {


    storeMovesCount: function(movesCount,minutes,seconds)
    {   
        window.StoreData(movesCount,minutes,seconds);
    },
};
mergeInto(LibraryManager.library, JsPlugin);