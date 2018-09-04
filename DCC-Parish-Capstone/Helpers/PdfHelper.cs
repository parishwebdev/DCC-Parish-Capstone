using SelectPdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCC_Parish_Capstone.Helpers
{
    public class PdfHelper
    {

        public FileResult DownloadArticleAsPDF(string html, string articleName)
        {
            // read parameters from the webpage
            string htmlString = html;

            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.CssMediaType = HtmlToPdfCssMediaType.Screen;
            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertHtmlString(htmlString);


            // save pdf document
            byte[] pdf = doc.Save();

            // close pdf document
            doc.Close();

            // return resulted pdf document
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = articleName + ".pdf";

            return fileResult;
        }


    }
}