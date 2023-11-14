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

namespace Presentation.Helpers
{
    public static class CrudHelpers
    {
        private static IUrlHelper GetUrlHelper<T>(IHtmlHelper<T> html)
        {
            return ((IUrlHelperFactory)html.ViewContext.HttpContext.RequestServices.GetService(typeof(IUrlHelperFactory))).GetUrlHelper(html.ViewContext);
        }

        /*beging*/
        public static IHtmlContent InitCrudPopupsForGrid<T>(
                 this IHtmlHelper<T> html,
                 string gridId,
                 string crudController,
                 int createPopupHeight = 430,
                 int maxWidth = 0,
                 bool reload = false,
                 string area = null,
                 string inlineContId = null)
        {
            var url = GetUrlHelper(html);
            gridId = html.Awe().GetContextPrefix() + gridId;

            var refreshGrid = "refreshGrid";
            var format = "utils.{0}('" + gridId + "')";

            var createFunc = string.Format(format, reload ? refreshGrid : "itemCreated");
            var editFunc = string.Format(format, reload ? refreshGrid : "itemEdited");
            var delFunc = string.Format(format, reload ? refreshGrid : "itemDeleted");
            var delConfirmFunc = string.Format(format, "delConfirmLoad");

            var create = html.Awe()
                    .InitPopupForm()
                    .Name("create" + gridId)
                    .Group(gridId)
                    .Height(createPopupHeight)
                    .MaxWidth(maxWidth)
                    .Url(url.Action("Create", crudController, new { area }))
                    .Title("Create item")
                    .Modal()
                    .Success(createFunc);

            var edit = html.Awe()
                .InitPopupForm()
                .Name("edit" + gridId)
                .Group(gridId)
                .Height(createPopupHeight)
                .MaxWidth(maxWidth)
                .Url(url.Action("Edit", crudController, new { area }))
                .Title("Edit item")
                .Modal()
                .Success(editFunc);

            var delete = html.Awe()
                  .InitPopupForm()
                  .Name("delete" + gridId)
                  .Group(gridId)
                  .Url(url.Action("Delete", crudController, new { area }))
                  .Title("Delete item")
                  .Success(delFunc)
                  .OnLoad(delConfirmFunc) // calls grid.api.select and animates the row
                  .Height(200)
                  .Modal();

            if (inlineContId != null)
            {
                create.Mod(o => o.Inline(inlineContId).ShowHeader(false));
                edit.Mod(o => o.Inline(inlineContId).ShowHeader(false));
                delete.Mod(o => o.Inline(inlineContId).ShowHeader(false));
            }

            var result = create.ToString() + edit + delete;

            return new HtmlString(result);
        }
        /*endg*/

        public static IHtmlContent InitCrudForGridNest<T>(this IHtmlHelper<T> html, string gridId, string crudController)
        {
            var url = GetUrlHelper(html);
            gridId = html.Awe().GetContextPrefix() + gridId;

            var result =
                html.Awe()
                    .InitPopupForm()
                    .Name("create" + gridId)
                    .Group(gridId)
                    .Url(url.Action("Create", crudController))
                    .Mod(o => o.Inline().ShowHeader(false))
                    .Success("utils.itemCreated('" + gridId + "')")
                    .ToString()
                + html.Awe()
                      .InitPopupForm()
                      .Name("edit" + gridId)
                      .Group(gridId)
                      .Url(url.Action("Edit", crudController))
                      .Mod(o => o.Inline().ShowHeader(false))
                      .Success("utils.itemEdited('" + gridId + "')")
                + html.Awe()
                      .InitPopupForm()
                      .Name("delete" + gridId)
                      .Group(gridId)
                      .Url(url.Action("Delete", crudController))
                      .Mod(o => o.Inline().ShowHeader(false))
                      .Success("utils.itemDeleted('" + gridId + "')");

            return new HtmlString(result);
        }

        /*beginal*/
        public static IHtmlContent InitCrudPopupsForAjaxList<T>(
           this IHtmlHelper<T> html, string ajaxListId, string controller, string popupName)
        {
            var url = GetUrlHelper(html);

            var result =
                html.Awe()
                    .InitPopupForm()
                    .Name("create" + popupName)
                    .Url(url.Action("Create", controller))
                    .Height(430)
                    .Success("utils.itemCreatedAlTbl('" + ajaxListId + "')")
                    .Group(ajaxListId)
                    .Title("create item")
                    .ToString()

                + html.Awe()
                      .InitPopupForm()
                      .Name("edit" + popupName)
                      .Url(url.Action("Edit", controller))
                      .Height(430)
                      .Success("utils.itemEditedAl('" + ajaxListId + "')")
                      .Group(ajaxListId)
                      .Title("edit item")

                + html.Awe()
                      .InitPopupForm()
                      .Name("delete" + popupName)
                      .Url(url.Action("Delete", controller))
                      .Success("utils.itemDeletedAl('" + ajaxListId + "')")
                      .Group(ajaxListId)
                      .OkText("Yes")
                      .CancelText("No")
                      .Height(200)
                      .Title("delete item");

            return new HtmlString(result);
        }
        /*endal*/

        /// <summary>
        /// initialize Delete PopupForms for grid
        /// </summary>
        /// <param name="html"></param>
        /// <param name="gridId"></param>
        /// <param name="crudController">controller containing the crud actions</param>
        /// <param name="action">delete action name</param>
        /// <param name="reload">reload grid after delete action success</param>
        /// <param name="area"></param>
        public static IHtmlContent InitDeletePopupForGrid<T>(
            this IHtmlHelper<T> html,
            string gridId,
            string crudController = null,
            string action = "Delete",
            bool reload = false,
            string area = null)
        {
            var url = GetUrlHelper(html);
            gridId = html.Awe().GetContextPrefix() + gridId;

            var utilf = "utils.{0}('" + gridId + "')";

            var delFunc = string.Format(utilf, reload ? "refreshGrid" : "itemDeleted");
            var delConfirmFunc = string.Format(utilf, "delConfirmLoad");

            var result =
                html.Awe()
                    .InitPopupForm()
                    .Name("delete" + gridId)
                    .Group(gridId)
                    .Url(url.Action(action, crudController, new { area }))
                    .Success(delFunc)
                    .OnLoad(delConfirmFunc) // delConfirmFunc calls grid.api.select and animates the row
                    .Height(200)
                    .Modal()
                    .ToString();

            return new HtmlString(result);
        }
    }
}
