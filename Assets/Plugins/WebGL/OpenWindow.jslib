var OpenWindowPlugin = {
    openWindow: function(url)
    {
        window.open(UTF8ToString(url));
    }
};

mergeInto(LibraryManager.library, OpenWindowPlugin);