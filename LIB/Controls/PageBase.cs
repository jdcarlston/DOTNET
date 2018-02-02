using LIB.Data;
using LIB.Extensions;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace LIB.Controls
{
    public class PageBase : System.Web.UI.Page
    {
        private string _metarobots;
        private string _metakeywords;
        private string _metadescription;
        private string _loc;
        private string _imgloc;

        public string MetaRobots
        {
            get { return _metarobots; }
            set { _metarobots = value; }
        }
        public new string MetaKeywords
        {
            get { return _metakeywords; }
            set { _metakeywords = value; }
        }
        public new string MetaDescription
        {
            get { return _metadescription; }
            set { _metadescription = value; }
        }

        public string PageName { get { return (new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath)).Name; } }
        public string Loc { get { return (!_loc.IsNullOrEmpty()) ? _loc : ResolveUrl("~/").ToString(); } set { _loc = value; } }
        public string ImgLoc
        {
            get { return (!_imgloc.IsNullOrEmpty()) ? _imgloc : this.Loc + "App_Themes/Default/images/"; }
            set { _imgloc = value; }
        }

        public string Path_Root { get { return AppCache.SitePaths.FirstOrDefault(p => p.Key.Equals("Root")).Value; } }

        private static string[] aspNetFormElements = new string[]
        {
        "__EVENTTARGET",
        "__EVENTARGUMENT",
        "__VIEWSTATE",
        "__EVENTVALIDATION",
        "__VIEWSTATEENCRYPTED",
        };

        protected override void Render(HtmlTextWriter writer)
        {
            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
            base.Render(htmlWriter);
            string html = stringWriter.ToString();
            int formStart = html.IndexOf("<form");
            int endForm = -1;
            if (formStart >= 0)
                endForm = html.IndexOf(">", formStart);

            if (endForm >= 0)
            {
                StringBuilder viewStateBuilder = new StringBuilder();
                foreach (string element in aspNetFormElements)
                {
                    int startPoint = html.IndexOf("<input type=\"hidden\" name=\"" + element + "\"");
                    if (startPoint >= 0 && startPoint > endForm)
                    {
                        int endPoint = html.IndexOf("/>", startPoint);
                        if (endPoint >= 0)
                        {
                            endPoint += 2;
                            string viewStateInput = html.Substring(startPoint, endPoint - startPoint);
                            html = html.Remove(startPoint, endPoint - startPoint);
                            viewStateBuilder.Append(viewStateInput).Append("\r\n");
                        }
                    }
                }

                if (viewStateBuilder.Length > 0)
                {
                    viewStateBuilder.Insert(0, "\r\n");
                    html = html.Insert(endForm + 1, viewStateBuilder.ToString());
                }
            }

            writer.Write(html);
        }
    }
}
