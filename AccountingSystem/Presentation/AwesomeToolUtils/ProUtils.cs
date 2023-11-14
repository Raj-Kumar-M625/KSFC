using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Omu.Awem.Helpers;
using Omu.AwesomeMvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Utils
{
    public static class ProUtils
    {
        private static IUrlHelper GetUrlHelper<T>(IHtmlHelper<T> html)
        {
            return ((IUrlHelperFactory)html.ViewContext.HttpContext.RequestServices.GetService(typeof(IUrlHelperFactory))).GetUrlHelper(html.ViewContext);
        }

        public static Column EditColumn<T>(IHtmlHelper<T> html, string gridId)
        {
            return new Column { Id = "edit", ClientFormat = html.EditFormatForGrid(gridId), Width = 55 }
                .Mod(o => o.Nohide());
        }

        public static Column DeleteColumn(string gridId)
        {
            return new Column { Id = "del", ClientFormatFunc = "proUtils.inlDelRes('" + gridId + "')", Width = 55 }
                .Mod(o => o.Nohide());
        }

        public static Column InlineEditColumn()
        {
            return new Column
            {
                Id = "edit",
                ClientFormat = InlineEditFormat(),
                Width = 120,
                Reorderable = false,
                Resizable = false
            };
        }

        public static Column InlineDeleteColumn(string gridId)
        {
            return new Column
            {
                Id = "del",
                ClientFormatFunc = "proUtils.inlDelRes('" + gridId + "')",
                Width = 100,
                Reorderable = false,
                Resizable = false
            };
        }

        public static string InlineEditFormat(bool nofocus = false)
        {
            var tabindex = nofocus ? "tabindex = \"-1\"" : string.Empty;
            return string.Format("<button type=\"button\" class=\"awe-btn o-gledtb awe-nonselect o-glh o-glbtn\" {0} >" + "Edit" + "</button>" +
                                 "<button type=\"button\" class=\"awe-btn o-glsvb awe-nonselect o-gl o-glbtn\">" + "Save" + "</button>", tabindex);
        }

        /// <summary>
        /// Init create, edit, delete popup Forms and Restore Call 
        /// </summary>
        public static IHtmlContent InitCrud<T>(
            this IHtmlHelper<T> html,
            string gridId,
            string crudController,
            int createPopupHeight = 430,
            int maxWidth = 0)
        {
            var url = GetUrlHelper(html);

            gridId = html.Awe().GetContextPrefix() + gridId;

            var result =
                html.Awe()
                    .InitPopupForm()
                    .Name("create" + gridId)
                    .Group(gridId)
                    .Height(createPopupHeight)
                    .MaxWidth(maxWidth)
                    .Url(url.Action("Create", crudController))
                    .Title("Create")
                    .Modal()
                    .Success("utils.itemCreated('" + gridId + "')")
                    .ToString()

                + html.Awe()
                    .InitPopupForm()
                    .Name("edit" + gridId)
                    .Group(gridId)
                    .Height(createPopupHeight)
                    .MaxWidth(maxWidth)
                    .Url(url.Action("Edit", crudController))
                    .Title("Edit")
                    .Modal()
                    .Success("utils.itemEdited('" + gridId + "')")

                + html.Awe()
                    .InitPopupForm()
                    .Name("delete" + gridId)
                    .Group(gridId)
                    .Url(url.Action("Delete", crudController))
                    .Success("utils.itemDeleted('" + gridId + "')")
                    .OnLoad("utils.delConfirmLoad('" + gridId + "')") // calls grid.api.select and animates the row
                    .Height(200)
                    .Modal()

                + html.Awe().InitCall("restore" + gridId)
                    .Url(url.Action("Restore", crudController));

            return new HtmlString(result);
        }
    }
}
