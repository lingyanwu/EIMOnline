using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wysnan.EIMOnline.Common.Framework.Enum;
using System.Text;
using Wysnan.EIMOnline.Common.Framework.Grid;
using Wysnan.EIMOnline.Business.Framework;
using Wysnan.EIMOnline.Tool.Extensions;
using Wysnan.EIMOnline.Tool.ToolMethod;
using Wysnan.EIMOnline.Tool.JqGridExtansions;
using Wysnan.EIMOnline.Common.Framework;
using Wysnan.EIMOnline.Common.Poco;
using System.Collections.Specialized;
using Wysnan.EIMOnline.IBLL;
using Wysnan.EIMOnline.Business;
using Spring.Context;
using Spring.Context.Support;
using System.Linq.Expressions;

namespace Wysnan.EIMOnline.MVC.Framework.Extensions
{
    public static class HtmlHelperExtension
    {

        //[Obsolete("由于cookie大小有限制，对于多字段的cookie缓存不适用，弃用这个方法", false)]
        public static MvcHtmlString Grid(this HtmlHelper helper, GridEnum gridEnum)
        {
            SystemEntity systemEntity = HttpContext.Current.Session[ConstEntity.Session_SystemEntity] as SystemEntity;
            string key = systemEntity.CurrentSecurityUser.ID + "_" + gridEnum.ToString();
            string cookieName = ConstEntity.Cookie_JqGridHtml + key;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                //直接读取cooke里内容
                string cookieValue = HttpUtility.UrlDecode(cookie.Value);
                return MvcHtmlString.Create(cookieValue);
            }

            #region jqGrid构造

            JqGrid jqGrid = GlobalEntity.Instance.Cache_JqGrid.JqGrids[gridEnum];
            if (jqGrid == null)
            {
                return MvcHtmlString.Empty;
            }
            var colModel = jqGrid._ColModel;
            if (colModel == null || colModel.Count == 0)
            {
                return MvcHtmlString.Empty;
            }
            string gridHtml = jqGrid.ConvertToHtml();
            #endregion

            #region write cookie
            jqGrid.WriteCookie(gridEnum, HttpUtility.UrlEncode(gridHtml));
            #endregion
            return MvcHtmlString.Create(gridHtml);
        }

