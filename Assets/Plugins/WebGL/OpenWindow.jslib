var OpenWindowPlugin = {
    openWindow: function(link)
    {
    	var url = Pointer_stringify(link);
        document.onmousedown = function()
        {
        	window.open(url);
        	document.onmousedown = null;
        }
    }
};

mergeInto(LibraryManager.library, OpenWindowPlugin);