using System;
using System.Text;
using System.Web;
using System.Xml;

namespace LIB.Extensions
{
    public static class EHttpRequest
    {
        public static string GetIpAddress(this HttpRequest request)
        {
            string ipaddress = String.Empty;

            if (request != null)
            {
                ipaddress = (request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null) ? request.ServerVariables["HTTP_X_FORWARDED_FOR"] : request.ServerVariables["REMOTE_ADDR"];
            }

            return ipaddress;
        }

        public static XmlDocument GetServerVariablesAsXml(this HttpRequest request)
        {
            XmlDocument xml = new XmlDocument();

            if (request != null)
            {
                XmlElement root = xml.CreateElement("HttpRequest");
                XmlAttribute attr1 = xml.CreateAttribute("xmlns:xsi");
                attr1.Value = "http://www.w3.org/2001/XMLSchema-instance";
                root.Attributes.Append(attr1);

                XmlAttribute attr2 = xml.CreateAttribute("xmlns:xsd");
                attr2.Value = "http://www.w3.org/2001/XMLSchema";
                root.Attributes.Append(attr2);

                xml.AppendChild(root);

                foreach (string i in request.ServerVariables)
                {
                    if (i != "ALL_HTTP" && i != "ALL_RAW")
                    {
                        XmlElement child = xml.CreateElement(i);
                        XmlCDataSection cdata = xml.CreateCDataSection(request.ServerVariables[i]);
                        child.AppendChild(cdata);
                        root.AppendChild(child);
                    }
                }
            }
            return xml;
        }

        public static string GetServerVariablesAsHtml(this HttpRequest request)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table><tr><th colspan=2>Server Variables</th></tr>");
            foreach (string i in request.ServerVariables)
            {
                if (i != "ALL_HTTP" && i != "ALL_RAW")
                {
                    sb.Append("<tr><td>");
                    sb.Append(i);
                    sb.Append("</td><td>");
                    sb.Append(HttpUtility.HtmlEncode(request.ServerVariables[i]));
                    sb.Append("</td></tr>");
                }
            }
            sb.Append("</table>");

            return sb.ToString();
        }
    }
}
