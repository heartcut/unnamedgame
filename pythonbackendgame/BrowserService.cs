using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace pythonbackendgame
{
    //i use this bit of javascript to get the height and width dimensions of the browser window
    public class BrowserService
    {
        private readonly IJSRuntime _js;

        public BrowserService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<BrowserDimension> GetDimensions()
        {
            return await _js.InvokeAsync<BrowserDimension>("getDimensions");
        }

        public async Task<ElementLocation> GetElementRect(string element)
        {
            return await _js.InvokeAsync<ElementLocation>("GetElementLocation", element);
        }
    }

    public class BrowserDimension
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class ElementLocation
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
    }
}
