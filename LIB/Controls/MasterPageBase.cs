using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Controls
{
    public class MasterPageBase : System.Web.UI.MasterPage
    {
        public PageBase PageBase { get { return (PageBase)this.Page; } }

        public string PageName { get { return (new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath)).Name; } }
        public string Loc { get { return PageBase.Loc; } set { PageBase.Loc = value; } }
        public string ImgLoc { get { return PageBase.ImgLoc; } set { PageBase.ImgLoc = value; } }

        public string MetaRobots { get { return PageBase.MetaRobots; } }
        public string MetaKeywords { get { return PageBase.MetaKeywords; } }
        public string MetaDescription { get { return PageBase.MetaDescription; } }

        public string Path_Root { get { return PageBase.Path_Root; } }

    }
}