        public static MvcHtmlString AreasMenu(this HtmlHelper helper)
        {
            IList<SystemModuleType> systemModuleTypes = GlobalEntity.Instance.Cache_SystemModuleType.SystemModuleTypes;
            IList<SystemModule> systemModules = GlobalEntity.Instance.Cache_SystemModule.SystemModules;

            if (systemModuleTypes != null)
            {
                StringBuilder areasMenuString = new StringBuilder();
                areasMenuString.Append("<ul>");
                foreach (var item in systemModuleTypes)
                {
                    var firstModule = systemModules.Where(a => a.ModuleTypeId == item.ID);
                    if (firstModule != null && firstModule.Count() > 0)
                    {
                        areasMenuString.AppendFormat("<li id=\"menu_li_{0}\" onclick=\"MenuTypeNavigation({0}, '{1}', '{2}', '{3}')\">{1}</li>", item.ID, item.ModuleTypeName, firstModule.First().ModuleMainUrl, "image");
                    }
                    else
                    {
                        areasMenuString.AppendFormat("<li id=\"menu_li_{0}\">{1}</li>", item.ID, item.ModuleTypeName);
                    }
                }
                areasMenuString.Append("</ul>");
                return MvcHtmlString.Create(areasMenuString.ToString());
            }
            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString Menu(this HtmlHelper helper)
        {
            IList<SystemModule> systemModules = GlobalEntity.Instance.Cache_SystemModule.SystemModules;

            if (systemModules == null)
            {
                return MvcHtmlString.Empty;
            }
            StringBuilder menuStr = new StringBuilder();
            menuStr.Append("<ul class=\"ui_menu\">");
            var list = systemModules.ToList();
            foreach (var item in list)
            {
                SystemModuleDetailPage systemModuleDetailPage = GlobalEntity.Instance.Cache_SystemModule.GetSystemModuleDetailPage(item);
                menuStr.AppendFormat("<li><a onclick=\"Navigation('{0}_{1}','{2}', '{3}', '{4}')\">{2}</a></li>", item.ID, systemModuleDetailPage.ID, item.ModuleName, item.ModuleMainUrl, item.ImageUrl);
            }
            menuStr.Append("</ul>");
            return MvcHtmlString.Create(menuStr.ToString());
        }

        #region SearchBoxFor控件

        /// <summary>
        /// SearchBoxFor控件
        /// </summary>
        /// <typeparam name="T">泛型参数</typeparam>
        /// <typeparam name="U">需要加载的属性字段</typeparam>
        /// <typeparam name="F">关联的属性ID字段</typeparam>
        /// <param name="helper">泛型参数</param>
        /// <param name="expression">只能是IBaseEntity类型</param>
        /// <param name="field">关联的属性ID字段</param>
        /// <returns></returns>
        public static MvcHtmlString SearchBoxFor<T, U, F>(this HtmlHelper<T> helper, Expression<Func<T, U>> expression, Expression<Func<T, F>> field)
            where U : IBaseEntity
            where F : struct
        {
            Type type = typeof(U);
            var systemModule = GlobalEntity.Instance.Cache_SystemModule.SystemModules.Where(a => a.ControllerModule == type.Name).FirstOrDefault();
            if (systemModule == null)
            {
                return MvcHtmlString.Empty;
            }
            Random random = new Random();
            int id = random.Next(1, 99);

            string lamda = expression.Lamda();
            string fieldId = field.Lamda();

            string divSearchBoxId = "searchbox" + lamda + id;//控件ID
            string divSearchId = "divSerach_" + lamda + id;//加载数据ID
            string dialogDivId = "dialog_" + lamda + id;//弹出页面ID
            string spanQId = "spanQ_" + lamda + id; //搜索按钮ID

            StringBuilder serachBox = new StringBuilder();
            serachBox.AppendFormat("<div class=\"SearchableBox\" id=\"{0}\"><div class=\"search_q\"><span class=\"spanQ\" id=\"{1}\">+</span></div><div class=\"search_t\"><input type=\"text\" name=\"t{2}\" readonly=\"readonly\" /><input type=\"hidden\" name=\"{2}\"/></div></div>", divSearchBoxId, spanQId, fieldId);
            serachBox.AppendFormat("<div class=\"dialog_modal\" id=\"{0}\" title=\"{1}\"><div id=\"{2}\" class=\"dialog_content\"></div></div>", dialogDivId, type.Name, divSearchId);
            serachBox.AppendFormat("<script type=\"text/javascript\" language=\"javascript\">$(function () {{ $(\"#{0}\").dialog({{ autoOpen: false,height: 500,width: 800,modal: true,resizable: false}});", dialogDivId);
            serachBox.AppendFormat("$(\"#{0}\").click(function () {{$(\"#{1}\").load(\"{2}\", function () {{ $(\"#{3}\").dialog(\"open\");var jqgrid= $(\"#list\"+$(\"#{1}\").find(\"div[id^='div_jq_']\").attr(\"jqid\"));jqgrid.setGridHeight('365');jqgrid.setGridWidth('840');"
                + "jqgrid.hideCol(\"cb\");jqgrid.jqGrid('setGridParam',{{onCellSelect:function(rowid,iCol){{$(\"#{4}\").find(\"input[name='t{5}']\").val(jqgrid.getCol(jqgrid.getGridParam('saerchTextField')));$(\"#{4}\").find(\"input[name='{5}']\").val(rowid);$(\"#{3}\").dialog(\"close\");" +
                "}}}});$(\"#{1}\").find(\"td[id*='GridButton']\").hide();}});}});}});</script>",
                spanQId, divSearchId, systemModule.ModuleMainUrl, dialogDivId, divSearchBoxId, fieldId);
            return MvcHtmlString.Create(serachBox.ToString());
        }

        #endregion

        #region checkBox控件
        public static MvcHtmlString CheckBoxForIP<T>(this HtmlHelper<T> helper, Expression<Func<T, bool>> expression)
        {
            StringBuilder checkBox = new StringBuilder();
            string field = expression.Lamda();
            bool value = false;

            if (helper.ViewData != null)
            {
                var model = helper.ViewData.ModelMetadata;
                if (model != null)
                {
                    var tempValue = model.Properties.FirstOrDefault(c => c.PropertyName == field).Model;
                    if (tempValue != null)
                    {
                        bool.TryParse(tempValue.ToString(), out value);
                    }
                }
            }
            checkBox.AppendFormat("<input id=\"{0}\" name=\"{0}\" type=\"checkbox\" {1} value=\"{2}\">", field, value == true ? "checked=\"checked\"" : "", value ? "True" : "False");
            //checkBox.AppendFormat("<input type=\"hidden\" name=\"{0}\" value=\"{1}\" />", field, value ? "true" : "false");
            checkBox.AppendFormat("<script type=\"text/javascript\">$(document).ready(function () {{$(\"#{0}\").iphoneStyle({{onChange: function(elem, value) {{ $('#{0}').val(value.toString());}} }});}});</script>", field);
            return MvcHtmlString.Create(checkBox.ToString());
        }
        #endregion

        #region 日期控件

        public static MvcHtmlString DatePickFor<T>(this HtmlHelper<T> helper, Expression<Func<T, DateTime>> expression)
        {
            StringBuilder datePick = new StringBuilder();
            string field = expression.Lamda();
            datePick.AppendFormat("<input type=\"text\" id=\"{0}\" name=\"{0}\" />", field);


            return MvcHtmlString.Empty;
        }

        #endregion

        #region 测试，不缓存
        public delegate MvcHtmlString MvcCacheCallback(HttpContextBase context);

        public static object Substitute(this HtmlHelper html, MvcCacheCallback cb)
        {
            html.ViewContext.HttpContext.Response.WriteSubstitution(
                c => HttpUtility.HtmlEncode(
                    cb(new HttpContextWrapper(c))
                ));
            return null;
        }
        #endregion


    }
}