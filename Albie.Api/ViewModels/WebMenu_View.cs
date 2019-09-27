using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albie.Api.ViewModels
{
    public class WebMenu_View
    {
        public string Label { get; set; }
        public string IconClass { get; set; }
        public string RouterLink { get; set; }
        public string RouterLinkActive { get; set; }
        public List<WebMenu_View> Children { get; set; }
        public bool HasChildren { get { return this.Children.Any(); } }
        public WebMenuWrapper_View Wrapper { get; set; }

        public WebMenu_View()
        {
            this.Children = new List<WebMenu_View>();
        }
    }
    public class WebMenuWrapper_View
    {
        public string RouterLinkActive { get; set; }
        public WebMenuWrapper_View(string wrapperRrouterLinkActive)
        {
            this.RouterLinkActive = wrapperRrouterLinkActive;
        }
    }
}
