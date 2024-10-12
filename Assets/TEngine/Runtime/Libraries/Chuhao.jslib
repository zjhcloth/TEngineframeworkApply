mergeInto(LibraryManager.library, {
    getStartTime : function() {
        //console.log("++++++++++++++++++++++++var", window.myGlobalVar);
        return window.myGlobalVar;
    }
});
